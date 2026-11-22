# Level 11 Spinner and Atari Mapping Fix Prompt

Date: 2026-11-21

Scope:
- Continue the uncommitted genuine Level 11 production candidate.
- Correct legacy/non-Atari Level 11 physical-spinner reversal without globally
  changing accepted Levels 1-10 input behavior.
- Add the missing genuine Level 11 Atari model-frame lookup and preserve
  fail-closed behavior for unsupported levels.
- Preserve row-8 geometry and neutral Level 11 calibration (1 / 1 / 0).
- Do not add gameplay features or visually calibrate Level 11.

Required behavior:
- Legacy genuine Level 11 clockwise physical spinner movement moves the
  Blaster left.
- Legacy genuine Level 11 counterclockwise physical spinner movement moves the
  Blaster right.
- Add focused Level 11 lane-direction and open-endpoint tests.
- Add Level 11 to the established Atari presentation model-frame architecture
  using recovered genuine row-8 geometry and existing open-tube normalization.
- Exercise world-point projection and keep Levels 1-10 and direct Level 15
  mappings unchanged.
- Preserve deterministic simulation, recording/playback, HUD, and existing
  Level 11 gameplay corrections.

Validation:
- Run focused spinner/controller, Level 11, Atari projection/presentation,
  cargo/Fuseball, complete solution tests, Release build, and `git diff --check`.
- Do not launch the interactive game.
- Do not stage, commit, push, reset, restore, stash, clean, or rebase.

Completion token: `TEMPEST_LEVEL11_LEGACY_SPINNER_AND_ATARI_MAPPING_FIX`
