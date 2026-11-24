using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.States;
using Bella.Systems;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Dig for Treasure (garden): dirt mounds pop up on the lawn. Tap one and Bella runs over and digs.
    /// Every mound hides something nice — a treat, a bone, a star, now and then a gift box — so a dig
    /// is never wasted. Eight digs per visit, then Bella lies down for a rest.
    /// </summary>
    public class DigActivity : Activity
    {
        private class Mound { public Transform t; public bool busy; }

        private const int DigsPerVisit = 8;
        private const int MoundCount = 4;

        private readonly List<Mound> mounds = new List<Mound>();
        private Text hint, digsLabel, treatsLabel;
        private int digs, found;
        private bool digging, over;

        protected override string Tune { get { return "music_garden"; } }
        protected override string Place { get { return "garden"; } }
        protected override string PlaceFloor { get { return "floor_grass"; } }

        protected override void OnBegin()
        {
            Image pill = UIKit.Panel("Treats", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Coin", pill.transform, "treat", 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            treatsLabel = UIKit.Label("Count", pill.transform, Bella.Needs.Treats.ToString(), 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(treatsLabel.rectTransform, 112f, 0f, 14f, 0f);

            Image dpill = UIKit.Panel("Digs", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(dpill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -240f), new Vector2(270f, 84f));
            digsLabel = UIKit.Label("Digs", dpill.transform, "0 / " + DigsPerVisit, 46, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Stretch(digsLabel.rectTransform, 0f, 0f, 0f, 0f);

            hint = UIKit.Label("Hint", UI, "Tap a dirt mound — Bella will dig!", 50, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(hint.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(0f, -290f), new Vector2(960f, 100f));
            hint.resizeTextForBestFit = true; hint.resizeTextMinSize = 28; hint.resizeTextMaxSize = 50;

            for (int i = 0; i < MoundCount; i++) AddMound();
            G.CameraRig.Focus(new Vector3(0f, 0f, 0.25f), 3.0f, 0.45f);
            Bella.BeginActivity(new Vector3(0f, 0f, -0.35f), 180f, "Happy", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Excited);
            });
        }

        protected override void OnEnd()
        {
            for (int i = 0; i < mounds.Count; i++) if (mounds[i].t != null) Destroy(mounds[i].t.gameObject);
            mounds.Clear();
            Bella.Puppy.ClearLook();
        }

        // ---------------------------------------------------------------- mounds

        private void AddMound()
        {
            Vector3 p = Vector3.zero;
            for (int tries = 0; tries < 30; tries++)
            {
                p = new Vector3(Random.Range(-1.1f, 1.1f), 0f, Random.Range(-0.15f, 0.8f));
                bool ok = true;
                for (int i = 0; i < mounds.Count; i++)
                    if (mounds[i].t != null && (mounds[i].t.localPosition - p).magnitude < 0.55f) { ok = false; break; }
                if (ok && (p - Bella.transform.position).magnitude > 0.45f) break;
            }
            var root = new GameObject("Mound");
            root.transform.SetParent(G.Room.transform, false);
            root.transform.localPosition = p;
            Art.Prim(PrimitiveType.Sphere, "Dirt", root.transform, new Vector3(0f, 0.02f, 0f), new Vector3(0.5f, 0.22f, 0.42f), new Color(0.50f, 0.34f, 0.22f));
            Art.Prim(PrimitiveType.Sphere, "DirtTop", root.transform, new Vector3(0f, 0.06f, 0f), new Vector3(0.3f, 0.16f, 0.26f), new Color(0.58f, 0.40f, 0.26f));
            mounds.Add(new Mound { t = root.transform });
            root.transform.localScale = Vector3.zero;
            StartCoroutine(PopIn(root.transform));
        }

        private System.Collections.IEnumerator PopIn(Transform t)
        {
            for (float e = 0f; e < 0.3f && t != null; e += Time.deltaTime)
            {
                float k = e / 0.3f;
                t.localScale = Vector3.one * (1.15f * Mathf.Sin(k * Mathf.PI * 0.5f) + 0.0f);
                yield return null;
            }
            if (t != null) t.localScale = Vector3.one;
        }

        private void Update()
        {
            if (!Running || !Ready || digging || over) return;
            if (!Pointer.Down || Pointer.OverUI) return;
            Ray ray = G.Cam.ScreenPointToRay(Pointer.Position);
            if (Mathf.Abs(ray.direction.y) < 0.0001f) return;
            float t = -ray.origin.y / ray.direction.y;
            if (t < 0f) return;
            Vector3 hit = ray.origin + ray.direction * t;
            Mound best = null; float bestD = 0.4f;
            for (int i = 0; i < mounds.Count; i++)
            {
                if (mounds[i].t == null || mounds[i].busy) continue;
                Vector3 m = mounds[i].t.position;
                float d = new Vector2(m.x - hit.x, m.z - hit.z).magnitude;
                if (d < bestD) { bestD = d; best = mounds[i]; }
            }
            if (best != null) Dig(best);
        }

        private void Dig(Mound m)
        {
            digging = true; m.busy = true;
            hint.text = "";
            AudioManager.Play("tap", 0.6f);
            Vector3 spot = m.t.position;
            Vector3 fromBella = (Bella.transform.position - spot); fromBella.y = 0f;
            if (fromBella.magnitude < 0.01f) fromBella = Vector3.back;
            Vector3 stand = spot + fromBella.normalized * 0.45f;
            stand.x = Mathf.Clamp(stand.x, -1.3f, 1.3f);
            float yaw = Quaternion.LookRotation(new Vector3(spot.x - stand.x, 0f, spot.z - stand.z), Vector3.up).eulerAngles.y;
            Bella.Puppy.LookAt(spot + Vector3.up * 0.1f);
            Bella.RunState(new WalkToState(Bella.transform, Bella.Anim, stand, Bella.Run * 0.9f, true, () =>
            {
                Bella.Puppy.ClearLook();
                AudioManager.Play("sniff", 0.6f);
                Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "Paw", 1.6f, () => Reveal(m)));
                StartCoroutine(DirtSpray(spot, 1.6f));
            }, yaw));
        }

        private System.Collections.IEnumerator DirtSpray(Vector3 at, float seconds)
        {
            for (float t = 0f; t < seconds && Running; t += 0.27f)
            {
                if (Fx.Has("dust")) Fx.Burst(at + Vector3.up * 0.12f, "dust", 2, 0.3f, 0.4f, 1.1f, 0.8f);
                else Fx.Burst(at + Vector3.up * 0.12f, "paw", 2, 0.14f, 1.0f, 1.3f, 0.7f);
                AudioManager.Play("pop", 0.25f, Random.Range(0.55f, 0.8f));
                yield return new WaitForSeconds(0.27f);
            }
        }

        private void Reveal(Mound m)
        {
            if (!Running) return;
            digs++;
            SaveSystem.Data.countDigs++;
            Vector3 at = m.t.position;
            float roll = Random.value;
            string icon; int treats = 0; string msg; bool gift = false;
            if (roll < 0.10f) { icon = "gift"; treats = 5; msg = "A gift box!  +5 treats"; gift = true; }
            else if (roll < 0.30f) { icon = "star"; treats = 3; msg = "A golden star!  +3 treats"; }
            else if (roll < 0.60f) { icon = "bone"; treats = 2; msg = "A bone!  +2 treats"; }
            else { icon = "treat"; treats = 1; msg = "A yummy treat!  +1 treat"; }

            int paid = Progress.PlayTreats(treats);
            if (paid > 0) { Bella.Needs.AddTreats(paid); found += paid; }
            else msg = msg.Substring(0, msg.IndexOf('!') + 1) + "  (Enough treats for today!)";
            if (treatsLabel != null) treatsLabel.text = Bella.Needs.Treats.ToString();
            digsLabel.text = digs + " / " + DigsPerVisit;
            hint.text = msg;

            StartCoroutine(Rise(at, icon));
            AudioManager.Play(gift ? "gift" : "coin", 0.85f);
            AudioManager.Voice("bark_happy");
            Fx.Sparkles(at + Vector3.up * 0.2f, 10);
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, 6);
            Bella.Puppy.PulseJoy();
            Bella.Needs.AddHappiness(6f);
            Bella.Needs.Tire(2.5f);
            SaveSystem.Flush();

            // the mound is used up; a fresh one pops up elsewhere
            if (m.t != null) Destroy(m.t.gameObject);
            mounds.Remove(m);
            if (digs < DigsPerVisit) AddMound();

            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "Happy", 1.3f, () =>
            {
                if (!Running) return;
                if (digs >= DigsPerVisit || Bella.Needs.Energy < 8f) { EndVisit(); return; }
                digging = false;
                Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Excited, "Happy", 9999f, null));
                hint.text = "Tap another mound!";
            }));
        }

        private System.Collections.IEnumerator Rise(Vector3 at, string icon)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(go.GetComponent<Collider>());
            go.transform.SetParent(G.Room.transform, false);
            Renderer r = go.GetComponent<Renderer>();
            r.sharedMaterial = Art.SpriteMat(Color.white, Art.Tex("Icons/" + icon));
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            for (float e = 0f; e < 1.4f && go != null; e += Time.deltaTime)
            {
                float k = e / 1.4f;
                go.transform.position = at + new Vector3(0f, 0.25f + k * 0.7f, -0.1f);
                float s = 0.4f + 0.12f * Mathf.Sin(k * Mathf.PI);
                go.transform.localScale = new Vector3(s, s, 1f);
                yield return null;
            }
            if (go != null) Destroy(go);
        }

        private void EndVisit()
        {
            over = true;
            Bella.Face.Flash(Expression.Sleepy, 2f);
            AudioManager.Voice("yawn_sleepy");
            Bella.RunState(new InteractionState(Bella.Face, Bella.Anim, Expression.Sleepy, "Sit", 2.4f, null));
            hint.text = found > 0 ? "Great digging!  " + found + " treats found" : "Great digging! Time to rest.";
            FinishAfter(2.8f);
        }
    }
}
