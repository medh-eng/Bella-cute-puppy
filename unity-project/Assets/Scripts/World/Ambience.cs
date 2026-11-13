using System.Collections.Generic;
using UnityEngine;
using Bella.Core;
using Bella.Needs;
using Bella.Pet;
using Bella.Save;

namespace Bella.World
{
    /// <summary>
    /// The little touches that make the room feel alive (docs/GDD.md §2):
    /// clouds drift past the window by day and stars twinkle at night;
    /// butterflies with heart-shaped wings flutter around in the daytime and
    /// fireflies glow after dark; a soft star nightlight watches over the bed.
    /// Tapping a butterfly is pure delight — sparkles, a happy yip, a little
    /// happiness, and its own sticker. Everything is quads + icons, no assets.
    /// </summary>
    public class Ambience : MonoBehaviour
    {
        private Room room;
        private DayNight dayNight;
        private Camera cam;
        private PetNeeds needs;
        private BellaController bella;

        // -- window dressing
        private readonly List<CloudData> clouds = new List<CloudData>();
        private readonly List<StarData> stars = new List<StarData>();

        // -- critters
        private readonly List<Butterfly> butterflies = new List<Butterfly>();
        private readonly List<Firefly> fireflies = new List<Firefly>();
        private Renderer nightlight;
        private float time;

        private class CloudData { public Transform t; public Renderer r; public float speed, phase, y, scale; }
        private class StarData { public Transform t; public Renderer r; public float phase, speed; public Vector3 pos; }

        private class Butterfly
        {
            public Transform root, wingL, wingR;
            public Renderer[] renderers;
            public Vector3 target;
            public float speed, flapPhase, retarget, cooldown, bobPhase;
            public bool fleeing;
        }

        private class Firefly { public Transform t; public Renderer r; public Vector3 home; public float phase, drift; }

        // Butterflies keep to the airy middle of the room, above Bella's head height.
        private static readonly Vector3 FlyMin = new Vector3(-1.6f, 0.55f, 0.2f);
        private static readonly Vector3 FlyMax = new Vector3(1.8f, 1.7f, 2.3f);

        public void Init(Room r, DayNight d, Camera c, PetNeeds n, BellaController b)
        {
            room = r; dayNight = d; cam = c; needs = n; bella = b;
            BuildWindowDressing();
            BuildButterflies();
            BuildFireflies();
            BuildNightlight();
            ApplyPeriod(true);
        }

        // ---------------------------------------------------------------- build

        private Renderer Quad(string name, Transform parent, Vector3 pos, float size, string icon, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = name;
            Collider col = go.GetComponent<Collider>();
            if (col != null) Destroy(col);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localScale = new Vector3(size, size, 1f);
            Renderer rend = go.GetComponent<Renderer>();
            rend.material = Art.SpriteMat(color, Art.Tex("Icons/" + icon));
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.receiveShadows = false;
            return rend;
        }

        /// <summary>Clouds and stars live on the window sky (in front of the sky quad, behind the bars).</summary>
        private void BuildWindowDressing()
        {
            for (int i = 0; i < 2; i++)
            {
                var c = new CloudData();
                c.y = i == 0 ? 2.14f : 1.86f;
                c.scale = i == 0 ? 0.34f : 0.26f;
                c.speed = i == 0 ? 0.021f : 0.03f;
                c.phase = i * 0.55f;
                c.r = Quad("Cloud" + i, room.transform, new Vector3(0.1f, c.y, 2.652f), c.scale, "cloud", Color.white);
                c.t = c.r.transform;
                clouds.Add(c);
            }
            // A handful of stars scattered over the window.
            Vector3[] spots =
            {
                new Vector3(-0.38f, 2.34f, 2.652f), new Vector3(0.52f, 2.42f, 2.652f),
                new Vector3(-0.14f, 1.78f, 2.652f), new Vector3(0.68f, 1.92f, 2.652f),
                new Vector3(0.24f, 2.18f, 2.652f), new Vector3(-0.52f, 1.98f, 2.652f),
            };
            for (int i = 0; i < spots.Length; i++)
            {
                var s = new StarData();
                s.pos = spots[i];
                s.phase = i * 1.1f;
                s.speed = 2.2f + 0.5f * (i % 3);
                s.r = Quad("Star" + i, room.transform, spots[i], 0.075f, "sparkle", Color.white);
                s.t = s.r.transform;
                stars.Add(s);
            }
        }

        /// <summary>Two butterflies with heart-shaped wings — one pink, one sky blue.</summary>
        private void BuildButterflies()
        {
            Color[] wingColors = { new Color(1f, 0.72f, 0.86f), new Color(0.62f, 0.84f, 1f) };
            for (int i = 0; i < 2; i++)
            {
                var b = new Butterfly();
                var root = new GameObject("Butterfly" + i);
                root.transform.SetParent(transform, false);
                b.root = root.transform;

                GameObject body = Art.Prim(PrimitiveType.Capsule, "Body", root.transform,
                    Vector3.zero, new Vector3(0.022f, 0.038f, 0.022f), new Color(0.45f, 0.32f, 0.25f));
                body.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

                b.wingL = new GameObject("WingL").transform;
                b.wingL.SetParent(root.transform, false);
                b.wingR = new GameObject("WingR").transform;
                b.wingR.SetParent(root.transform, false);
                Renderer wl = Quad("L", b.wingL, new Vector3(-0.055f, 0f, 0f), 0.11f, "heart", wingColors[i]);
                Renderer wr = Quad("R", b.wingR, new Vector3(0.055f, 0f, 0f), 0.11f, "heart", wingColors[i]);
                wl.transform.localRotation = Quaternion.Euler(90f, 0f, -35f);
                wr.transform.localRotation = Quaternion.Euler(90f, 0f, 35f);
                b.renderers = new[] { body.GetComponent<Renderer>(), wl, wr };

                // Generous tap target for little fingers.
                SphereCollider tap = root.AddComponent<SphereCollider>();
                tap.radius = 0.16f;

                b.root.position = RandomFlyPoint();
                b.target = RandomFlyPoint();
                b.speed = 0.32f + 0.08f * i;
                b.flapPhase = i * 2f;
                b.bobPhase = i * 3f;
                butterflies.Add(b);
            }
        }

        private void BuildFireflies()
        {
            for (int i = 0; i < 4; i++)
            {
                var f = new Firefly();
                f.home = new Vector3(Random.Range(0.4f, 2.1f), Random.Range(0.6f, 1.9f), Random.Range(1.5f, 2.4f));
                if (i % 2 == 0) f.home.x = Random.Range(-2.2f, -0.6f);
                f.phase = i * 1.7f;
                f.drift = 0.55f + 0.2f * (i % 2);
                f.r = Quad("Firefly" + i, transform, f.home, 0.045f, "circle", new Color(1f, 0.95f, 0.55f));
                f.t = f.r.transform;
                fireflies.Add(f);
            }
        }

        private void BuildNightlight()
        {
            nightlight = Quad("Nightlight", room.transform, new Vector3(-2.3f, 1.5f, 2.68f), 0.3f, "star", new Color(1f, 0.9f, 0.55f));
        }

        private Vector3 RandomFlyPoint()
        {
            return new Vector3(Random.Range(FlyMin.x, FlyMax.x), Random.Range(FlyMin.y, FlyMax.y), Random.Range(FlyMin.z, FlyMax.z));
        }

        // ---------------------------------------------------------------- live

        private bool DayCrittersOut => !dayNight.IsNight && !dayNight.IsSleepDim;
        private bool NightGlow => dayNight.IsNight || dayNight.IsSleepDim;

        private void ApplyPeriod(bool force)
        {
            bool day = DayCrittersOut;
            for (int i = 0; i < clouds.Count; i++) clouds[i].t.gameObject.SetActive(!dayNight.IsNight);
            for (int i = 0; i < stars.Count; i++) stars[i].t.gameObject.SetActive(dayNight.IsNight);
            for (int i = 0; i < butterflies.Count; i++) butterflies[i].root.gameObject.SetActive(day);
            for (int i = 0; i < fireflies.Count; i++) fireflies[i].t.gameObject.SetActive(dayNight.IsNight);
            if (nightlight != null) nightlight.gameObject.SetActive(NightGlow);
        }

        private void Update()
        {
            if (dayNight == null) return;
            time += Time.deltaTime;
            ApplyPeriod(false);

            UpdateWindow();
            if (DayCrittersOut) UpdateButterflies();
            if (dayNight.IsNight) UpdateFireflies();
            if (nightlight != null && nightlight.gameObject.activeSelf)
            {
                Color c = nightlight.material.color;
                c.a = 0.55f + 0.2f * Mathf.Sin(time * 1.3f);
                nightlight.material.color = c;
            }
        }

        private void UpdateWindow()
        {
            // Clouds slide gently across the window and fade near the frame.
            const float left = -0.52f, right = 0.72f;   // sky-local x range (sky centre 0.1, width 1.34)
            for (int i = 0; i < clouds.Count; i++)
            {
                CloudData c = clouds[i];
                if (!c.t.gameObject.activeSelf) continue;
                c.phase += c.speed * Time.deltaTime / (right - left);
                if (c.phase > 1f) c.phase -= 1f;
                float x = Mathf.Lerp(left, right, c.phase);
                c.t.localPosition = new Vector3(x, c.y + 0.02f * Mathf.Sin(time * 0.5f + i), 2.652f);
                Color col = c.r.material.color;
                col.a = 0.9f * Mathf.Sin(c.phase * Mathf.PI);
                c.r.material.color = col;
            }
            for (int i = 0; i < stars.Count; i++)
            {
                StarData s = stars[i];
                if (!s.t.gameObject.activeSelf) continue;
                float tw = 0.55f + 0.45f * Mathf.Sin(time * s.speed + s.phase);
                Color col = s.r.material.color;
                col.a = tw;
                s.r.material.color = col;
                s.t.localScale = Vector3.one * (0.06f + 0.025f * tw);
            }
        }

        private void UpdateButterflies()
        {
            float dt = Time.deltaTime;
            bool canTap = Pointer.Down && !Pointer.OverUI && bella != null && !bella.InActivity;
            RaycastHit hit = new RaycastHit();
            bool hasHit = canTap && Pointer.Raycast(cam, out hit);

            for (int i = 0; i < butterflies.Count; i++)
            {
                Butterfly b = butterflies[i];
                b.cooldown -= dt;
                b.retarget -= dt;

                // flap + bob
                b.flapPhase += dt * (b.fleeing ? 26f : 14f);
                float flap = 28f + 38f * Mathf.Sin(b.flapPhase);
                b.wingL.localRotation = Quaternion.Euler(0f, -flap, 0f);
                b.wingR.localRotation = Quaternion.Euler(0f, flap, 0f);
                b.bobPhase += dt * 3f;

                // wander
                Vector3 to = b.target - b.root.position;
                if (to.magnitude < 0.15f || b.retarget <= 0f)
                {
                    b.target = RandomFlyPoint();
                    b.retarget = Random.Range(4f, 8f);
                    b.fleeing = false;
                    to = b.target - b.root.position;
                }
                float speed = b.speed * (b.fleeing ? 3.2f : 1f);
                Vector3 step = to.normalized * speed * dt;
                step.y += 0.0016f * Mathf.Sin(b.bobPhase * 2.6f);
                b.root.position += step;
                if (to.sqrMagnitude > 0.0001f)
                {
                    Quaternion face = Quaternion.LookRotation(new Vector3(to.x, 0f, to.z).normalized, Vector3.up);
                    b.root.rotation = Quaternion.Slerp(b.root.rotation, face, 1f - Mathf.Exp(-dt * 4f));
                }

                // a happy tap
                if (hasHit && b.cooldown <= 0f && hit.collider != null && hit.collider.transform == b.root)
                {
                    b.cooldown = 2f;
                    b.fleeing = true;
                    b.target = RandomFlyPoint();
                    b.target.y = FlyMax.y;
                    b.retarget = 3f;
                    Fx.Sparkles(b.root.position, 10);
                    Fx.Hearts(b.root.position, 3);
                    AudioManager.Play("sparkle");
                    AudioManager.Voice("yip");
                    if (needs != null) needs.AddHappiness(4f);
                    SaveData d = SaveSystem.Data;
                    d.countButterflies++;
                    SaveSystem.Flush();
                }
            }
        }

        private void UpdateFireflies()
        {
            for (int i = 0; i < fireflies.Count; i++)
            {
                Firefly f = fireflies[i];
                float p = time * 0.35f + f.phase;
                Vector3 pos = f.home + new Vector3(
                    Mathf.Sin(p) * f.drift,
                    Mathf.Sin(p * 1.6f + 1f) * 0.25f,
                    Mathf.Cos(p * 0.8f) * 0.3f);
                f.t.position = pos;
                float glow = 0.35f + 0.65f * Mathf.Pow(0.5f + 0.5f * Mathf.Sin(time * 2.1f + f.phase * 2f), 2f);
                Color c = f.r.material.color;
                c.a = glow;
                f.r.material.color = c;
                f.t.localScale = Vector3.one * (0.035f + 0.02f * glow);
            }
        }
    }
}
