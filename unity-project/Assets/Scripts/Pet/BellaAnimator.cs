using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Bella.Core;

namespace Bella.Pet
{
    /// <summary>
    /// Plays named ranges (BellaClips) of the single baked animation take through
    /// the Playables API — no AnimatorController asset and no import settings are
    /// needed. Two clip playables are cross-faded; time is driven manually so
    /// every range loops or stops exactly on its own frames.
    /// </summary>
    public class BellaAnimator : MonoBehaviour
    {
        private PlayableGraph graph;
        private AnimationMixerPlayable mixer;
        private readonly AnimationClipPlayable[] slots = new AnimationClipPlayable[2];
        private readonly ClipDef[] defs = new ClipDef[2];
        private readonly float[] times = new float[2];
        private int active;
        private float fadeTime;
        private float fadeDuration;
        private bool ready;
        private Action onDone;
        private bool doneFired;

        public string Current { get; private set; } = "";
        public float Speed { get; set; } = 1f;
        public bool Ready => ready;

        public void Init(Animator animator, AnimationClip clip)
        {
            if (animator == null || clip == null) return;
            try
            {
                graph = PlayableGraph.Create("BellaAnimation");
                graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
                mixer = AnimationMixerPlayable.Create(graph, 2);
                for (int i = 0; i < 2; i++)
                {
                    slots[i] = AnimationClipPlayable.Create(graph, clip);
                    slots[i].SetApplyFootIK(false);
                    slots[i].SetSpeed(0.0);           // we set the time ourselves
                    graph.Connect(slots[i], 0, mixer, i);
                    mixer.SetInputWeight(i, i == 0 ? 1f : 0f);
                    defs[i] = BellaClips.All[0];
                }
                AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "BellaOut", animator);
                output.SetSourcePlayable(mixer);
                graph.Play();
                ready = true;
                Current = defs[0].name;
            }
            catch (Exception e)
            {
                Diagnostics.Note("Animation graph failed: " + e.Message);
                ready = false;
            }
        }

        /// <summary>Cross-fade to a looping (or held) clip.</summary>
        public void Play(string name, float fade = 0.2f)
        {
            StartClip(name, fade, null, false);
        }

        /// <summary>Play a clip from its start and call back when it finishes (loops: after one cycle).</summary>
        public void PlayOnce(string name, Action done, float fade = 0.15f)
        {
            StartClip(name, fade, done, true);
        }

        public float LengthOf(string name)
        {
            ClipDef d;
            return BellaClips.TryGet(name, out d) ? d.Length : 1f;
        }

        private void StartClip(string name, float fade, Action done, bool restart)
        {
            ClipDef def;
            if (!BellaClips.TryGet(name, out def))
            {
                if (done != null) done();
                return;
            }
            if (!restart && name == Current && done == null)
            {
                onDone = null;
                return;
            }
            Current = name;
            onDone = done;
            doneFired = false;
            if (!ready) return;   // placeholder puppy: callbacks are timed in Update below

            int next = 1 - active;
            defs[next] = def;
            times[next] = 0f;
            active = next;
            fadeDuration = Mathf.Max(0.0001f, fade);
            fadeTime = 0f;
        }

        private float placeholderTimer;

        private void Update()
        {
            float dt = Time.deltaTime * Speed;

            if (!ready)
            {
                // No skeleton: still honour one-shot callbacks after the clip's duration.
                if (onDone != null && !doneFired)
                {
                    placeholderTimer += dt;
                    ClipDef d;
                    BellaClips.TryGet(Current, out d);
                    if (placeholderTimer >= d.Length)
                    {
                        placeholderTimer = 0f;
                        FireDone();
                    }
                }
                else placeholderTimer = 0f;
                return;
            }

            for (int i = 0; i < 2; i++)
            {
                float len = Mathf.Max(0.001f, defs[i].Length);
                times[i] += dt;
                float t = times[i];
                if (defs[i].loop)
                {
                    if (i == active && onDone != null && !doneFired && t >= len) FireDone();
                    t = Mathf.Repeat(times[i], len);
                }
                else
                {
                    if (t >= len)
                    {
                        t = len;
                        if (i == active && !doneFired) FireDone();
                    }
                }
                slots[i].SetTime(defs[i].StartTime + t);
            }

            fadeTime += Time.deltaTime;
            float w = Mathf.Clamp01(fadeTime / fadeDuration);
            w = w * w * (3f - 2f * w);
            mixer.SetInputWeight(active, w);
            mixer.SetInputWeight(1 - active, 1f - w);
        }

        private void FireDone()
        {
            doneFired = true;
            Action cb = onDone;
            onDone = null;
            if (cb != null) cb();
        }

        private void OnDestroy()
        {
            if (graph.IsValid()) graph.Destroy();
        }
    }
}
