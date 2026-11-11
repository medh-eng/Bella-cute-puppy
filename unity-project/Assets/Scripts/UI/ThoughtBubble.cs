using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;

namespace Bella.UI
{
    /// <summary>
    /// Bella's thought bubble — the primary UI for pre-readers (docs/GDD.md §2).
    /// It floats above her head showing ONE icon for what she wants most:
    /// bone = hungry, bubbles = bath, Zzz = sleepy, ball = play, cross = feeling
    /// poorly. Tapping the bubble does that thing.
    /// </summary>
    public class ThoughtBubble : MonoBehaviour
    {
        private RectTransform rt;
        private RectTransform canvasRect;
        private Image icon;
        private Camera cam;
        private BellaController bella;
        private PetNeeds needs;
        private string wish = "";
        private float recheck;
        private float shown;      // 0..1 visibility
        private System.Action<string> onTapped;

        public string Wish => wish;

        public void Init(Transform hudRoot, Camera camera, BellaController b, System.Action<string> tapped)
        {
            cam = camera;
            bella = b;
            needs = b.Needs;
            onTapped = tapped;
            // Positions are worked out in the HUD root's own space (it is inset by the safe area).
            canvasRect = hudRoot as RectTransform;
            if (canvasRect == null) canvasRect = hudRoot.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            Image bubble = UIKit.Img("ThoughtBubble", hudRoot, Art.Icon("bubble"), Color.white, true);
            rt = bubble.rectTransform;
            UIKit.Place(rt, new Vector2(0f, 0f), new Vector2(0.25f, 0.05f), Vector2.zero, new Vector2(230f, 230f));
            icon = UIKit.Icon("Wish", rt, "bone", 118f);
            icon.rectTransform.anchoredPosition = new Vector2(0f, 26f);
            icon.gameObject.AddComponent<UIPulse>();

            Button btn = bubble.gameObject.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(() => { if (onTapped != null && wish != "") onTapped(wish); });
            rt.localScale = Vector3.zero;
        }

        private string ComputeWish()
        {
            if (needs.IsUnwell) return "clinic";
            if (needs.Hunger < 35f) return "feed";
            if (needs.Hygiene < 30f) return "bath";
            if (needs.Energy < 25f) return "sleep";
            if (needs.Happiness < 40f) return "play";
            return "";
        }

        private static string IconFor(string w)
        {
            switch (w)
            {
                case "clinic": return "health";
                case "feed": return "bone";
                case "bath": return "bubbles";
                case "sleep": return "zzz";
                case "play": return "ball";
                default: return "heart";
            }
        }

        private void LateUpdate()
        {
            if (bella == null || cam == null) return;

            recheck -= Time.deltaTime;
            if (recheck <= 0f)
            {
                recheck = 1f;
                string w = ComputeWish();
                if (w != wish)
                {
                    wish = w;
                    if (wish != "") icon.sprite = Art.Icon(IconFor(wish));
                }
            }

            bool visible = wish != "" && !bella.Busy && !bella.InActivity;
            shown = Mathf.MoveTowards(shown, visible ? 1f : 0f, Time.deltaTime * 4f);
            float s = shown * shown * (3f - 2f * shown);
            rt.localScale = new Vector3(s, s, 1f);
            if (shown <= 0f) return;

            Vector3 sp = cam.WorldToScreenPoint(bella.HeadPoint + Vector3.up * 0.32f);
            Vector2 local;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, sp, null, out local))
            {
                // anchors are bottom-left, the root rect's local origin is its centre
                Vector2 half = canvasRect.rect.size * 0.5f;
                Vector2 p = local + half + new Vector2(40f, 0f);
                p.x = Mathf.Clamp(p.x, 20f, canvasRect.rect.width - 200f);
                rt.anchoredPosition = p;
            }
        }
    }
}
