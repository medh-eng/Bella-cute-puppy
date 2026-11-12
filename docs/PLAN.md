# Bella the Cute Puppy — Master Development Plan (v3)

**Project:** A 3D virtual-pet mobile game starring Bella, a small, very happy Golden Retriever puppy — in the spirit of *Boo: The World's Cutest Dog*, with an entirely original character, art, and codebase.

**Repo:** `medh-eng/Bella-cute-puppy`
**Owner:** Pon (non-technical founder/director; hands-on with specs, review, and decisions)
**Primary builder:** Claude (Sonnet for ~90% of production work, Opus escalation for hard calls, Fable for rare high-leverage reviews — see §6)

**v3 changes (2026-11-11, later the same day):** the whole v1 game is now built (see §3). Unity 6.3 LTS replaces 2022.3. Rigging, animation, icons and audio were produced by scripts in `tools/pipeline/` rather than by Meshy presets and Freesound, so §2's table is kept as the fallback route for *future* characters and props.

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
2. Run Unity locally to compile/test — one PC session, scripted end to end in `UNITY_DAY.md` (the project sets itself up on first open; no manual editor wiring).
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

**Status on 11 Nov 2026: Phases 0–6 are complete in the repository, apart from the items that need
a computer with Unity or a Google account. Those are collected in [`UNITY_DAY.md`](./UNITY_DAY.md).**
Legend: ✅ done and in the repo · 🖥 needs the Unity PC session · 👤 needs Pon (account / people)

### Phase 0 — Foundation ✅
- [x] Repo, docs, plan
- [ ] 🖥 Install Unity Hub + **Unity 6.3 LTS** with Android Build Support, open `unity-project/`

### Phase 1 — Playable slice ✅
- [x] Five needs with live + offline decay, kindness cap and floor
- [x] FSM (Idle / Interaction / WalkTo / Sleep), local JSON save
- [x] Petting, feeding, sleeping, fetch
- [x] Whole game assembled in code by `GameRoot` — no manual scene wiring
- [ ] 🖥 First Play in Unity (checklist in `UNITY_DAY.md` step 4)

### Phase 2 — Real Bella ✅
- [x] AI mesh rigged by script in Blender (no Meshy rig needed), 15 baked animations in one take
- [x] Six face textures + blink; procedural tail, ears, look-at, squash-and-stretch
- [x] Primitive placeholder puppy kept as an automatic fallback if the model fails to import
- [ ] 🖥 Taste check — "does she feel like Bella?"

### Phase 3 — Room + daily rhythm ✅
- [x] Room with bowl, bed, ball; day/night lighting from the real clock; music for day and night
- [x] Greeting scaled to time away; wandering; sniffles event; thought bubbles

### Phase 4 — Mini-games ✅
- [x] Wardrobe (12 outfits, 4 slots) · Trick Training (6 gestures) · Clinic · Bubble Bath
- [ ] v1.1+: Photo Studio, Park/Frisbee, Track/Racing

### Phase 5 — Economy + retention ✅
- [x] Treats, daily present, days-together counter, 18-sticker book, growth over 15 days
- [x] Economy paced: 30 treats/day from repeatable play, unlimited from needed care → wardrobe ≈ 1–2 weeks
- [x] No IAP, no ads

### Phase 6 — Compliance + store prep ✅ (documents) / 🖥👤 (accounts and screenshots)
- [x] Parental gate, parents' area, in-game privacy summary
- [x] Privacy policy page (`docs/privacy.html`), store listing, every Play Console answer
      (`docs/store/`), feature graphic, 512 px icon, credits/provenance (`CREDITS.md`)
- [x] Age screen deliberately dropped — child-only audience, see `store/PLAY_CONSOLE_ANSWERS.md` §5
- [x] Safe-area handling for Android's edge-to-edge display
- [ ] 👤 Google Play developer account (US$25) — start early, verification takes days
- [ ] 👤 Turn on GitHub Pages; put a contact email in `docs/privacy.html`
- [ ] 🖥 Six store screenshots (menu **Bella → Take Screenshot**)
- [ ] 🖥 Upload keystore + signed `.aab`

### Phase 7 — Closed testing (≈ 3–4 weeks) 👤
- [x] Tester invitation, reminders, feedback form, production-access answers (`store/CLOSED_TEST_KIT.md`)
- [ ] 👤 Recruit 15–20 testers, run 14 days, apply for production access

### Phase 8 — Launch + post-launch
- [ ] Production review → launch → review Play Console stats → v1.1 mini-games

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
2. **ASO.** Parents type the searches ("puppy games for kids", "pet care game free no ads"). "**No ads**" is itself a top parent search term and our genuine differentiator — most competitors are ad-stuffed. Put it in the short and full description (not the app name — Play's metadata policy forbids it there).
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
| Engine | Unity (3D), confirmed over HTML5 option | 2026-11-11 |
| Unity version | **Unity 6.3 LTS**, built-in render pipeline. Replaces 2022.3/URP: Google Play now requires target API 36 + 16 KB pages, which 6.3 handles out of the box; the game's flat cartoon look needs no URP | 2026-11-11 |
| Art | 3D via AI (Meshy/Tripo), zero human spend | 2026-11-11 |
| Rigging | Done by script in Blender (`tools/pipeline/`) — no rigging service needed | 2026-11-11 |
| Audio | All sounds and music synthesised by script — no third-party clips to license | 2026-11-11 |
| Age screen | Dropped — only needed for mixed-audience apps | 2026-11-11 |
| Store title | "No ads" kept out of the app name (Play metadata policy); it is in the descriptions | 2026-11-11 |
| Faces | Texture-swap, not blendshapes | 2026-11-11 |
| v1 monetization | None (no ads, no IAP) | carried over |

Open decisions for Pon (answer anytime):
1. ~~Bella's signature trait~~ — settled by the generated art: teal collar with a gold paw tag.
3. Android package id — currently `com.bellacutepuppy.game`. It can never be changed after the first upload.
4. Public contact email for the store page and privacy policy.
2. Google Play account: personal ($25, needs the 12-tester test) — or do you have a registered business entity to use an organization account (exempt from the tester requirement)? The clinic's registration might qualify; worth checking.
