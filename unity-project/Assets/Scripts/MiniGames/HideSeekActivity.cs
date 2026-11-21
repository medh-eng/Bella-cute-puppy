using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Hide and Seek: Bella peeks out from behind one cushion, ducks down, the cushions shuffle,
    /// then you tap the one she is hiding behind. Three rounds with 3, 4 and 5 cushions.
    /// A wrong cushion just fades away; there is no penalty.
    /// </summary>
    public class HideSeekActivity : ArcadeActivity
    {
        private const int Rounds = 3;
        private static readonly Color[] Tints =
        {
            new Color(1f, 0.62f, 0.62f), new Color(0.62f, 0.8f, 1f), new Color(0.7f, 0.95f, 0.65f),
            new Color(1f, 0.85f, 0.5f), new Color(0.85f, 0.7f, 1f)
        };

        private readonly List<GameObject> scenery = new List<GameObject>();
        private readonly List<RectTransform> cushions = new List<RectTransform>();
        private readonly List<Vector2> targets = new List<Vector2>();
        private RectTransform bella;
        private int round, found, count, hideIdx, shufflesLeft, phase;
        private float size, timer;

        protected override string Tune { get { return "music_play"; } }

        protected override void OnBegin()
        {
            Image back = UIKit.Panel("Backdrop", UI, new Color(1f, 0.95f, 0.85f, 1f), false);
            UIKit.Stretch(back.rectTransform);
            back.transform.SetAsFirstSibling();
            BuildHud("Watch where Bella hides!", "paw");
            Pose("Sit");
            StartRound();
        }

        private void StartRound()
        {
            for (int i = 0; i < scenery.Count; i++) if (scenery[i] != null) Destroy(scenery[i]);
            scenery.Clear(); cushions.Clear(); targets.Clear();
            count = 3 + round;
            float spacing = 940f / count;
            size = Mathf.Min(250f, spacing - 24f);
            hideIdx = Random.Range(0, count);
            SetSub((round + 1) + " / " + Rounds);

            // Bella first, so every cushion is drawn on top of her.
            var go = new GameObject("Bella", typeof(RectTransform), typeof(RawImage));
            go.transform.SetParent(UI, false);
            RawImage ri = go.GetComponent<RawImage>();
            ri.texture = Art.Tex("Portraits/face_playful");
            ri.raycastTarget = false;
            bella = ri.rectTransform;
            UIKit.Place(bella, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(size, size));
            scenery.Add(go);

            for (int i = 0; i < count; i++)
            {
                int idx = i;
                Image c = UIKit.Panel("Cushion" + i, UI, Tints[i % Tints.Length]);
                Vector2 pos = new Vector2((i - (count - 1) * 0.5f) * spacing, 120f);
                UIKit.Place(c.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), pos, new Vector2(size, size * 0.8f));
                Image paw = UIKit.Icon("Paw", c.transform, "paw", size * 0.45f);
                paw.color = new Color(1f, 1f, 1f, 0.7f);
                Button b = c.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = c;
                b.onClick.AddListener(() => Pick(idx));
                cushions.Add(c.rectTransform);
                targets.Add(pos);
                scenery.Add(c.gameObject);
            }

            bella.anchoredPosition = targets[hideIdx] + new Vector2(0f, size * 0.55f);
            bella.gameObject.SetActive(true);
            phase = 0; timer = 1.5f; shufflesLeft = 2 + round * 2;
            SetHint("Bella is hiding...");
            AudioManager.Play("pop", 0.5f);
        }

        private void Update()
        {
            if (!Running || !Ready || Over) return;
            for (int i = 0; i < cushions.Count; i++)
                if (cushions[i] != null)
                    cushions[i].anchoredPosition = Vector2.Lerp(cushions[i].anchoredPosition, targets[i], 12f * Time.deltaTime);

            if (phase == 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f) { bella.gameObject.SetActive(false); phase = 1; timer = 0.4f; SetHint("Where did she go?"); }
            }
            else if (phase == 1)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (shufflesLeft <= 0) { phase = 2; SetHint("Tap the cushion Bella is hiding behind!"); return; }
                    shufflesLeft--;
                    int a = Random.Range(0, count), b = Random.Range(0, count - 1);
                    if (b >= a) b++;
                    Vector2 t = targets[a]; targets[a] = targets[b]; targets[b] = t;
                    AudioManager.Play("whoosh", 0.3f, Random.Range(0.9f, 1.3f));
                    timer = 0.55f;
                }
            }
        }

        private void Pick(int i)
        {
            if (!Running || Over || phase != 2) return;
            if (i == hideIdx)
            {
                phase = 3; found++;
                bella.anchoredPosition = cushions[i].anchoredPosition + new Vector2(0f, size * 0.55f);
                bella.gameObject.SetActive(true);
                UIKit.Pop(bella.gameObject);
                AudioManager.Play("chime", 0.9f);
                AudioManager.Voice("bark_happy");
                Cheer(8);
                SetScore(found.ToString());
                SetHint("Found her!");
                round++;
                if (round >= Rounds) After(1.6f, () => EndRound(2 + found * 2, "Found Bella " + found + " times!"));
                else After(1.8f, StartRound);
            }
            else
            {
                Image img = cushions[i].GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.35f);
                cushions[i].GetComponent<Button>().interactable = false;
                AudioManager.Play("pop", 0.4f, 0.8f);
                SetHint("Not here! Try another!");
            }
        }
    }
}
