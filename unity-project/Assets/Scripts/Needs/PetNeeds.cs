using System;
using UnityEngine;
using Bella.Save;

namespace Bella.Needs
{
    /// <summary>
    /// Tracks Bella's five core needs and applies BOTH decay paths from
    /// docs/ARCHITECTURE.md section 3:
    ///
    ///  1. LIVE decay every frame while playing (R_decay * Time.deltaTime).
    ///  2. OFFLINE decay on load/resume:
    ///         V_current = max(V_min, V_saved - (R_decay * offlineMultiplier * deltaSeconds))
    ///     with deltaSeconds clamped to [0, maxOfflineHours] — the clamp is both a
    ///     clock-manipulation guard and the "kindness cap" (docs/GDD.md section 4).
    ///
    /// Health intentionally does NOT decay — it drops via occasional gentle
    /// "sniffles" events (Phase 3) and is restored in The Clinic (Phase 4).
    /// </summary>
    public class PetNeeds : MonoBehaviour
    {
        [SerializeField] private NeedDecayConfig config;

        public NeedDecayConfig Config => config;

        public float Hunger { get; private set; } = 100f;
        public float Hygiene { get; private set; } = 100f;
        public float Energy { get; private set; } = 100f;
        public float Health { get; private set; } = 100f;
        public float Happiness { get; private set; } = 100f;

        public int Treats { get; private set; }

        /// <summary>Seconds Bella was away, computed on the last load (for the wake-up greeting).</summary>
        public double LastOfflineSeconds { get; private set; }

        public event Action OnNeedsChanged;

        private float saveTimer;
        private const float AutosaveIntervalSeconds = 20f;

        /// <summary>Allows the editor scene builder to inject the config without manual wiring.</summary>
        public void SetConfig(NeedDecayConfig c) => config = c;

        private void Start()
        {
            LoadAndApplyOfflineDecay();
        }

        private void Update()
        {
            if (config == null) return;

            float dt = Time.deltaTime;

            Hunger  = Clamp(Hunger  - config.hungerDecayPerSecond  * dt);
            Hygiene = Clamp(Hygiene - config.hygieneDecayPerSecond * dt);
            Energy  = Clamp(Energy  - config.energyDecayPerSecond  * dt);

            float happinessRate = config.happinessDecayPerSecond *
                (AnyNeedCritical ? config.happinessNeglectMultiplier : 1f);
            Happiness = Clamp(Happiness - happinessRate * dt);

            OnNeedsChanged?.Invoke();

            // Periodic autosave so a force-kill loses at most a few seconds.
            saveTimer += dt;
            if (saveTimer >= AutosaveIntervalSeconds)
            {
                saveTimer = 0f;
                Save();
            }
        }

        /// <summary>
        /// Bring saved values up to date with real elapsed time. UTC only — see
        /// docs/ARCHITECTURE.md section 3.
        /// </summary>
        public void LoadAndApplyOfflineDecay()
        {
            if (config == null) return;

            SaveData data = SaveSystem.Load();

            double elapsed = (DateTime.UtcNow - data.LastSavedUtc).TotalSeconds;
            elapsed = Math.Max(0, elapsed);                       // backwards clock => no decay
            LastOfflineSeconds = elapsed;
            elapsed = Math.Min(elapsed, config.maxOfflineHours * 3600.0); // kindness cap

            float mult = config.offlineDecayMultiplier;
            Hunger    = Clamp(data.hunger    - config.hungerDecayPerSecond    * mult * (float)elapsed);
            Hygiene   = Clamp(data.hygiene   - config.hygieneDecayPerSecond   * mult * (float)elapsed);
            Energy    = Clamp(data.energy    - config.energyDecayPerSecond    * mult * (float)elapsed);
            Happiness = Clamp(data.happiness - config.happinessDecayPerSecond * mult * (float)elapsed);
            Health    = data.health;
            Treats    = data.treats;

            OnNeedsChanged?.Invoke();
        }

        // ---- Restore actions -------------------------------------------------

        public void Feed()           => Feed(config != null ? config.feedAmount : 35f);
        public void Feed(float amt)  { Hunger    = Clamp(Hunger + amt);    Bump(); }
        public void Bathe(float amt) { Hygiene   = Clamp(Hygiene + amt);   Bump(); }
        public void Rest(float amt)  { Energy    = Clamp(Energy + amt);    Bump(); }
        public void Heal(float amt)  { Health    = Clamp(Health + amt);    Bump(); }

        public void Pet() => AddHappiness(config != null ? config.petHappinessAmount : 8f);
        public void AddHappiness(float amt) { Happiness = Clamp(Happiness + amt); Bump(); }

        public void AddTreats(int amount) { Treats = Mathf.Max(0, Treats + amount); Bump(); }

        // Note: restore actions deliberately do NOT write to disk — SleepState calls
        // Rest() every frame. Persistence is covered by the 20s autosave plus
        // OnApplicationPause/Quit saves.
        private void Bump() => OnNeedsChanged?.Invoke();

        // ---- Threshold queries ----------------------------------------------

        public bool IsEnergyLow       => config != null && Energy    <= config.lowEnergyThreshold;
        public bool IsHungerCritical  => config != null && Hunger    <= config.criticalHungerThreshold;
        public bool IsHygieneCritical => config != null && Hygiene   <= config.criticalHygieneThreshold;
        public bool IsHappinessLow    => config != null && Happiness <= config.lowHappinessThreshold;
        public bool AnyNeedCritical   => IsHungerCritical || IsHygieneCritical || IsEnergyLow;

        /// <summary>0..1 overall mood — drives tail-wag speed and expressions.</summary>
        public float Mood01 => Mathf.Clamp01(
            (Hunger + Hygiene + Energy + Happiness) / (4f * (config != null ? config.maxValue : 100f)));

        // ---- Persistence -----------------------------------------------------

        public void Save()
        {
            SaveData existing = SaveSystem.Load(); // preserve fields this class doesn't own
            existing.hunger = Hunger;
            existing.hygiene = Hygiene;
            existing.energy = Energy;
            existing.health = Health;
            existing.happiness = Happiness;
            existing.treats = Treats;
            SaveSystem.Save(existing);
        }

        private float Clamp(float v) =>
            Mathf.Clamp(v, config != null ? config.minValue : 0f, config != null ? config.maxValue : 100f);

        private void OnApplicationPause(bool paused)
        {
            if (paused) Save();
            else LoadAndApplyOfflineDecay();
        }

        private void OnApplicationQuit() => Save();
    }
}
