using System;
using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Nap on the bed: eyes closed, slow breathing, floating Zzz, energy refilling
    /// over the nap. Ends by itself when she is rested, or early when the player
    /// taps her awake (BellaController.WakeUp).
    /// </summary>
    public class SleepState : IState
    {
        private readonly PetNeeds needs;
        private readonly FaceExpressionController face;
        private readonly BellaAnimator anim;
        private readonly ProceduralPuppy puppy;
        private readonly Transform zzzPoint;
        private readonly float napSeconds;
        private readonly Action onWake;

        private float elapsed;
        private float energyPerSecond;
        private float zzzTimer;
        private bool woke;

        public SleepState(PetNeeds needs, FaceExpressionController face, BellaAnimator anim, ProceduralPuppy puppy,
            Transform zzzPoint, float napSeconds, Action onWake)
        {
            this.needs = needs;
            this.face = face;
            this.anim = anim;
            this.puppy = puppy;
            this.zzzPoint = zzzPoint;
            this.napSeconds = napSeconds;
            this.onWake = onWake;
        }

        public void Enter()
        {
            elapsed = 0f;
            woke = false;
            zzzTimer = 0.8f;
            face.Set(Expression.Soapy);       // closed, content eyes
            anim.Play("Sleep", 0.6f);
            puppy.WagAmount = 0f;
            float missing = Mathf.Max(0f, 100f - needs.Energy);
            energyPerSecond = napSeconds > 0f ? missing / napSeconds : missing;
            needs.DecayPaused = true;
        }

        public void Tick()
        {
            if (woke) return;
            elapsed += Time.deltaTime;
            needs.Rest(energyPerSecond * Time.deltaTime);

            zzzTimer -= Time.deltaTime;
            if (zzzTimer <= 0f)
            {
                zzzTimer = 2.2f;
                if (zzzPoint != null) Fx.Zzz(zzzPoint.position + Vector3.up * 0.25f);
                AudioManager.Play("snore", 0.35f);
            }

            if (elapsed >= napSeconds) Wake();
        }

        public void Wake()
        {
            if (woke) return;
            woke = true;
            if (onWake != null) onWake();
        }

        public void Exit()
        {
            puppy.WagAmount = 1f;
            needs.DecayPaused = false;
        }
    }
}
