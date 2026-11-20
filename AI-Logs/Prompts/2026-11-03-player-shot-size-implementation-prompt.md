# Player-Shot Size Refinement

Make the smallest rendering-only adjustment needed to make the Blaster's
player shots slightly larger, following Neal's arcade observation. First
inspect the C# shot geometry, perspective scaling, collision/gameplay
coupling, existing tests, and Mark Incitti's shot renderer. Do not redesign
projectile behavior or copy reference constants automatically.

Preserve shot speed, cadence, active limit, lifetime, collision, scoring,
enemy/Spike interactions, and all other gameplay behavior. Keep the
depth-dependent rendering finite and test that the first bounded adjustment
is larger than the accepted checkpoint. Update only materially affected
documentation, save an implementation report, run focused and full tests,
build, and `git diff --check`. After automated validation, stop for Neal's
manual visual comparison; do not tune repeatedly.
