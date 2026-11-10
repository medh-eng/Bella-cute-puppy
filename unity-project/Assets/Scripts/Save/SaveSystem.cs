using System;
using System.IO;
using UnityEngine;

namespace Bella.Save
{
    /// <summary>
    /// Local-only JSON persistence. No cloud sync, no accounts — a deliberate v1
    /// scope decision (docs/ARCHITECTURE.md section 5) that keeps the COPPA
    /// compliance surface at zero.
    ///
    /// Writes are atomic-ish: serialize to a temp file first, then replace the real
    /// file, so a mid-save app kill never leaves a corrupt save behind.
    /// </summary>
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "bella_save.json");
        private static string TempPath => SavePath + ".tmp";

        public static void Save(SaveData data)
        {
            try
            {
                data.lastSavedUtcTicks = DateTime.UtcNow.Ticks;
                if (data.firstPlayUtcTicks <= 0)
                    data.firstPlayUtcTicks = data.lastSavedUtcTicks;

                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(TempPath, json);

                if (File.Exists(SavePath)) File.Delete(SavePath);
                File.Move(TempPath, SavePath);
            }
            catch (Exception e)
            {
                // Never let a failed save crash the game — log and carry on.
                Debug.LogError($"[Bella] Save failed: {e.Message}");
            }
        }

        public static SaveData Load()
        {
            try
            {
                if (!File.Exists(SavePath))
                {
                    var fresh = new SaveData();
                    Save(fresh);
                    return fresh;
                }

                string json = File.ReadAllText(SavePath);
                var data = JsonUtility.FromJson<SaveData>(json);
                return data ?? new SaveData();
            }
            catch (Exception e)
            {
                Debug.LogError($"[Bella] Load failed, starting fresh: {e.Message}");
                return new SaveData();
            }
        }
    }
}
