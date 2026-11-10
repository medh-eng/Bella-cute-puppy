using UnityEngine;

namespace Bella.Pet
{
    /// <summary>
    /// Touch/click input: tapping Bella pets her. Uses the legacy input path (mouse
    /// position maps to primary touch on Android) so it works in the Editor and on
    /// device with zero setup. Attach to any always-active object; raycasts against
    /// Bella's collider.
    /// </summary>
    public class PetInteraction : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private BellaController bella;

        public void Wire(Camera c, BellaController b) { cam = c; bella = b; }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (cam == null || bella == null) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f) &&
                hit.collider.GetComponentInParent<BellaController>() == bella)
            {
                bella.PetBella();
            }
        }
    }
}
