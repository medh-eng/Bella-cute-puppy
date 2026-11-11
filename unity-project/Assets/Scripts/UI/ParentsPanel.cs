using System;
using UnityEngine;
using UnityEngine.UI;
using Bella.Core;
using Bella.Save;

namespace Bella.UI
{
    /// <summary>
    /// The Parents' area (reached only through the parental gate): sound and music
    /// switches, the privacy summary, a diagnostics read-out and "start over".
    /// Nothing here links out of the app or collects anything.
    /// </summary>
    public class ParentsPanel : MonoBehaviour
    {
        public const string PrivacyText =
            "PRIVACY — THE SHORT VERSION\n\n" +
            "Bella the Cute Puppy does not collect, store or share any personal information. " +
            "It has no adverts, no in-app purchases, no accounts, no chat and no analytics, and it never connects to the internet.\n\n" +
            "The only thing the game saves is Bella's own progress (her needs, treats, outfits, tricks and stickers). " +
            "That file stays on this device and is removed when the game is uninstalled or when you choose 'Start over' here.\n\n" +
            "The game is designed for young children and follows the Google Play Families policy.";

        private Canvas canvas;
        private GameObject root, mainPage, textPage, confirmPage;
        private Text sfxLabel, musicLabel, pageText;
        private Action onReset;

        public void Init(Action resetRequested)
        {
            onReset = resetRequested;
            canvas = UIKit.CreateCanvas("ParentsCanvas", 55);
            canvas.transform.SetParent(transform, false);
            root = UIKit.Rect("ParentsRoot", canvas.transform).gameObject;
            UIKit.Stretch(root.GetComponent<RectTransform>());
            UIKit.Dim(root.transform, 0.7f);

            Image card = UIKit.Panel("Card", root.transform, Art.Cream);
            UIKit.Place(card.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(900f, 1500f));
            var topC = new Vector2(0.5f, 1f);
            var mid = new Vector2(0.5f, 0.5f);

            Text head = UIKit.Label("Head", card.transform, "For parents", 64, Art.Ink);
            UIKit.Place(head.rectTransform, topC, mid, new Vector2(0f, -100f), new Vector2(700f, 90f));
            Button close = UIKit.IconButton("Close", card.transform, "close", 110f, Art.Rose, Hide);
            UIKit.Place(close.GetComponent<RectTransform>(), new Vector2(1f, 1f), mid, new Vector2(-40f, -40f), new Vector2(110f, 110f));

            // ---- main page
            mainPage = UIKit.Rect("Main", card.transform).gameObject;
            UIKit.Stretch(mainPage.GetComponent<RectTransform>(), 0f, 0f, 0f, 170f);
            var size = new Vector2(720f, 130f);
            Button sfx = UIKit.TextButton("Sfx", mainPage.transform, "", size, Art.Teal, ToggleSfx);
            UIKit.Place(sfx.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -110f), size);
            sfxLabel = sfx.GetComponentInChildren<Text>();
            Button mus = UIKit.TextButton("Music", mainPage.transform, "", size, Art.Teal, ToggleMusic);
            UIKit.Place(mus.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -260f), size);
            musicLabel = mus.GetComponentInChildren<Text>();
            Button priv = UIKit.TextButton("Privacy", mainPage.transform, "Privacy", size, Art.Lilac, () => ShowText(PrivacyText));
            UIKit.Place(priv.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -410f), size);
            Button about = UIKit.TextButton("About", mainPage.transform, "About this game", size, Art.Lilac, () => ShowText(AboutText()));
            UIKit.Place(about.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -560f), size);
            Button diag = UIKit.TextButton("Diag", mainPage.transform, "Diagnostics", size, Art.Lilac, () => ShowText(Diagnostics.Report()));
            UIKit.Place(diag.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -710f), size);
            Button reset = UIKit.TextButton("Reset", mainPage.transform, "Start over", size, Art.Rose, () => ShowPage(confirmPage));
            UIKit.Place(reset.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -900f), size);
            Text ver = UIKit.Label("Version", mainPage.transform, "Version " + Application.version, 30, new Color(0.5f, 0.4f, 0.35f));
            UIKit.Place(ver.rectTransform, new Vector2(0.5f, 0f), mid, new Vector2(0f, 70f), new Vector2(700f, 50f));

            // ---- text page
            textPage = UIKit.Rect("TextPage", card.transform).gameObject;
            UIKit.Stretch(textPage.GetComponent<RectTransform>(), 0f, 0f, 0f, 170f);
            pageText = UIKit.Label("Text", textPage.transform, "", 34, Art.Ink, TextAnchor.UpperLeft);
            UIKit.Stretch(pageText.rectTransform, 60f, 220f, 60f, 20f);
            pageText.fontStyle = FontStyle.Normal;
            pageText.resizeTextForBestFit = true; pageText.resizeTextMinSize = 16; pageText.resizeTextMaxSize = 36;
            pageText.verticalOverflow = VerticalWrapMode.Truncate;
            Button back = UIKit.TextButton("Back", textPage.transform, "Back", new Vector2(420f, 120f), Art.Teal, () => ShowPage(mainPage));
            UIKit.Place(back.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), mid, new Vector2(0f, 110f), new Vector2(420f, 120f));

            // ---- confirm page
            confirmPage = UIKit.Rect("Confirm", card.transform).gameObject;
            UIKit.Stretch(confirmPage.GetComponent<RectTransform>(), 0f, 0f, 0f, 170f);
            Text sure = UIKit.Label("Sure", confirmPage.transform,
                "Start over?\n\nThis erases Bella's treats, outfits, tricks and stickers on this device. It cannot be undone.", 42, Art.Ink);
            UIKit.Place(sure.rectTransform, topC, mid, new Vector2(0f, -330f), new Vector2(740f, 480f));
            Button yes = UIKit.TextButton("Yes", confirmPage.transform, "Yes, erase everything", size, Art.Rose, () =>
            {
                Hide();
                if (onReset != null) onReset();
            });
            UIKit.Place(yes.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -720f), size);
            Button no = UIKit.TextButton("No", confirmPage.transform, "No, keep playing", size, Art.Leaf, () => ShowPage(mainPage));
            UIKit.Place(no.GetComponent<RectTransform>(), topC, mid, new Vector2(0f, -880f), size);

            root.SetActive(false);
        }

        private static string AboutText()
        {
            return "BELLA THE CUTE PUPPY\n\n" +
                   "A gentle virtual-pet game with no adverts and no purchases.\n\n" +
                   "Bella, her room, her sounds and her music are original work made for this game. " +
                   "Her 3D model and pictures were created with AI image and 3D tools and then rigged, animated and painted for the game; " +
                   "all sounds are synthesised.\n\n" +
                   "Tips: tap the thought bubble above Bella to give her what she wants. " +
                   "Draw shapes in Trick Training to teach her tricks. Come back each day for a present.";
        }

        public void Show()
        {
            Refresh();
            ShowPage(mainPage);
            root.SetActive(true);
        }

        private void Hide()
        {
            AudioManager.Play("tap");
            root.SetActive(false);
        }

        private void ShowPage(GameObject page)
        {
            mainPage.SetActive(page == mainPage);
            textPage.SetActive(page == textPage);
            confirmPage.SetActive(page == confirmPage);
        }

        private void ShowText(string content)
        {
            pageText.text = content;
            ShowPage(textPage);
        }

        private void ToggleSfx()
        {
            SaveSystem.Data.sfxOn = !SaveSystem.Data.sfxOn;
            SaveSystem.Flush();
            Refresh();
        }

        private void ToggleMusic()
        {
            SaveSystem.Data.musicOn = !SaveSystem.Data.musicOn;
            SaveSystem.Flush();
            if (AudioManager.Instance != null) AudioManager.Instance.RefreshSettings();
            Refresh();
        }

        private void Refresh()
        {
            sfxLabel.text = "Sounds: " + (SaveSystem.Data.sfxOn ? "ON" : "OFF");
            musicLabel.text = "Music: " + (SaveSystem.Data.musicOn ? "ON" : "OFF");
        }
    }
}
