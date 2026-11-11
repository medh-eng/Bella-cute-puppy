using UnityEngine;
using Bella.Core;

namespace Bella.Pet
{
    /// <summary>
    /// Multisensory positive reinforcement (docs/GDD.md §2): bursts of floating
    /// hearts / stars / sparkles / bubbles built entirely in code.
    /// </summary>
    public static class Fx
    {
        public static void Hearts(Vector3 worldPos, int count = 8) { Burst(worldPos, "heart", count, 0.26f, -0.35f, 1.1f); }
        public static void Stars(Vector3 worldPos, int count = 10) { Burst(worldPos, "star", count, 0.24f, 0.4f, 2.2f); }
        public static void Sparkles(Vector3 worldPos, int count = 12) { Burst(worldPos, "sparkle", count, 0.22f, -0.1f, 1.2f); }
        public static void Bubbles(Vector3 worldPos, int count = 6) { Burst(worldPos, "bubbles", count, 0.22f, -0.25f, 0.7f); }
        public static void Zzz(Vector3 worldPos) { Burst(worldPos, "zzz", 1, 0.3f, -0.12f, 0.35f, 2.4f); }
        public static void Drops(Vector3 worldPos, int count = 10) { Burst(worldPos, "drop", count, 0.16f, 1.2f, 2.4f); }

        public static void Burst(Vector3 worldPos, string icon, int count, float size, float gravity, float speed, float life = 1.2f)
        {
            var go = new GameObject("Fx_" + icon);
            go.transform.position = worldPos;

            ParticleSystem ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystem.MainModule main = ps.main;
            main.playOnAwake = false;
            main.loop = false;
            main.duration = 0.3f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(life * 0.8f, life * 1.15f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.5f, speed);
            main.startSize = new ParticleSystem.MinMaxCurve(size * 0.7f, size * 1.2f);
            main.startRotation = new ParticleSystem.MinMaxCurve(-0.4f, 0.4f);
            main.startColor = Color.white;
            main.gravityModifier = gravity;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.stopAction = ParticleSystemStopAction.Destroy;

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)count) });

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.22f;

            ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
            sol.enabled = true;
            sol.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(
                new Keyframe(0f, 0.2f), new Keyframe(0.2f, 1f), new Keyframe(0.75f, 1f), new Keyframe(1f, 0f)));

            ParticleSystemRenderer r = go.GetComponent<ParticleSystemRenderer>();
            r.material = Art.SpriteMat(Color.white, Art.Tex("Icons/" + icon));
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;

            ps.Play();
            Object.Destroy(go, life * 1.3f + 1f);
        }
    }
}
