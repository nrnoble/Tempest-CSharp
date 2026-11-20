Faithful task specification reconstructed by the VS Code Agent. The original prompt was not archived verbatim during execution.

# Task: Documentation-only maintainability comment pass

Add comments and XML doc comments only. No executable change, renames, moves, or test changes.

Scope:
- Comment the WHY in C# sources, distinguishing established arcade behavior, behavior derived from Incitti, development-only behavior, and approximations needing arcade comparison.
- Areas: Flippers (climb, lethal direct rim arrival, rim pause, pivot, shootability on both lanes, bullets, cargo init), Eggs, death/reset, normal accounting, Spikers/spikes, enemy bullets, Tankers (dev-only, outward cargo, slot versus cargo identity), projection/rendering math. Tests: occasional organizational comments only.
- Add to TEMPEST-DEVELOPMENT-NOTES.md a SUSPECTED / NOT YET REPRODUCED entry: two rim Flippers approaching from opposite directions may converge on one lane and look like one Flipper; not reproduced, no gameplay change.
- Update .github/copilot-instructions.md with the maintainability-comment rule and process rules (no replacing working code merely for cleanliness; identify arcade/Incitti/C# disagreements; avoid broad refactoring in bug fixes; preserve validated behavior; report instruction conflicts).
- Archive this prompt and a report. Do not modify Reference/incitti. Do not commit or push.

Verification: dotnet build, dotnet test (expect 146 passing), git diff --check, and a comment-stripped comparison proving executable C# is unchanged.
