using System;
using System.IO;
using UnityEditor;
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
    /// </summary>
    [InitializeOnLoad]
    public static class BellaProjectSetup
    {
        public const string ScenePath = "Assets/Scenes/Main.unity";
        public const string MaterialsDir = "Assets/Resources/Materials";
        public const string PackageId = "com.bellacutepuppy.game";   // CHANGE before the first Play upload if you want another id
        private const string DoneKey = "Bella.SetupDone.v3";

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
            ApplyPlayerSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Bella] Project setup complete. Press Play to meet Bella.");
            if (interactive)
                EditorUtility.DisplayDialog("Bella", "Setup complete.\n\nPress the Play button at the top to meet Bella.", "OK");
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
            if (string.IsNullOrEmpty(PlayerSettings.bundleVersion) || PlayerSettings.bundleVersion == "0.1")
                PlayerSettings.bundleVersion = "1.0.0";

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageId);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            if (PlayerSettings.Android.bundleVersionCode < 1) PlayerSettings.Android.bundleVersionCode = 1;
            // The game never touches the network; make sure no INTERNET permission is requested.
            PlayerSettings.Android.forceInternetPermission = false;

            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Portraits/app_icon.png");
            if (icon != null)
                PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new[] { icon });
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

        [MenuItem("Bella/Reset Save Data")]
        public static void ResetSave()
        {
            string path = Path.Combine(Application.persistentDataPath, "bella_save.json");
            if (File.Exists(path)) File.Delete(path);
            Debug.Log("[Bella] Save data deleted.");
        }
    }
}
