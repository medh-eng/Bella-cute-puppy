using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Save;

namespace Bella.UI
{
    /// <summary>
    /// Opening screen: full-picture backdrop, the logo and "Tap to play" over the title music.
    /// On the very first launch four story cards follow (tap to turn the page). All art is optional:
    /// a missing picture simply leaves the cream background.
    /// </summary>
    public class TitleScreen : MonoBehaviour
    {
        private Canvas canvas;
        private RectTransform root;
        private Text tapLabel;
        private Action onDone;
        private int card;
        private bool storyMode;
        private float clock;
        public bool IsOpen => canvas != null;

        public void Show(Action done)
        {
            onDone = done;
            canvas = UIKit.CreateCanvas("TitleCanvas", 90);
            canvas.transform.SetParent(transform, false);
            root = UIKit.Rect("Root", canvas.transform);
            UIKit.Stretch(root);
            Image cream = UIKit.Img("Cream", root, null, Art.Cream, true);
            UIKit.Stretch(cream.rectTransform);
            AddCover("UI/title_bg", true);

            Sprite logo = Art.TrySprite("UI/logo");
            if (logo != null)
            {
                Image l = UIKit.Img("Logo", root, logo, Color.white);
                l.preserveAspect = true;
                UIKit.Place(l.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 330f), new Vector2(900f, 700f));
            }
            else
            {
                Text t = UIKit.Label("Name", root, "Bella the Cute Puppy", 90, Art.Ink, TextAnchor.MiddleCenter);
                UIKit.Place(t.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 330f), new Vector2(900f, 300f));
            }

            tapLabel = UIKit.Label("Tap", root, "Tap to play!", 72, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(tapLabel.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 330f), new Vector2(900f, 120f));

            Button b = cream.gameObject.AddComponent<Button>();
            b.transition = Selectable.Transition.None;
            b.targetGraphic = cream;
            b.onClick.AddListener(OnTap);

            Hold(true);
        }

        private void Hold(bool on)
        {
            GameRoot g = GameRoot.Instance;
            if (g == null) return;
            if (g.Hud != null) g.Hud.SetVisible(!on);
            if (on)
            {
                if (g.DayNight != null) g.DayNight.SetMusicOverride(true);
                if (g.Audio != null && g.Audio.Has("music_title")) g.Audio.SetMusic("music_title");
            }
            else if (g.DayNight != null) g.DayNight.SetMusicOverride(false);
        }

        private void AddCover(string path, bool first)
        {
            Texture2D tex = Art.TryTex(path);
            if (tex == null) return;
            var go = new GameObject("Cover", typeof(RectTransform), typeof(RawImage));
            go.transform.SetParent(root, false);
            RawImage ri = go.GetComponent<RawImage>();
            ri.texture = tex; ri.raycastTarget = false;
            UIKit.Stretch(ri.rectTransform);
            // Fill the screen without stretching the picture: crop the sides or the top and bottom.
            float screen = (float)Screen.width / Mathf.Max(1, Screen.height);
            float pic = (float)tex.width / Mathf.Max(1, tex.height);
            float uw = 1f, uh = 1f;
            if (pic > screen) uw = screen / pic; else uh = pic / screen;
            ri.uvRect = new Rect((1f - uw) * 0.5f, (1f - uh) * 0.5f, uw, uh);
            go.transform.SetSiblingIndex(first ? 1 : root.childCount - 1);
        }

        private void Update()
        {
            if (canvas == null) return;
            clock += Time.deltaTime;
            if (tapLabel != null) tapLabel.transform.localScale = Vector3.one * (1f + 0.05f * Mathf.Sin(clock * 3f));
        }

        private void OnTap()
        {
            AudioManager.Play("pop");
            SaveData d = SaveSystem.Data;
            if (!storyMode && !d.introSeen && Art.TryTex("Story/intro_1") != null)
            {
                storyMode = true; card = 1;
                ShowCard();
                return;
            }
            if (storyMode && card < 4 && Art.TryTex("Story/intro_" + (card + 1)) != null)
            {
                card++;
                ShowCard();
                return;
            }
            if (storyMode) { d.introSeen = true; SaveSystem.Flush(); }
            Close();
        }

        private void ShowCard()
        {
            for (int i = root.childCount - 1; i >= 0; i--)
                if (root.GetChild(i).name != "Cream") Destroy(root.GetChild(i).gameObject);
            AddCover("Story/intro_" + card, false);
            string hint = card < 4 ? "Tap for more" : "Tap to start";
            Text t = UIKit.Label("Next", root, hint, 56, Color.white, TextAnchor.MiddleCenter);
            UIKit.Place(t.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 120f), new Vector2(900f, 100f));
            tapLabel = t;
        }

        private void Close()
        {
            if (canvas != null) Destroy(canvas.gameObject);
            canvas = null;
            Hold(false);
            if (onDone != null) onDone();
        }
    }
}
