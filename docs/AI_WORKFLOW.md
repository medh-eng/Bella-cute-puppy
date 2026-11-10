# AI Workflow — Which Model/Tool for What (v2)

Goal: maximize what gets built with **minimum intervention from Pon** and **zero human spend**. Fable access is limited; Sonnet/Opus are the daily workhorses.

## 1. Claude (Sonnet / Opus) — the primary builder, used daily
**Default to Sonnet** for everything with a spec (after this plan, nearly everything):
- All C# scripts (FSM, needs, save, mini-games, UI, editor bootstrap scripts)
- Doc updates, debugging (paste the Unity error, get a fix), store copy, privacy policy drafts
- Breaking vague asks into implementation steps

**Escalate to Opus** for genuinely ambiguous/high-stakes calls: save-schema changes after launch, a performance wall, end-of-phase reviews.

**Session-start pattern:** "Continue Bella — repo medh-eng/Bella-cute-puppy, read docs/PLAN.md, do the next unchecked item in the current phase."

## 2. Claude Fable — reserved, limited
1. ~~Project-start sanity check~~ ✅ done 2026-11-11 (produced this v2 plan)
2. One mid-project unblock if a problem survives an Opus attempt
3. Pre-launch review: compliance checklist, store listing, overall quality bar

## 3. ChatGPT / Gemini (images) — concept + 2D production art
- Bella concept turnaround + expression sheet (exact prompts: `ART_BIBLE.md` §7)
- UI icons, buttons, backgrounds, store screenshots/feature graphic
- Gemini additionally: market/ASO research, pulling CURRENT Google Play Families policy text before submission (never trust any AI's memory of policy — including this repo's summaries; verify against the live policy page).

## 4. Meshy (primary) / Tripo3D (fallback) — the 3D factory
- Image→3D from the concept sheet, retopo if dense, **quadruped auto-rig**, preset animation clips (idle/walk/run), text-to-3D props.
- Free tiers: Meshy ~100 credits/month; Tripo has a free tier too. If a month's credits run out: wait, use the other tool, or buy a small credit pack (the only optional spend in the project).
- **Do NOT use Mixamo** — it rigs humanoids only; it cannot rig a quadruped like Bella.

## 5. Audio — Freesound.org (CC0 filter) for barks/chimes; pitch-shift in Unity. Keep a `CREDITS.md` listing every clip's source/license even for CC0 (clean paper trail for Play review).

## 6. The loop, step by step
1. Pon: "continue Bella" in a Sonnet session.
2. Claude writes code/docs straight into the repo and commits.
3. Pon: open Unity → menu **Bella → Build Test Scene** (or just press Play on the existing scene) → test → report feel in plain words.
4. Art phases: Pon pastes the prompts from `ART_BIBLE.md` into ChatGPT/Gemini/Meshy, downloads results into `Assets/Art/...`, pushes, and tells Claude — Claude wires them up.
5. Decisions only at the points flagged in `PLAN.md` §7.
