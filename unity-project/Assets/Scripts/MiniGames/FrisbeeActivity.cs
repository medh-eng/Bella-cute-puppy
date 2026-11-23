using UnityEngine;
using Bella.Core;
using Bella.Pet;
using Bella.States;

namespace Bella.MiniGames
{
    /// <summary>
    /// Frisbee (park): swipe up to throw — a long swipe flies far, a swipe to the side curves the throw. A gold target pad
    /// sits somewhere on the lawn; land the frisbee on it for three points, anywhere else for one. Bella chases the
    /// frisbee, catches it in her mouth and brings it back. Eight throws a visit.
    /// </summary>
    public class FrisbeeActivity : ArcadeActivity
    {
        private enum Phase { Starting, Ready, Flying, Fetching, Returning }

        private const int Throws = 8;
        private static readonly Vector3 ThrowFrom = new Vector3(0f, 0.55f, -0.45f);
        private static readonly Vector3 StandAt = new Vector3(0f, 0f, -0.3f);
        private const float FlightTime = 1.25f, PadRadius = 0.42f;

        private Phase phase = Phase.Starting;
        private Transform frisbee, pad;
        private Vector3 land, padCentre;
        private Vector2 swipeStart;
        private bool swiping;
        private int throwsLeft = Throws, score;
        private float flightT, curve;

        protected override string Tune { get { return "music_play"; } }
        protected override string Place { get { return "park"; } }
        protected override string PlaceFloor { get { return "floor_grass"; } }

        protected override void OnBegin()
        {
            BuildHud("Swipe up to throw the frisbee!", "toy_frisbee");
            SetSub(throwsLeft + " throws");
            BuildPad();
            BuildFrisbee();
            MovePad();
            G.CameraRig.Focus(new Vector3(0f, 0f, 0.45f), 3.4f, 0.5f);
            Bella.BeginActivity(StandAt, 180f, "PlayBow", () =>
            {
                Ready = true; phase = Phase.Ready;
                Bella.Face.Set(Expression.Excited);
            });
        }

        protected override void OnEnd()
        {
            Bella.ReleaseFromMouth();
            Bella.Puppy.ClearLook();
            if (frisbee != null) Destroy(frisbee.gameObject);
            if (pad != null) Destroy(pad.gameObject);
        }

        private void BuildPad()
        {
            var g = new GameObject("TargetPad");
            g.transform.SetParent(G.Room.transform, false);
            Art.Prim(PrimitiveType.Cylinder, "Ring", g.transform, new Vector3(0f, 0.012f, 0f), new Vector3(PadRadius * 2.1f, 0.004f, PadRadius * 2.1f), Art.Gold);
            Art.Prim(PrimitiveType.Cylinder, "Inner", g.transform, new Vector3(0f, 0.017f, 0f), new Vector3(PadRadius * 1.6f, 0.004f, PadRadius * 1.6f), Art.Pink);
            Art.Prim(PrimitiveType.Cylinder, "Bull", g.transform, new Vector3(0f, 0.022f, 0f), new Vector3(PadRadius * 0.7f, 0.004f, PadRadius * 0.7f), Art.Gold);
            pad = g.transform;
        }

        private void BuildFrisbee()
        {
            // The model lies flat in its own space; the root keeps the old "quad lying flat" convention
            // (X rotation 90 = flat, 80 = tilted in flight), so the model child is turned back by -90.
            var root = new GameObject("Frisbee");
            root.transform.SetParent(G.Room.transform, false);
            GameObject model = Art.Prop("toy_frisbee", root.transform, new Vector3(0f, -0.025f, 0f), 0.32f);
            if (model != null)
            {
                model.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                frisbee = root.transform;
                Rest();
                return;
            }
            Destroy(root);
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = "Frisbee";
            Destroy(go.GetComponent<Collider>());
            go.transform.SetParent(G.Room.transform, false);
            go.transform.localScale = new Vector3(0.32f, 0.32f, 1f);
            Renderer r = go.GetComponent<Renderer>();
            r.sharedMaterial = Art.SpriteMat(Color.white, Art.Tex("Icons/toy_frisbee"));
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            frisbee = go.transform;
            Rest();
        }

        private void Rest()
        {
            frisbee.position = G.Room.transform.TransformPoint(new Vector3(0.28f, 0.03f, -0.3f));
            frisbee.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        private void MovePad()
        {
            padCentre = new Vector3(Random.Range(-0.95f, 0.95f), 0f, Random.Range(0.45f, 1.25f));
            pad.localPosition = padCentre;
        }

        private void Update()
        {
            if (!Running || !Ready) return;
            if (phase == Phase.Ready)
            {
                if (Pointer.Down && !Pointer.OverUI) { swiping = true; swipeStart = Pointer.Position; }
                if (swiping && Pointer.Up)
                {
                    swiping = false;
                    Vector2 d = Pointer.Position - swipeStart;
                    if (d.y > 100f) Throw(d);
                }
            }
            else if (phase == Phase.Flying)
            {
                flightT += Time.deltaTime / FlightTime;
                float k = Mathf.Clamp01(flightT);
                Vector3 from = ThrowFrom, to = land + Vector3.up * 0.04f;
                Vector3 p = Vector3.Lerp(from, to, k);
                p.y += Mathf.Sin(k * Mathf.PI) * 0.55f;
                p.x += Mathf.Sin(k * Mathf.PI) * curve;                     // gentle bend
                frisbee.position = G.Room.transform.TransformPoint(p);
                frisbee.rotation = Quaternion.Euler(80f, k * 900f, 0f);
                if (k >= 1f) Landed();
            }
        }

        private void Throw(Vector2 swipe)
        {
            phase = Phase.Flying; flightT = 0f;
            throwsLeft--; SetSub(throwsLeft + " throws");
            float sw = Mathf.Max(1f, Screen.width), sh = Mathf.Max(1f, Screen.height);
            land = new Vector3(Mathf.Clamp(swipe.x / sw * 3.6f, -1.25f, 1.25f), 0f, Mathf.Clamp(0.15f + swipe.y / sh * 2.6f, 0.2f, 1.45f));
            curve = Mathf.Clamp(swipe.x / sw * 0.5f, -0.3f, 0.3f);
            AudioManager.Play("whoosh", 0.8f);
            Bella.Puppy.LookAt(G.Room.transform.TransformPoint(land + Vector3.up * 0.5f));
            SetHint("");
            // she is off the moment it leaves the hand
            Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, land - new Vector3(0f, 0f, 0.2f), Bella.Run * 1.25f, true, null, null));
        }

        private void Landed()
        {
            phase = Phase.Fetching;
            frisbee.rotation = Quaternion.Euler(90f, 0f, 0f);
            float dist = new Vector2(land.x - padCentre.x, land.z - padCentre.z).magnitude;
            bool bull = dist < PadRadius;
            score += bull ? 3 : 1;
            SetScore(score.ToString());
            AudioManager.Play(bull ? "chime" : "tap", 0.8f);
            SetHint(bull ? "On the target! +3" : "Nice throw! +1");
            Fx.Sparkles(frisbee.position, bull ? 12 : 4);
            if (bull) Cheer(6);
            // Bella is already running to it; give her a path to the frisbee itself and wait for her to arrive
            Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, land - new Vector3(0f, 0f, 0.1f), Bella.Run * 1.25f, true, Grab, null));
        }

        private void Grab()
        {
            if (!Running) return;
            Bella.Puppy.ClearLook();
            AudioManager.Play("tap", 0.7f);
            Bella.HoldInMouth(frisbee, frisbee.localScale);
            Bella.Face.Set(Expression.Happy);
            phase = Phase.Returning;
            Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, StandAt, Bella.Run * 0.9f, true, Deliver, 180f));
        }

        private void Deliver()
        {
            if (!Running) return;
            Bella.ReleaseFromMouth();
            Rest();
            AudioManager.Play("pant", 0.5f);
            Bella.Puppy.PulseJoy();
            if (throwsLeft <= 0)
            {
                phase = Phase.Starting;
                EndRound(1 + score / 3, score + " points!");
                return;
            }
            MovePad();
            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "PlayBow", 9999f, null));
            phase = Phase.Ready;
            SetHint("Again! Swipe up to throw.");
        }
    }
}
