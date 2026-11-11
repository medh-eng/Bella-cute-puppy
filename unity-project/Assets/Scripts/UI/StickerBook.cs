using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Save;
using Bella.Systems;

namespace Bella.UI
{
    /// <summary>
    /// The sticker memory-book (docs/GDD.md §3): a grid of moments shared with
    /// Bella. Earned stickers are bright; the rest are soft "?" circles so there is
    /// always something to look forward to. Collection without money.
    /// </summary>
    public class StickerBook : MonoBehaviour
    {
        private Canvas canvas;
        private GameObject root;
        private readonly List<Image> badges = new List<Image>();
        private readonly List<Image> icons = new List<Image>();
        private readonly List<Text> titles = new List<Text>();
        private Text counter;

        public void Init()
        {
            canvas = UIKit.CreateCanvas("StickerCanvas", 40);
            canvas.transform.SetParent(transform, false);
            root = UIKit.Rect("StickerRoot", canvas.transform).gameObject;
            UIKit.Stretch(root.GetComponent<RectTransform>());
            UIKit.Dim(root.transform, 0.6f);

            Image card = UIKit.Panel("Card", root.transform, Art.Cream);
            UIKit.Stretch(card.rectTransform, 40f, 60f, 40f, 60f);
            var topC = new Vector2(0.5f, 1f);
            var mid = new Vector2(0.5f, 0.5f);

            Image bookIcon = UIKit.Icon("BookIcon", card.transform, "book", 120f);
            UIKit.Place(bookIcon.rectTransform, topC, mid, new Vector2(-150f, -100f), new Vector2(120f, 120f));
            counter = UIKit.Label("Counter", card.transform, "", 60, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Place(counter.rectTransform, topC, new Vector2(0f, 0.5f), new Vector2(-70f, -100f), new Vector2(400f, 100f));
            Button close = UIKit.IconButton("Close", card.transform, "close", 120f, Art.Rose, Hide);
            UIKit.Place(close.GetComponent<RectTransform>(), new Vector2(1f, 1f), mid, new Vector2(-50f, -50f), new Vector2(120f, 120f));

            const int cols = 3;
            for (int i = 0; i < Catalog.Stickers.Length; i++)
            {
                int col = i % cols, row = i / cols;
                RectTransform cell = UIKit.Rect("Sticker_" + Catalog.Stickers[i].id, card.transform);
                UIKit.Place(cell, topC, mid, new Vector2((col - 1) * 310f, -330f - row * 262f), new Vector2(290f, 250f));
                Image badge = UIKit.Img("Badge", cell, Art.Icon("circle"), Color.white);
                UIKit.Place(badge.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 0f), new Vector2(190f, 190f));
                Image icon = UIKit.Icon("Icon", badge.transform, "question", 124f);
                Text title = UIKit.Label("Title", cell, "", 28, Art.Ink);
                UIKit.Place(title.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 0f), new Vector2(290f, 56f));
                title.resizeTextForBestFit = true; title.resizeTextMinSize = 16; title.resizeTextMaxSize = 28;
                badges.Add(badge); icons.Add(icon); titles.Add(title);
            }
            root.SetActive(false);
        }

        public void Show()
        {
            SaveData d = SaveSystem.Data;
            int have = 0;
            for (int i = 0; i < Catalog.Stickers.Length; i++)
            {
                StickerDef def = Catalog.Stickers[i];
                bool owned = d.stickers.Contains(def.id);
                if (owned) have++;
                badges[i].color = owned ? def.color : new Color(0.88f, 0.84f, 0.8f);
                icons[i].sprite = Art.Icon(owned ? def.icon : "question");
                icons[i].color = owned ? Color.white : new Color(1f, 1f, 1f, 0.55f);
                titles[i].text = owned ? def.title : "";
            }
            counter.text = have + " / " + Catalog.Stickers.Length;
            root.SetActive(true);
            AudioManager.Play("whoosh");
        }

        private void Hide()
        {
            AudioManager.Play("tap");
            root.SetActive(false);
        }
    }
}
