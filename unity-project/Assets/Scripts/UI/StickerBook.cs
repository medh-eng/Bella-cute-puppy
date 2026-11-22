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

            // 12 stickers a page (3 x 4) so the book fits a 1080 x 1920 canvas whatever the count.
            const int cols = 3;
            for (int i = 0; i < PerPage; i++)
            {
                int col = i % cols, row = i / cols;
                RectTransform cell = UIKit.Rect("Sticker_" + i, card.transform);
                UIKit.Place(cell, topC, mid, new Vector2((col - 1) * 310f, -330f - row * 300f), new Vector2(290f, 280f));
                Image badge = UIKit.Img("Badge", cell, Art.Icon("circle"), Color.white);
                UIKit.Place(badge.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 0f), new Vector2(200f, 200f));
                Image icon = UIKit.Icon("Icon", badge.transform, "question", 124f);
                Text title = UIKit.Label("Title", cell, "", 28, Art.Ink);
                UIKit.Place(title.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 0f), new Vector2(290f, 66f));
                title.resizeTextForBestFit = true; title.resizeTextMinSize = 16; title.resizeTextMaxSize = 28;
                cells.Add(cell.gameObject); badges.Add(badge); icons.Add(icon); titles.Add(title);
            }

            var botC = new Vector2(0.5f, 0f);
            prev = UIKit.IconButton("Prev", card.transform, "back", 130f, Art.Teal, () => Turn(-1));
            UIKit.Place(prev.GetComponent<RectTransform>(), botC, mid, new Vector2(-300f, 110f), new Vector2(130f, 130f));
            next = UIKit.IconButton("Next", card.transform, "back", 130f, Art.Teal, () => Turn(1));
            UIKit.Place(next.GetComponent<RectTransform>(), botC, mid, new Vector2(300f, 110f), new Vector2(130f, 130f));
            next.transform.localScale = new Vector3(-1f, 1f, 1f);      // the back arrow, mirrored
            pageLabel = UIKit.Label("Page", card.transform, "", 40, Art.Ink);
            UIKit.Place(pageLabel.rectTransform, botC, mid, new Vector2(0f, 110f), new Vector2(360f, 80f));
            root.SetActive(false);
        }

        private const int PerPage = 12;
        private readonly List<GameObject> cells = new List<GameObject>();
        private Button prev, next;
        private Text pageLabel;
        private int page;

        private static int PageCount => Mathf.Max(1, (Catalog.Stickers.Length + PerPage - 1) / PerPage);

        public void Show()
        {
            SaveData d = SaveSystem.Data;
            int have = 0;
            for (int i = 0; i < Catalog.Stickers.Length; i++)
                if (d.stickers.Contains(Catalog.Stickers[i].id)) have++;
            counter.text = have + " / " + Catalog.Stickers.Length;
            page = 0;
            Fill();
            root.SetActive(true);
            AudioManager.Play("whoosh");
        }

        private void Turn(int dir)
        {
            int p = Mathf.Clamp(page + dir, 0, PageCount - 1);
            if (p == page) return;
            page = p;
            AudioManager.Play("tap");
            Fill();
        }

        private void Fill()
        {
            SaveData d = SaveSystem.Data;
            for (int i = 0; i < PerPage; i++)
            {
                int k = page * PerPage + i;
                bool exists = k < Catalog.Stickers.Length;
                cells[i].SetActive(exists);
                if (!exists) continue;
                StickerDef def = Catalog.Stickers[k];
                bool owned = d.stickers.Contains(def.id);
                Sprite art = owned ? Art.TrySprite("Stickers/" + def.id) : null;
                badges[i].color = owned ? (art != null ? Color.white : def.color) : new Color(0.88f, 0.84f, 0.8f);
                badges[i].sprite = art != null ? art : Art.Icon("circle");
                icons[i].gameObject.SetActive(art == null);           // the sticker picture already has its own badge
                icons[i].sprite = Art.Icon(owned ? def.icon : "question");
                icons[i].color = owned ? Color.white : new Color(1f, 1f, 1f, 0.55f);
                titles[i].text = owned ? def.title : "";
            }
            pageLabel.text = (page + 1) + " / " + PageCount;
            prev.interactable = page > 0;
            next.interactable = page < PageCount - 1;
        }

        private void Hide()
        {
            AudioManager.Play("tap");
            root.SetActive(false);
        }
    }
}
