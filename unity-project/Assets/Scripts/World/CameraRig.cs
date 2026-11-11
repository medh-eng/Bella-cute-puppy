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
            posTarget = HomePos; lookTarget = HomeLook; widthTarget = HomeWidth;
        }

        /// <summary>Close-up on a point (mini-games).</summary>
        public void Focus(Vector3 point, float visibleWidth = 2.1f, float lookHeight = 0.52f)
        {
            lookTarget = point + new Vector3(0f, lookHeight, 0f);
            posTarget = point + new Vector3(0f, 1.25f, -3.6f);
            widthTarget = visibleWidth;
        }

        private void LateUpdate()
        {
            if (cam == null) return;
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
