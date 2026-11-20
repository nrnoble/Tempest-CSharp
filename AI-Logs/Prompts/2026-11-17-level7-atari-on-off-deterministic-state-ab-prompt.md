# Level 7 Atari on/off deterministic state A/B

Perform a tightly bounded, read-only investigation of the same Level 7
recorded-input playback with Atari presentation disabled and enabled. The
primary question is whether underlying deterministic game state differs, or
whether the observed entity-position/configuration difference is presentation
side. Neal reports the same recording and playback boundaries (100, 150, 200,
250, 300) with matching HUD progression but visibly different entity
presentation. Treat this as manual motivation only; do not inspect images.

Confirm the exact current recording from authoritative runtime metadata,
especially `TEMP\GameplayRecordings\Level-7-slowmo.json`. Use identical
recording content, seed/startup restoration, production simulation, boundaries,
and source/worktree. Inspect the current configuration path to establish
whether `--atari-presentation` can affect game initialization, updates,
collisions, randomness, playback, or other state-changing behavior. Do not
assume it is render-only.

Compare initial restored state and every playback boundary, including the
requested boundaries and enough early boundaries to detect any pre-100
divergence. If identical at those points, continue through full playback.
Fingerprint only authoritative simulation state: level/high-level state,
score/lives/Zapper, wave and ledger counts, stable entity identity/type/state,
lane/depth/movement/routing/firing/cargo/spike/projectile/player state, and
other relevant simulation data. Exclude screen/presentation coordinates,
renderer/camera/framebuffer/timestamps/object addresses/unstable diagnostic
IDs. Do not change production objects or instrument the RNG. Document exact
fingerprint inputs and RNG-state limitations.

If a difference occurs, stop at the first differing boundary and report exact
last-equal/first-different state, then inspect only the narrow causal path. If
all states match, preserve Neal's visual observation, conclude only that the
difference is presentation-side, and recommend the narrowest next rendering
investigation, including whether the existing framebuffer diagnostic should be
used.

Do not modify source, tests, configuration, recordings, instrumentation,
presentation/gameplay behavior, diagnostics, durable documentation, or Git
state. Do not launch the interactive game, capture images, or inspect visual
assets. Preserve the worktree and verify the recording hash before/after.
Only create this prompt and its paired report under ignored `AI-Logs/`.

The report must answer which recording was used; whether the flag leaks into
simulation; initial and sampled state equality; any pre-100 or full-playback
divergence; exact state/fingerprint evidence; the narrowest next step; and
whether framebuffer instrumentation should be exercised, deferred, or
abandoned. End with exactly one allowed evidence classification.

TEMPEST_LEVEL7_ATARI_ON_OFF_DETERMINISTIC_STATE_AB
