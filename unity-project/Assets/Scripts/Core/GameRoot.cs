using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.MiniGames;
using Bella.Needs;
using Bella.Pet;
using Bella.Save;
using Bella.Systems;
using Bella.UI;
using Bella.World;

namespace Bella.Core
{
    /// <summary>
    /// The whole game is assembled here, in code, when Play starts — camera,
    /// light, room, Bella, sound, HUD, every system. There is NO hand-wired scene:
    /// any scene (even an empty one) becomes the game, because GameRoot boots
    /// itself automatically after the first scene loads.
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        public static GameRoot Instance { get; private set; }

        public Camera Cam { get; private set; }
        public CameraRig CameraRig { get; private set; }
        public Room Room { get; private set; }
        public DayNight DayNight { get; private set; }
        public PetNeeds Needs { get; private set; }
        public BellaController Bella { get; private set; }
        public AudioManager Audio { get; private set; }
        public Hud Hud { get; private set; }
        public Popups Popups { get; private set; }
        public Progress Progress { get; private set; }
        public RoomWander Wander { get; private set; }

        private ThoughtBubble bubble;
        private ParentalGate gate;
        private ParentsPanel parents;
        private StickerBook stickers;
        private Activity activity;
        private bool built;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            if (Instance != null) return;
            if (FindObjectOfType<GameRoot>() != null) return;
            new GameObject("GameRoot").AddComponent<GameRoot>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            try
            {
                Build();
                built = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                ShowFatal(e);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- construction

        private void Build()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            // Anything a template scene left behind would fight with our own camera and light.
            foreach (Camera c in FindObjectsOfType<Camera>()) c.gameObject.SetActive(false);
            foreach (Light l in FindObjectsOfType<Light>()) l.gameObject.SetActive(false);

            // --- camera + light
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            camGo.transform.SetParent(transform, false);
            Cam = camGo.AddComponent<Camera>();
            Cam.clearFlags = CameraClearFlags.SolidColor;
            Cam.backgroundColor = Art.Cream;
            Cam.nearClipPlane = 0.1f;
            Cam.farClipPlane = 60f;
            camGo.AddComponent<AudioListener>();
            CameraRig = camGo.AddComponent<CameraRig>();
            CameraRig.Init(Cam);

            var lightGo = new GameObject("Sun");
            lightGo.transform.SetParent(transform, false);
            lightGo.transform.rotation = Quaternion.Euler(48f, -28f, 0f);
            Light sun = lightGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = 0.35f;
            QualitySettings.shadowDistance = 16f;

            // --- sound
            Audio = new GameObject("Audio").AddComponent<AudioManager>();
            Audio.transform.SetParent(transform, false);
            Audio.Init();

            // --- room
            Room = new GameObject("Room").AddComponent<Room>();
            Room.transform.SetParent(transform, false);
            Room.Build();

            // --- Bella
            var bellaGo = new GameObject("Bella");
            bellaGo.transform.SetParent(transform, false);
            bellaGo.transform.position = Room.HomePoint;
            Needs = bellaGo.AddComponent<PetNeeds>();
            Needs.Init(ScriptableObject.CreateInstance<NeedDecayConfig>());
            Bella = bellaGo.AddComponent<BellaController>();
            Bella.Init(Needs, Room);
            gameObject.AddComponent<PetInteraction>().Init(Cam, Bella);

            DayNight = gameObject.AddComponent<DayNight>();
            DayNight.Init(sun, Cam, Room, Audio);
            Wander = gameObject.AddComponent<RoomWander>();
            Wander.Init(Bella, Room, DayNight);

            // --- UI
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                var es = new GameObject("EventSystem",
                    typeof(UnityEngine.EventSystems.EventSystem),
                    typeof(UnityEngine.EventSystems.StandaloneInputModule));
                es.transform.SetParent(transform, false);
            }
            Hud = gameObject.AddComponent<Hud>();
            Hud.Init(Needs, OnHudAction);
            bubble = gameObject.AddComponent<ThoughtBubble>();
            bubble.Init(Hud.Root, Cam, Bella, OnHudAction);
            Popups = gameObject.AddComponent<Popups>();
            Popups.Init();
            gate = gameObject.AddComponent<ParentalGate>();
            gate.Init();
            parents = gameObject.AddComponent<ParentsPanel>();
            parents.Init(ResetEverything);
            stickers = gameObject.AddComponent<StickerBook>();
            stickers.Init();

            // --- long loop
            Progress = gameObject.AddComponent<Progress>();
            Progress.Init(Needs, Bella, Popups, Hud);
            Progress.OnSessionStart(true);

            Diagnostics.Fact("Game built. Problems: " + Diagnostics.ProblemCount);
            Diagnostics.WriteFile();
            if (Diagnostics.ProblemCount > 0 && (Application.isEditor || Debug.isDebugBuild))
            {
                Text warn = UIKit.Label("SetupWarning", Hud.Root,
                    "Setup notes: " + Diagnostics.ProblemCount + " (gear > Diagnostics)", 26, Art.Rose, TextAnchor.LowerLeft);
                UIKit.Place(warn.rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(20f, 8f), new Vector2(900f, 40f));
            }
        }

        private void ShowFatal(Exception e)
        {
            // Last-resort screen so a non-programmer can screenshot the problem.
            Canvas canvas = UIKit.CreateCanvas("FatalCanvas", 100);
            Image bg = UIKit.Img("Bg", canvas.transform, null, Art.Cream, true);
            UIKit.Stretch(bg.rectTransform);
            Text t = UIKit.Label("Text", canvas.transform,
                "Bella could not start.\nPlease send a screenshot of this screen.\n\n" + e.GetType().Name + ": " + e.Message + "\n\n" + e.StackTrace,
                30, Art.Ink, TextAnchor.UpperLeft);
            t.fontStyle = FontStyle.Normal;
            UIKit.Stretch(t.rectTransform, 40f, 40f, 40f, 40f);
            t.resizeTextForBestFit = true; t.resizeTextMinSize = 14; t.resizeTextMaxSize = 30;
            t.verticalOverflow = VerticalWrapMode.Truncate;
        }

        // ---------------------------------------------------------------- HUD actions

        private void OnHudAction(string action)
        {
            if (!built || activity != null || Popups.IsOpen) return;
            Wander.Pause(10f);
            switch (action)
            {
                case "feed": Bella.RequestFeed(); break;
                case "sleep":
                    if (Bella.IsSleeping) Bella.WakeUp(); else Bella.RequestSleep();
                    break;
                case "play": Bella.RequestFetch(); break;
                case "bath": StartActivity<BathActivity>(); break;
                case "tricks": StartActivity<TrickActivity>(); break;
                case "wardrobe": StartActivity<WardrobeActivity>(); break;
                case "clinic": StartActivity<ClinicActivity>(); break;
                case "stickers": AudioManager.Play("tap"); stickers.Show(); break;
                case "gift": Progress.ClaimGift(); break;
                case "parents": AudioManager.Play("tap"); gate.Ask(parents.Show); break;
            }
        }

        private void StartActivity<T>() where T : Activity
        {
            if (activity != null) return;
            if (Bella.IsSleeping) Bella.WakeUp();
            if (Bella.Busy) return;     // finishing a meal or a fetch — try again in a moment
            Hud.SetVisible(false);
            var go = new GameObject(typeof(T).Name);
            go.transform.SetParent(transform, false);
            activity = go.AddComponent<T>();
            activity.Begin(this);
        }

        public void ActivityFinished(Activity a)
        {
            if (activity != a) return;
            activity = null;
            CameraRig.GoHome();
            Hud.SetVisible(true);
            Bella.EndActivity();
            Progress.CheckStickers();
        }

        /// <summary>Lights down + quieter music while Bella naps.</summary>
        public void SetSleepMode(bool sleeping)
        {
            if (DayNight != null) DayNight.SetSleepDim(sleeping);
        }

        private void ResetEverything()
        {
            SaveSystem.ResetAll();
            Instance = null;
            gameObject.SetActive(false);
            Destroy(gameObject);
            new GameObject("GameRoot").AddComponent<GameRoot>();
        }

        // ---------------------------------------------------------------- app lifecycle

        private void Update()
        {
            if (!built) return;
            if (Input.GetKeyDown(KeyCode.Escape))      // Android back button
            {
                if (activity != null) activity.Finish();
                else if (!Popups.IsOpen) Application.Quit();
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (!built || paused) return;
            // Coming back from the background may mean a new day (PetNeeds reloads itself).
            Progress.OnSessionStart(false);
        }
    }
}
