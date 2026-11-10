using System;

namespace Bella.Save
{
    /// <summary>
    /// Plain data container persisted to disk. Kept deliberately minimal and free of
    /// any personally identifiable information, per the COPPA data-minimization
    /// requirement in docs/GDD.md section 7 - this file never holds a name, device
    /// ID, or anything beyond in-game state.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public float hunger = 100f;
        public float hygiene = 100f;
        public float energy = 100f;
        public float health = 100f;
        public float happiness = 100f;

        public int currency = 0;

        /// <summary>
        /// UTC timestamp of the last save. Always DateTime.UtcNow, never local time -
        /// see docs/ARCHITECTURE.md section 3 for why (prevents clock-manipulation
        /// exploits against the decay system).
        /// </summary>
        public DateTime lastSavedUtc = DateTime.UtcNow;
    }
}
