using System;
using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Save;
using Bella.States;
using Bella.Systems;
using Bella.World;

namespace Bella.Pet
{
    /// <summary>
    /// Bella's "brain": owns the state objects and decides WHEN to transition
    /// (states only do, never orchestrate — docs/ARCHITECTURE.md §2). Its public
    /// methods are the whole gameplay API used by the UI, input and mini-games.
    /// </summary>
    public class BellaController : MonoBehaviour
    {
        public PetNeeds Needs { get; private set; }
        public BellaModel Model { get; private set; }
        public BellaAnimator Anim { get; private set; }
        public FaceExpressionController Face { get; private set; }
        public ProceduralPuppy Puppy { get; private set; }
        public Accessories Outfits { get; private set; }

        /// <summary>True while walking / eating / sleeping / fetching — new care requests are ignored.</summary>
        public bool Busy { get; private set; }

        /// <summary>True while a mini-game owns Bella.</summary>
        public bool InActivity { get; private set; }

        public bool IsSleeping => sleepState != null;

        public event Action OnPetted;
        public event Action<string> OnCareDone;   // "feed", "sleep", "fetch"

        private PetStateMachine fsm;
        private Room room;
        private IdleState idle;
        private SleepState sleepState;
        private float petCooldown;

        private const float WalkSpeed = 0.95f;
        private const float RunSpeed = 2.3f;
        private const float RewardBelow = 75f;   // care only pays treats when it was actually needed

        public Vector3 HeadPoint =>
            Model != null && Model.HeadCentre != null ? Model.HeadCentre.position : transform.position + Vector3.up * 0.7f;

        public void Init(PetNeeds needs, Room roomRef)
        {
            Needs = needs;
            room = roomRef;
            fsm = gameObject.AddComponent<PetStateMachine>();

            Model = gameObject.AddComponent<BellaModel>();
            Model.Build();

            Anim = gameObject.AddComponent<BellaAnimator>();
            Anim.Init(Model.Animator, Model.Clip);

            Face = gameObject.AddComponent<FaceExpressionController>();
            Face.Init(Model.FaceRenderer, Model.IsRealModel);

            Puppy = gameObject.AddComponent<ProceduralPuppy>();
            Puppy.Init(Model);

            Outfits = gameObject.AddComponent<Accessories>();
            Outfits.Init(Model, SaveSystem.Data.equippedOutfits);

            // One capsule for tap / stroke raycasts.
            CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
            col.center = new Vector3(0f, 0.5f, 0.05f);
            col.radius = 0.36f;
            col.height = 1.05f;

            Art.GroundDisc("BlobShadow", transform, new Vector3(0f, 0.012f, 0f), 0.85f, new Color(0f, 0f, 0f, 0.22f));

            idle = new IdleState(Needs, Face, Anim, transform);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            fsm.ChangeState(idle);
        }

        private void Update()
        {
            if (Needs == null) return;
            Puppy.Mood = Needs.Mood01;
            if (petCooldown > 0f) petCooldown -= Time.deltaTime;
        }

        // ---------------------------------------------------------------- petting

        /// <summary>Tap or stroke Bella: joy burst + happiness. Always reacts a little, even mid-task.</summary>
        public void PetBella(Vector3 hitPoint)
        {
            if (IsSleeping) { WakeUp(); return; }
            if (petCooldown > 0f) return;
            petCooldown = 0.32f;

            Needs.Pet();
            Puppy.PulseJoy();
            Fx.Hearts(hitPoint + Vector3.up * 0.15f, 4);
            AudioManager.Play("pop", 0.6f, UnityEngine.Random.Range(0.9f, 1.3f));
            if (UnityEngine.Random.value < 0.3f) AudioManager.Voice("yip");

            SaveSystem.Data.countPets++;
            if (OnPetted != null) OnPetted();

            if (!Busy && !InActivity)
                Face.Flash(Expression.Excited, 0.9f);
        }

        // ---------------------------------------------------------------- care loops

        public void RequestFeed()
        {
            if (Busy || InActivity || room == null) return;
            Busy = true;
            bool rewarded = Needs.Hunger < RewardBelow;
            room.SetBowlFull(true);
            AudioManager.Play("tap");
            Puppy.LookAt(room.BowlPosition);

            Vector3 stand = room.BowlStandPoint;
            float yaw = YawTowards(stand, room.BowlPosition);
            fsm.ChangeState(new WalkToState(transform, Anim, stand, RunSpeed * 0.8f, true, () =>
            {
                Puppy.ClearLook();
                AudioManager.Play("munch");
                fsm.ChangeState(new InteractionState(Face, Anim, Expression.Soapy, "Eat", 2.6f, () =>
                {
                    room.SetBowlFull(false);
                    Needs.Feed();
                    Needs.AddHappiness(4f);
                    SaveSystem.Data.countMeals++;
                    Celebrate(rewarded ? 2 : 0);
                    FinishCare("feed");
                }));
            }, yaw));
        }

        public void RequestSleep()
        {
            if (Busy || InActivity || room == null) return;
            Busy = true;
            bool rewarded = Needs.Energy < RewardBelow;
            AudioManager.Play("tap");

            fsm.ChangeState(new WalkToState(transform, Anim, room.BedPosition, WalkSpeed, false, () =>
            {
                if (GameRoot.Instance != null) GameRoot.Instance.SetSleepMode(true);
                float nap = Mathf.Lerp(4f, 9f, Mathf.Clamp01((100f - Needs.Energy) / 100f));
                sleepState = new SleepState(Needs, Face, Anim, Puppy, Model.HeadCentre, nap, () =>
                {
                    sleepState = null;
                    if (GameRoot.Instance != null) GameRoot.Instance.SetSleepMode(false);
                    SaveSystem.Data.countNaps++;
                    AudioManager.Voice("yip");
                    Celebrate(rewarded ? 2 : 0);
                    FinishCare("sleep");
                });
                fsm.ChangeState(sleepState);
            }, 180f));
        }

        public void WakeUp()
        {
            if (sleepState != null) sleepState.Wake();
        }

        public void RequestFetch()
        {
            if (Busy || InActivity || room == null) return;
            if (Needs.Energy < 8f)
            {
                // Too sleepy to play: a gentle yawn, never a refusal that feels like failure.
                Face.Flash(Expression.Sleepy, 1.6f);
                AudioManager.Voice("whine");
                Anim.Play("Sit", 0.3f);
                return;
            }
            Busy = true;
            Vector3 landing = room.RandomFloorPoint(0.5f);
            Face.Set(Expression.Excited);
            AudioManager.Play("whoosh");
            AudioManager.Voice("bark");
            Puppy.LookAt(landing + Vector3.up * 0.4f);
            room.ThrowBall(landing, () =>
            {
                Puppy.ClearLook();
                Vector3 dir = (landing - transform.position);
                dir.y = 0f;
                Vector3 stop = landing - dir.normalized * 0.32f;
                fsm.ChangeState(new WalkToState(transform, Anim, stop, RunSpeed, true, () =>
                {
                    room.HideBall();
                    fsm.ChangeState(new InteractionState(Face, Anim, Expression.Excited, "Happy", 1.1f, () =>
                    {
                        fsm.ChangeState(new WalkToState(transform, Anim, room.HomePoint, RunSpeed, true, () =>
                        {
                            room.ResetBall();
                            Needs.AddHappiness(14f);
                            Needs.Tire(6f);
                            SaveSystem.Data.countFetches++;
                            Celebrate(Progress.PlayTreats(1));
                            FinishCare("fetch");
                        }, 180f));
                    }));
                }));
            });
        }

        /// <summary>Wander helper — walks to a point unless she is busy.</summary>
        public bool WalkToPoint(Vector3 point, Action onArrived)
        {
            if (Busy || InActivity) return false;
            fsm.ChangeState(new WalkToState(transform, Anim, point, WalkSpeed, false, () =>
            {
                fsm.ChangeState(idle);
                if (onArrived != null) onArrived();
            }));
            return true;
        }

        private void FinishCare(string what)
        {
            Busy = false;
            fsm.ChangeState(new InteractionState(Face, Anim, Expression.Excited, "Happy", 1.0f, () =>
            {
                if (!Busy && !InActivity) fsm.ChangeState(idle);
            }));
            Needs.Save();
            if (OnCareDone != null) OnCareDone(what);
        }

        /// <summary>Hearts + squash + optional treats: the standard "well done" beat.</summary>
        public void Celebrate(int treats)
        {
            Puppy.PulseJoy();
            Fx.Hearts(HeadPoint + Vector3.up * 0.3f, 8);
            AudioManager.Play("chime", 0.8f);
            if (treats > 0) Needs.AddTreats(treats);
        }

        // ---------------------------------------------------------------- mini-game control

        /// <summary>A mini-game takes over: Bella walks to a spot, turns, and holds a pose.</summary>
        public void BeginActivity(Vector3 spot, float yaw, string holdClip, Action ready)
        {
            WakeUp();
            InActivity = true;
            Busy = true;
            Needs.DecayPaused = true;
            Puppy.ClearLook();
            fsm.ChangeState(new WalkToState(transform, Anim, spot, RunSpeed, true, () =>
            {
                fsm.ChangeState(null);
                Anim.Play(holdClip, 0.3f);
                if (ready != null) ready();
            }, yaw));
        }

        public void EndActivity()
        {
            InActivity = false;
            Busy = false;
            Needs.DecayPaused = false;
            Puppy.ClearLook();
            Anim.Speed = 1f;
            fsm.ChangeState(idle);
            Needs.Save();
        }

        /// <summary>Perform a trick (mini-game or on request). Looping tricks are held for def.hold seconds.</summary>
        public void PerformTrick(TrickDef def, Action done)
        {
            if (def == null) { if (done != null) done(); return; }
            Face.Set(Expression.Excited);
            if (def.loop)
            {
                fsm.ChangeState(new InteractionState(Face, Anim, Expression.Excited, def.clip, def.hold, done));
            }
            else
            {
                fsm.ChangeState(null);
                Anim.PlayOnce(def.clip, done);
            }
        }

        public void PlayPose(string clip, Expression expression)
        {
            fsm.ChangeState(null);
            Face.Set(expression);
            Anim.Play(clip, 0.3f);
        }

        public void PlayOnce(string clip, Action done)
        {
            fsm.ChangeState(null);
            Anim.PlayOnce(clip, done);
        }

        /// <summary>Big hello after time away: excitement scales with hours gone (capped — delight, never guilt).</summary>
        public void Greet(double secondsAway)
        {
            if (Busy || InActivity) return;
            float excitement = Mathf.Clamp01((float)(secondsAway / (6 * 3600.0)));
            Busy = true;
            AudioManager.Voice("bark");
            Puppy.PulseJoy();
            Fx.Hearts(HeadPoint + Vector3.up * 0.3f, 6 + Mathf.RoundToInt(excitement * 10f));
            string clip = excitement > 0.5f ? "Spin" : "Happy";
            if (clip == "Spin")
            {
                fsm.ChangeState(null);
                Face.Set(Expression.Excited);
                Anim.PlayOnce("Spin", () => { Busy = false; fsm.ChangeState(idle); });
            }
            else
            {
                fsm.ChangeState(new InteractionState(Face, Anim, Expression.Excited, "Happy", 1.6f, () =>
                {
                    Busy = false;
                    fsm.ChangeState(idle);
                }));
            }
        }

        private static float YawTowards(Vector3 from, Vector3 to)
        {
            Vector3 d = to - from;
            d.y = 0f;
            if (d.sqrMagnitude < 1e-6f) return 180f;
            return Quaternion.LookRotation(d.normalized, Vector3.up).eulerAngles.y;
        }
    }
}
