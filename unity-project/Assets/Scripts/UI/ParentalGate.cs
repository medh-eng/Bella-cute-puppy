using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;

namespace Bella.UI
{
    /// <summary>
    /// Parental gate (docs/GDD.md §7, ARCHITECTURE.md §9): a two-number sum typed on
    /// a keypad. New numbers every attempt, no hints, a wrong answer simply closes.
    /// It guards the Parents' area (settings, privacy text, reset).
    /// </summary>
    public class ParentalGate : MonoBehaviour
    {
        private Canvas canvas;
        private GameObject root;
        private Text question, entry;
        private int answer;
        private string typed = "";
        private Action onPass;

        public void Init()
        {
            canvas = UIKit.CreateCanvas("GateCanvas", 60);
            canvas.transform.SetParent(transform, false);
            root = UIKit.Rect("GateRoot", canvas.transform).gameObject;
            UIKit.Stretch(root.GetComponent<RectTransform>());
            UIKit.Dim(root.transform, 0.7f);

            Image card = UIKit.Panel("Card", root.transform, Art.Cream);
            UIKit.Place(card.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(820f, 1280f));
            var topC = new Vector2(0.5f, 1f);
            var mid = new Vector2(0.5f, 0.5f);

            Text head = UIKit.Label("Head", card.transform, "Grown-ups only", 60, Art.Ink);
            UIKit.Place(head.rectTransform, topC, mid, new Vector2(0f, -100f), new Vector2(720f, 90f));
            Text sub = UIKit.Label("Sub", card.transform, "Please type the answer to continue.", 34, new Color(0.45f, 0.35f, 0.3f));
            UIKit.Place(sub.rectTransform, topC, mid, new Vector2(0f, -180f), new Vector2(720f, 60f));
            question = UIKit.Label("Question", card.transform, "", 84, Art.Ink);
            UIKit.Place(question.rectTransform, topC, mid, new Vector2(0f, -290f), new Vector2(720f, 110f));

            Image entryBack = UIKit.Panel("Entry", card.transform, Color.white, false);
            UIKit.Place(entryBack.rectTransform, topC, mid, new Vector2(0f, -420f), new Vector2(360f, 110f));
            entry = UIKit.Label("EntryText", entryBack.transform, "", 70, Art.Ink);
            UIKit.Stretch(entry.rectTransform);

            string[] keys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "C", "0", "OK" };
            for (int i = 0; i < keys.Length; i++)
            {
                string k = keys[i];
                int col = i % 3, row = i / 3;
                Color c = k == "OK" ? Art.Leaf : k == "C" ? Art.Rose : Art.Teal;
                Button b = UIKit.TextButton("Key_" + k, card.transform, k, new Vector2(200f, 140f), c, () => Press(k), 60);
                UIKit.Place(b.GetComponent<RectTransform>(), topC, mid, new Vector2((col - 1) * 220f, -580f - row * 158f), new Vector2(200f, 140f));
            }

            Button close = UIKit.IconButton("Close", card.transform, "close", 110f, Art.Rose, Hide);
            UIKit.Place(close.GetComponent<RectTransform>(), new Vector2(1f, 1f), mid, new Vector2(-40f, -40f), new Vector2(110f, 110f));
            root.SetActive(false);
        }

        public void Ask(Action passed)
        {
            onPass = passed;
            int a = UnityEngine.Random.Range(12, 40);
            int b = UnityEngine.Random.Range(11, 40);
            answer = a + b;
            typed = "";
            question.text = a + " + " + b + " = ?";
            entry.text = "";
            root.SetActive(true);
        }

        private void Press(string key)
        {
            AudioManager.Play("tap");
            if (key == "C") { typed = ""; }
            else if (key == "OK")
            {
                int value;
                bool ok = int.TryParse(typed, out value) && value == answer;
                Action cb = onPass;
                Hide();
                if (ok && cb != null) cb();
                return;
            }
            else if (typed.Length < 3) { typed += key; }
            entry.text = typed;
        }

        private void Hide()
        {
            onPass = null;
            root.SetActive(false);
        }
    }
}
