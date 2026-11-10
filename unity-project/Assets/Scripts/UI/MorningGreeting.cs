using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Full-screen morning card shown once per calendar day on first open.
/// Displays streak milestone message + a "Let's Play!" dismiss button.
/// Integrates with DailyRhythmManager.OnNewDay.
///
/// Built entirely in code by BellaSceneBootstrap — no prefab needed.
/// </summary>
public class MorningGreeting : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text       headlineText;
    [SerializeField] private Text       bodyText;
    [SerializeField] private Button     dismissButton;

    // ------------------------------------------------------------------ wiring

    public void Wire(GameObject p, Text headline, Text body, Button dismiss)
    {
        panel        = p;
        headlineText = headline;
        bodyText     = body;
        dismissButton = dismiss;

        dismissButton.onClick.AddListener(Dismiss);
        panel.SetActive(false);
    }

    // ------------------------------------------------------------------ lifecycle

    private void Start()
    {
        if (DailyRhythmManager.Instance != null)
            DailyRhythmManager.Instance.OnNewDay += Show;

        // Also show on first-ever launch
        var save = SaveSystem.Load();
        bool firstLaunch = save.dayStreak <= 1 && save.firstPlayUtcTicks == 0;
        if (firstLaunch) Show();
    }

    private void OnDestroy()
    {
        if (DailyRhythmManager.Instance != null)
            DailyRhythmManager.Instance.OnNewDay -= Show;
    }

    // ------------------------------------------------------------------ show / dismiss

    private void Show()
    {
        if (panel == null) return;
        var save = SaveSystem.Load();
        int streak = Mathf.Max(1, save.dayStreak);

        headlineText.text = GetHeadline(streak);
        bodyText.text     = GetBody(streak);

        panel.SetActive(true);
    }

    private void Dismiss() => panel?.SetActive(false);

    // ------------------------------------------------------------------ milestone strings

    private string GetHeadline(int streak)
    {
        if (streak == 1)  return "Welcome to Bella's World! 🐾";
        if (streak == 3)  return "3 Days in a Row! You're a Star! ⭐";
        if (streak == 7)  return "One Week! Bella is SO happy! 🎉";
        if (streak == 14) return "Two Whole Weeks! Amazing! 🏆";
        if (streak == 30) return "30 Days! Bella's Best Friend! 🌟";
        if (streak % 10 == 0) return $"{streak} Days! Incredible! 🔥";
        return $"Day {streak} — Bella missed you! 💛";
    }

    private string GetBody(int streak)
    {
        if (streak == 1)  return "Let's take care of Bella together!";
        if (streak == 7)  return "A special sticker has been added to your memory book!";
        if (streak >= 3)  return "Bella grew a little since yesterday. Keep it up!";
        return "Bella can't wait to play with you today.";
    }
}
