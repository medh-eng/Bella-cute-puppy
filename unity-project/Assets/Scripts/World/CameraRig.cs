using UnityEngine;

namespace Bella.World
{
    /// <summary>
    /// Smooth camera with two framings: the whole room (home) and a close-up used
    /// by the mini-games. The field of view adapts to the screen's aspect ratio so
    /// the same width of room is visible on every phone and tablet.
    /// </summary>
    public class CameraRig : MonoBehaviour
    {
        private Camera cam;
        private Vector3 posTarget, lookTarget, pos, look;
        private float widthTarget, width;

        // ---- player zoom (home view only): pinch with two fingers, drag with two fingers to look around
        private bool homeMode = true;
        private float zoom = 1f;
        private Vector2 pan;                       // metres, camera + look point move together
        private bool pinchActive;
        private float lastPinchDist;
        private Vector2 lastPinchMid;
        private const float MaxZoom = 2.2f;

        /// <summary>True from the second finger going down until all fingers are up, so taps are not mistaken for pinches.</summary>
        public static bool Pinching { get; private set; }

        private static readonly Vector3 HomePos = new Vector3(0f, 1.7f, -4.6f);
        private static readonly Vector3 HomeLook = new Vector3(0f, 0.72f, 0.5f);
        private const float HomeWidth = 3.1f;       // metres of room visible across the screen at the look point

        public void Init(Camera camera)
        {
            cam = camera;
            pos = posTarget = HomePos;
            look = lookTarget = HomeLook;
            width = widthTarget = HomeWidth;
            Apply();
        }

        public void GoHome()
        {
            homeMode = true;
            ApplyHomeTargets();
        }

        private void ApplyHomeTargets()
        {
            Vector3 off = new Vector3(pan.x, pan.y, 0f);
            posTarget = HomePos + off; lookTarget = HomeLook + off; widthTarget = HomeWidth / zoom;
        }

        private void ClampPan()
        {
            float f = 1f - 1f / zoom;
            pan.x = Mathf.Clamp(pan.x, -HomeWidth * 0.5f * f, HomeWidth * 0.5f * f);
            pan.y = Mathf.Clamp(pan.y, -1.0f * f, 0.5f * f);
        }

        private void HandleZoomInput()
        {
            if (!homeMode) { Pinching = false; pinchActive = false; return; }
            float scroll = Input.GetAxis("Mouse ScrollWheel");        // Editor / mouse
            if (Mathf.Abs(scroll) > 0.0001f) { zoom = Mathf.Clamp(zoom * (1f + scroll * 2f), 1f, MaxZoom); ClampPan(); ApplyHomeTargets(); }

            int n = Input.touchCount;
            if (n >= 2)
            {
                Vector2 a = Input.GetTouch(0).position, b = Input.GetTouch(1).position;
                float d = (a - b).magnitude;
                Vector2 mid = (a + b) * 0.5f;
                if (!pinchActive) { pinchActive = true; lastPinchDist = d; lastPinchMid = mid; }
                else
                {
                    if (lastPinchDist > 1f && d > 1f) zoom = Mathf.Clamp(zoom * (d / lastPinchDist), 1f, MaxZoom);
                    float mpp = width / Mathf.Max(1f, (float)Screen.width);          // metres per screen pixel
                    pan.x -= (mid.x - lastPinchMid.x) * mpp;
                    pan.y -= (mid.y - lastPinchMid.y) * mpp;
                    lastPinchDist = d; lastPinchMid = mid;
                    ClampPan();
                    ApplyHomeTargets();
                }
            }
            Pinching = pinchActive;
            if (n == 0) { pinchActive = false; Pinching = false; }
        }

        /// <summary>Close-up on a point (mini-games).</summary>
        public void Focus(Vector3 point, float visibleWidth = 2.1f, float lookHeight = 0.52f)
        {
            homeMode = false;
            zoom = 1f; pan = Vector2.zero;
            Pinching = false; pinchActive = false;
            lookTarget = point + new Vector3(0f, lookHeight, 0f);
            posTarget = point + new Vector3(0f, 1.25f, -3.6f);
            widthTarget = visibleWidth;
        }

        private void LateUpdate()
        {
            if (cam == null) return;
            HandleZoomInput();
            float k = 1f - Mathf.Exp(-Time.deltaTime * 4f);
            pos = Vector3.Lerp(pos, posTarget, k);
            look = Vector3.Lerp(look, lookTarget, k);
            width = Mathf.Lerp(width, widthTarget, k);
            Apply();
        }

        private void Apply()
        {
            cam.transform.position = pos;
            cam.transform.rotation = Quaternion.LookRotation((look - pos).normalized, Vector3.up);
            float dist = (look - pos).magnitude;
            float aspect = Mathf.Max(0.3f, cam.aspect);
            // vertical FOV that shows `width` metres horizontally at the look distance
            float halfV = Mathf.Atan((width * 0.5f / aspect) / dist) * Mathf.Rad2Deg;
            cam.fieldOfView = Mathf.Clamp(halfV * 2f, 20f, 75f);
        }
    }
}
