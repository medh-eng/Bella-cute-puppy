using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.MiniGames;
using Bella.Save;

namespace Bella.UI
{
    /// <summary>The "Places" menu: six places to visit. Locked ones show their price in treats; tapping buys and goes.</summary>
    public class PlacePicker : MonoBehaviour
    {
        private Canvas canvas;
        public bool IsOpen => canvas != null;

        public void Show(int treats, Action<string> onPick)
        {
            if (canvas != null) return;
            canvas = UIKit.CreateCanvas("PlacePickerCanvas", 40);
            canvas.transform.SetParent(transform, false);
            RectTransform root = UIKit.SafeRoot("Root", canvas.transform);

            Image dim = UIKit.Dim(canvas.transform, 0.55f);
            Button dimBtn = dim.gameObject.AddComponent<Button>();
            dimBtn.transition = Selectable.Transition.None;
            dimBtn.onClick.AddListener(Close);

            Image panel = UIKit.Panel("Panel", root, new Color(1f, 0.97f, 0.9f, 0.98f));
            UIKit.Place(panel.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(1000f, 1330f));

            Text title = UIKit.Label("Title", panel.transform, "Where to?", 70, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -90f), new Vector2(800f, 110f));

            SaveData d = SaveSystem.Data;
            for (int i = 0; i < PlaceDef.All.Length; i++)
            {
                PlaceDef def = PlaceDef.All[i];
                bool owned = def.Cost == 0 || d.ownedPlaces.Contains(def.Id);
                int col = i % 2, row = i / 2;
                Image card = UIKit.Panel("Card_" + def.Id, panel.transform, owned ? Color.white : new Color(0.93f, 0.93f, 0.93f));
                UIKit.Place(card.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2((col - 0.5f) * 460f, -380f - row * 330f), new Vector2(430f, 310f));
                Image pic = UIKit.Icon("Pic", card.transform, def.Icon, 170f);
                UIKit.Place(pic.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -110f), new Vector2(170f, 170f));
                if (!owned) pic.color = new Color(1f, 1f, 1f, 0.55f);
                string caption = owned ? def.Name : def.Name + "  " + def.Cost;
                Text n = UIKit.Label("Name", card.transform, caption, 44, treats >= def.Cost || owned ? Art.Ink : new Color(0.55f, 0.4f, 0.4f), TextAnchor.MiddleCenter);
                UIKit.Place(n.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 52f), new Vector2(400f, 70f));
                n.resizeTextForBestFit = true; n.resizeTextMinSize = 24; n.resizeTextMaxSize = 44;
                if (!owned)
                {
                    Image tr = UIKit.Icon("Cost", card.transform, "treat", 56f);
                    UIKit.Place(tr.rectTransform, new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-44f, -44f), new Vector2(56f, 56f));
                }
                string id = def.Id;
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
