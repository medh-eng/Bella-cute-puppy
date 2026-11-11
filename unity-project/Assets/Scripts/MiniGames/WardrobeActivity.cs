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
    /// The Wardrobe (docs/GDD.md §5): spend treats earned through care on hats,
    /// bows, glasses, bandanas and capes. Tap to buy, tap again to wear or take
    /// off; drag across Bella to turn her around. Nothing costs real money.
    /// </summary>
    public class WardrobeActivity : Activity
    {
        private readonly List<Image> cells = new List<Image>();
        private readonly List<GameObject> priceTags = new List<GameObject>();
        private readonly List<GameObject> checks = new List<GameObject>();
        private Text treatsLabel;
        private GameObject treatsPill;
        private Vector2 lastPointer;
        private bool turning;
        private float yaw = 180f;

        protected override void OnBegin()
        {
            // treats, top right
            Image pill = UIKit.Panel("Treats", UI, new Color(1f, 1f, 1f, 0.92f), false);
            UIKit.Place(pill.rectTransform, new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-40f, -120f), new Vector2(270f, 104f));
            Image coin = UIKit.Icon("Coin", pill.transform, "treat", 88f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 0f), new Vector2(88f, 88f));
            treatsLabel = UIKit.Label("Count", pill.transform, "0", 54, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(treatsLabel.rectTransform, 112f, 0f, 14f, 0f);
            treatsPill = pill.gameObject;

            // item sheet, bottom
            Image sheet = UIKit.Panel("Sheet", UI, new Color(1f, 0.97f, 0.9f, 0.96f));
            sheet.rectTransform.anchorMin = new Vector2(0f, 0f);
            sheet.rectTransform.anchorMax = new Vector2(1f, 0f);
            sheet.rectTransform.pivot = new Vector2(0.5f, 0f);
            sheet.rectTransform.anchoredPosition = new Vector2(0f, 20f);
            sheet.rectTransform.sizeDelta = new Vector2(-40f, 800f);

            const int cols = 4;
            for (int i = 0; i < Catalog.Outfits.Length; i++)
            {
                OutfitDef def = Catalog.Outfits[i];
                int col = i % cols, row = i / cols;
                Image cell = UIKit.Panel("Item_" + def.id, sheet.transform, Color.white);
                UIKit.Place(cell.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2((col - 1.5f) * 248f, -150f - row * 252f), new Vector2(228f, 232f));
                Image icon = UIKit.Icon("Icon", cell.transform, def.icon, 140f);
                icon.rectTransform.anchoredPosition = new Vector2(0f, 24f);
                icon.color = Color.Lerp(def.color, Color.white, 0.15f);

                RectTransform tag = UIKit.Rect("Price", cell.transform);
                UIKit.Place(tag, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 40f), new Vector2(180f, 56f));
                Image tc = UIKit.Icon("Coin", tag, "treat", 50f);
                tc.rectTransform.anchoredPosition = new Vector2(-44f, 0f);
                Text price = UIKit.Label("Text", tag, def.price.ToString(), 40, Art.Ink, TextAnchor.MiddleLeft);
                UIKit.Place(price.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(-12f, 0f), new Vector2(110f, 56f));

                Image check = UIKit.Icon("Worn", cell.transform, "check", 70f);
                UIKit.Place(check.rectTransform, new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-34f, -34f), new Vector2(70f, 70f));

                Button b = cell.gameObject.AddComponent<Button>();
                b.transition = Selectable.Transition.None;
                b.targetGraphic = cell;
                OutfitDef captured = def;
                b.onClick.AddListener(() => Tap(captured));
                cell.gameObject.AddComponent<PressFx>();

                cells.Add(cell);
                priceTags.Add(tag.gameObject);
                checks.Add(check.gameObject);
            }

            Refresh();
            G.CameraRig.Focus(G.Room.HomePoint, 1.9f, 0.02f);
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Sit", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Happy);
            });
        }

        private void Refresh()
        {
            SaveData d = SaveSystem.Data;
            treatsLabel.text = Bella.Needs.Treats.ToString();
            for (int i = 0; i < Catalog.Outfits.Length; i++)
            {
                OutfitDef def = Catalog.Outfits[i];
                bool owned = d.ownedOutfits.Contains(def.id);
                bool worn = Bella.Outfits.IsWearing(def.id);
                priceTags[i].SetActive(!owned);
                checks[i].SetActive(worn);
                cells[i].color = worn ? new Color(1f, 0.93f, 0.6f) : owned ? Color.white : new Color(0.96f, 0.93f, 0.9f);
            }
        }

        private void Tap(OutfitDef def)
        {
            if (!Ready || Ending) return;
            SaveData d = SaveSystem.Data;
            bool owned = d.ownedOutfits.Contains(def.id);

            if (!owned)
            {
                if (!Bella.Needs.SpendTreats(def.price))
                {
                    // Not enough treats yet: a soft nudge towards the counter, never a scolding.
                    AudioManager.Play("oops", 0.5f);
                    UIKit.Pop(treatsPill);
                    return;
                }
                d.ownedOutfits.Add(def.id);
                AudioManager.Play("coin");
                Wear(def);
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.2f, 12);
                AudioManager.Voice("bark");
                Bella.Needs.AddHappiness(8f);
            }
            else if (Bella.Outfits.IsWearing(def.id))
            {
                Bella.Outfits.Unequip(def.slot);
                AudioManager.Play("tap");
            }
            else
            {
                Wear(def);
            }

            d.equippedOutfits = Bella.Outfits.WornIds();
            SaveSystem.Flush();
            Refresh();
        }

        private void Wear(OutfitDef def)
        {
            Bella.Outfits.Equip(def);
            Bella.Puppy.PulseJoy();
            Bella.Face.Flash(Expression.Excited, 1.2f);
            Fx.Sparkles(Bella.HeadPoint, 8);
            AudioManager.Play("pop");
            Bella.Needs.AddHappiness(2f);
        }

        private void Update()
        {
            if (!Ready || Ending) return;
            if (Pointer.Down)
            {
                turning = !Pointer.OverUI;
                lastPointer = Pointer.Position;
            }
            if (turning && Pointer.Held)
            {
                float dx = (Pointer.Position.x - lastPointer.x) / Mathf.Max(1f, Screen.width);
                lastPointer = Pointer.Position;
                yaw -= dx * 360f;
                Bella.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            }
            if (Pointer.Up) turning = false;
        }
    }
}
