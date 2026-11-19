using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.States;
using Bella.Systems;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Fetch: the player picks the ball up with a finger, flicks it, and Bella chases it,
    /// picks it up in her mouth and brings it back to the drop mat — every single time.
    /// The ball has its own tiny physics (gravity, bounces, rolling, room walls), so it
    /// never needs colliders and can never get lost. Tapping the floor sends Bella there.
    /// </summary>
    public class FetchActivity : Activity
    {
        private enum Phase { Starting, Waiting, Held, Chasing, Grabbing, Returning, Celebrating }

        // ---- play area (metres, room space) -------------------------------------------------
        private static readonly Vector3 MatPoint = new Vector3(0f, 0f, -0.35f);     // where the ball is delivered
        private static readonly Vector3 StandPoint = new Vector3(0f, 0f, -0.02f);   // where Bella stands to drop it
        private const float XMin = -1.2f, XMax = 1.2f, ZMin = -0.55f, ZMax = 1.05f;
        private const float Radius = 0.11f;
        private const float Gravity = 9.8f;
        private const float Bounce = 0.58f;
        private const float RollDrag = 1.35f;

        private Phase phase = Phase.Starting;
        private GameObject ball;
        private Transform ballT;
        private GameObject shadow;
        private GameObject mat;
        private Vector3 vel;
        private bool carried;
        private float carryBlend;

        // finger tracking while the ball is held
        private float holdDepth;
        private readonly List<Vector3> samples = new List<Vector3>();   // x,y = screen pos, z = time

        // UI
        private Text hint, treatsLabel, streakLabel;
        private GameObject treatsPill;
        private float hintClear;
        private int streak;             // fetches in this visit
        private bool everThrown;

        protected override string Tune { get { return "music_play"; } }
        protected override string Place { get { return "park"; } }
        protected override string PlaceFloor { get { return "floor_grass"; } }

        protected override void OnBegin()
        {
            BuildMat();
            BuildBall();
            G.Room.HideBall();

            Image pill = UIKit.Panel("Treats", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Coin", pill.transform, "treat", 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            treatsLabel = UIKit.Label("Count", pill.transform, "0", 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(treatsLabel.rectTransform, 112f, 0f, 14f, 0f);
            treatsPill = pill.gameObject;

            Image streakPill = UIKit.Panel("Streak", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(streakPill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -240f), new Vector2(270f, 84f));
            Image ballIcon = UIKit.Icon("BallIcon", streakPill.transform, "ball", 66f);
            UIKit.Place(ballIcon.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(50f, 0f), new Vector2(66f, 66f));
            streakLabel = UIKit.Label("Count", streakPill.transform, "0", 46, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(streakLabel.rectTransform, 100f, 0f, 14f, 0f);

            hint = UIKit.Label("Hint", UI, "Flick the ball for Bella!", 52, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(hint.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -290f), new Vector2(960f, 100f));
            hint.resizeTextForBestFit = true; hint.resizeTextMinSize = 28; hint.resizeTextMaxSize = 52;

            RefreshLabels();
            G.CameraRig.Focus(new Vector3(0f, 0f, 0.25f), 2.9f, 0.45f);
            Bella.BeginActivity(StandPoint, 180f, "PlayBow", () =>
            {
                Ready = true;
                phase = Phase.Waiting;
                Bella.Face.Set(Expression.Excited);
            });
        }

        protected override void OnEnd()
        {
            Bella.Puppy.ClearLook();
            if (ball != null) Destroy(ball);
            if (shadow != null) Destroy(shadow);
            if (mat != null) Destroy(mat);
            G.Room.ResetBall();
            SaveSystem.Flush();
        }

        // ================================================================ scene bits

        private void BuildMat()
        {
            mat = new GameObject("DropMat");
            mat.transform.SetParent(G.Room.transform, false);
            mat.transform.localPosition = MatPoint;
            Art.Prim(PrimitiveType.Cylinder, "MatRing", mat.transform, new Vector3(0f, 0.014f, 0f), new Vector3(0.62f, 0.004f, 0.62f), Art.Gold);
            Art.Prim(PrimitiveType.Cylinder, "MatInner", mat.transform, new Vector3(0f, 0.019f, 0f), new Vector3(0.54f, 0.004f, 0.54f), Art.Pink);
            GameObject paw = GameObject.CreatePrimitive(PrimitiveType.Quad);
            paw.name = "MatPaw";
            Destroy(paw.GetComponent<Collider>());
            paw.transform.SetParent(mat.transform, false);
            paw.transform.localPosition = new Vector3(0f, 0.026f, 0f);
            paw.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            paw.transform.localScale = new Vector3(0.34f, 0.34f, 1f);
            Renderer pr = paw.GetComponent<Renderer>();
            pr.sharedMaterial = Art.SpriteMat(Color.white, Art.Tex("Icons/paw"));
            pr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        private void BuildBall()
        {
            ball = new GameObject("FetchBall");
            ball.transform.SetParent(G.Room.transform, false);
            ballT = ball.transform;
            if (Art.Prop("ball", ballT, new Vector3(0f, -Radius, 0f), Radius * 2f) == null)
            {
                Art.Prim(PrimitiveType.Sphere, "Ball", ballT, Vector3.zero, Vector3.one * Radius * 2f, Art.Rose);
                Art.Prim(PrimitiveType.Cylinder, "BallBand", ballT, Vector3.zero, new Vector3(Radius * 2.06f, Radius * 0.24f, Radius * 2.06f), Art.Gold);
            }
            shadow = Art.Prim(PrimitiveType.Cylinder, "BallShadow", G.Room.transform, Vector3.zero, new Vector3(0.2f, 0.002f, 0.2f), new Color(0.62f, 0.5f, 0.38f));
            // starts resting on the mat
            ballT.localPosition = new Vector3(MatPoint.x, Radius, MatPoint.z);
            vel = Vector3.zero;
            carried = false;
        }

        // ================================================================ loop

        private void Update()
        {
            if (!Running || ballT == null) return;
            HandleInput();
            if (carried) UpdateCarried(); else StepBall(Time.deltaTime);
            UpdateShadow();
            if (hint != null && hintClear > 0f && Time.time > hintClear) { hint.text = ""; hintClear = 0f; }
        }

        // ---- input ---------------------------------------------------------------------------

        private void HandleInput()
        {
            if (!Ready) return;

            if (Pointer.Down && !Pointer.OverUI && phase == Phase.Waiting && TryGrabBall())
            {
                phase = Phase.Held;
                samples.Clear();
                AudioManager.Play("tap", 0.6f);
                Bella.Puppy.LookAt(ballT.position);
                Bella.Face.Set(Expression.Excited);
            }

            if (phase == Phase.Held)
            {
                if (Pointer.Held)
                {
                    Vector2 sp = Pointer.Position;
                    Vector3 w = G.Cam.ScreenToWorldPoint(new Vector3(sp.x, sp.y, holdDepth));
                    w.x = Mathf.Clamp(w.x, XMin, XMax);
                    w.z = Mathf.Clamp(w.z, ZMin, ZMax);
                    w.y = Mathf.Clamp(w.y, Radius, 1.5f);
                    ballT.position = Vector3.Lerp(ballT.position, w, 1f - Mathf.Exp(-Time.deltaTime * 28f));
                    Bella.Puppy.LookAt(ballT.position);
                    samples.Add(new Vector3(sp.x, sp.y, Time.time));
                    while (samples.Count > 1 && Time.time - samples[0].z > 0.12f) samples.RemoveAt(0);
                }
                if (Pointer.Up) Release();
            }
            else if (phase == Phase.Waiting && Pointer.Up && !Pointer.OverUI)
            {
                TapFloor();
            }
        }

        private bool TryGrabBall()
        {
            Vector3 sp = G.Cam.WorldToScreenPoint(ballT.position);
            if (sp.z <= 0f) return false;
            float reach = Screen.height * 0.085f;
            if ((new Vector2(sp.x, sp.y) - Pointer.Position).magnitude > reach) return false;
            holdDepth = sp.z;
            vel = Vector3.zero;
            return true;
        }

        private void Release()
        {
            Vector2 flick = Vector2.zero;
            if (samples.Count >= 2)
            {
                Vector3 a = samples[0], b = samples[samples.Count - 1];
                float dt = Mathf.Max(0.03f, b.z - a.z);
                flick = new Vector2(b.x - a.x, b.y - a.y) / Screen.height / dt;   // screen-heights per second
            }
            float strength = flick.magnitude;
            Vector3 v = new Vector3(flick.x * 3.2f, 0f, flick.y * 4.4f);
            Vector3 flat = v;
            if (flat.magnitude > 6.5f) v = flat.normalized * 6.5f;
            v.y = strength < 0.2f ? 0f : 1.8f + Mathf.Clamp(flick.y, 0f, 2f) * 1.4f;
            vel = v;
            AudioManager.Play("whoosh", Mathf.Clamp01(0.3f + strength * 0.25f));
            everThrown = true;
            SetHint("");
            After(0.05f, StartChase);
            phase = Phase.Chasing;
        }

        private void TapFloor()
        {
            Ray ray = G.Cam.ScreenPointToRay(Pointer.Position);
            Plane floor = new Plane(Vector3.up, Vector3.zero);
            float enter;
            if (!floor.Raycast(ray, out enter)) return;
            Vector3 p = ray.GetPoint(enter);
            if (p.x < XMin || p.x > XMax || p.z < ZMin || p.z > ZMax + 0.4f) return;
            if ((new Vector2(p.x - ballT.position.x, p.z - ballT.position.z)).magnitude < 0.3f) return;
            p.y = 0f;
            AudioManager.Play("tap", 0.5f);
            Fx.Burst(p + Vector3.up * 0.05f, "paw", 3, 0.2f, -0.2f, 0.4f, 0.8f);
            phase = Phase.Starting;       // ignore input until she gets there
            Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, p, Bella.Run * 0.8f, true, () =>
            {
                phase = Phase.Waiting;
                Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "PlayBow", 9999f, null));
            }));
        }

        // ---- physics ---------------------------------------------------------------------------

        private void StepBall(float dt)
        {
            if (phase == Phase.Held) return;
            Vector3 p = ballT.position;
            bool grounded = p.y <= Radius + 0.002f && Mathf.Abs(vel.y) < 0.05f;

            if (!grounded) vel.y -= Gravity * dt;
            p += vel * dt;

            // room walls
            if (p.x < XMin) { p.x = XMin; vel.x = Mathf.Abs(vel.x) * 0.55f; }
            if (p.x > XMax) { p.x = XMax; vel.x = -Mathf.Abs(vel.x) * 0.55f; }
            if (p.z < ZMin) { p.z = ZMin; vel.z = Mathf.Abs(vel.z) * 0.55f; }
            if (p.z > ZMax) { p.z = ZMax; vel.z = -Mathf.Abs(vel.z) * 0.55f; }

            // floor
            if (p.y <= Radius)
            {
                p.y = Radius;
                if (vel.y < -0.6f)
                {
                    vel.y = -vel.y * Bounce;
                    AudioManager.Play("tap", Mathf.Clamp01(Mathf.Abs(vel.y) * 0.25f));
                }
                else vel.y = 0f;
                float drag = Mathf.Exp(-RollDrag * dt);
                vel.x *= drag;
                vel.z *= drag;
                if (new Vector2(vel.x, vel.z).magnitude < 0.08f) { vel.x = 0f; vel.z = 0f; }
            }

            // visible rolling
            Vector3 flat = new Vector3(vel.x, 0f, vel.z);
            if (flat.sqrMagnitude > 0.0004f)
            {
                Vector3 axis = Vector3.Cross(Vector3.up, flat.normalized);
                ballT.Rotate(axis, flat.magnitude / Radius * Mathf.Rad2Deg * dt, Space.World);
            }
            ballT.position = p;
        }

        private void UpdateShadow()
        {
            if (shadow == null) return;
            Vector3 p = ballT.position;
            shadow.transform.position = new Vector3(p.x, 0.022f, p.z);
            float k = Mathf.Clamp01(1f - (p.y - Radius) / 1.6f);
            shadow.transform.localScale = new Vector3(0.2f * (0.5f + 0.5f * k), 0.002f, 0.2f * (0.5f + 0.5f * k));
            shadow.SetActive(!carried);
        }

        private bool BallCalm
        {
            get
            {
                return ballT.position.y < 0.3f && new Vector3(vel.x, 0f, vel.z).magnitude < 1.4f && Mathf.Abs(vel.y) < 1.6f;
            }
        }

        private Vector3 BallOnFloor()
        {
            // lead a rolling ball a little, like a real dog does
            Vector3 p = ballT.position + new Vector3(vel.x, 0f, vel.z) * 0.18f;
            p.x = Mathf.Clamp(p.x, XMin, XMax);
            p.z = Mathf.Clamp(p.z, ZMin, ZMax);
            p.y = 0f;
            return p;
        }

        // ================================================================ Bella's round

        private void StartChase()
        {
            if (!Running) return;
            AudioManager.Voice("bark");
            Bella.Face.Set(Expression.Excited);
            Bella.Puppy.PulseJoy();
            Bella.Puppy.LookAt(ballT.position);
            Bella.RunState(new FetchChaseState(Bella.transform, Bella.Anim, BallOnFloor, () => BallCalm, Bella.Run * 1.15f, Grab));
        }

        private void Grab()
        {
            if (!Running) return;
            phase = Phase.Grabbing;
            vel = Vector3.zero;
            Bella.Puppy.LookAt(ballT.position);
            AudioManager.Play("tap", 0.7f);
            // a short head-down beat, then the ball is in her mouth
            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "Idle", 0.4f, () =>
            {
                carried = true;
                carryBlend = 0f;
                Bella.Puppy.ClearLook();
                Bella.Puppy.PulseJoy();
                Bella.Face.Set(Expression.Happy);
                phase = Phase.Returning;
                Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, StandPoint, Bella.Run * 0.85f, true, Deliver, 180f));
            }));
        }

        private void UpdateCarried()
        {
            carryBlend = Mathf.Min(1f, carryBlend + Time.deltaTime * 6f);
            Vector3 mouth = Bella.HeadPoint + Bella.transform.forward * 0.17f + Vector3.down * 0.1f;
            ballT.position = Vector3.Lerp(ballT.position, mouth, carryBlend);
            ballT.localScale = Vector3.one * Mathf.Lerp(1f, 0.8f, carryBlend);
        }

        private void Deliver()
        {
            if (!Running) return;
            // let go over the mat: the ball drops and rolls to rest by itself
            carried = false;
            ballT.localScale = Vector3.one;
            vel = new Vector3(0f, 0.2f, -0.35f);
            phase = Phase.Celebrating;
            AudioManager.Play("chime", 0.7f);

            bool tired = Reward();
            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "Happy", 1.4f, () =>
            {
                if (tired) { Sleepy(); return; }
                SnapBallToMat();
                phase = Phase.Waiting;
                Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "PlayBow", 9999f, null));
                SetHint(everThrown ? "Again! Flick the ball." : "", 3f);
            }));
        }

        /// <summary>Whatever the bounce did, the ball always ends up on the mat.</summary>
        private void SnapBallToMat()
        {
            Vector3 p = ballT.position;
            Vector2 off = new Vector2(p.x - MatPoint.x, p.z - MatPoint.z);
            if (off.magnitude > 0.28f || p.y > 0.4f)
            {
                ballT.position = new Vector3(MatPoint.x, Radius, MatPoint.z);
                vel = Vector3.zero;
            }
        }

        private bool Reward()
        {
            streak++;
            SaveData d = SaveSystem.Data;
            d.countFetches++;
            Bella.Needs.AddHappiness(10f);
            Bella.Needs.Tire(3.5f);
            int treats = Progress.PlayTreats(1);
            bool bonus = streak % 5 == 0;
            if (bonus) treats += Progress.PlayTreats(3);
            Bella.Puppy.PulseJoy();
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, bonus ? 14 : 8);
            if (bonus)
            {
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.35f, 14);
                AudioManager.Voice("bark_happy");
                SetHint("Combo x" + streak + "! Bonus treats!", 3f);
            }
            if (treats > 0) Bella.Needs.AddTreats(treats);
            SaveSystem.Flush();
            RefreshLabels();
            UIKit.Pop(treatsPill);
            return Bella.Needs.Energy < 8f;
        }

        private void Sleepy()
        {
            Bella.Face.Flash(Expression.Sleepy, 2f);
            AudioManager.Voice("yawn_sleepy");
            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Sleepy, "Sit", 2.4f, null));
            SetHint("Bella is sleepy now. Let her rest!", 3f);
            SnapBallToMat();
            FinishAfter(2.6f);
        }

        // ================================================================ UI helpers

        private void RefreshLabels()
        {
            if (treatsLabel != null) treatsLabel.text = Bella.Needs.Treats.ToString();
            if (streakLabel != null) streakLabel.text = streak.ToString();
        }

        private void SetHint(string text, float seconds = 0f)
        {
            if (hint == null) return;
            hint.text = text;
            hintClear = seconds > 0f ? Time.time + seconds : 0f;
        }
    }
}
