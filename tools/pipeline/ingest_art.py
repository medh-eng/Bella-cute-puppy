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
READY = os.path.join(ROOT, "art-source", "ready")   # processed, game-ready, waiting for code to use them
PLACES_READY = {"F-10": "festival", "F-11": "studio_pink", "F-12": "studio_rainbow"}
RUGS = ["rug_rainbow", "rug_paw", "rug_donut", "rug_flower"]
VIEWS = ["view_hills", "view_town", "view_beach", "view_snow"]
FX = {"H-01": ["fx_heart", "fx_star", "fx_sparkle", "fx_bubble", "fx_drop", "fx_note", "fx_paw", "fx_zzz", "fx_ring"],
      "H-02": ["fx_glow", "fx_rays", "fx_puff", "fx_foam", "fx_dust", "fx_beam", "fx_flare", "fx_smoke", "fx_dot"],
      "H-03": ["fx_confetti_1", "fx_confetti_2", "fx_confetti_3", "fx_petal", "fx_leaf", "fx_snow", "fx_rain",
               "fx_firework", "fx_splat"]}
SEASONS = {"I-01": "lights", "I-02": "colours", "I-03": "winter", "I-04": "birthday"}
SURF["E-14"] = "floor_stage"
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
    # the n biggest blobs are the pictures; everything smaller (confetti, sparks, bubbles) joins its nearest picture
    order = sorted(range(len(area)), key=lambda i: -area[i])
    if len(order) < n or area[order[n - 1]] < 0.002 * a.size:
        return None, int(sum(1 for x in area if x >= 0.002 * a.size))
    keep = set(order[:n]); min_area = area[order[n - 1]]
    big = [[objs[i][1].start, objs[i][0].start, objs[i][1].stop, objs[i][0].stop] for i in sorted(keep)]
    for i, (o, ar) in enumerate(zip(objs, area)):
        if i in keep: continue
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


def save_ready(im, rel, **kw):
    p = os.path.join(READY, rel); os.makedirs(os.path.dirname(p), exist_ok=True); im.save(p, optimize=True, **kw); return p


def clean_alpha(im, lo=70, hi=200):
    """AI 'transparent' sheets often carry a faint haze (alpha 30-60) behind everything: remove it"""
    im = im.convert("RGBA"); a = np.asarray(im).copy()
    al = a[..., 3].astype(np.float32)
    a[..., 3] = (np.clip((al - lo) / (hi - lo), 0, 1) * 255).astype(np.uint8)
    # only keep pixels within a few px of something solid, so leftover haze patches cannot survive
    near = ndimage.binary_dilation(al > 200, iterations=6)
    a[..., 3] = np.where(near, a[..., 3], 0)
    return Image.fromarray(a)


def grid_boxes(im, n, cols):
    """equal grid, each cell trimmed to what is in it (sheets that ask for 'each centred in its own cell')"""
    rgba = im.convert("RGBA"); w, h = rgba.size; rows = (n + cols - 1) // cols
    out = []
    for i in range(n):
        c, r = i % cols, i // cols
        out.append((c * w // cols, r * h // rows, (c + 1) * w // cols, (r + 1) * h // rows))
    return out


def opaque_grid(im, n, cols):
    """sheet of opaque pictures on a flat background: split on background-coloured rows and columns"""
    a = np.asarray(im.convert("RGB")).astype(int); bg = a[2, 2]
    fg = np.abs(a - bg).sum(axis=2) > 30
    def runs(v):
        r, st = [], None
        for i, x in enumerate(v):
            if x and st is None: st = i
            if not x and st is not None:
                if i - st > 40: r.append((st, i))
                st = None
        if st is not None and len(v) - st > 40: r.append((st, len(v)))
        return r
    xs, ys = runs(fg.mean(axis=0) > 0.3), runs(fg.mean(axis=1) > 0.3)
    if len(xs) != cols or len(ys) * cols != n: return None
    return [(x0, y0, x1, y1) for (y0, y1) in ys for (x0, x1) in xs]


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
        elif pid in PLACES_READY:
            im = im.convert("RGB").resize((1600, 1000), Image.LANCZOS)
            save_ready(im, "Places/%s.jpg" % PLACES_READY[pid], quality=84); done.append((pid, PLACES_READY[pid], "ready/ (no code uses it yet)"))
        elif pid == "E-15":
            rgba = clean_alpha(im); boxes, got = cut_sheet(rgba, 4, 2)
            if boxes is None: rejected.append((pid, "found %d rugs, expected 4" % got)); continue
            for nm, b in zip(RUGS, boxes):
                save_ready(finish(rgba.crop(b), 512, 0.02), "Surfaces/%s.png" % nm); done.append((pid, nm, "ready/ 512px"))
        elif pid == "E-16":
            rgba = clean_alpha(im, 20, 90); boxes, got = cut_sheet(rgba, 4, 1)
            if boxes is None: rejected.append((pid, "found %d strips, expected 4" % got)); continue
            for nm, b in zip(VIEWS, boxes):
                c = rgba.crop(b); c = c.crop(c.getbbox()); k = 768 / c.width
                c = c.resize((768, max(1, round(c.height * k))), Image.LANCZOS)
                canvas = Image.new("RGBA", (768, 320), (0, 0, 0, 0)); canvas.paste(c, (0, 320 - min(c.height, 320)))   # sits on the window sill
                save_ready(canvas, "Surfaces/%s.png" % nm); done.append((pid, nm, "ready/ 768x320"))
        elif pid == "E-17":
            boxes = opaque_grid(im, 9, 3)
            if boxes is None: rejected.append((pid, "could not find a 3x3 grid of pictures")); continue
            for i, b in enumerate(boxes):
                c = im.convert("RGB").crop(b).resize((256, 256), Image.LANCZOS)
                save_ready(c, "Surfaces/pic_%d.jpg" % (i + 1), quality=88); done.append((pid, "pic_%d" % (i + 1), "ready/ 256px"))
        elif pid in FX:
            rgba = im.convert("RGBA")
            for nm, b in zip(FX[pid], grid_boxes(rgba, 9, 3)):
                save_ready(finish(rgba.crop(b), 256, 0.04), "Fx/%s.png" % nm); done.append((pid, nm, "ready/ 256px"))
        elif pid in SEASONS:
            rgba = clean_alpha(im); boxes, got = cut_sheet(rgba, 9, 3)
            if boxes is None: rejected.append((pid, "found %d pictures, expected 9" % got)); continue
            for i, b in enumerate(boxes):
                save_ready(finish(rgba.crop(b), 384), "Seasons/%s_%d.png" % (SEASONS[pid], i + 1)); done.append((pid, "%s_%d" % (SEASONS[pid], i + 1), "ready/ 384px"))
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
