# Read-only Level 15 terminal-Spiker investigation

## Scope and preservation

This is a read-only investigation. Do not modify production source, tests,
configuration, durable documentation, tracked project files, or reference
material. Create only this prompt record and its paired report under
`AI-Logs/Reports`. Do not stage, commit, push, amend, reset, restore, stash,
clean, rebase, or switch branches. Do not implement a terminal-Spiker fix or
begin Level 16.

Audit branch, HEAD, upstream/ahead-behind, index, tracked changes, and relevant
untracked Level 15 prompt/report records before investigating. Preserve all
pre-existing worktree content. Do not inspect screenshots, video, MAME
captures, UserData, Temp/TEMP, recordings, private scratch files, unrelated
assets, or raw ROMs. Use only the project-local Atari disassembly and directly
relevant tracked ROM-derived text/data; do not search the web.

## Current Level 15 baseline

The existing direct-start Level 15 candidate is the one Neal manually tested;
there is no missing candidate to locate. Normal campaign progression may end
at Level 14 and remains out of scope. Neal reports that Level 15 gameplay
otherwise passes, and accepts/fixes the presentation at Y gain `1.15`, scale
`1`, vertical offset `75`, and depth-Y gain `1`.

The corrected Level 15 starting-Spike length has now passed manual testing.
Treat it as accepted and frozen. Do not alter or re-investigate that accepted
length or the presentation except for a causality check. The remaining
reproducible defect is that the last couple of Spikers repeatedly travel up
and down the tube and never convert into Tankers. This resembles the former
Level 11 terminal-Spiker defect, but similarity is not proof of cause.

## Evidence to reconcile

Read these historical reports and preserve their evidence classifications:

- `AI-Logs/Reports/2026-11-21-level11-spike-terminal-spiker-readonly-report.md`
- `AI-Logs/Reports/2026-11-21-level11-spike-length-terminal-spiker-correction-report.md`
- `AI-Logs/Reports/2026-11-23-level11-terminal-spiker-test-resolution-report.md`

Inspect the current Level 15 Spike-correction prompt/report and the latest
report documenting the ROM-backed Level 15 `$A0` starting-Spike correction.
Also inspect current source, tests, durable behavior/development documents,
and the local disassembly. Older reports may reflect earlier repository
states; do not rewrite them.

The Level 11 comparison model is that the shared ROM Spiker grows the Spike,
reverses, returns/hops, and checks generic `enemies_pending`. Nonzero pending
supply keeps the same enemy a Spiker; zero rewrites that same active slot as a
Flipper-carrying Tanker. A bounded C# Level 11 translation converts the same
original identity at its return boundary, with two latent cargo identities.
A later test correction made assertions identity-specific; unrelated active
Spikers may remain. Verify current code and local ROM evidence rather than
copying that behavior by analogy.

## Required investigation

Trace the Atari shared Spiker lifecycle, including growth, reversal, return,
the exact terminal test, pending-value meaning/lifecycle, nonzero and zero
branches, level-data versus shared-code behavior, same-slot conversion, cargo,
other active enemies, ordinary Tanker limits, and any Level 15 exception.

Trace current C# Level 15 from `Spiker.Update` through `TempestGame.UpdateSpikers`,
terminal conversion, Level 15 configuration/selection, ledger pending supply,
reserve return, Tanker creation/cargo, ordinary admission and selection,
active Spiker minimum/maximum, completion, and all-rim identity handling.
Answer explicitly whether the candidate enters conversion, what happens at
the terminal boundary, whether the same identity can be selected as a Spiker
again, whether the minimum makes that deterministic, whether selection/cargo
limits increase the chance, and whether a zero pending count is replenished by
returning the Spiker.

Keep the identity analysis specific: before/after original ID, type/state,
Tanker ID/state, both cargo IDs/owner/state, and pending count before/after.
Unrelated active Spikers remain legal. Determine whether the classification is
a genuine regression, an unwired direct Level 15 candidate, a configuration/
ledger issue, or another supported cause. Do not call it a regression without
repository-history evidence that Level 15 previously converted.

Check whether the accepted `$A0` Spike-height correction changed any terminal
state, reserve, pending, conversion, cargo, selection, or identity code. Note
any limited timing relationship through Spike growth if the source establishes
one; do not alter the accepted Spike correction.

Run the smallest useful existing headless tests first with explicit hang
bounds. Do not create tests or diagnostic infrastructure. Report whether an
existing deterministic Level 15 simulation exposes the terminal transition;
static evidence is acceptable if conclusive. Do not launch the graphical game.

## Disposition

Conclude with exactly one:

- **READY FOR BOUNDED CORRECTION** — root cause, intended Atari behavior,
  C# translation, implementation point, identity/ledger effects, and focused
  regression plan are established. Describe the smallest fix but do not make it.
- **NEEDS FURTHER EVIDENCE** — identify the missing evidence and narrowest
  next investigation.
- **NO PRODUCTION DEFECT FOUND** — show why the behavior is expected or the
  current code already converts correctly.

If ready, specify the existing source mechanism to reuse; narrow eligibility
change; Level-15-specific versus shared scope; pending-count gate; identity
preservation; two cargo identities; ordinary Tanker cap and unrelated-enemy
behavior; completion/all-rim invariants; focused deterministic tests; and
Level 11/other-level regressions that must remain unchanged.

The paired report must record the baseline, manual observation, frozen Spike
acceptance, historical and ROM evidence, current C# path, identity/ledger
analysis, deterministic-test results, regression-versus-unimplemented
classification, Spike-height causality, facts versus hypotheses, commands and
results, disposition, unresolved evidence, and confirmation that no project
files were changed or staged/committed/pushed. Preserve all unrelated Level 15
behavior and current campaign status.

TEMPEST_LEVEL15_TERMINAL_SPIKER_READONLY
