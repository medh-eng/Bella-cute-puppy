using UnityEngine;
using Bella.Core;
using Bella.Needs;

namespace Bella.States
{
    /// <summary>
    /// Bella's default state. While idle, continuously checks need thresholds and
    /// plays the appropriate idle sub-behavior (tired/yawning, begging) - see
    /// docs/ARCHITECTURE.md section 2. Interaction/Locomotion states interrupt this
    /// one and return to it when they finish (wired up by PetStateMachine).
    ///
    /// NOTE: Animator parameter names ("IsTired", "IsBegging") are placeholders -
    /// align them with whatever the final Animator Controller uses once Bella's
    /// rig/animations are delivered (docs/ART_BIBLE.md section 7).
    /// </summary>
    public class IdleState : IState
    {
        private readonly PetNeeds needs;
        private readonly Animator animator;

        public IdleState(PetNeeds needs, Animator animator)
        {
            this.needs = needs;
            this.animator = animator;
        }

        public void Enter()
        {
            animator?.SetBool("IsTired", false);
            animator?.SetBool("IsBegging", false);
        }

        public void Tick()
        {
            if (needs == null || animator == null) return;

            animator.SetBool("IsTired", needs.IsEnergyLow);
            animator.SetBool("IsBegging", needs.IsHungerCritical);
        }

        public void Exit()
        {
            animator?.SetBool("IsTired", false);
            animator?.SetBool("IsBegging", false);
        }
    }
}
