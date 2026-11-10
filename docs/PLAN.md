# Bella the Cute Puppy — Master Development Plan (v2)

**Project:** A 3D virtual-pet mobile game starring Bella, a small, very happy Golden Retriever puppy — in the spirit of *Boo: The World's Cutest Dog*, with an entirely original character, art, and codebase.

**Repo:** `medh-eng/Bella-cute-puppy`
**Owner:** Pon (non-technical founder/director; hands-on with specs, review, and decisions)
**Primary builder:** Claude (Sonnet for ~90% of production work, Opus escalation for hard calls, Fable for rare high-leverage reviews — see §6)

**v2 changes (2026-11-11):** Android/Google Play only (iOS cut). Zero human spend — all art via AI tools. 3D pipeline confirmed: **ChatGPT/Gemini concept image → Meshy or Tripo3D image-to-3D → auto-rig (quadruped) → preset + procedural animation → Unity**. Mixamo removed (humanoid-only, cannot rig a dog). Google Play closed-testing requirement added as its own phase. Blendshapes replaced by face-texture swapping (AI-generated models don't come with blendshapes). Growth strategy made explicit (§5).

Supporting docs:
- [`GDD.md`](./GDD.md) — game design: core loop, needs, mini-games, differentiators, compliance
- [`ARCHITECTURE.md`](./ARCHITECTURE.md) — FSM, offline-decay math, expression system, performance
- [`ART_BIBLE.md`](./ART_BIBLE.md) — Bella's look + the exact AI art pipeline with ready-to-paste prompts
- [`AI_WORKFLOW.md`](./AI_WORKFLOW.md) — which AI model/tool handles which task

---

## 1. Goal and operating principle

**Goal:** ship a polished, COPPA/Families-compliant virtual-pet game to **Google Play (Android only)**, built with minimum hands-on effort from Pon and **zero money spent on people** (total cash outlay: the one-time $25 Google Play developer fee, plus optional small AI-tool credits if free tiers run out).

**Operating principle — "minimum intervention":** Claude does design docs, all code, architecture, and project management. Pon's role:
1. Answer the few genuinely human decisions (accounts, approvals, taste calls on Bella's look).
2. Run Unity locally to compile/test (Claude writes everything, including an editor script that builds the test scene from one menu click — no manual editor wiring).
3. Generate art via AI tools (Claude supplies exact prompts; Pon pastes them into ChatGPT/Gemini/Meshy/Tripo and downloads results into the repo).
4. Playtest and report feel in plain language.
5. Recruit ~12 friendly testers for the mandatory Google Play closed test (§4, Phase 7).

---

## 2. The art pipeline (the v2 centerpiece)

No artist. The pipeline, verified November 2026:

| Step | Tool | Output | Cost |
|---|---|---|---|
| 1. Concept sheet | ChatGPT Images / Gemini | Bella turnaround (front/side/back) + expression sheet, per `ART_BIBLE.md` prompts | Free |
| 2. Image → 3D | **Meshy** (or Tripo3D as fallback) | Textured GLB/FBX of Bella | Free tier: Meshy 100 credits/month |
| 3. Auto-rig | Meshy rigging (supports **quadrupeds**) or Tripo auto-rig | Rigged FBX/GLB, Unity-ready | Same credits |
| 4. Animations | Meshy animation library (quadruped presets: walk, run, idle) + **procedural animation in code** (tail wag, bounce, squash-stretch, ear flop — Claude writes these) | Animation clips + runtime motion | Free |
| 5. Facial expressions | **Face-texture swapping**, not blendshapes: Claude's `FaceExpressionController` swaps AI-generated face textures (happy/sad/sleepy/begging) on the head material | Code + 4–6 PNG faces | Free |
| 6. Props/environment | Meshy text-to-3D for bowl/bed/toys; or Unity primitives + AI textures | GLB props | Free tier |
| 7. UI art | ChatGPT/Gemini image gen (icons, buttons, backgrounds — Claude supplies prompts) | PNGs | Free |
| 8. Audio | Freesound.org CC0 (barks, chimes) + jsfxr-style generated SFX | WAV/OGG | Free |

**Key insight:** AI-generated 3D models arrive without blendshapes and with limited animation presets. We compensate in code — procedural secondary motion and texture-swapped faces are cheap, fully controllable, and (bonus) read as *more* cartoon-cute than realistic facial rigging. This is how games like Animal Crossing do faces.

**Important Mixamo note:** Mixamo rigs humanoids only — it cannot rig Bella. Meshy and Tripo both auto-rig quadrupeds natively; use them end-to-end.

---

## 3. Phased roadmap

### Phase 0 — Foundation ✅
- [x] Repo scaffolded, docs written, starter scripts committed
- [x] v2 plan revision (this document)
- [ ] Pon: install Unity Hub + **Unity 2022.3 LTS**, open `unity-project/`, confirm no errors

### Phase 1 — Playable grey-box slice ✅ (code complete, needs Pon's first run)
Everything works with a placeholder puppy built from primitives — no art needed yet.
- [x] Full needs system (5 needs), live + offline decay (JsonUtility DateTime bug fixed — timestamps now stored as ticks)
- [x] FSM: Idle / Interaction / WalkTo / Sleep states
- [x] Save/load, UTC-based offline decay
- [x] Tap-to-pet with hearts particle burst; drag-food-to-Bella feeding
- [x] Need-bars HUD (icon-based, pre-reader friendly)
- [x] Procedural animation (idle bounce, tail wag, squash on pet) on the placeholder
- [x] Face expression controller (texture-swap architecture, tint-based on placeholder)
- [x] **One-click scene builder**: Unity menu `Bella → Build Test Scene` constructs the whole playable scene — Pon never wires anything manually
- [ ] Pon: open Unity → `Bella → Build Test Scene` → press Play → feed/pet Bella → close and reopen to confirm offline decay. Report feel.

### Phase 2 — Real Bella (AI art pass 1)
- Pon: run `ART_BIBLE.md` §7 prompts → concept sheet → pick favorite → Meshy image-to-3D → auto-rig → download GLB+FBX into `Assets/Art/Bella/`.
- Claude: import setup, animator controller wiring preset clips + procedural layer, face-texture system on the real model.
- Pon: taste check — "does she feel like Bella?"

### Phase 3 — The room + daily rhythm
- Environment (one cozy room; AI-generated props), day/night tint matching real local clock, sleep loop, wake-up greeting (Bella's excitement scales with hours away — capped, so never guilt).
- Illness events + thought-bubble emotion system (icon bubbles: bone = hungry, Zzz = sleepy — pre-reader UI).

### Phase 4 — Mini-games (easiest → hardest)
1. **The Wardrobe** (dress-up; AI-generated accessory textures)
2. **Trick Training** (NEW — draw a shape on screen, Bella learns the trick; gesture recognition is simple code, feels magical, and is this game's differentiator)
3. **The Clinic** (tool-matching care puzzle)
4. **Bubble Bath** (scrub mini-sequence; upgraded from a chore to a game)
5. v1.1+: Photo Studio, Park/Frisbee, Track/Racing

### Phase 5 — Economy + retention systems
- Treats currency, daily gift, sticker memory-book (collection = long-term retention, no money involved), Bella grows slightly over real weeks (puppy → young dog toggle).
- No IAP, no ads in v1 (deliberate — see GDD §7).

### Phase 6 — Compliance + store prep
- Neutral age screen, parental gate, Families Policy declaration, Data Safety form answers (all "no collection"), privacy policy text, store listing copy + ASO keywords, screenshots/feature graphic (AI-generated).
- Pon: create Google Play developer account ($25, one-time) — do this EARLY in Phase 5, account verification can take days.

### Phase 7 — Closed testing (mandatory, plan for ~3–4 weeks)
Google requires new personal developer accounts to run a closed test with **12 testers opted in for 14 continuous days** before production access — and since 2026 Google checks that testers actually *use* the app, not just install it.
- Pon: recruit 12–15 people (clinic staff, family, friends — overshoot so dropouts don't reset the clock), collect their Gmail addresses.
- Claude: tester instructions message (what to tap, how often), feedback form questions.
- Use internal testing track (up to 100 testers, no review) for fast iteration *in parallel*.

### Phase 8 — Launch + post-launch
- Apply for production access → production review → launch.
- Then: review Play Console stats, apply for the **Teacher Approved** program, iterate with v1.1 mini-games.

---

## 4. What "done" looks like for v1 (scope control)

- 1 cozy room, day/night cycle
- Full 5-need system with offline decay
- Real AI-generated rigged 3D Bella with face expressions + procedural cuteness
- 3 mini-games: Wardrobe, Trick Training, Clinic (+ Bubble Bath care loop)
- Treats currency + daily gift + sticker book
- COPPA-clean: no ads, no IAP, no data collection, parental gate on external links
- Passed 14-day closed test, live on Google Play

---

## 5. Growth strategy (the honest version of "viral")

Child-directed apps legally cannot use viral machinery (social feeds, friend invites, share-to-unlock). What actually grows kids' games:

1. **Retention → ranking.** Play Store rewards D1/D7 retention. Our levers: offline decay pulls kids back, daily gift, sticker book, Bella's growth, wake-up greeting. All positive-framing — no punishment mechanics (Families policy also restricts manipulative design).
2. **ASO.** Parents type the searches ("puppy games for kids", "pet care game free no ads"). "**No ads**" is itself a top parent search term and our genuine differentiator — most competitors are ad-stuffed. Put it in the title/short description.
3. **Teacher Approved program.** Google's curated Kids tab; most devs never apply. A compliant, ad-free, gentle pet game is exactly what it exists to feature. Apply immediately post-launch.
4. **Parent-shareable moments.** Photo of dressed-up Bella exported via OS share sheet (behind parental gate) → WhatsApp — the one legal "share loop", and WhatsApp is the distribution channel in India.
5. **Ratings prompt** aimed at the parent (behind the gate), only after a happy session.

---

## 6. AI model allocation

Unchanged in spirit: **Sonnet** for all spec'd production work (scripts, docs, debugging), **Opus** for ambiguous/architectural calls, **Fable** reserved for: this sanity-check pass (done), one mid-project unblock if something resists Opus, one pre-launch compliance/quality review. Full mapping incl. ChatGPT/Gemini/Meshy/Tripo roles: `AI_WORKFLOW.md`.

---

## 7. Decisions log

| Decision | Choice | Date |
|---|---|---|
| Platform | Android / Google Play only | 2026-11-11 |
| Engine | Unity 2022.3 LTS (URP) | 2026-11-11 (confirmed over HTML5 option) |
| Art | 3D via AI (Meshy/Tripo), zero human spend | 2026-11-11 |
| Rigging | Meshy/Tripo quadruped auto-rig (NOT Mixamo — humanoid only) | 2026-11-11 |
| Faces | Texture-swap, not blendshapes | 2026-11-11 |
| v1 monetization | None (no ads, no IAP) | carried over |

Open decisions for Pon (answer anytime):
1. Bella's signature trait — collar color (soft red vs teal) or a heart-shaped ear patch? (Locks in before Phase 2 art generation.)
2. Google Play account: personal ($25, needs the 12-tester test) — or do you have a registered business entity to use an organization account (exempt from the tester requirement)? The clinic's registration might qualify; worth checking.
