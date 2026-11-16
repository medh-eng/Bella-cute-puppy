# Website — Medhaneeka Games

Live at **https://games.medhaneekahealthcare.in/**, hosted on Pon's Hostinger plan (not GitHub
Pages, so it keeps working after this repository is made private).

## The rule: nothing about a game before it is on Google Play

Until a game is published, the public site shows only its **name, logo and "Coming soon"**.
No descriptions, features, screenshots or genre labels. (Pon's decision, 16 Nov 2026.)

## What is public now

| Page | Source file | Address |
|---|---|---|
| Games hub: five logos, "Coming soon" | `docs/index.html` | `/` |
| Website privacy policy (general) | `docs/privacy.html` | `/privacy.html` |
| Bella's privacy policy (needed by Play Console before launch) | `docs/privacy-bella.html` | `/privacy-bella.html` |

The five games on the hub: Bella the Cute Puppy, Black Swan 2049, Corebound, Indistock, Vyuha.
Logos are in `docs/assets/logo_*.jpg`.

## What is held back

`docs/held-for-launch/bella.html` is Bella's full page (description, faces, parent notes). It is
**not uploaded**. On launch day it moves back to `docs/bella.html`, gets added to `PAGES` in the
build script, and the hub card links to it.

## Building the upload

```
python3 tools/site/make_upload.py
```

This writes `site-upload/` and `site-upload.zip` (both git-ignored). Each page is a single
self-contained HTML file with its images built in, so there is no assets folder to upload — the
first upload on 16 Nov lost its images that way.

## Uploading (Hostinger File Manager)

1. Open the folder for `games.medhaneekahealthcare.in`.
2. Delete what is there from the earlier upload: `bella.html` and the `assets` folder (if present).
3. Upload `index.html`, `privacy.html` and `privacy-bella.html` from the zip, replacing the old files.
4. Open the site and press refresh; check the five logos show and the footer says "Privacy policy".

## Addresses to use elsewhere

- **Play Console → privacy policy (Bella):** `https://games.medhaneekahealthcare.in/privacy-bella.html`
- **Play Console → store listing → website:** `https://games.medhaneekahealthcare.in/`
- **Company site:** point "Games ↗" and "Explore games" to `https://games.medhaneekahealthcare.in/`
  and remove the old `/games/` page.

## When a game launches

1. Write its privacy policy as `docs/privacy-<game>.html`, list it at the bottom of
   `docs/privacy.html`, add it to `PAGES` in `tools/site/make_upload.py`.
2. Add its page (description, screenshots) and link the hub card to it.
3. Swap its "Coming soon" pill for a link to Google Play. Bella's will be
   `https://play.google.com/store/apps/details?id=com.medhaneeka.bella`
4. Rebuild, upload.
