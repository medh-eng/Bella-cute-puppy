#!/usr/bin/env python3
"""Print Resources/ size totals and fail if a limit from docs/ASSET_PROMPTS.md §2.0 rule 7 is broken."""
import os, sys
from PIL import Image
RES = os.path.join(os.path.dirname(__file__), "..", "..", "unity-project", "Assets", "Resources")
LIMIT_TOTAL = 60 * 1024 * 1024  # counted without the SKIP icons below
MAXW = {"Icons": 256, "Portraits": 512, "Stickers": 512, "Surfaces": 512, "Places": 1600, "Fx": 256, "Seasons": 384}
# store-listing icons are 1024 px on purpose; they are only read by the editor icon setup, not at runtime
SKIP = {"app_icon.png", "app_icon_foreground.png"}
bad, tot, by = [], 0, {}
for d, _, fs in os.walk(RES):
    for f in fs:
        if f.endswith(".meta"): continue
        p = os.path.join(d, f); sz = os.path.getsize(p)
        if f in SKIP: continue
        tot += sz
        top = os.path.relpath(p, RES).split(os.sep)[0]; by[top] = by.get(top, 0) + sz
        if f not in SKIP and top in MAXW and f.lower().endswith((".png", ".jpg")):
            w = Image.open(p).width
            if w > MAXW[top]: bad.append("%s is %d px wide (max %d)" % (os.path.relpath(p, RES), w, MAXW[top]))
for k in sorted(by): print("%-10s %6.1f MB" % (k, by[k] / 1048576))
print("TOTAL      %6.1f MB (limit 60)" % (tot / 1048576))
if tot > LIMIT_TOTAL: bad.append("Resources over 60 MB")
for b in bad: print("OVER:", b)
sys.exit(1 if bad else 0)
