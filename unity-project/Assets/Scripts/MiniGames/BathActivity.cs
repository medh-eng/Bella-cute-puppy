using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.Systems;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// Bubble Bath (docs/GDD.md §5): three tactile steps, each done by rubbing a
    /// finger over Bella — sponge her bubbly, shower the bubbles away, towel her
    /// dry — then she shakes off in a spray of sparkles. Restores Hygiene.
    /// </summary>
    public class BathActivity : Activity
    {
        private readonly string[] tools = { "sponge", "shower", "towel" };
        private readonly Image[] toolIcons = new Image[3];
        private readonly List<GameObject> soap = new List<GameObject>();
        private Image fill, cursor;
        private int step;
        private float progress;
        private Vector2 lastPointer;
        private float soundTimer;
        private bool wasNeeded;
        private Material soapMaterial;

        protected override void OnBegin()
        {
            wasNeeded = Bella.Needs.Hygiene < 75f;
            soapMaterial = Art.Lit(Color.white);

            // progress bar
            Image bar = UIKit.Panel("Bar", UI, new Color(1f, 1f, 1f, 0.85f), false);
            UIKit.Place(bar.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(60f, -120f), new Vector2(640f, 70f));
            fill = UIKit.Panel("Fill", bar.transform, Art.Sky, false);
            fill.rectTransform.anchorMin = new Vector2(0f, 0f);
            fill.rectTransform.anchorMax = new Vector2(0f, 1f);
            fill.rectTransform.pivot = new Vector2(0f, 0.5f);
            fill.rectTransform.anchoredPosition = new Vector2(8f, 0f);
            fill.rectTransform.sizeDelta = new Vector2(0f, -16f);

            // the three steps, bottom
            for (int i = 0; i < 3; i++)
            {
                Image back = UIKit.Img("Step" + i, UI, Art.Icon("circle"), Color.white);
                UIKit.Place(back.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2((i - 1) * 250f, 170f), new Vector2(200f, 200f));
                toolIcons[i] = UIKit.Icon("Icon", back.transform, tools[i], 130f);
                toolIcons[i].gameObject.AddComponent<UIPulse>().enabled = false;
            }

            cursor = UIKit.Icon("Cursor", UI, tools[0], 150f);
            cursor.gameObject.SetActive(false);

            G.Room.ShowTub(true);
            G.CameraRig.Focus(G.Room.HomePoint, 2.3f, 0.55f);
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Sit", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Happy);
                AudioManager.Play("splash");
                Fx.Drops(Bella.transform.position + Vector3.up * 0.4f, 8);
                SetStep(0);
            });
        }

        private void SetStep(int s)
        {
            step = s;
            progress = 0f;
            for (int i = 0; i < 3; i++)
            {
                toolIcons[i].color = i < s ? new Color(1f, 1f, 1f, 0.35f) : Color.white;
                UIPulse pulse = toolIcons[i].GetComponent<UIPulse>();
                pulse.enabled = i == s;
                toolIcons[i].transform.parent.GetComponent<Image>().color = i == s ? Art.Gold : Color.white;
            }
            if (s < 3) cursor.sprite = Art.Icon(tools[s]);
            fill.color = s == 0 ? Art.Sky : s == 1 ? Art.Teal : Art.Pink;
            UpdateBar();
        }

        private void UpdateBar()
        {
            fill.rectTransform.sizeDelta = new Vector2(624f * Mathf.Clamp01(progress), -16f);
        }

        private void Update()
        {
            if (!Ready || Ending || step > 2) return;

            bool held = Pointer.Held && !Pointer.OverUI;
            cursor.gameObject.SetActive(held);
            if (!held) { lastPointer = Pointer.Position; return; }

            cursor.rectTransform.anchoredPosition = PointerLocal() + new Vector2(0f, 60f);
            if (Pointer.Down) lastPointer = Pointer.Position;

            RaycastHit hit;
            if (!PointerOnBella(out hit)) { lastPointer = Pointer.Position; return; }

            float moved = (Pointer.Position - lastPointer).magnitude / Mathf.Max(1f, Screen.height);
            lastPointer = Pointer.Position;
            if (moved <= 0f) return;

            progress += moved * 0.42f;
            UpdateBar();
            Bella.Puppy.LookAt(hit.point + Vector3.up * 0.2f);

            soundTimer -= Time.deltaTime;
            if (step == 0)
            {
                Bella.Face.Set(Expression.Soapy);
                if (soap.Count < 46 && Random.value < 0.5f) AddSoap(hit.point);
                if (soundTimer <= 0f) { soundTimer = 0.45f; AudioManager.Play("bubbles", 0.6f); Fx.Bubbles(hit.point, 2); }
            }
            else if (step == 1)
            {
                int keep = Mathf.RoundToInt(soap.Count * 0.0f + (1f - progress) * 46f);
                while (soap.Count > Mathf.Max(0, keep))
                {
                    GameObject g = soap[soap.Count - 1];
                    soap.RemoveAt(soap.Count - 1);
                    if (g != null) Destroy(g);
                }
                if (soundTimer <= 0f) { soundTimer = 0.4f; AudioManager.Play("splash", 0.45f); Fx.Drops(hit.point + Vector3.up * 0.3f, 6); }
            }
            else
            {
                Bella.Face.Set(Expression.Excited);
                if (soundTimer <= 0f) { soundTimer = 0.35f; AudioManager.Play("whoosh", 0.5f); Fx.Sparkles(hit.point, 3); }
            }

            if (progress >= 1f)
            {
                AudioManager.Play("chime");
                Bella.Puppy.PulseJoy();
                if (step < 2) SetStep(step + 1);
                else Complete();
            }
        }

        private void AddSoap(Vector3 worldPoint)
        {
            GameObject b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            b.name = "Soap";
            Destroy(b.GetComponent<Collider>());
            b.transform.SetParent(Bella.Model.Pivot, true);
            b.transform.position = worldPoint + Random.insideUnitSphere * 0.07f;
            b.transform.localScale = Vector3.one * Random.Range(0.07f, 0.15f);
            b.GetComponent<Renderer>().sharedMaterial = soapMaterial;
            soap.Add(b);
        }

        private void ClearSoap()
        {
            for (int i = 0; i < soap.Count; i++) if (soap[i] != null) Destroy(soap[i]);
            soap.Clear();
        }

        private void Complete()
        {
            step = 3;
            cursor.gameObject.SetActive(false);
            ClearSoap();
            Bella.Puppy.ClearLook();
            Bella.Face.Set(Expression.Soapy);
            AudioManager.Play("splash");
            Fx.Drops(Bella.HeadPoint, 16);
            Bella.PlayOnce("Shake", () =>
            {
                Bella.Needs.Bathe(100f);
                Bella.Needs.AddHappiness(10f);
                SaveSystem.Data.countBaths++;
                Bella.PlayPose("Sit", Expression.Excited);
                Fx.Sparkles(Bella.HeadPoint, 16);
                AudioManager.Play("success");
                AudioManager.Voice("bark");
                Bella.Celebrate(wasNeeded ? 5 : Progress.PlayTreats(1));
                FinishAfter(1.6f);
            });
        }

        protected override void OnEnd()
        {
            ClearSoap();
            G.Room.ShowTub(false);
        }
    }
}
