using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the day streak and a morning greeting banner.
/// Hooks into DailyRhythmManager.OnNewDay so it updates automatically.
///
/// Scene wiring: attach to a Canvas child.  Call Wire() from BellaSceneBootstrap
/// (or Inspector).
/// </summary>
public class DayStreakUI : MonoBehaviour
{
    [SerializeField] private Text  streakLabel;    // e.g. "🔥 Day 7"
    [SerializeField] private Text  greetingLabel;  // morning line
    [SerializeField] private float greetingDisplaySeconds = 5f;

    private float _greetingTimer;

    // ------------------------------------------------------------------ wiring

    public void Wire(Text streak, Text greeting)
    {
        streakLabel   = streak;
        greetingLabel = greeting;
    }

    // ------------------------------------------------------------------ lifecycle

    private void Start()
    {
        RefreshStreak();

        if (DailyRhythmManager.Instance != null)
        {
            DailyRhythmManager.Instance.OnNewDay      += OnNewDay;
            DailyRhythmManager.Instance.OnPeriodChanged += _ => RefreshStreak();
        }

        // Show greeting once on launch (not only on new day)
        ShowGreeting();
    }

    private void OnDestroy()
    {
        if (DailyRhythmManager.Instance != null)
        {
            DailyRhythmManager.Instance.OnNewDay       -= OnNewDay;
            DailyRhythmManager.Instance.OnPeriodChanged -= _ => RefreshStreak();
        }
    }

    private void Update()
    {
        if (_greetingTimer > 0f)
        {
            _greetingTimer -= Time.deltaTime;
            if (_greetingTimer <= 0f && greetingLabel != null)
                greetingLabel.gameObject.SetActive(false);
        }
    }

    // ------------------------------------------------------------------ helpers

    private void RefreshStreak()
    {
        if (streakLabel == null) return;
        var save = SaveSystem.Load();
        int s = Mathf.Max(1, save.dayStreak);
        streakLabel.text = s >= 7 ? $"🔥 Day {s}" : $"⭐ Day {s}";
    }

    private void OnNewDay()
    {
        RefreshStreak();
        ShowGreeting();
    }

    private void ShowGreeting()
    {
        if (greetingLabel == null) return;
        string line = DailyRhythmManager.Instance != null
            ? DailyRhythmManager.Instance.GetGreeting()
            : "Welcome back! Bella missed you!";
        greetingLabel.text = line;
        greetingLabel.gameObject.SetActive(true);
        _greetingTimer = greetingDisplaySeconds;
    }
}
