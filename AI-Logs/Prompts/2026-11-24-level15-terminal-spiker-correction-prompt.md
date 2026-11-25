# Bounded Level 15 terminal-Spiker lifecycle correction

Use `AI-Logs/Reports/2026-11-24-level15-terminal-spiker-readonly-report.md` as
the factual handoff and primary investigation authority. Its disposition is
READY FOR BOUNDED CORRECTION; do not repeat the investigation unless repository
evidence contradicts it.

The direct-start Level 15 candidate uses
`UsesLevel11ParameterDevelopmentBaseline = true` and
`UsesLevel11RomWavePolicy = false`. Preserve that classification and keep
Level 15 direct-start/development-only. The accepted `$A0` starting-Spike
correction and presentation defaults (Y gain 1.15, scale 1, vertical offset 75,
depth-Y gain 1) are frozen. Do not change movement, selection policy,
campaign progression, tube presentation, or unrelated behavior.

The defect is that `TempestGame.UpdateSpikers` omits the development-baseline
flag from three existing shared Level-11-equivalent terminal-Spiker decisions:
reading generic pending originals, restarting the same Spiker when pending
supply is positive, and converting that same identity to a Tanker when supply
is zero. Reuse this lifecycle; do not implement a Level-15-specific converter
or change the candidate to ROM-policy mode. Preserve the existing return
boundary approximation.

Add deterministic direct Level 15 tests that track the exact converting
identity. Cover: pending-positive same-Spiker restart without reserve return or
pending inflation; pending-zero same-ID in-place Tanker conversion with exactly
two distinct latent cargo identities, ownership and ordinals; conversion at
ordinary Tanker capacity; conversion despite unrelated active enemies and an
unrelated Spiker; and completion/all-rim remaining blocked while Tanker/cargo
obligations are unresolved. Keep the existing Level 11 terminal tests and all
Level 4-14 lifecycle, ledger, cargo, reconstruction, completion, Level 15
Spike-height, and presentation regressions intact.

Before editing, record branch, HEAD, upstream/ahead-behind, index, tracked
changes, and relevant Level 15 records. Preserve all pre-existing work and
protected user/reference material. Do not stage, commit, push, clean, reset,
stash, switch branches, inspect unrelated visual/user data, or launch the
interactive game. Do not begin Level 16.

Run bounded validation: the new tests; existing Level 15 candidate and direct
start/Spike-height tests; Level 11 terminal tests; relevant Spiker/Tanker,
cargo/ledger, Level 4-9 and Level 12-14 lifecycle, completion/all-rim and
reconstruction tests; the existing seeded Level 15 headless simulation;
complete Release solution tests; Release solution build; and `git diff
--check`. Record every exact command and outcome. Treat blocked, skipped,
timed-out, failed, or incomplete validation as not passing.

After automated validation, stop for Neal's manual acceptance using:

`dotnet run --project src\Tempest.Game -- --atari-presentation --level 15 --tube-y-gain 1.15 --tube-scale 1 --tube-vertical-offset 75 --tube-depth-y-gain 1`

Ask Neal to verify the corrected starting Spikes, normal Spiker behavior while
generic supply remains, terminal Spiker conversion near wave end, normal
Tanker cargo release and wave completion, unchanged gameplay and presentation,
and absence of new regressions. Manual terminal-Spiker acceptance remains
pending until Neal reports.

TEMPEST_LEVEL15_TERMINAL_SPIKER_CORRECTION
