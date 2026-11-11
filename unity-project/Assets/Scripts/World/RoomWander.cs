using UnityEngine;
using Bella.Pet;

namespace Bella.World
{
    /// <summary>
    /// Idle life: every so often Bella trots to a new spot in the room. Pauses
    /// while she is busy, in a mini-game, or sleepy at night.
    /// </summary>
    public class RoomWander : MonoBehaviour
    {
        private BellaController bella;
        private Room room;
        private DayNight dayNight;
        private float timer = 9f;
        private bool walking;

        public void Init(BellaController b, Room r, DayNight d)
        {
            bella = b; room = r; dayNight = d;
        }

        /// <summary>Hold off wandering (the player is interacting).</summary>
        public void Pause(float seconds)
        {
            timer = Mathf.Max(timer, seconds);
        }

        private void Update()
        {
            if (bella == null || walking) return;
            if (bella.Busy || bella.InActivity) { timer = Mathf.Max(timer, 5f); return; }
            timer -= Time.deltaTime;
            if (timer > 0f) return;

            bool sleepy = (dayNight != null && dayNight.IsNight) || bella.Needs.IsEnergyLow;
            timer = Random.Range(9f, 18f) * (sleepy ? 2.2f : 1f);
            walking = bella.WalkToPoint(room.RandomFloorPoint(), () => walking = false);
        }

        private void LateUpdate()
        {
            // If a care action interrupted the walk, the arrival callback never fires.
            if (walking && bella != null && (bella.Busy || bella.InActivity)) walking = false;
        }
    }
}
