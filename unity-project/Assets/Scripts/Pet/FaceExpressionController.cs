using UnityEngine;

namespace Bella.Pet
{
    public enum Expression { Happy, Excited, Sad, Sleepy, Begging, Soapy }

    /// <summary>
    /// Swaps Bella's facial expression. Architecture per docs/ARCHITECTURE.md section 4:
    /// AI-generated models have no blendshapes, so faces are TEXTURES swapped on the
    /// head/face renderer (Animal-Crossing style).
    ///
    /// Two modes, auto-detected:
    ///  - Texture mode (Phase 2+): assign faceTextures (index = Expression enum order);
    ///    the controller swaps mainTexture on the face renderer's material.
    ///  - Tint mode (Phase 1 grey-box): no textures assigned; the controller tints the
    ///    face renderer a readable mood color instead. Same public API, so the real
    ///    model drops in with zero code changes.
    /// </summary>
    public class FaceExpressionController : MonoBehaviour
    {
        [Tooltip("Renderer that shows the face (face plane / head mesh / placeholder sphere).")]
        [SerializeField] private Renderer faceRenderer;

        [Tooltip("One texture per Expression enum value, in order: Happy, Excited, Sad, Sleepy, Begging, Soapy. Leave empty in grey-box phase.")]
        [SerializeField] private Texture2D[] faceTextures;

        public Expression Current { get; private set; } = Expression.Happy;

        private MaterialPropertyBlock block;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        public void SetFaceRenderer(Renderer r) => faceRenderer = r;

        public void Set(Expression e)
        {
            if (e == Current && block != null) return;
            Current = e;
            Apply();
        }

        private void Apply()
        {
            if (faceRenderer == null) return;

            if (faceTextures != null && faceTextures.Length > (int)Current && faceTextures[(int)Current] != null)
            {
                // Texture mode — instantiates the material once (per-renderer), then swaps textures.
                faceRenderer.material.mainTexture = faceTextures[(int)Current];
                return;
            }

            // Tint mode (grey-box): readable mood colors, no material instantiation.
            block ??= new MaterialPropertyBlock();
            Color c = Current switch
            {
                Expression.Happy   => new Color(1.00f, 0.85f, 0.45f), // warm gold
                Expression.Excited => new Color(1.00f, 0.55f, 0.30f), // bright orange
                Expression.Sad     => new Color(0.55f, 0.65f, 0.95f), // soft blue
                Expression.Sleepy  => new Color(0.70f, 0.60f, 0.90f), // lavender
                Expression.Begging => new Color(1.00f, 0.95f, 0.70f), // pale cream
                Expression.Soapy   => new Color(0.70f, 0.95f, 1.00f), // bubble cyan
                _ => Color.white
            };
            faceRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, c); // URP Lit
            block.SetColor(ColorId, c);     // Standard / fallback
            faceRenderer.SetPropertyBlock(block);
        }

        private void Start() => Apply();
    }
}
