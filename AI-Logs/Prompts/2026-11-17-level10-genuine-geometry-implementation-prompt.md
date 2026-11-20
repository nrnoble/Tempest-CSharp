# Implement diagnostic Level 10 with recovered genuine geometry

Implement Experiment 2: retain diagnostic Level 10's Level-7-like gameplay while replacing the Level 7 model and Atari-presentation aliases with the established ROM-backed displayed-Level-10 shape-row-9 geometry. Do not rediscover geometry; use the authoritative values in `AI-Logs/Reports/2026-11-17-level10-geometry-recovery.md`.

The 16 ordered points form an open polyline with 15 playable segments and no 15-to-0 closing segment. Keep Level 7 unchanged as frozen diagnostic evidence, preserve all pre-existing worktree changes, keep Level 10 outside campaign progression, and adapt only geometry-dependent routing required by the open topology. Preserve direct Level 10 startup and deterministic recording/playback in both Atari-presentation modes. Do not research authentic Level 10 gameplay, investigate the motion defect, inspect screenshots/videos, launch the game, change unrelated diagnostics, or update durable behavior specifications.

Add focused tests for the recovered geometry and ordering, open topology, distinction from Level 7, reused gameplay configuration, Atari presentation using Level 10 geometry, direct startup, recording/playback, and unchanged Level 7 behavior. Run bounded focused tests, the bounded full regression suite, Release build, and `git diff --check`, using existing timeout/hang safeguards. Create a paired implementation report documenting evidence, changes, results, Git state, and exact recording/playback commands. Stop for Neal's manual comparison; report its result as PENDING. Do not stage, commit, push, or launch the interactive game.

TEMPEST_IMPLEMENT_DIAGNOSTIC_LEVEL10_GENUINE_GEOMETRY
