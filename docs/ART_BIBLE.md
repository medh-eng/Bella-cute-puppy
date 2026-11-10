# Bella — Character Art Bible

## 1. Who Bella is
A small, very happy Golden Retriever puppy. Warm, affectionate, a little clumsy, endlessly enthusiastic. She is the emotional center of the game — every visual choice should make the player want to protect and nurture her.

## 2. Why this must NOT look like "Boo"
*Boo — The World's Cutest Dog* is a real, trademarked character (a white Pomeranian with a specific signature haircut), commercially licensed and merchandised. Bella must be built from original reference and original art at every stage — no tracing, no asset reuse, no name reuse. This is a legal requirement, not just a creative preference. Distinguishing factors to lock in early:

| Trait | Boo (avoid) | Bella (use) |
|---|---|---|
| Breed | Pomeranian | Golden Retriever |
| Base color | White | Golden/cream gradient |
| Signature haircut | Distinct round teddy-bear trim | Natural puppy coat, floppier, no stylized trim |
| Ear shape | Small, rounded, tucked | Classic Retriever floppy ears (a strong silhouette difference on its own) |
| Build | Very round, compact | Slightly leggier puppy proportions, still exaggerated-cute |

## 3. Psychological design target: Kindchenschema ("baby schema")
These proportions are not optional polish — they are the mechanism that makes players emotionally bond with Bella:
- Oversized head relative to body
- Large, low-set eyes
- Small nose
- Shortened, slightly clumsy-looking limbs

## 4. Color and mood
- Primary palette: warm, saturated golden and cream gradients (true to the Golden Retriever breed) for Bella herself.
- Environment palette: soft pastels and bright primary accents — cheerful, non-threatening, never dark or desaturated.
- Signature visual trait (pick one and keep it consistent across all art, for brand recognizability): e.g. a specific collar color (suggest a soft red or teal, distinct from any "iconic" existing pet-character color), or one single floppy ear with a small distinguishing marking.

## 5. Expression system
Built via **blendshapes/morph targets** on the base mesh (not bone-based facial rigging):
- Smile / joyful (default "happy" state)
- Droopy / sad eyes (neglect state)
- Open-mouth pant (post-play / warm state)
- Begging (hunger-critical sub-state)
- Yawning (energy-low sub-state)

## 6. Shading approach
Toon/cel-shaded (flat color bands: base, shadow, highlight) — see `ARCHITECTURE.md` §4. This is both a performance decision and a visual-differentiation decision: a cel-shaded Bella reads as clearly distinct from Boo's photoreal-fuzzy look at a glance, which matters for avoiding any look-alike concern as well as for carving out your own visual identity.

## 7. Asset list needed from an artist (for Phase 3)
1. Base mesh (puppy body, proportioned per §3)
2. Rig (skeleton for body/limb/tail animation)
3. Blendshapes per §5
4. Idle, walk, sit, sleep, eat, bark/react, tail-wag animation clips
5. Albedo texture (toon-shaded palette per §4/§6)
6. A small wardrobe set for The Wardrobe mini-game (collar variants, a hat, a bandana — start with 3-5 items for v1, expand later)

## 8. Where to find an artist if not DIY
- Freelance platforms (Fiverr/Upwork) for a rigged low-poly stylized puppy model — often cheaper and faster than building from scratch in Blender yourself, given you're non-technical in this specific domain.
- Or local: Hyderabad's AVGC ecosystem (TVAGA for networking, IGDC conference for freelance discovery) — see `GDD.md` §8.
