using System.Collections.Generic;
using UnityEngine;
using Bella.Core;
using Bella.Systems;

namespace Bella.World
{
    /// <summary>
    /// Dresses the living room for the festival that is on (Systems/Seasons): flat pictures from
    /// Resources/Seasons/&lt;festival&gt;_1..9 hung on the back wall, stood on the window sill and floor,
    /// or laid flat on the floor like a rangoli. Every picture is optional — a missing file is simply
    /// skipped. The everyday pennant string is hidden while a festival garland hangs.
    ///
    /// Camera note (CameraRig: 3.1 m wide at the look point): at the back wall only about x ±2.1 is
    /// on screen on a phone in portrait, so every spot below stays inside ±1.95.
    /// </summary>
    public class FestiveDecor : MonoBehaviour
    {
        private enum Kind { Wall, Stand, Flat }

        private struct Spot
        {
            public Kind kind; public Vector3 pos; public float size;
            public Spot(Kind k, float x, float y, float z, float s) { kind = k; pos = new Vector3(x, y, z); size = s; }
        }

        // Back wall face is at z 2.70; the window sky sits at 2.655 and its bars at 2.645.
        private static readonly Dictionary<string, Spot> Spots = new Dictionary<string, Spot>
        {
            { "garland",    new Spot(Kind.Wall, 0.10f, 2.74f, 2.600f, 1.70f) },   // over the window (wide pictures)
            { "topL",       new Spot(Kind.Wall, -1.25f, 3.22f, 2.640f, 1.25f) },  // high on the wall (wide pictures)
            { "topR",       new Spot(Kind.Wall, 1.35f, 3.22f, 2.640f, 1.25f) },
            { "highL",      new Spot(Kind.Wall, -1.10f, 3.20f, 2.640f, 0.50f) },
            { "highR",      new Spot(Kind.Wall, 1.30f, 3.20f, 2.640f, 0.50f) },
            { "aboveFrame", new Spot(Kind.Wall, -1.55f, 2.66f, 2.640f, 0.42f) },  // above the paw picture
            { "underFrame", new Spot(Kind.Wall, -1.55f, 1.40f, 2.640f, 0.42f) },
            { "wallR",      new Spot(Kind.Wall, 1.75f, 1.40f, 2.640f, 0.45f) },   // under the toy shelf
            { "pane",       new Spot(Kind.Wall, -0.22f, 2.30f, 2.650f, 0.42f) },  // in the window, upper left pane
            { "pane2",      new Spot(Kind.Wall, 0.42f, 1.72f, 2.650f, 0.28f) },   // lower right pane
            { "sillL",      new Spot(Kind.Stand, -0.45f, 1.385f, 2.560f, 0.32f) },
            { "sillC",      new Spot(Kind.Stand, 0.10f, 1.385f, 2.560f, 0.34f) },
            { "sillR",      new Spot(Kind.Stand, 0.65f, 1.385f, 2.560f, 0.32f) },
            { "floorL",     new Spot(Kind.Stand, -1.90f, 0f, 2.500f, 0.80f) },    // peeks out from behind the armchair
            { "floorR",     new Spot(Kind.Stand, 0.55f, 0f, 2.450f, 0.42f) },     // between the lamp and the basket
            { "flat",       new Spot(Kind.Flat, 0.30f, 0.014f, 1.80f, 0.80f) },   // in front of the window, behind the rug
            { "flat2",      new Spot(Kind.Flat, 1.00f, 0.014f, -0.45f, 0.60f) },  // front right
        };

        /// <summary>Which picture (1-9, see docs/ASSET_PROMPTS_V2.md Pack I) goes where, per festival. Scale 1 = the spot's size.</summary>
        private static readonly Dictionary<string, (int pic, string spot, float scale)[]> Layouts =
            new Dictionary<string, (int, string, float)[]>
        {
            { "lights", new[] {
                (5, "garland", 1f), (8, "topL", 1f), (8, "topR", 1f), (1, "sillL", 1f), (7, "sillC", 1f), (1, "sillR", 1f),
                (2, "flat", 1f), (3, "flat2", 1f), (4, "aboveFrame", 1f), (9, "pane", 1f), (4, "wallR", 1f) } },
            { "colours", new[] {
                (9, "garland", 1f), (1, "sillL", 1f), (2, "sillC", 1f), (3, "sillR", 1f), (4, "aboveFrame", 1f),
                (5, "wallR", 1f), (6, "highR", 1f), (4, "highL", 0.8f), (7, "floorR", 0.9f), (8, "floorL", 0.6f), (5, "flat2", 1f) } },
            { "winter", new[] {
                (7, "garland", 1f), (7, "topL", 1f), (7, "topR", 1f), (1, "floorL", 1f), (4, "aboveFrame", 1f), (2, "wallR", 1f),
                (5, "sillL", 1f), (6, "sillC", 1f), (9, "sillR", 1f), (8, "pane", 1f), (8, "pane2", 1f), (3, "underFrame", 1f) } },
            { "birthday", new[] {
                (5, "garland", 1f), (5, "topL", 1f), (5, "topR", 1f), (1, "floorL", 1f), (2, "floorR", 1.3f), (4, "sillC", 1.1f),
                (6, "sillL", 1f), (8, "sillR", 1f), (3, "aboveFrame", 1f), (9, "wallR", 1f) } },
        };

        private Room room;
        private GameObject root;
        private string shownId = "";
        private readonly List<Transform> sway = new List<Transform>();
        private readonly List<Transform> glow = new List<Transform>();
        private readonly List<Vector3> glowBase = new List<Vector3>();

        public string ShownId => shownId;

        public void Init(Room r)
        {
            room = r;
            root = new GameObject("FestiveDecor");
            root.transform.SetParent(room.transform, false);    // a Room child, so places hide it with the rest of the decor
            root.SetActive(false);
        }

        /// <summary>Show this festival's decorations (null = none). Cheap when nothing changed.</summary>
        public void Apply(FestivalDef f)
        {
            string id = f != null ? f.id : "";
            if (id != shownId)
            {
                shownId = id;
                Clear();
                if (f != null) Build(f);
            }
            Sync();
        }

        /// <summary>Visibility follows the festival, but never draws over a place backdrop.</summary>
        public void Sync()
        {
            if (room == null || root == null) return;
            bool on = shownId != "" && root.transform.childCount > 0;
            if (room.InPlace) return;   // Room restores what it hid when the place closes; the next Sync fixes the rest
            root.SetActive(on);
            if (room.Bunting != null) room.Bunting.SetActive(!on);
        }

        private void Clear()
        {
            sway.Clear(); glow.Clear(); glowBase.Clear();
            for (int i = root.transform.childCount - 1; i >= 0; i--) Destroy(root.transform.GetChild(i).gameObject);
        }

        private void Build(FestivalDef f)
        {
            (int pic, string spot, float scale)[] layout;
            if (!Layouts.TryGetValue(f.id, out layout)) return;
            int made = 0;
            bool cake = false;
            if (f.id == "birthday" && Spots.TryGetValue("floorR", out Spot cakeSpot))
                cake = Art.Prop("food_cake", root.transform, cakeSpot.pos, 0.36f, 190f) != null;   // the model takes the "floorR" place of the picture
            for (int i = 0; i < layout.Length; i++)
            {
                if (cake && layout[i].spot == "floorR") continue;
                Texture2D tex = Art.TryTex("Seasons/" + f.id + "_" + layout[i].pic);
                Spot s;
                if (tex == null || !Spots.TryGetValue(layout[i].spot, out s)) continue;
                tex.wrapMode = TextureWrapMode.Clamp;
                float size = s.size * layout[i].scale;

                GameObject q = GameObject.CreatePrimitive(PrimitiveType.Quad);
                q.name = f.id + "_" + layout[i].pic + "@" + layout[i].spot;
                Destroy(q.GetComponent<Collider>());     // decoration only: taps go through to the room
                q.transform.SetParent(root.transform, false);
                Vector3 p = s.pos;
                if (s.kind == Kind.Stand) p.y += size * 0.47f;    // pictures keep ~6 % empty margin: sink a little so they touch down
                q.transform.localPosition = p;
                q.transform.localScale = new Vector3(size, size, 1f);
                if (s.kind == Kind.Flat) q.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                Renderer r = q.GetComponent<Renderer>();
                r.sharedMaterial = Art.SpriteMat(Color.white, tex);
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                made++;

                if (s.kind == Kind.Wall && (layout[i].spot == "garland" || layout[i].spot.StartsWith("top"))) sway.Add(q.transform);
                else if (layout[i].spot == "pane" || layout[i].spot == "sillL" || layout[i].spot == "sillR")
                {
                    glow.Add(q.transform);
                    glowBase.Add(q.transform.localScale);
                }
            }
            Diagnostics.Fact("Festival decor: " + f.id + ", " + made + " pictures");
        }

        private void Update()
        {
            if (root == null || !root.activeInHierarchy) return;
            float t = Time.time;
            for (int i = 0; i < sway.Count; i++)
                if (sway[i] != null) sway[i].localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(t * 0.9f + i * 1.7f) * 1.6f);
            for (int i = 0; i < glow.Count; i++)
                if (glow[i] != null) glow[i].localScale = glowBase[i] * (1f + 0.04f * Mathf.Sin(t * 3.1f + i * 2.3f));
        }
    }
}
