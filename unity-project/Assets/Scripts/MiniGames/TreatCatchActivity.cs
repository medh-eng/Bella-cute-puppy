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
    /// Treat Catch (kitchen): treats and golden stars drop from the top; slide a finger left and right
    /// and Bella runs under them. A round lasts 30 seconds. Nothing is ever lost or missed in a way that
    /// matters — a dropped treat just pops. Pays from the shared daily play allowance.
    /// </summary>
    public class TreatCatchActivity : Activity
    {
        private class Drop { public Transform t; public float speed, spin; public bool golden; }

        private const float RoundSeconds = 30f;
        private const float XLimit = 1.05f;
        private const float BellaZ = 0.1f;
        private const float SpawnY = 2.5f;
        private const float CatchY = 0.62f;

        private readonly List<Drop> drops = new List<Drop>();
        private Text hint, scoreLabel, timeLabel;
        private float timeLeft = RoundSeconds, spawnTimer = 0.6f, targetX, runBlend;
        private int score, bonusTreats;
        private bool playing, finished;
        private string lastClip = "";

        protected override string Tune { get { return "music_play"; } }
        protected override string Place { get { return "kitchen"; } }
        protected override string PlaceFloor { get { return "floor_checker"; } }

        protected override void OnBegin()
        {
            Image pill = UIKit.Panel("Score", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Treat", pill.transform, "treat", 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            scoreLabel = UIKit.Label("Count", pill.transform, "0", 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(scoreLabel.rectTransform, 112f, 0f, 14f, 0f);

            Image tpill = UIKit.Panel("Time", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(tpill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -240f), new Vector2(270f, 84f));
            timeLabel = UIKit.Label("Time", tpill.transform, "30", 50, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Stretch(timeLabel.rectTransform, 0f, 0f, 0f, 0f);

            hint = UIKit.Label("Hint", UI, "Slide your finger to catch the treats!", 50, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(hint.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -290f), new Vector2(960f, 100f));
            hint.resizeTextForBestFit = true; hint.resizeTextMinSize = 28; hint.resizeTextMaxSize = 50;

            G.CameraRig.Focus(new Vector3(0f, 0f, BellaZ), 3.0f, 0.9f);
            Bella.BeginActivity(new Vector3(0f, 0f, BellaZ), 180f, "PlayBow", () =>
            {
                Ready = true; playing = true;
                Bella.Face.Set(Expression.Excited);
            });
        }

        protected override void OnEnd()
        {
            for (int i = 0; i < drops.Count; i++) if (drops[i].t != null) Destroy(drops[i].t.gameObject);
            drops.Clear();
        }

        private void Update()
        {
            if (!Running || !playing) return;
            float dt = Time.deltaTime;

            // steer: finger x on screen -> room x
            if (Pointer.Held && !Pointer.OverUI)
            {
                Ray ray = G.Cam.ScreenPointToRay(Pointer.Position);
                if (Mathf.Abs(ray.direction.y) > 0.0001f)
                {
                    float t = -ray.origin.y / ray.direction.y;
                    if (t > 0f) targetX = Mathf.Clamp((ray.origin + ray.direction * t).x, -XLimit, XLimit);
                }
            }
            MoveBella(dt);

            timeLeft -= dt;
            if (timeLabel != null) timeLabel.text = Mathf.Max(0, Mathf.CeilToInt(timeLeft)).ToString();
            if (timeLeft <= 0f) { EndRound(); return; }

            spawnTimer -= dt;
            if (spawnTimer <= 0f)
            {
                SpawnDrop();
                spawnTimer = Mathf.Lerp(0.85f, 0.45f, 1f - timeLeft / RoundSeconds);   // a little faster as the round goes
            }
            StepDrops(dt);
        }

        private void MoveBella(float dt)
        {
            Transform b = Bella.transform;
            Vector3 p = b.position;
            float dx = targetX - p.x;
            bool moving = Mathf.Abs(dx) > 0.04f;
            if (moving)
            {
                float step = Mathf.Min(Mathf.Abs(dx), Bella.Run * 1.5f * dt);
                p.x += Mathf.Sign(dx) * step;
                b.position = p;
                Quaternion look = Quaternion.LookRotation(new Vector3(Mathf.Sign(dx), 0f, 0f), Vector3.up);
                b.rotation = Quaternion.RotateTowards(b.rotation, look, 900f * dt);
            }
            else
            {
                Quaternion face = Quaternion.Euler(0f, 180f, 0f);
                b.rotation = Quaternion.RotateTowards(b.rotation, face, 600f * dt);
            }
            string clip = moving ? "Run" : "Happy";
            if (clip != lastClip) { Bella.Anim.Play(clip, 0.12f); lastClip = clip; }
        }

        private void SpawnDrop()
        {
            bool golden = Random.value < 0.18f;
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = golden ? "GoldenTreat" : "Treat";
            Destroy(go.GetComponent<Collider>());
            go.transform.SetParent(G.Room.transform, false);
            go.transform.localPosition = new Vector3(Random.Range(-XLimit, XLimit), SpawnY, BellaZ - 0.05f);
            float size = golden ? 0.34f : 0.28f;
            go.transform.localScale = new Vector3(size, size, 1f);
            Renderer r = go.GetComponent<Renderer>();
            r.sharedMaterial = Art.SpriteMat(Color.white, Art.Tex(golden ? "Icons/star" : "Icons/treat"));
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            drops.Add(new Drop { t = go.transform, golden = golden, speed = Random.Range(1.1f, 1.7f) * (golden ? 1.25f : 1f), spin = Random.Range(-60f, 60f) });
        }

        private void StepDrops(float dt)
        {
            Vector3 mouth = Bella.HeadPoint;
            for (int i = drops.Count - 1; i >= 0; i--)
            {
                Drop d = drops[i];
                if (d.t == null) { drops.RemoveAt(i); continue; }
                Vector3 p = d.t.position;
                p.y -= d.speed * dt;
                d.t.position = p;
                d.t.rotation = Quaternion.Euler(0f, 0f, d.t.eulerAngles.z + d.spin * dt);

                bool near = Mathf.Abs(p.x - mouth.x) < 0.3f && p.y < CatchY + 0.25f && p.y > CatchY - 0.35f;
                if (near) { Caught(d); drops.RemoveAt(i); Destroy(d.t.gameObject); continue; }
                if (p.y < 0.08f)
                {
                    Fx.Burst(p, "sparkle", 3, 0.16f, 0.2f, 0.4f, 0.6f);
                    AudioManager.Play("pop", 0.35f, 0.8f);
                    drops.RemoveAt(i); Destroy(d.t.gameObject);
                }
            }
        }

        private void Caught(Drop d)
        {
            int worth = d.golden ? 3 : 1;
            score += worth;
            AudioManager.Play(d.golden ? "chime" : "coin", 0.8f, Random.Range(0.95f, 1.15f));
            Fx.Sparkles(Bella.HeadPoint + Vector3.up * 0.1f, d.golden ? 10 : 5);
            if (d.golden) Fx.Stars(Bella.HeadPoint + Vector3.up * 0.2f, 8);
            Bella.Puppy.PulseJoy();
            if (scoreLabel != null) { scoreLabel.text = score.ToString(); }
            if (Random.value < 0.25f) AudioManager.Voice("yip");
        }

        private void EndRound()
        {
            if (finished) return;
            finished = true; playing = false;
            for (int i = 0; i < drops.Count; i++) if (drops[i].t != null) Destroy(drops[i].t.gameObject);
            drops.Clear();

            SaveData d = SaveSystem.Data;
            d.countCatchRounds++;
            Bella.Anim.Play("Happy", 0.2f);
            Bella.Needs.AddHappiness(12f);
            Bella.Needs.Tire(4f);
            int want = Mathf.Clamp(Mathf.CeilToInt(score / 4f), 1, 8);
            int treats = Progress.PlayTreats(want);
            if (treats > 0) Bella.Needs.AddTreats(treats);
            SaveSystem.Flush();
            AudioManager.Voice("bark_happy");
            AudioManager.Play("success", 0.8f);
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, 12);
            Fx.Stars(Bella.HeadPoint + Vector3.up * 0.35f, 14);
            hint.text = score + " caught!" + (treats > 0 ? "  +" + treats + " treats" : "");
            FinishAfter(2.8f);
        }
    }
}
