using UnityEngine;

namespace Bella.Core
{
    /// <summary>
    /// Drives Bella's behavior by holding exactly one active IState at a time and
    /// delegating Unity's Update loop to it. Attach this to the Bella GameObject.
    ///
    /// Other scripts (e.g. an input handler, or PetNeeds noticing a critical need)
    /// call ChangeState(...) to request a transition; this class does not decide
    /// WHEN to transition, only HOW to transition cleanly (Exit old -> Enter new).
    /// </summary>
    public class PetStateMachine : MonoBehaviour
    {
        public IState CurrentState { get; private set; }

        /// <summary>
        /// Swap the active state. Safe to call with the same state twice (no-op)
        /// to avoid accidentally re-triggering Enter() animations/sounds.
        /// </summary>
        public void ChangeState(IState newState)
        {
            if (newState == CurrentState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        private void Update()
        {
            CurrentState?.Tick();
        }
    }
}
