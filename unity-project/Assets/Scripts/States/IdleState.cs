using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;

namespace Bella.States
{
    /// <summary>
    /// Bella's default state. The most urgent need owns her face; every few
    /// seconds she picks a little "life" behaviour (sit, play-bow, a sneeze when
    /// she has the sniffles) and she turns to face the player.
    /// </summary>
    public class IdleState : IState
    {
        private readonly PetNeeds needs;
        private readonly FaceExpressionController face;
        private readonly BellaAnimator anim;
        private readonly Transform self;

        private float behaviourTimer;
        private float faceYaw = 180f;

        public IdleState(PetNeeds needs, FaceExpressionController face, BellaAnimator anim, Transform self)
        {
            this.needs = needs;
            this.face = face;
            this.anim = anim;
            this.self = self;
        }

        public void Enter()
        {
            anim.Speed = 1f;
            anim.Play("Idle", 0.25f);
            behaviourTimer = Random.Range(4f, 8f);
            faceYaw = 180f + Random.Range(-18f, 18f);
        }

        public void Tick()
        {
            Expression e =
                needs.IsUnwell ? Expression.Sad :
                needs.IsHungerCritical ? Expression.Begging :
                needs.IsEnergyLow ? Expression.Sleepy :
                needs.IsHappinessLow ? Expression.Sad :
                Expression.Happy;
            face.Set(e);

            // Turn to look at the player (the camera sits on the -Z side of the room).
            Quaternion want = Quaternion.Euler(0f, faceYaw, 0f);
            self.rotation = Quaternion.RotateTowards(self.rotation, want, 240f * Time.deltaTime);

            behaviourTimer -= Time.deltaTime;
            if (behaviourTimer > 0f) return;
            behaviourTimer = Random.Range(5f, 10f);

            if (needs.IsUnwell && Random.value < 0.6f)
            {
                AudioManager.Voice("sneeze");
                anim.PlayOnce("Sneeze", () => anim.Play("Idle", 0.25f));
                return;
            }
            if (needs.IsEnergyLow || needs.IsHungerCritical)
            {
                anim.Play("Sit", 0.4f);
                return;
            }
            float r = Random.value;
            if (r < 0.35f) anim.Play("Sit", 0.4f);
            else if (r < 0.55f && needs.Mood01 > 0.6f) anim.Play("PlayBow", 0.35f);
            else anim.Play("Idle", 0.35f);
        }

        public void Exit() { }
    }
}
