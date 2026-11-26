# Tempest Level 17 Spike Eligibility Root-Cause Investigation and Bounded Correction

Investigate and correct the Level 17 Spike eligibility defect.

Neal's direct arcade/MAME observation establishes that displayed Level 17 has no Spikes and Spikes do not resume until displayed Level 20. Gameplay other than Spike eligibility is frozen. Presentation and palette are frozen. Prior Level 17 reports are authority for current presentation and gameplay checkpoints.

Required work:

1. Trace the current C# Level 17 starting-Spike initialization and the earlier `$A0` attribution.
2. Trace the ROM `wave_spikeht`, `min_spikers`, and `max_spikers` eligibility/parameter logic.
3. Establish the displayed/internal/cycle-local Level 17-20 boundary and the full 16-position `wave_spikeht` pattern.
4. Apply the smallest generic correction that separates Spike eligibility from parameter data.
5. Audit Level 17 enemy-count evidence without changing shared enemy behavior.
6. Run focused tests, the full suite, Release build, `git diff --check`, and both headless recordings.
7. Create the required substantive prompt and paired report. Do not stage, commit, push, or launch the interactive game.

TEMPEST-END-LEVEL17-SPIKE-ELIGIBILITY-ROOT-CAUSE-CORRECTION-COMPLETE
