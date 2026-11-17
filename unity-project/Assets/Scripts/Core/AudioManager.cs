using System.Collections.Generic;
using UnityEngine;
using Bella.Save;

namespace Bella.Core
{
    /// <summary>
    /// Sound effects, one music loop per moment of the day, and a quiet nature
    /// ambience layer (birdsong / crickets) underneath. Clips live in Resources/Audio:
    /// some are synthesised by tools/pipeline/gen_audio.py, the rest were supplied as
    /// recordings and prepared by tools/pipeline/ingest_audio.py. Where each supplied
    /// file came from is recorded in art-inbox/audio/LEDGER.txt and CREDITS.md.
    ///
    /// Variants: clips named name_1, name_2 ... are alternatives of one sound ("name");
    /// asking for "name" plays one of them at random, never the same twice running.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource sfx;
        private AudioSource music;
        private AudioSource ambience;
        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, List<AudioClip>> variants = new Dictionary<string, List<AudioClip>>();
        private readonly Dictionary<string, int> lastVariant = new Dictionary<string, int>();
        private string currentMusic = "";
        private string currentAmbience = "";
        private float musicTarget = 0.35f;
        private float ambienceTarget = 0.16f;
        private float duck = 1f;

        public void Init()
        {
            Instance = this;
            sfx = gameObject.AddComponent<AudioSource>();
            sfx.playOnAwake = false;
            music = gameObject.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = true;
            music.volume = 0f;
            ambience = gameObject.AddComponent<AudioSource>();
            ambience.playOnAwake = false;
            ambience.loop = true;
            ambience.volume = 0f;

            AudioClip[] all = Resources.LoadAll<AudioClip>("Audio");
            for (int i = 0; i < all.Length; i++)
            {
                clips[all[i].name] = all[i];
                string baseName = VariantBase(all[i].name);
                if (baseName == null) continue;
                List<AudioClip> list;
                if (!variants.TryGetValue(baseName, out list)) { list = new List<AudioClip>(); variants[baseName] = list; }
                list.Add(all[i]);
            }
            Diagnostics.Fact("Audio clips loaded: " + all.Length + " (" + variants.Count + " sounds with variants)");
            if (all.Length == 0) Diagnostics.Note("No audio clips found in Resources/Audio.");
        }

        public static void Play(string name, float volume = 1f, float pitch = 1f)
        {
            if (Instance != null) Instance.PlayInternal(name, volume, pitch);
        }

        /// <summary>
        /// Bella's voice. Synthesised sounds are pitched up a little so they are "hers". Real recordings
        /// are left almost as they are; with fewer than three variants a wider wobble keeps repeats from
        /// sounding like a tape loop.
        /// </summary>
        public static void Voice(string name)
        {
            float lo = 1.05f, hi = 1.25f;
            if (Instance != null)
            {
                List<AudioClip> list;
                if (Instance.variants.TryGetValue(name, out list))
                {
                    if (list.Count >= 3) { lo = 0.97f; hi = 1.06f; }
                    else { lo = 0.92f; hi = 1.12f; }
                }
            }
            Play(name, 0.9f, Random.Range(lo, hi));
        }

        /// <summary>"bark_2" -> "bark"; null when the name does not end in _digits.</summary>
        private static string VariantBase(string name)
        {
            int u = name.LastIndexOf('_');
            if (u <= 0 || u == name.Length - 1) return null;
            for (int i = u + 1; i < name.Length; i++)
                if (name[i] < '0' || name[i] > '9') return null;
            return name.Substring(0, u);
        }

        /// <summary>The clip to play for a name: a random variant if it has any, else the plain clip.</summary>
        private AudioClip Resolve(string name)
        {
            List<AudioClip> list;
            if (variants.TryGetValue(name, out list) && list.Count > 0)
            {
                int pick = Random.Range(0, list.Count);
                int last;
                if (list.Count > 1 && lastVariant.TryGetValue(name, out last) && pick == last)
                    pick = (pick + 1 + Random.Range(0, list.Count - 1)) % list.Count;
                lastVariant[name] = pick;
                return list[pick];
            }
            AudioClip c;
            clips.TryGetValue(name, out c);
            return c;
        }

        /// <summary>True when a clip (or variants of it) exists — lets callers fall back to another tune.</summary>
        public bool Has(string name)
        {
            return clips.ContainsKey(name) || variants.ContainsKey(name);
        }

        private void PlayInternal(string name, float volume, float pitch)
        {
            if (!SaveSystem.Data.sfxOn) return;
            AudioClip c = Resolve(name);
            if (c == null) return;
            sfx.pitch = pitch;
            sfx.PlayOneShot(c, volume);
        }

        public void SetMusic(string name)
        {
            if (name == currentMusic) return;
            currentMusic = name;
            AudioClip c = Resolve(name);
            if (c != null)
            {
                music.clip = c;
                music.volume = 0f;
                if (SaveSystem.Data.musicOn) music.Play();
            }
            else
            {
                music.Stop();
            }
        }

        /// <summary>The quiet nature layer under the music. Empty name = silence.</summary>
        public void SetAmbience(string name)
        {
            if (name == currentAmbience) return;
            currentAmbience = name;
            AudioClip c;
            if (!string.IsNullOrEmpty(name) && clips.TryGetValue(name, out c) && c != null)
            {
                ambience.clip = c;
                ambience.volume = 0f;
                if (SaveSystem.Data.musicOn) ambience.Play();
            }
            else
            {
                ambience.Stop();
                ambience.clip = null;
            }
        }

        public void SetDuck(float factor)
        {
            duck = Mathf.Clamp01(factor);
        }

        public void RefreshSettings()
        {
            if (!SaveSystem.Data.musicOn)
            {
                music.Stop();
                ambience.Stop();
            }
            else
            {
                if (!music.isPlaying && music.clip != null) music.Play();
                if (!ambience.isPlaying && ambience.clip != null) ambience.Play();
            }
        }

        private void Update()
        {
            if (music == null) return;
            bool on = SaveSystem.Data.musicOn;
            music.volume = Mathf.MoveTowards(music.volume, on ? musicTarget * duck : 0f, Time.unscaledDeltaTime * 0.4f);
            if (ambience != null)
                ambience.volume = Mathf.MoveTowards(ambience.volume, on ? ambienceTarget * duck : 0f, Time.unscaledDeltaTime * 0.2f);
        }
    }
}
