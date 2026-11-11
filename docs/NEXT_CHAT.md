# Handoff — start the next chat here

**To start:** open a new chat in this project and say
*"Continue Bella — repo medh-eng/Bella-cute-puppy, read docs/NEXT_CHAT.md."*
If the Unity session has happened, paste the report from the bottom of `docs/UNITY_DAY.md` too.

Last updated: 11 Nov 2026.

---

## 1. Where the project is

Every phase of `docs/PLAN.md` that can be done without Unity or a Google account is finished and on
`main`: the full v1 game (45 scripts), Bella's rigged/animated model, faces, 19 sounds, 54 icons,
store listing, Play Console answers, privacy page, closed-test kit, feature graphic.

**The honest limit:** nobody has opened this project in Unity yet. The scripts compile against
hand-written stand-ins for the Unity API (`tools/stubcheck/check.sh`), and the animations were
previewed in Blender, but the first real run is still ahead. Expect a short fix-up round after it.

## 2. What is left, in order

### A. Nothing blocks Unity day
There is no remaining pre-Unity work that is required. Pon can go straight to `docs/UNITY_DAY.md`.

### B. Things only Pon can do (phone is enough)
1. **Decisions** (see §4) — package id and contact email at minimum.
2. Google Play developer account (US$25; verification can take days).
3. GitHub → Settings → Pages → branch `main`, folder `/docs` (repo must be public), so
   `docs/privacy.html` has a web address. Then replace `CONTACT-EMAIL-HERE` in that file.
4. Collect 15–20 tester Gmail addresses (`docs/store/CLOSED_TEST_KIT.md`).

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
Closed test (14 days) → production access → launch → v1.1 (§5).

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
| Android back button closes the app from inside a mini-game | `GameRoot.Update` | Android 16 predictive-back: may need `android:enableOnBackInvokedCallback="false"` via a custom manifest |
| Build fails | First red Console line | Usually a missing Android module in Unity Hub, or the keystore password |
| Play Console rejects the bundle | The message text | Target API below 36 → check Player → Target API Level = Automatic and the SDK installed by the Hub; debug-signed → keystore not selected |

Unverified assumptions worth a look even if everything seems fine: animation loop seams, ear/tail
bone axes (`ProceduralPuppy` rotates around the pivot's axes), accessory anchor positions on the
real model (`BellaModel.BuildAnchors` uses Blender-space coordinates), music loop points, text
fitting on small phones.

## 4. Decisions only Pon can make

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
- Regenerating assets: `tools/pipeline/` (Blender 4+/5 as the `bpy` Python module, numpy, Pillow);
  source mesh `art-source/bella_raw.glb`, rigged scene `art-source/bella_rigged_animated.blend`.
  If animations are re-exported, regenerate `Pet/BellaClips.cs` from `tools/pipeline/clips.json`.
- Facts checked on 11 Nov 2026 that will age: Play requires target API 36 for new apps; closed
  test = 12 testers × 14 days for new personal accounts; Unity 6.3 LTS is supported to Dec 2027.
  Re-check before upload.
- Model use: this was the reserved Fable pass. Sonnet is fine for the post-Unity fix-up round;
  escalate only if a problem survives it.
