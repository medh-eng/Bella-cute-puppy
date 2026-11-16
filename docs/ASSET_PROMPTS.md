# Bella — the "big game" asset pack

**What this is:** every prompt needed to replace Bella's script-made placeholder art and sound with
generated art, 3D models and real audio — plus exact instructions for where each file goes and what
code to change. Written 16 Nov 2026 (Fable pass) so that **Pon can generate** and **Sonnet can wire it in**.

- **Part 1** (Packs A–L) is for Pon: copy a prompt, paste it into the named tool, save the result with the ID as its file name.
- **Part 2** is for Claude Sonnet: the build plan, file conventions and code changes.
- **Part 3** is ready-made messages Pon pastes into a Sonnet chat to start each round of work.

About 145 picture and model prompts plus about 70 Pixabay searches. Nobody needs to do them all — the waves in §0.5 say what to do first.

---

## 0. Read this first (Pon)

### 0.1 Which tool for which job

| Job | Use | Why |
|---|---|---|
| Icons, buttons, stickers, anything that must have a **see-through background** | **ChatGPT Plus** | It can save a real transparent PNG. Ask for "transparent background" and download the PNG. |
| Big full-frame pictures: title screen, room backdrops, wallpapers, store art | **Gemini Pro** | Sharper at large sizes and very good at keeping Bella looking like the reference picture. |
| Anything with Bella in it | Either, but **always attach the reference picture first** (§0.3) | Otherwise every picture is a slightly different dog. |
| Logo and anything with words | **ChatGPT Plus** | Better at spelling. |
| 3D models | **Tripo 3D**, image-to-3D | Make the picture in ChatGPT/Gemini first (free), then spend Tripo credits only on pictures you like. |
| Music, sound effects, real puppy sounds | **Pixabay** | Search words are in Pack K. |

If a tool cannot give a transparent background, change the words "Transparent background" to
**"flat solid mid-grey background, colour #808080, with no shadow on it"** — Sonnet's script removes the grey.

### 0.2 Three licence points to settle before launch

1. **Tripo free credits are very likely not allowed in a published game.** Tripo's own guide says the Free plan's 300 monthly credits are "strictly for non-commercial" use, and reviewers report free models are public under a CC BY 4.0 licence with about 15 downloads a month. Commercial rights come with the paid plans (about US$16–20 for one month). **Recommended:** prepare all the Step-1 pictures for free, then pay for a single month and generate everything in that month. Check <https://www.tripo3d.ai/pricing> on the day — do not rely on this paragraph.
2. **Pixabay** music and sound effects are free for commercial use without credit, but may not be re-sold as stand-alone files (shipping them inside the game is fine). Keep the ledger in Pack K anyway — Google Play can ask where audio came from.
3. **ChatGPT and Gemini pictures** may be used commercially under their current terms. Gemini adds an invisible watermark, and sometimes a small visible mark in a corner; if you see one, regenerate that picture in ChatGPT instead.

Because the game is for children: nothing scary, no weapons, no real brands or characters from other games or films, no real people. Every prompt below is already written that way — do not add "in the style of" any named studio or film.

### 0.3 Start every picture chat like this

Open a **new chat**, attach `art-source/faces_2d/expressions_sheet.png` from the repo (or any saved picture of Bella you like best), and send:

```text
This is Bella, the main character of my children's mobile game. She is my own original character. In this chat I will ask for game art. Whenever Bella appears, keep her exactly like this reference: same face, same proportions, same golden-cream colours, same teal collar with the gold paw tag. Always use the same soft 3D toy style for everything so all pictures match. Do not add any text unless I ask. Reply "Ready" and wait.
```

Do one **pack per chat** (Pack C in one chat, Pack D in another). Staying in one chat keeps the style matching; a fresh chat for each pack stops it drifting.

### 0.4 Saving and handing over

1. Download the full-size picture. Do not screenshot.
2. Name it with the prompt's ID: `C-01.png`. A second try you also like: `C-01b.png`.
3. Put everything in the repo folder **`art-inbox/`** — on github.com open the repo → `art-inbox` → *Add file* → *Upload files* → drag them in → *Commit changes*. (Or attach them to the Sonnet chat.) 3D models go in `art-inbox/3d/`, audio in `art-inbox/audio/`.
4. Tell Sonnet using the messages in Part 3.

**Quality check before saving** (ten seconds each): Does it still read clearly when you shrink it to thumbnail size? Is every item complete and not cut off at the edge? No stray letters or numbers? Does Bella look like Bella? For sheets: are there exactly the right number of items, in the right order? If not, reply to the tool with what is wrong ("item 4 is missing, redo the sheet with all 9") rather than starting again.

### 0.5 What to do first

| Wave | Packs | Effort | What changes on screen |
|---|---|---|---|
| **1 — the big jump** | A-01, A-02, A-03 · B-01, B-02 · C-01, C-02, C-07 · K music (5 tracks) + K puppy voice | one evening | Logo and title screen, glossy buttons, real music, a puppy that sounds like a puppy |
| **2 — the room comes alive** | rest of C · D (portraits + stickers) · E (wallpaper, floor, window) · H (sparkles) · G Wave 1 (12 models) · rest of K | 2–3 sittings | Real furniture, illustrated stickers, room themes to buy |
| **3 — more game** | F (new places) · G Waves 2–3 · I (festivals) · J (store and website) · L (optional) | as time allows | Garden, festivals, friends who visit, new store page |

Stop after any wave and the game is still complete — every new file has a fallback to what exists today.


---

## Pack A — Brand and first impression

The first five seconds decide whether a game feels "big". Today the game opens straight into the room with Unity's default font.

#### A-01 — Game logo

*Tool:* ChatGPT · *Save as:* `A-01.png` · *Becomes:* `Resources/UI/logo.png` · *Final size:* 1536×1024 → trimmed, max 1024 wide  
*Note:* Ask for a second version with only the word "Bella" (save as `A-01b.png`) for small spaces.

```text
Design a game logo wordmark that reads exactly: "Bella" in very large letters, with a smaller line under it that reads exactly: "the Cute Puppy".
Fat, bubbly, rounded hand-made letters in warm gold with a cream highlight on top, a thick dark cocoa-brown outline and a soft pink drop shadow. A small pink paw print sits just above the two letters "l", and a tiny teal collar tag with a gold paw hangs from the final "a". A few small gold sparkles around it. Check the spelling carefully: B-e-l-l-a.
Transparent background. Wide image, logo centred with empty margin on all sides. No other text.
Style: premium children's mobile game logo, glossy, toy-like, friendly, not scary, not realistic.
```

#### A-02 — Title screen key art

*Tool:* Gemini · *Save as:* `A-02.png` · *Becomes:* `Resources/UI/title_bg.jpg` · *Final size:* 1080×1920 JPG  
*Note:* Generate 3–4 and pick the one where Bella matches the reference best. This same picture is reused on the website and store.

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint a tall portrait (9:16) title-screen illustration: Bella sitting in the middle of a sunny, cosy playroom, head tilted, tongue out, tail mid-wag, looking straight at the viewer. Behind her: a big window with soft morning light and drifting clouds, pink curtains, a lilac doughnut dog bed, a rose-red ball, a toy shelf, rainbow bunting near the ceiling. Soft light rays and a few floating sparkles.
Composition: keep the top 30% of the picture calm and uncluttered (plain wall and bunting only) because a logo will be placed there, and keep the bottom 15% simple wooden floor because a button will be placed there. Bella's face sits just below the centre.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### A-03 — App icon v2

*Tool:* ChatGPT · *Save as:* `A-03.png` · *Becomes:* `Resources/Portraits/app_icon.png`, `docs/store/app_icon_512.png` · *Final size:* 1024×1024 → 512×512  
*Note:* Also ask: "now give me the same Bella head alone on a transparent background" → save as `A-03b.png` (adaptive icon foreground).

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a square app icon: only Bella's head and the top of her teal collar, facing the viewer, huge happy eyes, open smile with a little pink tongue, one ear slightly lifted. She fills about 70% of the square and is centred, with nothing important in the outer 15% on any side. Background: a soft teal-to-sky-blue radial glow with a faint ring of tiny white paw prints. No border, no rounded-corner mask, no text.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### A-04 — Loading and waiting pictures (set of 3, one request each)

*Tool:* Gemini · *Save as:* `A-04.png` · *Becomes:* `Resources/UI/loading_1.png` … `_3.png` · *Final size:* 768×768  
*Note:* Save as `A-04-1.png`, `A-04-2.png`, `A-04-3.png`.

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint a small cosy spot illustration, square: [SCENE]. Soft round pastel background blob behind her, nothing else. Space around the edges.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.

Run this three times, replacing [SCENE] with:
1. Bella curled up asleep on a lilac cushion with a tiny snore bubble
2. Bella leaping after a pink heart-winged butterfly
3. Bella peeking over the rim of a bubbly bathtub with foam on her head
```

#### A-05 — Studio mark

*Tool:* ChatGPT · *Save as:* `A-05.png` · *Becomes:* `Resources/UI/studio.png`, website header · *Final size:* max 768 wide

```text
Design a small, simple studio logo for a family-friendly games studio. The words read exactly: "Medhaneeka Games". Above the words, a simple emblem: a rounded game-controller shape whose two buttons form a paw print. Colours: teal and gold on transparent background. Clean, modern, rounded letters, flat with a very soft gradient. Check the spelling carefully: M-e-d-h-a-n-e-e-k-a. No other text.
```

#### A-06 — First-day story cards (4 pictures, one request each, same chat)

*Tool:* Gemini · *Save as:* `A-06.png` · *Becomes:* `Resources/Story/intro_1.png` … `_4.png` · *Final size:* 810×1080 JPG  
*Note:* Replaces the plain "This is Bella!" popup on first launch. Save as `A-06-1.png` … `A-06-4.png`.

```text
Attached is my character reference. Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Paint story card [N] of 4 for a wordless picture story, tall portrait (3:4), rounded-corner storybook feel, same style and same room in every card:
[SCENE]
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.

Scenes:
1. A big cardboard box with air holes and a pink ribbon sits on a doormat in a sunny hallway; two golden ear tips and a wagging tail poke out of the top.
2. The box has tipped over; Bella tumbles out upside down, surprised, ribbon on her head.
3. Bella looks up at the viewer with huge hopeful eyes, one paw raised, a small heart floating above her.
4. Bella asleep in a lilac doughnut bed in the playroom at dusk, the empty box beside her now holding her toys.
```


---

## Pack B — UI skin (buttons, panels, bars)

The code colours buttons itself, so most of these are asked for in **white and light grey** — the game tints them teal, pink, gold and so on. Do not ask for coloured versions.

#### B-01 — Jelly buttons and badges (tintable)

*Tool:* ChatGPT · *Save as:* `B-01.png` · *Becomes:* `Resources/UI/btn_round.png`, `btn_pill.png`, `btn_square.png`, `badge.png` · *Final size:* 512 px each (pill 768×256)

```text
Create a sheet of 4 blank game-UI shapes, all in pure white with very soft light-grey shading only (they will be colour-tinted later), each with a glossy highlight along the top and a slightly darker lower edge so they look like raised jelly buttons:
1. a perfect circle button
2. a wide pill-shaped button (about 3 times wider than tall)
3. a rounded square button
4. a small circular badge with a scalloped edge
Layout: 2 by 2 grid, wide gaps, nothing touching, flat front view, no perspective, perfectly symmetrical. No icons, no text. Transparent background. Square image.
```

#### B-02 — Popup panel frame (9-slice)

*Tool:* ChatGPT · *Save as:* `B-02.png` · *Becomes:* `Resources/UI/panel_popup.png` · *Final size:* 512×512, border 96 px  
*Note:* The edges must be uniform so the game can stretch it to any size.

```text
Create one blank game-UI popup panel, flat front view, perfectly symmetrical: a rounded rectangle of warm cream paper (#FFF5DE) with a completely plain, flat, empty centre. Around the edge: a thick teal border with a thin row of even white stitches inside it, identical on all four sides. Corners are large and round and all four are identical. A very soft shadow underneath. No icons, no text, no decoration in the centre. Transparent background. Square image, the panel fills 85% of it.
```

#### B-03 — Ribbon header and small parts

*Tool:* ChatGPT · *Save as:* `B-03.png` · *Becomes:* `Resources/UI/ribbon.png`, `tag_price.png`, `tag_owned.png`, `tag_new.png`, `bubble_speech.png`, `bubble_thought.png` · *Final size:* ribbon 768 wide, others 256–384

```text
Create a sheet of 6 blank game-UI parts, soft 3D toy look, each separate with wide gaps, flat front view, no text anywhere:
1. a wide rose-red ribbon banner with folded ends (blank, for a title)
2. a gold price tag with a string hole (blank)
3. a leaf-green "owned" rosette with a white tick
4. a pink "new" starburst (blank)
5. a white speech bubble with a tail at the bottom left
6. a white thought bubble made of a cloud and two small circles
Layout: 2 columns by 3 rows. Transparent background. Tall image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### B-04 — Meters and progress bars

*Tool:* ChatGPT · *Save as:* `B-04.png` · *Becomes:* `Resources/UI/ring.png`, `ring_full.png`, `bar_bone.png`, `bar_bone_fill.png`, `shield.png` · *Final size:* ring 256, bars 768×192

```text
Create a sheet of 5 blank game-UI parts, flat front view, each separate with wide gaps, no text:
1. an empty round meter ring: a thick cream ring with a soft inner shadow, empty hole in the middle (transparent)
2. the same ring glowing soft gold (for a full meter)
3. an empty horizontal progress bar shaped like a long dog bone: cream outline, hollow pale inside
4. a matching bone-shaped fill in solid white with light grey shading (it will be tinted)
5. a small gold star-shaped level shield, blank
Layout: single column for the two bones, the three round ones in a row above. Transparent background.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### B-05 — Shop card and shelf

*Tool:* ChatGPT · *Save as:* `B-05.png` · *Becomes:* `Resources/UI/card_shop.png`, `shelf.png`, `pedestal.png` · *Final size:* card 384×512, shelf 768×128, pedestal 384

```text
Create a sheet of 3 blank game-UI parts for a toy-shop screen, flat front view, wide gaps, no text:
1. a tall rounded shop card in white with a pale pink inner panel and a little scalloped awning in pink and white stripes across its top; the centre is empty
2. a horizontal wooden shelf plank with two small brackets, light honey wood
3. a round spotlight pedestal in lilac with a soft glow on top (for showing the selected item)
Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### B-06 — Daily present, three states

*Tool:* ChatGPT · *Save as:* `B-06.png` · *Becomes:* `Resources/UI/gift_closed.png`, `gift_opening.png`, `gift_open.png` · *Final size:* 512 each

```text
Create a sheet of 3 pictures of the same present box in a row, same size and angle, wide gaps:
1. closed pink present with gold ribbon and bow, gently shaking with two motion marks
2. the same present with its lid popping up and golden light bursting out
3. the open box, lid beside it, with gold sparkles and three paw-print coins flying out
Transparent background. Wide image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### B-07 — Seven-day reward tiles

*Tool:* ChatGPT · *Save as:* `B-07.png` · *Becomes:* `Resources/UI/day_tile.png`, `day_tile_big.png`, `stamp.png` · *Final size:* 256 / 384 / 256

```text
Create a sheet of 8 blank game-UI tiles in a 4 by 2 grid, flat front view, wide gaps, no text and no numbers:
1–6. six identical small rounded cream tiles with a teal top tab (calendar-page look), empty centre
7. one larger gold tile with a tiny crown on its tab and a soft glow (the big day-7 prize)
8. a leaf-green round stamp with a white paw print (the "collected" stamp)
Transparent background.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### B-08 — Tutorial hands

*Tool:* ChatGPT · *Save as:* `B-08.png` · *Becomes:* `Resources/UI/hand_tap.png`, `hand_hold.png`, `hand_swipe.png`, `hand_circle.png` · *Final size:* 384 each

```text
Create a sheet of 4 pictures in a 2 by 2 grid, wide gaps: a cute white cartoon glove hand with a cocoa-brown outline, shown 1) pointing and tapping with a small ring ripple at the fingertip, 2) pressing and holding with two rings, 3) swiping with a curved motion trail, 4) drawing a circle with a dotted trail. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```


---

## Pack C — Icons (10 sheets of 9 = 90 icons)

Each sheet replaces the script-drawn icons of the same file name, plus new ones for later features. Order matters: the list order is how the files get named.

#### C-01 — Care buttons (replaces the 5 big action buttons and need meters)

*Tool:* ChatGPT · *Save as:* `C-01.png` · *Becomes:* `Resources/Icons/` → `bone`, `bath`, `zzz`, `ball`, `tricks`, `heart`, `health`, `bubbles`, `treat` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a chunky cream dog bone with a tiny bite mark
2. a white claw-foot bathtub overflowing with bubbles
3. a lilac crescent-moon pillow with three small sleep bubbles rising
4. a rose-red bouncy ball with a gold star band
5. a gold magic wand with a star tip and sparkles
6. a plump glossy pink heart
7. a white heart with a rose-red plus sign in the middle
8. a cluster of three shiny soap bubbles with rainbow glints
9. a round golden biscuit coin stamped with a paw print
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-02 — Menu buttons

*Tool:* ChatGPT · *Save as:* `C-02.png` · *Becomes:* `Resources/Icons/` → `gift`, `book`, `wardrobe`, `clinic`, `gear`, `home`, `trophy`, `star`, `paw` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a pink present box with a gold ribbon bow, lid slightly lifted with light coming out
2. a teal sticker album with a gold star on the cover
3. a small white wardrobe with a pink bow hanging on its open door
4. a cheerful white doctor's bag with a rose-red heart on it
5. a soft rounded lilac settings cog
6. a tiny cream house with a teal roof and a heart-shaped window
7. a gold trophy cup with a paw print on it
8. a fat glossy gold star with a happy shine
9. a pink and cream paw print
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-03 — Food (dog-safe foods only)

*Tool:* ChatGPT · *Save as:* `C-03.png` · *Becomes:* `Resources/Icons/` → `food_kibble`, `food_treat`, `food_fancy`, `cake`, `food_carrot`, `food_watermelon`, `food_popsicle`, `food_water`, `food_pumpkin` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a rose-red bowl heaped with brown kibble
2. a bone-shaped biscuit with a small heart cut out
3. a fancy plate of chicken, rice and carrot under a lifted silver dome
4. a small pink birthday cake with one candle and a bone on top
5. a crunchy orange carrot with green leaves
6. a smiling slice of watermelon without seeds
7. a peanut-butter and banana frozen pup-sicle on a bone-shaped stick
8. a sky-blue bowl of fresh water with a droplet splashing
9. a little bowl of orange pumpkin mash with a sprig on top
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-04 — Bath and vet

*Tool:* ChatGPT · *Save as:* `C-04.png` · *Becomes:* `Resources/Icons/` → `sponge`, `shower`, `towel`, `brush`, `drop`, `thermometer`, `medicine`, `bandage`, `stethoscope` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a yellow bath sponge covered in foam
2. a teal shower head spraying soft water drops
3. a fluffy folded pink towel with a paw embroidered on it
4. a wooden pet brush with soft bristles
5. a single shiny water drop
6. a chunky friendly thermometer with a heart at the tip
7. a small bottle of pink syrup with a spoon
8. a plaster with tiny hearts printed on it
9. a teal stethoscope curled into a heart shape
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-05 — Wardrobe items

*Tool:* ChatGPT · *Save as:* `C-05.png` · *Becomes:* `Resources/Icons/` → `item_bow`, `item_bandana`, `item_partyhat`, `item_hat`, `item_flower`, `item_glasses`, `item_cape`, `item_crown`, `item_scarf` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a pink hair bow
2. a rose-red triangular bandana with white dots
3. a sky-blue cone party hat with a gold pom-pom
4. a little plum top hat with a rose band
5. a white daisy with a gold centre
6. round cocoa-brown glasses
7. a rose-red hero cape with a gold star clasp
8. a gold crown with rose gems
9. a cosy striped teal and cream scarf
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-06 — Sky and nature

*Tool:* ChatGPT · *Save as:* `C-06.png` · *Becomes:* `Resources/Icons/` → `sun`, `moon`, `cloud`, `rainbow`, `butterfly`, `sparkle`, `flower`, `leaf`, `raincloud` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a smiling golden sun with soft rounded rays
2. a sleepy crescent moon wearing a tiny nightcap
3. a fluffy white cloud
4. a pastel rainbow with a cloud at each end
5. a butterfly with heart-shaped pink and lilac wings
6. a four-point white-gold sparkle
7. a pink tulip with two leaves
8. a fresh green leaf
9. a soft grey-blue cloud with three raindrops
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-07 — System buttons (simple, bold, readable when tiny)

*Tool:* ChatGPT · *Save as:* `C-07.png` · *Becomes:* `Resources/Icons/` → `back`, `close`, `check`, `lock`, `question`, `sound_on`, `sound_off`, `camera`, `shop` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a fat rounded arrow pointing left
2. a fat rounded X cross
3. a fat rounded tick mark in leaf green
4. a gold padlock with a heart-shaped keyhole
5. a round lilac speech bubble with a paw print inside
6. a teal loudspeaker with two sound waves
7. a teal loudspeaker with a small rose-red cross beside it
8. a cute pink toy camera with a big lens
9. a striped shop awning over a little counter
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-08 — New outfit icons (for the 3D outfit pack G-41 onwards)

*Tool:* ChatGPT · *Save as:* `C-08.png` · *Becomes:* `Resources/Icons/` → `item_sunhat`, `item_beanie`, `item_headphones`, `item_backpack`, `item_tiara`, `item_chefhat`, `item_piratehat`, `item_wizardhat`, `item_sunglasses` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a straw sun hat with a pink ribbon
2. a knitted teal beanie with a cream pom-pom
3. chunky pink headphones
4. a tiny sky-blue backpack with a bone keyring
5. a silver tiara with a pink heart gem
6. a white chef's hat
7. a friendly pirate hat with a bone-and-paw badge
8. a lilac wizard hat covered in gold stars
9. heart-shaped rose sunglasses
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-09 — New places and features

*Tool:* ChatGPT · *Save as:* `C-09.png` · *Becomes:* `Resources/Icons/` → `place_garden`, `place_park`, `place_kitchen`, `place_bathroom`, `place_stage`, `photo`, `calendar`, `map`, `music` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a green garden gate with flowers and a tiny doghouse behind it
2. a park tree with a yellow slide beside it
3. a cute pastel fridge with a paw magnet
4. a round mirror above a tiny sink with a rubber duck
5. red theatre curtains with a spotlight and a star
6. an instant photo frame showing a pink heart
7. a tear-off calendar page with a gold star on it
8. a folded treasure map with a paw-print trail
9. two bouncing pink music notes
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### C-10 — Toys

*Tool:* ChatGPT · *Save as:* `C-10.png` · *Becomes:* `Resources/Icons/` → `toy_rope`, `toy_duck`, `toy_teddy`, `toy_frisbee`, `toy_plushbone`, `toy_tennis`, `toy_bubblewand`, `toy_kite`, `toy_squeaky` · *Final size:* 256×256 each

```text
Create a sheet of 9 game icons for a children's puppy-care game.
1. a knotted rainbow rope tug toy
2. a yellow rubber duck
3. a small brown teddy bear with a teal bow
4. an orange frisbee with a paw print
5. a soft blue plush bone with stitches
6. a fuzzy lime tennis ball
7. a bubble wand blowing three bubbles
8. a diamond kite with a ribbon tail
9. a pink squeaky toy shaped like a doughnut
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```


---

## Pack D — Bella in 2D (portraits, stickers, story art)

Attach the reference picture first (§0.3). These are what make the popups, sticker book and shop feel hand-illustrated.

> Do **not** try to regenerate the six face textures in `Resources/Bella/Faces/` with AI — those are painted onto the 3D model's own texture layout and only the pipeline script can make them.

#### D-01 — Portrait set 1 (replaces the 6 popup portraits, adds 3)

*Tool:* ChatGPT or Gemini · *Save as:* `D-01.png` · *Becomes:* `Resources/Portraits/` → `face_happy`, `face_excited`, `face_sad`, `face_sleepy`, `face_begging`, `face_silly`, `face_love`, `face_proud`, `face_surprised` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 head-and-shoulders portraits of Bella, all facing the viewer, same size, same angle:
1. happy: open smile, sparkling eyes
2. excited: huge grin, star-shaped highlights in her eyes, tongue out
3. gently sad: droopy ears and big wet eyes, small frown (soft, not distressing)
4. sleepy: half-closed eyes and a tiny yawn
5. begging: enormous watery puppy eyes, chin lowered
6. silly: eyes squeezed shut in a grin, tongue sideways, a blob of foam on her head
7. in love: heart-shaped highlights in her eyes, blushing cheeks
8. proud: chin up, eyes closed, smug little smile, a gold star beside her
9. surprised: round eyes, small round mouth, ears lifted
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### D-02 — Portrait set 2 (9 new moods)

*Tool:* ChatGPT or Gemini · *Save as:* `D-02.png` · *Becomes:* `Resources/Portraits/` → `face_hungry`, `face_dirty`, `face_sniffles`, `face_clean`, `face_playful`, `face_party`, `face_chilly`, `face_summer`, `face_goodnight` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 head-and-shoulders portraits of Bella, all facing the viewer, same size, same angle:
1. hungry: licking her lips, eyes looking up hopefully
2. muddy: a mud smudge on her nose and cheek, cheeky grin
3. under the weather: a little thermometer in her mouth and a cosy scarf, still calm and brave
4. freshly bathed: fluffy fur, sparkles around her, a towel on her head
5. playful: a rose-red ball held in her mouth, eyebrows raised
6. party: a sky-blue party hat, confetti on her head, laughing
7. chilly: wearing a teal beanie, a snowflake on her nose
8. summer: heart-shaped sunglasses, tongue out
9. goodnight: eyes closed, peaceful smile, a tiny moon and star beside her
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### D-03 — Sticker sheet 1 — firsts

*Tool:* ChatGPT or Gemini · *Save as:* `D-03.png` · *Becomes:* `Resources/Stickers/` → `first_pet`, `first_meal`, `first_bath`, `first_nap`, `first_fetch`, `first_trick`, `all_tricks`, `first_outfit`, `fashion_5` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 collectible stickers showing Bella's whole body in little scenes. Each sticker is a compact, die-cut shape with everything inside one thick white sticker border and a soft shadow, like real vinyl stickers.
1. Bella being stroked by a small cartoon hand, eyes closed in bliss, hearts above
2. Bella with her face in a rose-red bowl, kibble crumbs flying
3. Bella in a tub of bubbles with a rubber duck on her head
4. Bella asleep on her back in a lilac bed, paws in the air
5. Bella running back proudly with a rose-red ball in her mouth
6. Bella sitting up nicely with one paw raised, a gold star above
7. Bella on a tiny podium wearing a gold medal, confetti falling
8. Bella admiring herself in a small mirror wearing a pink bow
9. Bella striking a pose in a crown, cape and sunglasses
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### D-04 — Sticker sheet 2 — milestones

*Tool:* ChatGPT or Gemini · *Save as:* `D-04.png` · *Becomes:* `Resources/Stickers/` → `first_checkup`, `first_gift`, `streak_3`, `streak_7`, `streak_14`, `streak_30`, `grown_1`, `grown_2`, `cuddles_50` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 collectible stickers showing Bella's whole body in little scenes. Each sticker is a compact, die-cut shape with everything inside one thick white sticker border and a soft shadow, like real vinyl stickers.
1. Bella sitting bravely while a stethoscope rests on her chest, a heart plaster on her paw
2. Bella popping out of a pink present box with ribbon on her ear
3. Bella holding up three gold stars on a string
4. Bella sliding down a pastel rainbow
5. Bella sunbathing under a big smiling sun
6. Bella hugging a big pink heart-shaped cake
7. Bella standing next to a height chart giraffe, stretching up on tiptoe
8. Bella looking surprised that her old tiny collar no longer fits, new bigger collar on
9. Bella buried in a pile of fifty tiny pink hearts, only her face showing
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

#### D-05 — Sticker sheet 3 — friends and new adventures

*Tool:* ChatGPT or Gemini · *Save as:* `D-05.png` · *Becomes:* `Resources/Stickers/` → `butterfly_1`, `butterfly_20`, `friend_bird`, `friend_snail`, `garden_visit`, `rainy_day`, `first_photo`, `night_owl`, `first_story` · *Final size:* 512×512 each

```text
Bella is exactly the puppy in the attached reference image: a baby golden retriever with an oversized round head, very large low-set glossy brown eyes, a tiny dark nose, a short muzzle, floppy honey-coloured ears, golden-cream fur with a cream chest and muzzle, short chubby legs, big paws, a small fluffy tail, and a soft teal collar with a small gold paw-shaped tag. Keep her identical to the reference in every picture.
Create a sheet of 9 collectible stickers showing Bella's whole body in little scenes. Each sticker is a compact, die-cut shape with everything inside one thick white sticker border and a soft shadow, like real vinyl stickers.
1. Bella going cross-eyed at a pink butterfly sitting on her nose
2. Bella lying in flowers surrounded by many butterflies
3. Bella nose to beak with a tiny round bluebird on a window sill
4. Bella lying flat watching a snail with a pink shell
5. Bella rolling in garden grass with a daisy in her mouth
6. Bella under a too-big yellow umbrella, splashing in a puddle
7. Bella posing in front of a toy camera with a flash sparkle
8. Bella watching fireflies through the window at night
9. Bella peeking out of a cardboard box with a pink ribbon
Layout: a tidy 3 by 3 grid of 9 separate pictures in this exact reading order (left to right, top row first). Each one sits centred in its own cell with wide empty gaps around it; nothing overlaps or touches; all are the same size and seen from the same angle (front view, very slightly from above). Each has a thick cream-white sticker outline. Transparent background. Square image.
Style: premium children's mobile game art with a soft 3D-rendered toy look, like polished vinyl and clay. Rounded chunky shapes, smooth gradients, gentle light from the top left with one soft glossy highlight, no hard black outlines. Warm pastel palette: cream #FFF5DE, teal #7DD6C4, pink #FF96B0, gold #FFD640, sky blue #A8D9FA, orange #FFAB59, lilac #BDA8F2, leaf green #78D178, rose red #F05461, and dark cocoa brown #4A3024 wherever black would normally be used. Cosy and friendly, never scary, never photorealistic. No text, no letters, no numbers, no watermark, no logos.
```

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

## Pack E — Room surfaces: wallpapers, floors, rugs, window views

These turn the single plain room into **room themes the child can buy with treats** — the cheapest way to add a lot of content. All use Gemini (or ChatGPT) with no reference picture needed.

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


---

## Pack F — New places (backdrops)

Each place is one wide painting that stands where the back wall is, plus a floor from Pack E and a few models from Pack G. The camera only shows the middle of the painting on a phone, so the important things must be in the centre.

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


---

## Pack G — 3D models (Tripo)

Today every piece of furniture is built from cubes and spheres in code. Real models are the single biggest step towards looking like a big studio game.

**How to make each one (two steps — this saves credits):**

1. **Picture first.** Paste the prompt into ChatGPT or Gemini. Regenerate until you like it — this costs no Tripo credits. Save as `G-01.png`.
2. **Tripo → Image to 3D.** Upload that picture. Settings: newest model version · texture **on** · if there is a style choice pick the plain/default one (not "realistic") · if there is a low-poly or "smart low poly" switch, turn it on · if it asks for a face/polygon limit, enter **8000**. Generate, spin it round, check the back. Download as **GLB** → `G-01.glb` → upload to `art-inbox/3d/`.

**Reject a model if:** parts are fused that should be separate, the back is a smeared copy of the front, there are floating bits, or it has holes. Re-rolling the *picture* is free; re-rolling the *model* is not — fix the picture first (plainer background, more "chunky", fewer thin parts).

**Do not generate with Tripo:** Bella herself (see Pack L), anything cloth-like that must bend (capes, bandanas, scarves — these stay code-made and can be recoloured for free), water, or flat things like rugs and pictures (Pack E covers those).

Every model below is optional: if a file is missing the game keeps drawing today's simple shape.


### Wave 1 — do these first (12 models, fits roughly one month of free credits)

| ID | File | Model | Size in game | Where it goes |
|---|---|---|---|---|
| G-01 | `bed` | Dog bed | 1.25 m | Room.cs bed (BedBase/BedCushion) |
| G-02 | `bowl` | Food bowl | 0.46 m | Room.cs Bowl/BowlInside |
| G-03 | `kibble` | Kibble mound | 0.34 m | Room.cs Kibble (shown when the bowl is full) |
| G-04 | `ball` | Ball | 0.22 m | Room.cs Ball/BallBand |
| G-05 | `tub` | Bathtub | 1.55 m | Room.cs Tub (keep the code-made water disc inside) |
| G-06 | `plant` | Potted plant | 0.75 m | Room.cs Pot/Leaf1-3 |
| G-07 | `toyshelf` | Toy shelf | 0.9 m | Room.cs BuildToyShelf |
| G-08 | `basket` | Toy basket | 0.55 m | Room.cs BuildToyBasket |
| G-16 | `toy_duck` | Rubber duck | 0.2 m | floats in the tub during Bubble Bath |
| G-43 | `hat_crown` | Crown | 0.22 m | Accessories.cs 'crown' |
| G-71 | `friend_bird` | Pip the bluebird | 0.16 m | new Friends.cs: lands on the window sill in the morning |

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


### Wave 2

| ID | File | Model | Size in game | Where it goes |
|---|---|---|---|---|
| G-09 | `armchair` | Armchair | 0.9 m | new decor, left wall |
| G-10 | `nightlight` | Star night-light | 0.6 m | Ambience.cs nightlight (keep the code glow) |
| G-11 | `waterbowl` | Water bowl | 0.4 m | new, beside the food bowl |
| G-12 | `windowbox` | Window flower box | 1.5 m | on the WindowSill |
| G-15 | `toy_rope` | Rope toy | 0.3 m | fetch / play variants |
| G-17 | `toy_teddy` | Teddy bear | 0.3 m | bed companion at night |
| G-18 | `toy_frisbee` | Frisbee | 0.26 m | park fetch (v1.1) |
| G-21 | `food_treat` | Bone biscuit | 0.2 m | Feeding shop, placed on the bowl |
| G-22 | `food_fancy` | Fancy meal | 0.42 m | Feeding shop |
| G-23 | `food_cake` | Birthday cake | 0.4 m | birthday / streak rewards |
| G-27 | `shampoo` | Shampoo bottle | 0.3 m | Bath activity |
| G-28 | `sponge` | Sponge | 0.2 m | Bath activity |
| G-31 | `vetbag` | Doctor's bag | 0.4 m | Clinic activity |
| G-41 | `hat_top` | Top hat | 0.34 m | Accessories.cs 'tophat' |
| G-42 | `hat_party` | Party hat | 0.3 m | Accessories.cs 'partyhat' |
| G-44 | `hat_sun` | Sun hat | 0.4 m | new outfit |
| G-45 | `hat_beanie` | Beanie | 0.26 m | new outfit |
| G-49 | `bow` | Hair bow | 0.26 m | Accessories.cs 'bow' |
| G-50 | `glasses` | Round glasses | 0.3 m | Accessories.cs 'glasses' |
| G-61 | `doghouse` | Doghouse | 1.2 m | Garden place |
| G-62 | `tree` | Garden tree | 2.2 m | Garden place |
| G-64 | `flowers` | Flower patch | 0.5 m | Garden place, repeated |
| G-72 | `friend_snail` | Momo the snail | 0.18 m | new Friends.cs: crosses the rug on rainy days |
| G-81 | `fest_diya` | Clay lamp | 0.14 m | Festival of Lights decor, 5 along the baseboard |
| G-82 | `fest_lantern` | Paper star lantern | 0.5 m | Festival of Lights, hangs by the window |
| G-85 | `fest_balloons` | Balloon bunch | 1.0 m | Birthday |
| G-88 | `fest_gifts` | Present stack | 0.5 m | Birthday / winter / daily gift |

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


### Wave 3

| ID | File | Model | Size in game | Where it goes |
|---|---|---|---|---|
| G-13 | `rocking` | Rocking horse toy | 0.6 m | new decor |
| G-14 | `bookcase` | Low bookcase | 1.0 m | new decor, right wall |
| G-19 | `toy_plushbone` | Plush bone | 0.3 m | play variants |
| G-20 | `toy_tennis` | Tennis ball | 0.2 m | fetch variant |
| G-24 | `food_carrot` | Carrot | 0.25 m | Feeding shop |
| G-25 | `food_watermelon` | Watermelon slice | 0.25 m | Feeding shop |
| G-26 | `food_popsicle` | Pup-sicle | 0.22 m | Feeding shop, summer |
| G-29 | `showerhead` | Shower head | 0.3 m | Bath activity |
| G-30 | `towel` | Folded towel | 0.35 m | Bath activity |
| G-32 | `thermometer` | Thermometer | 0.25 m | Clinic activity |
| G-33 | `medicine` | Medicine | 0.22 m | Clinic activity |
| G-34 | `stethoscope` | Stethoscope | 0.3 m | Clinic activity |
| G-46 | `hat_chef` | Chef hat | 0.28 m | new outfit |
| G-47 | `hat_pirate` | Pirate hat | 0.36 m | new outfit |
| G-48 | `hat_wizard` | Wizard hat | 0.34 m | new outfit |
| G-51 | `sunglasses` | Heart sunglasses | 0.3 m | new outfit |
| G-52 | `headphones` | Headphones | 0.34 m | new outfit |
| G-53 | `tiara` | Tiara | 0.24 m | new outfit |
| G-54 | `backpack` | Backpack | 0.3 m | new outfit (Back anchor) |
| G-63 | `fence` | Fence piece | 1.5 m | Garden place, repeated |
| G-65 | `wateringcan` | Watering can | 0.35 m | Garden mini-game (v1.1) |
| G-66 | `mushroom` | Mushroom stool | 0.35 m | Garden decor |
| G-73 | `friend_ladybird` | Dot the ladybird | 0.1 m | new Friends.cs: sits on the plant |
| G-74 | `friend_frog` | Hoppy the frog | 0.16 m | Garden / rainy days |
| G-83 | `fest_tree` | Little festive tree | 0.9 m | Winter holidays |
| G-84 | `fest_snowman` | Snowman | 0.6 m | Winter holidays, garden |
| G-86 | `fest_pumpkin` | Smiling pumpkin | 0.35 m | Harvest party |
| G-87 | `fest_colours` | Colour powder plates | 0.4 m | Festival of Colours |

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


---

## Pack H — Sparkle: particles and effects

Small, but it is what makes taps feel rewarding. Ask for **white or very pale** shapes where noted — the game tints them.

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


---

## Pack I — Festivals and seasons

Something big games do and small ones do not: the room **changes with the calendar**. The game reads only the phone's date (no internet), hangs the decorations for a few days, plays a festive tune and offers a festival sticker. Kept to lights, colours, kites, food and weather — no religious figures or symbols, no flags — so it is welcoming for every family and safe for store review.

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


---

## Pack J — Store page, website and adverts

The Play listing is where "looks like a big game" matters most: most people decide from the icon and the first two screenshots.

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

## Pack K — Music and sound from Pixabay

No prompts here — Pixabay is a library, so these are **search words** and what to listen for. Go to pixabay.com → *Music* (or *Sound Effects*) → search → listen → *Download*.

**Rename each download to the file name in the first column** (keep `.mp3`), upload to `art-inbox/audio/`, and add one line per file to `art-inbox/audio/LEDGER.txt`:

```text
music_day.mp3 | "Track title on Pixabay" | artist name | https://pixabay.com/music/... | downloaded 2026-11-20
```

### K1 — Music

Rules for every track: **instrumental only** (no singing, no talking) · one mood from start to finish · no long quiet intro · 1–3 minutes · sounds original, not like a famous song or film tune · gentle enough to hear for ten minutes without getting annoyed. Instruments that suit Bella: ukulele, glockenspiel, marimba, pizzicato strings, soft piano, whistling, music box. If the track's page shows a "Content ID" notice, pick another — it can cause claims on YouTube videos of the game.

| File name | Plays when | Search words to try | Listen for |
|---|---|---|---|
| `music_title` | Title screen | happy ukulele kids · cute intro playful | A bright, hummable tune — this is the game's "theme" |
| `music_morning` | 6 am–12 noon | gentle morning acoustic · sunny morning ukulele | Fresh, light, unhurried |
| `music_day` | 12 noon–5 pm | playful children marimba · happy kids pizzicato | Bouncy, medium speed |
| `music_evening` | 5 pm–8 pm | cozy acoustic guitar warm · gentle waltz | Golden, relaxed |
| `music_night` | 8 pm–6 am | lullaby music box · baby sleep lullaby soft | Very soft and slow; nothing sudden |
| `music_play` | All mini-games | quirky funny cartoon · playful comedy fast | Quick and giggly |
| `music_bath` | Bubble bath | cute bossa nova · bubbly playful light | Splashy, light |
| `music_shop` | Wardrobe and food shop | kawaii cute shop · cute cafe | Sweet, a little stylish |
| `music_garden` | Garden and park | spring flute happy · pastoral cheerful | Open-air, birdsong-friendly |
| `music_stage` | Tricks | circus fun kids · ragtime piano playful | Show-time |
| `music_fest_lights` | Festival of Lights | indian festive instrumental happy · sitar tabla joyful | Warm and celebratory |
| `music_fest_colours` | Festival of Colours | indian dhol happy instrumental · bollywood style upbeat instrumental | Energetic but not harsh |
| `music_fest_winter` | Winter holidays | cozy winter bells · christmas sleigh bells original | Choose an original tune, not a known carol |
| `music_fest_party` | Birthday, New Year | kids party upbeat · birthday party fun instrumental | Choose an original tune |

Short jingles (2–5 seconds, from *Sound Effects*): `jingle_win` (success fanfare short) · `jingle_levelup` (level up) · `jingle_gift` (magic reveal) · `jingle_sticker` (achievement chime) · `jingle_goodnight` (music box short).

### K2 — Bella's voice (the biggest upgrade in the whole pack)

Search in *Sound Effects*. Choose recordings of a **small dog or puppy**, close and clean — no traffic, no people talking, no music, no echo. Short is good. Download **three different ones** for the starred rows and name them `_1`, `_2`, `_3` so she never sounds like a tape loop.

| File name | Search words | Pick the one that sounds… |
|---|---|---|
| `bark_1/2/3` ★ | puppy bark · small dog bark single | cute and high, one or two barks |
| `bark_happy_1/2/3` ★ | happy dog bark · excited puppy | bouncy, playful |
| `yip_1/2/3` ★ | puppy yip · puppy squeak excited | tiny and delighted — **never** like a cry of pain |
| `whine_1/2` | puppy whine · puppy whimper soft | asking, not distressed |
| `whine_sad` | sad puppy whine | gentle; keep it short |
| `whimper_hungry` | puppy whimper begging | pleading, cute |
| `yawn_sleepy` | dog yawn | squeaky yawn |
| `snore` | dog snoring · puppy sleeping breathing | soft, slow, loopable |
| `sneeze` | dog sneeze | small and funny |
| `munch` | dog eating kibble · dog crunching food | crunchy, about 2 seconds |
| `lap` | dog drinking water | lapping, 2 seconds |
| `pant` | dog panting | happy panting, loopable |
| `sniff` | dog sniffing | quick curious sniffs |
| `shake` | dog shaking · dog collar jingle | whole-body shake after the bath |
| `growl_play` | puppy play growl | silly tug-of-war growl, not angry |
| `howl` | puppy howl cute | a tiny "aroo" |
| `paws` | dog running wood floor · dog paws tapping | little claws on a wooden floor |
| `lick` | dog licking | short |

### K3 — Game sounds

| File name | Used for | Search words |
|---|---|---|
| `tap` | every button | ui pop · bubble click soft |
| `pop` | bubbles bursting | bubble pop |
| `coin` | earning treats | coin collect · game coin |
| `chime` | a need is full | magic chime · bell ding soft |
| `sparkle` | clean, shiny | sparkle magic · twinkle |
| `success` | activity finished | success · win short happy |
| `oops` | wrong gesture | cartoon boing soft · gentle wrong |
| `whoosh` | screens sliding | swoosh · whoosh soft |
| `splash` | bath | water splash small |
| `bubbles` | bath | bubbles water · blowing bubbles |
| `gift` | daily present | gift open · magic reveal |
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

### K4 — Background atmosphere (loops, 30–60 seconds)

| File name | Search words |
|---|---|
| `amb_birds` | morning birds garden · birdsong calm |
| `amb_crickets` | crickets night calm |
| `amb_rain` | rain on window gentle · soft rain |
| `amb_garden` | summer meadow breeze birds |
| `amb_beach` | gentle waves beach calm |

### K5 — Optional: a theme tune of your own

If your Gemini app offers music making, these give Bella a tune no other game has. Check the tool's terms for commercial use before shipping one; Pixabay remains the safe default.

```text
A 60-second instrumental theme for a children's puppy game. Ukulele strum, glockenspiel melody, soft hand claps, pizzicato bass, a playful whistle doubling the tune. Bright, bouncy, 110 beats per minute, major key, simple melody a child could hum. No vocals. Ends so it can loop back to the start smoothly.
```
```text
A 90-second instrumental lullaby for a sleeping puppy. Music box and soft felt piano, warm pad underneath, very slow, about 60 beats per minute, no percussion, no sudden notes, same gentle volume throughout. No vocals. Loops smoothly.
```
```text
A 45-second instrumental mini-game tune: fast, giggly, xylophone and pizzicato strings with a kazoo-like lead and light snare brushes, 140 beats per minute, cartoon chase feeling but friendly. No vocals. Loops smoothly.
```
```text
A 60-second festive instrumental for a festival of lights in a children's game: bright sitar-style plucked melody, tabla and hand bells, bansuri flute answering phrases, joyful and warm, 105 beats per minute. No vocals. Loops smoothly.
```

---

## Pack L — Optional: a new Bella model

**Recommendation: do not do this before launch.** Bella's present model already has a hand-built skeleton, 15 animations and face textures, all checked. A new mesh means re-running the whole rig pipeline and re-checking everything. Do it only if, after Waves 1–2, Bella herself looks plainer than her new surroundings — and give that job to Opus, not Sonnet.

#### L-01 — Clean turnaround for 3D

*Tool:* Gemini · *Save as:* `L-01.png`

```text
Attached is my character reference. Create a clean character turnaround sheet of this exact puppy for 3D modelling: four full-body views in a row — front, left side, back, and three-quarter — all the same size, standing in the same neutral pose on all four legs. Important for modelling: all four legs clearly separated with gaps between them, tail lifted up and away from the body, ears hanging slightly away from the cheeks, mouth closed, eyes open, teal collar with gold paw tag visible. Soft 3D toy style with matte flat colours and only gentle shading, even lighting with no cast shadows. Plain white background. No text.
```

Then in Tripo: **multi-view image to 3D** if your plan has it (front, side, back cut from the sheet), otherwise the front view alone · texture on · quad mesh if offered · about 15,000 faces · download GLB as `L-01.glb` into `art-inbox/3d/`.

---
---

# Part 2 — Wiring guide (for Claude Sonnet)

Read `docs/NEXT_CHAT.md` §6 first; everything there still applies. This part adds what the asset pack needs.

## 2.0 Ground rules

1. **Every generated asset is optional at run time.** Load it; if it is missing, build what the game builds today. Never delete a primitive-building code path or a `tools/pipeline/gen_*.py` generator — they are the fallback and the licence-clean baseline.
2. **Replace by file name where one exists.** Icons, portraits and sounds keep their names, so most of Packs C, D-01 and K need no code at all.
3. **No new packages, no INTERNET permission, no `FindObjectOfType`.** Unity cannot import GLB without a package — convert to FBX in the pipeline instead (§2.5).
4. **Run `tools/stubcheck/check.sh` after every code change**; add real signatures to the stubs for any new Unity API.
5. **Save data:** add fields with defaults only. Never rename or remove a field. `JsonUtility` fills missing fields with their defaults, so old saves keep working.
6. **Provenance:** every file that ships gets a row in `CREDITS.md` (tool, prompt ID, date; for audio: the Pixabay title, artist and URL from `LEDGER.txt`). Update the `AudioManager` class comment, which currently says there are no third-party audio licences.
7. **Size budget:** `Resources/` ≤ 60 MB in total (it is 13 MB today). Per file: icons 256, portraits and stickers 512, UI parts ≤ 768, backdrops ≤ 1600 wide JPG, tileable surfaces 512, prop textures 512 (1024 for bed, tub, doghouse, tree), props ≤ 3,000 triangles (≤ 6,000 for those four), music OGG at about 96 kbps. Add `tools/pipeline/budget.py` to print the totals and fail if a limit is broken.
8. **Look before committing art:** open every processed image and check it. Reject and report to Pon (with the prompt ID) anything with stray text, cut-off edges, a wrong item count or a Bella who does not match — do not ship a bad cell just because the sheet arrived.
9. **Tripo licence gate:** do not move any model from `art-inbox/3d/` into `Resources/` until Pon confirms it was generated on a plan that allows commercial use. Record his answer in `CREDITS.md`.

## 2.1 Folders

| Folder | Holds | Import rule to add in `BellaImportSettings` |
|---|---|---|
| `art-inbox/` (repo root) | Pon's raw downloads, named by prompt ID; `manifest.json` lists every sheet's cell names | not imported by Unity |
| `art-source/generated/` | The chosen originals after processing (move them here out of the inbox) | not imported |
| `Resources/Icons`, `Resources/Portraits` | as today | as today |
| `Resources/UI`, `Resources/Stickers`, `Resources/Fx`, `Resources/Seasons`, `Resources/Story` | new 2D art | same as Icons, but `maxTextureSize = 2048` for `UI/title_bg` and `Story/` |
| `Resources/Surfaces` | wallpapers, floors, rugs, window strips, wall pictures | files starting `wall_` or `floor_`: `wrapMode = Repeat`, mip-maps **on**; the rest as Icons |
| `Resources/Places` | backdrops | no mip-maps, Clamp, `maxTextureSize = 2048`, Android override ASTC 6×6 |
| `Resources/Props` | `name.fbx` + `name_tex.png` | model: `materialImportMode = None`, `importAnimation = false`, `meshCompression = Medium`, no cameras/lights; texture: mip-maps on, max 1024 |
| `Resources/Fonts` | one OFL font | none needed |
| `Resources/Audio` | as today | set `forceToMono = false` for `music_`; keep mono for the rest |

Bump `GetVersion()` so everything re-imports once.

## 2.2 Step 1 — `tools/pipeline/ingest_art.py`

One script that turns the inbox into game files. Behaviour:

- Read `art-inbox/manifest.json`. An entry with `cells`, `grid` and `dir` is a **sheet**: cut it into cells and save each as `<dir>/<cell>.png`.
- **Background removal:** if the image has no real alpha, flood-fill from all four edges with a tolerance, treating the corner colour (expected `#808080` or white) as background; then erode 1 px and feather 1 px to remove the fringe.
- **Cutting sheets:** do not trust even grid lines — AI sheets are never exact. Find connected blobs in the alpha channel, merge blobs closer than about 2 % of the image width (so a sparkle next to an icon stays with it), then assign each merged blob to the grid cell containing its centre. If the number of occupied cells does not match the manifest, stop and report that sheet instead of guessing.
- **Finish each cell:** crop to content, pad to a square with 6 % margin, resize with Lanczos to the size given in this document, save as PNG (JPG for the opaque backdrops and surfaces).
- **Tileable surfaces:** check the seam by comparing opposite edge rows; if the mean difference is visible, repair by offset-and-blend (shift by half, blend a soft band across the old seam). Write a 2×2 tiled preview to `docs/preview/` and look at it.
- Print a contact sheet per pack into `docs/preview/` so Pon can see at a glance what was accepted.
- Moves the used originals to `art-source/generated/`.

## 2.3 Step 2 — small helpers in `Core/Art.cs`

```csharp
/// <summary>Like Tex(), but quiet: returns null when the file is not there (generated art is optional).</summary>
public static Texture2D TryTex(string resourcePath)
{
    Texture2D t;
    if (texCache.TryGetValue(resourcePath, out t)) return t;      // caches misses too (null)
    t = Resources.Load<Texture2D>(resourcePath);
    texCache[resourcePath] = t;
    return t;
}

/// <summary>Sprite from Resources, or null when missing.</summary>
public static Sprite TrySprite(string resourcePath)
{
    Sprite s;
    if (iconCache.TryGetValue(resourcePath, out s) && s != null) return s;
    Texture2D t = TryTex(resourcePath);
    if (t == null) return null;
    s = UnityEngine.Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
    iconCache[resourcePath] = s;
    return s;
}

/// <summary>
/// A generated 3D model from Resources/Props, scaled so its largest side is `size` metres and
/// standing on the floor at localPos. Returns null when the model is not there — the caller
/// then builds the primitive version exactly as before.
/// </summary>
public static GameObject Prop(string name, Transform parent, Vector3 localPos, float size, float yaw = 0f)
{
    GameObject src = Resources.Load<GameObject>("Props/" + name);
    if (src == null) return null;
    var holder = new GameObject("Prop_" + name);
    holder.transform.SetParent(parent, false);
    holder.transform.localPosition = localPos;
    holder.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
    GameObject inst = Object.Instantiate(src, holder.transform, false);
    inst.transform.localPosition = Vector3.zero;          // the maths below scales about the holder's origin
    Material mat = Lit(Color.white, TryTex("Props/" + name + "_tex"));
    // Bounds in the holder's own space, from the meshes (renderer.bounds is world-space and
    // would be wrong under a rotated or scaled parent).
    bool any = false; Vector3 min = Vector3.zero, max = Vector3.zero;
    MeshFilter[] mfs = inst.GetComponentsInChildren<MeshFilter>();
    for (int i = 0; i < mfs.Length; i++)
    {
        Renderer r = mfs[i].GetComponent<Renderer>();
        if (r != null) r.sharedMaterial = mat;
        if (mfs[i].sharedMesh == null) continue;
        Bounds b = mfs[i].sharedMesh.bounds;
        Matrix4x4 m = holder.transform.worldToLocalMatrix * mfs[i].transform.localToWorldMatrix;
        for (int c = 0; c < 8; c++)
        {
            Vector3 p = m.MultiplyPoint3x4(b.center + Vector3.Scale(b.extents,
                new Vector3((c & 1) == 0 ? -1f : 1f, (c & 2) == 0 ? -1f : 1f, (c & 4) == 0 ? -1f : 1f)));
            if (!any) { min = max = p; any = true; } else { min = Vector3.Min(min, p); max = Vector3.Max(max, p); }
        }
    }
    if (!any) { Object.Destroy(holder); return null; }
    Vector3 dim = max - min;
    float biggest = Mathf.Max(dim.x, Mathf.Max(dim.y, dim.z));
    float k = biggest > 0.0001f ? size / biggest : 1f;
    inst.transform.localScale = inst.transform.localScale * k;
    Vector3 centre = (min + max) * 0.5f;
    inst.transform.localPosition = -new Vector3(centre.x, min.y, centre.z) * k;   // base centre on the holder
    return holder;
}
```

Usage pattern everywhere (bed shown):

```csharp
if (Art.Prop("bed", t, bed, 1.25f) == null)
{
    Art.Prim(PrimitiveType.Cylinder, "BedBase", t, ...);      // today's code, unchanged
    Art.Prim(PrimitiveType.Cylinder, "BedCushion", t, ...);
}
```

Also add `Art.TileTex(path)` (as `TryTex`, but sets `wrapMode = Repeat`) and make `Art.GetFont()` try `Resources.Load<Font>("Fonts/Baloo2-Bold")` first.

**Font:** add Baloo 2 Bold from the google/fonts repository (SIL Open Font Licence; it covers Latin **and Devanagari**, which makes the parked Hindi/Marathi work possible later). Ship `OFL.txt` beside it and add a `CREDITS.md` row. After switching, check every label still fits (`ParentsPanel`, popups, shop prices) — metrics differ from the built-in font.

## 2.4 Wiring, pack by pack

| Pack | Code to touch | What to do | Done when |
|---|---|---|---|
| **C icons** | none for replaced names | Run ingest. Keep the script-made `circle`, `panel`, `bubble` and `gesture_*` — they are tinted or drawn over in code. Before replacing any icon, grep for places it is drawn with a non-white tint (`UIKit.Img(…, Art.Icon("x"), someColour)`); those need a white shape, so keep the old file under `x_mask`. | All screens show new icons; nothing has a coloured square behind it |
| **B UI skin** | `UI/UIKit.cs`, `Core/Art.cs` (`Panel`) | `IconButton`/`TextButton`: background = `TrySprite("UI/btn_round")` (pill for text) else today's circle; keep tinting by `bg`. `Panel()`: prefer `UI/panel_popup` as a 9-slice with border = 96/512 of its width, and do **not** tint it (it is already cream) — add a parameter rather than changing every caller's colour. Need meters in `Hud.cs`: `UI/ring` over the fill. Shop cards in `WardrobeActivity`/`FeedingActivity`: `UI/card_shop`, `tag_price`, `tag_owned`. `ThoughtBubble.cs`: `UI/bubble_thought` + `UI/wish_*`. Daily gift: `UI/gift_*` three-frame open. | 20:9 phone and 4:3 tablet layouts still fit (the 12 Nov checks) |
| **A brand** | new `UI/TitleScreen.cs`, `GameRoot.Start` | Studio mark 1 s → `title_bg` (aspect-fill) with `logo` gently bobbing and a pulsing paw "play" button → fade to room. Plays `music_title`. Skip the whole screen if `UI/title_bg` is missing. `Story/intro_1..4`: swipe-through shown once (`SaveData.introSeen`) in place of the first popup. App icon: replace `Portraits/app_icon.png`; `BellaProjectSetup` already picks it up — add the adaptive-icon foreground from `A-03b` if present. | Cold start shows title in < 2 s; Android Back on the title screen exits |
| **D portraits** | `Systems/Progress.cs` | Same names are drop-in. Use the new moods where they fit: `face_love` on cuddles milestones, `face_proud` on tricks, `face_sniffles` on the sniffles popup, `face_goodnight` at night, `face_party` on streaks. | Every popup still has a portrait |
| **D stickers** | `UI/StickerBook.cs`, `Systems/Catalog.cs` | Draw `TrySprite("Stickers/" + id)` untinted if present, else today's tinted icon. New ids in D-05/D-06 become `StickerDef`s only when the feature that awards them exists. | Locked stickers still show as a silhouette (tint the art dark, alpha 0.35) |
| **H effects** | `Pet/Fx.cs` | `Burst` looks up `Fx/fx_<name>` first, then `Icons/<name>`. Add `Fx.Confetti`, `Fx.Petals`, `Fx.Poof` (`fx_puff`) for purchases, level-ups and outfit changes; `fx_rays` rotating slowly behind the daily gift and new stickers. | Nothing allocates per frame — pool as `Burst` already does |
| **E surfaces** | `World/Room.cs`, `Systems/Catalog.cs`, `Save/SaveData.cs`, new `MiniGames/DecorActivity.cs` | Keep references to the wall, floor and rug renderers in `Build()`. `Room.ApplyTheme(wall, floor, rug)`: `Art.Lit(Color.white, Art.TileTex(…))` with `mainTextureScale` ≈ (6, 2) on the back wall, (4, 2) on the side walls, (6, 6) on the floor; rug = unlit quad lying flat with the rug PNG (replace the two cylinders). `RoomThemeDef` table with treat prices (first one free), a Decor shop cloned from `WardrobeActivity`, `SaveData.ownedThemes` / `wallId` / `floorId` / `rugId` / `viewId` / `pictureId`. Window strip: an unlit quad just behind the window bars and in front of the orb and clouds (sky is at z 2.655, clouds 2.652, orb 2.65, bars 2.645 — put it at about 2.648), tinted by `DayNight` so it darkens at night. Wall picture: tap the frame to cycle `pic_1..9`. | Default look with no files present is identical to today |
| **G props** | `World/Room.cs`, `Pet/Accessories.cs`, `MiniGames/*`, `World/Ambience.cs` | `Art.Prop(...) ?? primitives` at each place named in the pack table. Bowl: keep `SetBowlFull` toggling the `kibble` object, whichever kind it is. Tub: keep the code-made water disc, sized to the model's rim. Ball: `ThrowBall` moves the holder; spin the child. Wearables: in `Accessories.Build`, try `Art.Prop("hat_top", anchor, Vector3.zero, size)` before the primitive `switch`; give each `OutfitDef` an optional `model` and tint off (models are pre-coloured, so one model = one outfit; colour variants stay primitive). Sit hats 1–2 cm into the head so they do not float; verify on the real model with all 15 animations. | Diagnostics line "Props loaded: n" added; missing props never log an error |
| **F places** | new `World/Places.cs`, `MiniGames/Activity.cs` | Put all room decor under one `Decor` parent so it can be hidden. `Places.Show(id)`: backdrop quad 6.4 × 4 m just in front of `WallBack` (unlit, `TryTex("Places/…")`), floor texture, that place's props; `Places.Home()` restores. First use: `Activity.Begin` switches place (bath → `bathroom`, clinic → `clinic`, tricks → `stage`, wardrobe → `studio_pink`) and `GameRoot.ActivityFinished` returns home — the game gains four locations with no new gameplay. Load backdrops on demand and call `Resources.UnloadUnusedAssets()` after returning home. Garden and park as visitable places come with the v1.1 mini-games. | Camera framing per place checked at 20:9 and 4:3; no backdrop edge visible |
| **Friends (G-71…)** | new `World/Friends.cs` | Follow the butterfly code in `Ambience.cs`: Pip lands on the window sill on mornings (hop, head-tilt by code, `chirp`), Momo crosses the rug on rainy days, Dot sits on the plant. Tap → `Fx.Hearts` + sound + counter → stickers `friend_bird`, `friend_snail`. No rigs: bob, hop and slide transforms only. | They never enter Bella's wander area mid-walk |
| **I seasons** | new `World/Seasons.cs`, `UI/ParentsPanel.cs` | Device date only. Fixed dates: winter 20 Dec–1 Jan, New Year 31 Dec–2 Jan, kite day 13–15 Jan, harvest 25–31 Oct, rainy season 15 Jun–15 Sep on about one day in three (hash of the date, so it is stable all day; adds `amb_rain`, rain streaks on the window, the snail). **Diwali and Holi move every year — look the dates up for 2026–2032 from a calendar source and put them in a table with the source in a comment; do not compute or guess them.** Bella's birthday = each anniversary of `firstPlayUtcTicks`, plus a "one month together" party on day 30. A season hangs its sprites at fixed anchor points (bunting line, baseboard row, window corners, rug centre for rangoli), swaps music, and offers its sticker. Parents' area: "Festival decorations" on/off (`SaveData.seasonsOn = true`). | With the date forced in the Editor, each season shows and clears correctly |
| **K audio** | `Core/AudioManager.cs`, `tools/pipeline/ingest_audio.py`, `World/DayNight.cs` | Ingest with ffmpeg: trim leading/trailing silence, short fades (5 ms in, 30 ms out) on effects, loudness-normalise (music about −18 LUFS, effects peak −3 dB, voices matched to each other), effects → mono 44.1 kHz WAV, music and ambience → stereo OGG. **Delete the old `.wav` of the same name when adding a replacement** — two files with one name in `Resources/Audio` is a silent bug. Loops: find a zero-crossing loop point and cross-fade the tail into the head (0.5–2 s) so music and ambience wrap cleanly; listen to every wrap. Variants: when `name_1…n` exist, `Play(name)` picks one at random, never the same twice running. `Voice()`: real puppy samples must not be pitched up 1.05–1.25 any more — use 0.97–1.06. New hooks: `lap` on drinking, `shake` after bath, `paws` under `WalkToState` at run speed, `pant` after fetch, `sniff` in idle, `purchase` in shops, `sticker`, `page`, `rustle`. Music per activity via the existing override: bath → `music_bath`, shops → `music_shop`, tricks → `music_stage`. | Old synthesised files remain reproducible from `gen_audio.py`; `CREDITS.md` lists every Pixabay file |
| **J store** | `docs/` | Compose the feature graphic (art + logo), build six framed screenshots from Pon's real captures on `frame_bg` with short captions, refresh `docs/index.html` / `bella.html` hero, then hand Pon a fresh zip for Hostinger (see `docs/WEBSITE.md`). Store screenshots must show the real game. | Previewed at 400 px and 1200 px |
| **L new Bella** | `tools/pipeline/*` | Opus-level. New GLB → `build_rig.py` → `build_anim.py` → the three Blender checks in `NEXT_CHAT.md` §1 → regenerate `BellaClips.cs` and faces. Keep the old FBX until the new one passes everything. | All 12 Nov checks pass again |

## 2.5 Step for 3D — `tools/pipeline/prop_convert.py` (Blender as the `bpy` module)

For each `art-inbox/3d/G-xx.glb`: import → join all meshes → apply transforms → decimate to the triangle budget → merge by distance, recalculate normals, shade smooth → move the origin to the centre of the base and the model to the world origin → save the base-colour image resized to 512/1024 as `<file>_tex.png` → export FBX without embedded media using the **same axis settings as Bella's FBX export in `build_anim.py`** → render a 4-view preview to `docs/preview/props/` and look at it. Map IDs to file names from `manifest.json`. Wearables: after conversion, check the hollow underside is open and re-centre the origin at the middle of the opening rather than the lowest point.

## 2.6 Order of work

1. Ingest tool + import settings + `Art` helpers + font (one session, no visible change yet — verify the game looks identical with no new files).
2. Wave 1 files: icons C-01/02/07, UI B-01/02, title screen A-01/02/03, audio K1 (5 tracks) + K2.
3. Wave 2: remaining icons, portraits, stickers, effects, room themes, Wave-1 props, remaining audio.
4. Wave 3: places, friends, seasons, store page.

After each session: stubcheck, budget script, update `CREDITS.md`, `docs/ACHIEVEMENTS.md` and `docs/NEXT_CHAT.md`, commit, push. Tell Pon exactly which IDs were accepted, which were rejected and why, and what to check in Unity (`Bella → Set Up Project`, then Play).

## 2.7 Things that will go wrong

- **Sheet has 8 or 10 items, or they are out of order** → do not guess; list the cells you see and ask Pon to regenerate that one sheet.
- **"Transparent" PNG that is really a painted checkerboard** → detect (no alpha + two alternating greys) and ask for the grey-background version.
- **Icons from different sheets differ in weight or outline** → normalise visible size (content fills about 86 % of the cell) and accept small differences; do not redraw.
- **Tripo model has baked highlights that fight the game light** → fine under `Legacy Shaders/Diffuse`; if it looks too shiny, multiply the texture by 0.92 and add 4 % desaturation in the convert step.
- **Model is fine but faces the wrong way** → `yaw` parameter; record the value in the call, not in the file.
- **APK grows too much** → backdrops and music are the big ones; lower JPG quality to 82 and OGG to 80 kbps before cutting content.
- **A pretty asset makes the fallback look broken next to it** → that is expected mid-wave; finish the pack rather than mixing.

---

# Part 3 — Messages for Pon to paste into Sonnet

Start each in the Bella project. One message per sitting.

**Setup (before any art is uploaded):**
```text
Continue Bella — repo medh-eng/Bella-cute-puppy. Read docs/NEXT_CHAT.md, then docs/ASSET_PROMPTS.md Part 2. Do step 1 of §2.6 only: build tools/pipeline/ingest_art.py, ingest_audio.py and budget.py, extend BellaImportSettings for the new folders, add the Art helpers from §2.3 and the Baloo 2 font. No visible change is expected yet. Run stubcheck, commit and push, and tell me what to check in Unity.
```

**Wave 1 art is in the inbox:**
```text
Continue Bella. I uploaded Wave 1 files to art-inbox/ (IDs: A-01, A-02, A-03, B-01, B-02, C-01, C-02, C-07) and audio to art-inbox/audio/ with LEDGER.txt. Follow docs/ASSET_PROMPTS.md Part 2: ingest them, look at every result, wire the icons, UI skin, title screen and audio. Tell me which IDs you rejected and why so I can regenerate them. Update CREDITS.md and NEXT_CHAT.md, commit and push.
```

**Wave 2 — 2D:**
```text
Continue Bella. New files in art-inbox/: the rest of Pack C, Pack D, Pack E and Pack H. Ingest and wire them per docs/ASSET_PROMPTS.md §2.4: portraits, illustrated stickers, effects, and the room themes with a Decor shop. List rejected IDs. Commit and push.
```

**Wave 2 — 3D:**
```text
Continue Bella. I uploaded 3D models to art-inbox/3d/ (IDs: G-01 … ). Licence: I generated them on Tripo's [plan name] plan on [date]. Convert them with prop_convert.py, show me the previews, and wire them in with Art.Prop and the primitive fallbacks per docs/ASSET_PROMPTS.md §2.4–2.5. Commit and push.
```

**Wave 3:**
```text
Continue Bella. New files: Pack F backdrops, Pack I festival sheets, more Pack G models (friends and festival props). Build Places, Friends and Seasons per docs/ASSET_PROMPTS.md §2.4. Look up the real Diwali and Holi dates for 2026–2032 and cite the source in the code. Commit and push.
```

**Store page:**
```text
Continue Bella. I uploaded J-01 to J-04 and six real phone screenshots to art-inbox/store/. Build the feature graphic, framed store screenshots and the refreshed website per docs/ASSET_PROMPTS.md Pack J, and give me the zip for Hostinger.
```

**If something looks wrong in Unity:**
```text
Continue Bella. After the last asset update, this looks wrong in Unity: [describe, or paste the red Console line]. Check docs/ASSET_PROMPTS.md §2.7 and docs/NEXT_CHAT.md §3 first.
```
