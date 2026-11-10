using UnityEngine;

namespace Bella.Needs
{
    /// <summary>
    /// Designer-tunable decay rates for Bella's needs, per docs/GDD.md section 4 and
    /// docs/ARCHITECTURE.md section 3. ScriptableObject so feel can be tuned in the
    /// Inspector without touching code.
    ///
    /// Create via: Assets > Create > Bella > Need Decay Config
    /// (The editor scene builder creates one automatically if none exists.)
    /// </summary>
    [CreateAssetMenu(fileName = "NeedDecayConfig", menuName = "Bella/Need Decay Config")]
    public class NeedDecayConfig : ScriptableObject
    {
        [Header("LIVE decay — per second while the app is open")]
        [Tooltip("Satiety drop per second during play. 0.02 = ~6 points over a 5-minute session.")]
        public float hungerDecayPerSecond = 0.02f;

        [Tooltip("Hygiene drop per second during play.")]
        public float hygieneDecayPerSecond = 0.008f;

        [Tooltip("Passive energy drop per second (mini-games subtract extra on top).")]
        public float energyDecayPerSecond = 0.005f;

        [Tooltip("Happiness drop per second when everything else is fine.")]
        public float happinessDecayPerSecond = 0.01f;

        [Tooltip("Happiness decays this many times faster while any other need is critical.")]
        public float happinessNeglectMultiplier = 3f;

        [Header("OFFLINE decay — applied to elapsed real time while the app was closed")]
        [Tooltip("Offline rates = live rates x this multiplier. 0.15 means hunger goes 100->0 in roughly 9 hours away.")]
        [Range(0f, 1f)] public float offlineDecayMultiplier = 0.15f;

        [Tooltip("KINDNESS CAP (docs/GDD.md section 4): offline time beyond this many hours is ignored, so returning after a month never shows a miserable puppy.")]
        public float maxOfflineHours = 12f;

        [Header("Thresholds (0-100 scale)")]
        public float lowEnergyThreshold = 20f;
        public float criticalHungerThreshold = 20f;
        public float criticalHygieneThreshold = 15f;
        public float lowHappinessThreshold = 25f;

        [Header("Restore amounts")]
        public float feedAmount = 35f;
        public float petHappinessAmount = 8f;
        public float sleepRestoresTo = 100f;

        [Header("Floor / ceiling")]
        public float minValue = 0f;
        public float maxValue = 100f;
    }
}
