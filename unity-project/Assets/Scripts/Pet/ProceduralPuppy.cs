using UnityEngine;

namespace Bella.Pet
{
    /// <summary>
    /// The "cuteness engine" (docs/ARCHITECTURE.md section 4): transform-level
    /// secondary motion that works identically on the grey-box placeholder and the
    /// final rigged model's bones. This — not the preset clips — is what makes Bella
    /// feel alive: idle breathing, happiness-driven tail wag, squash-and-stretch on
    /// affection.
    /// </summary>
    public class ProceduralPuppy : MonoBehaviour
    {
        [Header("Wired by the scene builder / model setup")]
        [SerializeField] private Transform body;   // breathing bounce target
        [SerializeField] private Transform tail;   // wag target
        [SerializeField] private Transform head;   // tilt target (optional)

        [Header("Breathing")]
        [SerializeField] private float breatheAmount = 0.02f;
        [SerializeField] private float breatheSpeed = 2.2f;

        [Header("Tail wag (speed follows mood)")]
        [SerializeField] private float wagMinSpeed = 1.5f;
        [SerializeField] private float wagMaxSpeed = 14f;
        [SerializeField] private float wagAngle = 35f;

        /// <summary>0..1 — set each frame from PetNeeds.Mood01 by BellaController.</summary>
        public float Mood { get; set; } = 1f;

        private Vector3 bodyBaseScale = Vector3.one;
        private Quaternion tailBaseRot = Quaternion.identity;
        private float squash;      // >0 while a squash pulse is playing
        private float squashVel;
        private float excitement;  // extra wag burst after affection

        public void Wire(Transform bodyT, Transform tailT, Transform headT)
        {
            body = bodyT; tail = tailT; head = headT;
            CacheBase();
        }

        private void Start() => CacheBase();

        private void CacheBase()
        {
            if (body != null) bodyBaseScale = body.localScale;
            if (tail != null) tailBaseRot = tail.localRotation;
        }

        /// <summary>Joyful squash-and-stretch pulse — call on petting/feeding.</summary>
        public void PulseJoy()
        {
            squash = 0.25f;
            excitement = 1f;
        }

        private void Update()
        {
            float t = Time.time;

            // Breathing + squash share the body scale.
            if (body != null)
            {
                float breathe = Mathf.Sin(t * breatheSpeed) * breatheAmount;
                squash = Mathf.SmoothDamp(squash, 0f, ref squashVel, 0.18f);
                float sy = 1f + breathe - squash;          // squash down...
                float sxz = 1f + squash * 0.6f;            // ...bulge out
                body.localScale = new Vector3(
                    bodyBaseScale.x * sxz, bodyBaseScale.y * sy, bodyBaseScale.z * sxz);
            }

            // Tail wag — frequency is live emotional telemetry.
            if (tail != null)
            {
                excitement = Mathf.MoveTowards(excitement, 0f, Time.deltaTime * 0.4f);
                float moodBoosted = Mathf.Clamp01(Mood + excitement);
                float speed = Mathf.Lerp(wagMinSpeed, wagMaxSpeed, moodBoosted * moodBoosted);
                float angle = Mathf.Sin(t * speed) * wagAngle * Mathf.Lerp(0.4f, 1f, moodBoosted);
                tail.localRotation = tailBaseRot * Quaternion.Euler(0f, angle, 0f);
            }

            // Gentle curious head bob.
            if (head != null)
            {
                float tilt = Mathf.Sin(t * 0.7f) * 4f;
                head.localRotation = Quaternion.Euler(0f, 0f, tilt);
            }
        }
    }
}
