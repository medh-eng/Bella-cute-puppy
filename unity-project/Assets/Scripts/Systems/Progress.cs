using System;
using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;
using Bella.Save;
using Bella.UI;

namespace Bella.Systems
{
    /// <summary>
    /// The long loop (docs/GDD.md §3, PLAN.md Phase 5): days together, the daily
    /// present, Bella slowly growing, the gentle "sniffles" event and the sticker
    /// book. Everything is framed positively — a missed day is never punished and
    /// nothing here can be bought.
    /// </summary>
    public class Progress : MonoBehaviour
    {
        private PetNeeds needs;
        private BellaController bella;
        private Popups popups;
        private Hud hud;
        private float stickerTimer = 2f;

        /// <summary>
        /// Treats from things that can be repeated endlessly (fetch, practising tricks, extra
        /// baths and check-ups) share one daily allowance. Care that Bella actually needed, the
        /// daily present and learning a new trick are never limited. This keeps the wardrobe a
        /// two-week goal instead of an afternoon of tapping, without ever saying "no" to play.
        /// </summary>
        public const int DailyPlayTreats = 30;

        /// <summary>Returns how many of the wanted treats today's play allowance still covers.</summary>
        public static int PlayTreats(int wanted)
        {
            SaveData d = SaveSystem.Data;
            string today = TodayKey();
            if (d.playPayDateKey != today)
            {
                d.playPayDateKey = today;
                d.playPaidToday = 0;
            }
            int pay = Mathf.Clamp(wanted, 0, Mathf.Max(0, DailyPlayTreats - d.playPaidToday));
            d.playPaidToday += pay;
            return pay;
        }

        public bool GiftAvailable => SaveSystem.Data.lastGiftDateKey != TodayKey();

        public static string TodayKey()
        {
            return DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>Whole real days since the very first launch.</summary>
        public static int DaysSinceFirstPlay()
        {
            double days = (DateTime.UtcNow - SaveSystem.Data.FirstPlayUtc).TotalDays;
            return Mathf.Clamp((int)Math.Floor(Math.Max(0.0, days)), 0, 100000);
        }

        /// <summary>0 = little puppy, 1 = getting bigger (day 5+), 2 = big puppy (day 15+).</summary>
        public static int GrowthStage()
        {
            int d = DaysSinceFirstPlay();
            return d >= 15 ? 2 : d >= 5 ? 1 : 0;
        }

        public static float GrowthScale()
        {
            return Mathf.Lerp(0.86f, 1f, Mathf.Clamp01(DaysSinceFirstPlay() / 15f));
        }

        public void Init(PetNeeds n, BellaController b, Popups p, Hud h)
        {
            needs = n; bella = b; popups = p; hud = h;
            bella.Puppy.GrowthScale = GrowthScale();
            bella.OnCareDone += _ => CheckStickers();
        }

        /// <summary>Called once when the game opens and again whenever it returns from the background.</summary>
        public void OnSessionStart(bool appLaunch)
        {
            SaveData d = SaveSystem.Data;
            string today = TodayKey();
            bool newDay = d.lastStreakDateKey != today;

            if (newDay)
            {
                bool firstEver = d.dayStreak <= 0;
                d.dayStreak = Mathf.Max(0, d.dayStreak) + 1;      // "days together" — never resets
                d.bestStreak = Mathf.Max(d.bestStreak, d.dayStreak);
                d.lastStreakDateKey = today;

                // The sniffles: an occasional, gentle reason to visit The Clinic.
                if (!firstEver && d.dayStreak > 2 && !d.sniffles && UnityEngine.Random.value < 0.18f)
                {
                    d.sniffles = true;
                    needs.SetHealth(55f);
                }
                SaveSystem.Flush();

                if (firstEver)
                {
                    popups.Show("Portraits/face_happy", Art.Teal, "This is Bella!",
                        "Pat her, feed her, and tap her thought bubble to see what she wants.", "success");
                }
                else
                {
                    string portrait = DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 6 ? "Portraits/face_sleepy" : "Portraits/face_excited";
                    popups.Show(portrait, Art.Gold, "Day " + d.dayStreak + " with Bella!",
                        "She is so happy to see you.", "success");
                }
            }

            int stage = GrowthStage();
            if (stage > d.growthStageSeen)
            {
                d.growthStageSeen = stage;
                SaveSystem.Flush();
                popups.Show("Portraits/face_excited", Art.Leaf,
                    stage == 1 ? "Bella is growing!" : "What a big puppy!",
                    "Look how much she has grown with your care.", "gift");
            }
            bella.Puppy.GrowthScale = GrowthScale();

            hud.SetDay(d.dayStreak);
            hud.SetGiftAvailable(GiftAvailable);
            if (appLaunch && !newDay && needs.LastOfflineSeconds > 600) bella.Greet(needs.LastOfflineSeconds);
            if (newDay && !SaveSystem.WasFreshInstall) bella.Greet(needs.LastOfflineSeconds);
            CheckStickers();
        }

        public void ClaimGift()
        {
            if (!GiftAvailable) return;
            SaveData d = SaveSystem.Data;
            d.lastGiftDateKey = TodayKey();
            d.countGifts++;
            int amount = 10 + Mathf.Min(10, d.dayStreak);
            needs.AddTreats(amount);
            needs.Save();
            hud.SetGiftAvailable(false);
            popups.Show("Icons/gift", Art.Pink, "A present!", "+" + amount + " treats for Bella's wardrobe.", "gift");
            bella.Celebrate(0);
            CheckStickers();
        }

        public void CureSniffles()
        {
            SaveSystem.Data.sniffles = false;
            needs.SetHealth(100f);
            needs.Save();
        }

        private void Update()
        {
            // Cheap periodic sweep catches counters changed anywhere (petting, mini-games).
            stickerTimer -= Time.deltaTime;
            if (stickerTimer <= 0f)
            {
                stickerTimer = 3f;
                if (!popups.IsOpen && !bella.InActivity) CheckStickers();
            }
        }

        public void CheckStickers()
        {
            SaveData d = SaveSystem.Data;
            Try("first_pet", d.countPets >= 1);
            Try("cuddles_50", d.countPets >= 50);
            Try("first_meal", d.countMeals >= 1);
            Try("first_bath", d.countBaths >= 1);
            Try("first_nap", d.countNaps >= 1);
            Try("first_fetch", d.countFetches >= 1);
            Try("first_trick", d.learnedTricks.Count >= 1);
            Try("all_tricks", d.learnedTricks.Count >= Catalog.Tricks.Length);
            Try("first_outfit", d.ownedOutfits.Count >= 1);
            Try("fashion_5", d.ownedOutfits.Count >= 5);
            Try("first_checkup", d.countCheckups >= 1);
            Try("first_gift", d.countGifts >= 1);
            Try("streak_3", d.dayStreak >= 3);
            Try("streak_7", d.dayStreak >= 7);
            Try("streak_14", d.dayStreak >= 14);
            Try("streak_30", d.dayStreak >= 30);
            Try("grown_1", GrowthStage() >= 1);
            Try("grown_2", GrowthStage() >= 2);
            Try("butterfly_1", d.countButterflies >= 1);
            Try("butterfly_20", d.countButterflies >= 20);
        }

        private void Try(string id, bool condition)
        {
            if (!condition) return;
            SaveData d = SaveSystem.Data;
            if (d.stickers.Contains(id)) return;
            StickerDef def = Catalog.FindSticker(id);
            if (def == null) return;
            d.stickers.Add(id);
            SaveSystem.Flush();
            popups.Show("Icons/" + def.icon, def.color, "New sticker!", def.title, "sparkle");
        }
    }
}
