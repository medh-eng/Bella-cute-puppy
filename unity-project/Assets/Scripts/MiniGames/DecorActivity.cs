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
    /// The Decor shop: buy wallpapers and floors with Treats and see them in the room straight away.
    /// Same shape as the Wardrobe and Feeding shops. The original look is always free ("Classic").
    /// Wallpapers and floors come from Resources/Surfaces; a theme whose picture is missing is hidden.
    /// </summary>
    public class DecorActivity : Activity
    {
        private const int PerPage = 6;

        private class Entry
        {
            public string kind;          // "wall" | "floor" | "rug" | "view" | "pic"
            public ThemeDef def;         // null = the free original look
            public Image cell;
            public GameObject price, check;
        }

        private readonly List<Entry> all = new List<Entry>();
        private readonly List<Entry> shown = new List<Entry>();
        private RectTransform sheet;
        private Text treatsLabel, pageLabel;
        private GameObject treatsPill;
        private int page;

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

            Image sheetImg = UIKit.Panel("Sheet", UI, new Color(1f, 0.97f, 0.9f, 0.96f));
            sheet = sheetImg.rectTransform;
            sheet.anchorMin = new Vector2(0f, 0f);
            sheet.anchorMax = new Vector2(1f, 0f);
            sheet.pivot = new Vector2(0.5f, 0f);
            sheet.anchoredPosition = new Vector2(0f, 20f);
            sheet.sizeDelta = new Vector2(-40f, 700f);

            // entries: Classic first, then every theme whose picture exists
            string[] kinds = { "wall", "floor", "rug", "view", "pic" };
            for (int k = 0; k < kinds.Length; k++)
            {
                int before = all.Count;
                all.Add(new Entry { kind = kinds[k], def = null });
                for (int i = 0; i < Catalog.Themes.Length; i++)
                    if (Catalog.Themes[i].kind == kinds[k] && Art.TryTex("Surfaces/" + Catalog.Themes[i].file) != null)
                        all.Add(new Entry { kind = kinds[k], def = Catalog.Themes[i] });
                if (all.Count == before + 1) all.RemoveAt(before);    // no pictures for this kind: no lonely "Classic" cell
            }

            for (int i = 0; i < all.Count; i++) BuildCell(all[i]);

            Button prev = UIKit.TextButton("Prev", sheet, "<", new Vector2(150f, 90f), Art.Teal, () => Turn(-1), 56);
            UIKit.Place(prev.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(40f, 30f), new Vector2(150f, 90f));
            Button next = UIKit.TextButton("Next", sheet, ">", new Vector2(150f, 90f), Art.Teal, () => Turn(1), 56);
            UIKit.Place(next.GetComponent<RectTransform>(), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-40f, 30f), new Vector2(150f, 90f));
            pageLabel = UIKit.Label("Page", sheet, "", 40, Art.Ink, TextAnchor.MiddleCenter);
            UIKit.Place(pageLabel.rectTransform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 30f), new Vector2(500f, 90f));

            ShowPage();
            G.CameraRig.Focus(G.Room.HomePoint, 3.6f, 0.45f);    // wide enough to see the walls and floor
            Bella.BeginActivity(G.Room.HomePoint, 180f, "Sit", () =>
            {
                Ready = true;
                Bella.Face.Set(Expression.Happy);
            });
        }

        private void BuildCell(Entry e)
        {
            string name = e.def == null ? "Classic" : e.def.id;
            Image cell = UIKit.Panel("Theme_" + e.kind + "_" + name, sheet, Color.white);
            e.cell = cell;

            // picture: the surface texture itself (or the original colour for "Classic")
            Color classic = e.kind == "wall" ? new Color(1f, 0.91f, 0.82f)
                          : e.kind == "floor" ? new Color(0.94f, 0.80f, 0.62f)
                          : e.kind == "rug" ? Art.Teal
                          : e.kind == "view" ? Art.Sky : Color.white;
            Image pic = UIKit.Panel("Swatch", cell.transform, classic, false);
            UIKit.Place(pic.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -16f), new Vector2(190f, 150f));
            if (e.def != null)
            {
                Sprite sp = Art.TrySprite("Surfaces/" + e.def.file);
                if (sp != null) { pic.sprite = sp; pic.type = Image.Type.Simple; pic.color = Color.white; }
            }

            RectTransform tag = UIKit.Rect("Price", cell.transform);
            UIKit.Place(tag, new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(0f, 50f), new Vector2(190f, 56f));
            if (e.def != null)
            {
                Image tc = UIKit.Icon("Coin", tag, "treat", 50f);
                tc.rectTransform.anchoredPosition = new Vector2(-44f, 0f);
                Text price = UIKit.Label("Text", tag, e.def.price.ToString(), 40, Art.Ink, TextAnchor.MiddleLeft);
                UIKit.Place(price.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(-12f, 0f), new Vector2(110f, 56f));
            }
            else
            {
                UIKit.Label("Text", tag, "Classic", 38, Art.Leaf, TextAnchor.MiddleCenter);
            }
            e.price = tag.gameObject;

            Image check = UIKit.Icon("Chosen", cell.transform, "check", 70f);
            UIKit.Place(check.rectTransform, new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-34f, -34f), new Vector2(70f, 70f));
            e.check = check.gameObject;

            Button b = cell.gameObject.AddComponent<Button>();
            b.transition = Selectable.Transition.None;
            b.targetGraphic = cell;
            Entry captured = e;
            b.onClick.AddListener(() => Tap(captured));
            cell.gameObject.AddComponent<PressFx>();
            cell.gameObject.SetActive(false);
        }

        private int PageCount { get { return Mathf.Max(1, (all.Count + PerPage - 1) / PerPage); } }

        private void Turn(int d)
        {
            if (Ending) return;
            page = (page + d + PageCount) % PageCount;
            AudioManager.Play("tap");
            ShowPage();
        }

        private void ShowPage()
        {
            shown.Clear();
            for (int i = 0; i < all.Count; i++)
            {
                bool on = i / PerPage == page;
                all[i].cell.gameObject.SetActive(on);
                if (!on) continue;
                int k = i % PerPage;
                int col = k % 3, row = k / 3;
                UIKit.Place(all[i].cell.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2((col - 1f) * 256f, -170f - row * 260f), new Vector2(236f, 250f));
                shown.Add(all[i]);
            }
            pageLabel.text = (page + 1) + " / " + PageCount;
            Refresh();
        }

        private bool Chosen(Entry e)
        {
            SaveData d = SaveSystem.Data;
            string id = e.def == null ? "" : e.def.id;
            return d.GetDecor(e.kind) == id;
        }

        private void Refresh()
        {
            SaveData d = SaveSystem.Data;
            treatsLabel.text = Bella.Needs.Treats.ToString();
            for (int i = 0; i < all.Count; i++)
            {
                Entry e = all[i];
                bool owned = e.def == null || d.ownedThemes.Contains(e.def.id);
                bool chosen = Chosen(e);
                e.price.SetActive(!owned);
                e.check.SetActive(chosen);
                e.cell.color = chosen ? new Color(1f, 0.93f, 0.6f) : owned ? Color.white : new Color(0.96f, 0.93f, 0.9f);
            }
        }

        private void Tap(Entry e)
        {
            if (!Ready || Ending) return;
            SaveData d = SaveSystem.Data;
            bool owned = e.def == null || d.ownedThemes.Contains(e.def.id);
            if (!owned)
            {
                if (!Bella.Needs.SpendTreats(e.def.price))
                {
                    AudioManager.Play("oops", 0.5f);
                    UIKit.Pop(treatsPill);
                    return;
                }
                d.ownedThemes.Add(e.def.id);
                AudioManager.Play("coin");
                Bella.Needs.AddHappiness(6f);
                Bella.Face.Flash(Expression.Excited, 1.2f);
                Fx.Stars(Bella.HeadPoint + Vector3.up * 0.2f, 10);
            }
            else
            {
                AudioManager.Play("pop");
            }
            string id = e.def == null ? "" : e.def.id;
            d.SetDecor(e.kind, id);
            G.Room.ApplyTheme(d.wallId, d.floorId);
            G.Room.ApplyExtras(d.rugId, d.viewId, d.picId);
            SaveSystem.Flush();
            Refresh();
        }
    }
}
