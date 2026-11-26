# Level 19 Final Closeout

Date: 2026-11-26

## 1. Executive summary

**PASS — accepted Level 19 production closeout.** Neal has manually accepted
Level 19 gameplay and corrected presentation and separately accepted the
Level 3 post-correction regression. The final implementation preserves the
canonical Shape 3 owner, cycle-1 palette, medium/default Level 19 wave,
shared actor architecture, terminal boundary, and existing Levels 1-18 behavior.
Fresh focused tests, full suite, recordings, and Release build pass.

## 2. Starting repository state

- Branch: `master`
- HEAD before closeout: `9a8f788a41e42ea578fb1dcad90dd74a9a3cc60b`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: no staged closeout content at starting audit
- Tracked modifications: the bounded Level 19 implementation, generic Shape 3
  correction, tests, and documentation work
- Untracked: required Level 19 AI-Logs, Level19 source/test files, and protected
  `Reference/arcade/MBUCOD.V05.txt`
- Protected TEMP/UserData/Videos/ROM assets were excluded and not modified

## 3. Level 19 manual acceptance

**MANUAL ACCEPTANCE — LEVEL 19:** Neal reports that the final candidate passes
medium/default manual testing, including canonical Shape 3 presentation,
vertical placement, size/perspective, cycle-1 palette, HUD, Flippers, Tankers,
cargo, Pulsars, firing pressure, Spike/Spiker/Fuseball absence, re-entry,
natural 18→19 progression, direct startup, and terminal behavior. No further
Level 19 testing is required. Easy/hard remain unaccepted.

## 4. Level 3 regression acceptance

**MANUAL REGRESSION ACCEPTANCE — LEVEL 3:** Neal reports that Level 3 remains
correct after the shared projection dispatch changed from displayed-level
identity to canonical shape-row identity.

## 5. Final Level 19 mapping

- Displayed level: 19
- Progression index: 18
- ROM `curlevel`: `$12`
- Effective one-based `lev`: `$13`
- Cycle: 2
- Cycle-local position: 3
- Remap index: 2
- ROM shape row: 2
- Board index: 2
- Palette index: 1
- Medium/default policy
- Terminal supported level

## 6. Shape 3 reuse

Level 19 reuses the canonical Level 3 Shape 3 definition and closed 16-lane
topology. It has no copied coordinates or Level 19 geometry.

## 7. Cycle-1 palette

Palette index 1 uses `00 07 0B 08 44 03 0C 0C` and the established semantic
roles. Tube, HUD, player, enemy, Egg, and Pulsar pulse roles remain independent
of tube shape.

## 8. Wave supply and ranges

- Original supply: 22
- Physical envelope: 7
- Initial active Eggs: 4
- Maximum active Eggs: 4
- Maximum approaching Eggs: 2
- Hatch interval: 50 ticks
- Flippers: 0-3
- Tanker: exactly 1
- Pulsars: 2-3
- Spikers: 0
- Fuseballs: 0

## 9. Spike/Spiker absence

No starting or dynamic Spikes are eligible. Spiker and Fuseball ranges are
zero. Dormant raw parameters do not activate actors or effects.

## 10. Tanker cargo

One Tanker carries exactly two distinct Flipper cargo identities outside
original supply. Natural release, shot release, Super Zapper, re-entry,
reconstruction, and completion retain the shared generic architecture.

## 11. EightAdvanceTurn

Ordinary and cargo Flippers use shared `EightAdvanceTurn`: eight depth advances,
one held-depth lane turn, repeat, then shared rim pursuit. Levels 6 and 18
retain their existing behavior.

## 12. Rim lifecycle

The below-rim program terminates at rim arrival and the actor enters the
existing shared rim lifecycle. No target-level rim engine was added.

## 13. Speed

Raw `$A9` remains a host 2.71875 Flipper/Tanker climb translation. This is
PROJECT TRANSLATION, not literal unit equivalence.

## 14. Firing

Medium holdoff is 29 ticks, final slot is 2, maximum active enemy shots is 3,
and shared host shot movement remains 3. Flippers and Tankers are eligible;
Pulsars are not medium-firing.

## 15. Level 19 Pulsars

Threshold `$14` maps to approximately `392.92035f`; fixed host speed is 1;
deep-side toward-rim host speed is `87/55`; pulse beat is `$04`. Shared
pause, reversal, lane, rim, pulse, collision, reconstruction, and completion
behavior is retained.

## 16. Level 17/18 Pulsar preservation

Level 17 no-threshold compatibility and Level 18 `$14` threshold policy remain
unchanged. Natural progression now continues to Level 19.

## 17. Natural progression

Level 18 ExitZoom → Level 19 Approach → Level 19 Playing remains the production
progression.

## 18. Direct startup

`--start-level 19` and `--level 19` resolve to the same terminal Level 19
production configuration.

## 19. Terminal boundary

Level 19 is terminal. Level 20 remains unsupported, unimplemented, unreachable,
and rejected through progression/direct startup.

## 20. Original Shape 3 defect

Displayed Level 19 resolved canonical Shape 3 metadata/calibration but the
adapter projection composition was keyed by `displayedLevel == 3`.

## 21. Exact root cause

Level 19 satisfied shape/metadata/calibration resolution but missed the
displayed-level 3 projection branch and fell into generic reciprocal projection.

## 22. Generic shape-row correction

The projection branch now uses canonical
`RomTubeMappingCatalog.ResolveShapeRow(displayedLevel) == 2`, preserving
Level 3 and applying to every supported repeated owner of Shape 3.

## 23. Level 3 numerical preservation

The corrected branch preserves the existing Level 3 composition and constants.

## 24. Level 19 output equivalence

Level 19 effective tube and representative entity/lane coordinates match
canonical Level 3 output within the permanent test tolerances.

## 25. Repeated-shape equivalence

Permanent numerical theory coverage protects Level 1/17, Level 2/18, and
Level 3/19.

## 26. Durable effective-output invariant

Repeated shapes reuse effective production presentation, not only geometry,
metadata, model frame, or calibration objects. Shape-specific dispatch is
keyed by canonical ROM shape identity.

## 27. Future repeated-shape requirement

The next implementation milestone must add newly supported 20/4 through 32/16
pairs to effective-output equivalence coverage. No unsupported Level 20+
configuration was created.

## 28. Files changed

Production: Level 19 configuration/wave/progression/presentation integration
and generic shape-row dispatch. Tests: Level 19 permanent coverage and startup,
recording, metadata, and presentation regressions. Documentation: behavior spec,
development notes, and repository instructions.

## 29. Tests added/updated

Level 19 identity/wave/eligibility/cargo/routes/speed/Pulsar/firing/progression
coverage plus stronger repeated-shape effective-output theory were added or
updated alongside existing startup and presentation coverage.

## 30. Durable docs

Updated:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`

## 31. AI-Logs inventory and safety

The three required Level 19 prompt/report pairs are preserved. Bounded
credential-category scan found only normal prompt/report text for the
security policy; no credible credential value was identified or reproduced.
The closeout prompt/report pair was added.

## 32. Focused Level 19 validation

**PASS:** 49 Level 17-19 tests passed, 0 failed.

## 33. Focused presentation validation

**PASS:** 157 presentation/metadata/repeated-shape/renderer tests passed,
0 failed.

## 34. Shared regressions

**PASS:** 176 Level 15, route, cargo, Pulsar, firing, and palette-filtered tests
passed, 0 failed.

## 35. Level 15 seed-1982

Included by the complete suite and shared Level 15 coverage.

## 36. Levels 1-16 recording

**PASS:** unmodified `TEMP/GameplayRecordings/lvl1-16.json`

- achieved boundary: 35,350
- remaining frames: 0
- completion: `RecordingCompleted`
- final level: 17 Playing
- score: 134,039
- lives: 9

## 37. Level 17-origin recording

**PASS:** unmodified `TEMP/GameplayRecordings/lvl-17-test.json`

- achieved boundary: 2,609
- remaining frames: 0
- completion: `RecordingCompleted`
- final level: 18 `PlayerDying`
- score: 8,175
- lives: 8

## 38. Full suite

**PASS:** 2,113 tests passed, 0 failed, 0 skipped.

## 39. Release build

**PASS:** fresh non-incremental Release solution build succeeded with
0 warnings and 0 errors.

## 40. Whitespace

**PASS:** `git diff --check` reported no whitespace errors before staging.
`git diff --cached --check` will be run after staging.

## 41. Staging audit

Selective staging will include production, tests, durable docs, and required
AI-Logs only.

## 42. Excluded assets

Excluded: TEMP recordings and working assets, UserData, Videos, screenshots,
raw ROM/reference material, `Reference/arcade/MBUCOD.V05.txt`, editor state,
generated output, and unrelated untracked paths.

## 43. Primary commit

Pending commit/push in this closeout.

## 44. Push result

Pending.

## 45. Independent GitHub verification

Pending GitHub connector/branch verification after push.

## 46. Representative AI-Logs verification

The closeout prompt and paired final report are expected in the staged set
alongside ROM, implementation, and Shape 3 prompt/report pairs.

## 47. Report-finalization commit

At most one report-only finalization commit may follow primary commit if
commit/push/GitHub facts must be recorded.

## 48. Final repository state

Pending staged commit/push audit.

## 49. Remaining unrelated material

`Reference/arcade/MBUCOD.V05.txt` remains protected unrelated untracked
material and is excluded.

## 50. Level 20+ untouched

No Level 20 or later code, presentation, wave configuration, or progression
was added or investigated beyond the documented unsupported boundary.

## 51. Recommended next milestone

No gameplay or presentation changes are recommended. A separate future
milestone may begin only if explicitly authorized for easy/hard variants,
Level 20, or new arcade evidence.
