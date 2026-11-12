using System;
using System.Collections.Generic;

namespace Bella.Save
{
    [Serializable]
    public class TrickProgress
    {
        public string id;
        public int count;
    }

    /// <summary>
    /// Plain data container persisted to disk. Deliberately free of any personally
    /// identifiable information (docs/GDD.md section 7) — it never holds a name,
    /// age, device ID, or anything beyond in-game state.
    ///
    /// IMPORTANT: Unity's JsonUtility CANNOT serialize System.DateTime. Timestamps
    /// are stored as long tick counts and rebuilt with DateTimeKind.Utc.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int saveVersion = 3;

        public float hunger = 100f;
        public float hygiene = 100f;
        public float energy = 100f;
        public float health = 100f;
        public float happiness = 100f;

        /// <summary>Soft currency earned through care and mini-games. Never purchasable.</summary>
        public int treats = 20;

        public List<string> ownedOutfits = new List<string>();
        public List<string> equippedOutfits = new List<string>();
        public List<string> learnedTricks = new List<string>();
        public List<TrickProgress> trickProgress = new List<TrickProgress>();
        public List<string> stickers = new List<string>();

        public long lastSavedUtcTicks = 0;
        public long firstPlayUtcTicks = 0;

        public int dayStreak = 0;
        public int bestStreak = 0;
        public string lastStreakDateKey = "";
        public string lastGiftDateKey = "";
        public int growthStageSeen = 0;

        public bool sniffles = false;

        // Daily allowance of treats from repeatable play (fetch, extra tricks...).
        public string playPayDateKey = "";
        public int playPaidToday = 0;

        // Lifetime counters (drive stickers).
        public int countPets = 0;
        public int countMeals = 0;
        public int countBaths = 0;
        public int countNaps = 0;
        public int countFetches = 0;
        public int countGifts = 0;
        public int countCheckups = 0;
        public int countTricks = 0;

        // Parent settings.
        public bool sfxOn = true;
        public bool musicOn = true;

        public DateTime LastSavedUtc =>
            lastSavedUtcTicks > 0 ? new DateTime(lastSavedUtcTicks, DateTimeKind.Utc) : DateTime.UtcNow;

        public DateTime FirstPlayUtc =>
            firstPlayUtcTicks > 0 ? new DateTime(firstPlayUtcTicks, DateTimeKind.Utc) : DateTime.UtcNow;

        public int GetTrickCount(string id)
        {
            for (int i = 0; i < trickProgress.Count; i++)
                if (trickProgress[i].id == id) return trickProgress[i].count;
            return 0;
        }

        public int AddTrickCount(string id)
        {
            for (int i = 0; i < trickProgress.Count; i++)
            {
                if (trickProgress[i].id == id)
                {
                    trickProgress[i].count++;
                    return trickProgress[i].count;
                }
            }
            trickProgress.Add(new TrickProgress { id = id, count = 1 });
            return 1;
        }

        /// <summary>Repairs lists that an older or hand-edited save may have left null.</summary>
        public void Sanitize()
        {
            if (ownedOutfits == null) ownedOutfits = new List<string>();
            if (equippedOutfits == null) equippedOutfits = new List<string>();
            if (learnedTricks == null) learnedTricks = new List<string>();
            if (trickProgress == null) trickProgress = new List<TrickProgress>();
            if (stickers == null) stickers = new List<string>();
            if (lastStreakDateKey == null) lastStreakDateKey = "";
            if (lastGiftDateKey == null) lastGiftDateKey = "";
            if (playPayDateKey == null) playPayDateKey = "";
            if (playPaidToday < 0) playPaidToday = 0;
            if (treats < 0) treats = 0;
            saveVersion = 3;
        }
    }
}
