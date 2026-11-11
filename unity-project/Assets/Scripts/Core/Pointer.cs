using UnityEngine;
using UnityEngine.EventSystems;

namespace Bella.Core
{
    /// <summary>
    /// One-finger input that works identically with a mouse in the Editor and a
    /// touch on device (legacy Input: the first touch is mirrored to the mouse).
    /// </summary>
    public static class Pointer
    {
        public static bool Down => Input.GetMouseButtonDown(0);
        public static bool Held => Input.GetMouseButton(0);
        public static bool Up => Input.GetMouseButtonUp(0);
        public static Vector2 Position => Input.mousePosition;

        /// <summary>True when the finger is on a UI element (so the 3D world should ignore it).</summary>
        public static bool OverUI
        {
            get
            {
                EventSystem es = EventSystem.current;
                if (es == null) return false;
                if (Input.touchCount > 0)
                    return es.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                return es.IsPointerOverGameObject();
            }
        }

        public static bool Raycast(Camera cam, out RaycastHit hit)
        {
            hit = new RaycastHit();
            if (cam == null) return false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, 100f);
        }
    }
}
