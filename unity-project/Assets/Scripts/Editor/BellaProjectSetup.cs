using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bella.EditorTools
{
    /// <summary>
    /// One-time project setup that runs BY ITSELF the first time the project is
    /// opened (and again from the menu Bella > Set Up Project). It creates the
    /// three materials the game needs, an (empty) Main scene, the build list and
    /// the Android player settings — so the only thing left to do is press Play.
    ///
    /// Menu:
    ///   Bella > Set Up Project      re-run the setup
    ///   Bella > Open Main Scene
    ///   Bella > Build Android APK   (test build -> Builds/Bella.apk)
    ///   Bella > Build Android AAB   (Play Store bundle -> Builds/Bella.aab; needs a keystore)
    ///   Bella > Reset Save Data
    ///   Bella > Take Screenshot      (Play mode -> Screenshots/, for the store listing)
    /// </summary>
    [InitializeOnLoad]
    public static class BellaProjectSetup
    {
        public const string ScenePath = "Assets/Scenes/Main.unity";
        public const string MaterialsDir = "Assets/Resources/Materials";
        public const string PackageId = "com.bellacutepuppy.game";   // CHANGE before the first Play upload if you want another id
        private const string DoneKey = "Bella.SetupDone.v4";

        static BellaProjectSetup()
        {
            EditorApplication.delayCall += AutoRun;
        }

        private static void AutoRun()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            bool done = EditorPrefs.GetBool(DoneKey + Application.dataPath, false);
            if (done && File.Exists(ScenePath) && File.Exists(MaterialsDir + "/BellaLit.mat")) return;
            try
            {
                Run(false);
                EditorPrefs.SetBool(DoneKey + Application.dataPath, true);
            }
            catch (Exception e)
            {
                Debug.LogError("[Bella] Automatic setup failed: " + e);
            }
        }

        [MenuItem("Bella/Set Up Project")]
        public static void RunFromMenu()
        {
            Run(true);
            EditorPrefs.SetBool(DoneKey + Application.dataPath, true);
        }

        private static void Run(bool interactive)
        {
            CreateMaterials();
            CreateScene();
            // Each settings step is independent: one unsupported setting must never block the rest.
            Step("player settings", ApplyPlayerSettings);
            Step("android settings", ApplyAndroidSettings);
            Step("app icon", ApplyIcon);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Bella] Project setup complete. Press Play to meet Bella.");

            bool restartNeeded = false;
            Step("input handling", () => { restartNeeded = EnsureClassicInput(); });
            if (restartNeeded)
            {
                AssetDatabase.SaveAssets();
                bool now = EditorUtility.DisplayDialog("Bella",
                    "One setting was changed so that touch input works (Active Input Handling = Input Manager).\n\n" +
                    "Unity needs to restart once for it to take effect.", "Restart Unity now", "Later");
                if (now) EditorApplication.OpenProject(Directory.GetCurrentDirectory());
                return;
            }
            if (interactive)
                EditorUtility.DisplayDialog("Bella", "Setup complete.\n\nPress the Play button at the top to meet Bella.", "OK");
        }

        private static void Step(string what, Action action)
        {
            try { action(); }
            catch (Exception e) { Debug.LogWarning("[Bella] Setup step '" + what + "' was skipped: " + e.Message); }
        }

        /// <summary>
        /// The game reads touches with the classic Input class. Unity 6 can default a project to the
        /// new Input System only, which would make every tap throw — switch it back (0 = classic, 2 = both).
        /// Returns true when the setting was changed (the Editor must restart to apply it).
        /// </summary>
        private static bool EnsureClassicInput()
        {
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
            if (assets == null || assets.Length == 0 || assets[0] == null) return false;
            var so = new SerializedObject(assets[0]);
            SerializedProperty p = so.FindProperty("activeInputHandler");
            if (p == null) return false;
            if (p.intValue == 0 || p.intValue == 2) return false;
            p.intValue = 0;
            so.ApplyModifiedPropertiesWithoutUndo();
            Debug.Log("[Bella] Active Input Handling set to Input Manager (classic).");
            return true;
        }

        // ------------------------------------------------------------------ materials

        private static void CreateMaterials()
        {
            Directory.CreateDirectory(MaterialsDir);
            MakeMaterial("BellaLit", "Legacy Shaders/Diffuse", "Standard");
            MakeMaterial("BellaTransparent", "Legacy Shaders/Transparent/Diffuse", "Sprites/Default");
            MakeMaterial("BellaSprite", "Sprites/Default", "Unlit/Transparent");
        }

        private static void MakeMaterial(string name, string shaderName, string fallback)
        {
            string path = MaterialsDir + "/" + name + ".mat";
            if (AssetDatabase.LoadAssetAtPath<Material>(path) != null) return;
            Shader shader = Shader.Find(shaderName);
            if (shader == null) shader = Shader.Find(fallback);
            if (shader == null)
            {
                Debug.LogWarning("[Bella] Shader not found for material " + name);
                return;
            }
            AssetDatabase.CreateAsset(new Material(shader), path);
        }

        // ------------------------------------------------------------------ scene

        private static void CreateScene()
        {
            if (!File.Exists(ScenePath))
            {
                if (EditorSceneManager.GetActiveScene().isDirty) return;   // never discard someone's unsaved work
                Directory.CreateDirectory("Assets/Scenes");
                Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                // The scene is intentionally empty: Bella.Core.GameRoot builds the game when Play starts.
                new GameObject("README - press Play (GameRoot builds everything in code)");
                EditorSceneManager.SaveScene(scene, ScenePath);
            }
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            if (EditorSceneManager.GetActiveScene().path != ScenePath && !EditorSceneManager.GetActiveScene().isDirty)
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        [MenuItem("Bella/Open Main Scene")]
        public static void OpenMainScene()
        {
            if (!File.Exists(ScenePath)) CreateScene();
            else if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        // ------------------------------------------------------------------ player settings

        private static void ApplyPlayerSettings()
        {
            PlayerSettings.companyName = "Bella Puppy Games";
            PlayerSettings.productName = "Bella the Cute Puppy";
            if (string.IsNullOrEmpty(PlayerSettings.bundleVersion) || PlayerSettings.bundleVersion == "0.1" || PlayerSettings.bundleVersion == "1.0")
                PlayerSettings.bundleVersion = "1.0.0";

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;
        }

        private static void ApplyAndroidSettings()
        {
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, PackageId);
            // Google Play needs 64-bit, which needs IL2CPP. ARMv7 is kept for older budget phones.
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            // Minimum Android version is left at this Unity version's own default.
            // Target = "Automatic (highest installed)" so the build meets Google Play's current target-API rule.
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            if (PlayerSettings.Android.bundleVersionCode < 1) PlayerSettings.Android.bundleVersionCode = 1;
            // The game never touches the network; make sure no INTERNET permission is requested.
            PlayerSettings.Android.forceInternetPermission = false;
        }

        private static void ApplyIcon()
        {
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Portraits/app_icon.png");
            if (icon != null)
                PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { icon }, IconKind.Any);
        }

        // ------------------------------------------------------------------ builds

        [MenuItem("Bella/Build Android APK (test)")]
        public static void BuildApk()
        {
            Build(false);
        }

        [MenuItem("Bella/Build Android AAB (Play Store)")]
        public static void BuildAab()
        {
            Build(true);
        }

        private static void Build(bool appBundle)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
            {
                EditorUtility.DisplayDialog("Bella",
                    "Android Build Support is not installed.\n\nUnity Hub > Installs > (gear) > Add modules > Android Build Support (tick OpenJDK and Android SDK & NDK Tools).", "OK");
                return;
            }
            if (appBundle && !PlayerSettings.Android.useCustomKeystore)
            {
                bool go = EditorUtility.DisplayDialog("Bella",
                    "No upload keystore is set yet, so this bundle would be signed with a debug key and Google Play will refuse it.\n\n" +
                    "Set one first: Edit > Project Settings > Player > Android tab > Publishing Settings > Keystore Manager " +
                    "(see docs/UNITY_DAY.md, step 6).", "Build anyway", "Cancel");
                if (!go) return;
            }
            Run(false);
            Directory.CreateDirectory("Builds");
            EditorUserBuildSettings.buildAppBundle = appBundle;
            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = appBundle ? "Builds/Bella.aab" : "Builds/Bella.apk",
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);
            bool ok = report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded;
            EditorUtility.DisplayDialog("Bella", ok
                ? "Build finished:\n" + options.locationPathName
                : "Build failed. Open Window > General > Console, copy the first red error and send it to Claude.", "OK");
            if (ok) EditorUtility.RevealInFinder(options.locationPathName);
        }

        [MenuItem("Bella/Take Screenshot (store listing)")]
        public static void TakeScreenshot()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("Bella", "Press Play first, then use this menu while the game is running.\n\nTip: set the Game window to 1080 x 1920 (portrait).", "OK");
                return;
            }
            Directory.CreateDirectory("Screenshots");
            string file = "Screenshots/bella_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(file);
            Debug.Log("[Bella] Screenshot saved: " + Path.GetFullPath(file));
        }

        [MenuItem("Bella/Reset Save Data")]
        public static void ResetSave()
        {
            string path = Path.Combine(Application.persistentDataPath, "bella_save.json");
            if (File.Exists(path)) File.Delete(path);
            Debug.Log("[Bella] Save data deleted.");
        }
    }
}
