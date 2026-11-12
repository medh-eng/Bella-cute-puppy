using UnityEngine;

namespace Bella.Pet
{
    /// <summary>
    /// The "cuteness engine" (docs/ARCHITECTURE.md §4): secondary motion layered on
    /// TOP of the baked animation every LateUpdate — a tail wag whose speed is
    /// Bella's mood, floppy-ear lag, a head that follows your finger, and a
    /// squash-and-stretch pulse when she is delighted.
    /// </summary>
    public class ProceduralPuppy : MonoBehaviour
    {
        private BellaModel model;

        /// <summary>0..1 — set each frame from PetNeeds.Mood01.</summary>
        public float Mood { get; set; } = 1f;

        /// <summary>Uniform size multiplier (growth system).</summary>
        public float GrowthScale { get; set; } = 1f;

        /// <summary>Tail wag strength multiplier (0 while asleep).</summary>
        public float WagAmount { get; set; } = 1f;

        /// <summary>Raises the whole puppy (metres) — e.g. onto her bed cushion while she naps.</summary>
        public float HeightOffset { get; set; }

        private float height, heightVel;
        private float squash, squashVel;
        private float excitement;
        private float wagPhase;

        private bool hasLook;
        private Vector3 lookTarget;
        private float lookWeight;

        private Vector3 lastHeadPos;
        private float earAngle, earVel;

        public void Init(BellaModel m)
        {
            model = m;
            if (model.Head != null) lastHeadPos = model.Head.position;
        }

        /// <summary>Joyful squash-and-stretch pulse — call on petting/feeding/rewards.</summary>
        public void PulseJoy()
        {
            squash = 0.22f;
            excitement = 1f;
        }

        public void LookAt(Vector3 worldPoint)
        {
            hasLook = true;
            lookTarget = worldPoint;
        }

        public void ClearLook()
        {
            hasLook = false;
        }

        private void LateUpdate()
        {
            if (model == null || model.Pivot == null) return;
            float dt = Time.deltaTime;
            Transform pivot = model.Pivot;

            height = Mathf.SmoothDamp(height, HeightOffset, ref heightVel, 0.2f);
            pivot.localPosition = new Vector3(0f, height, 0f);

            // --- squash & stretch on the whole puppy ------------------------------
            squash = Mathf.SmoothDamp(squash, 0f, ref squashVel, 0.16f);
            float sy = 1f - squash;
            float sxz = 1f + squash * 0.55f;
            float bounce = model.IsRealModel ? 0f : Mathf.Sin(Time.time * 2.2f) * 0.02f;
            pivot.localScale = new Vector3(GrowthScale * sxz, GrowthScale * (sy + bounce), GrowthScale * sxz);

            // --- tail wag: frequency is live emotional telemetry --------------------
            excitement = Mathf.MoveTowards(excitement, 0f, dt * 0.35f);
            float mood = Mathf.Clamp01(Mood + excitement);
            float speed = Mathf.Lerp(3f, 17f, mood * mood);
            wagPhase += dt * speed;
            float amp = Mathf.Lerp(8f, 26f, mood) * WagAmount;
            for (int i = 0; i < model.Tail.Count; i++)
            {
                Transform t = model.Tail[i];
                if (t == null) continue;
                float a = Mathf.Sin(wagPhase - i * 0.7f) * amp * (i == 0 ? 1f : 0.7f);
                t.rotation = Quaternion.AngleAxis(a, pivot.up) * t.rotation;
            }

            // --- head follows the finger / the food ---------------------------------
            lookWeight = Mathf.MoveTowards(lookWeight, hasLook ? 1f : 0f, dt * 4f);
            if (lookWeight > 0.001f && model.Head != null && model.Nose != null)
            {
                Vector3 current = model.Nose.position - model.Head.position;
                Vector3 wanted = lookTarget - model.Head.position;
                if (current.sqrMagnitude > 1e-6f && wanted.sqrMagnitude > 1e-6f)
                {
                    Quaternion delta = Quaternion.FromToRotation(current.normalized, wanted.normalized);
                    float angle;
                    Vector3 axis;
                    delta.ToAngleAxis(out angle, out axis);
                    if (angle > 180f) angle -= 360f;
                    angle = Mathf.Clamp(angle, -40f, 40f) * lookWeight;
                    if (axis.sqrMagnitude > 1e-6f && !float.IsNaN(axis.x))
                        model.Head.rotation = Quaternion.AngleAxis(angle, axis) * model.Head.rotation;
                }
            }

            // --- floppy ears lag behind vertical head movement -----------------------
            if (model.Head != null && model.EarL != null && model.EarR != null && dt > 1e-5f)
            {
                float vy = (model.Head.position.y - lastHeadPos.y) / dt;
                lastHeadPos = model.Head.position;
                float target = Mathf.Clamp(-vy * 14f, -30f, 30f);
                earAngle = Mathf.SmoothDamp(earAngle, target, ref earVel, 0.12f);
                if (Mathf.Abs(earAngle) > 0.05f)
                {
                    model.EarL.rotation = Quaternion.AngleAxis(earAngle, pivot.forward) * model.EarL.rotation;
                    model.EarR.rotation = Quaternion.AngleAxis(-earAngle, pivot.forward) * model.EarR.rotation;
                }
            }
        }
    }
}
