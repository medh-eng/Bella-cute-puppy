using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Car Race: Bella drives down a three-lane road. Tap left or right of the car to change lane; collect treats and
    /// stars, steer round the cones. Bumping a cone only slows the car down and costs a point. 45 seconds.
    /// </summary>
    public class CarRaceActivity : ArcadeActivity
    {
        private class Thing { public RectTransform rt; public int lane; public int kind; }   // 0 cone, 1 treat, 2 star

        private const float RoundSeconds = 45f, LaneW = 260f, CarY = -640f;
        private const int DashCount = 14;
        private const float DashGap = 200f;

        private readonly List<Thing> things = new List<Thing>();
        private readonly List<RectTransform> dashes = new List<RectTransform>();
        private RectTransform car;
        private int lane = 1;
        private float carX, speed = 620f, dashOffset, spawnTimer = 0.8f, timeLeft = RoundSeconds, elapsed, hurt, shake;
        private int score;

        protected override string Tune { get { return "music_play"; } }

        protected override void OnBegin()
        {
            // road
            Image road = UIKit.Img("Road", UI, null, new Color(0.30f, 0.31f, 0.36f, 1f));
            UIKit.Place(road.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(LaneW * 3f + 40f, 3400f));
            for (int s = -1; s <= 1; s += 2)
            {
                Image edge = UIKit.Img("Edge", UI, null, Color.white);
                UIKit.Place(edge.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(s * (LaneW * 1.5f + 10f), 0f), new Vector2(14f, 3400f));
            }
            for (int s = -1; s <= 1; s += 2)
                for (int i = 0; i < DashCount; i++)
                {
                    Image d = UIKit.Img("Dash", UI, null, new Color(1f, 1f, 1f, 0.85f));
                    UIKit.Place(d.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(s * LaneW * 0.5f, 0f), new Vector2(12f, 90f));
                    dashes.Add(d.rectTransform);
                }

            // car with Bella at the wheel
            Image body = UIKit.Panel("Car", UI, new Color(0.96f, 0.40f, 0.45f), false);
            car = body.rectTransform;
            UIKit.Place(car, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, CarY), new Vector2(150f, 240f));
            float[] wx = { -84f, 84f }, wy = { -80f, 80f };
            for (int a = 0; a < 2; a++) for (int b = 0; b < 2; b++)
            {
                Image w = UIKit.Img("Wheel", car, null, new Color(0.18f, 0.16f, 0.2f));
                UIKit.Place(w.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(wx[a], wy[b]), new Vector2(26f, 62f));
            }
            Image roof = UIKit.Panel("Roof", car, new Color(1f, 0.85f, 0.88f), false);
            UIKit.Place(roof.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -10f), new Vector2(120f, 130f));
            var face = new GameObject("Bella", typeof(RectTransform), typeof(RawImage));
            face.transform.SetParent(car, false);
            RawImage fi = face.GetComponent<RawImage>();
            fi.texture = Art.Tex("Portraits/face_happy"); fi.raycastTarget = false;
            UIKit.Place(fi.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -8f), new Vector2(112f, 112f));

            BuildHud("Tap left or right to steer!", "treat");
            Pose("Happy");
            // the HUD was added after the road; make sure it sits on top
            for (int i = 0; i < UI.childCount; i++)
            {
                string n = UI.GetChild(i).name;
                if (n == "Score" || n == "Sub" || n == "Hint" || n == "Back") UI.GetChild(i).SetAsLastSibling();
            }
        }

        private void Update()
        {
            if (!Running || !Ready || Over) return;
            float dt = Time.deltaTime;
            elapsed += dt;
            timeLeft -= dt;
            SetSub(Mathf.Max(0, Mathf.CeilToInt(timeLeft)).ToString());
            if (timeLeft <= 0f) { EndRound(2 + score / 3, score + " points!"); return; }

            // steering: tap left or right of the car
            if (Pointer.Down && !Pointer.OverUI)
            {
                float x = PointerLocal().x;
                if (x < carX - 20f) lane = Mathf.Max(0, lane - 1); else if (x > carX + 20f) lane = Mathf.Min(2, lane + 1);
                AudioManager.Play("whoosh", 0.35f, 1.4f);
            }
            carX = Mathf.MoveTowards(carX, (lane - 1) * LaneW, 1500f * dt);
            hurt = Mathf.Max(0f, hurt - dt);
            shake = Mathf.Max(0f, shake - dt * 3f);
            car.anchoredPosition = new Vector2(carX + Mathf.Sin(Time.time * 60f) * 14f * shake, CarY);

            float target = Mathf.Min(1050f, 620f + elapsed * 9f);
            speed = Mathf.MoveTowards(speed, hurt > 0f ? target * 0.5f : target, 600f * dt);
            dashOffset = (dashOffset + speed * dt) % (DashGap * DashCount);
            for (int i = 0; i < dashes.Count; i++)
            {
                float y = ((i % DashCount) * DashGap + dashOffset) % (DashGap * DashCount) - DashGap * DashCount * 0.5f;
                Vector2 p = dashes[i].anchoredPosition; p.y = y; dashes[i].anchoredPosition = p;
            }

            spawnTimer -= dt;
            if (spawnTimer <= 0f) { Spawn(); spawnTimer = Mathf.Lerp(0.8f, 0.5f, elapsed / RoundSeconds); }
            StepThings(dt);
        }

        private void Spawn()
        {
            int l = Random.Range(0, 3);
            float r = Random.value;
            int kind = r < 0.38f ? 0 : (r < 0.88f ? 1 : 2);
            RectTransform rt;
            if (kind == 0)
            {
                Image cone = UIKit.Img("Cone", UI, null, new Color(1f, 0.58f, 0.2f));
                Image stripe = UIKit.Img("Stripe", cone.transform, null, Color.white);
                UIKit.Place(stripe.rectTransform, new Vector2(0.5f, 0.55f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(70f, 16f));
                UIKit.Place(cone.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2((l - 1) * LaneW, 1300f), new Vector2(76f, 96f));
                rt = cone.rectTransform;
            }
            else
            {
                Image ic = UIKit.Icon(kind == 2 ? "Star" : "Treat", UI, kind == 2 ? "star" : "treat", kind == 2 ? 120f : 100f);
                UIKit.Place(ic.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2((l - 1) * LaneW, 1300f), ic.rectTransform.sizeDelta);
                rt = ic.rectTransform;
            }
            things.Add(new Thing { rt = rt, lane = l, kind = kind });
            // keep the HUD on top of new things
            if (HintLabel != null) HintLabel.transform.SetAsLastSibling();
        }

        private void StepThings(float dt)
        {
            for (int i = things.Count - 1; i >= 0; i--)
            {
                Thing t = things[i];
                Vector2 p = t.rt.anchoredPosition;
                p.y -= speed * dt;
                t.rt.anchoredPosition = p;
                bool near = Mathf.Abs(p.y - CarY) < 120f && Mathf.Abs(p.x - carX) < 110f;
                if (near)
                {
                    if (t.kind == 0)
                    {
                        if (hurt <= 0f)
                        {
                            hurt = 0.9f; shake = 1f;
                            score = Mathf.Max(0, score - 1); SetScore(score.ToString());
                            AudioManager.Play("oops", 0.8f);
                            SetHint("Oops! Steer round the cones.");
                        }
                    }
                    else
                    {
                        score += t.kind == 2 ? 3 : 1; SetScore(score.ToString());
                        AudioManager.Play(t.kind == 2 ? "chime" : "coin", 0.8f, Random.Range(0.95f, 1.15f));
                        if (t.kind == 2) Cheer(5); else Bella.Puppy.PulseJoy();
                    }
                    Destroy(t.rt.gameObject); things.RemoveAt(i); continue;
                }
                if (p.y < -1300f) { Destroy(t.rt.gameObject); things.RemoveAt(i); }
            }
        }
    }
}
