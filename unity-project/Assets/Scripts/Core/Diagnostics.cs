using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bella.Core
{
    /// <summary>
    /// Collects non-fatal setup problems (missing model, missing sound...) so they
    /// can be shown in the Parents' area and written to a text file. This is how a
    /// non-programmer reports "what went wrong" without reading the Unity console.
    /// </summary>
    public static class Diagnostics
    {
        private static readonly List<string> notes = new List<string>();
        private static readonly List<string> facts = new List<string>();

        public static void Note(string message)
        {
            if (notes.Contains(message)) return;
            notes.Add(message);
            Debug.LogWarning("[Bella] " + message);
        }

        public static void Fact(string message)
        {
            facts.Add(message);
            Debug.Log("[Bella] " + message);
        }

        public static int ProblemCount => notes.Count;

        public static string Report()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Bella diagnostics — version " + Application.version + ", Unity " + Application.unityVersion);
            sb.AppendLine(notes.Count == 0 ? "No problems found." : "Problems: " + notes.Count);
            for (int i = 0; i < notes.Count; i++) sb.AppendLine(" ! " + notes[i]);
            for (int i = 0; i < facts.Count; i++) sb.AppendLine(" - " + facts[i]);
            return sb.ToString();
        }

        public static void WriteFile()
        {
            try
            {
                File.WriteAllText(Path.Combine(Application.persistentDataPath, "bella_diagnostics.txt"), Report());
            }
            catch (System.Exception)
            {
                // Diagnostics must never break the game.
            }
        }
    }
}
