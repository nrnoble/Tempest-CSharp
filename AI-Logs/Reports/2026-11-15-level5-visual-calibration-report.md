# Level 5 Visual Calibration

Date: 2026-11-16
Scope: READ-ONLY visual calibration
Status: Analysis complete; manual acceptance not performed

## Executive conclusion

**Disposition: READY FOR ONE AXIS-SPECIFIC CANDIDATE.**

The one authorized MAME frame has a measurable outer Level 5 tube envelope
that is substantially narrower and modestly shorter, in normalized viewport
dimensions, than the current full-extent C# Level 5 envelope. Its X and Y
extent ratios differ materially, so a uniform scale alone does not reproduce
the reference proportions. The apparent tube center is nearly horizontally
centered and about 3.2% of viewport height below the accepted common center.
The frame shows no tube/HUD intersection.

The minimum first candidate is render-only: apply a uniform extent scale of
approximately `0.880`, an additional X-only aspect factor of approximately
`0.762` (net X scale approximately `0.670`), and a bounded downward center
exception of approximately `3.2%` of viewport height. These are normalized
MAME-calibrated candidate values, not exact arcade behavior, ROM-backed
values, or accepted project behavior. The frame supports testing this isolated
candidate; Neal's manual acceptance remains required.

## Authorized visual evidence

Inspected exactly:

`C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-5\Tempest-MAME-frame_5880.png`

No other screenshot, image, video, recording, extracted frame, contact sheet,
or visual asset was inspected or compared. The single frame is treated as a
strong practical MAME reference, not a pixel-perfect specification.

## Image measurements

Image dimensions are **480 x 640 pixels** (RGB). The aspect ratio is 3:4,
matching the controlling report's 720 x 960 portrait viewport.

| Measurement | Approximate value |
|---|---:|
| Outer/near-rim tube bounds | x=89..389, y=143..537 pixels |
| Outer tube width x height | 301 x 395 pixels |
| Tube width / height in pixels | 0.762 |
| Normalized tube bounds (left, right, top, bottom) | 0.185, 0.813, 0.223, 0.841 |
| Normalized width x height | 0.627 x 0.617 of viewport |
| Left / right margins | 18.5% / 18.8% |
| Top / bottom margins | 22.3% / 15.9% |
| Outer-bounds center | x=49.9%, y=53.2% of viewport |

Bounds are from the connected blue tube-line component; edge location is
uncertain by about 1 pixel due to rasterization and antialiasing. Normalized
bounds use pixel edges (right/bottom exclusive). The inner/far-end contour is
visually around x=216..263, y=380..444 pixels (roughly 48 x 65 pixels), but
its lines are crowded with temporary gameplay objects. Allow at least 5-8
pixels of uncertainty there; it is not used to set candidate scale.

Persistent HUD content is approximately x=42..281, y=89..141: upper green
text/score-like elements around x=91..281, y=89..121; a small blue element
around x=252..260, y=109..123; and yellow icons around x=42..87, y=129..141.
The blue tube envelope begins at y=143. The nearest visually aligned HUD
element is the blue HUD element above the tube's top cap, with about 19 empty
raster rows between their bounds (approximately 20 pixels edge-to-edge).
The yellow icons are horizontally separated from the upper tube cap.

The yellow player/ship-like outline and the small colored shapes near the
center/bottom are temporary gameplay objects, not tube bounds or persistent
HUD. They were excluded from the tube measurements. No tube-line pixel occurs
within the persistent HUD's observed Y range (through y=141), and no tube line
extends beneath HUD elements while remaining in that HUD Y band.

## Presentation-center analysis

The outer tube's bounds center is approximately `(49.9%, 53.2%)`; within
measurement uncertainty, it is centered horizontally and shifted down by
about 3.2% of viewport height from the viewport/common center `(50%, 50%)`.
The tube occupies the playfield below the HUD and leaves more top than bottom
margin. This is consistent with the accepted composition concept and suggests
a small downward placement exception to preserve the HUD area. It is not
evidence for separate viewports or for literal coordinate equivalence with
the C# center.

The measured extent is not simply a taller tube around the common center:
the normalized MAME width and height differ substantially from the current
C# near-rim envelope, and the MAME center is slightly below common center.
The inner/far-end outline appears coherent with the outer tube's centerline,
but is too crowded to provide an independent precise anchor.

## HUD interaction

1. **Vertical-envelope overlap:** The controlling source report finds that
   full-size common-center Level 5 C# placement overlaps the normal C# HUD
   vertical band by 22.364 host pixels. The MAME frame itself does not show
   this collision: its tube begins below the visible persistent HUD.
2. **Actual geometric intersection:** None is visible in the authorized
   frame. No tube line intersects HUD elements.
3. **Visible separation:** The tube's top cap is about 20 pixels below the
   lower blue HUD element where their X ranges overlap. The lower yellow icons
   are horizontally remote from that cap. The tube and persistent HUD are
   visibly distinct.
4. **Center versus HUD:** The slight downward offset and larger top margin
   are consistent with a bounded placement exception for HUD clearance; they
   do not establish that the exact offset is an accepted C# value.

The mathematical full-envelope overlap is a conservative C# baseline result,
not proof that tube lines collide with HUD text. The MAME visual resolves the
actual-intersection question for this frame as **no visible collision**.

## Baseline comparison

The controlling report gives the current C# near-rim envelope as
`673.527 x 673.527` host pixels. In the 720 x 960 portrait viewport, its
normalized extent is approximately `0.935 x 0.702`. Common-center translation
changes position, not extent; its normalized near bounds are approximately
`x=0.032..0.968`, `y=0.149..0.851`. The un-translated legacy center is
vertically above the accepted common center.

| Near-rim measure | MAME frame | C# full extent, normalized |
|---|---:|---:|
| Width / viewport width | 0.627 | 0.935 |
| Height / viewport height | 0.617 | 0.702 |
| X center | 0.499 | 0.500 |
| Y center | 0.532 | 0.500 after common-center translation |

The MAME-to-C# extent ratios are approximately `0.670` on X and `0.880` on Y.
Thus full current extent is inconsistent with the observed bounds, and one
uniform reduction cannot match both axes. The image supports a smaller,
axis-specific render-only candidate. These ratios compare normalized viewport
dimensions, not raw pixels across differently sized displays.

For HUD context, the source-derived common-center full-size C# tube has a
normalized top of about 14.9%, above the C# HUD boundary at about 17.3%.
The MAME tube's measured top is about 22.3%; its own HUD extends to about
22.0%. These two HUD layouts are not treated as identical, but both the
baseline calculation and frame support testing reduced extent/placement rather
than inferring collision from the envelope alone.

## Aspect analysis

The MAME tube's raw pixel width/height ratio is approximately `0.762`.
After viewport normalization its width-to-height proportion is approximately
`1.016`, compared with approximately `1.333` for the C# normalized near-rim
envelope. Their ratio is approximately `0.762`. The frame therefore clearly
supports testing an X-specific presentation difference; no Level 2 factor is
borrowed or generalized.

MAME cannot establish an exact physical-arcade aspect specification. The
aspect factor is a visual-calibration candidate only. No model-coordinate,
projection-source, or gameplay geometry change is supported.

## First-candidate specification

One isolated Level 5 render-only candidate:

1. Scale the Level 5 gameplay presentation (tube and associated world
   presentation, not HUD or simulation state) about the projected near-rim
   origin.
2. Apply uniform extent scale `0.880` and an additional X-only aspect factor
   `0.762`, for net X scale `0.670` and Y scale `0.880`.
3. Place the near-rim anchor at approximately `(50.0%, 53.2%)` of the
   portrait viewport: retain the common horizontal center and add a downward
   exception of about `3.2%` of viewport height. Relative to the baseline's
   common-center translation, that extra shift is approximately 30.7 host
   pixels (about 24.4 logical Y units using the report's host/logical
   derivative).

Value provenance:

- **Existing accepted C# presentation / source math:** common target
  `(50%, 50%)`, near-rim origin anchor, 720 x 960 portrait viewport, and the
  baseline full-size bounds/host-to-logical mapping. The source report's
  `+112.021645` logical-Y translation aligns the origin to common center.
- **Normalized MAME visual calibration:** measured bounds and center;
  `0.670` net X scale, `0.880` Y/uniform scale, `0.762` additional X factor,
  and approximately `+3.2%` viewport-height placement exception. These are
  rounded first-candidate values, with visual/raster uncertainty, not
  ROM-backed or exact.
- **Project translation:** implement only as a Level 5 presentation transform;
  do not alter model points, lanes, topology, simulation, depth progression,
  recordings/playback, HUD, or accepted Levels 1-4.

The candidate is not an accepted behavior change. It must be manually
evaluated before any acceptance or durable behavior-spec update.

## Decision-relevant unknowns

| Unknown | Could change disposition? | Candidate architecture? | Model/gameplay? | HUD/center? | First manual test? |
|---|---|---|---|---|---|
| Inner/far-end contour is partly obscured; its exact bounds are uncertain. | No; outer near bounds determine the mismatch. | No. | No. | No. | No; inspect overall tube/depth appearance as part of normal acceptance. |
| MAME's raster/display mapping is practical rather than pixel-perfect. | Not for a bounded candidate; could revise its numeric tuning. | No; remains presentation-only. | No. | Could slightly revise placement. | Yes; compare candidate proportions and center against the reference. |
| Neal's preferred center tolerance and positive HUD gap are not established. | Could affect final acceptance, not whether to test this candidate. | No. | No. | Yes; may tune the bounded offset. | Yes; explicitly inspect HUD clearance and centering. |
| A single frame cannot establish whether the measured proportions are invariant across other MAME views. | No; additional visual inspection is outside this authorization and is not needed to try one candidate. | No. | No. | No. | No change to the first test; manual acceptance remains authoritative. |

No remaining unknown warrants more source, ROM, or gameplay investigation for
this disposition. The candidate must remain bounded and easy to reject or
retune after Neal's manual comparison.

## Smallest next milestone

Exactly one next milestone: **implement one isolated Level 5 render-only
candidate using the normalized MAME extent/aspect and bounded downward
placement above, then validate presentation invariants and stop for Neal's
manual visual acceptance.** Preserve Levels 1-4, gameplay/model geometry,
depth progression, and deterministic recording/playback. Do not proceed to
other levels or unrelated defects.

## Scope and completion

- Prompt: `AI-Logs/Prompts/2026-11-15-level5-visual-calibration-prompt.md`.
- Report: `AI-Logs/Reports/2026-11-15-level5-visual-calibration-report.md`.
- Exactly the two authorized artifacts were created; neither was staged or
  committed.
- Authorized visual inspected: exactly
  `UserData/Screenshots/Level-5/Tempest-MAME-frame_5880.png`.
- Controlling handoff read completely:
  `AI-Logs/Reports/2026-11-15-level5-presentation-baseline-investigation-report.md`.
  No production source, ROM, or other reference/handoff material was opened.
- Calculations: bounded RGB PNG decoding and color-component bounds on the
  authorized image; normalized image bounds, margins, center, C# extent ratios,
  and candidate scale/translation from the controlling report. No temporary
  image or other visual file was generated. No package was installed.
- Starting Git state: branch `master`; HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`; upstream `origin/master`;
  tracking divergence `0/0`; index empty. There were 16 modified tracked
  files and 8 untracked paths before this task. This matched the prior baseline
  report's recorded worktree counts and was preserved.
- Ending state verified: branch `master`, HEAD
  `385f603a94391756b2ca6cccd524dc17b281e782`, upstream `origin/master`,
  divergence `0/0`, and empty index are unchanged. The same 16 tracked
  modifications and 8 pre-existing untracked paths remain; no tracked diff
  names changed. Only these two ignored AI-Logs artifacts were added.
- No source, tests, geometry, configuration, durable behavior documentation,
  references, or visual assets were modified. No build, test, game launch, or
  gameplay was run. Nothing was staged, committed, pushed, cleaned, reset,
  restored, or stashed.
- Automated validation: **NOT REQUIRED** for this read-only calibration.
- Neal's manual visual/gameplay acceptance: **NOT PERFORMED**.
