# Rooms and mini-games — plan (19 Nov 2026)

Pon's goals: more engagement, mini-games a child enjoys, rewards (coins/treats, gifts, new items),
and several rooms/places. Rules that stay: no fail states, no ads/networking, every generated asset
optional with a fallback, repeatable play pays from a daily allowance (`Progress.DailyPlayTreats` = 30).

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
3. **Hide and seek** — Bella hides behind 3 cushions; find her; harder with more cushions.
4. **Paw memory** — flip cards, match pairs (uses existing sticker/icon art).
5. **Bubble pop** — bubbles float up, pop for treats (bath-adjacent, reuses `Fx.Bubbles`).
6. **Agility course** — swipe to jump hurdles in the park (needs a jump clip: only new animation required).

## Rewards, kept in balance
- Today's allowance of 30 repeatable treats makes the wardrobe a 1-2 week goal. More games can share that pool
  or get their own small pool — decide before building the second game (see open question below).
- Gifts: a "mystery box" (decor item, outfit colour, sticker) from combos, digging and a daily wheel — uses the
  existing gift popup and catalog; no new currency needed.
- Per-game stickers and a trophy shelf make progress visible without more coins.

## Open question for Pon
Raise `DailyPlayTreats` (for example to 45) now that play is more active, or give each mini-game its own daily pool?
