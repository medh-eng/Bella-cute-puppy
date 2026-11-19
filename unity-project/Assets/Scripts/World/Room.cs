using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bella.Core;
using Bella.States;

namespace Bella.World
{
    /// <summary>
    /// Bella's one cozy room, built entirely from primitives at runtime: floor, rug,
    /// walls, a window that shows the real time of day, her bed, bowl and ball, and
    /// the bath tub that appears for Bubble Bath.
    /// </summary>
    public class Room : MonoBehaviour
    {
        public Vector3 HomePoint => new Vector3(0f, 0f, 0.15f);
        public Vector3 BedPosition => PosOf("bed", new Vector3(-0.92f, 0f, 1.2f));
        public Vector3 BowlPosition => PosOf("bowl", new Vector3(1.0f, 0f, 1.0f));
        public Vector3 BowlStandPoint => BowlPosition + (HomePoint - BowlPosition).normalized * 0.5f;

        // Tappable, movable furniture (see RoomObject). Positions are remembered in the save file.
        private readonly Dictionary<string, RoomObject> objs = new Dictionary<string, RoomObject>();
        public IEnumerable<RoomObject> Objects { get { return objs.Values; } }
        public RoomObject FindObject(string id) { RoomObject o; return objs.TryGetValue(id, out o) ? o : null; }

        private Vector3 PosOf(string id, Vector3 fallback)
        {
            RoomObject o;
            return objs.TryGetValue(id, out o) && o != null ? o.transform.localPosition : fallback;
        }

        public Renderer WindowSky { get; private set; }
        public Renderer WindowOrb { get; private set; }

        /// <summary>Height of the (flattened) tub's inside floor and of its water, metres.</summary>
        public const float TubFloor = 0.07f;
        public const float TubWaterY = 0.27f;

        private GameObject kibble;
        private GameObject ball;
        private GameObject tub;
        private Vector3 ballHome = new Vector3(-1.2f, 0.11f, -0.2f);
        private Coroutine ballRoutine;

        // Optional generated art: wallpapers / floors (Decor shop) and scenery backdrops (mini-game places).
        private Renderer floorRend;
        private Renderer[] wallRends;
        private Material floorOriginal;
        private Material[] wallOriginals;
        private string wallTheme = "", floorTheme = "";
        private GameObject backdrop;
        private Texture2D backdropTex;
        private readonly List<GameObject> hiddenForPlace = new List<GameObject>();
        public bool InPlace { get { return backdrop != null; } }

        public void Build()
        {
            Transform t = transform;
            var wood = new Color(0.94f, 0.80f, 0.62f);
            var wall = new Color(1.00f, 0.91f, 0.82f);
            var trim = new Color(1.00f, 0.98f, 0.94f);

            // Floor + rug
            GameObject floor = Art.Prim(PrimitiveType.Plane, "Floor", t, new Vector3(0f, 0f, 1f), new Vector3(1.2f, 1f, 1.2f), wood);
            floorRend = floor.GetComponent<Renderer>();
            floorRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            floorOriginal = floorRend.sharedMaterial;
            Art.Prim(PrimitiveType.Cylinder, "Rug", t, new Vector3(0f, 0.004f, 0.25f), new Vector3(2.5f, 0.004f, 1.9f), Art.Teal);
            Art.Prim(PrimitiveType.Cylinder, "RugInner", t, new Vector3(0f, 0.008f, 0.25f), new Vector3(2.05f, 0.004f, 1.5f), new Color(0.80f, 0.95f, 0.90f));

            // Walls
            GameObject wallBack = Art.Prim(PrimitiveType.Cube, "WallBack", t, new Vector3(0f, 1.8f, 2.75f), new Vector3(12f, 4.4f, 0.1f), wall);
            Art.Prim(PrimitiveType.Cube, "Baseboard", t, new Vector3(0f, 0.09f, 2.69f), new Vector3(12f, 0.18f, 0.06f), trim);
            GameObject wallLeft = Art.Prim(PrimitiveType.Cube, "WallLeft", t, new Vector3(-3.2f, 1.8f, 0.5f), new Vector3(0.1f, 4.4f, 8f), wall * 0.97f);
            GameObject wallRight = Art.Prim(PrimitiveType.Cube, "WallRight", t, new Vector3(3.2f, 1.8f, 0.5f), new Vector3(0.1f, 4.4f, 8f), wall * 0.97f);
            wallRends = new[] { wallBack.GetComponent<Renderer>(), wallLeft.GetComponent<Renderer>(), wallRight.GetComponent<Renderer>() };
            wallOriginals = new[] { wallRends[0].sharedMaterial, wallRends[1].sharedMaterial, wallRends[2].sharedMaterial };
            // Wainscot stripe gives the wall some life.
            Art.Prim(PrimitiveType.Cube, "Stripe", t, new Vector3(0f, 0.95f, 2.695f), new Vector3(12f, 0.07f, 0.03f), Art.Pink);

            // Window (sky + sun/moon are driven by DayNight)
            Art.Prim(PrimitiveType.Cube, "WindowFrame", t, new Vector3(0.1f, 2.0f, 2.69f), new Vector3(1.5f, 1.25f, 0.06f), trim);
            GameObject sky = GameObject.CreatePrimitive(PrimitiveType.Quad);
            sky.name = "WindowSky";
            Destroy(sky.GetComponent<Collider>());
            sky.transform.SetParent(t, false);
            sky.transform.localPosition = new Vector3(0.1f, 2.0f, 2.655f);
            sky.transform.localScale = new Vector3(1.34f, 1.09f, 1f);
            WindowSky = sky.GetComponent<Renderer>();
            WindowSky.sharedMaterial = Art.SpriteMat(Art.Sky);
            WindowSky.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Quad);
            orb.name = "WindowOrb";
            Destroy(orb.GetComponent<Collider>());
            orb.transform.SetParent(t, false);
            orb.transform.localPosition = new Vector3(0.38f, 2.2f, 2.65f);
            orb.transform.localScale = new Vector3(0.42f, 0.42f, 1f);
            WindowOrb = orb.GetComponent<Renderer>();
            WindowOrb.sharedMaterial = Art.SpriteMat(Color.white, Art.Tex("Icons/sun"));
            WindowOrb.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            Art.Prim(PrimitiveType.Cube, "WindowBarV", t, new Vector3(0.1f, 2.0f, 2.645f), new Vector3(0.05f, 1.1f, 0.02f), trim);
            Art.Prim(PrimitiveType.Cube, "WindowBarH", t, new Vector3(0.1f, 2.0f, 2.645f), new Vector3(1.36f, 0.05f, 0.02f), trim);
            Art.Prim(PrimitiveType.Cube, "WindowSill", t, new Vector3(0.1f, 1.35f, 2.62f), new Vector3(1.7f, 0.07f, 0.2f), trim);
            Art.Prim(PrimitiveType.Cube, "CurtainL", t, new Vector3(-0.78f, 2.0f, 2.62f), new Vector3(0.3f, 1.45f, 0.05f), Art.Pink);
            Art.Prim(PrimitiveType.Cube, "CurtainR", t, new Vector3(0.98f, 2.0f, 2.62f), new Vector3(0.3f, 1.45f, 0.05f), Art.Pink);
            Art.Prim(PrimitiveType.Cylinder, "CurtainRod", t, new Vector3(0.1f, 2.76f, 2.62f), new Vector3(0.04f, 1.05f, 0.04f), Art.Gold, new Vector3(0f, 0f, 90f));

            // Framed paw picture
            Art.Prim(PrimitiveType.Cube, "Frame", t, new Vector3(-1.55f, 2.05f, 2.69f), new Vector3(0.62f, 0.62f, 0.05f), Art.Gold);
            GameObject pic = GameObject.CreatePrimitive(PrimitiveType.Quad);
            pic.name = "Picture";
            Destroy(pic.GetComponent<Collider>());
            pic.transform.SetParent(t, false);
            pic.transform.localPosition = new Vector3(-1.55f, 2.05f, 2.66f);
            pic.transform.localScale = new Vector3(0.52f, 0.52f, 1f);
            pic.GetComponent<Renderer>().sharedMaterial = Art.SpriteMat(Color.white, Art.Tex("Icons/paw"));

            // ---- furniture: tap = Bella walks there, long-press then drag = move it (positions are saved)
            RoomObject plant = MakeObject("plant", new Vector3(1.75f, 0f, 2.25f), 0.2f, 0.9f);
            Art.Prim(PrimitiveType.Cylinder, "Pot", plant.transform, new Vector3(0f, 0.17f, 0f), new Vector3(0.34f, 0.17f, 0.34f), Art.Orange);
            Art.Prim(PrimitiveType.Sphere, "Leaf1", plant.transform, new Vector3(0f, 0.62f, 0f), new Vector3(0.5f, 0.6f, 0.5f), Art.Leaf);
            Art.Prim(PrimitiveType.Sphere, "Leaf2", plant.transform, new Vector3(-0.17f, 0.5f, -0.05f), new Vector3(0.3f, 0.36f, 0.3f), Art.Leaf * 0.92f);
            Art.Prim(PrimitiveType.Sphere, "Leaf3", plant.transform, new Vector3(0.17f, 0.52f, -0.03f), new Vector3(0.3f, 0.36f, 0.3f), Art.Leaf * 0.92f);

            RoomObject chair = MakeObject("armchair", new Vector3(-1.25f, 0f, 2.1f), 0.5f, 0.9f, 12f);
            Art.Prop("armchair", chair.transform, Vector3.zero, 0.9f);
            RoomObject lamp = MakeObject("nightlight", new Vector3(-0.3f, 0f, 2.35f), 0.2f, 0.6f);
            Art.Prop("nightlight", lamp.transform, Vector3.zero, 0.6f);
            RoomObject water = MakeObject("waterbowl", new Vector3(1.48f, 0f, 1.28f), 0.2f, 0.2f);
            Art.Prop("waterbowl", water.transform, Vector3.zero, 0.4f);

            BuildBunting(t);
            BuildToyShelf(t);
            BuildToyBasket(t);

            // Bed
            RoomObject bedObj = MakeObject("bed", new Vector3(-0.92f, 0f, 1.2f), 0.62f, 0.3f);
            if (Art.Prop("bed", bedObj.transform, Vector3.zero, 1.25f) == null)
            {
                Art.Prim(PrimitiveType.Cylinder, "BedBase", bedObj.transform, new Vector3(0f, 0.07f, 0f), new Vector3(1.25f, 0.07f, 1.05f), Art.Lilac);
                Art.Prim(PrimitiveType.Cylinder, "BedCushion", bedObj.transform, new Vector3(0f, 0.09f, 0f), new Vector3(1.0f, 0.065f, 0.82f), Art.Cream);
            }

            // Bowl + kibble
            RoomObject bowlObj = MakeObject("bowl", new Vector3(1.0f, 0f, 1.0f), 0.25f, 0.25f);
            if (Art.Prop("bowl", bowlObj.transform, Vector3.zero, 0.46f) == null)
            {
                Art.Prim(PrimitiveType.Cylinder, "Bowl", bowlObj.transform, new Vector3(0f, 0.06f, 0f), new Vector3(0.46f, 0.06f, 0.46f), Art.Rose);
                Art.Prim(PrimitiveType.Cylinder, "BowlInside", bowlObj.transform, new Vector3(0f, 0.064f, 0f), new Vector3(0.37f, 0.06f, 0.37f), new Color(0.75f, 0.22f, 0.27f));
            }
            kibble = Art.Prop("kibble", bowlObj.transform, new Vector3(0f, 0.05f, 0f), 0.34f);
            if (kibble == null)
                kibble = Art.Prim(PrimitiveType.Sphere, "Kibble", bowlObj.transform, new Vector3(0f, 0.105f, 0f), new Vector3(0.34f, 0.1f, 0.34f), new Color(0.62f, 0.4f, 0.22f));
            kibble.SetActive(false);

            WalkToState.Steer = Steer;
            WalkToState.Clamp = PushOut;

            // Ball
            // generated model: an empty "Ball" centre point with the model hung around it (it moves and spins as before)
            var ballRoot = new GameObject("Ball");
            ballRoot.transform.SetParent(t, false);
            ballRoot.transform.localPosition = ballHome;
            if (Art.Prop("ball", ballRoot.transform, new Vector3(0f, -0.11f, 0f), 0.22f) != null) ball = ballRoot;
            else
            {
                Destroy(ballRoot);
                ball = Art.Prim(PrimitiveType.Sphere, "Ball", t, ballHome, Vector3.one * 0.22f, Art.Rose);
                Art.Prim(PrimitiveType.Cylinder, "BallBand", ball.transform, Vector3.zero, new Vector3(1.03f, 0.12f, 1.03f), Art.Gold);
            }

            // the ball can be tapped (she fetches it) but not moved
            SphereCollider ballCol = ball.AddComponent<SphereCollider>();
            ballCol.radius = ball == ballRoot ? 0.2f : 0.9f;
            RoomObject ballObj = ball.AddComponent<RoomObject>();
            ballObj.id = "ball"; ballObj.movable = false; ballObj.solid = false; ballObj.radius = 0.15f; ballObj.height = 0.25f;

            // Bath tub (hidden until Bubble Bath)
            tub = new GameObject("Tub");
            tub.transform.SetParent(t, false);
            tub.transform.localPosition = HomePoint;
            GameObject tubModel = Art.Prop("tub", tub.transform, Vector3.zero, 1.45f);
            if (tubModel != null)
            {
                // The generated tub is tall (rim about 0.6 m): squash it to 55 % so Bella sits in it with her head
                // and chest showing. Its inside floor is then about 0.07 m high (BathActivity lifts her by TubFloor).
                tubModel.transform.localScale = new Vector3(1f, 0.55f, 1f);
                Art.Prim(PrimitiveType.Cylinder, "Water", tub.transform, new Vector3(0f, TubWaterY, 0f), new Vector3(1.15f, 0.02f, 0.58f), new Color(0.62f, 0.86f, 0.98f));
                Art.Prop("toy_duck", tub.transform, new Vector3(0.35f, TubWaterY + 0.01f, 0.08f), 0.22f, -25f);
            }
            else
            {
                Art.Prim(PrimitiveType.Cylinder, "TubWall", tub.transform, new Vector3(0f, 0.15f, 0f), new Vector3(1.55f, 0.15f, 1.3f), Color.white);
                Art.Prim(PrimitiveType.Cylinder, "Water", tub.transform, new Vector3(0f, 0.16f, 0f), new Vector3(1.4f, 0.15f, 1.15f), new Color(0.62f, 0.86f, 0.98f));
                for (int i = 0; i < 4; i++)
                {
                    float x = (i % 2 == 0 ? -1f : 1f) * 0.55f;
                    float z = (i < 2 ? -1f : 1f) * 0.42f;
                    Art.Prim(PrimitiveType.Sphere, "TubFoot", tub.transform, new Vector3(x, 0.04f, z), Vector3.one * 0.14f, Art.Gold);
                }
            }
            tub.SetActive(false);
        }

        // ---------------------------------------------------------------- decor

        private static Mesh pennantMesh;

        /// <summary>Unit pennant triangle hanging point-down, facing the camera.</summary>
        private static Mesh PennantMesh()
        {
            if (pennantMesh != null) return pennantMesh;
            var verts = new System.Collections.Generic.List<Vector3>
            {
                new Vector3(-0.5f, 0f, 0f), new Vector3(0.5f, 0f, 0f), new Vector3(0f, -1f, 0f)
            };
            var tris = new System.Collections.Generic.List<int> { 0, 1, 2 };
            pennantMesh = new Mesh();
            pennantMesh.name = "BellaPennant";
            pennantMesh.SetVertices(verts);
            pennantMesh.SetTriangles(tris, 0);
            pennantMesh.RecalculateNormals();
            pennantMesh.RecalculateBounds();
            return pennantMesh;
        }

        /// <summary>A string of party pennants near the ceiling — the room is always a little festive.</summary>
        private void BuildBunting(Transform t)
        {
            Art.Prim(PrimitiveType.Cube, "BuntingRibbon", t, new Vector3(0f, 2.96f, 2.67f), new Vector3(5.3f, 0.03f, 0.02f), Art.Gold);
            Color[] cols = { Art.Pink, Art.Gold, Art.Teal, Art.Lilac, Art.Leaf, Art.Orange };
            const int count = 11;
            for (int i = 0; i < count; i++)
            {
                float k = i / (float)(count - 1);
                float x = Mathf.Lerp(-2.45f, 2.45f, k);
                float droop = 0.1f * Mathf.Sin(k * Mathf.PI * 2f);   // two gentle waves
                var flag = new GameObject("Pennant" + i);
                flag.transform.SetParent(t, false);
                flag.transform.localPosition = new Vector3(x, 2.94f - Mathf.Abs(droop), 2.665f);
                flag.transform.localScale = new Vector3(0.24f, 0.3f, 1f);
                flag.transform.localRotation = Quaternion.Euler(0f, 0f, droop * 40f);
                flag.AddComponent<MeshFilter>().sharedMesh = PennantMesh();
                flag.AddComponent<MeshRenderer>().sharedMaterial = Art.Lit(cols[i % cols.Length]);
            }
        }

        /// <summary>A little shelf on the back wall with a teddy, picture books and a spare ball.</summary>
        private void BuildToyShelf(Transform t)
        {
            if (Art.Prop("toyshelf", t, new Vector3(2.05f, 1.82f, 2.55f), 1.0f) != null) return;
            Art.Prim(PrimitiveType.Cube, "Shelf", t, new Vector3(2.05f, 1.9f, 2.6f), new Vector3(1.05f, 0.055f, 0.24f), new Color(1.00f, 0.98f, 0.94f));
            Art.Prim(PrimitiveType.Cube, "ShelfBracketL", t, new Vector3(1.65f, 1.8f, 2.66f), new Vector3(0.05f, 0.16f, 0.1f), new Color(1.00f, 0.98f, 0.94f));
            Art.Prim(PrimitiveType.Cube, "ShelfBracketR", t, new Vector3(2.45f, 1.8f, 2.66f), new Vector3(0.05f, 0.16f, 0.1f), new Color(1.00f, 0.98f, 0.94f));

            // Teddy bear
            var brown = new Color(0.72f, 0.52f, 0.34f);
            var creamy = new Color(0.95f, 0.86f, 0.70f);
            Vector3 b = new Vector3(1.78f, 1.93f, 2.6f);
            Art.Prim(PrimitiveType.Sphere, "TeddyBody", t, b + new Vector3(0f, 0.085f, 0f), new Vector3(0.16f, 0.17f, 0.14f), brown);
            Art.Prim(PrimitiveType.Sphere, "TeddyTummy", t, b + new Vector3(0f, 0.08f, -0.045f), new Vector3(0.1f, 0.11f, 0.06f), creamy);
            Art.Prim(PrimitiveType.Sphere, "TeddyHead", t, b + new Vector3(0f, 0.22f, 0f), new Vector3(0.13f, 0.12f, 0.12f), brown);
            Art.Prim(PrimitiveType.Sphere, "TeddySnout", t, b + new Vector3(0f, 0.2f, -0.055f), new Vector3(0.055f, 0.045f, 0.04f), creamy);
            Art.Prim(PrimitiveType.Sphere, "TeddyEarL", t, b + new Vector3(-0.055f, 0.285f, 0f), new Vector3(0.05f, 0.05f, 0.03f), brown);
            Art.Prim(PrimitiveType.Sphere, "TeddyEarR", t, b + new Vector3(0.055f, 0.285f, 0f), new Vector3(0.05f, 0.05f, 0.03f), brown);
            Art.Prim(PrimitiveType.Sphere, "TeddyArmL", t, b + new Vector3(-0.085f, 0.1f, -0.02f), new Vector3(0.05f, 0.09f, 0.05f), brown);
            Art.Prim(PrimitiveType.Sphere, "TeddyArmR", t, b + new Vector3(0.085f, 0.1f, -0.02f), new Vector3(0.05f, 0.09f, 0.05f), brown);

            // Picture books, leaning sizes, and a spare ball
            Art.Prim(PrimitiveType.Cube, "Book1", t, new Vector3(2.12f, 2.05f, 2.62f), new Vector3(0.055f, 0.24f, 0.17f), Art.Rose);
            Art.Prim(PrimitiveType.Cube, "Book2", t, new Vector3(2.19f, 2.04f, 2.62f), new Vector3(0.055f, 0.21f, 0.16f), Art.Teal);
            Art.Prim(PrimitiveType.Cube, "Book3", t, new Vector3(2.27f, 2.03f, 2.62f), new Vector3(0.055f, 0.19f, 0.15f), Art.Gold, new Vector3(0f, 0f, -8f));
            Art.Prim(PrimitiveType.Sphere, "ShelfBall", t, new Vector3(2.43f, 1.99f, 2.6f), Vector3.one * 0.12f, Art.Sky);
        }

        /// <summary>A wicker toy basket in the back corner, out of Bella's wander area.</summary>
        private void BuildToyBasket(Transform t)
        {
            RoomObject o = MakeObject("basket", new Vector3(1.15f, 0f, 2.2f), 0.28f, 0.5f);
            if (Art.Prop("basket", o.transform, Vector3.zero, 0.55f) != null) return;
            var wicker = new Color(0.82f, 0.62f, 0.40f);
            Transform b = o.transform;
            Art.Prim(PrimitiveType.Cylinder, "Basket", b, new Vector3(0f, 0.16f, 0f), new Vector3(0.55f, 0.16f, 0.55f), wicker);
            Art.Prim(PrimitiveType.Cylinder, "BasketInner", b, new Vector3(0f, 0.17f, 0f), new Vector3(0.47f, 0.16f, 0.47f), wicker * 0.75f);
            Art.Prim(PrimitiveType.Sphere, "BasketBall1", b, new Vector3(-0.1f, 0.3f, -0.05f), Vector3.one * 0.17f, Art.Teal);
            Art.Prim(PrimitiveType.Sphere, "BasketBall2", b, new Vector3(0.1f, 0.31f, 0.08f), Vector3.one * 0.16f, Art.Pink);
            Art.Prim(PrimitiveType.Sphere, "BasketBall3", b, new Vector3(0.02f, 0.34f, -0.12f), Vector3.one * 0.14f, Art.Gold);
        }

        // ---------------------------------------------------------------- tappable / movable furniture

        private RoomObject MakeObject(string id, Vector3 defaultPos, float radius, float height, float yaw = 0f)
        {
            var go = new GameObject("Obj_" + id);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = SavedSpot(id, defaultPos);
            go.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
            BoxCollider box = go.AddComponent<BoxCollider>();
            box.center = new Vector3(0f, height * 0.5f, 0f);
            box.size = new Vector3(radius * 2f, height, radius * 2f);
            RoomObject o = go.AddComponent<RoomObject>();
            o.id = id; o.radius = radius; o.height = height; o.home = defaultPos;
            objs[id] = o;
            return o;
        }

        private static Vector3 SavedSpot(string id, Vector3 fallback)
        {
            var list = Bella.Save.SaveSystem.Data.objectSpots;
            for (int i = 0; i < list.Count; i++)
            {
                string[] p = list[i].Split('|');
                float x, z;
                if (p.Length == 3 && p[0] == id
                    && float.TryParse(p[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out x)
                    && float.TryParse(p[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out z))
                    return ClampSpot(new Vector3(x, 0f, z));
            }
            return fallback;
        }

        /// <summary>Keeps furniture inside what the camera shows.</summary>
        public static Vector3 ClampSpot(Vector3 p)
        {
            return new Vector3(Mathf.Clamp(p.x, -1.6f, 1.6f), 0f, Mathf.Clamp(p.z, -0.7f, 2.4f));
        }

        /// <summary>Put a piece of furniture down at a new spot and remember it.</summary>
        public void SetObjectSpot(RoomObject o, Vector3 p, bool save)
        {
            if (o == null) return;
            o.transform.localPosition = ClampSpot(p);
            if (!save) return;
            var list = Bella.Save.SaveSystem.Data.objectSpots;
            for (int i = list.Count - 1; i >= 0; i--) if (list[i].StartsWith(o.id + "|")) list.RemoveAt(i);
            Vector3 q = o.transform.localPosition;
            list.Add(o.id + "|" + q.x.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture) + "|" +
                     q.z.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            Bella.Save.SaveSystem.Flush();
        }

        /// <summary>Where Bella may walk: the floor the camera shows.</summary>
        public static Vector3 ClampWalk(Vector3 p)
        {
            return new Vector3(Mathf.Clamp(p.x, -1.6f, 1.6f), 0f, Mathf.Clamp(p.z, -0.6f, 2.3f));
        }

        private const float WalkMargin = 0.2f;

        /// <summary>Pushes a floor point out of every solid piece of furniture.</summary>
        public Vector3 PushOut(Vector3 p)
        {
            foreach (RoomObject o in objs.Values)
            {
                if (o == null || !o.solid || !o.gameObject.activeInHierarchy) continue;
                Vector3 c = o.transform.localPosition;
                float dx = p.x - c.x, dz = p.z - c.z;
                float r = o.radius + WalkMargin, m = Mathf.Sqrt(dx * dx + dz * dz);
                if (m >= r) continue;
                if (m < 0.001f) { dx = 0f; dz = -1f; m = 1f; }
                p.x = c.x + dx / m * r;
                p.z = c.z + dz / m * r;
            }
            return p;
        }

        /// <summary>
        /// Next point to aim at when walking from -> to: the target itself, or a point beside the first
        /// piece of furniture that is in the way.
        /// </summary>
        public Vector3 Steer(Vector3 from, Vector3 to)
        {
            Vector3 d = to - from; d.y = 0f;
            float len = d.magnitude;
            if (len < 0.05f) return to;
            Vector3 dir = d / len;
            RoomObject block = null; float bestAlong = len;
            foreach (RoomObject o in objs.Values)
            {
                if (o == null || !o.solid || !o.gameObject.activeInHierarchy) continue;
                Vector3 c = o.transform.localPosition;
                float r = o.radius + WalkMargin;
                Vector3 toC = new Vector3(c.x - from.x, 0f, c.z - from.z);
                float along = Vector3.Dot(toC, dir);
                if (along < 0.05f || along > len + r) continue;
                float side = Mathf.Abs(toC.x * dir.z - toC.z * dir.x);       // distance from the centre to the line of travel
                if (side >= r) continue;
                Vector3 tt = new Vector3(c.x - to.x, 0f, c.z - to.z);
                if (tt.magnitude < r) continue;                                // the target is next to / inside it: just go there
                if (along < bestAlong) { bestAlong = along; block = o; }
            }
            if (block == null) return to;
            Vector3 cc = block.transform.localPosition;
            float rr = block.radius + WalkMargin + 0.08f;
            Vector3 toCc = new Vector3(cc.x - from.x, 0f, cc.z - from.z);
            float cross = toCc.x * dir.z - toCc.z * dir.x;                     // which side the centre lies on
            Vector3 perp = cross > 0f ? new Vector3(dir.z, 0f, -dir.x) : new Vector3(-dir.z, 0f, dir.x);
            Vector3 way = new Vector3(cc.x, 0f, cc.z) + perp * -rr;
            // aim just past the obstacle, on the clear side
            return new Vector3(way.x, from.y, way.z);
        }

        // ---------------------------------------------------------------- generated surfaces and places

        /// <summary>
        /// Wallpaper and floor from Resources/Surfaces (empty id, or a missing file, = the original
        /// primitive-colour look). Safe to call again at any time.
        /// </summary>
        public void ApplyTheme(string wallId, string floorId)
        {
            wallTheme = wallId ?? ""; floorTheme = floorId ?? "";
            SetSurface(floorRend, floorOriginal, floorTheme, new Vector2(6f, 6f));
            if (wallRends == null) return;
            Vector2[] tiling = { new Vector2(6f, 2f), new Vector2(4f, 2f), new Vector2(4f, 2f) };
            for (int i = 0; i < wallRends.Length; i++)
                SetSurface(wallRends[i], wallOriginals[i], wallTheme, tiling[i]);
        }

        private static void SetSurface(Renderer r, Material original, string id, Vector2 tiling)
        {
            if (r == null) return;
            Texture2D tex = string.IsNullOrEmpty(id) ? null : Art.TileTex("Surfaces/" + id);
            if (tex == null) { r.sharedMaterial = original; return; }
            Material m = Art.Lit(Color.white, tex);
            m.mainTextureScale = tiling;
            r.sharedMaterial = m;
        }

        /// <summary>
        /// Swap the room for a scenery backdrop (Resources/Places/id.jpg) while a mini-game runs: the wall
        /// decor, bed and bowl are hidden, the picture hangs just in front of the back wall, and the floor
        /// can change too. Returns false (and changes nothing) when the picture is missing.
        /// </summary>
        public bool ShowPlace(string placeId, string floorId)
        {
            if (string.IsNullOrEmpty(placeId) || backdrop != null) return false;
            Texture2D tex = Resources.Load<Texture2D>("Places/" + placeId);
            if (tex == null) return false;
            tex.wrapMode = TextureWrapMode.Clamp;
            backdropTex = tex;

            // Hide everything on the wall and the furniture; keep floor, walls, tub and ball.
            hiddenForPlace.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject c = transform.GetChild(i).gameObject;
                string n = c.name;
                if (n == "Floor" || n == "WallBack" || n == "WallLeft" || n == "WallRight" || n == "Tub" || n == "Ball") continue;
                if (!c.activeSelf) continue;
                c.SetActive(false);
                hiddenForPlace.Add(c);
            }

            backdrop = GameObject.CreatePrimitive(PrimitiveType.Quad);
            backdrop.name = "PlaceBackdrop";
            Destroy(backdrop.GetComponent<Collider>());
            backdrop.transform.SetParent(transform, false);
            backdrop.transform.localPosition = new Vector3(0f, 1.9f, 2.695f);   // bottom edge a hand below the floor, so no gap   // just in front of the back wall's face (z 2.70)
            backdrop.transform.localScale = new Vector3(6.3f, 4.2f, 1f);
            Renderer r = backdrop.GetComponent<Renderer>();
            r.sharedMaterial = Art.SpriteMat(Color.white, tex);
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;

            if (!string.IsNullOrEmpty(floorId)) SetSurface(floorRend, floorOriginal, floorId, new Vector2(6f, 6f));
            return true;
        }

        /// <summary>Back to the living room: restores hidden decor and the player's chosen wallpaper and floor.</summary>
        public void HomePlace()
        {
            if (backdrop == null) return;
            Destroy(backdrop);
            backdrop = null;
            for (int i = 0; i < hiddenForPlace.Count; i++)
                if (hiddenForPlace[i] != null) hiddenForPlace[i].SetActive(true);
            hiddenForPlace.Clear();
            backdropTex = null;
            ApplyTheme(wallTheme, floorTheme);
            Resources.UnloadUnusedAssets();
        }

        public void SetBowlFull(bool full)
        {
            if (kibble != null) kibble.SetActive(full);
        }

        public void ShowTub(bool show)
        {
            if (tub != null) tub.SetActive(show);
        }

        private bool NearFurniture(Vector3 p, float extra)
        {
            foreach (RoomObject o in objs.Values)
            {
                if (o == null || !o.solid) continue;
                Vector3 c = o.transform.localPosition;
                float dx = p.x - c.x, dz = p.z - c.z;
                if (dx * dx + dz * dz < (o.radius + extra) * (o.radius + extra)) return true;
            }
            return false;
        }

        public Vector3 RandomFloorPoint(float minDistanceFromHome = 0f)
        {
            for (int i = 0; i < 16; i++)
            {
                var p = new Vector3(UnityEngine.Random.Range(-0.95f, 0.95f), 0f, UnityEngine.Random.Range(-0.35f, 0.75f));
                if (NearFurniture(p, 0.35f)) continue;
                if ((p - HomePoint).magnitude < minDistanceFromHome) continue;
                return p;
            }
            return HomePoint + new Vector3(0.6f, 0f, 0.1f);
        }

        /// <summary>A floor point at least minDistance from where she stands, clear of furniture (for fetch).</summary>
        public Vector3 RandomFloorPointAwayFrom(Vector3 from, float minDistance)
        {
            for (int i = 0; i < 30; i++)
            {
                var p = new Vector3(UnityEngine.Random.Range(-1.2f, 1.2f), 0f, UnityEngine.Random.Range(-0.4f, 1.0f));
                if (NearFurniture(p, 0.4f)) continue;
                if ((p - from).magnitude < minDistance) continue;
                return p;
            }
            // nothing fits: the far side of the room from her
            return new Vector3(from.x > 0f ? -1.0f : 1.0f, 0f, 0.3f);
        }

        // ---------------------------------------------------------------- ball

        public void ThrowBall(Vector3 landing, Action landed)
        {
            if (ballRoutine != null) StopCoroutine(ballRoutine);
            ballRoutine = StartCoroutine(BallArc(landing, landed));
        }

        public void HideBall()
        {
            if (ball != null) ball.SetActive(false);
        }

        public void ResetBall()
        {
            if (ball == null) return;
            ball.SetActive(true);
            ball.transform.localPosition = ballHome;
        }

        private IEnumerator BallArc(Vector3 landing, Action landed)
        {
            ball.SetActive(true);
            Vector3 from = new Vector3(landing.x * 0.3f, 1.0f, -2.2f);
            Vector3 to = new Vector3(landing.x, 0.11f, landing.z);
            const float dur = 0.75f;
            for (float e = 0f; e < dur; e += Time.deltaTime)
            {
                float k = e / dur;
                Vector3 p = Vector3.Lerp(from, to, k);
                p.y += Mathf.Sin(k * Mathf.PI) * 1.7f;      // a high lob: it comes down in front of Bella, never through her
                ball.transform.position = p;
                ball.transform.Rotate(360f * Time.deltaTime, 0f, 0f);
                yield return null;
            }
            // one little bounce
            const float bounce = 0.28f;
            for (float e = 0f; e < bounce; e += Time.deltaTime)
            {
                Vector3 p = to;
                p.y += Mathf.Sin(e / bounce * Mathf.PI) * 0.16f;
                ball.transform.position = p;
                yield return null;
            }
            ball.transform.position = to;
            ballRoutine = null;
            if (landed != null) landed();
        }
    }
}
