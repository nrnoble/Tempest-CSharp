# Level 17 Rev. 2 CPU ROM address-map validation and final evidence recovery

**Scope:** Read-only evidence investigation for Level 17.

**Mandatory objective:** Establish and independently validate the local Atari
Tempest Rev. 2 CPU ROM chip-to-address mapping.

**Constraints:**
- Repository-local ROM and disassembly evidence only.
- No Internet ROM or disassembly replacement material.
- No production code or test changes.
- No durable behavior-specification or development-note changes.
- No Git staging, commit, or push.
- No interactive-game launch.
- Stop after the evidence/readiness report.
- Preserve all pre-existing tracked and untracked work.

**Sequence:** Read the two prior Level 17 reports, validate the CPU address
layout against multiple known-byte anchors, stop recovery if mapping fails,
then recover only the Level 17 Flipper/Pulsar/palette/cargo/shot facts needed
for implementation readiness. Classify each finding as ROM-BACKED,
PROJECT TRANSLATION, APPROXIMATION, UNRESOLVED, or another project terminology
consistent with the durable evidence rules.

**Completion token:** TEMPEST-END-LEVEL17-REV2-ROM-MAP-FINAL-EVIDENCE-COMPLETE
