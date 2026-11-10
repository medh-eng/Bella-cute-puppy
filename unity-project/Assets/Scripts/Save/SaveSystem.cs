using System.IO;
using UnityEngine;

namespace Bella.Save
{
    /// <summary>
    /// Local-only JSON persistence. No cloud sync, no account system - this is a
    /// deliberate v1 scope decision (docs/ARCHITECTURE.md section 5) that keeps the
    /// COPPA compliance surface as small as possible. Revisit only if a real need
    /// for cross-device save arises.
    /// </summary>
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "bella_save.json");

        public static void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(SavePath, json);
        }

        public static SaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                var fresh = new SaveData();
                Save(fresh);
                return fresh;
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
    }
}
