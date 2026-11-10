using UnityEngine;
using UnityEngine.UI;
using Bella.Needs;

namespace Bella.UI
{
    /// <summary>
    /// Icon-first need bars (pre-reader friendly — docs/GDD.md §2). The bars are the
    /// "parent layer"; Bella's thought bubbles (Phase 3) will be the primary kid UI.
    /// Built programmatically by the scene builder; this component just updates fills.
    /// </summary>
    public class NeedsHud : MonoBehaviour
    {
        [SerializeField] private PetNeeds needs;
        [SerializeField] private Image hungerFill;
        [SerializeField] private Image hygieneFill;
        [SerializeField] private Image energyFill;
        [SerializeField] private Image happinessFill;
        [SerializeField] private Text treatsLabel;

        public void Wire(PetNeeds n, Image hunger, Image hygiene, Image energy, Image happiness, Text treats)
        {
            needs = n;
            hungerFill = hunger; hygieneFill = hygiene;
            energyFill = energy; happinessFill = happiness;
            treatsLabel = treats;
        }

        private void OnEnable()
        {
            if (needs != null) needs.OnNeedsChanged += Refresh;
        }

        private void OnDisable()
        {
            if (needs != null) needs.OnNeedsChanged -= Refresh;
        }

        private void Start() => Refresh();

        private void Refresh()
        {
            if (needs == null) return;
            float max = needs.Config != null ? needs.Config.maxValue : 100f;
            SetBar(hungerFill, needs.Hunger / max);
            SetBar(hygieneFill, needs.Hygiene / max);
            SetBar(energyFill, needs.Energy / max);
            SetBar(happinessFill, needs.Happiness / max);
            if (treatsLabel != null) treatsLabel.text = $"Treats: {needs.Treats}";
        }

        private static void SetBar(Image img, float v01)
        {
            if (img == null) return;
            img.fillAmount = Mathf.Clamp01(v01);
            // Green -> amber -> soft red, never alarming (kindness cap in color form).
            img.color = Color.Lerp(new Color(0.95f, 0.55f, 0.45f), new Color(0.45f, 0.85f, 0.5f), v01);
        }
    }
}
