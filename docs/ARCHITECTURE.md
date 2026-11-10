# Bella the Cute Puppy — Technical Architecture

## 1. Stack

| Component | Tool | Why |
|---|---|---|
| Engine | Unity 3D (URP — Universal Render Pipeline) | Industry standard for mobile; URP scales from low-end to high-end devices |
| Language | C# | Unity's native scripting language |
| 3D modeling/rigging | Blender (free) or Maya | Base mesh, skeleton rig, blendshapes |
| Texturing | Substance 3D Painter, or hand-painted in Blender/Photoshop | Albedo textures; noise/alpha maps if shell-fur is attempted later |
| Version control | Git / GitHub (this repo) | Already set up |
| Build/CI (optional, later) | Unity Cloud Build | Lets Pon get builds without owning high-end hardware |

## 2. Finite State Machine (FSM) — Bella's behavior

Avoid a monolithic `Update()` method. Use the State Design Pattern: a common `IState` interface, one class per behavioral state, and a `PetStateMachine` that holds the current state and delegates to it.

```
IState
 ├── Enter()
 ├── Tick()   (called every frame by the state machine)
 └── Exit()
```

### Core states (v1)
- **IdleState** — default state. Continuously checks need thresholds:
  - Energy low → sub-behavior: yawning animation ("tired" idle variant)
  - Hunger critical → sub-behavior: begging animation + vocalization
- **InteractionState** — interrupt-driven, triggered by touch input (petting, feeding). Overrides idle immediately, plays a joyful reaction animation + particle burst, then returns to Idle.
- **LocomotionState** — triggered when Bella needs to move to an object (e.g., walking to the food bowl when feeding starts). Uses Unity's built-in NavMesh for pathfinding within the room.

Each state is its own script in `Assets/Scripts/States/`. See starter code already committed in this repo (`IState.cs`, `PetStateMachine.cs`, `IdleState.cs`).

## 3. Needs system and offline decay

Needs are floats that decay over real elapsed time, including while the app is closed or the device is off. The formula (do not use `Time.deltaTime` alone for long-term decay — that only ticks while the app is running):

```
V_current = max(V_min, V_saved - (R_decay * ΔT))
```

- `V_current` — the recalculated need value on app resume
- `V_saved` — the value stored at last save
- `R_decay` — decay rate per second for that specific need
- `ΔT` — elapsed seconds between the saved timestamp and now
- `V_min` — floor (typically 0) so values never go negative

**Critical implementation detail:** timestamp must use `DateTime.UtcNow`, not local device time, logged in `OnApplicationPause`/`OnApplicationFocus` and compared against the saved timestamp on resume. This prevents players from manipulating their device clock to bypass decay. See `Assets/Scripts/Save/` starter code.

## 4. Rendering / shader strategy

Realistic strand-based fur simulation is too GPU-expensive for mobile. Two options, in order of recommendation:

1. **Toon/cel shading (v1 default)** — flat, stylized shading with distinct light bands (base/shadow/highlight). Cheap, performant, visually distinct from Boo's realistic-fur look (which is a plus, not just a cost saving). Unity's official Toon Shader (URP) is free and open-source.
2. **Shell-based fur (stretch goal, not v1)** — multiple semi-transparent extruded shell passes + a noise alpha map for a fuzzy look (e.g. the FurFX approach). Each shell is an extra draw call — cap shell count hard to hold 30–60fps on mid-range devices. Only attempt this after v1 ships and only if performance budget allows.

Facial expression: use **blendshapes/morph targets**, not complex facial bone rigging. A handful of target shapes (smile, sad, open-mouth pant) interpolated via the Animator or direct C#, driven by the needs/happiness state.

## 5. Save system
- Local JSON (or Unity's `PlayerPrefs` for very small data, but prefer a JSON file in `Application.persistentDataPath` for anything beyond a few values) holding: each need's `V_saved`, last-saved UTC timestamp, currency, owned outfits/unlocks.
- No cloud save in v1 (keeps COPPA data-collection surface at zero). Add cloud save only if you later add account/login — which triggers a much bigger compliance lift, so avoid unless there's a real reason.

## 6. Project folder layout
See `PLAN.md` §4 for the full repo tree. Within `Assets/Scripts/`:
- `Core/` — `IState.cs`, `PetStateMachine.cs`
- `States/` — one file per state
- `Needs/` — `PetNeeds.cs`, `NeedDecayConfig.cs` (a ScriptableObject so decay rates are designer-tunable without touching code)
- `Save/` — `SaveData.cs`, `SaveSystem.cs`
- `MiniGames/` — one subfolder per mini-game, added as each is built (Phase 4)
- `UI/` — need-bar widgets, currency display, age-gate screen, parental gate

## 7. Performance targets
- 30–60 fps on mid-range Android devices (the realistic floor for the target market, not flagship-only).
- Avoid: global illumination, complex specular highlights, uncapped shell-fur passes, per-frame heavy physics on more than a couple of active objects.

## 8. Compliance-relevant technical notes
- Any analytics/crash-reporting SDK must be configured in "child-safe" mode (or omitted in v1) — default configs often collect device identifiers.
- Age-gate logic must branch at the code level: under-13 path permanently disables data collection and ad SDK initialization for that session, not just hides a UI element.
