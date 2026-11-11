using System;
using UnityEngine;
using Bella.Core;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// A timed reaction (being petted, eating, celebrating): sets a face and an
    /// animation, runs for a duration, then fires a completion callback.
    /// </summary>
    public class InteractionState : IState
    {
        private readonly FaceExpressionController face;
        private readonly BellaAnimator anim;
        private readonly Expression expression;
        private readonly string clip;
        private readonly float duration;
        private readonly Action onEnter;
        private readonly Action onComplete;
        private float elapsed;
        private bool done;

        public InteractionState(FaceExpressionController face, BellaAnimator anim, Expression expression,
            string clip, float duration, Action onComplete, Action onEnter = null)
        {
            this.face = face;
            this.anim = anim;
            this.expression = expression;
            this.clip = clip;
            this.duration = duration;
            this.onComplete = onComplete;
            this.onEnter = onEnter;
        }

        public void Enter()
        {
            elapsed = 0f;
            done = false;
            face.Set(expression);
            if (!string.IsNullOrEmpty(clip)) anim.Play(clip, 0.2f);
            if (onEnter != null) onEnter();
        }

        public void Tick()
        {
            if (done) return;
            elapsed += Time.deltaTime;
            if (elapsed >= duration)
            {
                done = true;
                if (onComplete != null) onComplete();
            }
        }

        public void Exit() { }
    }
}
