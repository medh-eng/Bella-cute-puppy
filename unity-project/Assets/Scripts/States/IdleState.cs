using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Bella's default state. Watches need thresholds and drives the matching idle
    /// expression + animator flags (docs/ARCHITECTURE.md section 2). Interaction /
    /// WalkTo / Sleep states interrupt this one and return when finished.
    /// Animator is optional (null on the grey-box placeholder).
    /// </summary>
    public class IdleState : IState
    {
        private readonly PetNeeds needs;
        private readonly FaceExpressionController face;
        private readonly Animator animator; // may be null until Phase 2

        public IdleState(PetNeeds needs, FaceExpressionController face, Animator animator = null)
        {
            this.needs = needs;
            this.face = face;
            this.animator = animator;
        }

        public void Enter()
        {
            if (animator != null)
            {
                animator.SetBool("IsTired", false);
                animator.SetBool("IsBegging", false);
            }
        }

        public void Tick()
        {
            if (needs == null) return;

            // Priority order: the most urgent need owns the face.
            Expression e =
                needs.IsHungerCritical  ? Expression.Begging :
                needs.IsEnergyLow       ? Expression.Sleepy :
                needs.IsHappinessLow    ? Expression.Sad :
                                          Expression.Happy;
            face?.Set(e);

            if (animator != null)
            {
                animator.SetBool("IsTired", needs.IsEnergyLow);
                animator.SetBool("IsBegging", needs.IsHungerCritical);
            }
        }

        public void Exit()
        {
            if (animator != null)
            {
                animator.SetBool("IsTired", false);
                animator.SetBool("IsBegging", false);
            }
        }
    }
}
