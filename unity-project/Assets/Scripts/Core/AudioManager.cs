using System.Collections.Generic;
using UnityEngine;
using Bella.Save;

namespace Bella.Core
{
    /// <summary>
    /// Sound effects + two music loops (day / night). Every clip is an original
    /// synthesised sound shipped in Resources/Audio (see tools/pipeline/gen_audio.py),
    /// so there are no third-party audio licences to track.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource sfx;
        private AudioSource music;
        private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
        private string currentMusic = "";
        private float musicTarget = 0.35f;
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

            AudioClip[] all = Resources.LoadAll<AudioClip>("Audio");
            for (int i = 0; i < all.Length; i++) clips[all[i].name] = all[i];
            Diagnostics.Fact("Audio clips loaded: " + all.Length);
            if (all.Length == 0) Diagnostics.Note("No audio clips found in Resources/Audio.");
        }

        public static void Play(string name, float volume = 1f, float pitch = 1f)
        {
            if (Instance != null) Instance.PlayInternal(name, volume, pitch);
        }

        /// <summary>Bella's voice: the same clips pitched up a little so they are "hers".</summary>
        public static void Voice(string name)
        {
            Play(name, 0.9f, Random.Range(1.05f, 1.25f));
        }

        private void PlayInternal(string name, float volume, float pitch)
        {
            if (!SaveSystem.Data.sfxOn) return;
            AudioClip c;
            if (!clips.TryGetValue(name, out c) || c == null) return;
            sfx.pitch = pitch;
            sfx.PlayOneShot(c, volume);
        }

        public void SetMusic(string name)
        {
            if (name == currentMusic) return;
            currentMusic = name;
            AudioClip c;
            if (clips.TryGetValue(name, out c) && c != null)
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

        public void SetDuck(float factor)
        {
            duck = Mathf.Clamp01(factor);
        }

        public void RefreshSettings()
        {
            if (!SaveSystem.Data.musicOn)
            {
                music.Stop();
            }
            else if (!music.isPlaying && music.clip != null)
            {
                music.Play();
            }
        }

        private void Update()
        {
            if (music == null) return;
            float target = SaveSystem.Data.musicOn ? musicTarget * duck : 0f;
            music.volume = Mathf.MoveTowards(music.volume, target, Time.unscaledDeltaTime * 0.4f);
        }
    }
}
