using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Jigsaw: drag the pieces from the tray onto the picture. Three puzzles a visit — 2x2, 3x3, 4x4 —
    /// cut from Bella's own places. A piece dropped close to its home snaps in; otherwise it simply waits.
    /// </summary>
    public class JigsawActivity : ArcadeActivity
    {
        private class Piece { public RectTransform rt; public int slot; public bool placed; }

        private static readonly string[] Pics = { "park", "garden", "kitchen", "beach", "snow", "stage" };
        private static readonly int[] Sizes = { 2, 3, 4 };
        private const float BoardSize = 720f;
        private static readonly Vector2 BoardCentre = new Vector2(0f, 300f);
        private const float TrayScale = 0.6f;

        private readonly List<Piece> pieces = new List<Piece>();
        private readonly List<GameObject> scenery = new List<GameObject>();
        private Piece held;
        private Vector2 grab;
        private int level, n, placedCount, solved;
        private float cell;
        private string pic;

        protected override string Tune { get { return "music_shop"; } }

        protected override void OnBegin()
        {
            BuildHud("Drag the pieces onto the picture!", "game_puzzle");
            Pose("Sit");
            StartPuzzle();
        }

        private void StartPuzzle()
        {
            for (int i = 0; i < scenery.Count; i++) if (scenery[i] != null) Destroy(scenery[i]);
            scenery.Clear(); pieces.Clear();
            n = Sizes[Mathf.Min(level, Sizes.Length - 1)];
            cell = BoardSize / n;
            placedCount = 0; held = null;
            pic = Pics[Random.Range(0, Pics.Length)];
            Texture2D tex = Art.Tex("Places/" + pic);
            float aspect = tex != null && tex.height > 0 ? (float)tex.width / tex.height : 1f;
            float uw = aspect > 1f ? 1f / aspect : 1f, uh = aspect > 1f ? 1f : aspect;
            float x0 = (1f - uw) * 0.5f, y0 = (1f - uh) * 0.5f;
            SetSub((level + 1) + " / " + Sizes.Length);

            // frame + faint guide picture
            Image frame = UIKit.Panel("Frame", UI, new Color(1f, 0.97f, 0.9f, 0.95f), false);
            UIKit.Place(frame.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), BoardCentre, new Vector2(BoardSize + 40f, BoardSize + 40f));
            scenery.Add(frame.gameObject);
            RawImage guide = NewRaw("Guide", tex, new Rect(x0, y0, uw, uh));
            UIKit.Place(guide.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), BoardCentre, new Vector2(BoardSize, BoardSize));
            guide.color = new Color(1f, 1f, 1f, 0.22f);
            scenery.Add(guide.gameObject);

            for (int s = 0; s < n * n; s++)
            {
                int r = s / n, c = s % n;
                var rect = new Rect(x0 + c * uw / n, y0 + (n - 1 - r) * uh / n, uw / n, uh / n);
                RawImage ri = NewRaw("Piece" + s, tex, rect);
                var p = new Piece { rt = ri.rectTransform, slot = s };
                UIKit.Place(p.rt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(Random.Range(-360f, 360f), Random.Range(-780f, -420f)), new Vector2(cell - 3f, cell - 3f));
                p.rt.localScale = Vector3.one * TrayScale;
                pieces.Add(p);
                scenery.Add(ri.gameObject);
            }
        }

        private RawImage NewRaw(string name, Texture tex, Rect uv)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(RawImage));
            go.transform.SetParent(UI, false);
            RawImage ri = go.GetComponent<RawImage>();
            ri.texture = tex; ri.uvRect = uv; ri.raycastTarget = false;
            return ri;
        }

        private Vector2 SlotCentre(int s)
        {
            int r = s / n, c = s % n;
            return BoardCentre + new Vector2((c + 0.5f - n * 0.5f) * cell, (n * 0.5f - r - 0.5f) * cell);
        }

        private void Update()
        {
            if (!Running || !Ready || Over) return;
            if (Pointer.Down && !Pointer.OverUI)
            {
                Vector2 pt = PointerLocal();
                for (int i = pieces.Count - 1; i >= 0; i--)
                {
                    Piece p = pieces[i];
                    if (p.placed) continue;
                    float half = cell * 0.5f * p.rt.localScale.x;
                    Vector2 d = pt - p.rt.anchoredPosition;
                    if (Mathf.Abs(d.x) < half && Mathf.Abs(d.y) < half)
                    {
                        held = p; grab = p.rt.anchoredPosition - pt;
                        p.rt.localScale = Vector3.one;
                        p.rt.SetAsLastSibling();
                        AudioManager.Play("tap", 0.5f);
                        break;
                    }
                }
            }
            if (held != null && Pointer.Held) held.rt.anchoredPosition = PointerLocal() + grab;
            if (held != null && Pointer.Up) Drop();
        }

        private void Drop()
        {
            Piece p = held; held = null;
            Vector2 target = SlotCentre(p.slot);
            if ((p.rt.anchoredPosition - target).magnitude < cell * 0.5f)
            {
                p.placed = true;
                p.rt.anchoredPosition = target;
                p.rt.localScale = Vector3.one;
                placedCount++;
                AudioManager.Play("coin", 0.8f, Random.Range(0.95f, 1.2f));
                Cheer(2);
                if (placedCount == n * n) Solved();
            }
            else
            {
                p.rt.localScale = Vector3.one * TrayScale;
                AudioManager.Play("pop", 0.35f, 0.8f);
            }
        }

        private void Solved()
        {
            solved++;
            AudioManager.Play("chime", 0.9f);
            AudioManager.Voice("bark_happy");
            Cheer(10);
            SetHint(solved < Sizes.Length ? "Puzzle done! Next one..." : "All puzzles done!");
            level++;
            if (level >= Sizes.Length) After(1.5f, () => EndRound(2 + solved * 2, solved + " puzzles solved!"));
            else After(1.8f, () => { if (!Over) { StartPuzzle(); SetHint("Drag the pieces onto the picture!"); } });
        }
    }
}
