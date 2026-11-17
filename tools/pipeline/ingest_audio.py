#!/usr/bin/env python3
"""
Turn downloaded music / sound files into game-ready audio for Resources/Audio.

  python3 ingest_audio.py --src <folder with mp3/wav/ogg> --out <Resources/Audio> --ledger <LEDGER.txt>

Needs ffmpeg and numpy only. File names (after an optional "1a2b3c4d-" upload prefix) become the
game's clip names: music_*  -> looping stereo OGG, loudness-matched to the game's own tracks
                   amb_*    -> looping stereo OGG, quiet
                   anything else -> short mono 44.1 kHz WAV (trimmed, faded, level-matched)
Names ending in _1, _2 ... are variants of one sound; the game picks one at random.

What it does to music (and why):
  * Real tracks end with a fade-out, which would leave a dip of silence every time the loop
    restarts. It finds where the music stops being "full", then looks for the cut point whose last
    3 seconds line up best (by rhythm/loudness shape) with the first 3 seconds of the music, and
    cross-fades the two. The result loops with no dip and no click.
  * Everything is set to the same loudness as the earlier synthesised tracks (the game's music
    volume was tuned on those) and kept clear of clipping.
Run it again any time; it is safe to repeat. Superseded synthesised .wav files are removed.
"""
import argparse, hashlib, os, re, subprocess, sys, wave
import numpy as np

SR = 44100
XF = 2.5                      # loop cross-fade, seconds
MUSIC_TARGET = {"music_night": -21.0}
MUSIC_DEFAULT = -18.5
AMB_TARGET = -22.0
VOICE_TARGET = -16.0          # LUFS for sounds, with the peak ceiling below
SFX_PEAK_DB = -2.0
SPLIT = {"whine_sad"}         # recordings that hold several separate whines -> variants


def run(cmd, data=None):
    return subprocess.run(cmd, input=data, capture_output=True, check=True)


def decode(path):
    raw = run(["ffmpeg", "-v", "error", "-i", path, "-f", "f32le", "-ac", "2", "-ar", str(SR), "-"]).stdout
    return np.frombuffer(raw, dtype="<f4").reshape(-1, 2).copy()


def lufs(x):
    if len(x) < SR // 2:
        return None
    p = subprocess.run(["ffmpeg", "-hide_banner", "-nostats", "-f", "f32le", "-ar", str(SR), "-ac", "2", "-i", "-",
                        "-af", "ebur128", "-f", "null", "-"], input=x.astype("<f4").tobytes(), capture_output=True)
    m = re.findall(r"I:\s+(-?[\d.]+) LUFS", p.stderr.decode())
    if not m:
        return None
    v = float(m[-1])
    return v if v > -69 else None


def peak_db(x):
    return 20 * np.log10(max(float(np.max(np.abs(x))), 1e-9))


def db_windows(mono, win, hop):
    sq = np.concatenate([[0.0], np.cumsum(mono.astype(np.float64) ** 2)])
    starts = np.arange(0, len(mono) - win, hop)
    ms = (sq[starts + win] - sq[starts]) / win
    return starts, 10 * np.log10(ms + 1e-12)


def gain_for(x, target):
    L = lufs(x)
    g = (target - L) if L is not None else (-3.0 - peak_db(x))
    g = min(g, SFX_PEAK_DB - peak_db(x)) if target == VOICE_TARGET else g
    out = x * (10 ** (g / 20))
    pk = float(np.max(np.abs(out)))
    if pk > 0.89:                       # keep ~1 dB below full scale
        out = out * (0.89 / pk)
    return out, (L, g)


# ---------------------------------------------------------------- music loops

def make_loop(x):
    """Return (loop, info) — a seamless loop of x."""
    mono = x.mean(axis=1)
    n = len(mono)
    st, lv = db_windows(mono, SR, SR // 4)
    med = float(np.median(lv))
    full = lv >= med - 10.0
    first = int(st[np.argmax(full)])
    last = int(st[len(full) - 1 - np.argmax(full[::-1])]) + SR     # end of the last "full" second
    s0 = 0 if first < int(0.3 * SR) else first
    E = min(last, n)
    X = int(XF * SR)
    if E - s0 < 3 * X + 20 * SR:
        # too short to hunt for a loop point: plain cross-fade of the tail into the start
        t, score = E, float("nan")
    else:
        fr = 441
        nf = n // fr
        env = np.sqrt((mono[:nf * fr].reshape(nf, fr) ** 2).mean(axis=1))
        nX = X // fr
        f0 = s0 // fr
        head = env[f0:f0 + nX]
        head = (head - head.mean()) / (head.std() + 1e-9)
        lo = max((s0 + X) // fr + nX + 1, (E - 25 * SR) // fr)
        hi = E // fr
        best, t, score = -2, E, 0
        for tf in range(lo, hi):
            tail = env[tf - nX:tf]
            c = float(np.dot((tail - tail.mean()) / (tail.std() + 1e-9), head) / nX)
            if c > best:
                best, t = c, tf * fr
        score = best
    tail = x[t - X:t]
    head = x[s0:s0 + X]
    k = np.linspace(0, np.pi / 2, X)[:, None]
    blend = tail * np.cos(k) + head * np.sin(k)          # equal-power
    loop = np.concatenate([x[s0 + X:t - X], blend]).astype(np.float32)
    # seam check: level around the join, against the track's normal level
    two = np.concatenate([loop, loop])
    seam = len(loop)
    around = two[seam - SR // 2:seam + SR // 2]
    seam_db = 10 * np.log10(float(np.mean(around.mean(axis=1) ** 2)) + 1e-12)
    jump = float(np.max(np.abs(two[seam] - two[seam - 1])))
    info = dict(start=s0 / SR, cut=t / SR, orig=n / SR, loop=len(loop) / SR, match=score,
                seam_vs_median_db=seam_db - med, jump=jump)
    return loop, info


# ---------------------------------------------------------------- short sounds

def trim(x, thr_db=-40.0, head_pad=0.015, tail_pad=0.12):
    mono = np.abs(x).max(axis=1)
    pk = float(mono.max())
    idx = np.where(mono > pk * 10 ** (thr_db / 20))[0]
    if len(idx) == 0:
        return x
    a = max(0, int(idx[0] - head_pad * SR))
    b = min(len(x), int(idx[-1] + tail_pad * SR))
    return x[a:b]


def fade(x, fin=0.006, fout=0.05):
    x = x.copy()
    a, b = int(fin * SR), int(fout * SR)
    if a and len(x) > a:
        x[:a] *= np.linspace(0, 1, a)[:, None]
    if b and len(x) > b:
        x[-b:] *= np.linspace(1, 0, b)[:, None]
    return x


def split_events(x, thr_db=-42.0, min_len=0.5, merge_gap=0.12):
    mono = x.mean(axis=1)
    fr = SR // 100
    nf = len(mono) // fr
    env = 10 * np.log10((mono[:nf * fr].reshape(nf, fr) ** 2).mean(axis=1) + 1e-12)
    act = env > thr_db
    g = int(merge_gap * 100)
    if g:                                   # close pauses shorter than merge_gap
        act = np.convolve(act.astype(float), np.ones(2 * g + 1), mode="same") > 0
    ev, i = [], 0
    while i < nf:
        if act[i]:
            j = i
            while j < nf and act[j]:
                j += 1
            ev.append([i, j])
            i = j
        else:
            i += 1
    out = []
    for a, b in ev:
        if (b - a) / 100 >= min_len:
            out.append(x[max(0, a * fr - int(0.04 * SR)):min(len(x), b * fr + int(0.12 * SR))])
    return out


def write_wav(path, x):
    mono = x.mean(axis=1)
    pcm = (np.clip(mono, -1, 1) * 32767).astype("<i2")
    with wave.open(path, "wb") as w:
        w.setnchannels(1); w.setsampwidth(2); w.setframerate(SR)
        w.writeframes(pcm.tobytes())


def write_ogg(path, x):
    run(["ffmpeg", "-y", "-v", "error", "-f", "f32le", "-ar", str(SR), "-ac", "2", "-i", "-",
         "-c:a", "libvorbis", "-q:a", "3", "-map_metadata", "-1", path], x.astype("<f4").tobytes())


# ---------------------------------------------------------------- main

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--src", required=True)
    ap.add_argument("--out", required=True)
    ap.add_argument("--ledger", required=True)
    ap.add_argument("--source", default="supplied by Pon; origin and licence to be confirmed before release")
    ap.add_argument("--date", default="")
    a = ap.parse_args()

    files = sorted(f for f in os.listdir(a.src) if f.lower().endswith((".mp3", ".wav", ".ogg", ".flac", ".m4a")))
    ledger = open(a.ledger).read() if os.path.exists(a.ledger) else ""
    new_ledger = []
    report = []
    for f in files:
        stem = re.sub(r"^[0-9a-f]{8}-", "", os.path.splitext(f)[0])
        path = os.path.join(a.src, f)
        sha = hashlib.sha256(open(path, "rb").read()).hexdigest()[:12]
        x = decode(path)
        made = []
        if stem.startswith(("music_", "amb_")):
            loop, info = make_loop(x)
            target = AMB_TARGET if stem.startswith("amb_") else MUSIC_TARGET.get(stem, MUSIC_DEFAULT)
            y, (L, g) = gain_for(loop, target)
            dst = os.path.join(a.out, stem + ".ogg")
            write_ogg(dst, y)
            old = os.path.join(a.out, stem + ".wav")
            for junk in (old, old + ".meta"):
                if os.path.exists(junk):
                    os.remove(junk)
            made.append(stem + ".ogg")
            flag = ""
            if not (info["match"] == info["match"]) or info["match"] < 0.25 or info["seam_vs_median_db"] < -6:
                flag = "  <-- CHECK BY EAR"
            report.append(f"{stem:20s} {info['orig']:6.1f}s -> loop {info['loop']:6.1f}s (start {info['start']:.1f}, cut {info['cut']:.1f}) "
                          f"match {info['match']:.2f} seam {info['seam_vs_median_db']:+.1f} dB  gain {g:+.1f} dB (was {L} LUFS)"
                          f"  {os.path.getsize(dst)//1024} KB{flag}")
        else:
            pieces = split_events(x) if stem in SPLIT else [trim(x)]
            if stem in SPLIT and pieces:        # drop faint background bits that would only be noise once levelled
                top = max(float(np.max(np.abs(p))) for p in pieces)
                pieces = [p for p in pieces if float(np.max(np.abs(p))) >= top * 10 ** (-18 / 20)]
            names = [f"{stem}_{i + 1}" for i in range(len(pieces))] if stem in SPLIT else [stem]
            for nm, p in zip(names, pieces):
                p = fade(p)
                y, (L, g) = gain_for(p, VOICE_TARGET)
                dst = os.path.join(a.out, nm + ".wav")
                write_wav(dst, y)
                made.append(nm + ".wav")
                report.append(f"{nm:20s} {len(p)/SR:5.2f}s  gain {g:+.1f} dB  peak {peak_db(y):.1f} dBFS  {os.path.getsize(dst)//1024} KB")
            # a recording or variants supersede the synthesised sound of the base name
            base = re.sub(r"_\d+$", "", stem)
            old = os.path.join(a.out, base + ".wav")
            if os.path.exists(old) and (base != stem or stem in SPLIT):
                os.remove(old)
                report.append(f"  removed superseded synthesised {base}.wav (reproducible with gen_audio.py)")
        for m in made:
            line = f"{m} | original upload: {stem}{os.path.splitext(f)[1]} sha256:{sha} | {a.source} | {a.date}"
            if f"sha256:{sha}" not in ledger:
                new_ledger.append(line)
    if new_ledger:
        with open(a.ledger, "a") as fh:
            fh.write("\n".join(new_ledger) + "\n")
    print("\n".join(report))


if __name__ == "__main__":
    main()
