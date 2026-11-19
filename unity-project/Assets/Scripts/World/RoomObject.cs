using UnityEngine;

namespace Bella.World
{
    /// <summary>
    /// A piece of furniture or a toy in the room. It carries a box collider so taps can find it,
    /// a footprint radius (Bella walks around it, never through it) and a flag for whether the
    /// player may pick it up and put it somewhere else.
    /// </summary>
    public class RoomObject : MonoBehaviour
    {
        public string id = "";
        public float radius = 0.3f;     // footprint on the floor, metres
        public float height = 0.5f;
        public bool movable = true;
        public bool solid = true;       // Bella walks around it
        public Vector3 home;            // where it stands when nothing has been moved

        /// <summary>Where a visitor should stand to reach it: just outside the footprint, on the side facing the room's middle.</summary>
        public Vector3 StandPoint
        {
            get
            {
                Vector3 p = transform.localPosition;
                Vector3 d = new Vector3(-p.x, 0f, 0.15f - p.z);
                if (d.magnitude < 0.3f) d = new Vector3(0f, 0f, -1f);
                d.Normalize();
                return new Vector3(p.x, 0f, p.z) + d * (radius + 0.5f);
            }
        }
    }
}
