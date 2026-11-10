# Bella the Cute Puppy — Game Design Document (v2)

## 1. Concept
A 3D mobile virtual-pet simulation for **Android** starring **Bella**, a small, very happy Golden Retriever puppy. The player cares for Bella (feeding, bathing, dressing, playing, teaching tricks) and she responds with warm, exaggerated, emotionally legible reactions. Genre precedent: *Boo — The World's Cutest Dog* and *My Talking Angela/Tom* — Bella is an original character and must not reuse their name, likeness, color design, or assets.

**Positioning in one line (also the ASO hook):** *the gentle, ad-free puppy game parents can trust.*

## 2. Target audience and psychology
- Primary: children aged 4–8 (pre-readers → UI leans on icons, sound, color — not text).
- Secondary: casual gamers and pet lovers of any age.
- **Kindchenschema** ("baby schema"): oversized head, large low-set eyes, small nose, clumsy proportions — these elicit caregiving responses and are core to Bella's model (see `ART_BIBLE.md` §3).
- Warm saturated palette; soft pastels + bright primaries for environments.
- **High error tolerance. No fail states.** Poor mini-game performance still gets encouraging feedback — Bella is always happy you tried.
- Immediate multisensory reward on every interaction: bark/chime + animation + particle burst (hearts/stars).
- **Emotional legibility system (NEW):** Bella shows *thought bubbles* with icons — bone = hungry, soap bubble = dirty, Zzz = sleepy, heart = loves you. A 4-year-old reads her state instantly without text or even looking at the need bars. The bars become the "parent/older kid" layer; the bubbles are the primary UI.

## 3. Core gameplay loop
Open app → Bella greets you (excitement scaled to time away, capped — delight, never guilt) → read her thought bubble → attend to needs (feed/bathe/sleep/play) → earn treats from care + mini-games → spend treats on outfits/stickers/toys → needs decay over real time → return.

**Long loop (NEW, retention):** Bella *grows* — over real calendar weeks she gets slightly bigger and unlocks new tricks/poses (puppy → young dog, purely visual + celebratory, never a paywall). Sticker memory-book fills with moments ("first bath", "first trick", "7-day friend streak"). Collection without money.

## 4. Needs system
Five variables on a 0–100 scale, decaying over real time **including while the app is closed** (UTC-timestamp math — `ARCHITECTURE.md` §3). Decay also ticks live during play.

| Need | Depletes via | Restored via |
|---|---|---|
| Satiety | Time | Drag kibble/treats/water to Bella's mouth |
| Hygiene | Time (faster after outdoor play) | Bubble-bath mini-sequence (scrub + rinse) |
| Energy | Playtime/mini-games | Sleep: tuck Bella into bed, lullaby, lights dim |
| Health | Occasional random "sniffles" events (gentle, never scary) | The Clinic mini-game (thermometer, bandage, medicine) |
| Happiness | Neglect of the above | Petting, tricks, mini-games, dressing up |

**Kindness cap (NEW, child-psych guardrail):** needs never visually hit a distressing state. At 0 hunger Bella looks wistful and shows a bone bubble — she never starves, shakes, or cries. Neglect = "she misses you", never "you hurt her". This matters both ethically and for Families-policy review optics.

## 5. Mini-games

| Module | Mechanic | Why it's fun | v1? |
|---|---|---|---|
| **The Wardrobe** | Drag-and-drop outfits/accessories | Self-expression, collection | ✅ v1 |
| **Trick Training** (NEW — differentiator) | Draw a shape on screen (circle → roll over, line down → sit, zigzag → dance); Bella "learns" it; later she performs on request | Feels like genuine teaching/communication — the core fantasy of having a puppy. Simple gesture recognition, huge perceived magic | ✅ v1 |
| **The Clinic** | Match the right tool to the ailment | Empathy, problem-solving | ✅ v1 |
| **Bubble Bath** | Scrub until sparkly, rinse, dry with towel | Tactile satisfaction (upgraded from chore to mini-game) | ✅ v1 (as the Hygiene loop) |
| The Photo Studio | Pose + stickers, OS share-sheet export behind parental gate | Creativity + the only legal share loop | v1.1 |
| The Park (Frisbee) | Physics swipe | Skill mastery | v1.1 |
| The Track (Racing) | Timed obstacles | Light thrill | v1.2 |

## 6. Art direction (summary — full spec + AI pipeline in `ART_BIBLE.md`)
- Stylized cartoon 3D, AI-generated via image→3D (Meshy/Tripo), quadruped auto-rig.
- Facial expressions via **face-texture swapping** (happy/sad/sleepy/begging/excited faces as textures on the head material) — not blendshapes. Reads as classic cartoon, performs perfectly on low-end phones.
- Secondary motion is **procedural in code**: tail wag speed = happiness, idle breathing bounce, squash-and-stretch on petting, ear physics. This is where the "alive" feeling comes from, and it's free.
- Bella must be distinguishable from Boo at a glance: Golden Retriever (not Pomeranian), golden/cream gradient (not white), floppy ears, leggier puppy proportions, one signature trait (collar color or ear patch — Pon to pick).

## 7. Compliance and monetization (child-directed rules)
Squarely child-directed under COPPA + Google Play Families. Non-negotiable:
- **Zero data collection in v1.** No PII, no location, no device identifiers, no analytics SDK (or child-safe-configured only), no crash reporter that grabs IDs. Data Safety form answers: "No data collected or shared."
- **Neutral age screen** on first launch (birth-year dropdown, not "are you over 13?").
- **Parental gate** (randomized math problem) before: external links, share-sheet export, ratings prompt. v1 has no IAP/ads so the gate surface is tiny.
- **No ads, no IAP in v1** — this is both compliance simplification AND the marketing hook (§3 of `PLAN.md` growth strategy). Monetization decision deferred until retention is proven; if ever added: contextual-only ads via Families-certified SDKs, or a single "supporter pack" IAP behind the gate.
- Photo features (v1.1): local storage only; share only via OS share sheet behind the gate.
- Google Play specifics: Families Policy declaration, target-audience declaration ("ages 5 & under" + "6–8"), store listing must not misrepresent, closed test required first (`PLAN.md` Phase 7).

## 8. Audio direction
- Bella vocals: friendly puppy barks/whines — CC0 from Freesound, pitch-shifted in Unity to make them "hers" (free + legally clean).
- UI: soft chimes, pops. Music: one gentle loop for day, one lullaby for night. CC0 sources or simple generated compositions.
- Every need-restoring action has a unique, satisfying sound signature (the "slot machine principle" used benevolently).

## 9. Success metrics (once live)
- D1/D7/D30 retention (casual pet-sim directional benchmarks: D1 ~35–40%, D7 ~10–15% — verify current numbers at launch)
- Sessions/day and session length (healthy pet-game pattern: 3–5 short sessions/day)
- Mini-game completion + repeat rate (bounce = redesign)
- Treat economy health (always a little short of the next outfit, never broke, never capped)
- Play Store rating ≥ 4.4 (gate: ratings prompt only after happy sessions, parent-directed)
- Teacher Approved acceptance
