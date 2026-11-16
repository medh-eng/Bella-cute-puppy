# Website — Medhaneeka Games

The games site lives in this repository's `docs/` folder and is served free by GitHub Pages.
Nothing has to be uploaded anywhere: a push to `main` updates the live site within a minute or two.

| Page | File | Address today |
|---|---|---|
| Games hub (Bella, Corebound, Indistock, Vyuha) | `docs/index.html` | `https://medh-eng.github.io/Bella-cute-puppy/` |
| Bella's own page | `docs/bella.html` | `https://medh-eng.github.io/Bella-cute-puppy/bella.html` |
| Bella privacy policy (the address Play Console needs) | `docs/privacy.html` | `https://medh-eng.github.io/Bella-cute-puppy/privacy.html` |

Images are in `docs/assets/`. The pages use no trackers, no cookies, no outside fonts or scripts.
All links between pages are relative, so the site works at any address.

## Giving it its own subdomain: `games.medhaneekahealthcare.in`

Two steps, both from a phone. Do them in this order.

**1. At the place where the domain's DNS is managed** (the registrar or hosting panel for
`medhaneekahealthcare.in` → "DNS" / "Zone editor" → Add record):

| Type | Name / Host | Value / Points to | TTL |
|---|---|---|---|
| CNAME | `games` | `medh-eng.github.io` | default |

**2. On GitHub:** repository → **Settings → Pages → Custom domain** → type
`games.medhaneekahealthcare.in` → **Save**. Wait for the DNS check to turn green (minutes to a few
hours), then tick **Enforce HTTPS**.

GitHub adds a file called `docs/CNAME` by itself — leave it alone. Do **not** create that file
before the DNS record exists, or the site goes offline until it does.

Afterwards the old `medh-eng.github.io/Bella-cute-puppy/...` addresses forward to the new ones
automatically, so the privacy link already given to Google keeps working.

## After the subdomain is live — changes on the main company site

On `medhaneekahealthcare.in` (the uploaded theme):
- Point the **Games ↗** menu item and the **Explore games** button to `https://games.medhaneekahealthcare.in/`
- Remove the `/games/` page, or replace it with a redirect, so there is one games page, not two

## When a game launches

In `docs/index.html`, swap that game's `Coming soon` pill for a link to its Google Play page.
For Bella, also change the "Coming soon to Google Play" badge in `docs/bella.html` into a link:
`https://play.google.com/store/apps/details?id=com.medhaneeka.bella`

## Later, if the studio grows

The hub can move to its own repository (for example `medh-eng/games-site`) and take the subdomain
with it; Bella's pages would move along. Nothing about today's setup blocks that.
