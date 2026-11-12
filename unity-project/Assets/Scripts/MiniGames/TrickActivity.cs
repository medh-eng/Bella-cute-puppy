using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.Systems;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Trick Training — the game's differentiator (docs/GDD.md §5). Draw a shape
    /// anywhere on the screen and Bella performs the matching trick. Three
    /// successful tries and she has "learned" it (a filled row of stars + a
    /// sticker). The cards along the top are the picture-only instructions.
    /// </summary>
    public class TrickActivity : Activity
    {
        private readonly List<Vector2> stroke = new List<Vector2>();
        private readonly List<Image> dots = new List<Image>();
        private readonly Dictionary<string, Image[]> pips = new Dictionary<string, Image[]>();
        private readonly Dictionary<string, Image> cards = new Dictionary<string, Image>();
        private int dotIndex;
        private bool drawing, performing;
        private float trailFade;
        private Image hintIcon;

        protected override void OnBegin()
        {
            // trick cards: gesture picture + three stars
            for (int i = 0; i < Catalog.Tricks.Length; i++)
            {
                TrickDef def = Catalog.Tricks[i];
                int col = i % 3, row = i / 3;
                Image card = UIKit.Panel("Card_" + def.id, UI, new Color(1f, 1f, 1f, 0.9f), false);
                UIKit.Place(card.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2(110f + (col - 1) * 250f, -130f - row * 215f), new Vector2(230f, 195f));
                Image glyph = UIKit.Icon("Glyph", card.transform, "gesture_" + def.gesture, 112f);
                glyph.rectTransform.anchoredPosition = new Vector2(0f, 26f);
                var row3 = new Image[Catalog.TricksToLearn];
                for (int p = 0; p < row3.Length; p++)
                {
                    row3[p] = UIKit.Icon("Pip" + p, card.transform, "star", 46f);
                    row3[p].rectTransform.anchoredPosition = new Vector2((p - 1) * 52f, -64f);
                }
                pips[def.id] = row3;
                cards[def.id] = card;
            }

            // finger-trail dots
            for (int i = 0; i < 70; i++)
            {
                Image d = UIKit.Img("Dot", UI, Art.Icon("circle"), new Color(1f, 0.85f, 0.3f, 0.95f));
                UIKit.Place(d.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(34f, 34f));
                d.gameObject.SetActive(false);
                dots.Add(d);
            }

            // the "draw here" hint: a big faded glyph of the next trick to learn
            hintIcon = UIKit.Icon("Hint", UI, "gesture_down", 300f);
            UIKit.Place(hintIcon.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 330f), new Vector2(300f, 300f));
            hintIcon.color = new Color(1f, 1f, 1f, 0.55f);
            hintIcon.gameObject.AddComponent<UIPulse>();

            RefreshCards();
            G.CameraRig.Focus(G.Room.HomePoint, 2.5f, 0.75f);
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Idle", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Excited);
                AudioManager.Voice("yip");
            });
        }

        private void RefreshCards()
        {
            SaveData d = SaveSystem.Data;
            string next = "";
            for (int i = 0; i < Catalog.Tricks.Length; i++)
            {
                TrickDef def = Catalog.Tricks[i];
                int count = Mathf.Min(Catalog.TricksToLearn, d.GetTrickCount(def.id));
                bool learned = d.learnedTricks.Contains(def.id);
                Image[] row = pips[def.id];
                for (int p = 0; p < row.Length; p++)
                    row[p].color = p < count ? Color.white : new Color(0.3f, 0.25f, 0.2f, 0.22f);
                cards[def.id].color = learned ? new Color(1f, 0.95f, 0.7f, 0.95f) : new Color(1f, 1f, 1f, 0.9f);
                if (next == "" && !learned) next = def.gesture;
            }
            if (next == "") next = Catalog.Tricks[Random.Range(0, Catalog.Tricks.Length)].gesture;
            hintIcon.sprite = Art.Icon("gesture_" + next);
        }

        private void Update()
        {
            // fade the trail after the finger lifts
            if (!drawing && trailFade > 0f)
            {
                trailFade -= Time.deltaTime * 2.5f;
                float a = Mathf.Clamp01(trailFade);
                for (int i = 0; i < dots.Count; i++)
                {
                    if (!dots[i].gameObject.activeSelf) continue;
                    Color c = dots[i].color; c.a = a * 0.95f; dots[i].color = c;
                    if (a <= 0f) dots[i].gameObject.SetActive(false);
                }
            }

            if (!Ready || Ending || performing) return;

            if (Pointer.Down && !Pointer.OverUI)
            {
                drawing = true;
                stroke.Clear();
                dotIndex = 0;
                for (int i = 0; i < dots.Count; i++) dots[i].gameObject.SetActive(false);
                hintIcon.gameObject.SetActive(false);
            }

            if (drawing && Pointer.Held)
            {
                Vector2 p = Pointer.Position;
                if (stroke.Count == 0 || (stroke[stroke.Count - 1] - p).magnitude > Screen.height * 0.012f)
                {
                    stroke.Add(p);
                    Image d = dots[dotIndex % dots.Count];
                    dotIndex++;
                    d.gameObject.SetActive(true);
                    d.color = new Color(1f, 0.85f, 0.3f, 0.95f);
                    d.rectTransform.anchoredPosition = PointerLocal();
                    Bella.Puppy.LookAt(G.Cam.ScreenToWorldPoint(new Vector3(p.x, p.y, 3.2f)));
                }
            }

            if (drawing && Pointer.Up)
            {
                drawing = false;
                trailFade = 1f;
                Bella.Puppy.ClearLook();
                Judge(GestureRecognizer.Recognize(stroke));
            }
        }

        private void Judge(string gesture)
        {
            TrickDef def = gesture == "" ? null : Catalog.FindTrickByGesture(gesture);
            if (def == null)
            {
                // Not a failure — just a curious puppy.
                AudioManager.Play("oops", 0.5f);
                Bella.Face.Flash(Expression.Begging, 1.2f);
                hintIcon.gameObject.SetActive(true);
                return;
            }

            performing = true;
            AudioManager.Voice("bark");
            Bella.PerformTrick(def, () =>
            {
                performing = false;
                Reward(def);
                Bella.PlayPose("Idle", Expression.Excited);
                hintIcon.gameObject.SetActive(true);
            });
        }

        private void Reward(TrickDef def)
        {
            SaveData d = SaveSystem.Data;
            d.countTricks++;
            int count = d.AddTrickCount(def.id);
            Bella.Needs.AddHappiness(6f);
            Bella.Needs.Tire(1.5f);

            bool justLearned = count >= Catalog.TricksToLearn && !d.learnedTricks.Contains(def.id);
            if (justLearned)
            {
                d.learnedTricks.Add(def.id);
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.3f, 18);
                AudioManager.Play("success");
                Bella.Celebrate(10);
                UIKit.Pop(cards[def.id].gameObject);
            }
            else
            {
                // Practice pays from the shared daily play allowance, then tricks are just for fun.
                int pay = Progress.PlayTreats(2);
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.3f, 6);
                Bella.Celebrate(pay);
            }
            SaveSystem.Flush();
            RefreshCards();
        }
    }
}
