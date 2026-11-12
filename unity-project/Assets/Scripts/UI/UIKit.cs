using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Bella.Core;

namespace Bella.UI
{
    /// <summary>Squish-on-press feedback for every button.</summary>
    public class PressFx : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private float target = 1f;
        private float current = 1f;

        public void OnPointerDown(PointerEventData eventData) { target = 0.9f; }
        public void OnPointerUp(PointerEventData eventData) { target = 1f; }

        private void OnDisable() { target = 1f; current = 1f; transform.localScale = Vector3.one; }

        private void Update()
        {
            current = Mathf.Lerp(current, target, 1f - Mathf.Exp(-Time.unscaledDeltaTime * 22f));
            transform.localScale = new Vector3(current, current, 1f);
        }
    }

    /// <summary>One-shot "pop in" bounce.</summary>
    public class UIPop : MonoBehaviour
    {
        private float t = 1f;

        public void Play() { t = 0f; }

        private void Update()
        {
            if (t >= 1f) return;
            t = Mathf.Min(1f, t + Time.unscaledDeltaTime * 3.2f);
            float s = 1f + Mathf.Sin(t * Mathf.PI) * 0.22f * (1f - t) + (t < 0.25f ? (t / 0.25f - 1f) * 0.4f : 0f);
            transform.localScale = new Vector3(s, s, 1f);
            if (t >= 1f) transform.localScale = Vector3.one;
        }
    }

    /// <summary>Gentle endless pulse to draw a pre-reader's eye to "tap me".</summary>
    public class UIPulse : MonoBehaviour
    {
        public float amount = 0.08f;
        public float speed = 4f;

        private void OnDisable() { transform.localScale = Vector3.one; }

        private void Update()
        {
            float s = 1f + Mathf.Sin(Time.unscaledTime * speed) * amount;
            transform.localScale = new Vector3(s, s, 1f);
        }
    }

    /// <summary>
    /// Keeps a full-screen UI root inside the phone's safe area (camera cut-outs,
    /// rounded corners, gesture bar). Android 15+ always draws apps edge-to-edge,
    /// so without this the top row of the HUD could sit under the camera hole.
    /// </summary>
    public class SafeArea : MonoBehaviour
    {
        private RectTransform rt;
        private Rect applied = new Rect(-1f, -1f, -1f, -1f);
        private int w, h;

        private void OnEnable()
        {
            rt = transform as RectTransform;
            Apply();
        }

        private void Update()
        {
            if (Screen.width != w || Screen.height != h || Screen.safeArea != applied) Apply();
        }

        private void Apply()
        {
            if (rt == null) return;
            w = Screen.width; h = Screen.height;
            applied = Screen.safeArea;
            if (w <= 0 || h <= 0 || applied.width <= 0f || applied.height <= 0f) return;
            Vector2 min = new Vector2(applied.xMin / w, applied.yMin / h);
            Vector2 max = new Vector2(applied.xMax / w, applied.yMax / h);
            if (float.IsNaN(min.x) || float.IsNaN(min.y) || float.IsNaN(max.x) || float.IsNaN(max.y)) return;
            rt.anchorMin = new Vector2(Mathf.Clamp01(min.x), Mathf.Clamp01(min.y));
            rt.anchorMax = new Vector2(Mathf.Clamp01(max.x), Mathf.Clamp01(max.y));
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    /// <summary>Code-only uGUI construction helpers (reference resolution 1080 x 1920, portrait).</summary>
    public static class UIKit
    {
        public static Canvas CreateCanvas(string name, int sortOrder)
        {
            var go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortOrder;
            CanvasScaler scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            // Expand: the canvas is never smaller than 1080 x 1920 in either direction, so a layout
            // designed for that size fits tall 20:9 phones and squarer tablets alike.
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            return canvas;
        }

        /// <summary>Full-screen root that stays inside the device's safe area.</summary>
        public static RectTransform SafeRoot(string name, Transform parent)
        {
            RectTransform rt = Rect(name, parent);
            Stretch(rt);
            rt.gameObject.AddComponent<SafeArea>();
            return rt;
        }

        public static RectTransform Rect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            return rt;
        }

        /// <summary>Fixed-size element anchored to one point of its parent.</summary>
        public static void Place(RectTransform rt, Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 size)
        {
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
        }

        public static void Stretch(RectTransform rt, float left = 0f, float bottom = 0f, float right = 0f, float top = 0f)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(left, bottom);
            rt.offsetMax = new Vector2(-right, -top);
        }

        public static Image Img(string name, Transform parent, Sprite sprite, Color color, bool raycast = false)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Image img = go.GetComponent<Image>();
            img.sprite = sprite;
            img.color = color;
            img.raycastTarget = raycast;
            img.preserveAspect = false;
            return img;
        }

        public static Image Icon(string name, Transform parent, string icon, float size)
        {
            Image img = Img(name, parent, Art.Icon(icon), Color.white);
            img.preserveAspect = true;
            Place(img.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(size, size));
            return img;
        }

        public static Image Panel(string name, Transform parent, Color color, bool raycast = true)
        {
            Image img = Img(name, parent, Art.Panel(), color, raycast);
            img.type = Image.Type.Sliced;
            return img;
        }

        public static Text Label(string name, Transform parent, string content, int size, Color color,
            TextAnchor align = TextAnchor.MiddleCenter)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            Text text = go.GetComponent<Text>();
            text.font = Art.GetFont();
            text.text = content;
            text.fontSize = size;
            text.fontStyle = FontStyle.Bold;
            text.color = color;
            text.alignment = align;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        /// <summary>Round icon button with press feedback and a tap sound.</summary>
        public static Button IconButton(string name, Transform parent, string icon, float size, Color bg, UnityAction onClick)
        {
            Image back = Img(name, parent, Art.Icon("circle"), bg, true);
            back.rectTransform.sizeDelta = new Vector2(size, size);
            Image ring = Img("Shade", back.transform, Art.Icon("circle"), new Color(0f, 0f, 0f, 0.10f));
            Place(ring.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -size * 0.045f), new Vector2(size, size));
            ring.transform.SetAsFirstSibling();
            Image face = Img("Face", back.transform, Art.Icon("circle"), bg);
            Stretch(face.rectTransform);
            Icon("Icon", back.transform, icon, size * 0.62f);
            back.color = new Color(1f, 1f, 1f, 0f);   // the invisible root only catches the tap

            Button b = back.gameObject.AddComponent<Button>();
            b.transition = Selectable.Transition.None;
            b.targetGraphic = back;
            if (onClick != null) b.onClick.AddListener(onClick);
            back.gameObject.AddComponent<PressFx>();
            return b;
        }

        public static Button TextButton(string name, Transform parent, string caption, Vector2 size, Color bg, UnityAction onClick, int fontSize = 44)
        {
            Image back = Panel(name, parent, bg);
            back.rectTransform.sizeDelta = size;
            Text label = Label("Text", back.transform, caption, fontSize, Color.white);
            Stretch(label.rectTransform, 12f, 6f, 12f, 6f);
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 18;
            label.resizeTextMaxSize = fontSize;
            Button b = back.gameObject.AddComponent<Button>();
            b.transition = Selectable.Transition.None;
            b.targetGraphic = back;
            if (onClick != null) b.onClick.AddListener(onClick);
            back.gameObject.AddComponent<PressFx>();
            return b;
        }

        public static void Pop(GameObject go)
        {
            if (go == null) return;
            UIPop p = go.GetComponent<UIPop>();
            if (p == null) p = go.AddComponent<UIPop>();
            p.Play();
        }

        /// <summary>Full-screen dim layer that blocks taps to whatever is behind it.</summary>
        public static Image Dim(Transform parent, float alpha = 0.55f)
        {
            Image dim = Img("Dim", parent, null, new Color(0.16f, 0.1f, 0.2f, alpha), true);
            Stretch(dim.rectTransform);
            return dim;
        }
    }
}
