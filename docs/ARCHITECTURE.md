# Bella the Cute Puppy — Technical Architecture (v3)

## 1. Stack

| Component | Tool | Why |
|---|---|---|
| Engine | **Unity 6.3 LTS**, built-in render pipeline, legacy diffuse shaders | Meets Google Play's target-API-36 / 16 KB rules out of the box; flat cartoon shading needs no URP and runs on low-end phones |
| Language | C# | Unity native |
| 3D character | AI image → AI image-to-3D mesh → **rigged, weighted and animated by script in Blender** (`tools/pipeline/`) → one FBX | Zero-cost, no artist, fully reproducible |
| Animation | 15 clips baked into ONE take, played as frame ranges through the Playables API (`BellaAnimator` + `BellaClips`) + **procedural secondary motion in C#** | No AnimatorController asset, no import settings to click; procedural motion is where the cuteness lives |
| Input | Classic `Input` class, wrapped by `Core/Pointer.cs` (first touch = mouse) | Same code path in Editor and on device; the setup script forces *Active Input Handling = Input Manager* |
| UI | uGUI built entirely in code (`UI/UIKit.cs`), 1080 × 1920 reference, safe-area aware | Nothing to wire in the Inspector |
| Faces | Texture-swap on head material (`FaceExpressionController`) | AI models have no blendshapes; texture faces are the cartoon-correct, mobile-cheap alternative |
| Version control | Git / GitHub (this repo) | Set up |
| Target | Android only, IL2CPP, ARM64 + ARMv7, target API = highest installed, min API = Unity default, 30–60 fps on mid-range devices | The actual market |

## 2. Finite State Machine — Bella's behavior

State Design Pattern: `IState` (Enter/Tick/Exit), one class per state, `PetStateMachine` MonoBehaviour delegating `Update()`.

### States (Phase 1, all implemented)
- **IdleState** — default. Watches need thresholds → sets animator/expression flags (tired yawn, hungry begging). Occasional random "life" behaviors on a timer (ear twitch, look around, sit).
- **InteractionState** — interrupt-driven (petting, feeding). Plays joyful reaction + particles, then auto-returns to Idle after a configurable duration.
- **WalkToState** — moves Bella to a target (food bowl, bed). v1 uses direct steering with turn-toward-target in a single room — **no NavMesh** (one rectangular room needs no pathfinding; removing the dependency simplifies the scene). Add `com.unity.ai.navigation` only if rooms gain obstacles.
- **SleepState** — entered via the bed; dims lights, restores energy over a short on-screen sequence (plus "slept while away" credit from offline time).

Transitions are requested by *systems* (input, needs, UI), never decided inside states — states only do, they don't orchestrate.

## 3. Needs: live decay + offline decay

Two decay paths, same config:
1. **Live:** `PetNeeds.Update()` applies `R_decay * Time.deltaTime` each frame while playing. (v1 bugfix: the original starter code only decayed on resume — needs froze during play.)
2. **Offline:** on load/resume: `V_current = max(V_min, V_saved − R_decay_offline × ΔT)` with ΔT from **UTC timestamps**. Offline rates are gentler than live rates (configurable multiplier) so a school day away ≈ "hungry and happy to see you", and there is a **max offline decay cap** so returning after a month never shows a miserable puppy (kindness cap, GDD §4).

**Serialization bug fixed:** `JsonUtility` cannot serialize `System.DateTime` — it silently drops the field, which would have broken offline decay entirely. Timestamps are now stored as `long utcTicks` (`DateTime.UtcNow.Ticks`) and reconstructed via `new DateTime(ticks, DateTimeKind.Utc)`.

Clock-manipulation guard: `ΔT` is clamped to ≥ 0 and ≤ cap; a backwards clock just means "no decay", never negative time or rewards.

## 4. Expression & procedural animation layer

### FaceExpressionController
- Enum `Expression { Happy, Excited, Sad, Sleepy, Begging, Soapy }`.
- Swaps the head material's base texture from an assigned array (AI-generated face PNGs on the model's UV face region — or on a simple quad "face plane" parented to the head bone if UVs are awkward, Animal-Crossing style).
- On the Phase-1 placeholder it tints a face sphere instead; the API is identical, so swapping in the real model is a drop-in.

### ProceduralPuppy (the cuteness engine)
All transform-level, animation-system-independent, works on placeholder AND rigged model root:
- Idle breathing bounce (sine on local Y scale/pos)
- Tail wag: wag frequency/amplitude mapped to Happiness (continuous emotional telemetry — players *feel* the needs system without reading bars)
- Squash-and-stretch pulse on pet/feed
- Head tilt toward touch point (later: toward dragged food)
- Ear flop lag (simple spring on ear transforms once the rigged model provides ear bones)

## 5. Save system
- Local JSON at `Application.persistentDataPath/bella_save.json`. No cloud, no accounts (COPPA surface = zero).
- `SaveData`: needs, `utcTicks`, treats currency, owned outfit IDs, learned tricks, sticker IDs, first-play date (for growth system), day-streak counter.
- Atomic-ish write (write temp file, then move) to survive kills mid-save.
- Versioned (`saveVersion` int) for forward migration.

## 6. Construction — everything in code (minimum-intervention keystone)

- **`Core/GameRoot.cs`** boots itself after any scene loads (`RuntimeInitializeOnLoadMethod`) and builds
  camera, light, audio, room, Bella, HUD, popups, parental gate, parents' panel, sticker book and the
  progress system. If construction throws, it shows a plain-language error screen to screenshot.
- **`Core/Art.cs`** makes every material, primitive, sprite and font at run time from a few files in
  `Resources/`. Materials are cloned from three template assets in `Resources/Materials/` so their
  shaders are included in Android builds.
- **`Core/Diagnostics.cs`** collects non-fatal problems (missing model, missing clips); readable in
  the parents' area and written to `bella_diagnostics.txt`.
- **`Pet/BellaModel.cs`** loads `Resources/Bella/bella.fbx` and normalises orientation, scale and
  position from bone positions, so it does not depend on importer axis/scale choices. If the model or
  skeleton is missing it builds a primitive placeholder puppy and the game stays playable.
- **`Editor/BellaProjectSetup.cs`** runs automatically on first open: creates the template materials,
  an empty `Assets/Scenes/Main.unity`, the build list, Android player settings and the app icon;
  fixes input handling. Menu **Bella →** Set Up Project / Build APK / Build AAB / Take Screenshot / Reset Save.
- **`Editor/BellaImportSettings.cs`** applies texture, model and audio import settings on import.
- No `.meta` files or `ProjectSettings` are committed yet — Unity generates them on first open.
  **Commit them after the first successful run** so asset identities stay stable.

## 7. Project folder layout
```
unity-project/Assets/Scripts/
├── Core/        GameRoot, Art, AudioManager, Diagnostics, Pointer, IState, PetStateMachine
├── States/      IdleState, InteractionState, WalkToState, SleepState
├── Needs/       PetNeeds, NeedDecayConfig
├── Save/        SaveData, SaveSystem
├── Pet/         BellaController, BellaModel, BellaAnimator, BellaClips, FaceExpressionController,
│                ProceduralPuppy, Accessories, Fx, PetInteraction
├── World/       Room, DayNight, CameraRig, RoomWander
├── Systems/     Catalog (outfits, tricks, stickers), Progress (days, gift, growth, sniffles, stickers)
├── UI/          UIKit, Hud, ThoughtBubble, Popups, StickerBook, ParentalGate, ParentsPanel
├── MiniGames/   Activity (base), TrickActivity + GestureRecognizer, WardrobeActivity,
│                ClinicActivity, BathActivity
└── Editor/      BellaProjectSetup, BellaImportSettings
```

Tuning lives in two places: `Systems/Catalog.cs` (prices, tricks, stickers) and
`Needs/NeedDecayConfig.cs` (decay rates, thresholds, kindness cap).

## 8. Performance targets
- 30–60 fps on mid-range Android; single skinned mesh, texture-swap faces, no realtime GI, baked/simple lighting, one directional light with soft shadows at short distance, built-in pipeline.
- AI-generated meshes are often dense: Bella's was decimated in the Blender pipeline before export.
- Checking without Unity: `tools/stubcheck/check.sh` compiles all scripts against stand-in Unity APIs.

## 9. Compliance-relevant technical notes
- v1 ships with **no analytics/crash SDKs at all**. If ever added: child-safe config, no device IDs.
- Age-gate branches in code: under-13 path keeps all collection/ads disabled (v1: they're disabled for everyone anyway).
- Parental gate: randomized two-number arithmetic, new numbers each attempt, no retry hints.
- `Packages/manifest.json` lists only the engine modules the game uses; there is no networking, analytics or web-request module in the build, and the setup script keeps the INTERNET permission off.
- No age screen: the audience is children only (see `store/PLAY_CONSOLE_ANSWERS.md` §5).
