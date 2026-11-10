using System;
using UnityEngine;
using Bella.Core;

namespace Bella.States
{
    /// <summary>
    /// Moves Bella to a world position with simple steering (turn + move) — no
    /// NavMesh; one rectangular room needs no pathfinding (docs/ARCHITECTURE.md
    /// section 2). Fires onArrive when within the threshold.
    /// </summary>
    public class WalkToState : IState
    {
        private readonly Transform self;
        private readonly Func<Vector3> targetGetter;
        private readonly float speed;
        private readonly float turnSpeed;
        private readonly Action onArrive;
        private readonly Animator animator; // optional

        private const float ArriveDistance = 0.45f;

        public WalkToState(Transform self, Func<Vector3> targetGetter, float speed,
                           Action onArrive, Animator animator = null, float turnSpeed = 540f)
        {
            this.self = self;
            this.targetGetter = targetGetter;
            this.speed = speed;
            this.onArrive = onArrive;
            this.animator = animator;
            this.turnSpeed = turnSpeed;
        }

        public void Enter() => animator?.SetBool("IsWalking", true);

        public void Tick()
        {
            Vector3 target = targetGetter();
            target.y = self.position.y; // stay on the floor plane

            Vector3 to = target - self.position;
            if (to.magnitude <= ArriveDistance)
            {
                onArrive?.Invoke();
                return;
            }

            // Turn toward target, then move forward — reads as "walking", not sliding.
            Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
            self.rotation = Quaternion.RotateTowards(self.rotation, look, turnSpeed * Time.deltaTime);
            self.position += self.forward * (speed * Time.deltaTime);
        }

        public void Exit() => animator?.SetBool("IsWalking", false);
    }
}
