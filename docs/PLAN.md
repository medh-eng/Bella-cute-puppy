# Bella the Cute Puppy — Master Development Plan

**Project:** A virtual-pet mobile game starring Bella, a small, very happy Golden Retriever puppy — in the spirit of *Boo: The World's Cutest Dog*, but an original character, original art, and original codebase.

**Repo:** `medh-eng/bella-cute-puppy`
**Owner:** Pon (non-technical founder/director; hands-on with specs, review, and decisions)
**Primary builder:** Claude (Sonnet for ~90% of production work, Opus escalation for the hardest design/architecture calls, Fable reserved for a small number of high-leverage sessions — see §5)

This document is the top-level plan. It links out to the supporting docs in this folder:
- [`GDD.md`](./GDD.md) — full game design: core loop, needs system, mini-games, monetization, compliance
- [`ARCHITECTURE.md`](./ARCHITECTURE.md) — technical architecture: FSM, offline-time math, shaders, project structure
- [`ART_BIBLE.md`](./ART_BIBLE.md) — Bella's character design spec (how she must look, and must NOT look like Boo)
- [`AI_WORKFLOW.md`](./AI_WORKFLOW.md) — exactly which AI model/tool handles which task, with ready-to-use prompts

---

## 1. Goal and operating principle

**Goal:** ship a polished, COPPA-compliant, mobile virtual-pet game to iOS/Android, built with minimum hands-on effort from Pon.

**Operating principle — "minimum intervention":** Claude does the design documentation, all code, all architecture decisions, and all project-management breakdown. Pon's role is reduced to four things:
1. Answering a small number of *decision* questions I can't make for you (budget, timeline, monetization appetite, store accounts).
2. Running Unity locally (or via a cloud build service) to actually compile/test, since I can write code but can't execute a game engine myself from chat.
3. Approving/rejecting generated art and audio (since licensed asset purchases and store submissions need a human + payment method).
4. Playtesting and giving feedback at each milestone.

Everything else — GDD, C# scripts, shader code, state machines, save-data logic, store-listing copy, ASO research synthesis, compliance checklists — is produced directly into this repo by Claude across sessions.

---

## 2. Phased roadmap

### Phase 0 — Foundation (this session)
- [x] Repo scaffolded (`docs/`, `unity-project/Assets/...`)
- [x] GDD, architecture doc, art bible, AI workflow doc written
- [x] Starter C# scripts for the needs system and FSM committed
- [ ] Pon: install Unity Hub + Unity 2022 LTS (or newer LTS) locally, open `unity-project/`, confirm it loads with no errors

### Phase 1 — Vertical slice (core loop only)
**Target:** one room, Bella idle/react/eat, one need (hunger) fully working, save/load with offline decay.
- Claude: write `PetNeeds`, `PetStateMachine`, `SaveSystem`, hunger-feeding interaction script, basic UI (need bars).
- Claude: write placeholder-cube/capsule test scene instructions so Pon can press Play without any final art.
- Pon: open Unity, press Play, confirm hunger decays over time and feeding restores it. Report back anything that errors.

### Phase 2 — Full needs system + Idle/Interaction/Locomotion FSM
- Claude: implement all 5 needs (hunger, hygiene, energy, health, happiness) per the decay formula in `ARCHITECTURE.md`.
- Claude: implement the 3 core FSM states (Idle, Interaction, Locomotion) with NavMesh-based walk-to-bowl behavior.
- Pon: test, report feel ("too fast decay", "animation missing" etc. — no code needed from you).

### Phase 3 — Character art pipeline
- Pon + an artist/freelancer (Hyderabad AVGC network, see `GDD.md` §8) OR an outsourced Fiverr/Upwork 3D artist: produce Bella's rigged 3D model + blendshapes per `ART_BIBLE.md`.
- Claude: write the Animator/Blendshape-driving code so the model slots in once delivered — this doesn't block on having final art; we build against a placeholder capsule first.
- Claude: write the Toon Shader / shell-fur shader setup instructions (Unity Toon Shader URP is the default recommendation — cheapest to integrate, good mobile performance; shell-fur is a stretch goal, not v1).

### Phase 4 — Mini-games (one at a time)
Build in this order (easiest → hardest, cheapest → most expensive to iterate):
1. **The Wardrobe** (drag-and-drop dress-up) — pure UI, no physics, fastest to ship.
2. **The Clinic** (match-the-tool puzzle) — simple logic, reuses UI patterns from Wardrobe.
3. **The Photo Studio** (camera + stickers, local-only save — no cloud upload, no social network integration, per COPPA).
4. **The Park (Frisbee)** (swipe-physics mini-game).
5. **The Track (Racing)** (hardest — NPC AI, obstacle course) — build last, cut from v1 if timeline is tight.

Each mini-game: Claude writes full C# + wires it into the currency system; Pon tests and approves.

### Phase 5 — Economy, monetization, compliance
- Claude: implement currency (treats/coins), daily reward, neutral age-gate screen, parental gate (math-problem style) before any IAP/ad flow.
- Claude: draft privacy policy, COPPA checklist, Google Play Families / Apple Kids Category checklist (see `GDD.md` §7).
- Pon: pick the actual ad network / IAP provider account and create the developer accounts (this requires your payment details — I cannot do this step).

### Phase 6 — Polish, store listing, submission
- Claude: write App Store/Play Store descriptions, keyword research synthesis (from Gemini's market research — see §5), privacy policy final text.
- Pon: create the actual store listings (requires your Apple/Google developer accounts), upload build, submit for review.

### Phase 7 — Post-launch
- Claude: analyze any analytics/review data you paste in, propose balancing changes, write patch-note copy.
- Pon: monitor live metrics, greenlight updates.

---

## 3. What "done" looks like for v1 (scope control)

To keep this achievable with minimum intervention, v1 ships with:
- 1 hub room
- Full needs system (5 needs)
- 2 mini-games (Wardrobe + Clinic) — Park/Track/Photo Studio are v1.1+
- Toon-shaded placeholder-to-final 3D model (no shell fur in v1)
- Currency + daily reward only (no IAP in v1 — add once the core loop is validated)
- COPPA-compliant age gate, no ads, no data collection in v1

This is intentionally smaller than the full feature list in the PDF guide — that guide describes a mature, fully-featured product; v1 here is the smallest version that is actually a complete, shippable game, so you get something real in weeks rather than a sprawling project that never finishes. Phase ordering above lets you extend indefinitely after v1 ships.

---

## 4. Repo structure

```
bella-cute-puppy/
├── docs/
│   ├── PLAN.md          ← this file
│   ├── GDD.md
│   ├── ARCHITECTURE.md
│   ├── ART_BIBLE.md
│   └── AI_WORKFLOW.md
└── unity-project/
    └── Assets/
        ├── Scripts/
        │   ├── Core/        (PetStateMachine, IState)
        │   ├── States/      (IdleState, InteractionState, LocomotionState)
        │   ├── Needs/        (PetNeeds, NeedDecayConfig)
        │   ├── Save/         (SaveSystem, SaveData, offline-time calc)
        │   ├── MiniGames/    (one subfolder per mini-game, added per Phase 4)
        │   └── UI/
        ├── Art/Bella/        (3D model, textures — populated in Phase 3)
        ├── Animations/
        ├── Prefabs/
        ├── Shaders/
        └── Audio/
```

---

## 5. AI model allocation strategy (how to spend your limited Fable access)

You have: **unlimited-ish Sonnet/Opus** (your main daily driver) and **limited Fable time/chats**. Plan accordingly:

| Use Fable for (rare, high-leverage, one-shot) | Use Sonnet/Opus for (everything else, all day) |
|---|---|
| One big upfront "sanity check" pass over this whole plan + the GDD before you start Phase 1, to catch anything structurally wrong before code exists | All C# script writing |
| The single hardest architecture decision point you hit mid-project (e.g., if NavMesh pathing + mobile performance fights you) | All day-to-day debugging |
| A final pre-launch review pass: COPPA compliance, store-listing copy, overall quality bar | All documentation, GDD updates, mini-game design |
| — | All of Phase 4 (every mini-game) |
| — | Market/ASO copy synthesis once you've gathered raw research (via Gemini, per your note) |

Rule of thumb: **default to Sonnet for anything with a clear spec** (which, after this plan exists, is almost everything). **Escalate to Opus** only when a task is genuinely ambiguous or architecturally risky and getting it wrong would be expensive to unwind. **Save Fable** for the few moments where you want a second, independent opinion on something load-bearing — not for routine building.

Full model-to-task mapping (including ChatGPT and Gemini, since you're already using Gemini for research) is in [`AI_WORKFLOW.md`](./AI_WORKFLOW.md).

---

## 6. What I need from you to keep moving

Nothing right now — the next session can proceed straight to Phase 1 scripts. When we get there, the only real decisions that are genuinely yours (not technical, not something I can decide for you) are:
1. Target launch platforms: iOS only, Android only, or both?
2. Rough budget for outsourced art (0 — placeholder/AI-assisted only, vs. hiring a 3D artist)?
3. Timeline pressure — is this a side project with no deadline, or tied to something (a demo, a pitch, a release date)?

Answer those whenever convenient; everything else I can keep building without blocking on you.
