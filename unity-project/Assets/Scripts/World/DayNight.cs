using System;
using UnityEngine;
using Bella.Core;

namespace Bella.World
{
    /// <summary>
    /// Matches the room to the player's REAL local clock (docs/PLAN.md Phase 3):
    /// warm morning light, bright afternoon, golden evening, moonlit night — and a
    /// dimmed "lights out" look while Bella naps.
    /// </summary>
    public class DayNight : MonoBehaviour
    {
        public enum Period { Morning, Afternoon, Evening, Night }

        public Period Current { get; private set; } = Period.Afternoon;
        public bool IsNight => Current == Period.Night;
        public bool IsSleepDim => sleepDim;

        private Light sun;
        private Camera cam;
        private Room room;
        private AudioManager audioManager;
        private bool sleepDim;
        private bool musicOverride;
        private float checkTimer;

        private Color tLight, tAmbient, tSky;
        private float tIntensity;
        private Color cLight, cAmbient, cSky;
        private float cIntensity;
        private bool first = true;

        public void Init(Light sunLight, Camera camera, Room roomRef, AudioManager audio)
        {
            sun = sunLight;
            cam = camera;
            room = roomRef;
            audioManager = audio;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            Evaluate();
        }

        public void SetSleepDim(bool on)
        {
            sleepDim = on;
            Evaluate();
        }

        /// <summary>
        /// While a mini-game plays its own tune, the period music stays out of the
        /// way (the 30-second clock check would otherwise stomp it mid-game).
        /// </summary>
        public void SetMusicOverride(bool on)
        {
            musicOverride = on;
            if (!on) Evaluate();
        }

        /// <summary>The festival on today (Systems/Seasons) or null; its tune replaces the daytime music.</summary>
        public Bella.Systems.FestivalDef Festival { get; private set; }

        public void SetFestival(Bella.Systems.FestivalDef f)
        {
            Festival = f;
            Evaluate();
        }

        public static Period PeriodForHour(int hour)
        {
            if (hour >= 6 && hour < 12) return Period.Morning;
            if (hour >= 12 && hour < 17) return Period.Afternoon;
            if (hour >= 17 && hour < 20) return Period.Evening;
            return Period.Night;
        }

        private void Evaluate()
        {
            Period p = PeriodForHour(DateTime.Now.Hour);
            Current = p;
            switch (p)
            {
                case Period.Morning:
                    tLight = new Color(1.00f, 0.94f, 0.84f); tIntensity = 1.15f;
                    tAmbient = new Color(0.46f, 0.45f, 0.49f); tSky = new Color(0.72f, 0.88f, 0.99f);
                    break;
                case Period.Afternoon:
                    tLight = new Color(1.00f, 0.97f, 0.90f); tIntensity = 1.2f;
                    tAmbient = new Color(0.48f, 0.48f, 0.52f); tSky = new Color(0.55f, 0.80f, 0.98f);
                    break;
                case Period.Evening:
                    tLight = new Color(1.00f, 0.79f, 0.58f); tIntensity = 1.1f;
                    tAmbient = new Color(0.46f, 0.40f, 0.44f); tSky = new Color(0.99f, 0.70f, 0.52f);
                    break;
                default:
                    tLight = new Color(0.72f, 0.78f, 1.00f); tIntensity = 0.8f;
                    tAmbient = new Color(0.36f, 0.36f, 0.52f); tSky = new Color(0.16f, 0.19f, 0.40f);
                    break;
            }
            if (sleepDim)
            {
                tIntensity *= 0.45f;
                tAmbient = Color.Lerp(tAmbient, new Color(0.26f, 0.27f, 0.45f), 0.75f);
            }

            if (room != null && room.WindowOrb != null)
            {
                bool night = p == Period.Night;
                room.WindowOrb.sharedMaterial.mainTexture = Art.Tex(night ? "Icons/moon" : "Icons/sun");
            }
            if (audioManager != null)
            {
                if (!musicOverride)
                {
                    string track = "music_day";
                    if (sleepDim || p == Period.Night) track = "music_night";
                    else if (p == Period.Morning) track = "music_morning";
                    else if (p == Period.Evening) track = "music_evening";
                    if (Festival != null && !sleepDim && p != Period.Night) track = Festival.music;
                    audioManager.SetMusic(track);
                }
                // Birdsong while the sun is up, crickets after dark, hush while she naps.
                string amb = "";
                if (!sleepDim)
                    amb = (p == Period.Morning || p == Period.Afternoon) ? "amb_birds" : "amb_crickets";
                audioManager.SetAmbience(amb);
            }

            if (first)
            {
                first = false;
                cLight = tLight; cAmbient = tAmbient; cSky = tSky; cIntensity = tIntensity;
                Apply();
            }
        }

        private void Update()
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > 30f)
            {
                checkTimer = 0f;
                Evaluate();
            }
            float k = 1f - Mathf.Exp(-Time.deltaTime * 2.5f);
            cLight = Color.Lerp(cLight, tLight, k);
            cAmbient = Color.Lerp(cAmbient, tAmbient, k);
            cSky = Color.Lerp(cSky, tSky, k);
            cIntensity = Mathf.Lerp(cIntensity, tIntensity, k);
            Apply();
        }

        private void Apply()
        {
            if (sun != null) { sun.color = cLight; sun.intensity = cIntensity; }
            RenderSettings.ambientLight = cAmbient;
            if (cam != null) cam.backgroundColor = new Color(Mathf.Min(1f, cAmbient.r * 1.5f), Mathf.Min(1f, cAmbient.g * 1.5f), Mathf.Min(1f, cAmbient.b * 1.5f), 1f);
            if (room != null && room.WindowSky != null) room.WindowSky.sharedMaterial.color = cSky;
        }
    }
}
