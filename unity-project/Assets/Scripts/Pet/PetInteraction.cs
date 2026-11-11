using UnityEngine;
using Bella.Core;

namespace Bella.Pet
{
    /// <summary>
    /// Tap or stroke Bella to pet her. While the finger is down near her she
    /// watches it (head follows). Disabled automatically during mini-games, which
    /// read the pointer themselves.
    /// </summary>
    public class PetInteraction : MonoBehaviour
    {
        private Camera cam;
        private BellaController bella;
        private bool startedOnUI;

        public void Init(Camera c, BellaController b)
        {
            cam = c;
            bella = b;
        }

        private void Update()
        {
            if (cam == null || bella == null) return;
            if (bella.InActivity) return;

            if (Pointer.Down) startedOnUI = Pointer.OverUI;

            if (Pointer.Held && !startedOnUI)
            {
                RaycastHit hit;
                if (Pointer.Raycast(cam, out hit))
                {
                    bool onBella = hit.collider.GetComponentInParent<BellaController>() == bella;
                    if (onBella) bella.PetBella(hit.point);
                    if (!bella.Busy) bella.Puppy.LookAt(hit.point + Vector3.up * (onBella ? 0.35f : 0.1f));
                }
            }

            if (Pointer.Up && !bella.Busy) bella.Puppy.ClearLook();
        }
    }
}
