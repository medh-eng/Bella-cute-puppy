using System;
using UnityEngine;
using Bella.Core;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Bella chases a moving target (the ball) with steering that re-aims every frame, like a
    /// real puppy: she leads the ball a little, slows down as she gets close, and only reports
    /// "reached" once she is next to it and the ball is calm enough to grab.
    /// </summary>
    public class FetchChaseState : IState
    {
        private readonly Transform self;
        private readonly BellaAnimator anim;
        private readonly Func<Vector3> target;      // ball position on the floor
        private readonly Func<bool> canGrab;        // ball is low and nearly still
        private readonly float speed;
        private readonly float timeout;
        private readonly Action onReach;
        private float elapsed;
        private bool done;

        private const float GrabDistance = 0.30f;

        public FetchChaseState(Transform self, BellaAnimator anim, Func<Vector3> target, Func<bool> canGrab,
            float speed, Action onReach, float timeout = 14f)
        {
            this.self = self;
            this.anim = anim;
            this.target = target;
            this.canGrab = canGrab;
            this.speed = speed;
            this.onReach = onReach;
            this.timeout = timeout;
        }

        public void Enter()
        {
            elapsed = 0f;
            done = false;
            anim.Speed = 1f;
            anim.Play("Run", 0.12f);
        }

        public void Tick()
        {
            if (done) return;
            elapsed += Time.deltaTime;

            Vector3 goal = target();
            Vector3 to = goal - self.position;
            to.y = 0f;
            float dist = to.magnitude;

            if ((dist <= GrabDistance && canGrab()) || elapsed >= timeout)
            {
                done = true;
                if (onReach != null) onReach();
                return;
            }

            if (dist > 0.001f)
            {
                Vector3 dir = to / dist;
                Quaternion look = Quaternion.LookRotation(dir, Vector3.up);
                self.rotation = Quaternion.RotateTowards(self.rotation, look, 700f * Time.deltaTime);
                float facing = Mathf.Clamp01(Vector3.Dot(self.forward, dir));
                // quick across the room, gentle for the last few centimetres
                float pace = Mathf.Clamp(dist / 0.9f, 0.35f, 1f);
                float step = Mathf.Min(dist, speed * pace * Mathf.Lerp(0.3f, 1f, facing) * Time.deltaTime);
                if (dist > GrabDistance * 0.6f) self.position += dir * step;
            }
        }

        public void Exit()
        {
            anim.Speed = 1f;
        }
    }
}
