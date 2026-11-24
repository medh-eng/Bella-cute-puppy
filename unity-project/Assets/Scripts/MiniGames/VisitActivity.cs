using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.Systems;
using Bella.UI;
using Bella.World;

namespace Bella.MiniGames
{
    /// <summary>One place Bella can visit: its backdrop, floor, music and (for some) a price in treats.</summary>
    public class PlaceDef
    {
        public string Id, Name, Icon, Floor, Music;
        public int Cost;
        public PlaceDef(string id, string name, string icon, string floor, string music, int cost)
        { Id = id; Name = name; Icon = icon; Floor = floor; Music = music; Cost = cost; }

        public static readonly PlaceDef[] All =
        {
            new PlaceDef("garden",  "Garden",  "place_garden",  "floor_grass",   "music_garden", 0),
            new PlaceDef("park",    "Park",    "place_park",    "floor_grass",   "music_play",   0),
            new PlaceDef("kitchen", "Kitchen", "place_kitchen", "floor_checker", "music_play",   0),
            new PlaceDef("beach",   "Beach",   "sun",           "floor_sand",    "music_day",    20),
            new PlaceDef("snow",    "Snow",    "cloud",         "floor_carpet",  "music_fest_winter", 30),
            new PlaceDef("stage",   "Stage",   "place_stage",   "floor_stage",   "music_stage",  40),
        };

        public static PlaceDef Find(string id)
        {
            for (int i = 0; i < All.Length; i++) if (All[i].Id == id) return All[i];
            return null;
        }
    }

    /// <summary>
    /// A relaxed visit to somewhere new: Bella is taken there, tap the ground to send her exploring.
    /// The first visit to each place pays a one-off souvenir of 5 treats; after that it is just for fun
    /// (a little happiness every few seconds, nothing from the daily play pool).
    /// </summary>
    public class VisitActivity : Activity
    {
        public static PlaceDef Next;

        private PlaceDef def;
        private Text hint;
        private float stayTimer, tapCooldown;
        private bool souvenirShown;

        protected override string Tune { get { return def != null ? def.Music : "music_play"; } }
        private string placeArt;
        protected override string Place
        {
            get
            {
                if (def == null) return null;
                if (placeArt == null) placeArt = PickBackdrop(def.Id);
                return placeArt;
            }
        }

        /// <summary>Same place, different picture: the Stage rotates between three studios, the Park becomes the festival street while a festival is on.</summary>
        private static string PickBackdrop(string id)
        {
            if (id == "stage")
            {
                string[] studios = { "stage", "studio_pink", "studio_rainbow" };
                string pick = studios[Random.Range(0, studios.Length)];
                if (Resources.Load<Texture2D>("Places/" + pick) != null) return pick;
            }
            else if (id == "park" && Seasons.Today() != null && Resources.Load<Texture2D>("Places/festival") != null)
            {
                return "festival";
            }
            return id;
        }
        protected override string PlaceFloor { get { return def != null ? def.Floor : null; } }

        private void Awake() { def = Next; }

        protected override void OnBegin() { Build(); }

        private void Update()
        {
            if (!Running) return;
            if (!Ready) return;
            stayTimer += Time.deltaTime;
            tapCooldown -= Time.deltaTime;
            if (stayTimer > 12f) { stayTimer = 0f; Bella.Needs.AddHappiness(2f); Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, 2); }
            if (Pointer.Down && !Pointer.OverUI && tapCooldown <= 0f) TapGround();
        }

        private void Build()
        {
            hint = UIKit.Label("Hint", UI, "Tap the ground to explore " + (def != null ? def.Name : "") + "!", 50, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(hint.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -290f), new Vector2(960f, 100f));
            hint.resizeTextForBestFit = true; hint.resizeTextMinSize = 28; hint.resizeTextMaxSize = 50;
            G.CameraRig.Focus(new Vector3(0f, 0f, 0.4f), 3.2f, 0.45f);
            Bella.BeginActivity(new Vector3(0f, 0f, 0.2f), 180f, "Sit", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Excited);
                Souvenir();
            });
        }

        private void Souvenir()
        {
            if (souvenirShown || def == null) return;
            souvenirShown = true;
            SaveData d = SaveSystem.Data;
            d.countVisits++;
            if (!d.visitedPlaces.Contains(def.Id))
            {
                d.visitedPlaces.Add(def.Id);
                Bella.Needs.AddTreats(5);
                hint.text = "First visit! Bella found a souvenir  +5 treats";
                AudioManager.Play("success", 0.8f);
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.35f, 12);
            }
            Bella.Needs.AddHappiness(6f);
            SaveSystem.Flush();
            AudioManager.Voice("bark_happy");
        }

        private void TapGround()
        {
            Ray ray = G.Cam.ScreenPointToRay(Pointer.Position);
            Plane floor = new Plane(Vector3.up, Vector3.zero);
            float enter;
            if (!floor.Raycast(ray, out enter)) return;
            Vector3 p = ray.GetPoint(enter);
            p = Room.ClampSpot(p);
            tapCooldown = 0.4f;
            AudioManager.Play("tap", 0.5f);
            Fx.Burst(p + Vector3.up * 0.05f, "paw", 3, 0.2f, -0.2f, 0.4f, 0.8f);
            Bella.WalkToPoint(p, () =>
            {
                if (Running) { Bella.Face.Set(Expression.Happy); if (Random.value < 0.4f) AudioManager.Voice("sniff"); }
            });
        }
    }
}
