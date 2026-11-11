using System;
using System.Collections;
using UnityEngine;
using Bella.Core;

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
        public Vector3 BedPosition => new Vector3(-0.92f, 0f, 1.2f);
        public Vector3 BowlPosition => new Vector3(1.0f, 0f, 1.0f);
        public Vector3 BowlStandPoint => BowlPosition + (HomePoint - BowlPosition).normalized * 0.5f;

        public Renderer WindowSky { get; private set; }
        public Renderer WindowOrb { get; private set; }

        private GameObject kibble;
        private GameObject ball;
        private GameObject tub;
        private Vector3 ballHome = new Vector3(-1.2f, 0.11f, -0.2f);
        private Coroutine ballRoutine;

        public void Build()
        {
            Transform t = transform;
            var wood = new Color(0.94f, 0.80f, 0.62f);
            var wall = new Color(1.00f, 0.91f, 0.82f);
            var trim = new Color(1.00f, 0.98f, 0.94f);

            // Floor + rug
            GameObject floor = Art.Prim(PrimitiveType.Plane, "Floor", t, new Vector3(0f, 0f, 1f), new Vector3(1.2f, 1f, 1.2f), wood);
            floor.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Art.Prim(PrimitiveType.Cylinder, "Rug", t, new Vector3(0f, 0.004f, 0.25f), new Vector3(2.5f, 0.004f, 1.9f), Art.Teal);
            Art.Prim(PrimitiveType.Cylinder, "RugInner", t, new Vector3(0f, 0.008f, 0.25f), new Vector3(2.05f, 0.004f, 1.5f), new Color(0.80f, 0.95f, 0.90f));

            // Walls
            Art.Prim(PrimitiveType.Cube, "WallBack", t, new Vector3(0f, 2f, 2.75f), new Vector3(12f, 4f, 0.1f), wall);
            Art.Prim(PrimitiveType.Cube, "Baseboard", t, new Vector3(0f, 0.09f, 2.69f), new Vector3(12f, 0.18f, 0.06f), trim);
            Art.Prim(PrimitiveType.Cube, "WallLeft", t, new Vector3(-3.2f, 2f, 0.5f), new Vector3(0.1f, 4f, 8f), wall * 0.97f);
            Art.Prim(PrimitiveType.Cube, "WallRight", t, new Vector3(3.2f, 2f, 0.5f), new Vector3(0.1f, 4f, 8f), wall * 0.97f);
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

            // Plant
            Art.Prim(PrimitiveType.Cylinder, "Pot", t, new Vector3(1.75f, 0.17f, 2.25f), new Vector3(0.34f, 0.17f, 0.34f), Art.Orange);
            Art.Prim(PrimitiveType.Sphere, "Leaf1", t, new Vector3(1.75f, 0.62f, 2.25f), new Vector3(0.5f, 0.6f, 0.5f), Art.Leaf);
            Art.Prim(PrimitiveType.Sphere, "Leaf2", t, new Vector3(1.58f, 0.5f, 2.2f), new Vector3(0.3f, 0.36f, 0.3f), Art.Leaf * 0.92f);
            Art.Prim(PrimitiveType.Sphere, "Leaf3", t, new Vector3(1.92f, 0.52f, 2.22f), new Vector3(0.3f, 0.36f, 0.3f), Art.Leaf * 0.92f);

            // Bed
            Vector3 bed = BedPosition;
            Art.Prim(PrimitiveType.Cylinder, "BedBase", t, bed + new Vector3(0f, 0.07f, 0f), new Vector3(1.25f, 0.07f, 1.05f), Art.Lilac);
            Art.Prim(PrimitiveType.Cylinder, "BedCushion", t, bed + new Vector3(0f, 0.09f, 0f), new Vector3(1.0f, 0.065f, 0.82f), Art.Cream);

            // Bowl + kibble
            Vector3 bowl = BowlPosition;
            Art.Prim(PrimitiveType.Cylinder, "Bowl", t, bowl + new Vector3(0f, 0.06f, 0f), new Vector3(0.46f, 0.06f, 0.46f), Art.Rose);
            Art.Prim(PrimitiveType.Cylinder, "BowlInside", t, bowl + new Vector3(0f, 0.064f, 0f), new Vector3(0.37f, 0.06f, 0.37f), new Color(0.75f, 0.22f, 0.27f));
            kibble = Art.Prim(PrimitiveType.Sphere, "Kibble", t, bowl + new Vector3(0f, 0.105f, 0f), new Vector3(0.34f, 0.1f, 0.34f), new Color(0.62f, 0.4f, 0.22f));
            kibble.SetActive(false);

            // Ball
            ball = Art.Prim(PrimitiveType.Sphere, "Ball", t, ballHome, Vector3.one * 0.22f, Art.Rose);
            Art.Prim(PrimitiveType.Cylinder, "BallBand", ball.transform, Vector3.zero, new Vector3(1.03f, 0.12f, 1.03f), Art.Gold);

            // Bath tub (hidden until Bubble Bath)
            tub = new GameObject("Tub");
            tub.transform.SetParent(t, false);
            tub.transform.localPosition = HomePoint;
            Art.Prim(PrimitiveType.Cylinder, "TubWall", tub.transform, new Vector3(0f, 0.15f, 0f), new Vector3(1.55f, 0.15f, 1.3f), Color.white);
            Art.Prim(PrimitiveType.Cylinder, "Water", tub.transform, new Vector3(0f, 0.16f, 0f), new Vector3(1.4f, 0.15f, 1.15f), new Color(0.62f, 0.86f, 0.98f));
            for (int i = 0; i < 4; i++)
            {
                float x = (i % 2 == 0 ? -1f : 1f) * 0.55f;
                float z = (i < 2 ? -1f : 1f) * 0.42f;
                Art.Prim(PrimitiveType.Sphere, "TubFoot", tub.transform, new Vector3(x, 0.04f, z), Vector3.one * 0.14f, Art.Gold);
            }
            tub.SetActive(false);
        }

        public void SetBowlFull(bool full)
        {
            if (kibble != null) kibble.SetActive(full);
        }

        public void ShowTub(bool show)
        {
            if (tub != null) tub.SetActive(show);
        }

        public Vector3 RandomFloorPoint(float minDistanceFromHome = 0f)
        {
            for (int i = 0; i < 12; i++)
            {
                var p = new Vector3(UnityEngine.Random.Range(-0.95f, 0.95f), 0f, UnityEngine.Random.Range(-0.35f, 0.75f));
                if ((p - BedPosition).magnitude < 0.75f) continue;
                if ((p - BowlPosition).magnitude < 0.6f) continue;
                if ((p - HomePoint).magnitude < minDistanceFromHome) continue;
                return p;
            }
            return HomePoint + new Vector3(0.6f, 0f, 0.1f);
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
                p.y += Mathf.Sin(k * Mathf.PI) * 0.9f;
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
