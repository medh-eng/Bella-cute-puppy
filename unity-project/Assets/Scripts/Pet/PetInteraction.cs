using UnityEngine;
using Bella.Core;
using Bella.World;

namespace Bella.Pet
{
    /// <summary>
    /// Everything a finger does in the room (outside mini-games):
    ///   tap or stroke Bella        -> she is petted
    ///   tap furniture              -> Bella goes there (bed: she jumps up and sleeps, bowl: she eats,
    ///                                 ball: she fetches, others: she walks over and reacts)
    ///   tap the empty floor        -> Bella walks to that spot
    ///   press and HOLD furniture   -> it lifts; drag it anywhere on the floor and let go to place it
    /// While the finger is down she also watches it (head follows).
    /// </summary>
    public class PetInteraction : MonoBehaviour
    {
        private const float HoldSeconds = 0.45f;
        private const float TapSlopPixels = 28f;

        private Camera cam;
        private BellaController bella;
        private Room room;
        private bool startedOnUI;

        private RoomObject pressObj;
        private Vector2 downPos;
        private float downTime;
        private bool dragging;
        private bool onBellaAtDown;
        private Vector3 dragOffset;
        private bool movedFar;
        private bool hitOtherAtDown;

        public void Init(Camera c, BellaController b, Room r)
        {
            cam = c;
            bella = b;
            room = r;
        }

        private void Update()
        {
            if (cam == null || bella == null) return;
            if (bella.InActivity) { CancelPress(); return; }

            if (Pointer.Down)
            {
                startedOnUI = Pointer.OverUI;
                pressObj = null; dragging = false; movedFar = false; onBellaAtDown = false; hitOtherAtDown = false;
                if (!startedOnUI)
                {
                    downPos = Pointer.Position;
                    downTime = Time.time;
                    RaycastHit hit;
                    if (Pointer.Raycast(cam, out hit))
                    {
                        onBellaAtDown = hit.collider.GetComponentInParent<BellaController>() == bella;
                        if (!onBellaAtDown) pressObj = hit.collider.GetComponentInParent<RoomObject>();
                        hitOtherAtDown = !onBellaAtDown && pressObj == null;     // a butterfly or similar: not a floor tap
                    }
                }
            }

            if (Pointer.Held && !startedOnUI)
            {
                if ((Pointer.Position - downPos).magnitude > TapSlopPixels) movedFar = true;

                // pick up furniture after a long press
                if (pressObj != null && pressObj.movable && !dragging && !movedFar && Time.time - downTime > HoldSeconds
                    && !(pressObj.id == "bed" && bella.IsSleeping))
                {
                    dragging = true;
                    Vector3 floorPoint;
                    dragOffset = FloorPoint(out floorPoint) ? pressObj.transform.localPosition - floorPoint : Vector3.zero;
                    AudioManager.Play("pop", 0.7f, 1.3f);
                    Fx.Sparkles(pressObj.transform.position + Vector3.up * 0.3f, 6);
                }

                if (dragging)
                {
                    Vector3 fp;
                    if (FloorPoint(out fp)) room.SetObjectSpot(pressObj, fp + dragOffset, false);
                    // lift a little while held
                    Vector3 lp = pressObj.transform.localPosition;
                    pressObj.transform.localPosition = new Vector3(lp.x, 0.06f, lp.z);
                    return;
                }

                if (pressObj == null)
                {
                    RaycastHit hit;
                    if (Pointer.Raycast(cam, out hit))
                    {
                        bool onBella = hit.collider.GetComponentInParent<BellaController>() == bella;
                        if (onBella) bella.PetBella(hit.point);
                        if (!bella.Busy) bella.Puppy.LookAt(hit.point + Vector3.up * (onBella ? 0.35f : 0.1f));
                    }
                }
            }

            if (Pointer.Up)
            {
                if (!startedOnUI)
                {
                    if (dragging)
                    {
                        room.SetObjectSpot(pressObj, new Vector3(pressObj.transform.localPosition.x, 0f, pressObj.transform.localPosition.z), true);
                        AudioManager.Play("tap", 0.8f);
                        Fx.Sparkles(pressObj.transform.position + Vector3.up * 0.2f, 4);
                    }
                    else if (!movedFar && Time.time - downTime <= 1.2f)
                    {
                        if (pressObj != null) TapObject(pressObj);
                        else if (!onBellaAtDown && !hitOtherAtDown) TapFloor();
                    }
                }
                dragging = false;
                pressObj = null;
                if (!bella.Busy) bella.Puppy.ClearLook();
            }
        }

        private void CancelPress()
        {
            dragging = false;
            pressObj = null;
        }

        /// <summary>Where the finger's ray meets the floor (y = 0).</summary>
        private bool FloorPoint(out Vector3 point)
        {
            point = Vector3.zero;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Mathf.Abs(ray.direction.y) < 0.0001f) return false;
            float t = -ray.origin.y / ray.direction.y;
            if (t < 0f) return false;
            point = ray.origin + ray.direction * t;
            point.y = 0f;
            return true;
        }

        private void TapFloor()
        {
            if (bella.Busy || bella.IsSleeping) return;
            Vector3 p;
            if (!FloorPoint(out p)) return;
            p = Room.ClampWalk(p);
            AudioManager.Play("tap", 0.5f);
            Fx.Burst(p + Vector3.up * 0.05f, "paw", 3, 0.2f, -0.2f, 0.4f, 0.8f);
            if (bella.WalkToPoint(room.PushOut(p), null) && Bella.Core.GameRoot.Instance != null && Bella.Core.GameRoot.Instance.Wander != null)
                Bella.Core.GameRoot.Instance.Wander.Interrupted();
        }

        private void TapObject(RoomObject o)
        {
            if (bella.IsSleeping) { bella.WakeUp(); return; }
            if (bella.Busy) return;
            Vector3 at = o.transform.position;
            switch (o.id)
            {
                case "bed": bella.RequestSleep(); break;
                case "bowl": bella.RequestFeed(); break;
                case "ball": bella.RequestFetch(); break;
                case "waterbowl":
                    bella.RequestVisit(o.StandPoint, at, "Eat", Expression.Soapy, "splash", 2.0f, true); break;
                case "plant":
                    bella.RequestVisit(o.StandPoint, at, "Sneeze", Expression.Excited, "pop", 1.2f, false); break;
                case "basket":
                    bella.RequestVisit(o.StandPoint, at, "PlayBow", Expression.Excited, "pop", 1.6f, false); break;
                case "nightlight":
                    bella.RequestVisit(o.StandPoint, at, "Happy", Expression.Excited, "sparkle", 1.4f, false); break;
                default:
                    bella.RequestVisit(o.StandPoint, at, "Happy", Expression.Excited, "tap", 1.4f, false); break;
            }
        }
    }
}
