using UnityEngine;
using System;

/// <summary>
/// Tracks real-world time-of-day and fires events that change Bella's mood/behaviour.
/// Four periods: Morning 6-12, Afternoon 12-18, Evening 18-22, Night 22-6.
/// Also handles the day-streak increment (counted once per real calendar day).
/// </summary>
public class DailyRhythmManager : MonoBehaviour
{
    public enum TimeOfDay { Morning, Afternoon, Evening, Night }

    [Header("Period boundaries (24-hour clock)")]
    [SerializeField] private int morningStart   = 6;
    [SerializeField] private int afternoonStart = 12;
    [SerializeField] private int eveningStart   = 18;
    [SerializeField] private int nightStart     = 22;

    // Singleton — only one in the scene
    public static DailyRhythmManager Instance { get; private set; }

    // Current period
    public TimeOfDay CurrentPeriod { get; private set; } = TimeOfDay.Morning;

    // Events other systems listen to
    public event Action<TimeOfDay> OnPeriodChanged;   // period changed
    public event Action            OnNewDay;           // first open of a new calendar day

    private float _checkInterval = 60f;   // re-evaluate every 60 seconds
    private float _timer;

    // ------------------------------------------------------------------ lifecycle

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrentPeriod = GetPeriodForHour(DateTime.Now.Hour);
        CheckNewDay();
        _timer = _checkInterval; // fire first evaluation immediately on next Update
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = _checkInterval;
            EvaluatePeriod();
        }
    }

    // ------------------------------------------------------------------ period logic

    private void EvaluatePeriod()
    {
        TimeOfDay newPeriod = GetPeriodForHour(DateTime.Now.Hour);
        if (newPeriod != CurrentPeriod)
        {
            CurrentPeriod = newPeriod;
            OnPeriodChanged?.Invoke(CurrentPeriod);
            Debug.Log($"[DailyRhythm] Period → {CurrentPeriod}");
        }
    }

    private TimeOfDay GetPeriodForHour(int hour)
    {
        if (hour >= morningStart   && hour < afternoonStart) return TimeOfDay.Morning;
        if (hour >= afternoonStart && hour < eveningStart)   return TimeOfDay.Afternoon;
        if (hour >= eveningStart   && hour < nightStart)     return TimeOfDay.Evening;
        return TimeOfDay.Night;
    }

    // ------------------------------------------------------------------ streak / new-day logic

    private void CheckNewDay()
    {
        var save = SaveSystem.Load();
        string todayKey = DateTime.Now.ToString("yyyy-MM-dd");

        // firstPlayUtcTicks == 0 means first ever launch
        if (save.firstPlayUtcTicks == 0)
        {
            save.firstPlayUtcTicks = DateTime.UtcNow.Ticks;
            save.dayStreak = 1;
            save.lastStreakDateKey = todayKey;
            SaveSystem.Save(save);
            OnNewDay?.Invoke();
            return;
        }

        if (save.lastStreakDateKey == todayKey) return; // already counted today

        // Work out how many calendar days since last open
        DateTime lastDate;
        bool parsed = DateTime.TryParseExact(save.lastStreakDateKey, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out lastDate);

        int daysDiff = parsed ? (DateTime.Now.Date - lastDate.Date).Days : 2;

        if (daysDiff == 1)
        {
            // consecutive day — extend streak
            save.dayStreak = Mathf.Max(1, save.dayStreak) + 1;
        }
        else
        {
            // missed a day — reset streak
            save.dayStreak = 1;
        }

        save.lastStreakDateKey = todayKey;
        SaveSystem.Save(save);
        OnNewDay?.Invoke();
        Debug.Log($"[DailyRhythm] Day streak → {save.dayStreak}  (daysDiff={daysDiff})");
    }

    // ------------------------------------------------------------------ public helpers

    /// <summary>Returns a greeting line appropriate to the current time of day.</summary>
    public string GetGreeting(string petName = "Bella")
    {
        return CurrentPeriod switch
        {
            TimeOfDay.Morning   => $"Good morning! {petName} is so happy to see you! 🌞",
            TimeOfDay.Afternoon => $"{petName} has been waiting for you! 🐾",
            TimeOfDay.Evening   => $"Evening cuddle time with {petName}! 🌙",
            TimeOfDay.Night     => $"It's late… {petName} is getting sleepy! 😴",
            _                   => $"{petName} is happy to see you!"
        };
    }

    /// <summary>Returns true during Night period — Bella should prefer sleeping.</summary>
    public bool IsNightTime() => CurrentPeriod == TimeOfDay.Night;

    /// <summary>Returns true during Morning — good time to show streak/sticker reward.</summary>
    public bool IsMorning() => CurrentPeriod == TimeOfDay.Morning;
}
