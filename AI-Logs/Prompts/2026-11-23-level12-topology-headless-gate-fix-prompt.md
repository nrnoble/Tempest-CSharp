# Level 12 Topology and Headless Gate Fix

Correct the specific Level 12 production topology mismatch reported by Neal's
manual Atari-presentation launch. Do not redesign or recover the geometry;
preserve row 12, every ordered X/Y/angle value, presentation metadata, row
ordering, and the recovered gameplay policy. Correct the source of the C#
continuity mismatch, retain fail-closed Atari topology validation, preserve the
established closed-tube wraparound representation, and update the startup
message to include production Level 12 without claiming support beyond it.

Add a bounded deterministic Level 12 production-simulation soak that exercises
normal admission and enemy lifecycle, plus a focused regression using the
actual production definition through the Atari topology/model-frame projection
path. Cover calibration guides, closed lane connectivity and spinner seam
movement, supported/unsupported presentation levels, and preserve relevant
existing gameplay, progression, HUD, diagnostics, and other-level regressions.
Do not add an interactive Raylib dependency to headless validation.

Preserve unrelated worktree changes and local/protected material. Do not
launch the interactive game; do not stage, commit, push, reset, restore, stash,
clean, or rebase. Run bounded focused tests, the complete solution test suite,
a Release solution build, and `git diff --check`, with explicit command
timeouts. Do not weaken Windows security or retry a blocked artifact. Record
the results and unresolved/manual-acceptance status in the paired
`AI-Logs/Reports/` report.

After automated validation, hand off these commands for Neal's manual testing:

```powershell
dotnet run --project src\Tempest.Game -- --level 12
dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation
dotnet run --project src\Tempest.Game -- --level 12 --atari-presentation --calibration-guides
```

Stop for Neal after the automated gate. The Atari manual gate is no topology
exception, visible tube, and continuing gameplay; detailed visual calibration
and gameplay acceptance remain manual.

TEMPEST_LEVEL12_TOPOLOGY_AND_HEADLESS_GATE_FIX
