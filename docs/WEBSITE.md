# Website — Medhaneeka Games

The site's source lives in this repository, in `docs/` (`index.html`, `bella.html`,
`privacy.html`, `assets/`). It's plain HTML — no build step, no trackers, no outside fonts or
scripts.

**Hosting: Hostinger**, on Pon's existing hosting plan, at `games.medhaneekahealthcare.in`. This
was chosen over GitHub Pages because GitHub Pages' free tier requires the repository to stay
public — Hostinger hosting keeps working once the repo is made private again.

## Setting up the subdomain (hPanel, from a phone)

1. **hPanel → Domains → Subdomains** → create `games` under `medhaneekahealthcare.in`. Hostinger
   creates the subdomain and a document-root folder for it (often
   `public_html/games.medhaneekahealthcare.in` or similar — hPanel shows the exact path).
2. **hPanel → Files → File Manager** → open that new folder → **Upload** → upload the zip Claude
   provided (`games-medhaneekahealthcare-in.zip`) → **Extract** it there, so `index.html` sits
   directly in the folder (not inside a subfolder).
3. **hPanel → SSL** → make sure the subdomain has SSL (Hostinger issues one free per subdomain,
   usually automatic within a few minutes).
4. Visit `https://games.medhaneekahealthcare.in/` and `https://games.medhaneekahealthcare.in/privacy.html`
   to confirm both load over HTTPS.

## Keeping it updated

When Claude changes the site, it updates `docs/` in this repo and gives Pon a fresh zip of
`index.html`, `bella.html`, `privacy.html` and `assets/` to upload the same way (step 2 above) —
overwrite the existing files in File Manager, or delete the old ones first and re-extract.

Nothing about this depends on the GitHub repo's visibility, so the repo can be made private at
any point without the site going down.

## Use the subdomain everywhere

- **Play Console → privacy policy:** `https://games.medhaneekahealthcare.in/privacy.html`
- **Play Console → store listing → website:** `https://games.medhaneekahealthcare.in/bella.html`
- **Company site (medhaneekahealthcare.in):** point the **Games ↗** menu item and the
  **Explore games** button to `https://games.medhaneekahealthcare.in/`, and remove the old
  `/games/` page so there's one games page, not two.

## When a game launches

In `docs/index.html`, swap that game's `Coming soon` pill for a link to its Google Play page, then
re-zip and re-upload. For Bella, also turn the "Coming soon to Google Play" badge in
`docs/bella.html` into a link:
`https://play.google.com/store/apps/details?id=com.medhaneeka.bella`

## Later, if the studio grows

The site can move to its own repository and keep the same subdomain and Hostinger folder; nothing
here is tied to the Bella repo specifically.
