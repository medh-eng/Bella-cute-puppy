# Bella — Character Art Bible + AI Art Pipeline (v2)

## 1. Who Bella is
A small, very happy Golden Retriever puppy. Warm, affectionate, a little clumsy, endlessly enthusiastic. Every visual choice should make the player want to protect and nurture her.

## 2. Why this must NOT look like "Boo"
*Boo* is a real, trademarked character (white Pomeranian, signature teddy-bear trim). Bella must be original at every stage — no tracing, no asset reuse, no name reuse.

| Trait | Boo (avoid) | Bella (use) |
|---|---|---|
| Breed | Pomeranian | Golden Retriever |
| Base color | White | Golden/cream gradient |
| Haircut | Round teddy-bear trim | Natural floppy puppy coat |
| Ears | Small, rounded, tucked | Classic floppy Retriever ears (strong silhouette difference) |
| Build | Very round, compact | Slightly leggier puppy proportions, still exaggerated-cute |

## 3. Kindchenschema targets (non-negotiable proportions)
- Head ≈ 40% of total body volume (oversized)
- Large, low-set eyes (eye height ≈ vertical center of face or below)
- Small nose, short muzzle
- Short, slightly clumsy limbs; big paws
- Round everything — no sharp angles anywhere on the character

## 4. Color and mood
- Bella: warm saturated golden/cream gradient; darker honey ears; cream chest/muzzle.
- Signature trait (Pon to pick, then locked forever): **(a)** soft teal collar with a gold paw-tag, or **(b)** a small heart-shaped darker patch on the left ear.
- Environments: soft pastels + bright primary accents; never dark or desaturated.

## 5. Expression set (face textures, not blendshapes)
Six faces, shipped as PNGs swapped onto the head material (`FaceExpressionController`):
1. Happy (default) — open smile, sparkly eyes
2. Excited — huge smile, star eyes, tongue out
3. Sad/wistful — droopy eyes, small frown (used gently — see "kindness cap", GDD §4)
4. Sleepy — half-closed eyes, tiny yawn mouth
5. Begging — big watery "puppy eyes"
6. Soapy/silly — eyes closed happy, used in bath

All six must be generated in ONE AI session/image (an expression sheet) so they're stylistically identical.

## 6. Shading
Flat-ish cartoon look: URP Lit with high smoothness off, or Simple Lit; single warm directional light + ambient. If the Meshy texture comes out too "realistic", re-texture in Meshy with a "cute cartoon flat colors" style prompt.

---

## 7. THE AI ART PIPELINE — step by step (Pon executes, Claude supplied prompts)

### Step 1 — Concept turnaround (ChatGPT Images or Gemini)
Generate, pick the best, iterate until it feels right. Prompt:

> Character design sheet of an original cute cartoon golden retriever puppy character for a children's mobile game. Turnaround sheet: front view, side view, back view, 3/4 view of the SAME character in a neutral standing pose. Kindchenschema baby-schema proportions: oversized round head about 40% of body, very large low-set glossy eyes, tiny nose, short muzzle, short chubby legs, big paws, floppy ears, small wagging tail. Warm golden and cream gradient fur, darker honey-colored floppy ears, cream chest. Wearing a soft teal collar with a small gold paw-shaped tag. Flat cartoon shading, clean bold shapes, white background, full body visible in every view, no text labels. Style: modern kids' game character (think gentle, round, toy-like), NOT realistic.

(If Pon picks the ear-patch signature instead, swap the collar sentence.)

### Step 2 — Expression sheet (same tool, same session, reference the chosen image)
> Using this exact character, create an expression sheet: 6 versions of ONLY the face/head, same angle (front), same style: 1) happy open smile 2) excited with star eyes and tongue out 3) gently sad with droopy eyes 4) sleepy half-closed eyes 5) begging big watery puppy eyes 6) eyes-closed silly grin. Grid layout, white background, no text.

Crop the 6 faces into square PNGs → `Assets/Art/Bella/Faces/`.

### Step 3 — Image → 3D (Meshy, free tier 100 credits/month; fallback: Tripo3D)
- meshy.ai → Image to 3D → upload the FRONT view (add side view if the tool accepts multi-view).
- Settings: character/creature mode, symmetry ON, PBR off (we want flat color), target the "cartoon/stylized" option if offered.
- Generate 2–4 variants, pick the cutest. If the mesh is very dense, run Meshy's retopo/low-poly to roughly 15–25k tris.

### Step 4 — Auto-rig (Meshy Rigging — supports quadrupeds; Mixamo does NOT work for dogs)
- Open the model in Meshy → Animate → Rig → it detects "quadruped" → place the markers it asks for → Confirm.
- Export **FBX (rigged)** for Unity + keep a GLB backup. → `Assets/Art/Bella/Model/`.
- Fallback: Tripo3D auto-rig (`rig_type: quadruped`) if Meshy's rig deforms badly.

### Step 5 — Preset animations
- In Meshy's animation library pick quadruped clips: idle, walk, run (sit/sleep if available).
- Export each as FBX with animation → `Assets/Art/Bella/Animations/`.
- Everything the library lacks (tail wag, bounce, squash, head tilt) is **procedural in code** — already built, nothing to generate.

### Step 6 — Props (Meshy text-to-3D or primitives)
Food bowl, dog bed, ball, shampoo bottle. Prompt pattern:
> cute cartoon [red dog food bowl], children's game prop, flat colors, simple rounded shapes, low poly, white background

### Step 7 — UI art (ChatGPT/Gemini)
Icons (bone, drumstick, soap bubble, Zzz, heart, paw-coin "treat"), buttons, room background elements. One sheet per batch, consistent prompt suffix:
> …flat cartoon icon for a children's game UI, thick soft outline, pastel palette, white background, no text

### Quality bar for accepting any generated asset
- Reads clearly at thumbnail size
- No uncanny/realistic textures, no extra limbs/artifacts
- Style-matches the locked concept sheet
- For the 3D model: symmetrical, all four legs separate (not fused), tail distinct, head proportion preserved

## 8. If AI 3D output quality disappoints after ~3 attempts
Fallbacks in order, still zero human spend:
1. Tripo3D instead of Meshy (different generator, often different failure modes)
2. Generate a simpler "toy-like" Bella (chibi, even rounder — denser shapes survive image-to-3D better than slim limbs)
3. Hybrid: keep the grey-box primitive puppy but AI-texture it (it's already animated and cute via procedural motion)
