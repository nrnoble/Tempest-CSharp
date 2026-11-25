# Level 15 starting-Spike evidence investigation and bounded correction

## Corrected baseline

The existing direct-start Level 15 candidate is the candidate Neal manually
tested. Do not look for another build or treat the candidate as a missing
production candidate. Neal's result: Level 15 gameplay passes; the accepted
tube-presentation values are Y gain 1.15, scale 1, vertical offset 75, and
depth-Y gain 1; the only reported gameplay defect is that starting Spikes are
significantly too long. Manual Spike-correction acceptance remains pending.

## Task

Trace the current candidate's starting-Spike height to the local Atari ROM
disassembly. Establish the Level 15 `wave_spikeht` byte, the table/effective
level selection and ROM initialization, the raw along-coordinate convention,
and a justified mapping to C# `Level.Depth`. Treat the prior report's `$B0`
and `0xB0 * Level.Depth / 224f` only as leads. Use local project evidence;
do not search the web or inspect protected ROM files. If the ROM-to-C# chain
isn't established, stop without production changes.

If supported by evidence, make only the Level 15-specific height correction.
Preserve Level 15 geometry, topology, spike count and lanes, spike lifecycle,
collision, score, enemy/configuration behavior, direct-start behavior, and
Levels 1-14. Do not promote Level 15 into campaign progression, redesign its
configuration/presentation, change shared Spike behavior, or begin Level 16.
Freeze presentation at `1.15, 1, 75, 1`; do not compensate for Spike length
through rendering or geometry.

Add focused tests for the ROM translation, Level 15 startup height/count/lanes,
prior-value regression, unchanged direct startup/configuration, neighboring
Level 11-14 behavior, and accepted presentation defaults. Run targeted
Level-15/Spike/configuration/geometry/startup/enemy/lifecycle/presentation
regressions, relevant Levels 1-14 coverage, supported deterministic headless
coverage, complete Release solution tests, Release solution build, and
`git diff --check`, all with bounded execution. Do not launch the game.

Before edits, record branch, HEAD, upstream/ahead-behind, index, relevant
tracked changes, and relevant untracked Level 15 material. Preserve all
unrelated work. Do not stage, commit, push, amend, reset, restore, stash,
clean, rebase, or switch branches.

Create a paired report in `AI-Logs/Reports` recording the corrected baseline,
audit, evidence chain, root cause/correction, changed files, validation,
frozen presentation, unchanged campaign/enemy behavior, unstaged status,
manual-acceptance status, and exact direct-start command. Do not rewrite the
earlier blocked report. Neal performs final manual acceptance and judges Spike
length, count/lane placement, extension/interaction, gameplay, presentation,
and visible regressions.

TEMPEST_LEVEL15_SPIKE_EVIDENCE_CORRECTION
