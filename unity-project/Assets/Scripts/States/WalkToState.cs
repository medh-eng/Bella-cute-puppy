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

        private const float ArriveDistance = 0.08f;

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
            if ((target - self.position).magnitude > ArriveDistance)
            {
                anim.Speed = 1f;
                anim.Play(run ? "Run" : "Walk", 0.15f);
            }
        }

        public void Tick()
        {
            if (arrived) return;

            if (!turning)
            {
                Vector3 to = target - self.position;
                to.y = 0f;
                float dist = to.magnitude;
                if (dist <= ArriveDistance)
                {
                    self.position = target;
                    turning = true;
                }
                else
                {
                    Quaternion look = Quaternion.LookRotation(to / dist, Vector3.up);
                    self.rotation = Quaternion.RotateTowards(self.rotation, look, 620f * Time.deltaTime);
                    // Slow down while still turning so she doesn't moonwalk sideways.
                    float facing = Mathf.Clamp01(Vector3.Dot(self.forward, to / dist));
                    float step = Mathf.Min(dist, speed * Mathf.Lerp(0.25f, 1f, facing) * Time.deltaTime);
                    self.position += (to / dist) * step;
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
