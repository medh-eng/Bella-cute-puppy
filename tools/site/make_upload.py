#!/usr/bin/env python3
"""Build the Hostinger upload for games.medhaneekahealthcare.in.

Takes the public pages in docs/ and writes self-contained copies (every image
inlined into the HTML) to site-upload/, plus site-upload.zip. Self-contained
pages cannot lose their images through a missed folder during upload.

Only the pages listed in PAGES are published. docs/held-for-launch/ is left out
on purpose: game details stay private until the game is on Google Play.

Usage:  python3 tools/site/make_upload.py
"""
import base64, mimetypes, pathlib, re, shutil, zipfile

ROOT = pathlib.Path(__file__).resolve().parents[2]
DOCS = ROOT / "docs"
OUT = ROOT / "site-upload"
PAGES = ["index.html", "privacy.html", "privacy-bella.html"]

def inline(match):
    attr, rel = match.group(1), match.group(2)
    f = DOCS / rel
    mime = mimetypes.guess_type(f.name)[0]
    data = base64.b64encode(f.read_bytes()).decode()
    return f'{attr}="data:{mime};base64,{data}"'

if OUT.exists():
    shutil.rmtree(OUT)
OUT.mkdir()
for name in PAGES:
    html = (DOCS / name).read_text(encoding="utf-8")
    html = re.sub(r'(src|href)="(assets/[^"]+)"', inline, html)
    assert "assets/" not in html, name
    (OUT / name).write_text(html, encoding="utf-8")
    print(f"{name}: {len(html) // 1024} KB")
with zipfile.ZipFile(ROOT / "site-upload.zip", "w", zipfile.ZIP_DEFLATED) as z:
    for name in PAGES:
        z.write(OUT / name, name)
print("wrote site-upload/ and site-upload.zip")
