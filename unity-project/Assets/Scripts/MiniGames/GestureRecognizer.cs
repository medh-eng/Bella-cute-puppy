using System.Collections.Generic;
using UnityEngine;

namespace Bella.MiniGames
{
    /// <summary>
    /// Tiny, forgiving shape recogniser for Trick Training. Children draw wobbly
    /// shapes, so every test uses generous thresholds and nothing is "wrong" —
    /// an unrecognised doodle just gets a curious head-tilt.
    ///
    /// Returns: "up", "down", "left", "right", "circle", "zigzag" or "" (unknown).
    /// </summary>
    public static class GestureRecognizer
    {
        public static string Recognize(List<Vector2> raw)
        {
            if (raw == null || raw.Count < 4) return "";

            // 1. Thin the stroke so jitter between frames does not look like turns.
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            for (int i = 0; i < raw.Count; i++)
            {
                minX = Mathf.Min(minX, raw[i].x); maxX = Mathf.Max(maxX, raw[i].x);
                minY = Mathf.Min(minY, raw[i].y); maxY = Mathf.Max(maxY, raw[i].y);
            }
            float w = maxX - minX, h = maxY - minY;
            float diag = Mathf.Sqrt(w * w + h * h);
            float minSize = Mathf.Max(Screen.height, Screen.width) * 0.07f;
            if (diag < minSize) return "";                      // just a tap

            float spacing = diag * 0.06f;
            var pts = new List<Vector2> { raw[0] };
            for (int i = 1; i < raw.Count; i++)
                if ((raw[i] - pts[pts.Count - 1]).magnitude >= spacing) pts.Add(raw[i]);
            if ((raw[raw.Count - 1] - pts[pts.Count - 1]).magnitude > spacing * 0.3f) pts.Add(raw[raw.Count - 1]);
            if (pts.Count < 3) pts.Add(raw[raw.Count - 1]);

            float length = 0f;
            for (int i = 1; i < pts.Count; i++) length += (pts[i] - pts[i - 1]).magnitude;
            if (length < 1f) return "";
            Vector2 net = pts[pts.Count - 1] - pts[0];
            float straightness = net.magnitude / length;

            // 2. A fairly straight stroke is a direction — unless it clearly swings from side to
            //    side (a shallow three-stroke zigzag is "fairly straight" too).
            if (straightness > 0.78f &&
                Mathf.Max(Reversals(pts, true, diag * 0.15f), Reversals(pts, false, diag * 0.15f)) < 2)
            {
                if (Mathf.Abs(net.x) > Mathf.Abs(net.y)) return net.x > 0f ? "right" : "left";
                return net.y > 0f ? "up" : "down";
            }

            // 3. Signed turning: a circle keeps turning the same way.
            float turning = 0f;
            for (int i = 2; i < pts.Count; i++)
            {
                Vector2 a = pts[i - 1] - pts[i - 2];
                Vector2 b = pts[i] - pts[i - 1];
                turning += Vector2.SignedAngle(a, b);
            }
            bool roundish = w > diag * 0.3f && h > diag * 0.3f;
            if (Mathf.Abs(turning) > 230f && roundish && net.magnitude < diag * 0.6f) return "circle";

            // 4. Zigzag: the stroke reverses direction at least twice along one axis.
            int rx = Reversals(pts, true, diag * 0.1f);
            int ry = Reversals(pts, false, diag * 0.1f);
            if (Mathf.Max(rx, ry) >= 2) return "zigzag";

            // 5. A bent-but-mostly-straight line still counts as a direction.
            if (straightness > 0.55f)
            {
                if (Mathf.Abs(net.x) > Mathf.Abs(net.y)) return net.x > 0f ? "right" : "left";
                return net.y > 0f ? "up" : "down";
            }
            return "";
        }

        private static int Reversals(List<Vector2> pts, bool xAxis, float threshold)
        {
            int reversals = 0;
            int dir = 0;
            float extreme = xAxis ? pts[0].x : pts[0].y;
            for (int i = 1; i < pts.Count; i++)
            {
                float v = xAxis ? pts[i].x : pts[i].y;
                if (dir == 0)
                {
                    if (Mathf.Abs(v - extreme) > threshold) dir = v > extreme ? 1 : -1;
                    if (dir != 0) extreme = v;
                }
                else if (dir == 1)
                {
                    if (v > extreme) extreme = v;
                    else if (extreme - v > threshold) { dir = -1; extreme = v; reversals++; }
                }
                else
                {
                    if (v < extreme) extreme = v;
                    else if (v - extreme > threshold) { dir = 1; extreme = v; reversals++; }
                }
            }
            return reversals;
        }
    }
}
