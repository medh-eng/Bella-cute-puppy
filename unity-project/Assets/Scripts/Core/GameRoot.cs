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
        private GamePicker picker;
        private PlacePicker placePicker;
        private TitleScreen title;
        private Activity activity;
        private FestiveDecor festive;
        private bool built;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            if (Instance != null) return;
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

            CheckInputAvailable();

            // Anything a template scene left behind would fight with our own camera and light.
            // (Scene roots are walked by hand: the FindObjectsOfType family changes between Unity versions.)
            GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i] == gameObject) continue;
                foreach (Camera c in roots[i].GetComponentsInChildren<Camera>(true)) c.gameObject.SetActive(false);
                foreach (Light l in roots[i].GetComponentsInChildren<Light>(true)) l.gameObject.SetActive(false);
                foreach (AudioListener a in roots[i].GetComponentsInChildren<AudioListener>(true)) a.enabled = false;
            }

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
            sun.shadowStrength = 0.55f;
            QualitySettings.shadowDistance = 16f;

            // --- sound
            Audio = new GameObject("Audio").AddComponent<AudioManager>();
            Audio.transform.SetParent(transform, false);
            Audio.Init();

            // --- room
            Room = new GameObject("Room").AddComponent<Room>();
            Room.transform.SetParent(transform, false);
            Room.Build();
            Room.ApplyTheme(SaveSystem.Data.wallId, SaveSystem.Data.floorId);

            // --- Bella
            var bellaGo = new GameObject("Bella");
            bellaGo.transform.SetParent(transform, false);
            bellaGo.transform.position = Room.HomePoint;
            Needs = bellaGo.AddComponent<PetNeeds>();
            Needs.Init(ScriptableObject.CreateInstance<NeedDecayConfig>());
            Bella = bellaGo.AddComponent<BellaController>();
            Bella.Init(Needs, Room);
            gameObject.AddComponent<PetInteraction>().Init(Cam, Bella, Room);

            DayNight = gameObject.AddComponent<DayNight>();
            DayNight.Init(sun, Cam, Room, Audio);
            Wander = gameObject.AddComponent<RoomWander>();
            Wander.Init(Bella, Room, DayNight);
            gameObject.AddComponent<Ambience>().Init(Room, DayNight, Cam, Needs, Bella);
            festive = gameObject.AddComponent<FestiveDecor>();
            festive.Init(Room);

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
            parents.Init(ResetEverything, () => RefreshSeason(true));
            stickers = gameObject.AddComponent<StickerBook>();
            stickers.Init();
            picker = gameObject.AddComponent<GamePicker>();
            placePicker = gameObject.AddComponent<PlacePicker>();
            title = gameObject.AddComponent<TitleScreen>();

            // --- long loop
            Progress = gameObject.AddComponent<Progress>();
            Progress.Init(Needs, Bella, Popups, Hud);
            Progress.OnSessionStart(true);
            RefreshSeason(true);

            title.Show(null);

            Diagnostics.Fact("Game built. Problems: " + Diagnostics.ProblemCount);
            Diagnostics.WriteFile();
            if (Diagnostics.ProblemCount > 0 && (Application.isEditor || Debug.isDebugBuild))
            {
                Text warn = UIKit.Label("SetupWarning", Hud.Root,
                    "Setup notes: " + Diagnostics.ProblemCount + " (gear > Diagnostics)", 26, Art.Rose, TextAnchor.LowerLeft);
                UIKit.Place(warn.rectTransform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(20f, 8f), new Vector2(900f, 40f));
            }
        }

        /// <summary>
        /// The game reads touches through Unity's classic Input class. Newer Unity versions can
        /// switch that off in Player Settings; say so in plain words instead of failing silently.
        /// </summary>
        private static void CheckInputAvailable()
        {
            try
            {
                Vector3 unused = Input.mousePosition;
                if (unused.x < -1e9f) Debug.Log(unused);
            }
            catch (Exception)
            {
                throw new InvalidOperationException(
                    "Touch input is switched off in this project.\n" +
                    "Fix: stop Play, then use the menu Bella > Set Up Project and restart Unity when asked.\n" +
                    "(Manual fix: Edit > Project Settings > Player > Other Settings > Active Input Handling = Input Manager (Old).)");
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
            if (!built || activity != null || Popups.IsOpen || (picker != null && picker.IsOpen) || (placePicker != null && placePicker.IsOpen) || (title != null && title.IsOpen)) return;
            Wander.Pause(10f);
            switch (action)
            {
                case "feed": StartActivity<FeedingActivity>(); break;
                case "sleep":
                    if (Bella.IsSleeping) Bella.WakeUp(); else Bella.RequestSleep();
                    break;
                case "play":
                    picker.Show(id =>
                    {
                        switch (id)
                        {
                            case "catch": StartActivity<TreatCatchActivity>(); break;
                            case "dig": StartActivity<DigActivity>(); break;
                            case "ball": StartActivity<BallCatchActivity>(); break;
                            case "frisbee": StartActivity<FrisbeeActivity>(); break;
                            case "arrow": StartActivity<ArrowActivity>(); break;
                            case "blocks": StartActivity<BlockDropActivity>(); break;
                            case "jigsaw": StartActivity<JigsawActivity>(); break;
                            case "car": StartActivity<CarRaceActivity>(); break;
                            case "hide": StartActivity<HideSeekActivity>(); break;
                            case "memory": StartActivity<PawMemoryActivity>(); break;
                            case "bubbles": StartActivity<BubblePopActivity>(); break;
                            default: StartActivity<FetchActivity>(); break;
                        }
                    });
                    break;
                case "places": ShowPlaces(); break;
                case "bath": StartActivity<BathActivity>(); break;
                case "tricks": StartActivity<TrickActivity>(); break;
                case "wardrobe": StartActivity<WardrobeActivity>(); break;
                case "decor": StartActivity<DecorActivity>(); break;
                case "clinic": StartActivity<ClinicActivity>(); break;
                case "stickers": AudioManager.Play("tap"); stickers.Show(); break;
                case "gift": Progress.ClaimGift(); break;
                case "parents": AudioManager.Play("tap"); gate.Ask(parents.Show); break;
            }
        }

        private void ShowPlaces()
        {
            placePicker.Show(Needs.Treats, id =>
            {
                PlaceDef def = PlaceDef.Find(id);
                if (def == null) return;
                SaveData d = SaveSystem.Data;
                if (def.Cost > 0 && !d.ownedPlaces.Contains(id))
                {
                    if (!Needs.SpendTreats(def.Cost))
                    {
                        AudioManager.Play("pop", 0.4f, 0.7f);
                        ShowPlaces();
                        return;
                    }
                    d.ownedPlaces.Add(id);
                    SaveSystem.Flush();
                    AudioManager.Play("success", 0.8f);
                }
                VisitActivity.Next = def;
                StartActivity<VisitActivity>();
            });
        }

        private void StartActivity<T>() where T : Activity
        {
            if (activity != null) { Diagnostics.Note("Start " + typeof(T).Name + ": another activity is still running"); return; }
            if (Bella.IsSleeping) Bella.WakeUp();
            if (Bella.Busy)
            {
                // finishing a meal or a fetch — say so instead of silently ignoring the tap
                Diagnostics.Note("Start " + typeof(T).Name + ": Bella is busy");
                Popups.Show("Portraits/face_proud", Art.Sky, "One moment!", "Bella is still busy. Try again in a few seconds.", "tap");
                return;
            }
            Hud.SetVisible(false);
            GameObject go = null;
            try
            {
                go = new GameObject(typeof(T).Name);
                go.transform.SetParent(transform, false);
                activity = go.AddComponent<T>();
                activity.Begin(this);
            }
            catch (Exception e)
            {
                // Never leave the player stuck with a hidden HUD: undo, record the error and show it.
                Debug.LogError("Could not start " + typeof(T).Name + ": " + e);
                Diagnostics.Note("Start " + typeof(T).Name + " FAILED: " + e.GetType().Name + ": " + e.Message);
                if (go != null) Destroy(go);
                activity = null;
                if (Room != null) Room.HomePlace();
                if (DayNight != null) DayNight.SetMusicOverride(false);
                CameraRig.GoHome();
                Bella.EndActivity();
                Hud.SetVisible(true);
                Popups.Show("Portraits/face_surprised", Art.Rose, "Oops!",
                    typeof(T).Name + " could not start.\n" + e.GetType().Name + ": " + e.Message + "\nPlease send a screenshot of this.", "pop");
            }
        }

        public void ActivityFinished(Activity a)
        {
            if (activity != a) return;
            activity = null;
            if (DayNight != null) DayNight.SetMusicOverride(false);
            CameraRig.GoHome();
            Hud.SetVisible(true);
            Bella.EndActivity();
            Progress.CheckStickers();
            if (festive != null) festive.Sync();     // a place backdrop may have hidden the decorations
        }

        /// <summary>
        /// Decorations and music for today's festival (Systems/Seasons), and its greeting the first
        /// time. Runs at launch, on returning from the background and when parents flip the switch.
        /// </summary>
        public void RefreshSeason(bool greet)
        {
            FestivalDef f = Seasons.Today();
            festive.Apply(f);
            DayNight.SetFestival(f);
            if (greet && f != null) Progress.CheckFestival(f);
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
            // Editor only: F previews the festivals in turn (lights, colours, winter, birthday, then the real calendar).
            if (Application.isEditor && Input.GetKeyDown(KeyCode.F) && activity == null)
            {
                Seasons.PreviewIndex = Seasons.PreviewIndex + 1 >= Seasons.All.Length ? -1 : Seasons.PreviewIndex + 1;
                Debug.Log("Festival preview: " + (Seasons.PreviewIndex < 0 ? "off (real calendar)" : Seasons.All[Seasons.PreviewIndex].id));
                RefreshSeason(true);
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (!built || paused) return;
            // Coming back from the background may mean a new day (PetNeeds reloads itself).
            Progress.OnSessionStart(false);
            RefreshSeason(true);
        }
    }
}
