using UnityEngine;
using Bella.Core;

namespace Bella.Pet
{
    public enum Expression { Happy, Excited, Sad, Sleepy, Begging, Soapy }

    /// <summary>
    /// Bella's facial expressions (docs/ARCHITECTURE.md §4). AI-generated models
    /// have no blendshapes, so each expression is a complete body texture whose
    /// face region was repainted (tools/pipeline/faces.py); switching expression
    /// swaps the texture on her material. Eyes also blink on their own.
    ///
    /// On the placeholder puppy (no textures) the face sphere is tinted instead.
    /// </summary>
    public class FaceExpressionController : MonoBehaviour
    {
        private Renderer faceRenderer;
        private Material material;
        private readonly Texture2D[] textures = new Texture2D[6];
        private Texture2D blinkTexture;
        private bool textureMode;

        private float blinkTimer = 3f;
        private float blinkHold;
        private float holdTimer;
        private Expression heldExpression;

        public Expression Current { get; private set; } = Expression.Happy;

        public void Init(Renderer renderer, bool realModel)
        {
            faceRenderer = renderer;
            if (faceRenderer == null) return;
            material = faceRenderer.material;   // per-renderer instance

            if (realModel)
            {
                string[] names = { "happy", "excited", "sad", "sleepy", "begging", "blink" };
                int found = 0;
                for (int i = 0; i < names.Length; i++)
                {
                    textures[i] = Resources.Load<Texture2D>("Bella/Faces/bella_face_" + names[i]);
                    if (textures[i] != null) found++;
                }
                blinkTexture = textures[5];
                textureMode = textures[0] != null;
                Diagnostics.Fact("Face textures loaded: " + found + "/6");
                if (!textureMode) Diagnostics.Note("Bella's face textures are missing (Resources/Bella/Faces).");
            }
            Apply();
        }

        public void Set(Expression e)
        {
            if (holdTimer > 0f) { heldExpression = e; return; }
            if (e == Current) return;
            Current = e;
            Apply();
        }

        /// <summary>Show an expression for a moment, ignoring other Set() calls meanwhile.</summary>
        public void Flash(Expression e, float seconds)
        {
            heldExpression = Current;
            holdTimer = 0f;
            Current = e;
            Apply();
            holdTimer = seconds;
        }

        private void Update()
        {
            if (holdTimer > 0f)
            {
                holdTimer -= Time.deltaTime;
                if (holdTimer <= 0f)
                {
                    Current = heldExpression;
                    Apply();
                }
            }

            if (!textureMode || blinkTexture == null) return;
            bool eyesOpen = Current == Expression.Happy || Current == Expression.Sad || Current == Expression.Begging;
            if (blinkHold > 0f)
            {
                blinkHold -= Time.deltaTime;
                if (blinkHold <= 0f) Apply();
                return;
            }
            if (!eyesOpen) return;
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                blinkTimer = Random.Range(2.2f, 5.5f);
                blinkHold = 0.13f;
                material.mainTexture = blinkTexture;
            }
        }

        private void Apply()
        {
            if (material == null) return;
            if (textureMode)
            {
                Texture2D t = textures[(int)Current];
                if (t == null) t = textures[0];
                material.mainTexture = t;
                return;
            }

            Color c;
            switch (Current)
            {
                case Expression.Excited: c = new Color(1.00f, 0.80f, 0.55f); break;
                case Expression.Sad: c = new Color(0.75f, 0.80f, 0.95f); break;
                case Expression.Sleepy: c = new Color(0.82f, 0.76f, 0.92f); break;
                case Expression.Begging: c = new Color(1.00f, 0.97f, 0.80f); break;
                case Expression.Soapy: c = new Color(0.80f, 0.95f, 1.00f); break;
                default: c = new Color(0.98f, 0.92f, 0.78f); break;
            }
            material.color = c;
        }
    }
}
