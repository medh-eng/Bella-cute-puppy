using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Arrow Release: put a finger down, pull back to aim (dotted line shows where the arrow will go) and let go
    /// to shoot. Balloons float up the screen; pop them with twelve arrows. Golden balloons are worth three.
    /// </summary>
    public class ArrowActivity : ArcadeActivity
    {
        private class Balloon { public RectTransform rt; public float vy, wob, baseX; public bool golden; }
        private class Arrow { public RectTransform rt; public Vector2 vel; }

        private const int ArrowsPerRound = 12, BalloonTarget = 6;
        private const float Gravity = 650f, MaxPull = 380f, PullPower = 4.2f;
        private static readonly Vector2 BowPos = new Vector2(0f, -720f);
        private static readonly Color[] Cols =
        {
            new Color(0.96f, 0.40f, 0.45f), new Color(0.35f, 0.75f, 0.92f), new Color(0.45f, 0.80f, 0.45f),
            new Color(0.70f, 0.50f, 0.92f), new Color(1.00f, 0.62f, 0.28f),
        };

        private readonly List<Balloon> balloons = new List<Balloon>();
        private readonly List<Arrow> arrows = new List<Arrow>();
        private readonly List<Image> dots = new List<Image>();
        private RectTransform bow;
        private bool aiming;
        private Vector2 aimStart, pull;
        private int left = ArrowsPerRound, score;

        protected override string Tune { get { return "music_play"; } }

        protected override void OnBegin()
        {
            BuildHud("Pull back and let go to shoot!", "game_arrow");
            Image b = UIKit.Icon("Bow", UI, "paw", 170f);
            bow = b.rectTransform;
            UIKit.Place(bow, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), BowPos, new Vector2(170f, 170f));
            for (int i = 0; i < 10; i++)
            {
                Image d = UIKit.Img("Dot", UI, Art.Icon("circle"), new Color(1f, 1f, 1f, 0.9f));
                UIKit.Place(d.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), BowPos, Vector2.one * (22f - i));
                d.gameObject.SetActive(false);
                dots.Add(d);
            }
            for (int i = 0; i < BalloonTarget; i++) SpawnBalloon(true);
            SetSub(left + " arrows");
            Pose("Happy");
        }

        private void SpawnBalloon(bool anywhere)
        {
            bool golden = Random.value < 0.15f;
            Image im = UIKit.Img("Balloon", UI, Art.Icon("circle"), golden ? new Color(1f, 0.82f, 0.25f) : Cols[Random.Range(0, Cols.Length)]);
            float sz = Random.Range(130f, 170f);
            float x = Random.Range(-380f, 380f);
            float y = anywhere ? Random.Range(-200f, 800f) : Random.Range(-950f, -800f);
            UIKit.Place(im.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, y), new Vector2(sz, sz * 1.12f));
            Image shine = UIKit.Img("Shine", im.transform, Art.Icon("circle"), new Color(1f, 1f, 1f, 0.45f));
            UIKit.Place(shine.rectTransform, new Vector2(0.3f, 0.7f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(sz * 0.22f, sz * 0.3f));
            if (golden) { Image s = UIKit.Icon("Star", im.transform, "star", sz * 0.5f); }
            balloons.Add(new Balloon { rt = im.rectTransform, vy = Random.Range(110f, 190f), wob = Random.Range(0f, 6f), baseX = x, golden = golden });
        }

        private void Update()
        {
            if (!Running || !Ready) return;
            float dt = Time.deltaTime;
            StepBalloons(dt);
            StepArrows(dt);
            if (!Over) HandleAim();
            if (!Over && left <= 0 && arrows.Count == 0)
                EndRound(1 + score / 3, score + " points!");
        }

        private void HandleAim()
        {
            if (left <= 0) return;
            if (Pointer.Down && !Pointer.OverUI) { aiming = true; aimStart = PointerLocal(); pull = Vector2.zero; }
            if (aiming && Pointer.Held)
            {
                pull = Vector2.ClampMagnitude(PointerLocal() - aimStart, MaxPull);
                if (pull.y > 0f) pull.y = 0f;                      // only pull downwards, like drawing a bow
                Vector2 vel = -pull * PullPower;
                bool show = pull.magnitude > 40f;
                for (int i = 0; i < dots.Count; i++)
                {
                    dots[i].gameObject.SetActive(show);
                    float t = (i + 1) * 0.09f;
                    dots[i].rectTransform.anchoredPosition = BowPos + vel * t + new Vector2(0f, -0.5f * Gravity * t * t);
                }
                bow.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg - 90f);
            }
            if (aiming && Pointer.Up)
            {
                aiming = false;
                for (int i = 0; i < dots.Count; i++) dots[i].gameObject.SetActive(false);
                bow.localRotation = Quaternion.identity;
                if (pull.magnitude > 60f) Shoot(-pull * PullPower);
            }
        }

        private void Shoot(Vector2 vel)
        {
            left--;
            SetSub(left + " arrows");
            Image im = UIKit.Img("Arrow", UI, null, new Color(0.55f, 0.36f, 0.22f));
            UIKit.Place(im.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), BowPos, new Vector2(120f, 12f));
            Image tip = UIKit.Img("Tip", im.transform, null, new Color(0.45f, 0.45f, 0.5f));
            UIKit.Place(tip.rectTransform, new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(26f, 22f));
            Image fl = UIKit.Img("Fletch", im.transform, null, new Color(0.96f, 0.40f, 0.45f));
            UIKit.Place(fl.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(26f, 26f));
            arrows.Add(new Arrow { rt = im.rectTransform, vel = vel });
            AudioManager.Play("whoosh", 0.7f, Random.Range(1.1f, 1.4f));
        }

        private void StepArrows(float dt)
        {
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                Arrow a = arrows[i];
                a.vel.y -= Gravity * dt;
                Vector2 p = a.rt.anchoredPosition + a.vel * dt;
                a.rt.anchoredPosition = p;
                a.rt.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(a.vel.y, a.vel.x) * Mathf.Rad2Deg);
                Vector2 tip = p + a.vel.normalized * 60f;
                bool hit = false;
                for (int j = balloons.Count - 1; j >= 0; j--)
                {
                    Balloon b = balloons[j];
                    float rad = b.rt.sizeDelta.x * 0.5f;
                    if ((b.rt.anchoredPosition - tip).magnitude < rad + 8f)
                    {
                        PopBalloon(b);
                        balloons.RemoveAt(j);
                        hit = true; break;
                    }
                }
                if (hit || p.y < -1300f || Mathf.Abs(p.x) > 900f)
                {
                    Destroy(a.rt.gameObject);
                    arrows.RemoveAt(i);
                }
            }
        }

        private void PopBalloon(Balloon b)
        {
            int worth = b.golden ? 3 : 1;
            score += worth; SetScore(score.ToString());
            AudioManager.Play("pop", 0.8f, Random.Range(0.9f, 1.2f));
            AudioManager.Play(b.golden ? "chime" : "coin", 0.6f);
            Cheer(b.golden ? 6 : 2);
            StartCoroutine(Burst(b.rt));
            SetHint(b.golden ? "Golden balloon! +3" : "Pop!");
        }

        private IEnumerator Burst(RectTransform rt)
        {
            Image im = rt.GetComponent<Image>();
            for (float e = 0f; e < 0.25f && rt != null; e += Time.deltaTime)
            {
                float k = e / 0.25f;
                rt.localScale = Vector3.one * (1f + k * 0.8f);
                if (im != null) { Color c = im.color; c.a = 1f - k; im.color = c; }
                yield return null;
            }
            if (rt != null) Destroy(rt.gameObject);
        }

        private void StepBalloons(float dt)
        {
            for (int i = balloons.Count - 1; i >= 0; i--)
            {
                Balloon b = balloons[i];
                b.wob += dt * 1.6f;
                Vector2 p = b.rt.anchoredPosition;
                p.y += b.vy * dt;
                p.x = Mathf.Clamp(b.baseX + Mathf.Sin(b.wob) * 60f, -400f, 400f);
                b.rt.anchoredPosition = p;
                if (p.y > 1150f) { Destroy(b.rt.gameObject); balloons.RemoveAt(i); }
            }
            if (!Over) while (balloons.Count < BalloonTarget) SpawnBalloon(false);
        }
    }
}
