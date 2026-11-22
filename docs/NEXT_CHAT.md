# Handoff — start the next chat here

**To start:** open a new chat in this project and say
*"Continue Bella — repo medh-eng/Bella-cute-puppy, read docs/NEXT_CHAT.md."*
If the Unity session has happened, paste the report from the bottom of `docs/UNITY_DAY.md` too.

Last updated: 22 Nov 2026.

---

## 1. Where the project is

**Update, 22 Nov 2026 — Seasons (festival calendar) built, sticker art wired; 69 scripts compile (stubcheck), not run in Unity.**
Festival of Lights, Festival of Colours, winter holidays and Bella's birthday decorate the living room on their dates, change the daytime
music and give a once-a-year greeting + 15 treats + sticker (`Systems/Seasons.cs`, `World/FestiveDecor.cs`; the calendar was tested
date-by-date against the real code). Parents' area has a **Festivals: ON/OFF** switch; in the Editor the **F** key previews each festival.
The sticker book now shows the real sticker pictures and has pages (it ran off the screen with 31 stickers). About text corrected (music and
puppy sounds are library recordings, not synthesised). 36 decoration pictures moved into `Resources/Seasons` (0.7 MB; Resources 58.7 of 60 MB).
Rough layout drawing: `docs/preview/festivals_mock.png`. Details: `docs/ROOMS_AND_MINIGAMES.md`.

### Plan for the next work (22 Nov 2026), in order

1. **Pon — Unity playtest (the most important step now).** Since the 16 Nov Unity day about 20 new scripts (Fetch, 15 mini-games,
   Places, title screen, Seasons, sticker book) have only been compile-checked. Pull `main` → Bella → Set Up Project → Play, and go through
   `docs/UNITY_DAY.md` Step 4 (new lines for festivals and the sticker book at the end). Then Step 6: a test APK on the phone. Send the
   report, screenshots of anything odd, and the Diagnostics text.
2. **Claude — fix round from that report** (Sonnet is fine). Tuning points are listed per feature in `docs/ROOMS_AND_MINIGAMES.md`.
3. **Claude — wire the art that is still waiting in `art-source/ready/`**: rugs, window views and wall pictures as Decor-shop items
   (with saving), the extra effect sprites, 3 more place backdrops (one is the festival street F-10 → a Festival place during festivals).
   No Unity needed.
4. **Claude — Agility course** (last planned mini-game). Needs a new jump animation: the Blender pipeline (`tools/pipeline/build_anim.py`)
   needs the `bpy` module — try `pip install bpy` (needs Python 3.11) in the session; then re-run the three Blender checks before committing.
5. **Pon — optional art**: I-05…I-08 (rainy season, kites/spring, harvest, New Year) and festival props G-81…G-88 from
   `docs/ASSET_PROMPTS_V2.md`. Each new set needs a few lines in `Seasons.cs` (dates) and `FestiveDecor.cs` (layout).
6. **Before release (Pon):** record where each audio file came from and its licence (`CREDITS.md` still says unknown); Play organisation
   account (D-U-N-S); internal test with a few families; store screenshots into `docs/store/screenshots/`.

**Update, 21 Nov 2026 (evening) — three more mini-games, Places, title screen; all compile (stubcheck, 67 scripts), none run in Unity.**
Hide & Seek, Paw Memory, Bubble Pop (Play menu = 12 games); Places (map button, `PlacePicker`, `VisitActivity`, `PlaceDef`, stickers visit_1 / visit_all);
`UI/TitleScreen.cs` (title_bg + logo + `music_title`, then four story cards `Story/intro_1..4` once, `SaveData.introSeen`). Details in `docs/ROOMS_AND_MINIGAMES.md`.
.NET SDK installs with `apt-get install -y dotnet-sdk-8.0` in the cloud session, so stubcheck can run. Left: Agility course (needs a jump clip), seasonal places / Seasons calendar
(art in `art-source/ready/Seasons`, `music_fest_*`, `music_garden` now used by Garden), then the Unity build and playtest.

**18 Nov 2026 (evening): the to-do list for new art and audio is now `docs/ASSET_PROMPTS_V2.md` (finished packs, tiers, every remaining prompt). The old `ASSET_PROMPTS.md` stays as reference.**

**Update, 19 Nov 2026 (night) — playtest fixes, not yet run in Unity.** Furniture is now tappable and movable (`RoomObject`, positions saved in `SaveData.objectSpots`); `PetInteraction` handles floor taps, object taps and long-press drag; Bella steers around furniture (`WalkToState.Steer/Clamp`); sleeping = walk to bed edge, hop on (`BellaController.HopTo`), hop off on waking; shadow raised to 3 cm (it z-fought the rug at 1.2 cm); tub flattened to 55 % with Bella lifted onto its floor (`Room.TubFloor`); fetch lands >= 1.2 m from her and she stops 0.72 m short; eating makes the mouth and head move (`Face.Munch`, `Puppy.Chewing`); walls and place backdrops reach below the floor. Armchair, lamp, water bowl and basket defaults moved inside the camera view (they were off-screen). Open: wardrobe still shows primitive 3D pieces.

**Update, 19 Nov 2026 (evening) — second and third art batches and 11 props are in; repo is ready for a Unity build.** Details in `art-inbox/INTAKE_2026-11-19.md`. In the game: 13 wallpapers/floors + stage floor (decor shop), 9 place backdrops, wish icons, new particle sprites, 11 prop models (made on Triple 3D, whose terms allow commercial use; reduced by Blender to 2,500 triangles). Waiting in `art-source/ready/` until code uses them: rugs, window views, wall pictures, season stickers (calendar feature not built), more effects, 3 more backdrops. Resources 57.9 of 60 MB. Not looked at: I-05..I-08, J-01..J-04. Nothing has been run in Unity yet: `docs/UNITY_DAY.md` is the checklist.

**Update, 18 Nov 2026 — corrected art absorbed; nothing held any more.** 81 files from Pon (`art-inbox/INTAKE_2026-11-18.md`): all 18 Bella portraits (D-01, D-02) and 27 stickers (D-03…D-05, new `Resources/Stickers/`) with names that match the pictures, B-03…B-08 UI parts redone in the 3D-toy look, a friendly `item_piratehat`, and 11 other icon fixes. All at final size, checked by eye. `art-source/generated/held-for-regeneration/` is gone. D-01, D-02 and D-04 were regenerated a second time the same day (portraits fit the popup circle with 8 units to spare; stickers keep a 32 px margin; `streak_30` redrawn). Still to do: wire stickers and the 12 new portraits into code (ASSET_PROMPTS §2) and look at them on the real badges in Unity. Still not generated: D-06…D-08, E…J.

**Update, 17 Nov 2026 — first generated art is in the repo.** Pon generated Waves 1–2 art in ChatGPT; `art-inbox/INTAKE_2026-11-17.md` lists every file, what was accepted and what is held. Accepted: logo, title screen, studio mark, story cards, jelly buttons, popup panel, loading pictures, B-03…B-08 UI parts (flat vector, provisional), app icon and foreground, and 89 icons (C-01…C-10 except the pirate hat). **Not wired yet** — next is §2.3/2.4 of `docs/ASSET_PROMPTS.md` (Art helpers, title screen, UI skin). Held for regeneration: all Bella portraits and stickers (names do not match pictures — D-01…D-05) and `item_piratehat`. Still missing from the pack: audio is already in; E, F, G, H, I, J are not generated.

**Update, 19 Nov 2026 (evening) — Fetch mini-game and tap-to-move written, NOT yet compiled or played.**
New: `MiniGames/FetchActivity.cs` (HUD play button now opens it), `States/FetchChaseState.cs`,
`BellaController.Run`/`RunState`, tap-to-move in `PetInteraction.cs`, stickers `fetch_10`/`fetch_50`, stubcheck
stubs for `Plane`, `Space`, `Ray.GetPoint`, `Transform.Rotate(axis, angle, space)`. The session had no .NET SDK
and no route to download one, so `tools/stubcheck/check.sh` still has to be run first. Things to judge in
Unity: flick strength (`Release()`), grab reach, mouth offset of the carried ball (`UpdateCarried`), camera
framing (`Focus` in `OnBegin`), drop onto the mat. Old `BellaController.RequestFetch` is kept but unused.
Plan for rooms and more mini-games: `docs/ROOMS_AND_MINIGAMES.md`.

**Update, 16 Nov 2026 (evening) — the asset upgrade is planned.** Pon now has ChatGPT Plus, Gemini
Pro, Tripo 3D and Pixabay, so the zero-spend, script-made look is no longer the ceiling.
`docs/ASSET_PROMPTS.md` holds every generation prompt (about 145, plus 70 Pixabay searches), where
each file goes, and a wiring guide written for Sonnet (its Part 2) with ready-made kickoff messages
(Part 3). Pon uploads results to `art-inbox/`; nothing has been generated or wired yet. Rules that
matter most: every generated asset is optional with today's look as the fallback; no Tripo model
ships until Pon confirms it came from a plan that allows commercial use; `CREDITS.md` gets a row for
every shipped file. The next asset session starts with the "Setup" message in Part 3.

Every phase of `docs/PLAN.md` that can be done without Unity or a Google account is finished and on
`main`: the full v1 game (46 scripts), Bella's rigged/animated model, faces, 24 sounds (5 music
loops + 2 nature-ambience loops + 17 effects/voices), 57 icons, store listing, Play Console
answers, privacy page, closed-test kit, feature graphic.

**Update, 16 Nov 2026 — Unity day happened.** Pon installed Unity 6.3 LTS with Android Build
Support and opened the project for the first time: it built and played, first try — walking,
eating, napping, fetching, bathing, tricks and wardrobe all worked. One real bug was found and
fixed (see below). A fuller "what we've built" recap lives in `docs/ACHIEVEMENTS.md`.

Found and fixed this session:
- **Wardrobe camera bug**: `WardrobeActivity.cs` framed the camera at ground level (look-height
  `0.02`), pushing Bella's head — and any hat — above the visible frame. Fixed to `0.6`, matching
  Bath/Clinic/Tricks.
- **Feeding became its own shop** (`MiniGames/FeedingActivity.cs`, new), same shape as Wardrobe:
  kibble (free), a treat meal, and a fancy meal, each with its own hunger/happiness effect
  (`Systems/Catalog.cs` → `FoodDef`/`Foods`). The HUD's Bone button now opens it
  (`GameRoot.OnHudAction`) instead of feeding instantly.
- **7 more outfit colour variants** added to `Catalog.cs` (19 outfits total) — no new geometry
  needed, just new `OutfitDef` entries (clothing is built from primitives in `Accessories.cs`, not
  from Meshy/Tripo3D, so new colours/shapes are free).
- **4 new puppy-voice moods** synthesised the same from-scratch way as the rest of the audio
  (`tools/pipeline/gen_audio.py`): `bark_happy`, `yawn_sleepy`, `whine_sad`, `whimper_hungry` —
  wired into `IdleState.cs`, `BellaController.RequestSleep`/`RequestFeed`. Honest limit: synthesis
  from sine waves and noise reads as an expressive cartoon dog, not a recording — real realism
  needs a licensed sample library or a paid AI audio model, neither of which is in the zero-spend
  plan.
- 3 new icons for the Feeding shop (`food_kibble`, `food_treat`, `food_fancy`,
  `tools/pipeline/gen_icons.py`).

**Not yet done:** no phone build has been made or installed outside the Editor yet (next:
`Bella → Build Android APK (test)` from `docs/UNITY_DAY.md` §Step 6) — this session's new code
needs a fresh `Bella → Set Up Project` + Play pass first to catch anything the stub-only compile
(`tools/stubcheck/check.sh`) couldn't.

### Checked without Unity on 12 Nov 2026 (so these are no longer guesses)

| Check | How | Result |
|---|---|---|
| Model matches the code's assumptions | `bella.fbx` re-imported in Blender | 23 bones with the expected names, rest pose Root→Head (0, 0.52, 0.23) and Root→Hips (0, 0.30, −0.25) exactly, height 0.943, 14k triangles, every vertex weighted, one take of 24.97 s |
| Loop seams | start vs end pose of all 15 clips | 0.0° / 0 mm difference on every clip |
| Floor contact | lowest mesh point on every 2nd frame | **Was broken**: roll-over went 64 cm under the floor, dance and play-bow 16 cm, sit/sleep/eat/beg 5–7 cm. **Fixed** in the pipeline (`build_anim.ground`); now ≤ 6 mm on all clips. Roll-over re-posed so she rolls on her back with chin tucked |
| Animation vs. model normalising | inspected the take | The take keys the model's own top object, which could undo `BellaModel`'s turn/scale/position every frame. **Fixed**: the model now sits in a holder object |
| Trick drawing | 4,000 simulated wobbly strokes per shape (`tools/gesture-test/run.sh`) | lines 100 %, circle 99 %, zigzag 97.5 % (was 90 % — shallow zigzags were read as lines; fixed), taps ignored 100 % |
| UI on tall and wide screens | arithmetic on canvas sizes | **Was broken**: bottom buttons clipped on 20:9 phones, sticker book overflowed on tablets. **Fixed**: canvas scaler uses Expand |
| Popup portraits | looked at the files | **Was broken**: opaque white squares on round badges. **Fixed**: backgrounds cut out |
| Treat economy | read every reward | Fetch / trick practice / healthy check-ups paid without limit (whole wardrobe in ~1 hour). **Fixed**: repeatable play shares 30 treats a day (`Progress.PlayTreats`); needed care, the daily present and newly learned tricks are unlimited → wardrobe ≈ 1–2 weeks |
| Nap on the bed | read `Room` + `SleepState` | She lay inside the cushion; now raised onto it (`ProceduralPuppy.HeightOffset`) |
| Android 16 Back gesture | Unity 6.3 docs | Setup switches on `PlayerSettings.Android.predictiveBackSupport` (by reflection, so older Unity still compiles) |

Pictures of what to expect are in `docs/preview/`.

### Content pass on 13 Nov 2026 — music, ambience, decor, butterflies (46 scripts)

| What | Where |
|---|---|
| **Music suite**: an original loop per moment of the day — `music_morning` (fresh), `music_day` (bouncy, light percussion), `music_evening` (golden-hour waltz), `music_night` (true lullaby with warm pad) — plus `music_play`, a quick giggly tune all four mini-games switch to (`Activity.Begin` → `DayNight.SetMusicOverride`) | `tools/pipeline/gen_audio.py`, `World/DayNight.cs`, `MiniGames/Activity.cs` |
| **Nature ambience** under the music: sparse birdsong (`amb_birds`) while the sun is up, gentle crickets (`amb_crickets`, seam-perfect 16 s loop) after dark, hush while she naps. New third audio channel | `Core/AudioManager.cs` (`SetAmbience`), `World/DayNight.cs` |
| **Living room**: clouds drift across the window by day; stars twinkle at night; the window orb now shows a proper **moon** icon (was Zzz); 2 tappable **butterflies** with heart-wings by day; 4 fireflies + a pulsing star nightlight at night | new `World/Ambience.cs` (46th script), wired in `GameRoot` |
| **Butterfly tap** = sparkles + hearts + yip + 4 happiness + `countButterflies++` → stickers `butterfly_1` "Butterfly friend" and `butterfly_20` "Garden of friends" | `Save/SaveData.cs`, `Systems/Catalog.cs`, `Systems/Progress.cs` |
| **Room decor**: rainbow pennant bunting near the ceiling, a toy shelf (teddy bear, picture books, spare ball) right of the window, a wicker toy basket with balls in the back-left corner — all outside Bella's wander area | `World/Room.cs` |
| **3 new icons** (57 total): `moon`, `cloud`, `butterfly` — previewed, match the house style | `tools/pipeline/gen_icons.py` |
| QC done without Unity: all 5 music tracks verified bar-grid-exact, no dead gaps, no clipping, seamless wraps; ambience loop periods divide the loop length exactly; stubcheck ALL_OK | — |

Worth a look on Unity day: how the heart-wing butterflies read in motion, ambience volume balance (`AudioManager.ambienceTarget = 0.16`), bunting/shelf placement on the real camera.

**Update, 17 Nov 2026 — first audio batch absorbed.** Pon supplied 14 music tracks and 3 puppy sounds
(`art-inbox/audio/REPORT.md` has the numbers). `tools/pipeline/ingest_audio.py` turns uploads into game
files: music → seamless looping stereo OGG at the old tracks' loudness, sounds → trimmed mono WAV, names
ending `_1`, `_2` = random variants. The five period/play tracks replaced their synthesised `.wav`s;
`bark_2`, `yip_1`, `whine_sad_1…4` replaced `bark`, `yip`, `whine_sad` (variant support and a gentler
pitch wobble for real recordings are in `Core/AudioManager.cs`). Mini-games now pick their own tune
(`Activity.Tune`: bath → `music_bath`, wardrobe and feeding → `music_shop`, tricks → `music_stage`, rest →
`music_play`). **Not wired yet, waiting on features:** `music_title` (title screen), `music_garden`
(garden place), `music_fest_*` (Seasons). **Batch 2 (same day):** `bark_1…3`, `bark_happy_1…5`, `whimper_hungry_1…3`, `yawn_sleepy`, `munch_1…4`, `snore_1…6`,
`sniff_1…3`, `lap`, `pant`. Long recordings holding many sounds in a row are cut into separate clips by `RULES` in
`ingest_audio.py`. New hooks: `sniff` (idle, 20 % of idle moments, quiet), `pant` (after fetch), snore breaths spaced
3.2–4.4 s. `lap` waits for a water bowl; still missing real recordings: `yip_2/3`, `whine_1/2`, `sparkle`-type effects, `howl`, `paws`, `shake`, `growl_play`,
`lick`. **Open:** the source/licence of every supplied file is not recorded
(ledger and `CREDITS.md` say so) — ask Pon before release. **Not compiled:** no .NET SDK in that session, so
`tools/stubcheck/check.sh` still has to be run; and every loop needs one listen in Unity (flagged ones in the
report first). More audio is coming — re-run the script with `--src <folder>`.

## 2. What is left, in order

### A. Nothing blocks Unity day
There is no remaining pre-Unity work that is required. Pon can go straight to `docs/UNITY_DAY.md`.

### B. Things only Pon can do (phone is enough)
1. ~~Decisions~~ — all settled on 16 Nov 2026 (see §4).
2. Google Play **organisation** account for Medhaneeka Healthcare Private Limited — applied for,
   D-U-N-S number applied for. Waiting on Dun & Bradstreet, then Google's verification.
3. ~~Website hosting~~ — site is hosted on Pon's existing Hostinger plan at
   `games.medhaneekahealthcare.in`, not GitHub Pages (so it survives the repo going private
   later). GitHub Pages was turned on earlier but is no longer the live address — steps for the
   Hostinger subdomain are in `docs/WEBSITE.md`.
5. Testers: an organisation account does not need the 12-tester closed test. Still worth asking
   5–6 families to try the build through **Internal testing** before launch.

### C. Unity day (PC) — `docs/UNITY_DAY.md`
Install Unity 6.3 LTS → open → Play checklist → 6 screenshots → test APK on phone → keystore → AAB.

### D. First chat after Unity day (Claude)
1. Read Pon's report; fix whatever failed (see §3 for where to look first).
2. Ask Pon to push the project back, or to upload the generated `.meta` files and
   `ProjectSettings/` folder, and **commit them** — until then asset ids are regenerated on every
   fresh download. (If Pon cannot push from the PC: zip `unity-project/ProjectSettings` and
   attach it to the chat.)
3. Tune feel from Pon's notes: decay rates and prices live in `Needs/NeedDecayConfig.cs` and
   `Systems/Catalog.cs`; camera in `World/CameraRig.cs`; walk/run speeds in `Pet/BellaController.cs`.
4. Put the chosen screenshots in `docs/store/screenshots/`.

### E. Then
Internal test with a few families → production review → launch → v1.1 (§5). (The 14-day closed
test in `docs/store/CLOSED_TEST_KIT.md` only applies if the account ends up personal after all.)
On launch day: put the Play link on the website (`docs/WEBSITE.md` → "When a game launches").

## 3. If something is wrong on first run — most likely causes first

| Symptom | Look at | Likely fix |
|---|---|---|
| Red compile errors in Console | The file and line named in the first error | An API differs from the stub. Fix the call; add the real signature to `tools/stubcheck/UnityEngine.cs` or `UnityEditor.cs` |
| Editor script error blocks Play | `Editor/BellaProjectSetup.cs`, `Editor/BellaImportSettings.cs` | Each settings step is already wrapped in `Step(...)`; a compile error here is an API name change — delete the offending line, the setting can be made by hand |
| "Touch input is switched off" screen | Player Settings → Active Input Handling | Menu Bella → Set Up Project, restart Unity |
| Blocky placeholder puppy | Parents' area → Diagnostics | "bones not found" → check the names `Root/Hips/Head/Ear_L/Tail_1..3` in the imported FBX hierarchy; "not found" → FBX failed to import |
| Bella slides without moving legs | Diagnostics line "Animation take … length" | Clip missing or not ~24.97 s → check the Animation tab of `bella.fbx`; `BellaModel.TryBuildReal` picks the longest clip |
| Bella lies on her side / faces away / wrong size | `Pet/BellaModel.cs` step 1–3 | The reference vectors assume Root→Head = (0, 0.52, 0.23), Root→Hips = (0, 0.30, −0.25) in the rest pose |
| Bella is pink | Shader missing | `Resources/Materials/BellaLit.mat` was not created → Bella → Set Up Project; in a build, check the three materials exist |
| Face looks wrong / expressions do not change | `Pet/FaceExpressionController.cs` | Face textures are full-body textures; check they loaded (Diagnostics "Face textures loaded: 6/6") |
| Icons have dark edges or look blurry | `Editor/BellaImportSettings.cs` | Reimport `Resources/Icons` (right-click → Reimport) |
| HUD under the camera hole on the phone | `UI/UIKit.cs` → `SafeArea` | Confirm `Screen.safeArea` is reported; Player Settings → "Render outside safe area" may stay on |
| Android back button closes the app from inside a mini-game | `GameRoot.Update` | Check Player Settings → Android → Predictive Back Support is on (setup sets it); if Back still is not seen as Escape, handle it with a custom manifest |
| Build fails | First red Console line | Usually a missing Android module in Unity Hub, or the keystore password |
| Play Console rejects the bundle | The message text | Target API below 36 → check Player → Target API Level = Automatic and the SDK installed by the Hub; debug-signed → keystore not selected |
| No butterflies / fireflies / clouds | `World/Ambience.cs` | Built by `GameRoot.Start` right after `RoomWander`; day critters hide at night and during naps, night ones in the day — check `DayNight.Current` first |
| Music doesn't change with the time of day, or mini-game tune sticks | `World/DayNight.cs` | `SetMusicOverride(false)` must run in `GameRoot.ActivityFinished`; the 30 s clock check only switches music when no override is on |
| Ambience (birds/crickets) silent | `Core/AudioManager.cs` | Third source added in `Init`; follows the same music toggle in the parents' area |

Still unverified and worth a look even if everything seems fine: how Unity's importer treats the
FBX (the checks above used Blender's importer), accessory positions on the real model
(`BellaModel.BuildAnchors`), how the roll-over, dance and nap read in motion, music loop points,
text fitting on small phones.

## 4. Decisions — all settled 16 Nov 2026

| # | Decision | Settled as |
|---|---|---|
| 1 | Android package id | **`com.medhaneeka.bella`** (`BellaProjectSetup.PackageId`) — permanent after the first Play upload |
| 2 | Public contact email | **med25healthcare@gmail.com** (same address as the company site) — in `docs/privacy-bella.html`, the store listing and the website |
| 3 | Company name inside the app | **Medhaneeka Games** (`BellaProjectSetup.ApplyPlayerSettings`) |
| 4 | Play account type | **Organisation** — Medhaneeka Healthcare Private Limited; no 12-tester test |
| 5 | GitHub repo public? | **Yes**, Pages is on |
| 6 | Website | Games hub + Bella page + privacy policy in `docs/`, hosted on Hostinger at `games.medhaneekahealthcare.in`; steps in `docs/WEBSITE.md` |

Suggested Play **developer name** (the name shown under the game in the store): "Medhaneeka Games"
if Play Console allows a display name different from the legal name; otherwise the legal name.

### Original table, kept for reference

| # | Decision | Current default | Why it matters |
|---|---|---|---|
| 1 | Android package id | `com.bellacutepuppy.game` (`BellaProjectSetup.PackageId`) | **Permanent** after the first Play upload |
| 2 | Public contact email | placeholder in `docs/privacy.html` | Shown on the store page and policy; many developers make a dedicated Gmail |
| 3 | Company name inside the app | "Bella Puppy Games" (`BellaProjectSetup.ApplyPlayerSettings`) | Cosmetic; the store shows the Play account's developer name instead |
| 4 | Personal or organisation Play account | personal assumed | Organisation (needs a D-U-N-S number) skips the 12-tester test |
| 5 | Is the GitHub repo public? | unknown | Free GitHub Pages needs a public repo; otherwise host the privacy page elsewhere |

## 5. Ideas parked for v1.1 (not needed for launch)

- Hindi and Marathi for the handful of text strings (popups, parents' area) — all text is in
  `Systems/Progress.cs`, `UI/ParentsPanel.cs`, `UI/ParentalGate.cs`, `Systems/Catalog.cs`
- Photo Studio with share-sheet export behind the parental gate
- Park / frisbee and track mini-games
- More outfits and a second room
- Haptic tick on petting; rating prompt for parents behind the gate
- Check whether Google's Teacher Approved programme is open for submissions

## 6. Working notes for Claude

- `main` is the working branch (matches this repo's history). Commit small, describe plainly.
- Run `tools/stubcheck/check.sh` after every code change (needs `dotnet`). When a new Unity API is
  used, add its **real** signature to the stub files — never bend the stub to make code pass.
- Do not add networking, analytics, ads, or any package that needs the INTERNET permission: the
  store answers in `docs/store/PLAY_CONSOLE_ANSWERS.md` all depend on that.
- Do not use `FindObjectOfType` / `FindObjectsOfType` variants (they differ between Unity versions).
- Run `tools/gesture-test/run.sh` after touching `GestureRecognizer.cs`.
- After touching the music in `gen_audio.py`: every melody's unit count must equal its chord grid
  (bars × bar length) — the 13 Nov QC script pattern in the chat checked exactly this, plus
  dead-gap and clipping sweeps. Ambience loops stay seamless only if every modulation period
  divides the loop length exactly.
- After re-exporting animations, re-run the three Blender checks described in the table in §1
  (rest pose, loop seams, floor contact) before committing the FBX.
- Regenerating assets: `tools/pipeline/` (Blender 4+/5 as the `bpy` Python module, numpy, Pillow);
  source mesh `art-source/bella_raw.glb`, rigged scene `art-source/bella_rigged_animated.blend`.
  If animations are re-exported, regenerate `Pet/BellaClips.cs` from `tools/pipeline/clips.json`.
- **Website rule (Pon, 16 Nov 2026): nothing about any game is disclosed publicly before it is on
  Google Play.** The public site shows only names, logos and "Coming soon" for five games (Bella,
  Black Swan 2049, Corebound, Indistock, Vyuha), a general `privacy.html` and Bella's own
  `privacy-bella.html`. Bella's full page waits in `docs/held-for-launch/`. Build the upload with
  `python3 tools/site/make_upload.py` (single-file pages, images built in).
- The website source (index.html, privacy.html, privacy-bella.html, assets/) lives in `docs/` here, but is
  **served from Pon's Hostinger hosting**, not GitHub Pages — after any change, give Pon a fresh
  zip of those files to upload (see `docs/WEBSITE.md`). Preview with headless Chrome at 400 px and
  1200 px before handing it over. Keep it free of outside scripts, fonts and trackers — the
  privacy page's claims rest on that too.
- Repo note: public for now at Pon's choice, to be made private once work is complete. Nothing
  about the website depends on this (see `docs/WEBSITE.md`); check before relying on any `.github.io`
  address.
- Facts checked on 11 Nov 2026 that will age: Play requires target API 36 for new apps; closed
  test = 12 testers × 14 days for new personal accounts; Unity 6.3 LTS is supported to Dec 2027.
  Re-check before upload.
- Model use: this was the reserved Fable pass. Sonnet is fine for the post-Unity fix-up round;
  escalate only if a problem survives it.
