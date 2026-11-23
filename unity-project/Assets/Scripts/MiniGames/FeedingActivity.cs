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
    /// The Feeding shop: same shape as the Wardrobe (buy with Treats), but food
    /// is consumed on the spot rather than kept — each meal has its own hunger
    /// and happiness effect, so the choice matters every time.
    /// </summary>
    public class FeedingActivity : Activity
    {
        private readonly List<Image> cells = new List<Image>();
        private readonly List<GameObject> priceTags = new List<GameObject>();
        private Text treatsLabel;
        private GameObject treatsPill;
        private bool feeding;

        protected override string Tune { get { return "music_shop"; } }

        protected override void OnBegin()
        {
            Image pill = UIKit.Panel("Treats", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Coin", pill.transform, "treat", 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            treatsLabel = UIKit.Label("Count", pill.transform, "0", 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(treatsLabel.rectTransform, 112f, 0f, 14f, 0f);
            treatsPill = pill.gameObject;

            Image sheet = UIKit.Panel("Sheet", UI, new Color(1f, 0.97f, 0.9f, 0.96f));
            sheet.rectTransform.anchorMin = new Vector2(0f, 0f);
            sheet.rectTransform.anchorMax = new Vector2(1f, 0f);
            sheet.rectTransform.pivot = new Vector2(0.5f, 0f);
            sheet.rectTransform.anchoredPosition = new Vector2(0f, 20f);
            sheet.rectTransform.sizeDelta = new Vector2(-40f, 620f);

            const int cols = 3;
            for (int i = 0; i < Catalog.Foods.Length; i++)
            {
                FoodDef def = Catalog.Foods[i];
                int col = i % cols;
                Image cell = UIKit.Panel("Food_" + def.id, sheet.transform, Color.white);
                UIKit.Place(cell.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2((col - 1f) * 256f, -160f), new Vector2(236f, 300f));
                Image icon = UIKit.Icon("Icon", cell.transform, def.icon, 150f);
                icon.rectTransform.anchoredPosition = new Vector2(0f, 40f);

                RectTransform tag = UIKit.Rect("Price", cell.transform);
                UIKit.Place(tag, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 76f), new Vector2(190f, 56f));
                if (def.price > 0)
                {
                    Image tc = UIKit.Icon("Coin", tag, "treat", 50f);
                    tc.rectTransform.anchoredPosition = new Vector2(-50f, 0f);
                    Text price = UIKit.Label("Text", tag, def.price.ToString(), 40, Art.Ink, TextAnchor.MiddleLeft);
                    UIKit.Place(price.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(-18f, 0f), new Vector2(110f, 56f));
                }
                else
                {
                    UIKit.Label("Text", tag, "Free", 40, Art.Leaf, TextAnchor.MiddleCenter);
                }

                Text stats = UIKit.Label("Stats", cell.transform,
                    "+" + Mathf.RoundToInt(def.hunger) + " hunger\n+" + Mathf.RoundToInt(def.happiness) + " happy",
                    30, new Color(0.45f, 0.4f, 0.35f), TextAnchor.MiddleCenter);
                UIKit.Place(stats.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 24f), new Vector2(220f, 90f));

                Button b = cell.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = cell;
                FoodDef captured = def;
                b.onClick.AddListener(() => Tap(captured));
                cell.gameObject.AddComponent<PressFx>();

                cells.Add(cell);
                priceTags.Add(tag.gameObject);
            }

            Refresh();
            G.CameraRig.Focus(G.Room.HomePoint, 1.9f, 0.6f);
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Sit", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Happy);
            });
        }

        private void Refresh()
        {
            treatsLabel.text = Bella.Needs.Treats.ToString();
            for (int i = 0; i < Catalog.Foods.Length; i++)
            {
                FoodDef def = Catalog.Foods[i];
                bool affordable = def.price == 0 || Bella.Needs.Treats >= def.price;
                cells[i].color = affordable ? Color.white : new Color(0.9f, 0.88f, 0.86f);
            }
        }

        private void Tap(FoodDef def)
        {
            if (!Ready || Ending || feeding) return;
            if (def.price > 0 && !Bella.Needs.SpendTreats(def.price))
            {
                AudioManager.Play("oops", 0.5f);
                UIKit.Pop(treatsPill);
                return;
            }
            feeding = true;
            bool wasHungry = Bella.Needs.Hunger < 30f;
            Bella.Needs.Feed(def.hunger);
            Bella.Needs.AddHappiness(def.happiness);
            SaveSystem.Data.countMeals++;
            SaveSystem.Flush();

            Bella.Puppy.PulseJoy();
            Bella.Face.Flash(Expression.Excited, 1.2f);
            Fx.Sparkles(Bella.HeadPoint, 8);
            AudioManager.Play("munch");
            if (def.happiness >= 20f) AudioManager.Voice("bark_happy");
            else if (wasHungry) AudioManager.Voice("whimper_hungry");

            // the meal itself, when its model is in Resources/Props (the food's icon name is also its model name)
            Vector3 mealAt = G.Room.transform.InverseTransformPoint(Bella.transform.position + Bella.transform.forward * 0.55f);
            GameObject meal = Art.Prop(def.icon, G.Room.transform, mealAt, def.price >= 20 ? 0.42f : 0.3f);

            After(1.2f, () => { if (meal != null) Destroy(meal); feeding = false; Refresh(); });
        }
    }
}
