using System;
using UnityEngine;
using Bella.Save;

namespace Bella.Needs
{
    /// <summary>
    /// Tracks Bella's five core needs (satiety, hygiene, energy, health, happiness)
    /// and applies the offline-decay formula from docs/ARCHITECTURE.md section 3:
    ///
    ///     V_current = max(V_min, V_saved - (R_decay * deltaSeconds))
    ///
    /// Health is intentionally NOT decayed here - per docs/GDD.md section 4, health
    /// drops via occasional random "illness" events, not steady decay. Hook that up
    /// in a separate IllnessEventSystem once The Clinic mini-game is built (Phase 4).
    /// </summary>
    public class PetNeeds : MonoBehaviour
    {
        [SerializeField] private NeedDecayConfig config;

        public float Hunger { get; private set; }
        public float Hygiene { get; private set; }
        public float Energy { get; private set; }
        public float Health { get; private set; } = 100f;
        public float Happiness { get; private set; }

        public event Action<float> OnHungerChanged;
        public event Action<float> OnHygieneChanged;
        public event Action<float> OnEnergyChanged;
        public event Action<float> OnHappinessChanged;

        private void Awake()
        {
            LoadAndApplyOfflineDecay();
        }

        /// <summary>
        /// Call on app resume (and on first load) to bring saved need values up to
        /// date with however much real-world time has passed since last save.
        /// See docs/ARCHITECTURE.md section 3 for why this uses UTC, not local time.
        /// </summary>
        public void LoadAndApplyOfflineDecay()
        {
            SaveData data = SaveSystem.Load();

            double elapsedSeconds = (DateTime.UtcNow - data.lastSavedUtc).TotalSeconds;
            elapsedSeconds = Math.Max(0, elapsedSeconds); // guard against clock weirdness

            Hunger = ApplyDecay(data.hunger, config.hungerDecayPerSecond, elapsedSeconds);
            Hygiene = ApplyDecay(data.hygiene, config.hygieneDecayPerSecond, elapsedSeconds);
            Energy = ApplyDecay(data.energy, config.energyDecayPerSecond, elapsedSeconds);
            Happiness = ApplyDecay(data.happiness, config.happinessDecayPerSecond, elapsedSeconds);
            Health = data.health; // health does not passively decay - see class summary

            OnHungerChanged?.Invoke(Hunger);
            OnHygieneChanged?.Invoke(Hygiene);
            OnEnergyChanged?.Invoke(Energy);
            OnHappinessChanged?.Invoke(Happiness);
        }

        private float ApplyDecay(float savedValue, float decayPerSecond, double elapsedSeconds)
        {
            float decayed = savedValue - (decayPerSecond * (float)elapsedSeconds);
            return Mathf.Clamp(decayed, config.minValue, config.maxValue);
        }

        /// <summary>Call when the player feeds Bella (drag kibble/treat/water to her mouth).</summary>
        public void Feed(float amount)
        {
            Hunger = Mathf.Clamp(Hunger + amount, config.minValue, config.maxValue);
            OnHungerChanged?.Invoke(Hunger);
            Save();
        }

        /// <summary>Call when the bubble-bath mini-sequence completes.</summary>
        public void Bathe(float amount)
        {
            Hygiene = Mathf.Clamp(Hygiene + amount, config.minValue, config.maxValue);
            OnHygieneChanged?.Invoke(Hygiene);
            Save();
        }

        /// <summary>Call when the sleep sequence completes.</summary>
        public void Sleep(float amount)
        {
            Energy = Mathf.Clamp(Energy + amount, config.minValue, config.maxValue);
            OnEnergyChanged?.Invoke(Energy);
            Save();
        }

        /// <summary>Call on petting / successful mini-game participation.</summary>
        public void AddHappiness(float amount)
        {
            Happiness = Mathf.Clamp(Happiness + amount, config.minValue, config.maxValue);
            OnHappinessChanged?.Invoke(Happiness);
            Save();
        }

        public bool IsEnergyLow => Energy <= config.lowEnergyThreshold;
        public bool IsHungerCritical => Hunger <= config.criticalHungerThreshold;

        /// <summary>Persist current values with an up-to-date UTC timestamp.</summary>
        public void Save()
        {
            SaveSystem.Save(new SaveData
            {
                hunger = Hunger,
                hygiene = Hygiene,
                energy = Energy,
                health = Health,
                happiness = Happiness,
                lastSavedUtc = DateTime.UtcNow
            });
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
            else LoadAndApplyOfflineDecay();
        }
    }
}
