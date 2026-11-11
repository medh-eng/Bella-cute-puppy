using System; using UnityEngine; using UnityEngine.SceneManagement;
namespace UnityEditor {
    [AttributeUsage(AttributeTargets.Class)] public class InitializeOnLoadAttribute : Attribute {}
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)] public class MenuItem : Attribute { public MenuItem(string path) {} public MenuItem(string path, bool validate) {} public MenuItem(string path, bool validate, int priority) {} }
    public static class EditorApplication { public delegate void CallbackFunction(); public static CallbackFunction delayCall; public static bool isPlayingOrWillChangePlaymode => false; public static bool isPlaying { get; set; } public static void OpenProject(string projectPath, params string[] args) {} }
    public static class EditorPrefs { public static bool GetBool(string k, bool d) => d; public static void SetBool(string k, bool v) {} }
    public static class EditorUtility { public static bool DisplayDialog(string t, string m, string ok) => true; public static bool DisplayDialog(string t, string m, string ok, string cancel) => true; public static void RevealInFinder(string p) {} }
    public static class AssetDatabase { public static T LoadAssetAtPath<T>(string p) where T : UnityEngine.Object => null; public static void CreateAsset(UnityEngine.Object o, string path) {} public static void SaveAssets() {} public static void Refresh() {} public static UnityEngine.Object[] LoadAllAssetsAtPath(string p) => null; }
    public class EditorBuildSettingsScene { public EditorBuildSettingsScene(string path, bool enabled) {} }
    public static class EditorBuildSettings { public static EditorBuildSettingsScene[] scenes { get; set; } }
    public static class EditorUserBuildSettings { public static bool buildAppBundle { get; set; } }
    public enum BuildTargetGroup { Unknown = 0, Standalone = 1, iOS = 4, Android = 7 } public enum BuildTarget { StandaloneWindows64 = 19, Android = 13 }
    [Flags] public enum BuildOptions { None = 0, Development = 1 }
    public struct BuildPlayerOptions { public string[] scenes { get; set; } public string locationPathName { get; set; } public BuildTarget target { get; set; } public BuildTargetGroup targetGroup { get; set; } public BuildOptions options { get; set; } }
    public static class BuildPipeline { public static Build.Reporting.BuildReport BuildPlayer(BuildPlayerOptions o) => null; public static bool IsBuildTargetSupported(BuildTargetGroup g, BuildTarget t) => true; }
    public enum ScriptingImplementation { Mono2x = 0, IL2CPP = 1 } public enum UIOrientation { Portrait = 0, PortraitUpsideDown = 1, LandscapeRight = 2, LandscapeLeft = 3, AutoRotation = 4 }
    [Flags] public enum AndroidArchitecture : uint { None = 0, ARMv7 = 1, ARM64 = 2, All = 0xffffffff }
    public enum AndroidSdkVersions { AndroidApiLevelAuto = 0, AndroidApiLevel22 = 22, AndroidApiLevel23 = 23, AndroidApiLevel24 = 24 }
    public static class PlayerSettings {
        public static string companyName { get; set; } public static string productName { get; set; } public static string bundleVersion { get; set; } public static UIOrientation defaultInterfaceOrientation { get; set; }
        public static bool allowedAutorotateToPortrait { get; set; } public static bool allowedAutorotateToPortraitUpsideDown { get; set; } public static bool allowedAutorotateToLandscapeLeft { get; set; } public static bool allowedAutorotateToLandscapeRight { get; set; }
        public static void SetApplicationIdentifier(BuildTargetGroup g, string id) {} public static void SetScriptingBackend(BuildTargetGroup g, ScriptingImplementation s) {} public static void SetApplicationIdentifier(Build.NamedBuildTarget g, string id) {} public static void SetScriptingBackend(Build.NamedBuildTarget g, ScriptingImplementation s) {} public static void SetIcons(Build.NamedBuildTarget g, Texture2D[] icons, IconKind kind) {}
        public static class Android { public static AndroidArchitecture targetArchitectures { get; set; } public static AndroidSdkVersions minSdkVersion { get; set; } public static AndroidSdkVersions targetSdkVersion { get; set; } public static int bundleVersionCode { get; set; } public static bool forceInternetPermission { get; set; } public static bool useCustomKeystore { get; set; } }
    }
}
namespace UnityEditor.Build.Reporting { public enum BuildResult { Unknown, Succeeded, Failed, Cancelled } public struct BuildSummary { public BuildResult result => default; } public class BuildReport : UnityEngine.Object { public BuildSummary summary => default; } }
namespace UnityEditor.SceneManagement {
    public enum NewSceneSetup { EmptyScene, DefaultGameObjects } public enum NewSceneMode { Single, Additive } public enum OpenSceneMode { Single, Additive, AdditiveWithoutLoading }
    public static class EditorSceneManager { public static Scene NewScene(NewSceneSetup s, NewSceneMode m) => default; public static bool SaveScene(Scene s, string path) => true; public static Scene OpenScene(string p, OpenSceneMode m) => default; public static Scene GetActiveScene() => default; public static bool SaveCurrentModifiedScenesIfUserWantsTo() => true; }
}

namespace UnityEditor.Build { public struct NamedBuildTarget { public static readonly NamedBuildTarget Unknown, Standalone, Android, iOS; } }
namespace UnityEditor {
    public enum IconKind { Any = -1, Application = 0, Settings = 1, Notification = 2, Spotlight = 3, Store = 4 }
    public class SerializedProperty { public int intValue { get; set; } public bool boolValue { get; set; } }
    public class SerializedObject { public SerializedObject(UnityEngine.Object o) {} public SerializedProperty FindProperty(string n) => null; public bool ApplyModifiedPropertiesWithoutUndo() => true; }
    public class AssetImporter : UnityEngine.Object { }
    public class AssetPostprocessor { public string assetPath { get; set; } public AssetImporter assetImporter => null; public virtual uint GetVersion() => 0; }
    public enum TextureImporterType { Default = 0, NormalMap = 1, GUI = 2, Sprite = 8 }
    public enum TextureImporterNPOTScale { None = 0, ToNearest = 1, ToLarger = 2, ToSmaller = 3 }
    public class TextureImporter : AssetImporter { public TextureImporterType textureType { get; set; } public TextureImporterNPOTScale npotScale { get; set; } public bool mipmapEnabled { get; set; } public bool alphaIsTransparency { get; set; } public TextureWrapMode wrapMode { get; set; } public FilterMode filterMode { get; set; } public int maxTextureSize { get; set; } }
    public enum ModelImporterAnimationType { None = 0, Legacy = 1, Generic = 2, Human = 3 }
    public enum ModelImporterAnimationCompression { Off = 0, KeyframeReduction = 1, KeyframeReductionAndCompression = 2, Optimal = 3 }
    public enum ModelImporterMaterialImportMode { None = 0, ImportStandard = 1, ImportViaMaterialDescription = 2 }
    public class ModelImporter : AssetImporter { public ModelImporterAnimationType animationType { get; set; } public bool importAnimation { get; set; } public bool optimizeGameObjects { get; set; } public ModelImporterAnimationCompression animationCompression { get; set; } public ModelImporterMaterialImportMode materialImportMode { get; set; } public bool importBlendShapes { get; set; } public bool importCameras { get; set; } public bool importLights { get; set; } }
    public struct AudioImporterSampleSettings { public AudioClipLoadType loadType; public AudioCompressionFormat compressionFormat; public float quality; }
    public class AudioImporter : AssetImporter { public AudioImporterSampleSettings defaultSampleSettings { get; set; } public bool forceToMono { get; set; } }
}
namespace UnityEngine { public enum AudioClipLoadType { DecompressOnLoad, CompressedInMemory, Streaming } public enum AudioCompressionFormat { PCM, Vorbis, ADPCM, MP3 } }
