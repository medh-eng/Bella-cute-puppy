using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Pet;
using Bella.Save;
using Bella.UI;

namespace Bella.MiniGames
{
    /// <summary>
    /// The Clinic (docs/GDD.md §5): a gentle picture-matching check-up. Bella's
    /// bubble shows which tool she needs; tap the same tool below. A wrong tap
    /// is just "not that one" and the right tool starts to wiggle. Three rounds,
    /// then she is fit and bouncy again (and the sniffles are cured).
    /// </summary>
    public class ClinicActivity : Activity
    {
        private class Ailment
        {
            public string tool, pose;
            public Expression face;
            public bool sneezes;
        }

        private readonly Ailment[] ailments =
        {
            new Ailment { tool = "thermometer", pose = "Sit", face = Expression.Sleepy },
            new Ailment { tool = "medicine", pose = "Sit", face = Expression.Sad, sneezes = true },
            new Ailment { tool = "bandage", pose = "Paw", face = Expression.Begging },
            new Ailment { tool = "brush", pose = "Sit", face = Expression.Sad },
        };

        private readonly Dictionary<string, Button> toolButtons = new Dictionary<string, Button>();
        private readonly List<int> order = new List<int>();
        private readonly Image[] roundPips = new Image[3];
        private Image bubble, bubbleIcon;
        private int round;
        private int wrong;
        private float idle;
        private bool waiting;
        private bool wasUnwell;
        private float sneezeTimer;
        private Ailment current;

        protected override void OnBegin()
        {
            wasUnwell = Bella.Needs.IsUnwell;

            // what Bella needs
            bubble = UIKit.Img("Bubble", UI, Art.Icon("bubble"), Color.white);
            UIKit.Place(bubble.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(190f, -420f), new Vector2(330f, 330f));
            bubbleIcon = UIKit.Icon("Need", bubble.transform, "thermometer", 170f);
            bubbleIcon.rectTransform.anchoredPosition = new Vector2(0f, 36f);
            bubble.gameObject.SetActive(false);

            // round counter
            for (int i = 0; i < 3; i++)
            {
                roundPips[i] = UIKit.Icon("Round" + i, UI, "heart", 84f);
                UIKit.Place(roundPips[i].rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2(60f + (i - 1) * 100f, -120f), new Vector2(84f, 84f));
                roundPips[i].color = new Color(0.3f, 0.25f, 0.2f, 0.25f);
            }

            // the doctor's tray
            string[] tools = { "thermometer", "medicine", "bandage", "brush" };
            for (int i = 0; i < tools.Length; i++)
            {
                string tool = tools[i];
                Button b = UIKit.IconButton("Tool_" + tool, UI, tool, 210f, Color.white, () => Pick(tool));
                UIKit.Place(b.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f),
                    new Vector2((i - 1.5f) * 250f, 190f), new Vector2(210f, 210f));
                b.transform.Find("Icon").gameObject.AddComponent<UIPulse>().enabled = false;
                toolButtons[tool] = b;
            }

            // three different ailments, random order
            var pool = new List<int> { 0, 1, 2, 3 };
            while (order.Count < 3)
            {
                int k = Random.Range(0, pool.Count);
                order.Add(pool[k]);
                pool.RemoveAt(k);
            }
            if (wasUnwell && !order.Contains(1)) order[0] = 1;   // the sniffles get their medicine

            G.CameraRig.Focus(G.Room.HomePoint, 2.3f, 0.6f);
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Sit", () =>
            {
                Ready = true;
                StartRound();
            });
        }

        private void StartRound()
        {
            current = ailments[order[round]];
            wrong = 0;
            idle = 0f;
            waiting = true;
            sneezeTimer = 0.6f;
            Bella.PlayPose(current.pose, current.face);
            bubbleIcon.sprite = Art.Icon(current.tool);
            bubble.gameObject.SetActive(true);
            UIKit.Pop(bubble.gameObject);
            AudioManager.Voice("whine");
            SetHint(false);
        }

        private void SetHint(bool on)
        {
            foreach (KeyValuePair<string, Button> kv in toolButtons)
            {
                UIPulse pulse = kv.Value.transform.Find("Icon").GetComponent<UIPulse>();
                pulse.amount = 0.16f;
                pulse.enabled = on && current != null && kv.Key == current.tool;
            }
        }

        private void Update()
        {
            if (!Ready || Ending || !waiting) return;
            idle += Time.deltaTime;
            if (idle > 5f) SetHint(true);

            if (current.sneezes)
            {
                sneezeTimer -= Time.deltaTime;
                if (sneezeTimer <= 0f)
                {
                    sneezeTimer = 2.6f;
                    AudioManager.Voice("sneeze");
                    Bella.PlayOnce("Sneeze", () => { if (waiting) Bella.PlayPose(current.pose, current.face); });
                }
            }
        }

        private void Pick(string tool)
        {
            if (!Ready || Ending || !waiting) return;
            idle = 0f;
            if (tool != current.tool)
            {
                wrong++;
                AudioManager.Play("oops", 0.5f);
                UIKit.Pop(bubble.gameObject);
                if (wrong >= 1) SetHint(true);
                return;
            }

            waiting = false;
            SetHint(false);
            bubble.gameObject.SetActive(false);
            roundPips[round].color = Color.white;
            UIKit.Pop(roundPips[round].gameObject);
            AudioManager.Play("chime");
            Fx.Sparkles(Bella.HeadPoint, 12);
            Bella.Puppy.PulseJoy();
            Bella.PlayPose("Happy", Expression.Excited);
            AudioManager.Voice("yip");

            round++;
            if (round >= 3) After(1.3f, Complete);
            else After(1.5f, StartRound);
        }

        private void Complete()
        {
            SaveSystem.Data.countCheckups++;
            G.Progress.CureSniffles();
            Bella.Needs.AddHappiness(10f);
            AudioManager.Play("success");
            AudioManager.Voice("bark");
            Fx.Hearts(Bella.HeadPoint + Vector3.up * 0.3f, 14);
            Bella.Celebrate(wasUnwell ? 10 : 2);
            Bella.PlayOnce("Spin", () => Bella.PlayPose("Happy", Expression.Excited));
            FinishAfter(2.2f);
        }
    }
}
