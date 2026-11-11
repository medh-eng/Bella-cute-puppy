using System;
using UnityEngine;
using Bella.Save;

namespace Bella.Needs
{
    /// <summary>
    /// Tracks Bella's five needs and applies BOTH decay paths (docs/ARCHITECTURE.md §3):
    ///  1. LIVE decay every frame while playing.
    ///  2. OFFLINE decay on load/resume from UTC timestamps, clamped to
    ///     [0, maxOfflineHours] — clock-manipulation guard + "kindness cap".
    /// Health never decays on its own: it only drops through the gentle "sniffles"
    /// event and is restored in The Clinic.
    /// </summary>
    public class PetNeeds : MonoBehaviour
    {
        private NeedDecayConfig config;
        public NeedDecayConfig Config => config;

        public float Hunger { get; private set; } = 100f;
        public float Hygiene { get; private set; } = 100f;
        public float Energy { get; private set; } = 100f;
        public float Health { get; private set; } = 100f;
        public float Happiness { get; private set; } = 100f;
        public int Treats { get; private set; }

        /// <summary>Seconds Bella was away, computed on the last load (drives the greeting).</summary>
        public double LastOfflineSeconds { get; private set; }

        /// <summary>When true, live decay is paused (mini-games, sleeping).</summary>
        public bool DecayPaused { get; set; }

        public event Action OnNeedsChanged;
        public event Action<int> OnTreatsEarned;

        private float saveTimer;
        private float notifyTimer;
        private bool initialised;
        private const float AutosaveIntervalSeconds = 20f;

        public void Init(NeedDecayConfig c)
        {
            config = c;
            LoadAndApplyOfflineDecay();
            initialised = true;
        }

        private void Update()
        {
            if (!initialised || config == null) return;
            float dt = Time.deltaTime;

            if (!DecayPaused)
            {
                Hunger = Clamp(Hunger - config.hungerDecayPerSecond * dt);
                Hygiene = Clamp(Hygiene - config.hygieneDecayPerSecond * dt);
                Energy = Clamp(Energy - config.energyDecayPerSecond * dt);
                float rate = config.happinessDecayPerSecond *
                             (AnyNeedCritical ? config.happinessNeglectMultiplier : 1f);
                Happiness = Clamp(Happiness - rate * dt);
            }

            notifyTimer += dt;
            if (notifyTimer >= 0.25f)
            {
                notifyTimer = 0f;
                Raise();
            }

            saveTimer += dt;
            if (saveTimer >= AutosaveIntervalSeconds)
            {
                saveTimer = 0f;
                Save();
            }
        }

        public void LoadAndApplyOfflineDecay()
        {
            if (config == null) return;
            SaveData data = SaveSystem.Data;

            double elapsed = (DateTime.UtcNow - data.LastSavedUtc).TotalSeconds;
            elapsed = Math.Max(0, elapsed);                               // backwards clock => no decay
            LastOfflineSeconds = elapsed;
            elapsed = Math.Min(elapsed, config.maxOfflineHours * 3600.0); // kindness cap

            float mult = config.offlineDecayMultiplier;
            float e = (float)elapsed;
            // Kindness floor: time away never drops a need below a "wistful, not miserable" level.
            float floor = config.offlineFloor;
            Hunger = Mathf.Max(Mathf.Min(data.hunger, floor), Clamp(data.hunger - config.hungerDecayPerSecond * mult * e));
            Hygiene = Mathf.Max(Mathf.Min(data.hygiene, floor), Clamp(data.hygiene - config.hygieneDecayPerSecond * mult * e));
            Happiness = Mathf.Max(Mathf.Min(data.happiness, floor), Clamp(data.happiness - config.happinessDecayPerSecond * mult * e));
            // Time away counts as rest: energy comes back while the app is closed.
            Energy = Clamp(data.energy + config.offlineEnergyRestorePerHour * (e / 3600f));
            Health = Clamp(data.health);
            Treats = Mathf.Max(0, data.treats);
            Raise();
        }

        // ---- Restore actions -------------------------------------------------

        public void Feed() { Feed(config != null ? config.feedAmount : 35f); }
        public void Feed(float amt) { Hunger = Clamp(Hunger + amt); Raise(); }
        public void Bathe(float amt) { Hygiene = Clamp(Hygiene + amt); Raise(); }
        public void Rest(float amt) { Energy = Clamp(Energy + amt); Raise(); }
        public void Tire(float amt) { Energy = Clamp(Energy - amt); Raise(); }
        public void Heal(float amt) { Health = Clamp(Health + amt); Raise(); }
        public void SetHealth(float v) { Health = Clamp(v); Raise(); }
        public void Pet() { AddHappiness(config != null ? config.petHappinessAmount : 8f); }
        public void AddHappiness(float amt) { Happiness = Clamp(Happiness + amt); Raise(); }

        public void AddTreats(int amount)
        {
            Treats = Mathf.Max(0, Treats + amount);
            if (amount > 0 && OnTreatsEarned != null) OnTreatsEarned(amount);
            Raise();
        }

        public bool SpendTreats(int amount)
        {
            if (amount < 0 || Treats < amount) return false;
            Treats -= amount;
            Raise();
            Save();
            return true;
        }

        private void Raise()
        {
            if (OnNeedsChanged != null) OnNeedsChanged();
        }

        // ---- Threshold queries ----------------------------------------------

        public bool IsEnergyLow => config != null && Energy <= config.lowEnergyThreshold;
        public bool IsHungerCritical => config != null && Hunger <= config.criticalHungerThreshold;
        public bool IsHygieneCritical => config != null && Hygiene <= config.criticalHygieneThreshold;
        public bool IsHappinessLow => config != null && Happiness <= config.lowHappinessThreshold;
        public bool IsUnwell => Health < 90f;
        public bool AnyNeedCritical => IsHungerCritical || IsHygieneCritical || IsEnergyLow;

        /// <summary>0..1 overall mood — drives tail-wag speed.</summary>
        public float Mood01 => Mathf.Clamp01((Hunger + Hygiene + Energy + Happiness + Health) / 500f);

        // ---- Persistence -----------------------------------------------------

        public void Save()
        {
            if (!initialised) return;
            SaveData d = SaveSystem.Data;
            d.hunger = Hunger;
            d.hygiene = Hygiene;
            d.energy = Energy;
            d.health = Health;
            d.happiness = Happiness;
            d.treats = Treats;
            SaveSystem.Flush();
        }

        private float Clamp(float v)
        {
            return Mathf.Clamp(v, config != null ? config.minValue : 0f, config != null ? config.maxValue : 100f);
        }

        private void OnApplicationPause(bool paused)
        {
            if (!initialised) return;
            if (paused) Save();
            else LoadAndApplyOfflineDecay();
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }
}
