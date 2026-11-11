using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Needs;

namespace Bella.UI
{
    /// <summary>
    /// The home-screen HUD, icon-first for pre-readers: five need rings along the
    /// top, treats + day counter, and big round action buttons along the bottom.
    /// GameRoot supplies the callbacks; this class only draws and forwards taps.
    /// </summary>
    public class Hud : MonoBehaviour
    {
        public Canvas Canvas { get; private set; }
        public RectTransform Root { get; private set; }

        private PetNeeds needs;
        private readonly Image[] rings = new Image[5];
        private readonly GameObject[] gauges = new GameObject[5];
        private Text treatsLabel, dayLabel;
        private GameObject treatsPill, giftButton, clinicButton;
        private int shownTreats = -1;

        public void Init(PetNeeds n, Action<string> onAction)
        {
            needs = n;
            Canvas = UIKit.CreateCanvas("HudCanvas", 10);
            Canvas.transform.SetParent(transform, false);
            Root = UIKit.SafeRoot("HudRoot", Canvas.transform);

            var top = new Vector2(0f, 1f);
            string[] icons = { "bone", "bubbles", "zzz", "heart", "health" };
            Color[] colors = { Art.Orange, Art.Sky, Art.Lilac, Art.Pink, Art.Leaf };
            for (int i = 0; i < 5; i++)
            {
                Image back = UIKit.Img("Need_" + icons[i], Root, Art.Icon("circle"), new Color(1f, 1f, 1f, 0.85f));
                UIKit.Place(back.rectTransform, top, new Vector2(0.5f, 0.5f), new Vector2(100f + i * 152f, -110f), new Vector2(140f, 140f));
                Image ring = UIKit.Img("Ring", back.transform, Art.Icon("circle"), colors[i]);
                UIKit.Stretch(ring.rectTransform, 5f, 5f, 5f, 5f);
                ring.type = Image.Type.Filled;
                ring.fillMethod = Image.FillMethod.Radial360;
                ring.fillOrigin = (int)Image.Origin360.Top;
                ring.fillClockwise = true;
                Image inner = UIKit.Img("Inner", back.transform, Art.Icon("circle"), Art.Cream);
                UIKit.Stretch(inner.rectTransform, 19f, 19f, 19f, 19f);
                UIKit.Icon("Icon", back.transform, icons[i], 78f);
                rings[i] = ring;
                gauges[i] = back.gameObject;
            }

            // Parents' corner (behind the parental gate)
            Button gear = UIKit.IconButton("Parents", Root, "gear", 104f, new Color(1f, 1f, 1f, 0.8f), () => onAction("parents"));
            UIKit.Place(gear.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-84f, -110f), new Vector2(104f, 104f));

            // Treats + day pills
            Image pill = UIKit.Panel("TreatsPill", Root, new Color(1f, 1f, 1f, 0.9f), false);
            UIKit.Place(pill.rectTransform, top, new Vector2(0f, 0.5f), new Vector2(34f, -250f), new Vector2(250f, 96f));
            Image coin = UIKit.Icon("Coin", pill.transform, "treat", 84f);
            UIKit.Place(coin.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(52f, 0f), new Vector2(84f, 84f));
            treatsLabel = UIKit.Label("Treats", pill.transform, "0", 50, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(treatsLabel.rectTransform, 104f, 0f, 14f, 0f);
            treatsPill = pill.gameObject;

            Image dayPill = UIKit.Panel("DayPill", Root, new Color(1f, 1f, 1f, 0.9f), false);
            UIKit.Place(dayPill.rectTransform, top, new Vector2(0f, 0.5f), new Vector2(304f, -250f), new Vector2(250f, 96f));
            Image star = UIKit.Icon("Star", dayPill.transform, "star", 78f);
            UIKit.Place(star.rectTransform, new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(52f, 0f), new Vector2(78f, 78f));
            dayLabel = UIKit.Label("Day", dayPill.transform, "1", 50, Art.Ink, TextAnchor.MiddleLeft);
            UIKit.Stretch(dayLabel.rectTransform, 104f, 0f, 14f, 0f);

            // Daily gift (only visible when one is waiting)
            Button gift = UIKit.IconButton("Gift", Root, "gift", 150f, Art.Gold, () => onAction("gift"));
            UIKit.Place(gift.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(-100f, -262f), new Vector2(150f, 150f));
            gift.transform.Find("Icon").gameObject.AddComponent<UIPulse>().amount = 0.14f;
            giftButton = gift.gameObject;
            giftButton.SetActive(false);

            // Side buttons
            var bl = new Vector2(0f, 0f);
            var br = new Vector2(1f, 0f);
            Button book = UIKit.IconButton("Stickers", Root, "book", 150f, Art.Cream, () => onAction("stickers"));
            UIKit.Place(book.GetComponent<RectTransform>(), bl, new Vector2(0.5f, 0.5f), new Vector2(104f, 370f), new Vector2(150f, 150f));
            Button wardrobe = UIKit.IconButton("Wardrobe", Root, "wardrobe", 150f, Art.Cream, () => onAction("wardrobe"));
            UIKit.Place(wardrobe.GetComponent<RectTransform>(), br, new Vector2(0.5f, 0.5f), new Vector2(-104f, 370f), new Vector2(150f, 150f));
            Button clinic = UIKit.IconButton("Clinic", Root, "clinic", 150f, Art.Cream, () => onAction("clinic"));
            UIKit.Place(clinic.GetComponent<RectTransform>(), br, new Vector2(0.5f, 0.5f), new Vector2(-104f, 545f), new Vector2(150f, 150f));
            clinicButton = clinic.gameObject;

            // Main action row
            string[] acts = { "feed", "bath", "sleep", "play", "tricks" };
            string[] actIcons = { "bone", "bath", "zzz", "ball", "tricks" };
            Color[] actColors = { Art.Orange, Art.Sky, Art.Lilac, Art.Leaf, Art.Gold };
            for (int i = 0; i < acts.Length; i++)
            {
                string act = acts[i];
                Button b = UIKit.IconButton("Action_" + act, Root, actIcons[i], 184f, actColors[i], () => onAction(act));
                UIKit.Place(b.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f),
                    new Vector2((i - 2) * 204f, 150f), new Vector2(184f, 184f));
            }

            needs.OnNeedsChanged += Refresh;
            needs.OnTreatsEarned += OnTreats;
            Refresh();
        }

        private void OnDestroy()
        {
            if (needs != null)
            {
                needs.OnNeedsChanged -= Refresh;
                needs.OnTreatsEarned -= OnTreats;
            }
        }

        public void SetVisible(bool visible)
        {
            if (Root != null) Root.gameObject.SetActive(visible);
        }

        public void SetGiftAvailable(bool available)
        {
            if (giftButton != null) giftButton.SetActive(available);
        }

        /// <summary>Wiggle the Clinic button while Bella has the sniffles.</summary>
        public void SetClinicAlert(bool on)
        {
            if (clinicButton == null) return;
            Transform icon = clinicButton.transform.Find("Icon");
            if (icon == null) return;
            UIPulse pulse = icon.GetComponent<UIPulse>();
            if (on && pulse == null) { pulse = icon.gameObject.AddComponent<UIPulse>(); pulse.amount = 0.16f; }
            if (pulse != null) pulse.enabled = on;
        }

        public void SetDay(int day)
        {
            if (dayLabel != null) dayLabel.text = Mathf.Max(1, day).ToString();
        }

        private void OnTreats(int amount)
        {
            AudioManager.Play("coin", 0.7f);
            UIKit.Pop(treatsPill);
        }

        private void Refresh()
        {
            if (needs == null) return;
            SetRing(0, needs.Hunger);
            SetRing(1, needs.Hygiene);
            SetRing(2, needs.Energy);
            SetRing(3, needs.Happiness);
            SetRing(4, needs.Health);
            SetClinicAlert(needs.IsUnwell);
            if (needs.Treats != shownTreats)
            {
                shownTreats = needs.Treats;
                treatsLabel.text = shownTreats.ToString();
            }
        }

        private void SetRing(int i, float value)
        {
            if (rings[i] == null) return;
            rings[i].fillAmount = Mathf.Clamp01(value / 100f);
        }
    }
}
