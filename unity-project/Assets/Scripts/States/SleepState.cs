using System;
using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Nap on the bed: sleepy face, energy refills over the nap duration, then wakes
    /// up excited (docs/GDD.md section 4 — Energy loop). The on-screen nap is short
    /// and celebratory; real overnight recovery comes from offline time.
    /// </summary>
    public class SleepState : IState
    {
        private readonly PetNeeds needs;
        private readonly FaceExpressionController face;
        private readonly float napSeconds;
        private readonly Action onWake;

        private float elapsed;
        private float energyPerSecond;

        public SleepState(PetNeeds needs, FaceExpressionController face, float napSeconds, Action onWake)
        {
            this.needs = needs;
            this.face = face;
            this.napSeconds = napSeconds;
            this.onWake = onWake;
        }

        public void Enter()
        {
            elapsed = 0f;
            face?.Set(Expression.Sleepy);
            float target = needs.Config != null ? needs.Config.sleepRestoresTo : 100f;
            float missing = Mathf.Max(0f, target - needs.Energy);
            energyPerSecond = napSeconds > 0f ? missing / napSeconds : missing;
        }

        public void Tick()
        {
            elapsed += Time.deltaTime;
            needs.Rest(energyPerSecond * Time.deltaTime);
            if (elapsed >= napSeconds)
            {
                face?.Set(Expression.Excited);
                onWake?.Invoke();
            }
        }

        public void Exit() { }
    }
}
