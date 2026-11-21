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
        private Coroutine hopRoutine;

        private const float WalkSpeed = 0.95f;
        private const float RunSpeed = 2.3f;
        private const float RewardBelow = 75f;   // care only pays treats when it was actually needed

        public Vector3 HeadPoint =>
            Model != null && Model.HeadCentre != null ? Model.HeadCentre.position : transform.position + Vector3.up * 0.7f;

        /// <summary>Where a ball sits when she holds it: just in front of her nose, a little below.</summary>
        public Vector3 MouthPoint
        {
            get
            {
                if (Model != null && Model.Nose != null)
                    return Model.Nose.position + transform.forward * 0.03f + Vector3.down * 0.075f;
                return HeadPoint + transform.forward * 0.17f + Vector3.down * 0.1f;
            }
        }

        // the ball she is holding in her mouth (living-room fetch); follows her head every frame
        private Transform heldBall;
        private Vector3 heldBallScale;
        private float heldBlend;

        public void HoldInMouth(Transform ball, Vector3 baseScale)
        {
            heldBall = ball; heldBallScale = baseScale; heldBlend = 0f;
        }

        public void ReleaseFromMouth() { heldBall = null; }

        private void LateUpdate()
        {
            if (heldBall == null) return;
            heldBlend = Mathf.Min(1f, heldBlend + Time.deltaTime * 6f);
            heldBall.position = Vector3.Lerp(heldBall.position, MouthPoint, heldBlend);
            heldBall.localScale = heldBallScale * Mathf.Lerp(1f, 0.8f, heldBlend);
        }

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

            // 3 cm above the floor: the rug discs top out at 1.2 cm, and a shadow at the same height flickered against them
            Art.GroundDisc("BlobShadow", transform, new Vector3(0f, 0.03f, 0f), 0.85f, new Color(0f, 0f, 0f, 0.22f));

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

        public void RequestFeed() { RequestFeed(null); }

        /// <summary>Feed with a specific food (Feeding shop); null = the plain default meal.</summary>
        public void RequestFeed(FoodDef food)
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
                Face.Munch(2.6f);               // mouth opens and closes with the sound
                Puppy.Chewing = true;
                bool wasHungry = Needs.Hunger < 30f;
                fsm.ChangeState(new InteractionState(Face, Anim, Expression.Soapy, "Eat", 2.6f, () =>
                {
                    Puppy.Chewing = false;
                    Face.StopMunch();
                    room.SetBowlFull(false);
                    float hunger = food != null ? food.hunger : (Needs.Config != null ? Needs.Config.feedAmount : 35f);
                    float happy = food != null ? food.happiness : 4f;
                    Needs.Feed(hunger);
                    Needs.AddHappiness(happy);
                    SaveSystem.Data.countMeals++;
                    if (happy >= 20f) AudioManager.Voice("bark_happy");
                    else if (wasHungry) AudioManager.Voice("whimper_hungry");
                    Celebrate(rewarded ? 2 : 0);
                    FinishCare("feed");
                    if (UnityEngine.Random.value < 0.6f) StartCoroutine(DrinkSoon());
                }));
            }, yaw));
        }

        /// <summary>Walk to the water bowl and lap water: head down, mouth moving, the real lapping sound.</summary>
        public void RequestDrink()
        {
            if (Busy || InActivity || room == null) return;
            RoomObject bowl = room.FindObject("waterbowl");
            if (bowl == null) return;
            Busy = true;
            AudioManager.Play("tap", 0.5f);
            Vector3 at = bowl.transform.position;
            Puppy.LookAt(at + Vector3.up * 0.1f);
            Vector3 stand = bowl.StandPoint;
            fsm.ChangeState(new WalkToState(transform, Anim, stand, RunSpeed * 0.7f, true, () =>
            {
                const float seconds = 3.2f;
                Puppy.ClearLook();
                Face.Munch(seconds);
                Puppy.Chewing = true;
                StartCoroutine(LapSounds(seconds));
                fsm.ChangeState(new InteractionState(Face, Anim, Expression.Soapy, "Eat", seconds, () =>
                {
                    Puppy.Chewing = false;
                    Face.StopMunch();
                    Needs.AddHappiness(3f);
                    AudioManager.Play("pant", 0.35f);
                    FinishCare("drink");
                }));
            }, YawTowards(stand, at)));
        }

        private System.Collections.IEnumerator DrinkSoon()
        {
            yield return new WaitForSeconds(2.6f);       // after the "well done" beat
            if (!Busy && !InActivity && !IsSleeping) RequestDrink();
        }

        private System.Collections.IEnumerator LapSounds(float seconds)
        {
            // lap.wav is ~1.5 s of lapping: play it back to back for the whole drink
            for (float t = 0f; t < seconds - 0.4f; t += 1.45f)
            {
                AudioManager.Play("lap", 0.85f, UnityEngine.Random.Range(0.96f, 1.04f));
                yield return new WaitForSeconds(1.45f);
            }
        }

        public void RequestSleep()
        {
            if (Busy || InActivity || room == null) return;
            RoomObject bed = room.FindObject("bed");
            if (bed == null) return;
            Busy = true;
            bool rewarded = Needs.Energy < RewardBelow;
            AudioManager.Play("tap");
            AudioManager.Voice("yawn_sleepy");

            // walk to the bed's edge, face it, HOP up onto the cushion, then curl up
            Vector3 stand = bed.StandPoint;
            fsm.ChangeState(new WalkToState(transform, Anim, stand, WalkSpeed, false, () =>
            {
                AudioManager.Play("whoosh", 0.4f);
                HopTo(new Vector3(bed.transform.localPosition.x, 0f, bed.transform.localPosition.z), 0f, SleepState.BedHeight, 0.32f, 0.7f, 180f, () =>
                {
                    if (GameRoot.Instance != null) GameRoot.Instance.SetSleepMode(true);
                    float nap = Mathf.Lerp(4f, 9f, Mathf.Clamp01((100f - Needs.Energy) / 100f));
                    sleepState = new SleepState(Needs, Face, Anim, Puppy, Model.HeadCentre, nap, () =>
                    {
                        sleepState = null;
                        if (GameRoot.Instance != null) GameRoot.Instance.SetSleepMode(false);
                        SaveSystem.Data.countNaps++;
                        // hop back down beside the bed
                        Vector3 down = bed.StandPoint;
                        HopTo(down, SleepState.BedHeight, 0f, 0.22f, 0.55f, YawTowards(down, bed.transform.localPosition) + 180f, () =>
                        {
                            AudioManager.Voice("yip");
                            Celebrate(rewarded ? 2 : 0);
                            FinishCare("sleep");
                        });
                    });
                    fsm.ChangeState(sleepState);
                });
            }, YawTowards(stand, bed.transform.localPosition)));
        }

        /// <summary>
        /// A hop in an arc to a floor point: used to jump onto and off the bed. lift0 / lift1 are the heights
        /// she starts and ends at (the cushion is higher than the floor), apex is the extra height at the top.
        /// </summary>
        private void HopTo(Vector3 groundPos, float lift0, float lift1, float apex, float seconds, float endYaw, Action done)
        {
            if (hopRoutine != null) StopCoroutine(hopRoutine);
            hopRoutine = StartCoroutine(HopRoutine(groundPos, lift0, lift1, apex, seconds, endYaw, done));
        }

        private System.Collections.IEnumerator HopRoutine(Vector3 groundPos, float lift0, float lift1, float apex,
            float seconds, float endYaw, Action done)
        {
            fsm.ChangeState(null);
            Anim.Play("Happy", 0.1f);
            Puppy.SnapHeight(0f);
            Puppy.HopLift = lift0;
            Vector3 from = transform.position;
            Vector3 to = new Vector3(groundPos.x, from.y, groundPos.z);
            Quaternion r0 = transform.rotation;
            Quaternion r1 = Quaternion.Euler(0f, endYaw, 0f);
            for (float e = 0f; e < seconds; e += Time.deltaTime)
            {
                float k = Mathf.Clamp01(e / seconds);
                float ease = k * k * (3f - 2f * k);
                transform.position = Vector3.Lerp(from, to, ease);
                transform.rotation = Quaternion.Slerp(r0, r1, Mathf.Clamp01(k * 1.6f));
                Puppy.HopLift = Mathf.Lerp(lift0, lift1, ease) + Mathf.Sin(k * Mathf.PI) * apex;
                yield return null;
            }
            transform.position = to;
            transform.rotation = r1;
            Puppy.HopLift = 0f;
            Puppy.SnapHeight(lift1);
            hopRoutine = null;
            if (done != null) done();
        }

        /// <summary>Tap on a piece of furniture: she walks over, looks at it and reacts.</summary>
        public void RequestVisit(Vector3 stand, Vector3 lookAt, string clip, Expression expression, string sfx, float seconds, bool munch)
        {
            if (Busy || InActivity || room == null) return;
            Busy = true;
            AudioManager.Play("tap", 0.5f);
            Puppy.LookAt(lookAt + Vector3.up * 0.15f);
            fsm.ChangeState(new WalkToState(transform, Anim, stand, RunSpeed * 0.8f, true, () =>
            {
                if (!string.IsNullOrEmpty(sfx)) AudioManager.Play(sfx, 0.7f);
                if (munch) { Face.Munch(seconds); Puppy.Chewing = true; }
                fsm.ChangeState(new InteractionState(Face, Anim, expression, clip, seconds, () =>
                {
                    Puppy.Chewing = false;
                    Face.StopMunch();
                    Puppy.ClearLook();
                    Needs.AddHappiness(1.5f);
                    Busy = false;
                    fsm.ChangeState(idle);
                }));
            }, YawTowards(stand, lookAt)));
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
            Vector3 landing = room.RandomFloorPointAwayFrom(transform.position, 1.2f);
            Face.Set(Expression.Excited);
            AudioManager.Play("whoosh");
            AudioManager.Voice("bark");
            Puppy.LookAt(landing + Vector3.up * 0.4f);
            room.ThrowBall(landing, () =>
            {
                Puppy.ClearLook();
                Vector3 dir = (landing - transform.position);
                dir.y = 0f;
                Vector3 stop = landing - dir.normalized * 0.72f;   // stops in front of the ball, not on top of it
                fsm.ChangeState(new WalkToState(transform, Anim, stop, RunSpeed, true, () =>
                {
                    HoldInMouth(room.BallTransform, room.BallBaseScale);
                    AudioManager.Play("tap", 0.7f);
                    fsm.ChangeState(new InteractionState(Face, Anim, Expression.Excited, "Idle", 0.5f, () =>
                    {
                        Face.Set(Expression.Happy);
                        fsm.ChangeState(new WalkToState(transform, Anim, room.HomePoint, RunSpeed * 0.85f, true, () =>
                        {
                            ReleaseFromMouth();
                            room.ResetBall();
                            AudioManager.Play("chime", 0.6f);
                            Needs.AddHappiness(14f);
                            Needs.Tire(6f);
                            SaveSystem.Data.countFetches++;
                            Celebrate(Progress.PlayTreats(1));
                            AudioManager.Play("pant", 0.55f);   // out of breath after the run
                            FinishCare("fetch");
                        }, 180f));
                    }));
                }));
            });
        }

        /// <summary>Run speed, shared with mini-games that drive her with their own states.</summary>
        public float Run => RunSpeed;

        /// <summary>Lets a mini-game put Bella into a state of its own (only while it owns her).</summary>
        public void RunState(IState state)
        {
            fsm.ChangeState(state);
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
