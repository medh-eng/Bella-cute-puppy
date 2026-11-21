# Rooms and mini-games — plan (19 Nov 2026)

Pon's goals: more engagement, mini-games a child enjoys, rewards (coins/treats, gifts, new items),
and several rooms/places. Rules that stay: no fail states, no ads/networking, every generated asset
optional with a fallback, repeatable play pays from a daily allowance (`Progress.DailyPlayTreats` = 60, raised from 30 on 21 Nov 2026 when the arcade games arrived).

## Done
- **Fetch** (`MiniGames/FetchActivity.cs`, `States/FetchChaseState.cs`): in the park. Pick the ball up with a
  finger, flick it; it flies with its own physics (gravity, bounces, rolling, room walls). Bella chases it
  (leads a rolling ball), grabs it in her mouth, carries it to the **drop mat** and lets go on it. After a
  round the ball is snapped to the mat if it strayed. Tap the floor to send her somewhere. Every 5th fetch in a
  visit = combo bonus. Stickers: Fetch fan (10), Fetch champion (50).
- **Tap-to-move in the living room** (`Pet/PetInteraction.cs`): a quick tap on empty floor sends her there.

- **Play menu** (`UI/GamePicker.cs`): the ball button now opens a picker — Fetch, Treat Catch, Dig for Treasure.
- **Treat Catch** (`MiniGames/TreatCatchActivity.cs`, kitchen): 30 s, slide a finger, Bella runs under falling treats;
  golden stars worth 3. Payout from the daily play pool. Stickers: Treat catcher, Catch champion (10 rounds).
- **Dig for Treasure** (`MiniGames/DigActivity.cs`, garden): tap mounds, Bella digs, every mound hides treat / bone /
  star / gift. 8 digs a visit. Stickers: Treasure hunter, Master digger (25 digs).
- **Arcade pack, 21 Nov 2026** — the Play menu is now a 3x3 grid of nine games. Shared shell: `MiniGames/ArcadeActivity.cs`
  (score pills, hint, Bella cheers, one payout routine `EndRound`; counter `countArcade`; stickers Game time / lover / champion).
  New: **Ball Catch** (park, subclass of Treat Catch, balls fly in from the sides), **Block Drop** (8x14 falling blocks, 4 buttons,
  90 s, full board just clears), **Jigsaw** (2x2, 3x3, 4x4 cut from the place backdrops, drag to snap), **Arrows** (pull back and release,
  dotted aim line, 12 arrows, balloons), **Car Race** (3 lanes, tap left/right of the car, 45 s, cones only slow you), **Frisbee**
  (park, swipe to throw at a gold target pad, Bella catches it in her mouth, 8 throws). Four new icons from
  `tools/pipeline/gen_game_icons.py`. Daily play pool raised 30 -> 60. All logic compiles under stubcheck; none of it has run in
  Unity yet — first things to check: Jigsaw crop of the backdrops, Car Race HUD layering, Frisbee throw distances (`Throw`), Arrow `PullPower`.
- **Puzzle trio, 21 Nov 2026** — Play menu now 12 games (4 rows). All three are UI-only on the `ArcadeActivity` shell and pay from the daily play pool:
  **Hide & Seek** (`HideSeekActivity`: Bella peeks, ducks, cushions shuffle, tap the right one; 3 rounds of 3/4/5 cushions),
  **Paw Memory** (`PawMemoryActivity`: 3, 6, 8 pairs, no clock), **Bubble Pop** (`BubblePopActivity`: 40 s, gold bubbles = 3).
  Compile-checked (stubcheck, 64 scripts); not yet run in Unity. Check first: cushion/portrait layout (`Portraits/face_playful` assumed square),
  card sizes on small phones, bubble speed and tap radius, picker height (1660) on short screens.
- **Places, 21 Nov 2026** — new map button (left side of the HUD) opens `UI/PlacePicker.cs`: Garden, Park, Kitchen free; Beach 20, Snow 30,
  Stage 40 treats (bought once, `SaveData.ownedPlaces`). `MiniGames/VisitActivity.cs` (+ `PlaceDef` table): backdrop, floor and music per place, tap the
  ground to send Bella exploring, +2 happiness every 12 s, first visit to each place pays a 5-treat souvenir (`visitedPlaces`). Stickers `visit_1`,
  `visit_all`. Compile-checked, not run in Unity. Check: camera framing (`Focus` in `VisitActivity.Build`), the extra HUD button's position, `music_fest_winter` for snow.
- **21 Nov 2026 playtest fixes**: armchair faces the room and sits in the corner (smaller footprint); pinch /
  two-finger-drag zoom in the living room (`World/CameraRig.cs`); lower ambient + stronger key light + deeper fur
  tint for contrast (`World/DayNight.cs`, `Pet/BellaModel.cs`); living-room fetch now carries the ball in her mouth
  (`BellaController.HoldInMouth`, shared with the park); **drinking**: `BellaController.RequestDrink` — walks to the
  water bowl, head down, `lap` sound looped for the drink, +3 happy; she drinks after most meals and sometimes on her own.
  Tuning to check on the phone: `BellaController.MouthPoint` offsets, `CameraRig.MaxZoom`, ambient values.

## Rooms / places (backdrops already in `Resources/Places`: kitchen, garden, park, bedroom_night, beach, snow, stage, bathroom, clinic)
1. Garden (digging + butterflies), Kitchen (feeding), Bedroom (naps, night) as places you can switch to from a
   "Places" button, unlocked with treats. A place = backdrop + floor surface + a handful of props + its own music
   (`music_garden` is waiting) + one activity. `Room.ShowPlace` already does the backdrop and floor swap.
2. Seasonal places (snow, beach) tied to the festival stickers.

## Mini-games, in suggested order (each is an `Activity` subclass, ~200-300 lines)
1. **Treat catch** — treats fall, drag Bella left/right to catch; golden ones are worth more.
2. **Dig for treasure** (garden) — tap spots, Bella digs, finds treats, toys, sometimes a gift box.
3. ~~**Hide and seek**~~ (done) — Bella hides behind 3 cushions; find her; harder with more cushions.
4. ~~**Paw memory**~~ (done) — flip cards, match pairs (uses existing sticker/icon art).
5. ~~**Bubble pop**~~ (done) — bubbles float up, pop for treats (bath-adjacent, reuses `Fx.Bubbles`).
6. **Agility course** — swipe to jump hurdles in the park (needs a jump clip: only new animation required).

## Rewards, kept in balance
- Today's allowance of 30 repeatable treats makes the wardrobe a 1-2 week goal. More games can share that pool
  or get their own small pool — decide before building the second game (see open question below).
- Gifts: a "mystery box" (decor item, outfit colour, sticker) from combos, digging and a daily wheel — uses the
  existing gift popup and catalog; no new currency needed.
- Per-game stickers and a trophy shelf make progress visible without more coins.

## Open question for Pon
Raise `DailyPlayTreats` (for example to 45) now that play is more active, or give each mini-game its own daily pool?
