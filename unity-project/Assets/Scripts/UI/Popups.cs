using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;

namespace Bella.UI
{
    /// <summary>
    /// Queued modal cards: morning greeting, daily gift, sticker unlocked, growth
    /// milestones. One big picture, one short line, one big button — readable by a
    /// parent, tappable by a four-year-old.
    /// </summary>
    public class Popups : MonoBehaviour
    {
        private class Request
        {
            public string picture;      // Resources path of the picture (icon or portrait)
            public Color badge;
            public string title, body, sound;
            public Action onClose;
        }

        private readonly Queue<Request> queue = new Queue<Request>();
        private Canvas canvas;
        private GameObject root;
        private Image picture, badge;
        private Text title, body;
        private Request current;

        public bool IsOpen => current != null;

        public void Init()
        {
            canvas = UIKit.CreateCanvas("PopupCanvas", 50);
            canvas.transform.SetParent(transform, false);
            root = UIKit.Rect("PopupRoot", canvas.transform).gameObject;
            UIKit.Stretch(root.GetComponent<RectTransform>());
            UIKit.Dim(root.transform);

            Image card = UIKit.Panel("Card", root.transform, Art.Cream);
            UIKit.Place(card.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 40f), new Vector2(860f, 1040f));

            badge = UIKit.Img("Badge", card.transform, Art.Icon("circle"), Art.Teal);
            UIKit.Place(badge.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -300f), new Vector2(460f, 460f));
            picture = UIKit.Img("Picture", badge.transform, null, Color.white);
            picture.preserveAspect = true;
            UIKit.Place(picture.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(330f, 330f));

            title = UIKit.Label("Title", card.transform, "", 64, Art.Ink);
            UIKit.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -610f), new Vector2(760f, 150f));
            title.resizeTextForBestFit = true; title.resizeTextMinSize = 30; title.resizeTextMaxSize = 64;

            body = UIKit.Label("Body", card.transform, "", 40, new Color(0.45f, 0.35f, 0.3f));
            UIKit.Place(body.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -745f), new Vector2(740f, 130f));
            body.resizeTextForBestFit = true; body.resizeTextMinSize = 24; body.resizeTextMaxSize = 40;

            Button ok = UIKit.IconButton("Ok", card.transform, "check", 190f, Art.Leaf, Close);
            UIKit.Place(ok.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 110f), new Vector2(190f, 190f));

            root.SetActive(false);
        }

        public void Show(string picturePath, Color badgeColor, string titleText, string bodyText, string sound = "success", Action onClose = null)
        {
            queue.Enqueue(new Request
            {
                picture = picturePath, badge = badgeColor, title = titleText, body = bodyText, sound = sound, onClose = onClose
            });
            if (current == null) Next();
        }

        private void Next()
        {
            if (queue.Count == 0)
            {
                current = null;
                root.SetActive(false);
                return;
            }
            current = queue.Dequeue();
            picture.sprite = Art.SpriteAt(current.picture);
            bool portrait = current.picture.StartsWith("Portraits/");
            picture.rectTransform.sizeDelta = portrait ? new Vector2(420f, 420f) : new Vector2(300f, 300f);
            badge.color = current.badge;
            title.text = current.title;
            body.text = current.body;
            root.SetActive(true);
            UIKit.Pop(badge.gameObject);
            if (!string.IsNullOrEmpty(current.sound)) AudioManager.Play(current.sound);
        }

        private void Close()
        {
            if (current == null) return;
            AudioManager.Play("tap");
            Action cb = current.onClose;
            current = null;
            Next();
            if (cb != null) cb();
        }
    }
}
