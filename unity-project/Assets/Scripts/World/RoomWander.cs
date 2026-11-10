using UnityEngine;

/// <summary>
/// Makes Bella wander idle around the room by picking random points inside a
/// rectangular floor area and issuing WalkToState commands via BellaController.
///
/// Does NOT use NavMesh — WalkToState steers directly (matches ARCHITECTURE.md §5).
/// </summary>
[RequireComponent(typeof(BellaController))]
public class RoomWander : MonoBehaviour
{
    [Header("Room floor bounds (world-space, flat)")]
    [SerializeField] private Vector2 roomMin = new Vector2(-3f, -2f);
    [SerializeField] private Vector2 roomMax = new Vector2( 3f,  2f);

    [Header("Timing")]
    [Tooltip("Seconds Bella stays in one spot before wandering again")]
    [SerializeField] private float pauseMin = 4f;
    [SerializeField] private float pauseMax = 10f;

    [Header("Night-time override")]
    [Tooltip("After this hour Bella wanders less (encourages sleep)")]
    [SerializeField] private float sleepyPauseMultiplier = 2.5f;

    private BellaController _bella;
    private float _wanderTimer;
    private bool  _walking;

    private void Awake() => _bella = GetComponent<BellaController>();

    private void Start()
    {
        _wanderTimer = Random.Range(pauseMin, pauseMax); // slight delay so scene settles
    }

    private void Update()
    {
        if (_walking) return;   // BellaController is busy — don't interfere

        _wanderTimer -= Time.deltaTime;
        if (_wanderTimer > 0f) return;

        PickNextPoint();
    }

    private void PickNextPoint()
    {
        // During night, wander far less
        float mult = (DailyRhythmManager.Instance != null && DailyRhythmManager.Instance.IsNightTime())
                     ? sleepyPauseMultiplier : 1f;

        Vector3 target = new Vector3(
            Random.Range(roomMin.x, roomMax.x),
            transform.position.y,            // keep on the floor
            Random.Range(roomMin.y, roomMax.y));

        _walking = true;
        _bella.WalkToPoint(target, OnArrived);
    }

    private void OnArrived()
    {
        _walking = false;
        float mult = (DailyRhythmManager.Instance != null && DailyRhythmManager.Instance.IsNightTime())
                     ? sleepyPauseMultiplier : 1f;
        _wanderTimer = Random.Range(pauseMin, pauseMax) * mult;
    }

    /// <summary>Temporarily pause wandering (e.g. player is interacting).</summary>
    public void Pause(float seconds)
    {
        _wanderTimer = Mathf.Max(_wanderTimer, seconds);
    }

    // ------------------------------------------------------------------ Gizmo
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 1f, 0.5f, 0.3f);
        Vector3 center = new Vector3((roomMin.x + roomMax.x) / 2f, 0f, (roomMin.y + roomMax.y) / 2f);
        Vector3 size   = new Vector3(roomMax.x - roomMin.x, 0.05f, roomMax.y - roomMin.y);
        Gizmos.DrawCube(center, size);
        Gizmos.color = new Color(0.3f, 1f, 0.5f, 1f);
        Gizmos.DrawWireCube(center, size);
    }
#endif
}
