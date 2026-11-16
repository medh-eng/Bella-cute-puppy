# art-inbox — drop generated files here

Raw downloads from ChatGPT, Gemini, Tripo and Pixabay go in this folder, named by the prompt ID
from `docs/ASSET_PROMPTS.md`:

- pictures: `C-01.png`, `A-02.png` (a second good try: `C-01b.png`)
- 3D models: `3d/G-01.glb`
- audio: `audio/music_day.mp3`, `audio/bark_1.mp3` + one line each in `audio/LEDGER.txt`
- real phone screenshots for the store page: `store/`

`manifest.json` lists every prompt ID and, for sheets, the file name of each cell in reading order.
Claude processes this folder with `tools/pipeline/ingest_art.py` (see Part 2 of the prompt
document) and moves the used originals to `art-source/generated/`. Nothing here is read by Unity.
