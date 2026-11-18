# Bella — new asset list and prompts (V2)

**Written 18 Nov 2026** after checking the repo (`main`, commit `2f4f9c3`), every folder in `Resources/`, `art-inbox/` and the old `docs/ASSET_PROMPTS.md`. This replaces the *to-do* part of the old file; the old file stays as the reference for packs A–D, which are now done.

## 1. What is already finished (do not generate again)

| Pack | What | Status |
|---|---|---|
| A-01…A-06 | Logo, title screen, app icon, loading pictures, studio mark, 4 story cards | In repo |
| B-01…B-08 | Buttons, popup panel, ribbon/tags/bubbles, meters, shop card, gift, day tiles, tutorial hands | In repo (B-03…B-08 in the 3D-toy look since 18 Nov) |
| C-01…C-10 | 90 icons (plus 9 corrections on 18 Nov) | In repo, all names present |
| D-01, D-02 | 18 Bella portraits | In repo (18 Nov, second round fits the popup badge) |
| D-03, D-04, D-05 | 27 stickers | In repo (D-04 second round 18 Nov) |
| Music | 14 tracks: title, morning, day, evening, night, play, bath, shop, garden, stage, fest lights/colours/winter/party | In repo |
| Voice and sounds | barks, happy barks, yip_1, 4 sad whines, hungry whimpers, yawn, munch, snore, sniff, lap, pant + 20 synthesised effects | In repo; gaps listed in §3 |

Not in the repo and not to be added: the 18 portraits and 27 stickers in `Bella_Batch6…/Resources/` (wrong pictures, deleted from your folder on 18 Nov).

## 2. How the remaining work is split

| Tier | Why | Count |
|---|---|---|
| **1 — Needed to launch** | Store page picture, screenshot background, and the audio the game already tries to play | 2 pictures + 32 sounds |
| **2 — Makes it feel like a big game** | Sparkle effects, room themes, real furniture models, growth card, thought bubbles | 3 + 17 + 11 + 2 |
| **3 — More game** | New places, more models, festivals, website art | 12 + 55 + 9 + 3 |
| **4 — Optional** | New Bella model (do not do before launch) | 1 |

**Important:** nothing in Tiers 2–3 is wired into the game yet — the code for room themes, places, props, effects and seasons has not been written (the folders `Surfaces/`, `Places/`, `Props/`, `Fx/`, `Seasons/` do not exist). Generate one tier, hand it over with the Part 3 messages in the old file, and Sonnet wires it. The game works without any of it.

## 3. Rules that changed after the last two rounds

1. **Bella must look friendly in every picture.** The old Batch6 portraits showed an angry Bella. Every prompt now carries the extra lines below.
2. **Check names against pictures before you save.** Scrambled sheets cost two extra rounds.
3. **Stickers and portraits need empty margin** (at least 32 px of 512 for stickers; portraits must sit well inside the round badge).
4. Attach the Bella reference first in every chat (old file §0.3), one pack per chat.
5. Gemini pictures with a visible corner mark: regenerate in ChatGPT.
6. Nothing is published about any game before it is live on Google Play, so the website pictures (J-03, J-04, J-05) can wait until launch week.

**Add these lines to the end of every sheet prompt (lessons from the 17–18 Nov rounds):**

```text
Check your own work before you answer: count the pictures (there must be exactly the number I listed), and confirm picture number 1 is the first item in my list, number 2 the second, and so on in reading order — the picture in each cell must match its list item exactly, not a neighbouring one. Every face must look friendly: soft eyebrows, never angry, never frowning hard, never scary. Keep a clear empty margin of at least 6% of the picture's width inside each cell so nothing touches the cell edge. Nothing may be cut off.
```

**Always do this on your side too:** after the sheet appears, hold your list next to it and check each cell against its name *before* downloading. If even one is wrong, reply "picture 4 shows X but should show Y — redo the whole sheet in the same order" — do not accept a sheet with a scrambled order, because the file names will then lie.

---

# TIER 1 — Needed to launch

## 1A. Store pictures

#### J-01 — Feature graphic

*Tool:* Gemini · *Save as:* `J-01.png` · *Becomes:* `docs/store/feature_graphic_1024x500.png` · *Final size:* 1024×500  
*Note:* Sonnet adds the logo from A-01 on the left.

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint a wide banner (exactly 1024 by 500 proportions, about 2:1): on the right, Bella leaping joyfully towards the viewer with a rose-red ball, ears flying. On the left third, calm empty space with soft clouds for a logo to be added later. Background: a sunny garden with soft hills, a rainbow, floating bubbles and hearts. Bright, joyful, high contrast so it reads when small. No text.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### J-02 — Screenshot frame background

*Tool:* Gemini · *Save as:* `J-02.png` · *Becomes:* `docs/store/frame_bg.png` · *Final size:* 1080×1920  
*Note:* Make 2 more by asking "same again in gold-to-orange" and "lilac-to-sky-blue" → `J-02b.png`, `J-02c.png`.

```text
Create a tall portrait (9:16) background for app-store screenshots: a soft diagonal gradient from teal at the top to pink at the bottom, with a faint pattern of white paw prints and tiny stars, and a slightly darker rounded band across the top 18% where a caption will go. Nothing else — clean and simple. No text.
```

*Screenshots themselves must be real captures from your phone (Unity day) — they are not generated.*

## 1B. Audio the game already looks for

Search at pixabay.com → *Sound Effects* (see old file Pack K for the rules: small clean puppy recordings, no voices, no music). Save as the file name in the first column, upload to `art-inbox/audio/`, and add one line per file to `art-inbox/audio/LEDGER.txt` (title | artist | link | date). **The source and licence of the 17 Nov files are still not recorded — please add them as well.**

### Bella's voice — still missing

| File name | Search words | Pick the one that sounds… |
|---|---|---|
| `yip_2`, `yip_3` ★ (yip_1 is done) | puppy yip · puppy squeak excited | tiny and delighted — **never** like a cry of pain  |
| `whine_1/2` | puppy whine · puppy whimper soft | asking, not distressed  |
| `shake` | dog shaking · dog collar jingle | whole-body shake after the bath  |
| `growl_play` | puppy play growl | silly tug-of-war growl, not angry  |
| `howl` | puppy howl cute | a tiny "aroo"  |
| `paws` | dog running wood floor · dog paws tapping | little claws on a wooden floor  |
| `lick` | dog licking | short  |

### Game sounds — still missing

| File name | Used for | Search words |
|---|---|---|
| `purchase` | buying in shops | cash register cute · kaching soft |
| `sticker` | new sticker | sticker peel · paper stick |
| `page` | sticker book pages | page turn |
| `unlock` | new thing unlocked | unlock magic · level unlock |
| `squeak` | squeaky toys | squeaky toy |
| `bounce` | ball | rubber ball bounce |
| `scrub` | bath sponge | scrubbing brush soft |
| `shower` | rinsing | shower water short |
| `rustle` | changing outfits | cloth rustle · clothes |
| `shutter` | photos | camera shutter |
| `heartbeat` | vet stethoscope | heartbeat calm |
| `confetti` | parties | party popper |
| `firework` | festivals | firework distant soft |
| `chirp` | Pip the bluebird | bird chirp single |
| `ribbit` | Hoppy the frog | frog ribbit single |

Short jingles (2–5 s, *Sound Effects*): `jingle_win` (success fanfare short) · `jingle_levelup` (level up) · `jingle_gift` (magic reveal) · `jingle_sticker` (achievement chime) · `jingle_goodnight` (music box short).

### Background atmosphere — still missing (loops, 30–60 s)

| File name | Search words |
|---|---|
| `amb_rain` | rain on window gentle · soft rain |
| `amb_garden` | summer meadow breeze birds |
| `amb_beach` | gentle waves beach calm |

---

# TIER 2 — Makes it feel like a big game

## 2A. Sparkle effects (Pack H)

#### H-01 — Particles 1

*Tool:* ChatGPT · *Save as:* `H-01.png` · *Becomes:* `Resources/Fx/` → `fx_heart`, `fx_star`, `fx_sparkle`, `fx_bubble`, `fx_drop`, `fx_note`, `fx_paw`, `fx_zzz`, `fx_ring` · *Final size:* 128–256 each

```text
Create a sheet of 9 small game effect sprites.
1. a plump glossy pink heart
2. a fat gold star
3. a four-point white sparkle with a soft glow
4. a single soap bubble with a rainbow sheen and a white glint, mostly see-through
5. a sky-blue water drop
6. a pink music note
7. a white paw print
8. a lilac letter Z in a bubbly font (this one letter is wanted)
9. a thin soft white ring, like a ripple
Layout: a tidy 3 by 3 grid, each centred in its own cell with wide empty gaps, nothing touching, no outlines, no drop shadows. Transparent background. Square image. Soft 3D toy look, clean and simple. No other text.
```

#### H-02 — Particles 2 — soft shapes (all pure white, soft edges)

*Tool:* ChatGPT · *Save as:* `H-02.png` · *Becomes:* `Resources/Fx/` → `fx_glow`, `fx_rays`, `fx_puff`, `fx_foam`, `fx_dust`, `fx_beam`, `fx_flare`, `fx_smoke`, `fx_dot` · *Final size:* 128–256 each

```text
Create a sheet of 9 small game effect sprites.
1. a round soft glow fading to nothing at the edge
2. a circular burst of soft light rays
3. a small cartoon cloud puff
4. a clump of soap foam made of many tiny bubbles
5. a low dust puff, wider than tall
6. a soft vertical light beam, wider at the bottom
7. a small lens-flare star
8. a wispy curl of steam
9. a plain soft round dot
Layout: a tidy 3 by 3 grid, each centred in its own cell with wide empty gaps, nothing touching, no outlines, no drop shadows. Transparent background. Square image. Soft 3D toy look, clean and simple. No other text.
```

#### H-03 — Confetti, petals and weather

*Tool:* ChatGPT · *Save as:* `H-03.png` · *Becomes:* `Resources/Fx/` → `fx_confetti_1`, `fx_confetti_2`, `fx_confetti_3`, `fx_petal`, `fx_leaf`, `fx_snow`, `fx_rain`, `fx_firework`, `fx_splat` · *Final size:* 128–256 each

```text
Create a sheet of 9 small game effect sprites.
1. a pink paper confetti rectangle, slightly curled
2. a gold confetti spiral streamer
3. a teal confetti triangle
4. a pink flower petal
5. an orange autumn leaf
6. a white six-point snowflake
7. a pale blue slanted raindrop streak
8. a round firework burst in gold and pink
9. a splash of bright pink powder paint
Layout: a tidy 3 by 3 grid, each centred in its own cell with wide empty gaps, nothing touching, no outlines, no drop shadows. Transparent background. Square image. Soft 3D toy look, clean and simple. No other text.
```

## 2B. Room themes (Pack E) — wallpapers, floors, rugs, window views, wall pictures

#### E-01 — Wallpaper — paws

*Tool:* Gemini · *Save as:* `E-01.png` · *Becomes:* `Resources/Surfaces/wall_paws.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: warm cream wallpaper with small scattered pale-peach paw prints and tiny bones. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-02 — Wallpaper — clouds

*Tool:* Gemini · *Save as:* `E-02.png` · *Becomes:* `Resources/Surfaces/wall_clouds.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: sky-blue wallpaper with small fluffy white clouds and a few tiny gold stars. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-03 — Wallpaper — hearts

*Tool:* Gemini · *Save as:* `E-03.png` · *Becomes:* `Resources/Surfaces/wall_hearts.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: soft pink wallpaper with tiny white hearts in neat diagonal rows. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-04 — Wallpaper — night

*Tool:* Gemini · *Save as:* `E-04.png` · *Becomes:* `Resources/Surfaces/wall_night.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: deep lilac-blue night wallpaper with small gold stars and crescent moons. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-05 — Wallpaper — jungle

*Tool:* Gemini · *Save as:* `E-05.png` · *Becomes:* `Resources/Surfaces/wall_jungle.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: mint green wallpaper with soft darker-mint round leaves and tiny pink flowers. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-06 — Wallpaper — sunny

*Tool:* Gemini · *Save as:* `E-06.png` · *Becomes:* `Resources/Surfaces/wall_sunny.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: pale yellow wallpaper with thin white vertical stripes and tiny daisies. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-07 — Wallpaper — rainbow

*Tool:* Gemini · *Save as:* `E-07.png` · *Becomes:* `Resources/Surfaces/wall_rainbow.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: cream wallpaper with small pastel rainbows and white dots. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-08 — Wallpaper — tiles

*Tool:* Gemini · *Save as:* `E-08.png` · *Becomes:* `Resources/Surfaces/wall_tiles.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a wallpaper texture for a children's game room: glossy square pastel-aqua bathroom tiles with white grout and an occasional tile showing a tiny bubble design. Painted, soft, slightly velvety look.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-09 — Floor — wood

*Tool:* Gemini · *Save as:* `E-09.png` · *Becomes:* `Resources/Surfaces/floor_wood.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: warm honey-coloured wooden floorboards running left to right, soft grain, rounded plank edges, cartoon style.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-10 — Floor — checker

*Tool:* Gemini · *Save as:* `E-10.png` · *Becomes:* `Resources/Surfaces/floor_checker.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: soft checkerboard floor tiles in cream and pale teal.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-11 — Floor — carpet

*Tool:* Gemini · *Save as:* `E-11.png` · *Becomes:* `Resources/Surfaces/floor_carpet.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: fluffy pale pink carpet seen from directly above, soft fibres.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-12 — Floor — grass

*Tool:* Gemini · *Save as:* `E-12.png` · *Becomes:* `Resources/Surfaces/floor_grass.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: bright cartoon lawn grass seen from directly above with a few tiny daisies and clover.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-13 — Floor — sand

*Tool:* Gemini · *Save as:* `E-13.png` · *Becomes:* `Resources/Surfaces/floor_sand.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: soft golden beach sand seen from directly above with a few tiny shells and gentle ripples.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-14 — Floor — stage

*Tool:* Gemini · *Save as:* `E-14.png` · *Becomes:* `Resources/Surfaces/floor_stage.jpg` · *Final size:* 512×512 JPG, tileable

```text
Create a floor texture for a children's game: polished dark-honey stage floorboards with a soft sheen.
This must be a seamless tileable pattern: the left edge matches the right edge and the top matches the bottom, with no border, no vignette, no lighting gradient and no single large object. Perfectly flat, straight-on view. Square image. Low contrast and soft so that characters in front stand out. No text.
```

#### E-15 — Rugs, seen from directly above

*Tool:* ChatGPT · *Save as:* `E-15.png` · *Becomes:* `Resources/Surfaces/rug_rainbow.png`, `rug_paw.png`, `rug_donut.png`, `rug_flower.png` · *Final size:* 512 each

```text
Create a sheet of 4 round rugs seen from exactly straight above (perfect circles, no perspective):
1. a rainbow-ring rug with a cream centre
2. a teal rug with one big cream paw print
3. a pink doughnut rug with white icing and sprinkles
4. a sunflower rug with yellow petals and a brown centre
Soft woven fabric texture, flat even lighting, no shadows.
Layout: a tidy 2 by 2 grid of 4 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
```

#### E-16 — Window scenery strips

*Tool:* ChatGPT · *Save as:* `E-16.png` · *Becomes:* `Resources/Surfaces/view_hills.png`, `view_town.png`, `view_beach.png`, `view_snow.png` · *Final size:* 768×320 each  
*Note:* The game already draws the sky colour, sun, moon, clouds and stars behind these — which is why the sky must be transparent.

```text
Create 4 separate wide scenery strips stacked in one column with wide gaps between them. Each is a simple cartoon horizon silhouette scene that will be seen through a window, with the sky left completely transparent above it:
1. rolling green hills with three round trees and a tiny red-roofed house
2. a row of colourful little town houses with chimneys and one tall tree
3. a beach with a palm tree, a sandcastle and gentle waves
4. snowy mountains with pine trees
Each strip is wide (about 5 to 2), fills the bottom of its area and has a soft uneven top edge; nothing but transparency above. Soft, slightly hazy colours so they feel far away. Transparent background.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### E-17 — Pictures for the wall frame

*Tool:* ChatGPT or Gemini · *Save as:* `E-17.png` · *Becomes:* `Resources/Surfaces/pic_1.png` … `pic_9.png` · *Final size:* 256 each  
*Note:* The child taps the picture frame to change the painting.

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 small square paintings to hang in a child's room, each a simple picture filling its square to the edges (no frames):
1. Bella's portrait, smiling
2. a single pink paw print on cream
3. a red ball on a teal background
4. a rainbow over hills
5. Bella asleep under a crescent moon
6. a bone crossed with a carrot, like a funny coat of arms
7. a sunflower
8. a sailing boat on a pond
9. Bella and a bluebird, cheek to cheek
Layout: a tidy 3 by 3 grid with wide gaps, on a plain mid-grey #808080 background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

## 2C. Real furniture and props — Wave 1 (Pack G)

Two steps per model: picture in ChatGPT/Gemini first, then Tripo image-to-3D. **Settle the Tripo licence first** (old file §0.2): free credits are non-commercial.

#### G-01 — Dog bed

*Step 1, ChatGPT or Gemini → save the picture as* `G-01.png` · *Step 2, Tripo image-to-3D → download as* `G-01.glb` · *Becomes:* `Resources/Props/bed.fbx` + `bed_tex.png`

```text
One single 3D game prop on a plain pure white background: a round doughnut-shaped dog bed, lilac plush rim with a thick cream cushion in the middle and a small paw print embroidered on the front.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round doughnut-shaped dog bed, lilac plush rim with a thick cream cushion in the middle and a small paw print embroidered on the front, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-02 — Food bowl

*Step 1, ChatGPT or Gemini → save the picture as* `G-02.png` · *Step 2, Tripo image-to-3D → download as* `G-02.glb` · *Becomes:* `Resources/Props/bowl.fbx` + `bowl_tex.png`

```text
One single 3D game prop on a plain pure white background: an empty rose-red dog food bowl with a cream bone emblem on the side, darker red inside.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `an empty rose-red dog food bowl with a cream bone emblem on the side, darker red inside, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-03 — Kibble mound

*Step 1, ChatGPT or Gemini → save the picture as* `G-03.png` · *Step 2, Tripo image-to-3D → download as* `G-03.glb` · *Becomes:* `Resources/Props/kibble.fbx` + `kibble_tex.png`

```text
One single 3D game prop on a plain pure white background: a small rounded heap of brown dog kibble pieces, shaped to sit inside a bowl.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small rounded heap of brown dog kibble pieces, shaped to sit inside a bowl, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-04 — Ball

*Step 1, ChatGPT or Gemini → save the picture as* `G-04.png` · *Step 2, Tripo image-to-3D → download as* `G-04.glb` · *Becomes:* `Resources/Props/ball.fbx` + `ball_tex.png`

```text
One single 3D game prop on a plain pure white background: a rose-red rubber ball with a wide gold band and one gold star.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a rose-red rubber ball with a wide gold band and one gold star, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-05 — Bathtub

*Step 1, ChatGPT or Gemini → save the picture as* `G-05.png` · *Step 2, Tripo image-to-3D → download as* `G-05.glb` · *Becomes:* `Resources/Props/tub.fbx` + `tub_tex.png`

```text
One single 3D game prop on a plain pure white background: an empty white oval claw-foot bathtub with four round gold feet and a gold tap at one end, thick rounded rim, no water.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `an empty white oval claw-foot bathtub with four round gold feet and a gold tap at one end, thick rounded rim, no water, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-06 — Potted plant

*Step 1, ChatGPT or Gemini → save the picture as* `G-06.png` · *Step 2, Tripo image-to-3D → download as* `G-06.glb` · *Becomes:* `Resources/Props/plant.fbx` + `plant_tex.png`

```text
One single 3D game prop on a plain pure white background: a cheerful round-leaved houseplant in an orange pot with a white zigzag stripe.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a cheerful round-leaved houseplant in an orange pot with a white zigzag stripe, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-07 — Toy shelf

*Step 1, ChatGPT or Gemini → save the picture as* `G-07.png` · *Step 2, Tripo image-to-3D → download as* `G-07.glb` · *Becomes:* `Resources/Props/toyshelf.fbx` + `toyshelf_tex.png`

```text
One single 3D game prop on a plain pure white background: a small white wall shelf holding a brown teddy bear, three colourful picture books and a striped ball.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small white wall shelf holding a brown teddy bear, three colourful picture books and a striped ball, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-08 — Toy basket

*Step 1, ChatGPT or Gemini → save the picture as* `G-08.png` · *Step 2, Tripo image-to-3D → download as* `G-08.glb` · *Becomes:* `Resources/Props/basket.fbx` + `basket_tex.png`

```text
One single 3D game prop on a plain pure white background: a round wicker toy basket with two handles, filled with three colourful balls and a rope toy.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round wicker toy basket with two handles, filled with three colourful balls and a rope toy, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-16 — Rubber duck

*Step 1, ChatGPT or Gemini → save the picture as* `G-16.png` · *Step 2, Tripo image-to-3D → download as* `G-16.glb` · *Becomes:* `Resources/Props/toy_duck.fbx` + `toy_duck_tex.png`

```text
One single 3D game prop on a plain pure white background: a classic yellow rubber duck with an orange beak and a happy face.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a classic yellow rubber duck with an orange beak and a happy face, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-43 — Crown

*Step 1, ChatGPT or Gemini → save the picture as* `G-43.png` · *Step 2, Tripo image-to-3D → download as* `G-43.glb` · *Becomes:* `Resources/Props/hat_crown.fbx` + `hat_crown_tex.png`

```text
One single 3D game prop on a plain pure white background: a small gold crown with five rounded points and rose-red round gems, open in the middle.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small gold crown with five rounded points and rose-red round gems, open in the middle, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-71 — Pip the bluebird

*Step 1, ChatGPT or Gemini → save the picture as* `G-71.png` · *Step 2, Tripo image-to-3D → download as* `G-71.glb` · *Becomes:* `Resources/Props/friend_bird.fbx` + `friend_bird_tex.png`

```text
One single 3D game prop on a plain pure white background: a tiny round bluebird chick with a cream tummy, a small orange beak, dot eyes and stubby wings held close, standing on two little feet.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a tiny round bluebird chick with a cream tummy, a small orange beak, dot eyes and stubby wings held close, standing on two little feet, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

## 2D. Two small UI pictures

#### D-07 — Growing-up card

*Tool:* Gemini · *Save as:* `D-07.png` · *Becomes:* `Resources/UI/growth.png` (cut into `growth_1/2/3.png`) · *Final size:* 1536×768  
*Note:* Shown when she reaches a new growth stage.

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint one wide picture showing Bella at three ages standing side by side on a wooden floor against a plain cream wall with pencil height marks: on the left a tiny newborn-looking pup who is mostly head with barely-open eyes; in the middle Bella exactly as in the reference; on the right a slightly bigger, leggier Bella with a longer muzzle and bigger ears but the same face and collar. Each looks at the viewer happily.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### D-08 — Thought-bubble wishes

*Tool:* ChatGPT · *Save as:* `D-08.png` · *Becomes:* `Resources/UI/wish_feed.png`, `wish_bath`, `wish_sleep`, `wish_play`, `wish_pet`, `wish_tricks`, `wish_clinic`, `wish_garden`, `wish_wardrobe` · *Final size:* 256 each

```text
Create a sheet of 9 small simple pictures that will appear inside a puppy's thought bubble to show what she wants:
1. a full food bowl with a heart above it
2. a bubbly bathtub
3. a pillow with a moon
4. a ball mid-bounce with motion lines
5. a hand stroking (a small glove hand with hearts)
6. a magic wand with a star
7. a doctor's bag with a heart
8. a garden gate with flowers
9. a pink bow on a hanger
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

---

# TIER 3 — More game

## 3A. New places (Pack F, backdrops)

#### F-01 — Backdrop — Garden

*Tool:* Gemini · *Save as:* `F-01.png` · *Becomes:* `Resources/Places/garden.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_grass`.

```text
Paint a backdrop for a children's puppy game: a sunny back garden: a white picket fence, a round apple tree on the left, flower beds with tulips, a washing line with tiny socks, soft hills and blue sky with fluffy clouds beyond.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-02 — Backdrop — Park

*Tool:* Gemini · *Save as:* `F-02.png` · *Becomes:* `Resources/Places/park.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_grass`.

```text
Paint a backdrop for a children's puppy game: a cheerful park: a yellow slide and a red swing in the distance, round trees, a small pond with lily pads, a winding path, kites in the sky.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-03 — Backdrop — Bathroom

*Tool:* Gemini · *Save as:* `F-03.png` · *Becomes:* `Resources/Places/bathroom.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_checker`.

```text
Paint a backdrop for a children's puppy game: a cosy bathroom wall: pastel aqua tiles to shoulder height, cream wall above, a round mirror with a gold frame, a shelf with coloured bottles and a rubber duck, a small frosted window with a plant, fluffy towels on hooks.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-04 — Backdrop — Kitchen

*Tool:* Gemini · *Save as:* `F-04.png` · *Becomes:* `Resources/Places/kitchen.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_checker`.

```text
Paint a backdrop for a children's puppy game: a cosy kitchen wall: pastel mint cabinets, a rounded pink fridge with paw-print magnets, a window with gingham curtains, a shelf of jars and a hanging row of spoons, a steaming pot on a small stove.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-05 — Backdrop — Vet clinic

*Tool:* Gemini · *Save as:* `F-05.png` · *Becomes:* `Resources/Places/clinic.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_checker`.

```text
Paint a backdrop for a children's puppy game: a friendly animal doctor's room: pale sky-blue wall, a giraffe height chart, a cabinet with a red heart on it, picture frames showing a happy cat and rabbit, a weighing scale, a jar of bone biscuits on the counter.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-06 — Backdrop — Trick stage

*Tool:* Gemini · *Save as:* `F-06.png` · *Becomes:* `Resources/Places/stage.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_stage`.

```text
Paint a backdrop for a children's puppy game: a little theatre stage: plush rose-red curtains drawn to each side with gold tassels, a painted backdrop of stars and a crescent moon, three soft spotlights shining down, footlights along the bottom band.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-07 — Backdrop — Beach

*Tool:* Gemini · *Save as:* `F-07.png` · *Becomes:* `Resources/Places/beach.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_sand`.

```text
Paint a backdrop for a children's puppy game: a sunny beach: calm turquoise sea, a striped parasol and a sandcastle to one side, a palm tree, a small lighthouse far away, puffy clouds.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-08 — Backdrop — Bedroom at night

*Tool:* Gemini · *Save as:* `F-08.png` · *Becomes:* `Resources/Places/bedroom_night.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_carpet`.

```text
Paint a backdrop for a children's puppy game: a child's bedroom wall at night: deep lilac-blue wall, a string of warm fairy lights, a window showing a big moon and stars, a shelf of sleeping soft toys, a glowing star night-light.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-09 — Backdrop — Snowy garden

*Tool:* Gemini · *Save as:* `F-09.png` · *Becomes:* `Resources/Places/snow.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_carpet`.

```text
Paint a backdrop for a children's puppy game: a snowy garden at dusk: snow-covered fence and pine trees, a snowman in the distance, warm lit windows of a small house, gentle snowflakes, a pink and lilac sky.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-10 — Backdrop — Festival evening

*Tool:* Gemini · *Save as:* `F-10.png` · *Becomes:* `Resources/Places/festival.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_stage`.

```text
Paint a backdrop for a children's puppy game: a festive street at dusk: strings of warm lights and star lanterns, marigold garlands over a doorway, rows of little glowing clay lamps along a wall, soft fireworks blooming in a deep blue sky.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-11 — Backdrop — Photo studio — hearts

*Tool:* Gemini · *Save as:* `F-11.png` · *Becomes:* `Resources/Places/studio_pink.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_wood`.

```text
Paint a backdrop for a children's puppy game: a photo studio backdrop: a soft pink paper backdrop covered in large pale hearts, two little studio lights at the far sides.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### F-12 — Backdrop — Photo studio — rainbow

*Tool:* Gemini · *Save as:* `F-12.png` · *Becomes:* `Resources/Places/studio_rainbow.jpg` · *Final size:* 1600×1000 JPG  
*Note:* Pairs with floor `floor_wood`.

```text
Paint a backdrop for a children's puppy game: a photo studio backdrop: a big pastel rainbow arch painted on a cream wall with clouds at its feet and gold stars.
Format: wide landscape picture (16:10). It is a flat backdrop seen straight on, like a stage set — no strong perspective, no floor receding towards the viewer. The bottom 12% is a plain straight band where the backdrop meets the ground, perfectly level across the whole width. Put the most interesting things in the middle third; the left and right thirds are simpler. Leave the lower middle fairly clear because a puppy will stand in front of it. No characters, no animals, no people.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

## 3B. Festival stickers and festival sets (Pack D-06 and Pack I)

#### D-06 — Sticker sheet 4 — festivals

*Tool:* ChatGPT or Gemini · *Save as:* `D-06.png` · *Becomes:* `Resources/Stickers/` → `fest_lights`, `fest_colours`, `fest_winter`, `fest_birthday`, `fest_kites`, `fest_harvest`, `fest_newyear`, `fest_monsoon`, `fest_spring` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 collectible stickers showing Bella's whole body in little scenes. Each sticker is a compact, die-cut shape with everything inside one thick white sticker border and a soft shadow, like real vinyl stickers.
1. Bella sitting beside a row of little glowing clay lamps and a star lantern
2. Bella covered in pink, yellow and blue powder paint, laughing
3. Bella in a teal scarf beside a small snowman
4. Bella in a party hat blowing out one candle on a cake
5. Bella running with a diamond kite flying behind her
6. Bella sitting inside a big smiling pumpkin
7. Bella watching soft fireworks, wearing a gold bow tie
8. Bella sailing a paper boat in a puddle with a small frog
9. Bella wearing a crown of flowers among tulips
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-01 — Festival of Lights (Diwali week)

*Tool:* ChatGPT · *Save as:* `I-01.png` · *Becomes:* `Resources/Seasons/lights_1.png` … `lights_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a small clay oil lamp with a warm flame
2. a round rangoli floor pattern in pink, orange and yellow petals, seen from straight above
3. a second round rangoli pattern in teal, white and gold, seen from straight above
4. a star-shaped paper lantern with tassels
5. a marigold and mango-leaf door garland, straight and wide
6. a hand sparkler throwing gold sparks
7. an open box of round sweets
8. a string of warm fairy lights, straight and wide
9. a gold firework burst
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-02 — Festival of Colours (Holi)

*Tool:* ChatGPT · *Save as:* `I-02.png` · *Becomes:* `Resources/Seasons/colours_1.png` … `colours_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a brass plate heaped with pink powder
2. a brass plate heaped with yellow powder
3. a brass plate heaped with blue powder
4. a pink powder splash
5. a teal powder splash
6. a yellow powder splash
7. a small water balloon
8. a toy water squirter in rainbow colours
9. a white cloth bunting splashed with colours, straight and wide
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-03 — Winter holidays

*Tool:* ChatGPT · *Save as:* `I-03.png` · *Becomes:* `Resources/Seasons/winter_1.png` … `winter_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a small decorated pine tree in a pot
2. a red stocking with a bone poking out
3. a gold star tree-topper
4. a green wreath with a pink bow
5. a striped candy cane
6. a snow globe with a tiny doghouse inside
7. a string of coloured bulb lights, straight and wide
8. a paper snowflake
9. a mug of warm milk with a paw-print marshmallow
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-04 — Birthday

*Tool:* ChatGPT · *Save as:* `I-04.png` · *Becomes:* `Resources/Seasons/birthday_1.png` … `birthday_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a bunch of three balloons
2. a single gold star balloon
3. a party popper bursting with confetti
4. a pink cake with one candle
5. a rainbow paper bunting, straight and wide
6. a party blower
7. a paper crown
8. a wrapped present with a paw-print tag
9. a round badge with a gold star and ribbon tails
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-05 — Rainy season

*Tool:* ChatGPT · *Save as:* `I-05.png` · *Becomes:* `Resources/Seasons/monsoon_1.png` … `monsoon_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a yellow umbrella, open
2. a pair of tiny red rain boots
3. a folded paper boat
4. a shiny puddle seen from straight above
5. a smiling green frog on a leaf
6. a grey-blue cloud with raindrops
7. a pastel rainbow
8. a snail with a pink shell
9. a big green leaf used as an umbrella
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-06 — Kite day and spring

*Tool:* ChatGPT · *Save as:* `I-06.png` · *Becomes:* `Resources/Seasons/kites_1.png` … `kites_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a pink diamond kite with a ribbon tail
2. a teal box kite
3. a gold fish-shaped kite
4. a spool of kite string
5. a flower crown
6. a basket of mangoes
7. a sunflower
8. a row of tulips, straight and wide
9. a butterfly with yellow wings
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-07 — Harvest party

*Tool:* ChatGPT · *Save as:* `I-07.png` · *Becomes:* `Resources/Seasons/harvest_1.png` … `harvest_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a smiling pumpkin
2. a small hay bale
3. a basket of apples
4. a friendly scarecrow with a straw hat and a big smile
5. an ear of corn
6. an orange autumn leaf garland, straight and wide
7. a toffee apple
8. a wooden wheelbarrow of pumpkins
9. an acorn wearing a tiny scarf
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### I-08 — New Year

*Tool:* ChatGPT · *Save as:* `I-08.png` · *Becomes:* `Resources/Seasons/newyear_1.png` … `newyear_9.png` · *Final size:* 384 each

```text
Create a sheet of 9 festive decorations for a children's puppy game.
1. a gold bow tie
2. a sparkling party hat
3. a round clock face with no numbers and both hands pointing straight up
4. a gold and pink firework burst
5. a second firework burst in teal and lilac
6. a glittering star garland, straight and wide
7. a party horn
8. a confetti cannon
9. a gold balloon shaped like a star
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

## 3C. Remaining 3D models (Pack G, Waves 2–3)

#### G-09 — Armchair

*Step 1, ChatGPT or Gemini → save the picture as* `G-09.png` · *Step 2, Tripo image-to-3D → download as* `G-09.glb` · *Becomes:* `Resources/Props/armchair.fbx` + `armchair_tex.png`

```text
One single 3D game prop on a plain pure white background: a tiny plump teal armchair with a pink heart cushion and stubby wooden legs.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a tiny plump teal armchair with a pink heart cushion and stubby wooden legs, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-10 — Star night-light

*Step 1, ChatGPT or Gemini → save the picture as* `G-10.png` · *Step 2, Tripo image-to-3D → download as* `G-10.glb` · *Becomes:* `Resources/Props/nightlight.fbx` + `nightlight_tex.png`

```text
One single 3D game prop on a plain pure white background: a floor lamp shaped like a smiling gold star on a short white stand.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a floor lamp shaped like a smiling gold star on a short white stand, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-11 — Water bowl

*Step 1, ChatGPT or Gemini → save the picture as* `G-11.png` · *Step 2, Tripo image-to-3D → download as* `G-11.glb` · *Becomes:* `Resources/Props/waterbowl.fbx` + `waterbowl_tex.png`

```text
One single 3D game prop on a plain pure white background: a sky-blue dog water bowl with a white droplet emblem, clear blue water inside.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a sky-blue dog water bowl with a white droplet emblem, clear blue water inside, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-12 — Window flower box

*Step 1, ChatGPT or Gemini → save the picture as* `G-12.png` · *Step 2, Tripo image-to-3D → download as* `G-12.glb` · *Becomes:* `Resources/Props/windowbox.fbx` + `windowbox_tex.png`

```text
One single 3D game prop on a plain pure white background: a white window box planted with pink and yellow tulips.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a white window box planted with pink and yellow tulips, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-15 — Rope toy

*Step 1, ChatGPT or Gemini → save the picture as* `G-15.png` · *Step 2, Tripo image-to-3D → download as* `G-15.glb` · *Becomes:* `Resources/Props/toy_rope.fbx` + `toy_rope_tex.png`

```text
One single 3D game prop on a plain pure white background: a short knotted rope tug toy in rainbow colours with a knot at each end.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a short knotted rope tug toy in rainbow colours with a knot at each end, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-17 — Teddy bear

*Step 1, ChatGPT or Gemini → save the picture as* `G-17.png` · *Step 2, Tripo image-to-3D → download as* `G-17.glb` · *Becomes:* `Resources/Props/toy_teddy.fbx` + `toy_teddy_tex.png`

```text
One single 3D game prop on a plain pure white background: a small sitting brown teddy bear with a teal bow and stitched smile.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small sitting brown teddy bear with a teal bow and stitched smile, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-18 — Frisbee

*Step 1, ChatGPT or Gemini → save the picture as* `G-18.png` · *Step 2, Tripo image-to-3D → download as* `G-18.glb` · *Becomes:* `Resources/Props/toy_frisbee.fbx` + `toy_frisbee_tex.png`

```text
One single 3D game prop on a plain pure white background: an orange flying disc with a white paw print in the centre.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `an orange flying disc with a white paw print in the centre, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-21 — Bone biscuit

*Step 1, ChatGPT or Gemini → save the picture as* `G-21.png` · *Step 2, Tripo image-to-3D → download as* `G-21.glb` · *Becomes:* `Resources/Props/food_treat.fbx` + `food_treat_tex.png`

```text
One single 3D game prop on a plain pure white background: one bone-shaped golden biscuit with a small heart cut out of the middle.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `one bone-shaped golden biscuit with a small heart cut out of the middle, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-22 — Fancy meal

*Step 1, ChatGPT or Gemini → save the picture as* `G-22.png` · *Step 2, Tripo image-to-3D → download as* `G-22.glb` · *Becomes:* `Resources/Props/food_fancy.fbx` + `food_fancy_tex.png`

```text
One single 3D game prop on a plain pure white background: a white plate with a neat mound of rice, shredded chicken and three carrot coins, a sprig of green on top.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a white plate with a neat mound of rice, shredded chicken and three carrot coins, a sprig of green on top, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-23 — Birthday cake

*Step 1, ChatGPT or Gemini → save the picture as* `G-23.png` · *Step 2, Tripo image-to-3D → download as* `G-23.glb` · *Becomes:* `Resources/Props/food_cake.fbx` + `food_cake_tex.png`

```text
One single 3D game prop on a plain pure white background: a small pink frosted cake with white drips, one lit candle and a tiny bone on top.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small pink frosted cake with white drips, one lit candle and a tiny bone on top, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-27 — Shampoo bottle

*Step 1, ChatGPT or Gemini → save the picture as* `G-27.png` · *Step 2, Tripo image-to-3D → download as* `G-27.glb` · *Becomes:* `Resources/Props/shampoo.fbx` + `shampoo_tex.png`

```text
One single 3D game prop on a plain pure white background: a rounded teal pet shampoo bottle with a pink pump top and a bubble emblem, no writing.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a rounded teal pet shampoo bottle with a pink pump top and a bubble emblem, no writing, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-28 — Sponge

*Step 1, ChatGPT or Gemini → save the picture as* `G-28.png` · *Step 2, Tripo image-to-3D → download as* `G-28.glb` · *Becomes:* `Resources/Props/sponge.fbx` + `sponge_tex.png`

```text
One single 3D game prop on a plain pure white background: a thick yellow bath sponge with big holes.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a thick yellow bath sponge with big holes, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-31 — Doctor's bag

*Step 1, ChatGPT or Gemini → save the picture as* `G-31.png` · *Step 2, Tripo image-to-3D → download as* `G-31.glb` · *Becomes:* `Resources/Props/vetbag.fbx` + `vetbag_tex.png`

```text
One single 3D game prop on a plain pure white background: a cheerful white doctor's bag with a rose-red heart on the side and a gold clasp.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a cheerful white doctor's bag with a rose-red heart on the side and a gold clasp, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-41 — Top hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-41.png` · *Step 2, Tripo image-to-3D → download as* `G-41.glb` · *Becomes:* `Resources/Props/hat_top.fbx` + `hat_top_tex.png`

```text
One single 3D game prop on a plain pure white background: a little plum-purple top hat with a rose-red band, open and hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a little plum-purple top hat with a rose-red band, open and hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-42 — Party hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-42.png` · *Step 2, Tripo image-to-3D → download as* `G-42.glb` · *Becomes:* `Resources/Props/hat_party.fbx` + `hat_party_tex.png`

```text
One single 3D game prop on a plain pure white background: a sky-blue cone party hat with gold dots, a gold pom-pom on top and a gold rim, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a sky-blue cone party hat with gold dots, a gold pom-pom on top and a gold rim, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-44 — Sun hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-44.png` · *Step 2, Tripo image-to-3D → download as* `G-44.glb` · *Becomes:* `Resources/Props/hat_sun.fbx` + `hat_sun_tex.png`

```text
One single 3D game prop on a plain pure white background: a woven straw sun hat with a wide floppy brim and a pink ribbon, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a woven straw sun hat with a wide floppy brim and a pink ribbon, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-45 — Beanie

*Step 1, ChatGPT or Gemini → save the picture as* `G-45.png` · *Step 2, Tripo image-to-3D → download as* `G-45.glb` · *Becomes:* `Resources/Props/hat_beanie.fbx` + `hat_beanie_tex.png`

```text
One single 3D game prop on a plain pure white background: a knitted teal beanie with a folded cream cuff and a big cream pom-pom, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a knitted teal beanie with a folded cream cuff and a big cream pom-pom, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-49 — Hair bow

*Step 1, ChatGPT or Gemini → save the picture as* `G-49.png` · *Step 2, Tripo image-to-3D → download as* `G-49.glb` · *Becomes:* `Resources/Props/bow.fbx` + `bow_tex.png`

```text
One single 3D game prop on a plain pure white background: a plump pink ribbon bow with a rose-red knot. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a plump pink ribbon bow with a rose-red knot, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-50 — Round glasses

*Step 1, ChatGPT or Gemini → save the picture as* `G-50.png` · *Step 2, Tripo image-to-3D → download as* `G-50.glb` · *Becomes:* `Resources/Props/glasses.fbx` + `glasses_tex.png`

```text
One single 3D game prop on a plain pure white background: round cocoa-brown glasses with thick frames and lightly tinted lenses, arms folded open. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `round cocoa-brown glasses with thick frames and lightly tinted lenses, arms folded open, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-61 — Doghouse

*Step 1, ChatGPT or Gemini → save the picture as* `G-61.png` · *Step 2, Tripo image-to-3D → download as* `G-61.glb` · *Becomes:* `Resources/Props/doghouse.fbx` + `doghouse_tex.png`

```text
One single 3D game prop on a plain pure white background: a little cream wooden doghouse with a teal roof, an arched door and a bone sign above it with no writing.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a little cream wooden doghouse with a teal roof, an arched door and a bone sign above it with no writing, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-62 — Garden tree

*Step 1, ChatGPT or Gemini → save the picture as* `G-62.png` · *Step 2, Tripo image-to-3D → download as* `G-62.glb` · *Becomes:* `Resources/Props/tree.fbx` + `tree_tex.png`

```text
One single 3D game prop on a plain pure white background: a round lollipop-shaped tree with a fat trunk and three red apples.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round lollipop-shaped tree with a fat trunk and three red apples, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-64 — Flower patch

*Step 1, ChatGPT or Gemini → save the picture as* `G-64.png` · *Step 2, Tripo image-to-3D → download as* `G-64.glb` · *Becomes:* `Resources/Props/flowers.fbx` + `flowers_tex.png`

```text
One single 3D game prop on a plain pure white background: a small mound of grass with five tulips in pink, yellow and lilac.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small mound of grass with five tulips in pink, yellow and lilac, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-72 — Momo the snail

*Step 1, ChatGPT or Gemini → save the picture as* `G-72.png` · *Step 2, Tripo image-to-3D → download as* `G-72.glb` · *Becomes:* `Resources/Props/friend_snail.fbx` + `friend_snail_tex.png`

```text
One single 3D game prop on a plain pure white background: a smiling snail with a pink spiral shell and two short eye stalks.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a smiling snail with a pink spiral shell and two short eye stalks, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-81 — Clay lamp

*Step 1, ChatGPT or Gemini → save the picture as* `G-81.png` · *Step 2, Tripo image-to-3D → download as* `G-81.glb` · *Becomes:* `Resources/Props/fest_diya.fbx` + `fest_diya_tex.png`

```text
One single 3D game prop on a plain pure white background: a small terracotta oil lamp painted with white and gold dots, with a soft teardrop flame.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small terracotta oil lamp painted with white and gold dots, with a soft teardrop flame, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-82 — Paper star lantern

*Step 1, ChatGPT or Gemini → save the picture as* `G-82.png` · *Step 2, Tripo image-to-3D → download as* `G-82.glb` · *Becomes:* `Resources/Props/fest_lantern.fbx` + `fest_lantern_tex.png`

```text
One single 3D game prop on a plain pure white background: a hanging star-shaped paper lantern in pink and gold with tassels.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a hanging star-shaped paper lantern in pink and gold with tassels, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-85 — Balloon bunch

*Step 1, ChatGPT or Gemini → save the picture as* `G-85.png` · *Step 2, Tripo image-to-3D → download as* `G-85.glb` · *Becomes:* `Resources/Props/fest_balloons.fbx` + `fest_balloons_tex.png`

```text
One single 3D game prop on a plain pure white background: three round balloons in pink, gold and teal tied to a small star weight.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `three round balloons in pink, gold and teal tied to a small star weight, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-88 — Present stack

*Step 1, ChatGPT or Gemini → save the picture as* `G-88.png` · *Step 2, Tripo image-to-3D → download as* `G-88.glb` · *Becomes:* `Resources/Props/fest_gifts.fbx` + `fest_gifts_tex.png`

```text
One single 3D game prop on a plain pure white background: a stack of three wrapped presents in pastel paper with gold bows.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a stack of three wrapped presents in pastel paper with gold bows, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-13 — Rocking horse toy

*Step 1, ChatGPT or Gemini → save the picture as* `G-13.png` · *Step 2, Tripo image-to-3D → download as* `G-13.glb` · *Becomes:* `Resources/Props/rocking.fbx` + `rocking_tex.png`

```text
One single 3D game prop on a plain pure white background: a small pastel wooden rocking puppy toy with a rainbow saddle.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small pastel wooden rocking puppy toy with a rainbow saddle, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-14 — Low bookcase

*Step 1, ChatGPT or Gemini → save the picture as* `G-14.png` · *Step 2, Tripo image-to-3D → download as* `G-14.glb` · *Becomes:* `Resources/Props/bookcase.fbx` + `bookcase_tex.png`

```text
One single 3D game prop on a plain pure white background: a low white bookcase with rounded corners, colourful book spines and a small globe on top.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a low white bookcase with rounded corners, colourful book spines and a small globe on top, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-19 — Plush bone

*Step 1, ChatGPT or Gemini → save the picture as* `G-19.png` · *Step 2, Tripo image-to-3D → download as* `G-19.glb` · *Becomes:* `Resources/Props/toy_plushbone.fbx` + `toy_plushbone_tex.png`

```text
One single 3D game prop on a plain pure white background: a soft blue plush bone toy with visible cream stitching.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a soft blue plush bone toy with visible cream stitching, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-20 — Tennis ball

*Step 1, ChatGPT or Gemini → save the picture as* `G-20.png` · *Step 2, Tripo image-to-3D → download as* `G-20.glb` · *Becomes:* `Resources/Props/toy_tennis.fbx` + `toy_tennis_tex.png`

```text
One single 3D game prop on a plain pure white background: a fuzzy lime-green tennis ball with a white curved seam.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a fuzzy lime-green tennis ball with a white curved seam, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-24 — Carrot

*Step 1, ChatGPT or Gemini → save the picture as* `G-24.png` · *Step 2, Tripo image-to-3D → download as* `G-24.glb` · *Becomes:* `Resources/Props/food_carrot.fbx` + `food_carrot_tex.png`

```text
One single 3D game prop on a plain pure white background: a chunky orange carrot with a tuft of green leaves.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a chunky orange carrot with a tuft of green leaves, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-25 — Watermelon slice

*Step 1, ChatGPT or Gemini → save the picture as* `G-25.png` · *Step 2, Tripo image-to-3D → download as* `G-25.glb` · *Becomes:* `Resources/Props/food_watermelon.fbx` + `food_watermelon_tex.png`

```text
One single 3D game prop on a plain pure white background: a thick triangular slice of watermelon with no seeds.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a thick triangular slice of watermelon with no seeds, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-26 — Pup-sicle

*Step 1, ChatGPT or Gemini → save the picture as* `G-26.png` · *Step 2, Tripo image-to-3D → download as* `G-26.glb` · *Becomes:* `Resources/Props/food_popsicle.fbx` + `food_popsicle_tex.png`

```text
One single 3D game prop on a plain pure white background: a frozen cream-and-banana ice lolly on a bone-shaped stick.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a frozen cream-and-banana ice lolly on a bone-shaped stick, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-29 — Shower head

*Step 1, ChatGPT or Gemini → save the picture as* `G-29.png` · *Step 2, Tripo image-to-3D → download as* `G-29.glb` · *Becomes:* `Resources/Props/showerhead.fbx` + `showerhead_tex.png`

```text
One single 3D game prop on a plain pure white background: a round teal hand shower head with a white handle.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round teal hand shower head with a white handle, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-30 — Folded towel

*Step 1, ChatGPT or Gemini → save the picture as* `G-30.png` · *Step 2, Tripo image-to-3D → download as* `G-30.glb` · *Becomes:* `Resources/Props/towel.fbx` + `towel_tex.png`

```text
One single 3D game prop on a plain pure white background: a fluffy folded pink towel with a paw print embroidered in white.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a fluffy folded pink towel with a paw print embroidered in white, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-32 — Thermometer

*Step 1, ChatGPT or Gemini → save the picture as* `G-32.png` · *Step 2, Tripo image-to-3D → download as* `G-32.glb` · *Becomes:* `Resources/Props/thermometer.fbx` + `thermometer_tex.png`

```text
One single 3D game prop on a plain pure white background: a chunky toy-like white thermometer with a pink heart tip.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a chunky toy-like white thermometer with a pink heart tip, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-33 — Medicine

*Step 1, ChatGPT or Gemini → save the picture as* `G-33.png` · *Step 2, Tripo image-to-3D → download as* `G-33.glb` · *Becomes:* `Resources/Props/medicine.fbx` + `medicine_tex.png`

```text
One single 3D game prop on a plain pure white background: a small rounded bottle of pink syrup with a white cap, and a spoon leaning on it.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small rounded bottle of pink syrup with a white cap, and a spoon leaning on it, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-34 — Stethoscope

*Step 1, ChatGPT or Gemini → save the picture as* `G-34.png` · *Step 2, Tripo image-to-3D → download as* `G-34.glb` · *Becomes:* `Resources/Props/stethoscope.fbx` + `stethoscope_tex.png`

```text
One single 3D game prop on a plain pure white background: a toy stethoscope with a teal tube and a gold chest piece, lying in a loose loop.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a toy stethoscope with a teal tube and a gold chest piece, lying in a loose loop, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-46 — Chef hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-46.png` · *Step 2, Tripo image-to-3D → download as* `G-46.glb` · *Becomes:* `Resources/Props/hat_chef.fbx` + `hat_chef_tex.png`

```text
One single 3D game prop on a plain pure white background: a puffy white chef's hat, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a puffy white chef's hat, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-47 — Pirate hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-47.png` · *Step 2, Tripo image-to-3D → download as* `G-47.glb` · *Becomes:* `Resources/Props/hat_pirate.fbx` + `hat_pirate_tex.png`

```text
One single 3D game prop on a plain pure white background: a friendly dark-teal pirate hat with a gold trim and a white bone-and-paw badge, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a friendly dark-teal pirate hat with a gold trim and a white bone-and-paw badge, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-48 — Wizard hat

*Step 1, ChatGPT or Gemini → save the picture as* `G-48.png` · *Step 2, Tripo image-to-3D → download as* `G-48.glb` · *Becomes:* `Resources/Props/hat_wizard.fbx` + `hat_wizard_tex.png`

```text
One single 3D game prop on a plain pure white background: a soft lilac wizard hat with a bent tip, covered in small gold stars, hollow underneath. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a soft lilac wizard hat with a bent tip, covered in small gold stars, hollow underneath, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-51 — Heart sunglasses

*Step 1, ChatGPT or Gemini → save the picture as* `G-51.png` · *Step 2, Tripo image-to-3D → download as* `G-51.glb` · *Becomes:* `Resources/Props/sunglasses.fbx` + `sunglasses_tex.png`

```text
One single 3D game prop on a plain pure white background: heart-shaped rose-red sunglasses with dark pink lenses, arms open. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `heart-shaped rose-red sunglasses with dark pink lenses, arms open, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-52 — Headphones

*Step 1, ChatGPT or Gemini → save the picture as* `G-52.png` · *Step 2, Tripo image-to-3D → download as* `G-52.glb` · *Becomes:* `Resources/Props/headphones.fbx` + `headphones_tex.png`

```text
One single 3D game prop on a plain pure white background: chunky pink over-ear headphones with a cream headband and gold stars on the ear cups. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `chunky pink over-ear headphones with a cream headband and gold stars on the ear cups, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-53 — Tiara

*Step 1, ChatGPT or Gemini → save the picture as* `G-53.png` · *Step 2, Tripo image-to-3D → download as* `G-53.glb` · *Becomes:* `Resources/Props/tiara.fbx` + `tiara_tex.png`

```text
One single 3D game prop on a plain pure white background: a delicate silver tiara with one large pink heart gem in the centre. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a delicate silver tiara with one large pink heart gem in the centre, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-54 — Backpack

*Step 1, ChatGPT or Gemini → save the picture as* `G-54.png` · *Step 2, Tripo image-to-3D → download as* `G-54.glb` · *Becomes:* `Resources/Props/backpack.fbx` + `backpack_tex.png`

```text
One single 3D game prop on a plain pure white background: a tiny sky-blue backpack with a front pocket and a small bone keyring. It is a wearable for a toy puppy, so it must be one rigid piece, perfectly symmetrical left to right.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a tiny sky-blue backpack with a front pocket and a small bone keyring, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-63 — Fence piece

*Step 1, ChatGPT or Gemini → save the picture as* `G-63.png` · *Step 2, Tripo image-to-3D → download as* `G-63.glb` · *Becomes:* `Resources/Props/fence.fbx` + `fence_tex.png`

```text
One single 3D game prop on a plain pure white background: one straight piece of white picket fence with rounded tops and a tuft of grass at its base.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `one straight piece of white picket fence with rounded tops and a tuft of grass at its base, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-65 — Watering can

*Step 1, ChatGPT or Gemini → save the picture as* `G-65.png` · *Step 2, Tripo image-to-3D → download as* `G-65.glb` · *Becomes:* `Resources/Props/wateringcan.fbx` + `wateringcan_tex.png`

```text
One single 3D game prop on a plain pure white background: a teal watering can with a daisy painted on the side.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a teal watering can with a daisy painted on the side, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-66 — Mushroom stool

*Step 1, ChatGPT or Gemini → save the picture as* `G-66.png` · *Step 2, Tripo image-to-3D → download as* `G-66.glb` · *Becomes:* `Resources/Props/mushroom.fbx` + `mushroom_tex.png`

```text
One single 3D game prop on a plain pure white background: a fat red-capped toy mushroom with white dots.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a fat red-capped toy mushroom with white dots, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-73 — Dot the ladybird

*Step 1, ChatGPT or Gemini → save the picture as* `G-73.png` · *Step 2, Tripo image-to-3D → download as* `G-73.glb` · *Becomes:* `Resources/Props/friend_ladybird.fbx` + `friend_ladybird_tex.png`

```text
One single 3D game prop on a plain pure white background: a round red ladybird with five black dots, a tiny smiling face and six short legs.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round red ladybird with five black dots, a tiny smiling face and six short legs, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-74 — Hoppy the frog

*Step 1, ChatGPT or Gemini → save the picture as* `G-74.png` · *Step 2, Tripo image-to-3D → download as* `G-74.glb` · *Becomes:* `Resources/Props/friend_frog.fbx` + `friend_frog_tex.png`

```text
One single 3D game prop on a plain pure white background: a small round green frog sitting, with a wide smile and big friendly eyes.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small round green frog sitting, with a wide smile and big friendly eyes, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-83 — Little festive tree

*Step 1, ChatGPT or Gemini → save the picture as* `G-83.png` · *Step 2, Tripo image-to-3D → download as* `G-83.glb` · *Becomes:* `Resources/Props/fest_tree.fbx` + `fest_tree_tex.png`

```text
One single 3D game prop on a plain pure white background: a small plump green pine tree in a red pot, decorated with gold stars and pink baubles.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small plump green pine tree in a red pot, decorated with gold stars and pink baubles, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-84 — Snowman

*Step 1, ChatGPT or Gemini → save the picture as* `G-84.png` · *Step 2, Tripo image-to-3D → download as* `G-84.glb` · *Becomes:* `Resources/Props/fest_snowman.fbx` + `fest_snowman_tex.png`

```text
One single 3D game prop on a plain pure white background: a small three-ball snowman with a carrot nose, a teal scarf and stick arms.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a small three-ball snowman with a carrot nose, a teal scarf and stick arms, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-86 — Smiling pumpkin

*Step 1, ChatGPT or Gemini → save the picture as* `G-86.png` · *Step 2, Tripo image-to-3D → download as* `G-86.glb` · *Becomes:* `Resources/Props/fest_pumpkin.fbx` + `fest_pumpkin_tex.png`

```text
One single 3D game prop on a plain pure white background: a round orange pumpkin with a carved friendly smile, not scary.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `a round orange pumpkin with a carved friendly smile, not scary, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

#### G-87 — Colour powder plates

*Step 1, ChatGPT or Gemini → save the picture as* `G-87.png` · *Step 2, Tripo image-to-3D → download as* `G-87.glb` · *Becomes:* `Resources/Props/fest_colours.fbx` + `fest_colours_tex.png`

```text
One single 3D game prop on a plain pure white background: three small brass plates heaped with pink, yellow and blue colour powder.
Shown alone, whole object visible with margin, three-quarter view from slightly above, soft even studio light, no cast shadow on the background, no floor, no other objects, no hands, no text.
Stylised toy look: chunky rounded shapes, thick walls, no thin or wispy parts, no holes smaller than a finger, matte hand-painted flat colours with only soft shading, like a vinyl toy from a children's mobile game. Not realistic.
```

*Tripo text-only fallback:* `three small brass plates heaped with pink, yellow and blue colour powder, chunky stylised toy, rounded, flat matte hand-painted colours, children's mobile game prop, low poly, single object`

## 3D. Website and adverts (publish only at launch)

#### J-03 — Hero poster (website top, social posts)

*Tool:* Gemini · *Save as:* `J-03.png` · *Becomes:* `docs/assets/hero.jpg` · *Final size:* 1920×1080

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint a wide cinematic poster (16:9): Bella in the middle of her playroom surrounded by everything she loves, arranged like a movie poster — a bubbly bathtub with a rubber duck, a doughnut bed, a bowl of kibble, a pile of hats and a crown, a gold trophy, a tiny bluebird on her head, butterflies. Golden afternoon light through the window, sparkles in the air. Bella looks at the viewer, delighted. No text.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### J-04 — Square social post

*Tool:* Gemini · *Save as:* `J-04.png` · *Becomes:* `docs/assets/social_square.jpg` · *Final size:* 1080×1080  
*Note:* Sonnet writes "Coming soon", "Out now" and so on onto the blank sign.

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint a square picture: Bella holding a small blank cream sign in her mouth by its string, sitting on a teal circle, confetti around her. The sign is completely blank. Plain soft cream background. No text.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### J-05 — Eight-second teaser clips for social media

*Tool:* Gemini (Veo video, if your plan includes it) · *Save as:* `J-05.png` · *Becomes:* website and social only · *Final size:* 8 s MP4  
*Note:* Use these for the website and social posts only. A **Play Store** video must show the real game, so record that from the phone instead. Save as `J-05-1.mp4` …

```text
Attached is my character reference image. Animate this exact puppy character in the same soft 3D toy style. [ACTION] Bright cosy playroom, gentle camera push-in, soft sunlight, happy mood, cheerful light music with glockenspiel and ukulele, a small happy puppy bark at the end. No text, no captions, no people.

Run once for each [ACTION]:
1. She wakes up in her lilac doughnut bed, yawns, stretches, then spots the viewer and bounces up wagging.
2. She chases a pink butterfly in a circle, tumbles over, and sits up giggling with the butterfly on her nose.
3. She sits in a bubbly bathtub with a rubber duck on her head, then shakes and foam flies everywhere.
```

---

# TIER 4 — Optional

#### L-01 — Clean turnaround for 3D

*Tool:* Gemini · *Save as:* `L-01.png`

```text
Attached is my character reference. Create a clean character turnaround sheet of this exact puppy for 3D modelling: four full-body views in a row — front, left side, back, and three-quarter — all the same size, standing in the same neutral pose on all four legs. Important for modelling: all four legs clearly separated with gaps between them, tail lifted up and away from the body, ears hanging slightly away from the cheeks, mouth closed, eyes open, teal collar with gold paw tag visible. Soft 3D toy style with matte flat colours and only gentle shading, even lighting with no cast shadows. Plain white background. No text.
```

Then in Tripo: **multi-view image to 3D** if your plan has it (front, side, back cut from the sheet), otherwise the front view alone · texture on · quad mesh if offered · about 15,000 faces · download GLB as `L-01.glb` into `art-inbox/3d/`.
