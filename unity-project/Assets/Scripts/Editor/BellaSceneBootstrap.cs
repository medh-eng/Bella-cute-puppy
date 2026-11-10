using System.IO;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;
using Bella.UI;

namespace Bella.EditorTools
{
    /// <summary>
    /// Minimum-intervention keystone (docs/ARCHITECTURE.md §6): builds the entire
    /// Phase-1 playable grey-box scene from one menu click. Pon never wires a single
    /// Inspector reference by hand.
    ///
    /// Usage: Unity menu  Bella > Build Test Scene  → press Play.
    /// </summary>
    public static class BellaSceneBootstrap
    {
        private const string ScenePath = "Assets/Scenes/BellaTest.unity";
        private const string ConfigPath = "Assets/Config/NeedDecayConfig.asset";
        private const string MaterialsDir = "Assets/Art/Materials";

        [MenuItem("Bella/Build Test Scene")]
        public static void BuildScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // --- Lighting & camera -------------------------------------------------
            var lightGo = new GameObject("Directional Light");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.96f, 0.88f); // warm
            light.intensity = 1.1f;
            lightGo.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.62f, 0.6f, 0.66f);

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.78f, 0.88f, 0.95f); // soft sky
            camGo.transform.position = new Vector3(0f, 2.6f, -4.2f);
            camGo.transform.rotation = Quaternion.Euler(24f, 0f, 0f);
            camGo.AddComponent<AudioListener>();

            // --- Room --------------------------------------------------------------
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
            floor.GetComponent<Renderer>().sharedMaterial =
                Mat("Floor", new Color(0.93f, 0.85f, 0.72f)); // warm wooden tone

            CreateWall("Wall_Back", new Vector3(0, 1.25f, 6f), new Vector3(12f, 2.5f, 0.2f));
            CreateWall("Wall_Left", new Vector3(-6f, 1.25f, 0), new Vector3(0.2f, 2.5f, 12f));
            CreateWall("Wall_Right", new Vector3(6f, 1.25f, 0), new Vector3(0.2f, 2.5f, 12f));

            // --- Props: bowl + bed ---------------------------------------------------
            var bowl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bowl.name = "FoodBowl";
            bowl.transform.position = new Vector3(2.2f, 0.12f, 1.6f);
            bowl.transform.localScale = new Vector3(0.6f, 0.12f, 0.6f);
            bowl.GetComponent<Renderer>().sharedMaterial = Mat("Bowl", new Color(0.85f, 0.3f, 0.3f));

            var bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bed.name = "Bed";
            bed.transform.position = new Vector3(-2.4f, 0.15f, 2.0f);
            bed.transform.localScale = new Vector3(1.6f, 0.3f, 1.1f);
            bed.GetComponent<Renderer>().sharedMaterial = Mat("Bed", new Color(0.55f, 0.65f, 0.9f));

            // --- Bella (grey-box puppy from primitives) ------------------------------
            var bella = BuildPlaceholderBella(out Renderer faceRenderer,
                out Transform body, out Transform tail, out Transform head);

            // --- Components + wiring -------------------------------------------------
            var config = GetOrCreateConfig();

            var needs = bella.AddComponent<PetNeeds>();
            needs.SetConfig(config);

            bella.AddComponent<PetStateMachine>();

            var face = bella.AddComponent<FaceExpressionController>();
            face.SetFaceRenderer(faceRenderer);

            var puppy = bella.AddComponent<ProceduralPuppy>();
            puppy.Wire(body, tail, head);

            var controller = bella.AddComponent<BellaController>();
            controller.Wire(face, puppy, bowl.transform, bed.transform);

            var inputGo = new GameObject("Input");
            var interaction = inputGo.AddComponent<PetInteraction>();
            interaction.Wire(cam, controller);

            // --- UI ------------------------------------------------------------------
            BuildHud(needs, controller);

            // --- Save ----------------------------------------------------------------
            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("Bella",
                "Test scene built!\n\nPress Play:\n• Tap Bella to pet her (hearts!)\n• Feed button → she walks to the bowl\n• Sleep button → she naps in the bed\n\nClose Unity and reopen later to see offline decay.", "Let's go");
        }

        [MenuItem("Bella/Reset Save Data")]
        public static void ResetSave()
        {
            string path = Path.Combine(Application.persistentDataPath, "bella_save.json");
            if (File.Exists(path)) File.Delete(path);
            Debug.Log("[Bella] Save data deleted.");
        }

        // =====================================================================

        private static GameObject BuildPlaceholderBella(out Renderer faceRenderer,
            out Transform body, out Transform tail, out Transform head)
        {
            var golden = Mat("BellaGold", new Color(0.95f, 0.75f, 0.4f));
            var cream = Mat("BellaCream", new Color(0.98f, 0.92f, 0.78f));
            var honey = Mat("BellaHoney", new Color(0.78f, 0.55f, 0.28f));

            var root = new GameObject("Bella");
            root.transform.position = new Vector3(0f, 0f, 0.5f);

            // One collider on the root for pet-tap raycasts.
            var col = root.AddComponent<CapsuleCollider>();
            col.center = new Vector3(0f, 0.55f, 0f);
            col.radius = 0.45f;
            col.height = 1.1f;

            // Body — chubby puppy torso.
            body = Part(PrimitiveType.Sphere, "Body", root.transform,
                new Vector3(0f, 0.45f, 0f), new Vector3(0.7f, 0.6f, 0.95f), golden);

            // Head — oversized (kindchenschema: ~40% of volume, docs/ART_BIBLE.md §3).
            head = Part(PrimitiveType.Sphere, "Head", root.transform,
                new Vector3(0f, 0.95f, 0.45f), new Vector3(0.62f, 0.58f, 0.58f), golden);

            // Muzzle (cream).
            Part(PrimitiveType.Sphere, "Muzzle", head,
                new Vector3(0f, -0.18f, 0.42f), new Vector3(0.45f, 0.35f, 0.4f), cream);

            // Face sphere — the expression tint/texture target.
            var faceT = Part(PrimitiveType.Sphere, "Face", head,
                new Vector3(0f, 0.12f, 0.42f), new Vector3(0.5f, 0.42f, 0.25f), cream);
            faceRenderer = faceT.GetComponent<Renderer>();

            // Floppy ears (honey) — the signature Retriever silhouette.
            Part(PrimitiveType.Capsule, "Ear_L", head,
                new Vector3(-0.42f, 0.1f, 0f), new Vector3(0.18f, 0.32f, 0.1f), honey,
                Quaternion.Euler(0f, 0f, 25f));
            Part(PrimitiveType.Capsule, "Ear_R", head,
                new Vector3(0.42f, 0.1f, 0f), new Vector3(0.18f, 0.32f, 0.1f), honey,
                Quaternion.Euler(0f, 0f, -25f));

            // Tail: pivot at the body so the wag rotates around the base, not the middle.
            var tailPivot = new GameObject("TailPivot").transform;
            tailPivot.SetParent(root.transform, false);
            tailPivot.localPosition = new Vector3(0f, 0.55f, -0.45f);
            Part(PrimitiveType.Capsule, "TailMesh", tailPivot,
                new Vector3(0f, 0.12f, -0.12f), new Vector3(0.12f, 0.22f, 0.12f), golden,
                Quaternion.Euler(-40f, 0f, 0f));
            tail = tailPivot;

            // Four stubby legs.
            Leg(root.transform, "Leg_FL", new Vector3(-0.25f, 0.15f, 0.3f), golden);
            Leg(root.transform, "Leg_FR", new Vector3(0.25f, 0.15f, 0.3f), golden);
            Leg(root.transform, "Leg_BL", new Vector3(-0.25f, 0.15f, -0.3f), golden);
            Leg(root.transform, "Leg_BR", new Vector3(0.25f, 0.15f, -0.3f), golden);

            return root;
        }

        private static Transform Part(PrimitiveType type, string name, Transform parent,
            Vector3 localPos, Vector3 localScale, Material mat, Quaternion? localRot = null)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            Object.DestroyImmediate(go.GetComponent<Collider>()); // root capsule handles raycasts
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localScale = localScale;
            if (localRot.HasValue) go.transform.localRotation = localRot.Value;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go.transform;
        }

        private static void Leg(Transform parent, string name, Vector3 pos, Material mat) =>
            Part(PrimitiveType.Capsule, name, parent, pos, new Vector3(0.16f, 0.18f, 0.16f), mat);

        private static void CreateWall(string name, Vector3 pos, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = pos;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = Mat("Wall", new Color(0.92f, 0.88f, 0.95f));
        }

        // --- HUD built from code (icon-first; docs/GDD.md §2) --------------------

        private static void BuildHud(PetNeeds needs, BellaController controller)
        {
            var canvasGo = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            var es = new GameObject("EventSystem",
                typeof(UnityEngine.EventSystems.EventSystem),
                typeof(UnityEngine.EventSystems.StandaloneInputModule));
            es.transform.SetParent(null);

            var hud = canvasGo.AddComponent<NeedsHud>();

            // Need bars, top-left.
            var hunger = Bar(canvasGo.transform, "Food", 0);
            var hygiene = Bar(canvasGo.transform, "Wash", 1);
            var energy = Bar(canvasGo.transform, "Zzz", 2);
            var happiness = Bar(canvasGo.transform, "Joy", 3);

            // Treats label, top-right.
            var treats = Label(canvasGo.transform, "TreatsLabel", "Treats: 0", 34);
            var tr = treats.rectTransform;
            tr.anchorMin = tr.anchorMax = new Vector2(1f, 1f);
            tr.pivot = new Vector2(1f, 1f);
            tr.anchoredPosition = new Vector2(-30f, -30f);
            tr.sizeDelta = new Vector2(320f, 50f);

            hud.Wire(needs, hunger, hygiene, energy, happiness, treats);

            // Action buttons, bottom.
            MakeButton(canvasGo.transform, "FeedButton", "FEED", new Vector2(-180f, 90f),
                new Color(0.98f, 0.65f, 0.3f), controller, nameof(BellaController.RequestFeed));
            MakeButton(canvasGo.transform, "SleepButton", "SLEEP", new Vector2(180f, 90f),
                new Color(0.55f, 0.6f, 0.95f), controller, nameof(BellaController.RequestSleep));
        }

        private static Image Bar(Transform parent, string label, int index)
        {
            float y = -40f - index * 70f;

            var row = new GameObject($"Bar_{label}", typeof(RectTransform)).GetComponent<RectTransform>();
            row.SetParent(parent, false);
            row.anchorMin = row.anchorMax = new Vector2(0f, 1f);
            row.pivot = new Vector2(0f, 1f);
            row.anchoredPosition = new Vector2(30f, y);
            row.sizeDelta = new Vector2(420f, 54f);

            var text = Label(row, "Label", label, 30);
            var ltr = text.rectTransform;
            ltr.anchorMin = new Vector2(0f, 0f); ltr.anchorMax = new Vector2(0f, 1f);
            ltr.pivot = new Vector2(0f, 0.5f);
            ltr.anchoredPosition = Vector2.zero;
            ltr.sizeDelta = new Vector2(110f, 0f);
            text.alignment = TextAnchor.MiddleLeft;

            var bg = new GameObject("BarBG", typeof(Image)).GetComponent<Image>();
            bg.transform.SetParent(row, false);
            bg.color = new Color(0f, 0f, 0f, 0.25f);
            var btr = bg.rectTransform;
            btr.anchorMin = new Vector2(0f, 0.5f); btr.anchorMax = new Vector2(0f, 0.5f);
            btr.pivot = new Vector2(0f, 0.5f);
            btr.anchoredPosition = new Vector2(120f, 0f);
            btr.sizeDelta = new Vector2(280f, 34f);

            var fill = new GameObject("Fill", typeof(Image)).GetComponent<Image>();
            fill.transform.SetParent(bg.transform, false);
            fill.sprite = null;
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillAmount = 1f;
            var ftr = fill.rectTransform;
            ftr.anchorMin = Vector2.zero; ftr.anchorMax = Vector2.one;
            ftr.offsetMin = new Vector2(4f, 4f); ftr.offsetMax = new Vector2(-4f, -4f);

            return fill;
        }

        private static Text Label(Transform parent, string name, string content, int size)
        {
            var go = new GameObject(name, typeof(Text));
            go.transform.SetParent(parent, false);
            var text = go.GetComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = size;
            text.fontStyle = FontStyle.Bold;
            text.color = new Color(0.25f, 0.2f, 0.15f);
            text.alignment = TextAnchor.MiddleRight;
            return text;
        }

        private static void MakeButton(Transform parent, string name, string caption,
            Vector2 anchoredPos, Color color, BellaController target, string methodName)
        {
            var go = new GameObject(name, typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var img = go.GetComponent<Image>();
            img.color = color;
            var rt = img.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = new Vector2(300f, 110f);

            var label = Label(go.transform, "Text", caption, 40);
            label.color = Color.white;
            label.alignment = TextAnchor.MiddleCenter;
            var ltr = label.rectTransform;
            ltr.anchorMin = Vector2.zero; ltr.anchorMax = Vector2.one;
            ltr.offsetMin = ltr.offsetMax = Vector2.zero;

            var button = go.GetComponent<Button>();
            var action = (UnityEngine.Events.UnityAction)System.Delegate.CreateDelegate(
                typeof(UnityEngine.Events.UnityAction), target, methodName);
            UnityEventTools.AddPersistentListener(button.onClick, action);
        }

        // --- Assets ---------------------------------------------------------------

        private static NeedDecayConfig GetOrCreateConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<NeedDecayConfig>(ConfigPath);
            if (config != null) return config;
            Directory.CreateDirectory("Assets/Config");
            config = ScriptableObject.CreateInstance<NeedDecayConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
            return config;
        }

        private static Material Mat(string name, Color color)
        {
            Directory.CreateDirectory(MaterialsDir);
            string path = $"{MaterialsDir}/{name}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            // Pick the shader for the ACTIVE pipeline — URP shaders render magenta
            // when no URP asset is assigned in Graphics settings, and vice versa.
            bool urpActive = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null;
            Shader shader = urpActive ? Shader.Find("Universal Render Pipeline/Lit") : Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Standard");
            var mat = new Material(shader);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.15f);
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }
    }
}
