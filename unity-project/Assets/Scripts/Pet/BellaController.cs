using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.States;

namespace Bella.Pet
{
    /// <summary>
    /// Bella's "brain": owns the state objects and decides WHEN to transition
    /// (states themselves only do, never orchestrate — docs/ARCHITECTURE.md §2).
    /// Public methods are the whole gameplay API the UI and input layer call:
    /// PetBella(), RequestFeed(), RequestSleep().
    /// </summary>
    [RequireComponent(typeof(PetStateMachine))]
    [RequireComponent(typeof(PetNeeds))]
    public class BellaController : MonoBehaviour
    {
        [Header("Wired by the scene builder")]
        [SerializeField] private FaceExpressionController face;
        [SerializeField] private ProceduralPuppy puppy;
        [SerializeField] private Transform foodBowl;
        [SerializeField] private Transform bed;
        [SerializeField] private Animator animator; // optional until Phase 2

        [Header("Tuning")]
        [SerializeField] private float walkSpeed = 1.6f;
        [SerializeField] private float napSeconds = 5f;
        [SerializeField] private int treatsPerCare = 2;

        private PetStateMachine fsm;
        private PetNeeds needs;
        private IdleState idle;
        private bool busy; // true while walking/eating/sleeping — ignores new requests

        public PetNeeds Needs => needs;

        public void Wire(FaceExpressionController f, ProceduralPuppy p, Transform bowl, Transform bedT)
        {
            face = f; puppy = p; foodBowl = bowl; bed = bedT;
        }

        private void Awake()
        {
            fsm = GetComponent<PetStateMachine>();
            needs = GetComponent<PetNeeds>();
        }

        private void Start()
        {
            idle = new IdleState(needs, face, animator);
            fsm.ChangeState(idle);

            // Wake-up greeting: excitement scales with time away (capped — delight,
            // never guilt; docs/GDD.md §3).
            if (needs.LastOfflineSeconds > 600) // away > 10 minutes
                GreetPlayer();
        }

        private void Update()
        {
            if (puppy != null) puppy.Mood = needs.Mood01;
        }

        // ---- Gameplay API ------------------------------------------------------

        /// <summary>Tap/stroke Bella: joy burst + happiness.</summary>
        public void PetBella()
        {
            if (busy) { puppy?.PulseJoy(); return; } // always react a little, even mid-task
            needs.Pet();
            puppy?.PulseJoy();
            HeartBurst.Play(transform.position + Vector3.up * 0.9f);
            busy = true;
            fsm.ChangeState(new InteractionState(face, Expression.Excited, 1.2f, ReturnToIdle));
        }

        /// <summary>Feed button: Bella walks to the bowl, eats, gets happy.</summary>
        public void RequestFeed()
        {
            if (busy || foodBowl == null) return;
            busy = true;
            fsm.ChangeState(new WalkToState(transform, () => foodBowl.position, walkSpeed,
                onArrive: () => fsm.ChangeState(new InteractionState(
                    face, Expression.Excited, 2.0f,
                    onEnter: () =>
                    {
                        needs.Feed();
                        needs.AddHappiness(4f);
                        needs.AddTreats(treatsPerCare);
                        puppy?.PulseJoy();
                        HeartBurst.Play(foodBowl.position + Vector3.up * 0.6f);
                    },
                    onComplete: ReturnToIdle)),
                animator: animator));
        }

        /// <summary>Sleep button: Bella walks to the bed and naps.</summary>
        public void RequestSleep()
        {
            if (busy || bed == null) return;
            busy = true;
            fsm.ChangeState(new WalkToState(transform, () => bed.position, walkSpeed,
                onArrive: () => fsm.ChangeState(new SleepState(needs, face, napSeconds,
                    onWake: () =>
                    {
                        needs.AddTreats(treatsPerCare);
                        puppy?.PulseJoy();
                        ReturnToIdle();
                    })),
                animator: animator));
        }

        private void GreetPlayer()
        {
            busy = true;
            puppy?.PulseJoy();
            HeartBurst.Play(transform.position + Vector3.up * 0.9f);
            fsm.ChangeState(new InteractionState(face, Expression.Excited, 1.5f, ReturnToIdle));
        }

        private void ReturnToIdle()
        {
            busy = false;
            fsm.ChangeState(idle);
        }
    }
}
