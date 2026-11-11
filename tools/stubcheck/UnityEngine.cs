// Hand-written compile-time stand-ins for the parts of the Unity API this project uses.
// Signatures mirror Unity 2022.3. Only Mathf / Vector2 carry real maths (for logic tests).
using System;
using System.Collections;
using System.Collections.Generic;
namespace UnityEngine
{
    public class Object {
        public string name { get; set; }
        public static void Destroy(Object o) {} public static void Destroy(Object o, float t) {}
        public static void DestroyImmediate(Object o) {}
        public static T Instantiate<T>(T original) where T : Object => original;
        public static T Instantiate<T>(T original, Transform parent) where T : Object => original;
        public static T FindObjectOfType<T>() where T : Object => null;
        public static T[] FindObjectsOfType<T>() where T : Object => new T[0];
        public static void DontDestroyOnLoad(Object o) {}
        public static implicit operator bool(Object o) => !ReferenceEquals(o, null);
        public static bool operator ==(Object a, Object b) => ReferenceEquals(a, b);
        public static bool operator !=(Object a, Object b) => !ReferenceEquals(a, b);
        public override bool Equals(object o) => ReferenceEquals(this, o); public override int GetHashCode() => 0;
    }
    public class Component : Object {
        public Transform transform => null; public GameObject gameObject => null; public string tag { get; set; }
        public T GetComponent<T>() => default; public T GetComponentInChildren<T>() => default; public T GetComponentInChildren<T>(bool inactive) => default;
        public T GetComponentInParent<T>() => default; public T[] GetComponentsInChildren<T>() => null;
    }
    public class Behaviour : Component { public bool enabled { get; set; } public bool isActiveAndEnabled => true; }
    public class MonoBehaviour : Behaviour {
        public Coroutine StartCoroutine(IEnumerator r) => null; public void StopCoroutine(Coroutine c) {} public void StopAllCoroutines() {}
    }
    public class ScriptableObject : Object { public static T CreateInstance<T>() where T : ScriptableObject => null; }
    public class Coroutine {} public class YieldInstruction {} public class WaitForSeconds : YieldInstruction { public WaitForSeconds(float s) {} }
    public sealed class GameObject : Object { public T[] GetComponentsInChildren<T>(bool includeInactive) => new T[0];
        public GameObject() {} public GameObject(string name) {} public GameObject(string name, params Type[] components) {}
        public Transform transform => null; public string tag { get; set; } public bool activeSelf => true; public bool activeInHierarchy => true; public int layer { get; set; }
        public void SetActive(bool v) {}
        public T AddComponent<T>() where T : Component => null; public T GetComponent<T>() => default;
        public T GetComponentInChildren<T>() => default; public T GetComponentInChildren<T>(bool inactive) => default; public T GetComponentInParent<T>() => default;
        public static GameObject CreatePrimitive(PrimitiveType t) => null;
    }
    public enum PrimitiveType { Sphere, Capsule, Cylinder, Cube, Plane, Quad }
    public class Transform : Component, IEnumerable {
        public Vector3 position { get; set; } public Vector3 localPosition { get; set; } public Quaternion rotation { get; set; } public Quaternion localRotation { get; set; }
        public Vector3 localScale { get; set; } public Vector3 lossyScale => default; public Vector3 eulerAngles { get; set; } public Vector3 localEulerAngles { get; set; }
        public Vector3 forward { get; set; } public Vector3 up { get; set; } public Vector3 right { get; set; }
        public Transform parent { get; set; } public int childCount => 0; public Transform GetChild(int i) => null;
        public void SetParent(Transform p) {} public void SetParent(Transform p, bool worldPositionStays) {}
        public Transform Find(string n) => null; public Vector3 TransformPoint(Vector3 p) => p; public Vector3 InverseTransformPoint(Vector3 p) => p; public Vector3 TransformDirection(Vector3 p) => p;
        public void Rotate(float x, float y, float z) {} public void Rotate(Vector3 e) {} public void LookAt(Vector3 p) {}
        public void SetAsFirstSibling() {} public void SetAsLastSibling() {} public void SetSiblingIndex(int i) {}
        public IEnumerator GetEnumerator() => null;
    }
    public sealed class RectTransform : Transform {
        public Vector2 anchorMin { get; set; } public Vector2 anchorMax { get; set; } public Vector2 pivot { get; set; } public Vector2 anchoredPosition { get; set; }
        public Vector2 sizeDelta { get; set; } public Vector2 offsetMin { get; set; } public Vector2 offsetMax { get; set; } public Rect rect => default;
    }
    public static class RectTransformUtility {
        public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint) { localPoint = default; return true; }
    }
    public struct Rect { public Rect(float x, float y, float w, float h) { this.x=x; this.y=y; width=w; height=h; } public float x, y, width, height; public Vector2 size => new Vector2(width, height); public Vector2 center => default; public Vector2 position => new Vector2(x, y); public float xMin => x; public float yMin => y; public float xMax => x + width; public float yMax => y + height; public static bool operator ==(Rect a, Rect b) => a.x == b.x && a.y == b.y && a.width == b.width && a.height == b.height; public static bool operator !=(Rect a, Rect b) => !(a == b); public override bool Equals(object o) => o is Rect r && r == this; public override int GetHashCode() => 0; }
    public struct Vector2 {
        public float x, y; public Vector2(float x, float y) { this.x = x; this.y = y; }
        public static Vector2 zero => new Vector2(0, 0); public static Vector2 one => new Vector2(1, 1);
        public float magnitude => (float)Math.Sqrt(x * x + y * y); public float sqrMagnitude => x * x + y * y; public Vector2 normalized { get { float m = magnitude; return m > 1e-5f ? new Vector2(x / m, y / m) : zero; } }
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y); public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, float d) => new Vector2(a.x * d, a.y * d); public static Vector2 operator *(float d, Vector2 a) => new Vector2(a.x * d, a.y * d); public static Vector2 operator /(Vector2 a, float d) => new Vector2(a.x / d, a.y / d);
        public static Vector2 operator -(Vector2 a) => new Vector2(-a.x, -a.y);
        public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y; public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
        public override bool Equals(object o) => o is Vector2 v && v == this; public override int GetHashCode() => 0;
        public static implicit operator Vector2(Vector3 v) => new Vector2(v.x, v.y); public static implicit operator Vector3(Vector2 v) => new Vector3(v.x, v.y, 0);
        public static float Dot(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;
        public static float SignedAngle(Vector2 from, Vector2 to) { float un = (float)(Math.Acos(Math.Max(-1.0, Math.Min(1.0, Dot(from.normalized, to.normalized)))) * 180.0 / Math.PI); float s = Math.Sign(from.x * to.y - from.y * to.x); return un * s; }
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * Mathf.Clamp01(t);
    }
    public struct Vector3 {
        public float x, y, z; public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; } public Vector3(float x, float y) { this.x = x; this.y = y; z = 0; }
        public static Vector3 zero => default; public static Vector3 one => new Vector3(1, 1, 1); public static Vector3 up => new Vector3(0, 1, 0); public static Vector3 forward => new Vector3(0, 0, 1); public static Vector3 right => new Vector3(1, 0, 0); public static Vector3 down => new Vector3(0, -1, 0); public static Vector3 back => new Vector3(0,0,-1); public static Vector3 left => new Vector3(-1,0,0);
        public float magnitude => (float)Math.Sqrt(x * x + y * y + z * z); public float sqrMagnitude => x * x + y * y + z * z; public Vector3 normalized => this;
        public static Vector3 operator +(Vector3 a, Vector3 b) => a; public static Vector3 operator -(Vector3 a, Vector3 b) => a; public static Vector3 operator -(Vector3 a) => a;
        public static Vector3 operator *(Vector3 a, float d) => a; public static Vector3 operator *(float d, Vector3 a) => a; public static Vector3 operator /(Vector3 a, float d) => a;
        public static bool operator ==(Vector3 a, Vector3 b) => true; public static bool operator !=(Vector3 a, Vector3 b) => false;
        public override bool Equals(object o) => true; public override int GetHashCode() => 0;
        public static float Dot(Vector3 a, Vector3 b) => 0; public static Vector3 Cross(Vector3 a, Vector3 b) => a; public static float Distance(Vector3 a, Vector3 b) => 0;
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a; public static Vector3 MoveTowards(Vector3 a, Vector3 b, float d) => a; public static Vector3 ProjectOnPlane(Vector3 v, Vector3 n) => v;
    }
    public struct Vector4 { public float x, y, z, w; public Vector4(float x, float y, float z, float w) { this.x=x; this.y=y; this.z=z; this.w=w; } }
    public struct Quaternion {
        public float x, y, z, w; public static Quaternion identity => default; public Vector3 eulerAngles { get; set; }
        public static Quaternion Euler(float x, float y, float z) => default; public static Quaternion Euler(Vector3 e) => default;
        public static Quaternion LookRotation(Vector3 f) => default; public static Quaternion LookRotation(Vector3 f, Vector3 up) => default;
        public static Quaternion RotateTowards(Quaternion a, Quaternion b, float maxDeg) => a; public static Quaternion Slerp(Quaternion a, Quaternion b, float t) => a; public static Quaternion Lerp(Quaternion a, Quaternion b, float t) => a;
        public static Quaternion AngleAxis(float angle, Vector3 axis) => default; public static Quaternion FromToRotation(Vector3 a, Vector3 b) => default; public static Quaternion Inverse(Quaternion q) => q;
        public static float Angle(Quaternion a, Quaternion b) => 0; public void ToAngleAxis(out float angle, out Vector3 axis) { angle = 0; axis = default; }
        public static Quaternion operator *(Quaternion a, Quaternion b) => a; public static Vector3 operator *(Quaternion a, Vector3 b) => b;
    }
    public struct Color {
        public float r, g, b, a; public Color(float r, float g, float b, float a) { this.r=r; this.g=g; this.b=b; this.a=a; } public Color(float r, float g, float b) { this.r=r; this.g=g; this.b=b; a=1; }
        public static Color white => new Color(1,1,1,1); public static Color black => new Color(0,0,0,1); public static Color clear => default; public static Color red => default; public static Color gray => default;
        public static Color Lerp(Color a, Color b, float t) => a; public static Color operator *(Color a, float b) => a; public static Color operator *(Color a, Color b) => a; public static Color operator +(Color a, Color b) => a;
    }
    public struct Color32 { public byte r, g, b, a; }
    public static class Mathf {
        public const float PI = 3.14159274f; public const float Rad2Deg = 57.29578f; public const float Deg2Rad = 0.0174532924f; public const float Infinity = float.PositiveInfinity;
        public static float Sin(float f) => (float)Math.Sin(f); public static float Cos(float f) => (float)Math.Cos(f); public static float Tan(float f) => (float)Math.Tan(f); public static float Atan(float f) => (float)Math.Atan(f); public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);
        public static float Sqrt(float f) => (float)Math.Sqrt(f); public static float Abs(float f) => Math.Abs(f); public static int Abs(int f) => Math.Abs(f); public static float Exp(float f) => (float)Math.Exp(f); public static float Pow(float f, float p) => (float)Math.Pow(f, p);
        public static float Min(float a, float b) => a < b ? a : b; public static float Max(float a, float b) => a > b ? a : b; public static int Min(int a, int b) => a < b ? a : b; public static int Max(int a, int b) => a > b ? a : b;
        public static float Clamp(float v, float a, float b) => v < a ? a : v > b ? b : v; public static int Clamp(int v, int a, int b) => v < a ? a : v > b ? b : v; public static float Clamp01(float v) => Clamp(v, 0, 1);
        public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t); public static float MoveTowards(float a, float b, float d) => Math.Abs(b - a) <= d ? b : a + Math.Sign(b - a) * d;
        public static float SmoothDamp(float current, float target, ref float velocity, float smoothTime) => target;
        public static float Repeat(float t, float length) => Clamp(t - (float)Math.Floor(t / length) * length, 0f, length);
        public static float Floor(float f) => (float)Math.Floor(f); public static int FloorToInt(float f) => (int)Math.Floor(f); public static int RoundToInt(float f) => (int)Math.Round(f); public static int CeilToInt(float f) => (int)Math.Ceiling(f); public static float Sign(float f) => f >= 0 ? 1 : -1;
    }
    public static class Random { public static float value => 0.5f; public static float Range(float a, float b) => a; public static int Range(int a, int b) => a; public static Vector3 insideUnitSphere => default; public static Vector2 insideUnitCircle => default; }
    public static class Time { public static float deltaTime => 0.016f; public static float unscaledDeltaTime => 0.016f; public static float time => 0; public static float unscaledTime => 0; public static float timeScale { get; set; } public static int frameCount => 0; }
    public static class Debug { public static void Log(object o) {} public static void LogWarning(object o) {} public static void LogError(object o) {} public static void LogException(Exception e) {} public static bool isDebugBuild => false; }
    public static class Application {
        public static string persistentDataPath => "/tmp/bella_test"; public static string dataPath => ""; public static string version => "1"; public static string unityVersion => "2022.3";
        public static int targetFrameRate { get; set; } public static bool isEditor => false; public static bool isPlaying => true; public static void Quit() {} public static void OpenURL(string u) {}
    }
    public static class Screen { public static int width => 1080; public static int height => 1920; public static int sleepTimeout { get; set; } public static Rect safeArea => new Rect(0, 0, 1080, 1920); public static ScreenOrientation orientation { get; set; } }
    public enum ScreenOrientation { Portrait, LandscapeLeft, AutoRotation }
    public static class SleepTimeout { public const int NeverSleep = -1; public const int SystemSetting = -2; }
    public static class JsonUtility { public static string ToJson(object o) => ""; public static string ToJson(object o, bool pretty) => ""; public static T FromJson<T>(string s) => default; }
    public static class Resources { public static T Load<T>(string p) where T : Object => null; public static T[] LoadAll<T>(string p) where T : Object => new T[0]; public static T GetBuiltinResource<T>(string p) where T : Object => null; }
    public static class Input {
        public static bool GetMouseButtonDown(int b) => false; public static bool GetMouseButton(int b) => false; public static bool GetMouseButtonUp(int b) => false; public static Vector3 mousePosition => default;
        public static int touchCount => 0; public static Touch GetTouch(int i) => default; public static bool GetKeyDown(KeyCode k) => false;
    }
    public struct Touch { public int fingerId; public Vector2 position; }
    public enum KeyCode { Escape, Space }
    public struct Ray {} public struct RaycastHit { public Vector3 point => default; public Collider collider => null; public Transform transform => null; public Vector3 normal => default; }
    public static class Physics { public static bool Raycast(Ray r, out RaycastHit h, float max) { h = default; return false; } public static bool Raycast(Ray r, out RaycastHit h) { h = default; return false; } }
    public class Collider : Component { public bool enabled { get; set; } public bool isTrigger { get; set; } }
    public class CapsuleCollider : Collider { public Vector3 center { get; set; } public float radius { get; set; } public float height { get; set; } public int direction { get; set; } }
    public class SphereCollider : Collider { public Vector3 center { get; set; } public float radius { get; set; } }
    public class BoxCollider : Collider { public Vector3 center { get; set; } public Vector3 size { get; set; } }
    public class Camera : Behaviour {
        public static Camera main => null; public CameraClearFlags clearFlags { get; set; } public Color backgroundColor { get; set; } public float fieldOfView { get; set; } public float aspect { get; set; }
        public float nearClipPlane { get; set; } public float farClipPlane { get; set; } public bool orthographic { get; set; }
        public Ray ScreenPointToRay(Vector3 p) => default; public Vector3 WorldToScreenPoint(Vector3 p) => p; public Vector3 ScreenToWorldPoint(Vector3 p) => p;
    }
    public enum CameraClearFlags { Skybox = 1, SolidColor = 2, Depth = 3, Nothing = 4 }
    public class AudioListener : Behaviour {}
    public class AudioClip : Object { public float length => 0; }
    public class AudioSource : Behaviour { public AudioClip clip { get; set; } public float volume { get; set; } public float pitch { get; set; } public bool loop { get; set; } public bool playOnAwake { get; set; } public bool isPlaying => false; public float spatialBlend { get; set; }
        public void Play() {} public void Stop() {} public void Pause() {} public void PlayOneShot(AudioClip c) {} public void PlayOneShot(AudioClip c, float v) {} }
    public class Light : Behaviour { public LightType type { get; set; } public Color color { get; set; } public float intensity { get; set; } public LightShadows shadows { get; set; } public float shadowStrength { get; set; } }
    public enum LightType { Spot, Directional, Point } public enum LightShadows { None, Hard, Soft }
    public static class QualitySettings { public static float shadowDistance { get; set; } public static int vSyncCount { get; set; } }
    public static class RenderSettings { public static Rendering.AmbientMode ambientMode { get; set; } public static Color ambientLight { get; set; } public static bool fog { get; set; } }
    public class Shader : Object { public static Shader Find(string n) => null; public static int PropertyToID(string n) => 0; }
    public class Texture : Object { public int width => 0; public int height => 0; public TextureWrapMode wrapMode { get; set; } public FilterMode filterMode { get; set; } }
    public class Texture2D : Texture { public Texture2D(int w, int h) {} public static Texture2D whiteTexture => null; public void SetPixel(int x, int y, Color c) {} public void Apply() {} }
    public enum TextureWrapMode { Repeat, Clamp } public enum FilterMode { Point, Bilinear, Trilinear }
    public class Material : Object { public Material(Shader s) {} public Material(Material m) {} public Color color { get; set; } public Texture mainTexture { get; set; } public Shader shader { get; set; } public int renderQueue { get; set; }
        public bool HasProperty(string n) => false; public void SetColor(string n, Color c) {} public void SetFloat(string n, float f) {} public void SetTexture(string n, Texture t) {} }
    public class Renderer : Component { public Material material { get; set; } public Material sharedMaterial { get; set; } public Material[] sharedMaterials { get; set; } public bool enabled { get; set; } public Bounds bounds => default;
        public Rendering.ShadowCastingMode shadowCastingMode { get; set; } public bool receiveShadows { get; set; } }
    public class MeshRenderer : Renderer {} public class SkinnedMeshRenderer : Renderer { public bool updateWhenOffscreen { get; set; } public Transform rootBone { get; set; } public Transform[] bones { get; set; } public Bounds localBounds { get; set; } public Mesh sharedMesh { get; set; } }
    public class MeshFilter : Component { public Mesh sharedMesh { get; set; } public Mesh mesh { get; set; } }
    public class Mesh : Object { public void SetVertices(List<Vector3> v) {} public void SetTriangles(List<int> t, int submesh) {} public void RecalculateNormals() {} public void RecalculateBounds() {} }
    public struct Bounds { public Vector3 center, size, min, max, extents; }
    public class Sprite : Object {
        public static Sprite Create(Texture2D t, Rect r, Vector2 pivot) => null; public static Sprite Create(Texture2D t, Rect r, Vector2 pivot, float ppu) => null;
        public static Sprite Create(Texture2D t, Rect r, Vector2 pivot, float ppu, uint extrude, SpriteMeshType mt, Vector4 border) => null; }
    public enum SpriteMeshType { FullRect, Tight }
    public class Font : Object { public static Font CreateDynamicFontFromOSFont(string n, int size) => null; }
    public enum FontStyle { Normal, Bold, Italic, BoldAndItalic } public enum TextAnchor { UpperLeft, UpperCenter, UpperRight, MiddleLeft, MiddleCenter, MiddleRight, LowerLeft, LowerCenter, LowerRight }
    public enum HorizontalWrapMode { Wrap, Overflow } public enum VerticalWrapMode { Truncate, Overflow }
    public sealed class Canvas : Behaviour { public RenderMode renderMode { get; set; } public int sortingOrder { get; set; } public bool overrideSorting { get; set; } public float scaleFactor { get; set; } }
    public enum RenderMode { ScreenSpaceOverlay, ScreenSpaceCamera, WorldSpace }
    public class Animator : Behaviour { public bool applyRootMotion { get; set; } public AnimatorCullingMode cullingMode { get; set; } public float speed { get; set; } public void SetBool(string n, bool v) {} }
    public enum AnimatorCullingMode { AlwaysAnimate, CullUpdateTransforms, CullCompletely }
    public class AnimationClip : Object { public float length => 0; public bool legacy { get; set; } public float frameRate => 30; public bool isLooping => false; }
    public struct Keyframe { public Keyframe(float t, float v) {} } public class AnimationCurve { public AnimationCurve(params Keyframe[] k) {} }
    public enum ParticleSystemStopBehavior { StopEmittingAndClear, StopEmitting } public enum ParticleSystemSimulationSpace { Local, World, Custom } public enum ParticleSystemStopAction { None, Disable, Destroy, Callback } public enum ParticleSystemShapeType { Sphere, Hemisphere, Cone, Box, Circle }
    public class ParticleSystemRenderer : Renderer {}
    public class ParticleSystem : Component {
        public MainModule main => default; public EmissionModule emission => default; public ShapeModule shape => default; public SizeOverLifetimeModule sizeOverLifetime => default;
        public void Play() {} public void Stop() {} public void Stop(bool withChildren, ParticleSystemStopBehavior b) {}
        public struct MinMaxCurve { public MinMaxCurve(float c) {} public MinMaxCurve(float a, float b) {} public MinMaxCurve(float mult, AnimationCurve c) {} public static implicit operator MinMaxCurve(float f) => default; }
        public struct MinMaxGradient { public MinMaxGradient(Color c) {} public MinMaxGradient(Color a, Color b) {} public static implicit operator MinMaxGradient(Color c) => default; }
        public struct Burst { public Burst(float time, short count) {} public Burst(float time, MinMaxCurve count) {} }
        public struct MainModule { public bool playOnAwake { get; set; } public bool loop { get; set; } public float duration { get; set; } public MinMaxCurve startLifetime { get; set; } public MinMaxCurve startSpeed { get; set; } public MinMaxCurve startSize { get; set; } public MinMaxCurve startRotation { get; set; }
            public MinMaxGradient startColor { get; set; } public MinMaxCurve gravityModifier { get; set; } public ParticleSystemSimulationSpace simulationSpace { get; set; } public ParticleSystemStopAction stopAction { get; set; } public int maxParticles { get; set; } }
        public struct EmissionModule { public bool enabled { get; set; } public MinMaxCurve rateOverTime { get; set; } public void SetBursts(Burst[] b) {} }
        public struct ShapeModule { public bool enabled { get; set; } public ParticleSystemShapeType shapeType { get; set; } public float radius { get; set; } }
        public struct SizeOverLifetimeModule { public bool enabled { get; set; } public MinMaxCurve size { get; set; } }
    }
    [AttributeUsage(AttributeTargets.Field)] public class SerializeField : Attribute {} [AttributeUsage(AttributeTargets.Field)] public class HeaderAttribute : Attribute { public HeaderAttribute(string h) {} }
    [AttributeUsage(AttributeTargets.Field)] public class TooltipAttribute : Attribute { public TooltipAttribute(string h) {} } [AttributeUsage(AttributeTargets.Field)] public class RangeAttribute : Attribute { public RangeAttribute(float a, float b) {} }
    [AttributeUsage(AttributeTargets.Class)] public class RequireComponent : Attribute { public RequireComponent(Type t) {} }
    [AttributeUsage(AttributeTargets.Class)] public class CreateAssetMenuAttribute : Attribute { public string fileName { get; set; } public string menuName { get; set; } public int order { get; set; } }
    public enum RuntimeInitializeLoadType { AfterSceneLoad, BeforeSceneLoad, AfterAssembliesLoaded, BeforeSplashScreen, SubsystemRegistration }
    [AttributeUsage(AttributeTargets.Method)] public class RuntimeInitializeOnLoadMethodAttribute : Attribute { public RuntimeInitializeOnLoadMethodAttribute() {} public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType t) {} }
}
namespace UnityEngine.Rendering { public enum AmbientMode { Skybox = 0, Trilight = 1, Flat = 3, Custom = 4 } public enum ShadowCastingMode { Off, On, TwoSided, ShadowsOnly }
    public class RenderPipelineAsset : UnityEngine.Object {} public static class GraphicsSettings { public static RenderPipelineAsset currentRenderPipeline => null; } }
namespace UnityEngine.SceneManagement { public struct Scene { public string path => ""; public string name => ""; public bool isDirty => false; public int buildIndex => 0; public GameObject[] GetRootGameObjects() => new GameObject[0]; } public static class SceneManager { public static Scene GetActiveScene() => default; public static void LoadScene(int i) {} } }
namespace UnityEngine.Events { public delegate void UnityAction(); public delegate void UnityAction<T0>(T0 a); public class UnityEvent { public void AddListener(UnityAction a) {} public void RemoveListener(UnityAction a) {} public void RemoveAllListeners() {} public void Invoke() {} } }
namespace UnityEngine.EventSystems {
    public class UIBehaviour : MonoBehaviour {} public class BaseInputModule : UIBehaviour {} public class StandaloneInputModule : BaseInputModule {}
    public class EventSystem : UIBehaviour { public static EventSystem current { get; set; } public bool IsPointerOverGameObject() => false; public bool IsPointerOverGameObject(int pointerId) => false; }
    public class BaseEventData {} public class PointerEventData : BaseEventData { public Vector2 position { get; set; } public Vector2 delta { get; set; } }
    public interface IEventSystemHandler {} public interface IPointerDownHandler : IEventSystemHandler { void OnPointerDown(PointerEventData eventData); } public interface IPointerUpHandler : IEventSystemHandler { void OnPointerUp(PointerEventData eventData); }
    public interface IPointerClickHandler : IEventSystemHandler { void OnPointerClick(PointerEventData eventData); } public interface IDragHandler : IEventSystemHandler { void OnDrag(PointerEventData eventData); }
}
namespace UnityEngine.UI {
    using UnityEngine.EventSystems; using UnityEngine.Events;
    public class CanvasScaler : UIBehaviour { public enum ScaleMode { ConstantPixelSize, ScaleWithScreenSize, ConstantPhysicalSize } public enum ScreenMatchMode { MatchWidthOrHeight, Expand, Shrink }
        public ScaleMode uiScaleMode { get; set; } public Vector2 referenceResolution { get; set; } public ScreenMatchMode screenMatchMode { get; set; } public float matchWidthOrHeight { get; set; } public float referencePixelsPerUnit { get; set; } }
    public class GraphicRaycaster : UIBehaviour {}
    public abstract class Graphic : UIBehaviour { public virtual Color color { get; set; } public virtual bool raycastTarget { get; set; } public RectTransform rectTransform => null; public Canvas canvas => null; public virtual Material material { get; set; } public void SetNativeSize() {} }
    public abstract class MaskableGraphic : Graphic {}
    public class Image : MaskableGraphic {
        public enum Type { Simple, Sliced, Tiled, Filled } public enum FillMethod { Horizontal, Vertical, Radial90, Radial180, Radial360 } public enum Origin360 { Bottom, Right, Top, Left } public enum OriginHorizontal { Left, Right }
        public Sprite sprite { get; set; } public Type type { get; set; } public FillMethod fillMethod { get; set; } public float fillAmount { get; set; } public int fillOrigin { get; set; } public bool fillClockwise { get; set; } public bool preserveAspect { get; set; } public float pixelsPerUnitMultiplier { get; set; } public float alphaHitTestMinimumThreshold { get; set; } }
    public class RawImage : MaskableGraphic { public Texture texture { get; set; } }
    public class Text : MaskableGraphic { public string text { get; set; } public Font font { get; set; } public int fontSize { get; set; } public FontStyle fontStyle { get; set; } public TextAnchor alignment { get; set; }
        public HorizontalWrapMode horizontalOverflow { get; set; } public VerticalWrapMode verticalOverflow { get; set; } public bool resizeTextForBestFit { get; set; } public int resizeTextMinSize { get; set; } public int resizeTextMaxSize { get; set; } public float lineSpacing { get; set; } public bool supportRichText { get; set; } }
    public class Selectable : UIBehaviour { public enum Transition { None, ColorTint, SpriteSwap, Animation } public Transition transition { get; set; } public Graphic targetGraphic { get; set; } public bool interactable { get; set; } }
    public class Button : Selectable { public class ButtonClickedEvent : UnityEvent {} public ButtonClickedEvent onClick { get; set; } }
}
namespace UnityEngine.Playables {
    public enum DirectorUpdateMode { DSPClock, GameTime, UnscaledGameTime, Manual }
    public interface IPlayable {} public interface IPlayableOutput {}
    public struct Playable : IPlayable {}
    public struct PlayableGraph {
        public static PlayableGraph Create() => default; public static PlayableGraph Create(string name) => default; public void Destroy() {} public bool IsValid() => true; public void Play() {} public void Stop() {} public void Evaluate() {} public void Evaluate(float dt) {}
        public void SetTimeUpdateMode(DirectorUpdateMode m) {}
        public bool Connect<U, V>(U source, int sourceOutputPort, V destination, int destinationInputPort) where U : struct, IPlayable where V : struct, IPlayable => true;
    }
    public static class PlayableExtensions {
        public static void SetTime<U>(this U p, double v) where U : struct, IPlayable {} public static double GetTime<U>(this U p) where U : struct, IPlayable => 0; public static void SetSpeed<U>(this U p, double v) where U : struct, IPlayable {}
        public static void SetInputWeight<U>(this U p, int idx, float w) where U : struct, IPlayable {} public static void SetDuration<U>(this U p, double v) where U : struct, IPlayable {} public static void Pause<U>(this U p) where U : struct, IPlayable {} public static void Play<U>(this U p) where U : struct, IPlayable {}
    }
    public static class PlayableOutputExtensions { public static void SetSourcePlayable<U, V>(this U output, V value) where U : struct, IPlayableOutput where V : struct, IPlayable {} }
}
namespace UnityEngine.Animations {
    using UnityEngine.Playables;
    public struct AnimationClipPlayable : IPlayable { public static AnimationClipPlayable Create(PlayableGraph g, AnimationClip c) => default; public void SetApplyFootIK(bool v) {} public AnimationClip GetAnimationClip() => null; }
    public struct AnimationMixerPlayable : IPlayable { public static AnimationMixerPlayable Create(PlayableGraph g, int inputCount = 0) => default; }
    public struct AnimationPlayableOutput : IPlayableOutput { public static AnimationPlayableOutput Create(PlayableGraph g, string name, Animator target) => default; }
}

namespace UnityEngine { public static class ScreenCapture { public static void CaptureScreenshot(string filename) {} public static void CaptureScreenshot(string filename, int superSize) {} } }
