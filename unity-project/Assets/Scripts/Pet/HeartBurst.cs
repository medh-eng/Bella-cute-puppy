using UnityEngine;

namespace Bella.Pet
{
    /// <summary>
    /// Multisensory positive reinforcement (docs/GDD.md section 2): a burst of soft
    /// pink particles floating upward. Built entirely in code so the grey-box phase
    /// needs no assets; swap in a heart sprite later by assigning a material.
    /// </summary>
    public static class HeartBurst
    {
        private static Material particleMaterial;

        public static void Play(Vector3 worldPos)
        {
            var go = new GameObject("HeartBurst");
            go.transform.position = worldPos;

            var ps = go.AddComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = 1.1f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.8f, 1.6f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.12f, 0.22f);
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(1f, 0.45f, 0.65f), new Color(1f, 0.7f, 0.8f));
            main.gravityModifier = -0.35f; // float up
            main.loop = false;
            main.duration = 0.3f;
            main.stopAction = ParticleSystemStopAction.Destroy;

            var emission = ps.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10) });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.25f;

            var renderer = go.GetComponent<ParticleSystemRenderer>();
            renderer.material = GetMaterial();

            ps.Play();
        }

        private static Material GetMaterial()
        {
            if (particleMaterial != null) return particleMaterial;
            // Pick a particle shader for the ACTIVE pipeline (URP shaders render
            // magenta when URP isn't the assigned pipeline, and vice versa).
            bool urpActive = UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null;
            Shader shader = urpActive
                ? Shader.Find("Universal Render Pipeline/Particles/Unlit")
                : Shader.Find("Particles/Standard Unlit");
            if (shader == null) shader = Shader.Find("Sprites/Default"); // works everywhere
            particleMaterial = new Material(shader);
            return particleMaterial;
        }
    }
}
