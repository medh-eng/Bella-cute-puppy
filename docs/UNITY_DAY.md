# Unity day — the one sitting at the PC

Everything that can be done without Unity is already done and in this repository. This page is the
complete list of what is left for the day you sit at a computer. Plan for **2–3 hours**, most of it
waiting for downloads. No coding and no dragging things around inside Unity.

If anything goes wrong at any step: take a photo or screenshot of the screen and send it to Claude
with the words in the box at the bottom of this page.

---

## Before the day (can be done from the phone)

- [ ] Decide the two open choices in `docs/NEXT_CHAT.md` → "Decisions only Pon can make" (app id, contact email).
- [ ] Create the Google Play developer account (play.google.com/console, one-time US$25). Identity
      verification can take several days, so start early.
- [ ] Turn on GitHub Pages for the privacy policy (`docs/store/PLAY_CONSOLE_ANSWERS.md` step 1).
- [ ] Start collecting 15–20 testers' Gmail addresses (`docs/store/CLOSED_TEST_KIT.md`).

## Step 1 — Install Unity (≈ 45–60 min, mostly download)

1. Download and install **Unity Hub** from unity.com/download. Sign in with a free Unity account
   (Personal licence — free).
2. In Unity Hub: **Installs → Install Editor → Unity 6.3 LTS** (version numbers start with `6000.3`).
   Take the newest 6000.3 patch the Hub offers.
3. On the "Add modules" screen tick **Android Build Support**, including both sub-items
   **OpenJDK** and **Android SDK & NDK Tools**. Install. (About 10 GB in total.)

> Why Unity 6.3 and not the older 2022 version mentioned in early notes: since 31 August 2026 Google
> Play only accepts new apps built for Android 16 (API level 36) with 16 KB page support. Unity 6.3
> LTS does this out of the box; Unity 2022.3 needs manual work.

## Step 2 — Get the project (5 min)

1. On github.com/medh-eng/Bella-cute-puppy press the green **Code** button → **Download ZIP**.
2. Unzip it somewhere simple, for example `C:\Bella\`. Avoid OneDrive/Desktop-sync folders and
   very long paths.

## Step 3 — Open it (10–20 min the first time)

1. Unity Hub → **Projects → Add → Add project from disk** → choose the **`unity-project`** folder
   (the folder that contains `Assets`, not the outer one).
2. Click the project. If the Hub says the editor version differs, choose your installed 6000.3
   version and confirm.
3. Wait for the import to finish. The project **sets itself up automatically**: it creates the
   scene, the materials and the Android settings.
4. If a box appears saying *"Unity needs to restart once"* → press **Restart Unity now**. This
   happens at most once.

## Step 4 — Play (the moment of truth)

1. Press the **▶ Play** button at the top centre.
2. For the right shape: in the **Game** tab, open the resolution drop-down (it says "Free Aspect")
   → **+** → type **1080 × 1920** → select it.
3. Try each of these and tick what works (a picture of where the buttons sit:
   `docs/preview/home_screen_layout_mockup.png`):

- [ ] Bella is standing in her room, tail wagging, and she blinks
- [ ] Clicking/stroking her makes hearts appear
- [ ] **Bone** button: she walks to her bowl and eats
- [ ] **Zzz** button: she walks to her bed, lights dim, she naps
- [ ] **Ball** button: she fetches
- [ ] **Bath** button: bubble-bath game opens and can be finished
- [ ] **Tricks** button: drawing a downward line makes her sit; a circle makes her roll over
- [ ] **Wardrobe** (right side): buy and wear something
- [ ] **Clinic** (right side): three rounds of pick-the-tool
- [ ] **Book** (left side): sticker book opens
- [ ] **Gear** (top right): a sum appears; answering it opens the parents' page
- [ ] Sounds and music play

**If you see a brown, blocky puppy made of balls** instead of the fluffy Bella, the model did not
load. Open gear → (answer the sum) → **Diagnostics**, screenshot it, and send it to Claude.

## Step 5 — Screenshots for the store (10 min)

While the game is playing, use the top menu **Bella → Take Screenshot (store listing)** at each of
the six moments listed in `docs/store/STORE_LISTING.md`. The pictures are saved in
`unity-project/Screenshots/`.

## Step 6 — Build for the phone (20–40 min the first time)

**Test build first**

1. Stop Play. Top menu **Bella → Build Android APK (test)**. Wait.
2. The file `unity-project/Builds/Bella.apk` appears. Send it to your phone (USB cable, Google
   Drive or WhatsApp-to-yourself), tap it, allow "install unknown apps", install, play.
3. On the phone check: nothing hidden under the camera hole, buttons easy to press, sound works,
   and after closing the game for an hour Bella is a bit hungrier and greets you.

**Store build**

4. Make the upload key, once: **Edit → Project Settings → Player → Android tab (robot icon) →
   Publishing Settings → Keystore Manager → Keystore… → Create New → Anywhere**. Save it **outside**
   the project folder (e.g. `C:\Bella-keys\bella-upload.keystore`). Choose a password, add a key
   with alias `bella`, same password. Tick **Custom Keystore** and select it.
   - Write the password down and **back up the keystore file** (Google Drive is fine). Never put it
     in the GitHub repository.
5. Top menu **Bella → Build Android AAB (Play Store)** → `unity-project/Builds/Bella.aab`.
6. Upload that file in Play Console → **Testing → Closed testing → Create new release**, then follow
   `docs/store/PLAY_CONSOLE_ANSWERS.md` and `docs/store/CLOSED_TEST_KIT.md`.

For every later upload, raise the number at **Player → Other Settings → Bundle Version Code** by 1
first (2, 3, 4 …) — Google refuses a repeat number.

## What to send Claude afterwards

Copy this, fill it in, and paste it into a new chat:

```
Continue Bella — repo medh-eng/Bella-cute-puppy, read docs/NEXT_CHAT.md.
Unity day report:
- Unity version installed:
- Did it open and set itself up? (yes / error: ...)
- Play checklist: everything worked / these did not: ...
- Red errors in the Console window (Window > General > Console) — first one, copied exactly: ...
- On the phone: ...
- What felt wrong or boring: ...
[attach screenshots]
```

## Menu reference

| Menu | What it does |
|---|---|
| Bella → Set Up Project | Re-runs the automatic setup (safe to repeat) |
| Bella → Open Main Scene | Opens the (empty) scene the game starts from |
| Bella → Build Android APK (test) | Phone test build → `Builds/Bella.apk` |
| Bella → Build Android AAB (Play Store) | Store bundle → `Builds/Bella.aab` |
| Bella → Take Screenshot (store listing) | Saves the Game view to `Screenshots/` |
| Bella → Reset Save Data | Fresh puppy on this computer |
