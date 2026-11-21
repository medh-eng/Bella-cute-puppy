using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Block Drop: falling coloured blocks, like the classic. Four big buttons — left, turn, drop, right.
    /// Full rows vanish for points. If the stack reaches the top the board just clears and play goes on.
    /// A round lasts 90 seconds.
    /// </summary>
    public class BlockDropActivity : ArcadeActivity
    {
        private const int Cols = 8, Rows = 14;
        private const float Cell = 88f, RoundSeconds = 90f;

        // each shape: four (x, y) offsets
        private static readonly int[][] Shapes =
        {
            new[] {0,0, 1,0, 2,0, 3,0},   // line
            new[] {0,0, 1,0, 0,1, 1,1},   // square
            new[] {0,0, 1,0, 2,0, 1,1},   // T
            new[] {0,0, 1,0, 2,0, 0,1},   // L
            new[] {0,0, 1,0, 2,0, 2,1},   // J
            new[] {1,0, 2,0, 0,1, 1,1},   // S
            new[] {0,0, 1,0, 1,1, 2,1},   // Z
        };
        private static readonly Color[] Palette =
        {
            new Color(0.35f, 0.75f, 0.92f), new Color(1.00f, 0.80f, 0.30f), new Color(0.70f, 0.50f, 0.92f),
            new Color(1.00f, 0.62f, 0.28f), new Color(0.38f, 0.58f, 0.95f), new Color(0.45f, 0.80f, 0.45f),
            new Color(0.96f, 0.40f, 0.45f),
        };

        private readonly int[,] grid = new int[Rows, Cols];          // 0 = empty, else palette index + 1
        private Image[,] cells;
        private readonly int[] ox = new int[4], oy = new int[4];
        private int px, py, colour;
        private float timeLeft = RoundSeconds, fallTimer, elapsed;
        private int score, lines;

        protected override string Tune { get { return "music_play"; } }

        protected override void OnBegin()
        {
            BuildHud("Fill rows to clear them!", "game_blocks");
            Image board = UIKit.Panel("Board", UI, new Color(1f, 0.97f, 0.9f, 0.62f), false);
            UIKit.Place(board.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -340f),
                new Vector2(Cols * Cell + 16f, Rows * Cell + 16f));
            cells = new Image[Rows, Cols];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                {
                    Image im = UIKit.Img("C", board.transform, null, Color.white, false);
                    UIKit.Place(im.rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(8f + c * Cell + 2f, 8f + r * Cell + 2f), new Vector2(Cell - 4f, Cell - 4f));
                    cells[r, c] = im;
                }

            AddButton("gesture_left", -375f, () => Move(-1));
            AddButton("gesture_circle", -125f, Turn);
            AddButton("gesture_down", 125f, Drop);
            AddButton("gesture_right", 375f, () => Move(1));

            Pose("Sit");
            Spawn();
            Redraw();
        }

        private void AddButton(string icon, float x, UnityEngine.Events.UnityAction act)
        {
            Button b = UIKit.IconButton("Btn_" + icon, UI, icon, 190f, Art.Cream, act);
            UIKit.Place(b.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(x, 170f), new Vector2(190f, 190f));
        }

        private void Update()
        {
            if (!Running || !Ready || Over) return;
            float dt = Time.deltaTime;
            elapsed += dt;
            timeLeft -= dt;
            SetSub(Mathf.Max(0, Mathf.CeilToInt(timeLeft)).ToString());
            if (timeLeft <= 0f)
            {
                EndRound(2 + score / 6, score + " points, " + lines + " rows!");
                return;
            }
            fallTimer += dt;
            if (fallTimer >= Mathf.Max(0.35f, 0.95f - elapsed * 0.008f))
            {
                fallTimer = 0f;
                if (Fits(ox, oy, px, py - 1)) { py--; Redraw(); }
                else Lock();
            }
        }

        private bool Fits(int[] xs, int[] ys, int x, int y)
        {
            for (int i = 0; i < 4; i++)
            {
                int gx = x + xs[i], gy = y + ys[i];
                if (gx < 0 || gx >= Cols || gy < 0) return false;
                if (gy < Rows && grid[gy, gx] != 0) return false;
            }
            return true;
        }

        private void Spawn()
        {
            int s = Random.Range(0, Shapes.Length);
            for (int i = 0; i < 4; i++) { ox[i] = Shapes[s][i * 2]; oy[i] = Shapes[s][i * 2 + 1]; }
            colour = s;
            px = Cols / 2 - 1; py = Rows - 2;
            if (!Fits(ox, oy, px, py))
            {
                // board full: clear it and carry on
                for (int r = 0; r < Rows; r++) for (int c = 0; c < Cols; c++) grid[r, c] = 0;
                AudioManager.Play("whoosh", 0.6f);
                SetHint("Fresh board — keep going!");
                Cheer(3);
            }
        }

        private void Move(int dx)
        {
            if (!Ready || Over) return;
            if (Fits(ox, oy, px + dx, py)) { px += dx; AudioManager.Play("tap", 0.4f); Redraw(); }
        }

        private void Turn()
        {
            if (!Ready || Over) return;
            int[] nx = new int[4], ny = new int[4];
            int minX = 99, minY = 99;
            for (int i = 0; i < 4; i++) { nx[i] = -oy[i]; ny[i] = ox[i]; minX = Mathf.Min(minX, nx[i]); minY = Mathf.Min(minY, ny[i]); }
            for (int i = 0; i < 4; i++) { nx[i] -= minX; ny[i] -= minY; }
            int[] kicks = { 0, -1, 1, -2, 2 };
            for (int k = 0; k < kicks.Length; k++)
                if (Fits(nx, ny, px + kicks[k], py))
                {
                    for (int i = 0; i < 4; i++) { ox[i] = nx[i]; oy[i] = ny[i]; }
                    px += kicks[k];
                    AudioManager.Play("pop", 0.45f, 1.2f);
                    Redraw();
                    return;
                }
        }

        private void Drop()
        {
            if (!Ready || Over) return;
            while (Fits(ox, oy, px, py - 1)) py--;
            AudioManager.Play("tap", 0.7f, 0.8f);
            Lock();
        }

        private void Lock()
        {
            for (int i = 0; i < 4; i++)
            {
                int gx = px + ox[i], gy = py + oy[i];
                if (gy >= 0 && gy < Rows && gx >= 0 && gx < Cols) grid[gy, gx] = colour + 1;
            }
            score += 1;
            int cleared = 0;
            for (int r = 0; r < Rows; r++)
            {
                bool full = true;
                for (int c = 0; c < Cols; c++) if (grid[r, c] == 0) { full = false; break; }
                if (!full) continue;
                for (int rr = r; rr < Rows - 1; rr++) for (int c = 0; c < Cols; c++) grid[rr, c] = grid[rr + 1, c];
                for (int c = 0; c < Cols; c++) grid[Rows - 1, c] = 0;
                cleared++; r--;
            }
            if (cleared > 0)
            {
                lines += cleared;
                score += cleared * 5 + (cleared - 1) * 3;
                AudioManager.Play(cleared > 1 ? "chime" : "coin", 0.85f);
                AudioManager.Voice("yip");
                Cheer(4 + cleared * 2);
                SetHint(cleared > 1 ? cleared + " rows at once!" : "Row cleared!");
            }
            SetScore(score.ToString());
            fallTimer = 0f;
            Spawn();
            Redraw();
        }

        private void Redraw()
        {
            Color empty = new Color(1f, 1f, 1f, 0.28f);
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    cells[r, c].color = grid[r, c] == 0 ? empty : Palette[(grid[r, c] - 1) % Palette.Length];
            for (int i = 0; i < 4; i++)
            {
                int gx = px + ox[i], gy = py + oy[i];
                if (gx >= 0 && gx < Cols && gy >= 0 && gy < Rows) cells[gy, gx].color = Palette[colour % Palette.Length];
            }
        }
    }
}
