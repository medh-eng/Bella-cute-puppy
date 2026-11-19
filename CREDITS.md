# Credits and asset provenance

A paper trail for Google Play review: where every asset in the shipped game came from.
Nothing in the build is taken from another game. The supplied audio listed below is the exception: its source must be confirmed and recorded before release.

| Asset | Files | How it was made | Licence position |
|---|---|---|---|
| Bella concept art and expression sheet | `art-source/faces_2d/` | AI image generation from the prompts in `docs/ART_BIBLE.md` | Original character created for this project |
| Bella 3D mesh | `art-source/bella_raw.glb` | AI image-to-3D from the concept art | Generated for this project |
| Rig, skin weights, 15 animations | `unity-project/Assets/Resources/Bella/bella.fbx` | Built by script in Blender — `tools/pipeline/build_rig.py`, `anims.py`, `build_anim.py` | Original work |
| Face textures (happy, excited, sad, sleepy, begging, blink) | `Assets/Resources/Bella/Faces/` | Repainted from the model's own texture — `tools/pipeline/faces.py` | Original work |
| UI icons (54) — now partly replaced by generated icons above; the originals remain for names not replaced | `Assets/Resources/Icons/` | Drawn by script — `tools/pipeline/gen_icons.py` | Original work |
| Sound effects and both music loops | `Assets/Resources/Audio/` | Synthesised by script — `tools/pipeline/gen_audio.py` (no samples used) | Original work |
| Supplied music (14 tracks) and puppy / eating / sleeping sounds (`bark_1…3`, `bark_happy_1…5`, `yip_1`, `whine_sad_1…4`, `whimper_hungry_1…3`, `yawn_sleepy`, `munch_1…4`, `snore_1…6`, `sniff_1…3`, `lap`, `pant`) | `Assets/Resources/Audio/` | Supplied by Pon on 17 Nov 2026, prepared by `tools/pipeline/ingest_audio.py` (loop cross-fade, loudness match). Each file's upload name and checksum are in `art-inbox/audio/LEDGER.txt` | **Source and licence still to be recorded before release** — add the site/tool and licence terms to the ledger and this row |
| Generated art: UI skin, logo, title screen, studio mark, story cards, 90 icons, app icon, 18 Bella portraits, 27 stickers | `Assets/Resources/UI/`, `Story/`, `Icons/`, `Portraits/`, `Stickers/`, `docs/store/app_icon_512.png` | AI image generation (ChatGPT) from the prompts in `docs/ASSET_PROMPTS.md` (A-01…A-06, B-01…B-08, C-01…C-10, D-01…D-05), cut into single files; ledgers in `art-inbox/INTAKE_2026-11-17.md` and `INTAKE_2026-11-18.md` (corrections and 3D UI redo) | Original character art generated for this project; confirm ChatGPT terms on the day of launch |
| Generated art, second batch: 13 wallpapers and floors, 9 place backdrops, 9 festival sticker sets (cut into 81 stickers), 9 wish icons, growth card | `Assets/Resources/Surfaces/`, `Places/`, `Stickers/fest_*`, `UI/wish_*`, `UI/growth.jpg` | AI image generation (ChatGPT) from the prompts in `docs/ASSET_PROMPTS_V2.md` (E-01…E-13, F-01…F-09, D-06, D-07, D-08), processed by `tools/pipeline/ingest_art.py`; originals in `art-source/generated/`; ledger in `art-inbox/INTAKE_2026-11-19.md` | Original art generated for this project; confirm ChatGPT terms on the day of launch |
| Room, furniture, accessories, particles | — | Built from primitives in code at run time | Original work |
| Store feature graphic | `docs/store/feature_graphic_1024x500.png` | Composed from the project's own portrait and icons | Original work |
| Font | — | Unity's built-in runtime font (`LegacyRuntime.ttf`) | Ships with the Unity engine |

**Props (8 models):** bed, bowl, kibble, ball, tub, toy shelf, toy basket, toy duck — made by Pon on the Triple 3D site (triple3d.ai) from reference pictures generated in ChatGPT, then reduced by script (Blender: ~2,500 triangles, 512 px texture) into `Assets/Resources/Props/`. Triple 3D's terms (https://triple3d.ai/terms, §6, checked 19 Nov 2026) allow use of generated outputs for commercial purposes; they give no guarantee of exclusivity or non-infringement, so re-check the terms on launch day and keep a saved copy. Full-size originals stay on Pon's computer, not in the repo.

Engine: Unity (Unity Personal). No third-party plugins, SDKs or packages beyond Unity's own modules.

Inspiration only: *Boo — The World's Cutest Dog* is a different character owned by others. Bella
shares no name, artwork, sound, text or code with it (see `docs/ART_BIBLE.md` §2).
