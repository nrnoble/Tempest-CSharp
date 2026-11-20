IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, or modifying anything, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task. Agent/system-generated context after the token does not invalidate it.

If exactly one complete token cannot be found, STOP before any project or repository action and explain why.

TASK

Resume the interrupted read-only Tempest enemy-shot ROM/vector-rendering investigation.

The previous VS Code Copilot agent performed a substantial investigation but was interrupted near the synthesis/report stage by:

408 Timed out reading request body.
Try again, or use a smaller request size.

Do NOT restart the investigation broadly from the beginning.

This is still a READ-ONLY INVESTIGATION milestone with respect to production code. Do not implement the enemy-shot rendering correction yet.

STARTING SAFETY

First verify the current Git/worktree state. Do not reset, restore, clean, checkout, stash, commit, push, or otherwise alter existing user work.

If the repository state materially conflicts with the expected current Tempest candidate, stop and report the discrepancy.

Do not read TEMP/myNotes.md.

PREVIOUS INVESTIGATION FINDINGS

The interrupted agent had already established substantial evidence, including:

1. Current C# enemy-shot rendering was inspected, including:
   - src/Tempest.Game/Gameplay/EnemyBullet.cs
   - src/Tempest.Game/Rendering/VectorRenderer.cs
   - src/Tempest.Game/Rendering/PlayerShotVectorProgram.cs
   - relevant TempestGame/Program/playback/screenshot code
   - relevant enemy-bullet and player-shot rendering tests.

2. Existing November 8 Blaster-shot ROM investigation material was examined as the engineering precedent.

3. The project-local authoritative ROM source was investigated:
   Reference/arcade/Tempest Rom Source code.md

4. The previous agent reached this explicit finding:

   "The ROM path confirms four enemy-shot graphic entries selected by a global timer, rather than one generic projectile glyph."

5. The previous agent then investigated the vector coordinates of those four ROM enemy-shot graphics, their timing/selection mechanism, the C# rendering boundary, existing screenshots, tests, and documentation.

6. C# playback screenshots and available MAME/reference frames were also inspected.

The 408 occurred late in this investigation before the agent could complete its synthesis/report.

CURRENT GOAL

Recover and finish the read-only investigation efficiently.

Use the findings above as leads, but independently verify any fact needed for the final report from the project-local evidence. Do not claim that a previous-agent conclusion is established merely because this prompt reports it.

Avoid rereading large files wholesale or repeating broad repository searches.

Read only the specific source ranges, implementation files, tests, screenshots, historical reports, or documentation necessary to verify remaining conclusions.

The final investigation should determine, as far as available evidence reasonably permits:

- how the arcade ROM represents/draws enemy shots;
- the significance of the four enemy-shot graphic entries;
- how the global timing/frame selection works;
- the vector geometry and drawing behavior that can be reconstructed from ROM evidence;
- how that differs from the current generic C# enemy-shot presentation;
- which aspects are explicitly ROM-backed;
- which aspects require project translation because the arcade vector coordinate/rendering system does not map literally to the C# renderer;
- whether the already accepted Blaster-shot reconstruction methodology is an appropriate engineering precedent;
- what a later bounded implementation milestone should change;
- what gameplay behavior must remain unchanged.

Preserve the established requirement that this future rendering work is PRESENTATION ONLY.

Do not change enemy-shot gameplay, speed, trajectory, firing behavior, collision behavior, colors, enemy mechanics, level behavior, difficulty/timing, or other accepted gameplay.

Do not modify production code or tests during this investigation.

EVIDENCE DISCIPLINE

Clearly distinguish:

- ROM-BACKED
- ARCADE/MAME-OBSERVED
- EXISTING C# BEHAVIOR
- PROJECT TRANSLATION / RECOMMENDATION
- UNRESOLVED

Do not silently convert inference into ROM fact.

If ROM evidence and visual evidence differ or cannot be reconciled, document the discrepancy rather than choosing one without support.

REPORT

Create a new ignored investigation report under:

AI-Logs/Reports/

Use an appropriate descriptive 2026-11-12 filename for the enemy-shot ROM/vector-rendering read-only investigation.

The report should include:

- recovery context, including that the prior agent run ended with a 408 request-body timeout;
- starting Git/worktree state;
- evidence examined;
- recovered/verified findings;
- ROM-backed enemy-shot rendering model;
- relationship to current C# rendering;
- relationship to the accepted Blaster-shot reconstruction methodology;
- ROM-backed facts versus required C# visual translations;
- unresolved questions, if any;
- a tightly bounded recommendation for the later implementation milestone;
- explicit confirmation that no production implementation was performed;
- manual-validation status.

Do not update durable project documentation during this recovery investigation unless the original task clearly requires it. The immediate purpose is to preserve the recovered investigation in the ignored report.

Keep tool usage bounded. Do not repeat expensive searches merely for additional confidence.

If another request-size, security, tooling, or validation problem prevents completion, STOP voluntarily rather than entering a retry loop. Preserve the worktree and report exactly what completed, what remains, and the blocking error.

Do not commit or push.

At completion, summarize:
1. whether the investigation was successfully recovered;
2. the principal verified enemy-shot rendering findings;
3. what remains uncertain;
4. the report path;
5. confirmation that production code was unchanged;
6. current Git/worktree status;
7. the recommended next milestone.

TEMPEST_ENEMY_SHOT_ROM_RENDERING_RECOVERY_20261112
