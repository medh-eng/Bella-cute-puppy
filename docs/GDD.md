# Bella the Cute Puppy — Game Design Document

## 1. Concept
A mobile virtual-pet simulation starring **Bella**, a small, very happy Golden Retriever puppy. The player cares for Bella (feeding, bathing, dressing, playing) and she responds with warm, exaggerated, emotionally legible reactions. Genre precedent: *Boo — The World's Cutest Dog* (Coco Play/TabTale) and *My Talking Angela/Tom*-style nurture games — Bella is an original character and must not reuse their name, likeness, color design, or assets.

## 2. Target audience and psychology
- Primary: children aged 4–8 (pre-readers/early readers → UI must lean on icons, sound, and color, not text).
- Secondary: casual gamers and pet lovers of any age.
- Design must trigger **kindchenschema** ("baby schema"): oversized head relative to body, large low-set eyes, small nose, slightly clumsy proportions — these traits elicit caregiving responses and are central to Bella's model, not optional polish.
- Warm, saturated color palette (golden/cream gradients for Bella; soft pastels + bright primaries for environments) to keep the mood cheerful and non-threatening.
- **High error tolerance.** No fail states. A mini-game performed poorly must still give encouraging feedback, never a "you lost" message — frame everything as the pet being happy you tried.
- Immediate multisensory positive reinforcement on every successful interaction: a bark, a tail-wag animation, a particle burst (hearts/stars).

## 3. Core gameplay loop
Wake Bella → attend to her needs (feed/bathe/dress/play) → earn currency from mini-games and care → spend currency on unlocks (outfits, toys, room decor) → repeat, with needs decaying over real time so the player returns.

## 4. Needs system
Five tracked variables, each decaying continuously over time, including while the app is closed:

| Need | Depletes via | Restored via |
|---|---|---|
| Satiety (hunger) | Time | Feeding: drag kibble/treats/water to Bella's mouth |
| Hygiene | Time (faster after outdoor mini-games) | Bubble-bath mini-sequence: touch-based scrub/rinse until visually clean |
| Energy | Playtime/mini-games | Sleep state: play a lullaby, tuck Bella into bed |
| Health | Random "illness" events (unpredictable, not a steady decay) | "Vet care": simple diagnostic/treatment tools (thermometer, bandage) via The Clinic mini-game |
| Happiness | Neglect of the above | Petting (touchscreen), mini-game participation, affectionate interaction |

### Offline/background decay math
Needs must keep decaying while the app is closed, calculated on resume — see `ARCHITECTURE.md` for the exact formula and implementation (UTC timestamps, not local clock, to prevent manipulation via clock changes).

## 5. Mini-games (secondary loops)

| Module | Core mechanic | Player objective | Why it's fun |
|---|---|---|---|
| **The Park** (Frisbee) | Physics-based swipe | Time swipes to let Bella catch a flying disc | Skill mastery, hand-eye coordination |
| **The Track** (Racing) | Timed obstacle navigation | Guide Bella past an NPC-puppy race course | Light competitive thrill |
| **The Wardrobe** | Drag-and-drop | Spend currency on outfits/collars/accessories | Self-expression, collection |
| **The Photo Studio** | Camera sim + filters/stickers | Pose Bella, apply filters, "share" to a simulated in-app feed (never a real network — see §7) | Creativity |
| **The Clinic** | Puzzle/tool-matching | Identify Bella's ailment, pick the right tool | Empathy, problem-solving |

v1 ships with **Wardrobe + Clinic** only (cheapest to build, no physics, fastest path to a complete shippable loop). Park/Track/Photo Studio are v1.1+ (see `PLAN.md` §3).

## 6. Art direction (summary — full spec in `ART_BIBLE.md`)
- Stylized, not photorealistic. Toon-shaded (cel-shaded) 3D as the default — cheaper to render on mobile, and visually distinct from Boo's shell-fur-fuzzy look, which also sidesteps any visual-similarity concern.
- Facial expressiveness via blendshapes (morph targets) — smile, droopy-sad, open-mouth-pant — not complex face rigging.
- Bella must be visually distinguishable from Boo at a glance: different color treatment, different silhouette/ear shape emphasis, a distinct signature prop or marking (decide one signature visual trait in `ART_BIBLE.md`, e.g. a specific collar color or one floppy ear).

## 7. Compliance and monetization (child-directed app rules)

This app is squarely "child-directed" under COPPA and Google Play Families / Apple Kids Category rules. Non-negotiable requirements:
- **Data minimization**: no PII, no geolocation, no persistent device identifiers (IMEI/advertising ID) without verifiable parental consent. Disable default analytics/crash-SDK device-ID collection.
- **Neutral age screen** on first launch: a neutral birth-year dropdown, not a "are you over 13?" yes/no (which hints at the "correct" answer). Under-13 input must permanently disable data collection and behavioral ads for that session.
- **Photo Studio** images: local-device storage only, or shared only via OS-level share sheet with a parental gate — never auto-uploaded to any external server or real social network.
- **Parental gate** required before any action that: spends real money, collects data, or opens an external link. Implementation: a randomized math problem (e.g. "what is 14 + 27?") or a multi-finger gesture too complex for a young child.
- **Ads (if used post-v1)**: contextual only (never behavioral/tracking-based), served only via Google Play Families-certified ad SDKs, content filtered for age-appropriateness.
- v1 ships with **no ads, no IAP** — just currency earned through play — specifically to delay all of this compliance surface area until the core loop is proven fun. Add monetization in a later phase once this is validated, with Claude drafting the actual privacy policy and compliance checklist at that point.

## 8. Market/production context (from your Gemini research + the PDF guide)
- Engine: Unity 3D (URP) is industry-standard for this genre and is what this project uses.
- If you want local talent/infrastructure: Hyderabad's AVGC (Animation, VFX, Gaming, Comics) ecosystem — IMAGE Tower (studio space), STPI IMAGE CoE (startup accelerator, seed funding up to ₹5 lakh), TVAGA (industry association, B2B matchmaking), and IGDC (India's largest game dev conference, networking + freelance talent + publisher connects) — are real, India-specific resources worth exploring once you need an artist or want outside funding/visibility. Not required for Phase 1–2.

## 9. Success metrics (once live)
- D1/D7/D30 retention (industry benchmark for casual pet-sim: D1 ~35-40%+, D7 ~10-15%+ — treat as directional, verify current benchmarks closer to launch)
- Session length and sessions/day
- Mini-game completion rate (are players bouncing off a specific one?)
- Currency economy health (are players capped out on currency with nothing to spend it on, or always broke?)
