# AI Workflow — Which Model/Tool for What

Goal: maximize what gets built with **minimum intervention from Pon**, while respecting that Fable access is limited/time-boxed and Sonnet/Opus are the main workhorses.

## 1. Claude (Sonnet / Opus) — the primary builder, used daily

**Default to Sonnet** for almost everything — it's the right tool once a spec exists (and after this plan, nearly everything has one):
- Writing all C# scripts (FSM states, needs system, save/load, mini-game logic, UI wiring)
- Updating/extending the docs in this repo as the project evolves
- Debugging — paste an error or describe a bug, get a fix
- Drafting store-listing copy, privacy policy text, patch notes
- Breaking a vague ask ("make the dress-up screen") into an actual implementation

**Escalate to Opus** only for genuinely ambiguous or high-stakes calls:
- A architecture decision with real trade-offs you can't easily undo later (e.g., choosing a save-data schema before any save data exists, choosing between local-only vs. cloud-save implications)
- A gnarly performance problem that resists a first fix (e.g., frame-rate issues after adding shell-fur)
- Reviewing a finished phase end-to-end for correctness before you consider it "done"

**Prompting pattern that works well** (from the research guide, still accurate practice):
> "Act as a Lead Unity Software Engineer. Draft a C# script for '[StateName]' within Bella's Finite State Machine. It must interface with the existing `PetNeeds` class, decay the [variable] float using the saved-timestamp method in `ARCHITECTURE.md` §3, and trigger a transition to '[OtherState]' when [condition]. Follow SOLID principles and include XML doc comments."

## 2. Claude Fable — reserved, limited-use

Use Fable only for a handful of high-leverage moments, not routine work (see `PLAN.md` §5 for the full rationale):
1. **Project-start sanity check**: one session reviewing this whole `docs/` folder end-to-end before Phase 1 begins, looking specifically for structural mistakes that would be expensive to discover mid-build.
2. **The single hardest unresolved problem** you hit during the build (if any) — e.g. a performance wall that survives a first Sonnet/Opus attempt.
3. **Pre-launch review**: one pass over compliance (COPPA/Families checklist), store copy, and overall build quality before submission.

Everything else: don't spend Fable time on it — Sonnet handles it fine and you have far more of it.

## 3. ChatGPT — ideation and narrative support (optional, you may already have access)
Best used for: rapid brainstorming of mini-game concepts, alternate names/taglines, narrative flavor text, quick creative variations. Not the primary build tool here — Claude already owns the GDD and code, so only reach for ChatGPT if you want a second creative opinion on something like mini-game naming or marketing copy variations.

## 4. Gemini — market research and policy lookups (you're already using this)
Best used for: competitive research (what similar games are doing), ASO/keyword research, and pulling the *current* text of Google Play Families / Apple Kids Category policy (policy pages change — always verify against the live page before submission, don't rely on any AI's memory of policy text, including this document's compliance summary). Keep feeding Gemini's research output back to Claude in chat/this repo so it becomes part of the permanent plan rather than living only in a separate conversation.

## 5. Image/art generation tools (ChatGPT Images, Midjourney, etc.)
Useful for early mood boards and concept exploration (what should Bella's palette look like, environment concept art) but **not a substitute for a rigged, animation-ready 3D model** — AI image generators don't produce game-ready assets (rigs, blendshapes, clean topology). Use them for concept art only; commission or build the actual production model per `ART_BIBLE.md` §7–8.

## 6. The actual minimum-intervention workflow, step by step
1. Start a Claude session, point it at this repo (already done going forward — just say "continue Bella" and reference this plan).
2. Ask for the next unchecked item in `PLAN.md`'s phase list.
3. Claude writes the code/doc directly into the repo and commits.
4. You: open Unity, press Play, test, report back in plain language what felt right/wrong — no code required from you.
5. Repeat per phase. Only stop to make an actual decision at the points flagged in `PLAN.md` §6.
