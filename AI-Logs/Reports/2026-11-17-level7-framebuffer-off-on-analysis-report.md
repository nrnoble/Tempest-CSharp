# Level 7 Atari OFF/ON framebuffer analysis

Date: 2026-11-17
Status: **COMPLETE — READ-ONLY ANALYSIS**
Classification: **A. DEFECT PRESENT IN SUBMITTED GEOMETRY** for the measured
scale-relative apparent slowdown. This does not establish a rail-distance
motion law.

## Result

Neal's established manual A/B result is consistent with the synchronized
evidence: Atari OFF played normally; Atari ON showed the known presentation
problem. The prior deterministic-state report establishes identical
simulation state at all 3,511 boundaries, so simulation determinism was not
reopened.

The framebuffer reproduces the selected submitted entity geometry. For the
longest stable depth trajectory, Atari ON's submitted Tanker anchor advances
about 1.17 screen pixels per playback boundary at all four sampled depths,
while its submitted glyph width grows from 20.90 to 57.38 pixels. Anchor
travel per glyph width therefore falls from 0.056 to 0.020 (about 64%).
Rasterized feature-centroid motion shows the same overall fall, about 0.058
to 0.026. Atari OFF's corresponding submitted anchor motion grows from about
0.13 to 1.20 pixels per boundary as its glyph grows from 14.15 to 37.33
pixels. These are within-mode comparisons; the two modes intentionally use
different projections.

Thus the measured scale-relative slowdown is already in Atari-ON submitted
presentation geometry, not introduced by rasterization. This is not a claim
that Atari-ON absolute anchor speed decreases: it remains nearly constant.
Glyph width is an available, meaningful scale normalization, but is not a
measured fraction of the lane's remaining rail-to-rim distance.

## Capture completeness

| Mode | Runtime mode / presentation | Playback path | Requested / captured | Missed | PNGs | Correlated entity records | Boundary 301 |
|---|---|---|---:|---:|---:|---:|---|
| OFF | recorded playback / ExperimentalPortrait | `TEMP\GameplayRecordings\Level-7-slowmo.json` | 15 / 15 | none | 15 | 372, all linked | reached |
| ON | recorded playback / ExperimentalPortrait | `TEMP\GameplayRecordings\Level-7-slowmo.json` | 15 / 15 | none | 15 | 372, all linked | reached |

Both run records name the same playback file, selected windows, and exact
boundary list: 99–101, 149–151, 199–201, 249–251, and 299–301. All 15 frame
records in each trace have a matching capture record and PNG; file sizes
match capture metadata. Every PNG decoded successfully as a 1280x960 PNG.
All PNG chunk CRCs and compressed raster rows validated. Each trace contains
15 frame records and 372 entity records correlated by render-frame sequence
and boundary, with no unlinked frames.

Each trace has a `runEnded` record, but no explicit capture-complete or
capture-status record. The JSONL therefore cannot establish whether the
console completion message was emitted. All requested frames and files are
present, so this is not evidence of an incomplete capture and no rerun is
needed.

## Readback timing and observer effect

Durations are min / median / max across the 15 captured frames.

| Mode | Readback (ms) | PNG write (ms) | Total capture (ms) | Presented-call interval (s) | Render call (s) |
|---|---:|---:|---:|---:|---:|
| OFF | 1.54 / 1.79 / 2.93 | 54.15 / 55.57 / 57.37 | 55.93 / 57.55 / 59.67 | 0.0572 / 0.0598 / 0.0719 | 0.0567 / 0.0586 / 0.0614 |
| ON | 1.57 / 1.79 / 3.36 | 55.25 / 56.10 / 59.62 | 57.20 / 58.05 / 61.43 | 0.0581 / 0.0601 / 0.0742 | 0.0578 / 0.0589 / 0.0626 |

PNG export adds a measurable, recurring roughly 55–60 ms to each selected
render call. There is no isolated large capture-duration outlier; the
maximum capture duration is only modestly above the median in either mode.
This synchronous observer cost is consistent with Neal seeing no obvious
temporary stall, but it should be remembered when interpreting perceived
playback pacing. It does not explain the measured per-depth geometry
progression.

## Pixel method and usable tracks

Only the PNGs in the two authorized output folders were decoded. For each
sample, renderer-submitted `cameraTransformedCoordinates` constrained the
search to that entity's `hostGeometryBounds`. Submitted line segments were
sampled every 0.5 pixel; a foreground pixel (maximum RGB channel above 12)
within a 3x3 neighborhood counted as a raster match. Coverage was 98.0–100%
for the measured Tanker, Spiker, and shot segments (100% median in each
mode). Raster feature positions were intensity-weighted centroids of
foreground pixels within 1.5 pixels of those traced segments. This rejects
unrelated bright pixels outside the submitted geometry, but antialiasing,
overlap, and subpixel motion still cause centroid quantization.

- **Primary:** Tanker 15, `Climbing`, present in all five three-boundary
  windows except the last. It spans height 59–325 over the first four windows.
  At the final window it has been replaced by Tankers 44 and 45; their
  three-frame fragments were measured separately, not joined to Tanker 15.
- **Corroboration:** Spiker 13 is identifiable in all five windows, though
  its state/height progression is not one monotone trajectory across the
  separated windows.
- **Same-frame reference:** player shot 8 is identifiable in the first
  window. It is not treated as an enemy trajectory.

For Tanker 15, the weighted raster centroid remained within about 1.3 pixels
of the submitted-geometry centroid in the sampled images. The table gives
submitted anchor displacement and measured raster-centroid displacement for
each adjacent boundary. `Width` is the submitted `hostGeometryBounds` width
at the center boundary; `Norm.` is submitted displacement divided by that
width, separately for the two adjacent steps.

### Tanker 15 — Atari OFF

| Center boundary | Height | Width (px) | Submitted motion (px/boundary) | Raster centroid motion (px) | Norm. |
|---:|---:|---:|---:|---:|---:|
| 100 | 60.75 | 14.15 | 0.13 / 0.13 | 0.38 / 0.28 | 0.009 |
| 150 | 148.25 | 17.47 | 0.22 / 0.22 | 0.44 / 0.24 | 0.013 |
| 200 | 235.75 | 23.44 | 0.43 / 0.44 | 0.31 / 0.72 | 0.019 |
| 250 | 323.25 | 37.33 | 1.18 / 1.21 | 1.14 / 1.52 | 0.032 |

### Tanker 15 — Atari ON

| Center boundary | Height | Normalized depth | Width (px) | Submitted motion (px/boundary) | Raster centroid motion (px) | Norm. |
|---:|---:|---:|---:|---:|---:|---:|
| 100 | 60.75 | 0.8481 | 20.90 | 1.17 / 1.17 | 1.12 / 1.29 | 0.056 |
| 150 | 148.25 | 0.6294 | 33.06 | 1.17 / 1.17 | 1.43 / 1.21 | 0.035 |
| 200 | 235.75 | 0.4106 | 45.22 | 1.17 / 1.17 | 1.18 / 1.09 | 0.026 |
| 250 | 323.25 | 0.1919 | 57.38 | 1.17 / 1.17 | 1.42 / 1.54 | 0.020 |

The scale-normalized Atari-ON result decreases monotonically in submitted
geometry and by about 55% from first to last in raster-centroid motion. The
raw raster step lengths fluctuate because a one-pixel raster grid quantizes
small shifts; they do not show a monotonic decrease in absolute pixels.
Atari OFF has the opposite broad within-mode trend in submitted motion.
Player-shot 8 also tracks its submitted motion in the first window (OFF:
0.84/0.81 px submitted versus 0.65/0.80 px raster; ON: 3.18/3.18 versus
3.46/2.46 px), but it does not establish enemy behavior.

## Interpretation and limits

- **ESTABLISHED MANUAL OBSERVATION:** OFF normal; ON slowdown/presentation
  problem clearly present; no obvious temporary capture stalls; no noticed
  completion/error message.
- **LOG/METADATA FACT:** same recorded playback path and synchronized
  boundaries; all requested captures and entity correlations are present;
  simulation identity through complete playback is established by the
  separate deterministic-state report.
- **FRAMEBUFFER MEASUREMENT:** the representative submitted paths are
  rasterized at their expected locations. ON Tanker 15's movement relative
  to its expanding submitted/rasterized glyph decreases across the four
  observed depth windows.
- **INTERPRETATION:** the measured scale-relative slowdown is present before
  rasterization, in Atari-ON mapped-anchor progression relative to local
  glyph scale. This supports classification **A**. It is not a raw
  pixel-per-boundary slowdown, nor proof that glyph width is the arcade's
  correct rail-distance denominator.
- **UNRESOLVED:** the trace does not provide same-lane remaining rail-to-rim
  distance. The four Tanker 15 windows sample a broad height span but are
  separated by 50 playback boundaries, so intermediate curvature is not
  measured. No claim is made about an unobserved continuous speed curve.

The narrowest next investigation is to compare Atari-ON `mappedAnchor`
progression against same-lane presented rail distance and `localPresentationScale`
for a stable entity. No additional capture or correction is justified by this
analysis, and no fix was implemented.

## Preservation and validation

- **PASS** — both traces and all 30 authorized PNGs were SHA-256 hashed before
  and after analysis/report creation; hashes matched.
- **PASS** — exact requested boundaries, 15 files per mode, trace/file
  correlations, PNG dimensions, format, readability, and segment coverage
  were verified.
- **PASS** — no game launch, new capture, production/test/configuration edit,
  diagnostic-output edit, durable-doc edit, or Git operation that changes
  state was performed.
- **PASS** — only the required ignored prompt and report were created under
  `AI-Logs/`; no files were staged or committed. Pre-existing Git changes were
  preserved.
