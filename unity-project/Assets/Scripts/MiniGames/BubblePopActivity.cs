using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Bubble Pop: bubbles float up the screen for 40 seconds; tap them to pop. Gold bubbles are worth 3.
    /// Bubbles that float away cost nothing.
    /// </summary>
    public class BubblePopActivity : ArcadeActivity
    {
        private class Bubble { public RectTransform rt; public float x0, speed, phase, radius; public bool gold; }

        private const float Duration = 40f;
        private static readonly Color[] Tints =
        {
            new Color(0.7f, 0.88f, 1f), new Color(1f, 0.78f, 0.9f), new Color(0.8f, 1f, 0.85f), new Color(0.9f, 0.82f, 1f)
        };

        private readonly List<Bubble> bubbles = new List<Bubble>();
        private float timeLeft = Duration, spawnTimer, clock;
        private int score, popped;

        protected override string Tune { get { return "music_bath"; } }

        protected override void OnBegin()
        {
            BuildHud("Pop the bubbles! Gold ones count 3!", "bubbles");
            SetSub(Mathf.CeilToInt(timeLeft) + " s");
            Pose("Sit");
        }

        private void Spawn()
        {
            bool gold = Random.value < 0.12f;
            float size = Random.Range(150f, 230f);
            Image img = UIKit.Img("Bubble", UI, Art.Icon("bubble"), gold ? new Color(1f, 0.88f, 0.4f) : Tints[Random.Range(0, Tints.Length)]);
            img.preserveAspect = true;
            var b = new Bubble
            {
                rt = img.rectTransform, x0 = Random.Range(-400f, 400f), speed = Random.Range(170f, 300f),
                phase = Random.Range(0f, 6.28f), radius = size * 0.55f, gold = gold
            };
            UIKit.Place(b.rt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(b.x0, -1000f), new Vector2(size, size));
            if (gold)
            {
                Image star = UIKit.Icon("Star", b.rt, "star", size * 0.5f);
                star.color = Color.white;
            }
            bubbles.Add(b);
        }

        private void Update()
        {
            if (!Running || !Ready || Over) return;
            float dt = Time.deltaTime;
            clock += dt;
            timeLeft -= dt;
            SetSub(Mathf.Max(0, Mathf.CeilToInt(timeLeft)) + " s");

            spawnTimer -= dt;
            if (spawnTimer <= 0f && timeLeft > 1f) { Spawn(); spawnTimer = Random.Range(0.35f, 0.7f); }

            for (int i = bubbles.Count - 1; i >= 0; i--)
            {
                Bubble b = bubbles[i];
                Vector2 p = b.rt.anchoredPosition;
                p.y += b.speed * dt;
                p.x = b.x0 + Mathf.Sin(clock * 2f + b.phase) * 35f;
                b.rt.anchoredPosition = p;
                if (p.y > 1100f) { Destroy(b.rt.gameObject); bubbles.RemoveAt(i); }
            }

            if (Pointer.Down && !Pointer.OverUI)
            {
                Vector2 pt = PointerLocal();
                for (int i = bubbles.Count - 1; i >= 0; i--)
                {
                    Bubble b = bubbles[i];
                    if ((b.rt.anchoredPosition - pt).magnitude < b.radius) { Pop(b, i); break; }
                }
            }

            if (timeLeft <= 0f)
            {
                for (int i = 0; i < bubbles.Count; i++) Destroy(bubbles[i].rt.gameObject);
                bubbles.Clear();
                EndRound(1 + score / 5, score + " points!");
            }
        }

        private void Pop(Bubble b, int index)
        {
            Image spark = UIKit.Icon("Spark", UI, "sparkle", b.radius * 2f);
            spark.rectTransform.anchoredPosition = b.rt.anchoredPosition;
            Destroy(spark.gameObject, 0.25f);
            Destroy(b.rt.gameObject);
            bubbles.RemoveAt(index);
            int worth = b.gold ? 3 : 1;
            score += worth; popped++;
            SetScore(score.ToString());
            AudioManager.Play(b.gold ? "coin" : "pop", 0.7f, Random.Range(0.9f, 1.3f));
            if (popped % 6 == 0) Cheer(2);
        }
    }
}
