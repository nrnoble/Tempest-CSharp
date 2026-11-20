IMPORTANT: Read this entire prompt before taking any action.

This prompt is complete only if you can find the exact completion token at the very bottom of the prompt, appearing exactly once.

Before using any tool, reading or modifying any repository file, or running any command:

1. Read the entire prompt.
2. Locate the completion token at the bottom.
3. Verify that it appears exactly once.
4. If you cannot find it exactly once, STOP immediately and report that the prompt is incomplete.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Current accepted clean checkpoint:
a666ca2 Refine accepted Blaster shot size

IMPORTANT CURRENT WORKTREE CONDITION

The directory:

sfx/

is intentionally present and currently untracked.

It contains 12 authoritative WAV sound assets copied from Mark Incitti's Tempest implementation.

These WAV files are required game assets and MUST be included in this milestone and tracked by Git.

Do not:
- delete them;
- rename them;
- alter their contents;
- convert them;
- normalize them;
- resample them;
- trim them;
- add sfx/ or *.wav to .gitignore;
- move them into AI-Logs/, Videos/, Temp/, bin/, or obj/.

TEMP/ remains disposable and must not be modified, staged, deleted, cleaned, or otherwise disturbed.

MILESTONE

Implement the first complete sound-effects/audio foundation using the existing authoritative WAV assets.

BACKGROUND

Sound effects are important to gameplay and are not optional cosmetic polish.

The authoritative assets are:

sfx/bonus.wav
sfx/bullet.wav
sfx/flippershot.wav
sfx/killedbybullet.wav
sfx/killedbyflipper.wav
sfx/pulse.wav
sfx/shot.wav
sfx/spikeshot.wav
sfx/tick.wav
sfx/zap.wav
sfx/zoomin.wav
sfx/zoomout.wav

All 12 files are mono, 16-bit, uncompressed PCM WAV files at 22,050 Hz.

The filenames and use correspond to Mark Incitti's sfx.bmx and tempest.bmx.

The current C# AudioSystem.cs is empty, and Tempest.Game.csproj currently has no WAV content-copy rule.

GOAL

Add a small, maintainable audio system that:

- owns and loads the WAV assets once;
- plays sounds in response to established gameplay events;
- unloads all audio resources cleanly;
- does not load resources on every playback;
- does not make headless tests require an audio device;
- does not alter deterministic gameplay behavior;
- does not depend on the process working directory;
- supports appropriate overlapping one-shot playback;
- keeps authoritative source assets under the repository-root sfx/ directory.

Do not build a generalized audio engine, mixer, options screen, volume menu, dependency-injection framework, or configuration system.

SOURCE-VERIFIED EVENT MAPPINGS

Use the following mapping unless inspection of the current production path demonstrates that a mapped gameplay event does not yet exist.

1. tick.wav
   - Blaster lane movement.
   - Incitti also uses it for menu selection, but do not invent a menu-selection event if the C# project has no equivalent.
   - Avoid turning continuous spinner movement into an uncontrolled wall of overlapping tick sounds.
   - Match Incitti's lane-movement event semantics as closely as the current C# movement model permits.

2. shot.wav
   - A player shot is actually created/fired.
   - Do not play merely because Fire is held or pressed when firing is blocked.
   - Respect the existing cooldown, active-shot limit, death state, exit-state firing gates, and other established firing behavior.

3. bullet.wav
   - An enemy bullet is actually created/fired.
   - Do not play when a Tanker or other enemy attempts to fire but no bullet is created.

4. spikeshot.wav
   - A player shot successfully shortens a Spike by one segment.
   - Do not play for a miss or for an unrelated shot impact.

5. pulse.wav
   - A Pulsar begins its pulse state.
   - Trigger at the pulse-state transition, not every update while the Pulsar remains pulsing.

6. zap.wav
   - An enemy completes its delayed Super Zapper death/resolution sequence.
   - Do not play simply when the Super Zapper button is pressed.
   - For the first all-target charge, each enemy should produce the sound when its own delayed zap resolution completes, consistent with the existing staggered resolution behavior.
   - Do not play for rejected entities, bullets, Spikes, canceled/pre-empted zap resolutions, or enemies destroyed by an ordinary shot before zap expiry.

7. killedbybullet.wav
   - The Blaster is actually destroyed by an enemy bullet.

8. killedbyflipper.wav
   - The Blaster is destroyed by a Flipper.
   - Incitti reuses this sound for Spike, Pulsar, and Fuseball deaths.
   - Follow that established reuse for current corresponding C# death causes.
   - Inspect Tanker/Spiker or other current death paths carefully rather than assigning unsupported death sounds.

9. flippershot.wav
   - A Flipper is destroyed by a player shot.
   - Incitti reuses this sound for Pulsar, Fuseball, Tanker, and Spinner/Spiker destruction.
   - Follow the reference reuse where equivalent C# destruction events exist.
   - Do not play for escape, retreat, natural Tanker release, zap resolution, or non-destructive hits.

10. zoomout.wav
    - Established tube-exit/zoom-out transition.
    - Align playback with the actual accepted exit presentation rather than merely with an arbitrary level-completion flag.

11. zoomin.wav
    - Established between-level approach/zoom-in transition.
    - Align playback with the accepted LevelApproach presentation.
    - Do not restart it every update.

12. bonus.wav
    - Extra life awarded at the score threshold.
    - First determine whether the current C# project actually implements an extra-life award.
    - If that behavior exists, play the sound when the life is actually awarded.
    - If the behavior does not exist, retain the asset and semantic AudioSystem method but do not invent an extra-life system in this milestone.
    - Document the sound as awaiting the future extra-life gameplay event.

ARCHITECTURE

Use the smallest suitable architecture for this project.

Preferred direction:

1. AudioSystem owns all loaded Raylib Sound resources.
2. AudioSystem has semantic methods such as:
   - PlayPlayerMove()
   - PlayPlayerShot()
   - PlayEnemyBullet()
   - PlaySpikeHit()
   - PlayPulsarPulse()
   - PlayZapResolution()
   - PlayPlayerKilledByBullet()
   - PlayPlayerKilledByEnemy()
   - PlayEnemyDestroyed()
   - PlayZoomIn()
   - PlayZoomOut()
   - PlayBonusLife()

Names may differ if another clear, maintainable scheme fits the existing code better.

Do not expose WAV paths throughout TempestGame or individual entity classes.

3. The production composition root initializes the Raylib audio device and creates the real AudioSystem.

4. The normal shutdown path:
   - unloads all Sound resources;
   - then closes the audio device;
   - remains safe during ordinary application exit.

5. TempestGame and headless tests must not require real audio hardware or WAV loading.

Use a narrow test seam appropriate to the project, such as:
- an interface with real and silent implementations; or
- a callback/event sink arrangement.

Choose the smallest clear approach.

Do not introduce a general service container.

6. Audio failures must not alter simulation state, scoring, enemy accounting, level progression, or input behavior.

ASSET PATH AND BUILD OUTPUT

Keep the authoritative files at:

repository-root/sfx/*.wav

Configure Tempest.Game.csproj so the required WAV files are copied or linked into a predictable sfx/ directory under the build output.

Runtime loading must resolve assets relative to the application base/output directory, not the shell's current working directory.

Both of these launch patterns must be supported:

dotnet run --project src/Tempest.Game

dotnet run --project src/Tempest.Game -- <existing options>

Do not create manually maintained duplicate source copies of the WAV files.

OVERLAP AND REPETITION

Inspect Raylib sound behavior and current gameplay timing.

Requirements:

- rapid player shots must remain audible without corrupting gameplay;
- simultaneous or closely staggered enemy events must be handled sensibly;
- the delayed all-target Super Zapper may resolve multiple enemies at different ticks;
- a sound event must occur exactly once for each qualifying gameplay transition;
- state sounds such as Pulsar pulse and zoom transitions must not restart every simulation update;
- tick.wav must not become uncontrolled noise merely because high-resolution spinner input is sampled frequently.

If Incitti's exact playback behavior cannot be reproduced through the simplest Raylib call, preserve gameplay correctness and document the reasonable compromise.

GAMEPLAY SAFETY

Do not change:

- spinner or controller behavior;
- keyboard/mouse fallback behavior;
- firing cadence;
- shot limit;
- collision ordering;
- enemy movement;
- scoring;
- Super Zapper timing or selection;
- player-death causes or timing;
- death animation;
- enemy retraction;
- LevelApproach timing;
- tube-exit timing;
- level progression;
- score rollover;
- lives;
- enemy accounting;
- deterministic simulation behavior.

This milestone attaches sounds to existing accepted events. It must not redesign those events.

TESTS

Add focused automated coverage for the semantic audio-event seam without requiring:
- Raylib audio initialization;
- an audio device;
- WAV decoding during the test suite.

At minimum test, where the current production behavior provides the event:

1. A successfully created player shot emits one player-shot audio event.
2. Blocked firing emits no player-shot audio event.
3. A created enemy bullet emits one enemy-bullet event.
4. A successful Spike shortening emits one Spike-hit event.
5. A Pulsar entering pulse emits one pulse event and does not repeat every pulsing update.
6. An ordinary eligible enemy destruction emits one enemy-destruction event.
7. A natural Tanker release or enemy escape does not emit an enemy-destruction event.
8. Enemy-bullet player death emits the bullet-death event.
9. Applicable enemy-contact/Spike/Pulsar/Fuseball death emits the shared enemy-death event.
10. A completed zap resolution emits one zap event.
11. A pending zap pre-empted by ordinary destruction does not later emit a zap event.
12. Exit zoom emits zoom-out once.
13. LevelApproach emits zoom-in once.
14. Bonus sound behavior accurately reflects whether extra-life awarding currently exists.
15. Existing headless tests still run without audio hardware or copied runtime assets.

Do not write tests that merely duplicate private implementation details.

Prefer tests around observable semantic audio events and existing gameplay transitions.

DOCUMENTATION

Update appropriate tracked documentation to describe:

- sound effects are now implemented from the authoritative sfx/ WAV assets;
- sfx/ is required tracked game content;
- event mappings;
- any deliberately deferred sound event such as bonus.wav if extra-life logic does not yet exist;
- headless tests use a silent or recording audio seam;
- audio does not alter simulation behavior.

Keep evidence classifications honest:

- Incitti mappings are REFERENCE VERIFIED.
- The C# architecture and any necessary playback compromises are PROJECT DECISIONS.
- Do not label audio behavior ARCADE VERIFIED until Neal manually evaluates it.

Also save:

AI-Logs/Prompts/2026-11-02-audio-sfx-implementation-prompt.md

and an implementation report under:

AI-Logs/Reports/

using established naming conventions.

The AI-Logs content remains ignored and must not be staged.

VALIDATION

Run:

1. Focused audio tests.
2. The complete test suite:
   dotnet test TempestCSharp.slnx
3. Build:
   dotnet build TempestCSharp.slnx
4. git diff --check
5. git status --short --untracked-files=all
6. Full tracked diff review.
7. Confirm all 12 required sfx WAV files are visible as intended Git additions and are not ignored.
8. Confirm TEMP/ remains untouched.
9. Confirm AI-Logs remain ignored.
10. Confirm WAV files were not modified during implementation beyond becoming tracked additions.

Do not stage.
Do not commit.
Do not push.

MANUAL ACCEPTANCE CHECKLIST

Provide exact launch commands and a bounded checklist covering:

- spinner movement tick behavior;
- player firing;
- blocked firing;
- enemy bullet firing;
- ordinary enemy destruction;
- Spike shortening;
- Pulsar pulse;
- Super Zapper staggered resolutions;
- bullet-caused player death;
- enemy/contact-caused player death;
- zoom-out;
- zoom-in;
- no obvious audio restart loops;
- no obvious clipping or intolerable overlap;
- no gameplay regression.

Explicitly state whether bonus.wav can currently be exercised.

COMPLETION REPORT

At completion report:

- checkpoint and final status;
- files changed and added;
- all sound/event mappings implemented;
- any mapping deferred and why;
- AudioSystem ownership and lifetime;
- build-output asset strategy;
- headless-test strategy;
- focused-test result;
- full-suite result;
- build result;
- git diff --check result;
- exact manual launch commands;
- manual acceptance checklist;
- report path;
- confirmation that nothing was staged or committed.

COMPLETION TOKEN:
TEMPEST_AUDIO_SFX_IMPLEMENTATION_COMPLETE