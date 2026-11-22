# Level 11 initial Spike length and terminal Spiker conversion correction

Date: 2026-11-21

## Scope

Continue from the uncommitted genuine Level 11 production candidate. Use the read-only
implementation handoff as the factual basis:
`AI-Logs/Reports/2026-11-21-level11-spike-terminal-spiker-readonly-report.md`

Implement only:

1. Derive the genuine Level 11 initial Spike height from the preserved ROM
   `StartingSpikeHeightRaw = 0xB0` tip coordinate using
   `(0xF0 - rawAlong) * Level.Depth / (0xF0 - 0x10)`. With `Level.Depth = 400`,
   preserve and test approximately `114.285714`.
2. At the existing terminal/return boundary, repeat genuine Level 11 Spikers while
   `PendingGenericOriginalCount > 0`. When it reaches zero, convert the same Spiker
   original in place to a Tanker through the existing ledger conversion helper,
   preserving original identity and adding exactly two latent cargo identities.

Do not broaden scope, change presentation defaults or current Level 11 calibration,
alter presentation metadata or geometry, change Level 8/9 behavior, normalize Levels
4-7, or change direct Level 15. Preserve the current Level 11 candidate and all-rim
ledger/completion invariants.

## Validation

- Add focused Level 11 Spike-height and terminal lifecycle regression tests.
- Verify repeated Spiker behavior with pending generic originals.
- Verify in-place Tanker conversion, two latent cargo identities, and unchanged pending
  generic supply.
- Run the relevant existing Level 11, Spiker/Tanker/cargo, Ledger, replay, Level 4-9,
  presentation/HUD, complete solution test, Release build, and `git diff --check`
  validations.
- Do not launch the interactive game.
- Do not stage, commit, push, reset, restore, stash, clean, or rebase.

## Manual handoff

After automated validation, provide:

```text
dotnet run --project src\Tempest.Game -- --level 11
dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --tube-y-gain 1.5 --tube-scale 1.1
```

Completion token: `TEMPEST_LEVEL11_SPIKE_LENGTH_AND_TERMINAL_CONVERSION_FIX`
