namespace Bella.Core
{
    /// <summary>
    /// Common contract for every behavioral state in Bella's Finite State Machine.
    /// See docs/ARCHITECTURE.md section 2 for the design rationale.
    /// </summary>
    public interface IState
    {
        /// <summary>Called once when the state machine transitions into this state.</summary>
        void Enter();

        /// <summary>Called every frame while this state is active.</summary>
        void Tick();

        /// <summary>Called once when the state machine transitions out of this state.</summary>
        void Exit();
    }
}
