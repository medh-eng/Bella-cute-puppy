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
        private bool downOnBella;
        private Vector2 downPos;
        private float downTime;

        // room floor she may be sent to by a tap (same area she wanders in, a little wider)
        private const float XMin = -1.1f, XMax = 1.1f, ZMin = -0.5f, ZMax = 0.9f;

        public void Init(Camera c, BellaController b)
        {
            cam = c;
            bella = b;
        }

        private void Update()
        {
            if (cam == null || bella == null) return;
            if (bella.InActivity) return;

            if (Pointer.Down)
            {
                startedOnUI = Pointer.OverUI;
                downPos = Pointer.Position;
                downTime = Time.time;
                RaycastHit h0;
                downOnBella = Pointer.Raycast(cam, out h0) && h0.collider.GetComponentInParent<BellaController>() == bella;
            }

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

            // A quick tap on the empty floor sends her there.
            if (Pointer.Up && !startedOnUI && !downOnBella && !bella.Busy && !bella.IsSleeping
                && Time.time - downTime < 0.35f && (Pointer.Position - downPos).magnitude < Screen.height * 0.03f)
            {
                Ray ray = cam.ScreenPointToRay(Pointer.Position);
                Plane floor = new Plane(Vector3.up, Vector3.zero);
                float enter;
                if (floor.Raycast(ray, out enter))
                {
                    Vector3 p = ray.GetPoint(enter);
                    if (p.x >= XMin && p.x <= XMax && p.z >= ZMin && p.z <= ZMax)
                    {
                        p.y = 0f;
                        if (bella.WalkToPoint(p, null))
                        {
                            AudioManager.Play("tap", 0.5f);
                            Fx.Burst(p + Vector3.up * 0.05f, "paw", 3, 0.2f, -0.2f, 0.4f, 0.8f);
                        }
                    }
                }
            }
        }
    }
}
