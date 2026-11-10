using System;
using System.Collections.Generic;

namespace Bella.Save
{
    /// <summary>
    /// Plain data container persisted to disk. Deliberately free of any personally
    /// identifiable information per the COPPA data-minimization requirement in
    /// docs/GDD.md section 7 — never holds a name, device ID, or anything beyond
    /// in-game state.
    ///
    /// IMPORTANT: Unity's JsonUtility CANNOT serialize System.DateTime (it silently
    /// drops the field). All timestamps are therefore stored as long tick counts
    /// (DateTime.UtcNow.Ticks) and reconstructed with DateTimeKind.Utc.
    /// See docs/ARCHITECTURE.md section 3.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>Bump when the schema changes; lets future code migrate old saves.</summary>
        public int saveVersion = 2;

        public float hunger = 100f;
        public float hygiene = 100f;
        public float energy = 100f;
        public float health = 100f;
        public float happiness = 100f;

        /// <summary>Soft currency ("treats") earned through care and mini-games.</summary>
        public int treats = 0;

        /// <summary>IDs of owned wardrobe items (Phase 4).</summary>
        public List<string> ownedOutfits = new List<string>();

        /// <summary>IDs of tricks Bella has learned (Phase 4 — Trick Training).</summary>
        public List<string> learnedTricks = new List<string>();

        /// <summary>IDs of collected memory-book stickers (Phase 5).</summary>
        public List<string> stickers = new List<string>();

        /// <summary>UTC ticks of the last save. Always from DateTime.UtcNow.Ticks.</summary>
        public long lastSavedUtcTicks = 0;

        /// <summary>UTC ticks of the very first launch — drives Bella's growth system (Phase 5).</summary>
        public long firstPlayUtcTicks = 0;

        /// <summary>Consecutive-day care streak (positive framing only — never punishes a break).</summary>
        public int dayStreak = 0;

        public DateTime LastSavedUtc =>
            lastSavedUtcTicks > 0 ? new DateTime(lastSavedUtcTicks, DateTimeKind.Utc) : DateTime.UtcNow;
    }
}
