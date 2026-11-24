READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK:
Before using any tools, reading files, running commands, or making repository changes, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

TASK: Correct Level 10 production-campaign integration exposed by --start-level.

CONTEXT

The new --start-level candidate is automatically validated, but manual testing found one specific failure:

dotnet run --project src\Tempest.Game -- --start-level 10

returns:

--start-level requires an implemented production campaign level (currently ending at Level 12).

Other implemented production starting levels tested by Neal work.

Level 10 is already manually accepted. Do NOT reopen or redesign its gameplay, tube geometry, Atari presentation, calibration, or other accepted Level 10 behavior merely because this progression/integration defect was discovered.

Current repository evidence shows an architectural inconsistency that predates --start-level:

- LevelProgression.GetNextProductionLevel() currently progresses Level 9 directly to Level 11.
- Level 10 is therefore absent from the production campaign chain.
- GetDirectStartLevel() separately maps displayed Level 10 to Level10Diagnostic.
- Historical comments also describe Level 10 as diagnostic/outside the campaign.
- Level 10 has subsequently been manually accepted as a real game level, so this old diagnostic distinction appears stale unless current technical evidence proves otherwise.

OBJECTIVE

Make every implemented production level directly startable through the production-level startup mechanism and ensure it participates in the same real campaign progression reached through normal play.

For the currently implemented game, the intended production progression is:

1 -> 2 -> 3 -> 4 -> 5 -> 6 -> 7 -> 8 -> 9 -> 10 -> 11 -> 12

Level 10 must no longer be omitted from that production chain.

This is a campaign/progression integration correction, NOT a Level 10 gameplay or presentation redesign.

STEP 1 - BOUNDED INVESTIGATION BEFORE EDITING

Inspect the current Level 10 configuration, LevelProgression, startup parsing/configuration, progression handling, tests, and directly relevant documentation.

Determine:

1. Why Level 10 remains represented as Level10Diagnostic/outside the production progression chain.
2. Whether the already accepted Level 10 configuration can safely be promoted/reclassified as the production Level 10 used by ordinary progression.
3. Whether any code currently depends on the diagnostic designation or on the Level 9 -> Level 11 skip.
4. Whether there is any genuine technical incompatibility that prevents Level 10 from becoming a normal production campaign member.

Do not broaden this into archaeological research if the current code gives a clear answer.

If there is a substantive technical reason why the accepted Level 10 cannot safely become the normal production Level 10, STOP before implementing a workaround. Report the exact reason and evidence so Neal can decide what to do.

Do NOT simply special-case --start-level 10 while leaving the underlying production chain broken.

STEP 2 - IMPLEMENT THE MINIMAL CORRECTION

If no blocking technical incompatibility exists, correct the production model.

Requirements:

- Level 10 must be a production campaign level.
- Normal progression must become Level 9 -> Level 10 -> Level 11 -> Level 12.
- --start-level 10 must start the accepted production Level 10.
- Completing a campaign begun at Level 10 must progress normally to Level 11.
- Existing accepted Level 10 gameplay, geometry, presentation, calibration, enemies, timing, and mechanics must be preserved unless a strictly necessary integration change is identified.
- Existing Levels 1-9 and 11-12 behavior must remain unchanged except for the required Level 9 -> Level 10 progression correction.
- Do not create another independent list of production levels if the existing authoritative progression model can provide the answer.
- Preserve the design goal that future implemented production levels become automatically eligible through the authoritative production progression structure rather than accumulating per-level startup exceptions.
- Direct-only Level 15 behavior is outside this correction and must not accidentally become production.
- Preserve diagnostic/test scenarios that are genuinely diagnostics, but remove or update stale Level 10 diagnostic assumptions as necessary.
- Update misleading comments/error messages/documentation that still describe Level 10 as diagnostic or outside the campaign.

IMPORTANT START-LEVEL RULE

The intended invariant is:

Every implemented production level is directly startable and, once started, behaves as the corresponding point in the normal continuing production campaign.

Do not implement this by maintaining two independently curated notions of which production levels exist.

TESTS

Add focused regression coverage for the defect.

At minimum establish automatically that:

- production Level 10 is discoverable through the authoritative production chain;
- Level 9's next production level is Level 10;
- Level 10's next production level is Level 11;
- --start-level 10 parses/starts successfully;
- --start-level 10 uses the production Level 10 configuration rather than an obsolete diagnostic substitute;
- a bounded production progression test demonstrates Level 10 -> Level 11;
- all currently implemented production levels 1 through 12 are eligible for --start-level;
- direct-only Level 15 remains excluded from --start-level;
- unimplemented levels remain rejected;
- existing --level behavior is not unintentionally regressed.

Also run the appropriate existing startup, progression, Level 10, Level 11, Level 12, recording/playback, Atari-presentation, and campaign regression coverage.

Then run the complete solution test suite with the project's normal bounded hang timeout, Release build, and git diff --check.

Do not claim manual gameplay acceptance from automated tests.

MANUAL ACCEPTANCE

Do not launch the interactive game.

After automated validation, provide Neal with concise commands for manual verification, including at minimum:

dotnet run --project src\Tempest.Game -- --start-level 10
dotnet run --project src\Tempest.Game -- --start-level 10 --atari-presentation

Also identify the most practical manual check for confirming Level 10 -> Level 11 progression without changing production behavior solely to make the test easier.

Level 10 itself remains previously manually accepted. This manual gate concerns the corrected startup/progression integration and regression checking, not re-accepting its entire gameplay implementation from scratch.

SCOPE

Do not implement Level 13.
Do not change unrelated gameplay.
Do not tune existing tube presentation.
Do not redesign --start-level.
Do not commit or push yet.
Do not stage unrelated files.

Preserve unrelated tracked/untracked work already present in the worktree.

AI LOGGING

This is substantial Tempest work. Preserve this prompt under AI-Logs/Prompts and create a new paired report under AI-Logs/Reports according to project instructions. Do not rewrite historical reports.

The report must clearly distinguish:
- root cause;
- historical/stale diagnostic structure discovered;
- implementation changes;
- automated validation;
- remaining manual validation;
- any unresolved issue.

STOPPING CONDITION

Stop after the correction and automated validation are complete and provide the manual-test handoff.

If validation is blocked by Windows Application Control, Smart App Control, a stalled/hung operation, or another environmental condition, follow the repository's established safe-stop policy. Do not weaken security, retry the same blocked artifact indefinitely, substitute stale results, or claim blocked validation passed.

Do not commit or push. Manual acceptance comes first.

TEMPEST_LEVEL10_PRODUCTION_CHAIN_FIX
