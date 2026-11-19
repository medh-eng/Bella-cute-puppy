using System;
using UnityEngine;
using Bella.Core;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Moves Bella to a world position with simple steering (turn, then move) — no
    /// NavMesh; one small room needs no pathfinding. Optionally turns to a final
    /// facing before reporting arrival.
    /// </summary>
    public class WalkToState : IState
    {
        private readonly Transform self;
        private readonly BellaAnimator anim;
        private readonly Vector3 target;
        private readonly float speed;
        private readonly bool run;
        private readonly float? finalYaw;
        private readonly Action onArrive;
        private bool arrived;
        private bool turning;
        private float elapsed;

        private const float ArriveDistance = 0.08f;

        /// <summary>Set by the Room: picks a way around furniture, and keeps feet out of it.</summary>
        public static Func<Vector3, Vector3, Vector3> Steer;
        public static Func<Vector3, Vector3> Clamp;

        public WalkToState(Transform self, BellaAnimator anim, Vector3 target, float speed, bool run,
            Action onArrive, float? finalYaw = null)
        {
            this.self = self;
            this.anim = anim;
            this.target = new Vector3(target.x, self.position.y, target.z);
            this.speed = speed;
            this.run = run;
            this.onArrive = onArrive;
            this.finalYaw = finalYaw;
        }

        public void Enter()
        {
            arrived = false;
            turning = false;
            elapsed = 0f;
            if ((target - self.position).magnitude > ArriveDistance)
            {
                anim.Speed = 1f;
                anim.Play(run ? "Run" : "Walk", 0.15f);
            }
        }

        public void Tick()
        {
            if (arrived) return;

            // never get stuck behind furniture: after a while she simply is there
            elapsed += Time.deltaTime;
            if (!turning && elapsed > 8f) { self.position = target; turning = true; }

            if (!turning)
            {
                Vector3 toTarget = target - self.position;
                toTarget.y = 0f;
                float dist = toTarget.magnitude;
                if (dist <= ArriveDistance)
                {
                    self.position = target;
                    turning = true;
                }
                else
                {
                    Vector3 aim = Steer != null ? Steer(self.position, target) : target;
                    Vector3 to = aim - self.position;
                    to.y = 0f;
                    float d = to.magnitude;
                    if (d < 0.02f) { to = toTarget; d = dist; }
                    Quaternion look = Quaternion.LookRotation(to / d, Vector3.up);
                    self.rotation = Quaternion.RotateTowards(self.rotation, look, 620f * Time.deltaTime);
                    // Slow down while still turning so she doesn't moonwalk sideways.
                    float facing = Mathf.Clamp01(Vector3.Dot(self.forward, to / d));
                    float step = Mathf.Min(d, speed * Mathf.Lerp(0.25f, 1f, facing) * Time.deltaTime);
                    self.position += (to / d) * step;
                    if (Clamp != null)
                    {
                        Vector3 q = Clamp(self.position);
                        self.position = new Vector3(q.x, self.position.y, q.z);
                    }
                    return;
                }
            }

            if (finalYaw.HasValue)
            {
                Quaternion want = Quaternion.Euler(0f, finalYaw.Value, 0f);
                self.rotation = Quaternion.RotateTowards(self.rotation, want, 540f * Time.deltaTime);
                if (Quaternion.Angle(self.rotation, want) > 2f) return;
            }

            arrived = true;
            if (onArrive != null) onArrive();
        }

        public void Exit()
        {
            anim.Speed = 1f;
        }
    }
}
