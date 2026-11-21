using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;

namespace Bella.UI
{
    /// <summary>The "Play" menu: pick Fetch, Treat Catch or Dig for Treasure. Tap outside or the X to close.</summary>
    public class GamePicker : MonoBehaviour
    {
        private Canvas canvas;
        public bool IsOpen => canvas != null;

        public void Show(Action<string> onPick)
        {
            if (canvas != null) return;
            canvas = UIKit.CreateCanvas("GamePickerCanvas", 40);
            canvas.transform.SetParent(transform, false);
            RectTransform root = UIKit.SafeRoot("Root", canvas.transform);

            Image dim = UIKit.Dim(canvas.transform, 0.55f);
            Button dimBtn = dim.gameObject.AddComponent<Button>();
            dimBtn.transition = Selectable.Transition.None;
            dimBtn.onClick.AddListener(Close);

            Image panel = UIKit.Panel("Panel", root, new Color(1f, 0.97f, 0.9f, 0.98f));
            UIKit.Place(panel.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(960f, 1180f));

            Text title = UIKit.Label("Title", panel.transform, "Let's play!", 70, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -90f), new Vector2(800f, 110f));

            string[] ids = { "fetch", "catch", "dig" };
            string[] icons = { "place_park", "place_kitchen", "place_garden" };
            string[] names = { "Fetch", "Treat Catch", "Dig for Treasure" };
            string[] about = { "Flick the ball", "Slide to catch treats", "Tap mounds to dig" };
            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];
                Image row = UIKit.Panel("Row_" + id, panel.transform, Color.white);
                UIKit.Place(row.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -280f - i * 270f), new Vector2(860f, 240f));
                Image pic = UIKit.Icon("Pic", row.transform, icons[i], 190f);
                UIKit.Place(pic.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(125f, 0f), new Vector2(190f, 190f));
                Text n = UIKit.Label("Name", row.transform, names[i], 54, Art.Ink, TextAnchor.MiddleLeft);
                UIKit.Place(n.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(250f, 38f), new Vector2(580f, 80f));
                n.resizeTextForBestFit = true; n.resizeTextMinSize = 28; n.resizeTextMaxSize = 54;
                Text a = UIKit.Label("About", row.transform, about[i], 38, Art.Ink, TextAnchor.MiddleLeft);
                UIKit.Place(a.rectTransform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(250f, -42f), new Vector2(580f, 64f));
                a.color = new Color(Art.Ink.r, Art.Ink.g, Art.Ink.b, 0.7f);
                a.resizeTextForBestFit = true; a.resizeTextMinSize = 22; a.resizeTextMaxSize = 38;
                Button b = row.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = row;
                b.onClick.AddListener(() => { AudioManager.Play("pop"); Close(); if (onPick != null) onPick(id); });
            }

            Button close = UIKit.IconButton("Close", panel.transform, "close", 120f, Art.Cream, Close);
            UIKit.Place(close.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-30f, -30f), new Vector2(120f, 120f));
            UIKit.Pop(panel.gameObject);
            AudioManager.Play("tap");
        }

        public void Close()
        {
            if (canvas != null) Destroy(canvas.gameObject);
            canvas = null;
        }
    }
}
