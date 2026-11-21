using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Paw Memory: flip two cards at a time and find the matching pairs. Three boards:
    /// 3 pairs, 6 pairs, 8 pairs. Mismatches just flip back; there is no clock and no penalty.
    /// </summary>
    public class PawMemoryActivity : ArcadeActivity
    {
        private class Card { public Image bg; public Image face; public int sym; public bool up, matched; }

        private static readonly string[] Symbols = { "heart", "star", "bone", "toy_tennis", "treat", "butterfly", "sun", "moon" };
        private static readonly int[] Cols = { 3, 4, 4 };
        private static readonly int[] Rows = { 2, 3, 4 };

        private readonly List<Card> cards = new List<Card>();
        private readonly List<GameObject> scenery = new List<GameObject>();
        private Card first;
        private bool locked;
        private int level, moves, pairsLeft, solved;

        protected override string Tune { get { return "music_shop"; } }

        protected override void OnBegin()
        {
            Image back = UIKit.Panel("Backdrop", UI, new Color(0.93f, 0.97f, 1f, 1f), false);
            UIKit.Stretch(back.rectTransform);
            back.transform.SetAsFirstSibling();
            BuildHud("Find the matching pairs!", "paw");
            Pose("Sit");
            StartBoard();
        }

        private void StartBoard()
        {
            for (int i = 0; i < scenery.Count; i++) if (scenery[i] != null) Destroy(scenery[i]);
            scenery.Clear(); cards.Clear();
            first = null; locked = false; moves = 0;
            int cols = Cols[level], rows = Rows[level];
            int pairs = cols * rows / 2;
            pairsLeft = pairs;
            SetSub((level + 1) + " / " + Cols.Length);
            SetScore("0");

            var deck = new List<int>();
            for (int p = 0; p < pairs; p++) { deck.Add(p); deck.Add(p); }
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int t = deck[i]; deck[i] = deck[j]; deck[j] = t;
            }

            float cell = Mathf.Min(260f, 940f / cols - 20f);
            float gap = 20f;
            for (int i = 0; i < deck.Count; i++)
            {
                int r = i / cols, c = i % cols;
                Vector2 pos = new Vector2((c - (cols - 1) * 0.5f) * (cell + gap), 160f + ((rows - 1) * 0.5f - r) * (cell + gap));
                Image bg = UIKit.Panel("Card" + i, UI, new Color(0.62f, 0.8f, 1f));
                UIKit.Place(bg.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), pos, new Vector2(cell, cell));
                Image face = UIKit.Icon("Face", bg.transform, "paw", cell * 0.6f);
                face.color = new Color(1f, 1f, 1f, 0.6f);
                var card = new Card { bg = bg, face = face, sym = deck[i] };
                Button b = bg.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = bg;
                b.onClick.AddListener(() => Flip(card));
                cards.Add(card);
                scenery.Add(bg.gameObject);
            }
        }

        private void Show(Card c)
        {
            c.up = true;
            c.bg.color = Color.white;
            c.face.sprite = Art.Icon(Symbols[c.sym]);
            c.face.color = Color.white;
            UIKit.Pop(c.face.gameObject);
        }

        private void Hide(Card c)
        {
            c.up = false;
            c.bg.color = new Color(0.62f, 0.8f, 1f);
            c.face.sprite = Art.Icon("paw");
            c.face.color = new Color(1f, 1f, 1f, 0.6f);
        }

        private void Flip(Card c)
        {
            if (!Running || !Ready || Over || locked || c.up || c.matched) return;
            Show(c);
            AudioManager.Play("tap", 0.6f, Random.Range(0.95f, 1.15f));
            if (first == null) { first = c; return; }

            moves++;
            SetScore(moves.ToString());
            Card a = first; first = null;
            if (a.sym == c.sym)
            {
                a.matched = c.matched = true;
                pairsLeft--;
                AudioManager.Play("coin", 0.8f, Random.Range(1f, 1.25f));
                Cheer(3);
                if (pairsLeft == 0) BoardDone();
            }
            else
            {
                locked = true;
                After(0.8f, () => { Hide(a); Hide(c); locked = false; });
            }
        }

        private void BoardDone()
        {
            solved++;
            locked = true;
            AudioManager.Play("chime", 0.9f);
            AudioManager.Voice("bark_happy");
            Cheer(10);
            level++;
            if (level >= Cols.Length) { SetHint("All boards done!"); After(1.5f, () => EndRound(2 + solved * 2, solved + " boards matched!")); }
            else { SetHint("Great memory! Next board..."); After(1.8f, () => { if (!Over) { StartBoard(); SetHint("Find the matching pairs!"); } }); }
        }
    }
}
