# Bella — The Cute Puppy

A 3D mobile virtual-pet game for **Android / Google Play** starring Bella, a small, very happy Golden Retriever puppy. Built in the spirit of *Boo: The World's Cutest Dog* with an entirely original character, art, and codebase — and **zero human spend**: all art is AI-generated (ChatGPT/Gemini → Meshy/Tripo3D).

## Start here

👉 **[docs/PLAN.md](docs/PLAN.md)** — the v2 master plan: phases, the AI-3D art pipeline, Google Play closed-testing requirements, and the growth strategy.

Other docs:
- [docs/GDD.md](docs/GDD.md) — game design: core loop, needs, mini-games (incl. Trick Training), compliance
- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) — FSM, offline-decay math, face-texture expressions, procedural animation
- [docs/ART_BIBLE.md](docs/ART_BIBLE.md) — Bella's design spec + step-by-step AI art pipeline with ready-to-paste prompts
- [docs/AI_WORKFLOW.md](docs/AI_WORKFLOW.md) — which AI tool/model handles which task

## Quick start (Pon)

1. Install **Unity Hub** → install **Unity 2022.3 LTS** (any 2022.3.x).
2. Unity Hub → Open → select the `unity-project/` folder. Wait for import.
3. Menu bar → **Bella → Build Test Scene** (builds the whole playable scene — no manual setup).
4. Press **Play**:
   - **Tap Bella** → she gets pet, hearts burst, tail wags faster
   - **FEED** → she walks to the bowl and eats
   - **SLEEP** → she walks to the bed and naps (energy refills)
   - Watch the need bars drain over time
5. Stop Play, close Unity, reopen later → needs have decayed while away (offline decay).
6. Menu **Bella → Reset Save Data** any time you want a fresh puppy.

Report back in plain language what felt right or wrong — no code needed.

## Project structure
```
docs/              — all design/planning docs
unity-project/     — the Unity project (open this folder in Unity Hub)
```

## Status
**Phase 1 code complete** — playable grey-box slice (needs system, FSM, save/offline decay, pet/feed/sleep loops, procedural animation, one-click scene builder). Waiting on Pon's first Unity run. Next: Phase 2 — generate the real Bella via the AI art pipeline (`docs/ART_BIBLE.md` §7).
