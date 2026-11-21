using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;

namespace Bella.UI
{
    /// <summary>The "Play" menu: pick one of twelve games. Tap outside or the X to close.</summary>
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
            UIKit.Place(panel.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1000f, 1660f));

            Text title = UIKit.Label("Title", panel.transform, "Let's play!", 70, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -90f), new Vector2(800f, 110f));

            string[] ids = { "fetch", "catch", "dig", "ball", "frisbee", "arrow", "blocks", "jigsaw", "car", "hide", "memory", "bubbles" };
            string[] icons = { "place_park", "place_kitchen", "place_garden", "toy_tennis", "toy_frisbee", "game_arrow", "game_blocks", "game_puzzle", "game_car", "question", "paw", "bubbles" };
            string[] names = { "Fetch", "Treat Catch", "Dig", "Ball Catch", "Frisbee", "Arrows", "Blocks", "Jigsaw", "Car Race", "Hide & Seek", "Paw Memory", "Bubble Pop" };
            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];
                int col = i % 3, row = i / 3;
                Image card = UIKit.Panel("Card_" + id, panel.transform, Color.white);
                UIKit.Place(card.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2((col - 1) * 310f, -330f - row * 330f), new Vector2(290f, 310f));
                Image pic = UIKit.Icon("Pic", card.transform, icons[i], 190f);
                UIKit.Place(pic.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -120f), new Vector2(190f, 190f));
                Text n = UIKit.Label("Name", card.transform, names[i], 44, Art.Ink, TextAnchor.MiddleCenter);
                UIKit.Place(n.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 52f), new Vector2(270f, 70f));
                n.resizeTextForBestFit = true; n.resizeTextMinSize = 24; n.resizeTextMaxSize = 44;
                Button b = card.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = card;
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
