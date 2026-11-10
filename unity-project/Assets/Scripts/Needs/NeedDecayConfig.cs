using UnityEngine;

namespace Bella.Needs
{
    /// <summary>
    /// Designer-tunable decay rates for each of Bella's needs, per docs/GDD.md section 4.
    /// Exposed as a ScriptableObject so Pon (or any designer) can tune feel in the
    /// Unity Inspector without touching code.
    ///
    /// Create via: Assets > Create > Bella > Need Decay Config
    /// </summary>
    [CreateAssetMenu(fileName = "NeedDecayConfig", menuName = "Bella/Need Decay Config")]
    public class NeedDecayConfig : ScriptableObject
    {
        [Header("Decay rate per second (R_decay in docs/ARCHITECTURE.md section 3)")]
        [Tooltip("How fast satiety drops per second.")]
        public float hungerDecayPerSecond = 0.02f;

        [Tooltip("How fast hygiene drops per second.")]
        public float hygieneDecayPerSecond = 0.01f;

        [Tooltip("How fast energy drops per second (mainly consumed by mini-games, not idle time).")]
        public float energyDecayPerSecond = 0.015f;

        [Tooltip("How fast happiness drops per second when other needs are neglected.")]
        public float happinessDecayPerSecond = 0.01f;

        [Header("Thresholds (0-100 scale)")]
        public float lowEnergyThreshold = 20f;
        public float criticalHungerThreshold = 20f;

        [Header("Floor")]
        [Tooltip("V_min in the decay formula - needs never drop below this.")]
        public float minValue = 0f;

        [Tooltip("Needs are stored/clamped on this max scale, e.g. 0-100.")]
        public float maxValue = 100f;
    }
}
