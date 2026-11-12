using System.Collections.Generic;
using UnityEngine;
using Bella.Core;

namespace Bella.Pet
{
    /// <summary>
    /// Loads Bella's rigged model (Resources/Bella/bella.fbx), then normalises it
    /// purely from BONE positions so the result does not depend on how Unity's FBX
    /// importer chose axes or scale: after Build() Bella stands on the pivot,
    /// faces the pivot's +Z, and is BaseHeight tall.
    ///
    /// If the model is missing or its skeleton is not found, a primitive placeholder
    /// puppy is built instead so the game stays fully playable.
    /// </summary>
    public class BellaModel : MonoBehaviour
    {
        /// <summary>Height of the source mesh in Blender units (tools/pipeline).</summary>
        public const float SourceHeight = 0.943f;
        public const float BaseHeight = 0.95f;

        public Transform Pivot { get; private set; }
        public Transform ModelRoot { get; private set; }
        public bool IsRealModel { get; private set; }
        public Renderer BodyRenderer { get; private set; }
        public Renderer FaceRenderer { get; private set; }
        public Animator Animator { get; private set; }
        public AnimationClip Clip { get; private set; }

        public Transform Root, Hips, Spine, Chest, Neck, Head, EarL, EarR;
        public readonly List<Transform> Tail = new List<Transform>();

        // Accessory / effect anchors (children of bones, aligned to the pivot's axes).
        public Transform HeadTop, Eyes, EarSide, NeckFront, Back, Nose, HeadCentre;

        private float unitsPerSource = 1f;

        public void Build()
        {
            var pivotGo = new GameObject("Pivot");
            Pivot = pivotGo.transform;
            Pivot.SetParent(transform, false);
            unitsPerSource = BaseHeight / SourceHeight;

            bool ok = false;
            try { ok = TryBuildReal(); }
            catch (System.Exception e)
            {
                Diagnostics.Note("Model setup threw: " + e.Message);
                ok = false;
            }

            if (!ok)
            {
                if (ModelRoot != null) Destroy(ModelRoot.gameObject);
                Tail.Clear();
                BuildPlaceholder();
            }
            BuildAnchors();
            Diagnostics.Fact(IsRealModel ? "Bella model: rigged FBX loaded." : "Bella model: PLACEHOLDER in use.");
        }

        // ---------------------------------------------------------------- real model

        private bool TryBuildReal()
        {
            GameObject prefab = Resources.Load<GameObject>("Bella/bella");
            if (prefab == null)
            {
                Diagnostics.Note("Resources/Bella/bella.fbx not found (or not imported as a model).");
                return false;
            }

            // The imported model sits inside a holder, and ALL normalising (turn, size, position) is
            // done on the holder. The baked take also carries keys for the model's own top object;
            // if we adjusted that object directly the animation would undo it every frame.
            var holder = new GameObject("BellaHolder");
            holder.transform.SetParent(Pivot, false);
            ModelRoot = holder.transform;
            GameObject inst = Instantiate(prefab, ModelRoot);
            inst.name = "BellaModel";

            Root = Art.FindDeep(ModelRoot, "Root");
            Hips = Art.FindDeep(ModelRoot, "Hips");
            Spine = Art.FindDeep(ModelRoot, "Spine");
            Chest = Art.FindDeep(ModelRoot, "Chest");
            Neck = Art.FindDeep(ModelRoot, "Neck");
            Head = Art.FindDeep(ModelRoot, "Head");
            EarL = Art.FindDeep(ModelRoot, "Ear_L");
            EarR = Art.FindDeep(ModelRoot, "Ear_R");
            for (int i = 1; i <= 3; i++)
            {
                Transform t = Art.FindDeep(ModelRoot, "Tail_" + i);
                if (t != null) Tail.Add(t);
            }
            if (Root == null || Hips == null || Head == null)
            {
                Diagnostics.Note("Bella skeleton bones (Root/Hips/Head) not found in the imported model.");
                return false;
            }

            SkinnedMeshRenderer smr = inst.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (smr == null)
            {
                Diagnostics.Note("Bella model has no SkinnedMeshRenderer.");
                return false;
            }

            // --- 1. Orientation. Known rest-pose vectors (Unity axes: y up, z forward):
            //        Root->Head = (0, 0.52, 0.23), Root->Hips = (0, 0.30, -0.25)
            Vector3 a = Head.position - Root.position;
            Vector3 b = Hips.position - Root.position;
            if (a.sqrMagnitude < 1e-10f || b.sqrMagnitude < 1e-10f)
            {
                Diagnostics.Note("Bella skeleton is degenerate (zero-length reference bones).");
                return false;
            }
            Quaternion have = Quaternion.LookRotation(a.normalized, b.normalized);
            Quaternion want = Pivot.rotation * Quaternion.LookRotation(
                new Vector3(0f, 0.52f, 0.23f).normalized, new Vector3(0f, 0.30f, -0.25f).normalized);
            ModelRoot.rotation = (want * Quaternion.Inverse(have)) * ModelRoot.rotation;

            // --- 2. Scale, from the same bone distance (0.5686 source units).
            float dist = (Head.position - Root.position).magnitude;
            float wanted = 0.5686f * unitsPerSource;
            ModelRoot.localScale = ModelRoot.localScale * (wanted / dist);

            // --- 3. Position: the Root bone sits on the floor under her tummy.
            ModelRoot.position += Pivot.position - Root.position;

            // --- Renderer + material (texture is applied by FaceExpressionController).
            smr.updateWhenOffscreen = true;
            smr.sharedMaterial = Art.Lit(Color.white);
            smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            BodyRenderer = smr;
            FaceRenderer = smr;

            // --- Animation: one baked take, sampled by BellaAnimator through Playables.
            Animator = inst.GetComponent<Animator>();
            if (Animator == null) Animator = inst.AddComponent<Animator>();
            Animator.applyRootMotion = false;
            Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            AnimationClip[] clips = Resources.LoadAll<AnimationClip>("Bella/bella");
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null || clips[i].name.StartsWith("__preview__")) continue;
                if (Clip == null || clips[i].length > Clip.length) Clip = clips[i];
            }
            if (Clip == null)
                Diagnostics.Note("No animation clip found inside bella.fbx — Bella will slide instead of walking.");
            else
                Diagnostics.Fact("Animation take '" + Clip.name + "' length " + Clip.length.ToString("0.00") + "s (expected ~24.97s).");

            IsRealModel = true;
            return true;
        }

        // ---------------------------------------------------------------- placeholder

        private void BuildPlaceholder()
        {
            IsRealModel = false;
            Animator = null;
            Clip = null;
            var gold = new Color(0.95f, 0.75f, 0.4f);
            var cream = new Color(0.98f, 0.92f, 0.78f);
            var honey = new Color(0.78f, 0.55f, 0.28f);

            var root = new GameObject("BellaPlaceholder");
            ModelRoot = root.transform;
            ModelRoot.SetParent(Pivot, false);
            Root = ModelRoot;

            Hips = Art.Prim(PrimitiveType.Sphere, "Body", ModelRoot, new Vector3(0f, 0.38f, 0f), new Vector3(0.5f, 0.45f, 0.72f), gold).transform;
            Spine = Hips; Chest = Hips; Neck = Hips;
            BodyRenderer = Hips.GetComponent<Renderer>();

            var headGo = new GameObject("Head");
            Head = headGo.transform;
            Head.SetParent(ModelRoot, false);
            Head.localPosition = new Vector3(0f, 0.66f, 0.3f);
            Art.Prim(PrimitiveType.Sphere, "HeadMesh", Head, Vector3.zero, new Vector3(0.52f, 0.48f, 0.48f), gold);
            Art.Prim(PrimitiveType.Sphere, "Muzzle", Head, new Vector3(0f, -0.08f, 0.2f), new Vector3(0.24f, 0.18f, 0.2f), cream);
            Art.Prim(PrimitiveType.Sphere, "NoseTip", Head, new Vector3(0f, -0.04f, 0.3f), new Vector3(0.07f, 0.06f, 0.05f), Art.Ink);
            Art.Prim(PrimitiveType.Sphere, "Eye_L", Head, new Vector3(-0.1f, 0.05f, 0.21f), new Vector3(0.09f, 0.1f, 0.05f), Art.Ink);
            Art.Prim(PrimitiveType.Sphere, "Eye_R", Head, new Vector3(0.1f, 0.05f, 0.21f), new Vector3(0.09f, 0.1f, 0.05f), Art.Ink);
            FaceRenderer = Art.Prim(PrimitiveType.Sphere, "Face", Head, new Vector3(0f, 0.02f, 0.1f), new Vector3(0.44f, 0.38f, 0.3f), cream).GetComponent<Renderer>();

            EarL = new GameObject("Ear_L").transform; EarL.SetParent(Head, false); EarL.localPosition = new Vector3(-0.24f, 0.12f, 0f);
            EarR = new GameObject("Ear_R").transform; EarR.SetParent(Head, false); EarR.localPosition = new Vector3(0.24f, 0.12f, 0f);
            Art.Prim(PrimitiveType.Sphere, "EarMesh", EarL, new Vector3(-0.03f, -0.14f, 0f), new Vector3(0.12f, 0.32f, 0.2f), honey);
            Art.Prim(PrimitiveType.Sphere, "EarMesh", EarR, new Vector3(0.03f, -0.14f, 0f), new Vector3(0.12f, 0.32f, 0.2f), honey);

            var tailGo = new GameObject("Tail_1");
            tailGo.transform.SetParent(ModelRoot, false);
            tailGo.transform.localPosition = new Vector3(0f, 0.45f, -0.34f);
            Art.Prim(PrimitiveType.Capsule, "TailMesh", tailGo.transform, new Vector3(0f, 0.08f, -0.1f), new Vector3(0.09f, 0.14f, 0.09f), gold, new Vector3(-50f, 0f, 0f));
            Tail.Add(tailGo.transform);

            float[] xs = { -0.16f, 0.16f };
            float[] zs = { 0.22f, -0.22f };
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    Art.Prim(PrimitiveType.Capsule, "Leg", ModelRoot, new Vector3(xs[i], 0.13f, zs[j]), new Vector3(0.14f, 0.14f, 0.14f), gold);
        }

        // ---------------------------------------------------------------- anchors

        /// <summary>Source-model (Blender) coordinates → world, valid in the rest pose.</summary>
        public Vector3 SourcePoint(float bx, float by, float bz)
        {
            var local = new Vector3(bx, bz, -(by - 0.08f)) * unitsPerSource;
            return Pivot.TransformPoint(local);
        }

        private Transform Anchor(string name, Transform bone, float bx, float by, float bz)
        {
            var go = new GameObject(name);
            Transform t = go.transform;
            t.position = SourcePoint(bx, by, bz);
            t.rotation = Pivot.rotation;
            t.SetParent(bone != null ? bone : Pivot, true);
            // Children are authored in pivot-space metres regardless of bone scale.
            Vector3 ls = t.lossyScale;
            if (ls.x > 1e-5f && ls.y > 1e-5f && ls.z > 1e-5f)
                t.localScale = new Vector3(t.localScale.x / ls.x, t.localScale.y / ls.y, t.localScale.z / ls.z);
            return t;
        }

        private void BuildAnchors()
        {
            HeadTop = Anchor("Anchor_HeadTop", Head, 0f, -0.14f, 0.885f);
            Eyes = Anchor("Anchor_Eyes", Head, 0f, -0.335f, 0.695f);
            EarSide = Anchor("Anchor_EarSide", Head, 0.17f, -0.2f, 0.85f);
            Nose = Anchor("Anchor_Nose", Head, 0f, -0.41f, 0.655f);
            HeadCentre = Anchor("Anchor_HeadCentre", Head, 0f, -0.15f, 0.70f);
            NeckFront = Anchor("Anchor_NeckFront", Chest != null ? Chest : Hips, 0f, -0.31f, 0.40f);
            Back = Anchor("Anchor_Back", Spine != null ? Spine : Hips, 0f, 0.12f, 0.47f);
        }
    }
}
