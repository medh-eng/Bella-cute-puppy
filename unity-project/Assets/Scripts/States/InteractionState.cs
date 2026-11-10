using System;
using Bella.Core;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Interrupt-driven joyful reaction (petting, eating): sets an expression, runs
    /// for a short duration, fires a completion callback (BellaController returns
    /// Bella to Idle). docs/ARCHITECTURE.md section 2.
    /// </summary>
    public class InteractionState : IState
    {
        private readonly FaceExpressionController face;
        private readonly Expression expression;
        private readonly float duration;
        private readonly Action onEnter;
        private readonly Action onComplete;

        private float elapsed;

        public InteractionState(FaceExpressionController face, Expression expression,
                                float duration, Action onComplete, Action onEnter = null)
        {
            this.face = face;
            this.expression = expression;
            this.duration = duration;
            this.onComplete = onComplete;
            this.onEnter = onEnter;
        }

        public void Enter()
        {
            elapsed = 0f;
            face?.Set(expression);
            onEnter?.Invoke();
        }

        public void Tick()
        {
            elapsed += UnityEngine.Time.deltaTime;
            if (elapsed >= duration)
                onComplete?.Invoke();
        }

        public void Exit() { }
    }
}
