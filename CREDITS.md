# Credits and asset provenance

A paper trail for Google Play review: where every asset in the shipped game came from.
Nothing in the build is taken from another game, a stock library or a third-party sound pack.

| Asset | Files | How it was made | Licence position |
|---|---|---|---|
| Bella concept art and expression sheet | `art-source/faces_2d/` | AI image generation from the prompts in `docs/ART_BIBLE.md` | Original character created for this project |
| Bella 3D mesh | `art-source/bella_raw.glb` | AI image-to-3D from the concept art | Generated for this project |
| Rig, skin weights, 15 animations | `unity-project/Assets/Resources/Bella/bella.fbx` | Built by script in Blender — `tools/pipeline/build_rig.py`, `anims.py`, `build_anim.py` | Original work |
| Face textures (happy, excited, sad, sleepy, begging, blink) | `Assets/Resources/Bella/Faces/` | Repainted from the model's own texture — `tools/pipeline/faces.py` | Original work |
| Portraits and app icon | `Assets/Resources/Portraits/` | Cropped from the project's own expression sheet | Original work |
| UI icons (54) | `Assets/Resources/Icons/` | Drawn by script — `tools/pipeline/gen_icons.py` | Original work |
| Sound effects and both music loops | `Assets/Resources/Audio/` | Synthesised by script — `tools/pipeline/gen_audio.py` (no samples used) | Original work |
| Room, furniture, accessories, particles | — | Built from primitives in code at run time | Original work |
| Store feature graphic | `docs/store/feature_graphic_1024x500.png` | Composed from the project's own portrait and icons | Original work |
| Font | — | Unity's built-in runtime font (`LegacyRuntime.ttf`) | Ships with the Unity engine |

Engine: Unity (Unity Personal). No third-party plugins, SDKs or packages beyond Unity's own modules.

Inspiration only: *Boo — The World's Cutest Dog* is a different character owned by others. Bella
shares no name, artwork, sound, text or code with it (see `docs/ART_BIBLE.md` §2).
