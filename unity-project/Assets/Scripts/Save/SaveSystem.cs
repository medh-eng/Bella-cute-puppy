using System;
using System.IO;
using UnityEngine;

namespace Bella.Save
{
    /// <summary>
    /// Local-only JSON persistence. No cloud sync, no accounts, no network — the
    /// COPPA / Families compliance surface stays at zero (docs/ARCHITECTURE.md §5).
    ///
    /// One shared in-memory SaveData instance (Data) is used by every system;
    /// Flush() writes it atomically (temp file, then replace).
    /// </summary>
    public static class SaveSystem
    {
        private static SaveData data;

        /// <summary>True when this launch created a brand-new save (first ever run).</summary>
        public static bool WasFreshInstall { get; private set; }

        private static string SavePath => Path.Combine(Application.persistentDataPath, "bella_save.json");
        private static string TempPath => SavePath + ".tmp";

        public static SaveData Data
        {
            get
            {
                if (data == null) data = LoadFromDisk();
                return data;
            }
        }

        public static void Flush()
        {
            try
            {
                SaveData d = Data;
                d.lastSavedUtcTicks = DateTime.UtcNow.Ticks;
                if (d.firstPlayUtcTicks <= 0) d.firstPlayUtcTicks = d.lastSavedUtcTicks;

                string json = JsonUtility.ToJson(d, true);
                File.WriteAllText(TempPath, json);
                if (File.Exists(SavePath)) File.Delete(SavePath);
                File.Move(TempPath, SavePath);
            }
            catch (Exception e)
            {
                // Never let a failed save crash the game.
                Debug.LogWarning("[Bella] Save failed: " + e.Message);
            }
        }

        /// <summary>Wipes all progress (parents' area / editor menu).</summary>
        public static void ResetAll()
        {
            try
            {
                if (File.Exists(SavePath)) File.Delete(SavePath);
                if (File.Exists(TempPath)) File.Delete(TempPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[Bella] Reset failed: " + e.Message);
            }
            data = null;
        }

        private static SaveData LoadFromDisk()
        {
            WasFreshInstall = false;
            try
            {
                if (File.Exists(SavePath))
                {
                    string json = File.ReadAllText(SavePath);
                    SaveData loaded = JsonUtility.FromJson<SaveData>(json);
                    if (loaded != null)
                    {
                        loaded.Sanitize();
                        return loaded;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("[Bella] Load failed, starting fresh: " + e.Message);
            }

            WasFreshInstall = true;
            var fresh = new SaveData();
            fresh.firstPlayUtcTicks = DateTime.UtcNow.Ticks;
            fresh.lastSavedUtcTicks = fresh.firstPlayUtcTicks;
            return fresh;
        }
    }
}
