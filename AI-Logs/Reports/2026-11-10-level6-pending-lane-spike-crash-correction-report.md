# Level 6 pending-lane Spike crash correction

Date: 2026-11-10

Disposition: **AUTOMATED VALIDATION PASSED. CANDIDATE REMAINS UNCOMMITTED.
MANUAL LEVEL 6 ACCEPTANCE IS PENDING.**

## Starting state

- Branch: `master`, tracking `origin/master`.
- HEAD: `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`, the requested baseline.
- The index was clean. The existing diagnostic candidate was already present
  as six modified tracked files and two untracked code/test files; these
  matched the implementation report's file list.
- Initial `git diff --check` passed. The full candidate diff was reviewed.
- The candidate's earlier automated validation was not treated as sufficient
  for the manually reported crash.

## Failure and confirmed cause

Neal reported a repeatable startup crash within approximately ten seconds:

```text
System.InvalidOperationException:
Level 6 pending Egg lane 8 has no persistent Spike.
```

The exception originated in
`Level6AdmissionPolicy.GetPendingSegmentSpikeHeight` while hatching a
Level 6 Egg after the Tanker and Spiker minima had already been met.

Canonical Level 6 is a continuous 16-vertex tube with playable lanes
`0..15`. It creates 16 initial Spikes, one per lane. An Egg's `EdgeIndex` is
the current lane index; roaming moves it between adjacent indices, wrapping
between 15 and 0. In the ROM-derived representation, `pending_seg` contains
values `$0-$f`, and `spike_ht` is a 16-byte table indexed by tube segment.
Thus Egg lane 8 maps directly to segment 8; it is not an edge midpoint or a
one-based index, and no neighboring-lane or wrap remapping applies to lookup.

The missing object was not evidence of an omitted Level 6 lane or bad
initialization. C# removes a depleted Spike from `Level.Spikes` after a
player shot. The ROM keeps per-segment `spike_ht` state and writes zero when
a shot destroys a segment Spike (`$a1fa-$a210`). During the pending-entry
decision (`$9a4a-$9a59`), the ROM reads `spike_ht[pending_seg]`, substitutes
`$ff` when that value is zero, then compares it with `$cc`. The C# lookup
incorrectly assumed every playable lane always had a Spike object and threw
when the pending lane's Spike had been depleted.

Accordingly, any canonical Level 6 lane can encounter the same failure if
its Spike is depleted while a valid Egg is pending on that lane. No lane is
missing at startup; all 16 lanes directly correspond to the 16 initial
Spikes.

The local listing also confirmed the comparison direction. The ROM selects a
Spiker when the comparison value is `$cc` or greater, and a Tanker below
`$cc`. The candidate had converted the threshold to C# height but compared it
in the opposite direction. Correcting the segment comparison therefore also
required expressing the value in ROM-along coordinates and correcting that
coupled comparison; this was confined to Level 6's pending-segment policy.

## Smallest correction

- `GetPendingSegmentSpikeRomComparisonValue` first rejects an invalid lane,
  then looks up only the Spike on that exact segment.
- A live Spike is translated with the existing
  `Tanker.RomAlongPositionForHeight` mapping. A missing C# Spike maps to
  `$ff`, the ROM selector's established comparison sentinel for a zero
  `spike_ht`; no height is guessed and no global Spike scan is used.
- Post-minimum selection now chooses Spiker for mapped values `>= $cc`, and
  Tanker below `$cc`.
- Minima/maxima, physical headroom, cargo accounting, zero ordinary Flipper
  admission, and other lifecycle behavior were not changed. Level geometry
  and persistent-Spike lifecycle were not redesigned.
- TempestGame supplies the pending Egg's own lane to the mapping.

The ROM-to-C# height conversion remains an approximation, not a claim of
literal coordinate identity. For example, the C# initial Spike height 60
maps to ROM-along position 206.4 using the existing conversion; the ROM
threshold remains `$cc` (204).

## Tests

- Added a production-path regression: hatch the first three canonical Level
  6 Eggs to satisfy minima, move the remaining pending Egg to lane 8, remove
  that lane's depleted Spike, and hatch it. It verifies no exception, the
  `$ff` comparison value's Spiker result, and the ledger commitment.
- Added coverage that each of the 16 valid lanes maps to its directly
  corresponding Spike, including the closed-tube 15/0 traversal boundary.
- Updated the pending-lane-vs-global-scan and threshold tests to compare
  translated ROM-along values and the listing's `$cc` branch direction.
- Updated existing Level 6 production/firing expectations affected by that
  direction. Existing copied-configuration isolation coverage remains in
  place.
- Invalid lane 16 remains fail-closed with
  `ArgumentOutOfRangeException`.

The original policy tests did not include a valid lane whose Spike object
had been removed; they covered live Spike heights and an invalid out-of-range
lane instead.

## Validation

All commands used Release configuration.

- Exact lane-8 regression before the correction: **FAIL as expected**, with
  the reported `InvalidOperationException`.
- Exact production-path lane-8 regression after correction: **PASS**, 1/1.
- `Level6AdmissionPolicyTests`: **PASS**, 16 passed.
- Affected production, persistent-Spike/Spiker, Tanker/cargo,
  death/reconstruction, direct startup, campaign/recording/playback, and
  Levels 1-5/7-8 isolation batch: **PASS**, 691 passed, 0 failed, 0 skipped.
- Full requested command,
  `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min`:
  **PASS**. VSTest 17.14.1 executed; 1,184 passed, 0 failed, 0 skipped.
  The Blame collector reported that all tests finished and no sequence file
  was generated.
- `dotnet build TempestCSharp.slnx --configuration Release`: **PASS**, 0
  warnings, 0 errors.
- Final `git diff --check`: **PASS**.
- The complete candidate diff was reviewed after the changes.

During iteration, the first policy run and first affected batch exposed
existing assertions that expected the candidate's previous comparison
direction. Those assertions were updated to match the local ROM instructions;
the exact regression, policy suite, and affected batch then passed.

## Final repository state and handoff

- HEAD remains at `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933` on `master`,
  tracking `origin/master`.
- No files were staged; the candidate remains uncommitted. No commit or push
  was made.
- The source and test changes remain limited to the existing Level 6
  candidate and its focused tests. The prompt and this report are ignored
  `AI-Logs/` records.
- The interactive game was not launched. No Level 7 concurrency or `$0b`
  Flipper-movement investigation was started.

Neal's manual acceptance is still required. Use:

```powershell
dotnet run --project src\Tempest.Game -- --level 6 --lives 10
```

First acceptance gate: Level 6 runs beyond the previous repeatable
first-ten-second crash window without throwing. Do not treat automated
validation as manual acceptance; resume the broader concurrency and
death-reconstruction checklist only after that gate passes.
