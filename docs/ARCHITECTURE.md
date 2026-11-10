# Bella the Cute Puppy — Technical Architecture (v2)

## 1. Stack

| Component | Tool | Why |
|---|---|---|
| Engine | Unity 2022.3 LTS (URP) | Industry standard for mobile; URP scales to low-end Android |
| Language | C# | Unity native |
| 3D character | **AI pipeline: ChatGPT/Gemini image → Meshy/Tripo3D image-to-3D → quadruped auto-rig → GLB/FBX** | Zero-cost, no artist (see `ART_BIBLE.md` §7 for the exact workflow) |
| Animation | Meshy/Tripo preset clips (walk/idle/run) + **procedural secondary motion in C#** | AI presets are limited for quadrupeds; procedural motion fills the gap and is where the cuteness lives |
| Faces | Texture-swap on head material (`FaceExpressionController`) | AI models have no blendshapes; texture faces are the cartoon-correct, mobile-cheap alternative |
| Version control | Git / GitHub (this repo) | Set up |
| Target | Android only, min API ~24, 30–60 fps on mid-range devices | The actual market |

## 2. Finite State Machine — Bella's behavior

State Design Pattern: `IState` (Enter/Tick/Exit), one class per state, `PetStateMachine` MonoBehaviour delegating `Update()`.

### States (Phase 1, all implemented)
- **IdleState** — default. Watches need thresholds → sets animator/expression flags (tired yawn, hungry begging). Occasional random "life" behaviors on a timer (ear twitch, look around, sit).
- **InteractionState** — interrupt-driven (petting, feeding). Plays joyful reaction + particles, then auto-returns to Idle after a configurable duration.
- **WalkToState** — moves Bella to a target (food bowl, bed). v1 uses direct steering with turn-toward-target in a single room — **no NavMesh** (one rectangular room needs no pathfinding; removing the dependency simplifies the scene). Re-add `com.unity.ai.navigation` only if rooms gain obstacles.
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

## 6. Scene construction — editor bootstrap (minimum-intervention keystone)
`Assets/Scripts/Editor/BellaSceneBootstrap.cs` adds menu item **Bella → Build Test Scene**: programmatically creates camera, light, room, placeholder Bella (primitives: body/head/ears/tail/face), HUD canvas with need bars, feed button, event system, and wires every reference. Pon never drags a single reference in the Inspector. The same pattern will later wire the real model (menu: **Bella → Setup Imported Model**).

## 7. Project folder layout
```
unity-project/Assets/Scripts/
├── Core/        IState, PetStateMachine, GameBootstrap
├── States/      IdleState, InteractionState, WalkToState, SleepState
├── Needs/       PetNeeds, NeedDecayConfig
├── Save/        SaveData, SaveSystem
├── Pet/         ProceduralPuppy, FaceExpressionController, PetInteraction
├── UI/          NeedsHud, FeedButton (programmatic UI, icon-first)
├── Editor/      BellaSceneBootstrap
└── MiniGames/   (Phase 4, one folder per game)
```

## 8. Performance targets
- 30–60 fps on mid-range Android; single skinned mesh, texture-swap faces, no realtime GI, baked/simple lighting, one directional light, URP mobile settings.
- AI-generated meshes are often dense: run Meshy's retopo/low-poly option or decimate in Blender if the model exceeds ~15–25k tris.

## 9. Compliance-relevant technical notes
- v1 ships with **no analytics/crash SDKs at all**. If ever added: child-safe config, no device IDs.
- Age-gate branches in code: under-13 path keeps all collection/ads disabled (v1: they're disabled for everyone anyway).
- Parental gate: randomized two-number arithmetic, new numbers each attempt, no retry hints.
