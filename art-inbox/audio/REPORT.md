# Audio ingest report — 17 Nov 2026

Produced by tools/pipeline/ingest_audio.py. 'match' is how well the loop's cut point lines up with the start (1.00 = identical shape); 'seam' is the level dip at the join compared with the track's normal level (0 = none). Flagged lines need a listen.

```
music_fest_lights     139.8s -> loop  109.2s (start 0.0, cut 111.7) match 0.46 seam -5.7 dB  gain -9.1 dB (was -9.4 LUFS)  1588 KB
music_fest_colours    111.8s -> loop   93.5s (start 1.0, cut 97.0) match 0.61 seam -6.5 dB  gain -9.4 dB (was -9.1 LUFS)  1338 KB  <-- CHECK BY EAR
music_title            64.8s -> loop   53.8s (start 0.0, cut 56.3) match 1.00 seam -0.6 dB  gain -6.8 dB (was -11.7 LUFS)  759 KB
music_night           159.9s -> loop  154.2s (start 0.0, cut 156.7) match 0.58 seam -3.7 dB  gain -5.7 dB (was -15.3 LUFS)  1894 KB
music_morning          63.2s -> loop   50.5s (start 0.0, cut 53.0) match 0.93 seam -3.9 dB  gain -8.2 dB (was -10.3 LUFS)  741 KB
music_stage           110.0s -> loop  105.0s (start 0.0, cut 107.5) match 0.53 seam -0.1 dB  gain -3.6 dB (was -14.9 LUFS)  1248 KB
music_fest_winter     120.0s -> loop  100.2s (start 0.0, cut 102.7) match 0.68 seam -3.4 dB  gain -11.5 dB (was -7.0 LUFS)  1571 KB
music_garden          141.2s -> loop  115.5s (start 0.0, cut 118.0) match 0.42 seam -2.3 dB  gain -7.0 dB (was -11.5 LUFS)  1852 KB
music_fest_party       72.1s -> loop   60.8s (start 0.0, cut 63.3) match 0.68 seam +1.2 dB  gain -8.6 dB (was -9.9 LUFS)  785 KB
music_play             62.0s -> loop   40.5s (start 0.0, cut 43.0) match 0.65 seam -2.8 dB  gain -7.4 dB (was -11.1 LUFS)  514 KB
music_evening          94.2s -> loop   78.5s (start 0.0, cut 81.0) match 0.66 seam -0.9 dB  gain -1.3 dB (was -17.2 LUFS)  935 KB
music_bath            139.9s -> loop  121.9s (start 0.0, cut 124.4) match 0.77 seam -0.9 dB  gain -3.7 dB (was -14.8 LUFS)  1626 KB
bark_2                1.01s  gain -0.6 dB  peak -4.7 dBFS  86 KB
music_shop            112.6s -> loop   87.9s (start 0.0, cut 90.4) match 0.71 seam -2.4 dB  gain -3.4 dB (was -15.1 LUFS)  1101 KB
yip_1                 1.50s  gain -8.9 dB  peak -8.7 dBFS  129 KB
whine_sad_1           1.31s  gain +2.7 dB  peak -8.4 dBFS  112 KB
whine_sad_2           2.21s  gain -1.3 dB  peak -2.8 dBFS  190 KB
whine_sad_3           1.54s  gain -3.0 dB  peak -4.3 dBFS  132 KB
whine_sad_4           2.59s  gain +11.8 dB  peak -5.1 dBFS  223 KB
music_day             155.0s -> loop  139.8s (start 0.0, cut 142.3) match 0.66 seam -4.8 dB  gain -7.8 dB (was -10.7 LUFS)  2063 KB
```

## Batch 2 — 17 Nov 2026 (11 uploads → sounds)

```
sniff_1               0.79s  gain +22.0 dB  peak -4.6 dBFS  68 KB
sniff_2               0.72s  gain +16.1 dB  peak -7.0 dBFS  62 KB
sniff_3               0.96s  gain +19.3 dB  peak -5.2 dBFS  82 KB
munch_1               3.33s  gain +5.3 dB  peak -2.0 dBFS  286 KB
munch_2               1.07s  gain +7.4 dB  peak -2.0 dBFS  92 KB
munch_3               1.21s  gain +12.6 dB  peak -2.0 dBFS  104 KB
munch_4               2.36s  gain +5.0 dB  peak -2.0 dBFS  203 KB
  removed superseded synthesised munch.wav (reproducible with gen_audio.py)
yawn_sleepy           2.50s  gain -0.1 dB  peak -2.0 dBFS  215 KB
bark_happy_1          2.86s  gain +1.8 dB  peak -2.0 dBFS  246 KB
  removed superseded synthesised bark_happy.wav (reproducible with gen_audio.py)
bark_1                1.01s  gain -2.3 dB  peak -5.0 dBFS  86 KB
bark_3                1.15s  gain +0.3 dB  peak -4.9 dBFS  98 KB
lap                   1.53s  gain -2.4 dB  peak -2.0 dBFS  131 KB
bark_happy_2          2.16s  gain +1.6 dB  peak -2.0 dBFS  186 KB
bark_happy_3          2.55s  gain +4.9 dB  peak -2.0 dBFS  219 KB
bark_happy_4          2.17s  gain +3.9 dB  peak -2.0 dBFS  186 KB
bark_happy_5          2.28s  gain +1.5 dB  peak -2.0 dBFS  196 KB
pant                  4.26s  gain +14.6 dB  peak -2.0 dBFS  366 KB
whimper_hungry_1      1.49s  gain +8.5 dB  peak -7.0 dBFS  128 KB
whimper_hungry_2      1.12s  gain +11.0 dB  peak -2.0 dBFS  96 KB
whimper_hungry_3      1.40s  gain +9.2 dB  peak -3.3 dBFS  120 KB
  removed superseded synthesised whimper_hungry.wav (reproducible with gen_audio.py)
snore_1               1.42s  gain +2.7 dB  peak -2.0 dBFS  122 KB
snore_2               1.35s  gain +4.5 dB  peak -2.0 dBFS  116 KB
snore_3               1.36s  gain -4.3 dB  peak -2.0 dBFS  117 KB
snore_4               3.08s  gain -2.5 dB  peak -2.0 dBFS  265 KB
snore_5               1.37s  gain +5.4 dB  peak -2.0 dBFS  118 KB
snore_6               3.39s  gain -1.4 dB  peak -2.0 dBFS  292 KB
  removed superseded synthesised snore.wav (reproducible with gen_audio.py)
```
