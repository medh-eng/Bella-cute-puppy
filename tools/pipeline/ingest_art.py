#!/usr/bin/env python3
"""Turn Pon's ChatGPT/Gemini downloads (named by prompt ID) into game files.

usage: python3 tools/pipeline/ingest_art.py <folder with E-01.png, F-01.png, D-08.png ...>

Accepted IDs are listed in PLAN below. Sheets are cut by connected blobs of alpha (not by grid
lines, AI sheets are never exact); if the blob count differs from the expected cell count the
sheet is reported and skipped. Tileable surfaces get a seam check and, if needed, an
offset-and-blend repair. Rejected/unknown IDs are listed, never guessed.
Output goes under unity-project/Assets/Resources/; originals are copied to art-source/generated/.
"""
import sys, os, glob, shutil
import numpy as np
from PIL import Image
from scipy import ndimage

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
RES = os.path.join(ROOT, "unity-project", "Assets", "Resources")
GEN = os.path.join(ROOT, "art-source", "generated")

SURF = {"E-01": "wall_paws", "E-02": "wall_clouds", "E-03": "wall_hearts", "E-04": "wall_night",
        "E-05": "wall_jungle", "E-06": "wall_sunny", "E-07": "wall_rainbow", "E-08": "wall_tiles",
        "E-09": "floor_wood", "E-10": "floor_checker", "E-11": "floor_carpet", "E-12": "floor_grass",
        "E-13": "floor_sand"}
PLACES = {"F-01": "garden", "F-02": "park", "F-03": "bathroom", "F-04": "kitchen", "F-05": "clinic",
          "F-06": "stage", "F-07": "beach", "F-08": "bedroom_night", "F-09": "snow"}
SHEETS = {  # id -> (dir, cells in reading order, grid cols, size)
    "D-06": ("Stickers", ["fest_lights", "fest_colours", "fest_winter", "fest_birthday", "fest_kites",
                          "fest_harvest", "fest_newyear", "fest_monsoon", "fest_spring"], 3, 512),
    "D-08": ("UI", ["wish_feed", "wish_bath", "wish_sleep", "wish_play", "wish_pet", "wish_tricks",
                    "wish_clinic", "wish_garden", "wish_wardrobe"], 3, 256)}


def seam(a):
    """mean abs difference across the wrap (right->left, bottom->top) vs. inside the image"""
    f = a.astype(np.float32)
    wrap = (np.abs(f[:, 0] - f[:, -1]).mean() + np.abs(f[0] - f[-1]).mean()) / 2
    inner = (np.abs(f[:, 1] - f[:, 0]).mean() + np.abs(f[1] - f[0]).mean()) / 2
    return wrap, inner


def make_tileable(im):
    """shift by half and blend a soft band across the old seam using the un-shifted image"""
    a = np.asarray(im.convert("RGB")).astype(np.float32)
    h, w, _ = a.shape
    sh = np.roll(np.roll(a, h // 2, 0), w // 2, 1)       # seam now in the middle
    yy, xx = np.mgrid[0:h, 0:w]
    band = 0.08
    wx = np.clip(np.abs(xx - w / 2) / (band * w), 0, 1)   # 0 at centre line
    wy = np.clip(np.abs(yy - h / 2) / (band * h), 0, 1)
    wgt = np.minimum(wx, wy)[..., None]                   # 1 away from the seams: use shifted image
    # blend towards the original (whose edges are interior there, but whose centre is far from its own seam)
    out = sh * wgt + np.roll(np.roll(a, 0, 0), 0, 1) * (1 - wgt)
    return Image.fromarray(np.clip(out, 0, 255).astype(np.uint8)), out


def save_res(im, rel, **kw):
    p = os.path.join(RES, rel); os.makedirs(os.path.dirname(p), exist_ok=True); im.save(p, **kw); return p


def cut_sheet(im, n, cols):
    """blobs of alpha; tiny blobs (sparkles, bubbles) join the nearest big one; None if count != n"""
    a = np.asarray(im.convert("RGBA"))[..., 3] > 24
    lab, k = ndimage.label(a)
    objs = ndimage.find_objects(lab)
    area = ndimage.sum(a, lab, range(1, k + 1))
    min_area = 0.002 * a.size
    big = [[o[1].start, o[0].start, o[1].stop, o[0].stop] for o, ar in zip(objs, area) if ar >= min_area]
    if len(big) != n:
        return None, len(big)
    for o, ar in zip(objs, area):
        if ar >= min_area: continue
        cx, cy = (o[1].start + o[1].stop) / 2, (o[0].start + o[0].stop) / 2
        d = lambda b: max(b[0] - cx, 0, cx - b[2]) ** 2 + max(b[1] - cy, 0, cy - b[3]) ** 2
        t = min(big, key=d)
        t[0], t[1], t[2], t[3] = min(t[0], o[1].start), min(t[1], o[0].start), max(t[2], o[1].stop), max(t[3], o[0].stop)
    boxes = sorted(big, key=lambda b: (b[1] + b[3]) / 2)
    rows = [sorted(boxes[i:i + cols], key=lambda b: b[0]) for i in range(0, n, cols)]
    return [tuple(b) for r in rows for b in r], n


def finish(cell, size, pad=0.06):
    bb = cell.getbbox()
    cell = cell.crop(bb)
    s = int(max(cell.size) * (1 + 2 * pad))
    sq = Image.new("RGBA", (s, s), (0, 0, 0, 0)); sq.paste(cell, ((s - cell.width) // 2, (s - cell.height) // 2))
    return sq.resize((size, size), Image.LANCZOS)


def main(src):
    done, rejected = [], []
    for f in sorted(glob.glob(os.path.join(src, "**", "*.png"), recursive=True)):
        pid = os.path.splitext(os.path.basename(f))[0]
        im = Image.open(f)
        if pid in SURF:
            im = im.convert("RGB"); w0, i0 = seam(np.asarray(im))
            if w0 > i0 * 3 + 6:                       # visible seam
                im, _ = make_tileable(im); note = "seam repaired"
            else: note = "seam ok"
            im = im.resize((512, 512), Image.LANCZOS)
            save_res(im, "Surfaces/%s.jpg" % SURF[pid], quality=88); done.append((pid, SURF[pid], note))
        elif pid in PLACES:
            im = im.convert("RGB").resize((1600, 1000), Image.LANCZOS)
            save_res(im, "Places/%s.jpg" % PLACES[pid], quality=84); done.append((pid, PLACES[pid], "1600x1000"))
        elif pid in SHEETS:
            d, names, cols, size = SHEETS[pid]
            boxes, got = cut_sheet(im.convert("RGBA"), len(names), cols)
            if boxes is None: rejected.append((pid, "found %d pictures, expected %d" % (got, len(names)))); continue
            rgba = im.convert("RGBA")
            for nm, b in zip(names, boxes):
                save_res(finish(rgba.crop(b), size), "%s/%s.png" % (d, nm)); done.append((pid, nm, "%dpx" % size))
        elif pid == "D-07":
            im = im.convert("RGB").resize((1536, 768), Image.LANCZOS)
            save_res(im, "UI/growth.jpg", quality=86); done.append((pid, "growth", "1536x768"))
        else:
            rejected.append((pid, "not accepted / no recipe")); continue
        os.makedirs(GEN, exist_ok=True); shutil.copy2(f, os.path.join(GEN, os.path.basename(f)))
    for d in done: print("OK  ", *d)
    for r in rejected: print("SKIP", *r)


if __name__ == "__main__":
    main(sys.argv[1])
