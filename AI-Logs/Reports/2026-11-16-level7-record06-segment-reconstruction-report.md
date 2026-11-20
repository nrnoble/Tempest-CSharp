# Level 7 Record-06 Atari Vector Segment Reconstruction

**Status: PASS — read-only reconstruction complete**

## Direct answer

**Yes: the C# Level 7 direct-Atari prototype arranges and connects the 16
record-06 points as Atari does.** Atari’s traced vector path connects each
far point to the near point with the same index and uses the same cyclic
neighbors for both rims. The rim traversal direction is reversed, which does
not change the undirected segment graph.

There is one exact-emission difference: Atari emits each visible rail twice,
once in each direction, while C# emits it once. Atari also emits zero-Z
transitions through near-rim neighbors as part of the rail sequence; the
separate rim pass emits those near-rim edges visibly. Thus the simple segment
graphs are equivalent, but the command multisets and draw order are not.
Neither difference rearranges the endpoints or changes the tube’s topology.

## Scope and evidence

The investigation was limited to the requested point arrangement,
connectivity, and vector-command construction. No MAME source or behavior,
screenshots, ROM files, projection formulas, display mapping, or gameplay
systems were investigated. No source, tests, configuration, durable
documentation, or Git state was changed.

Local evidence inspected:

- `Reference/arcade/Tempest Rom Source code.md`: RAM layout; level/tube
  setup at `$c235-$c2e7`; Level tube projection and vector construction at
  `$c30d-$c452`; endpoint projection storage at `$c473-$c4e0`; related
  endpoint consumers at `$b4d8-$b4fc` and `$c66d-$c6a0`; and vector
  serialization helpers at `$c767-$c79d` and `$df70-$dfad`.
- `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`: record-06 catalog and
  point ordering.
- `src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`:
  signed-point packing and projector output shape (the projection formula
  was not re-evaluated).
- `src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs`: current
  endpoints, rails, and rims.
- `src/Tempest.Game/Rendering/VectorRenderer.cs`: prototype emission order.
- `tests/Tempest.Game.Tests/AtariTubeRecordPrototypeTests.cs`: tests for the
  record, endpoints, same-index rails, and cyclic rims.
- `.github/copilot-instructions.md`: current project constraints.

No relevant record-06 segment-connectivity passage was found in
`TEMPEST-BEHAVIOR-SPEC.md` or `TEMPEST-DEVELOPMENT-NOTES.md`.

## Repository starting state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Index: clean
- Worktree: already contained substantial unrelated tracked modifications and
  untracked prototype/reference files, including the C# code inspected here.
  These pre-existing changes were preserved.

## Atari data flow and layout

1. `$c235` selects the tube record and copies its ordered `lev_x`/`lev_y`
   entries into `tube_x`/`tube_y` in a descending loop (`$c2a2-$c2c3`).
   The table and RAM indices remain aligned: each `tube_x[i]` /
   `tube_y[i]` corresponds to record point `i`.
2. The normal tube path is called from `$b371` into `$c30d`.
   `$c30d` calls `$c473` for the far set with position `$f0` and index
   offset `$4f`; it then sets the near position to `$10`, passes through
   `$c453`, and calls `$c473` with index offset `$0f`.
3. `$c473` iterates source indices 15 down to 0, calls `$c098` to obtain the
   two projected coordinate words, and serializes them into four indexed
   byte arrays (`$c47f-$c4dc`). The output index decreases in lockstep with
   the source-point index.

The four low-set arrays occupy 16 bytes each:

| `$c473` store slot | Near set, index `i` | Far set, index `i` |
|---|---:|---:|
| 1 | `$031a+i` | `$035a+i` |
| 2 | `$032a+i` | `$036a+i` |
| 3 | `$033a+i` | `$037a+i` |
| 4 | `$034a+i` | `$038a+i` |

The far-set addresses result from the `$4f` starting offset: the stores at
`$031a/$032a/$033a/$034a` use `$40+i`, yielding `$035a/$036a/$037a/$038a`.
The near-set stores use `$0f` down to `$00`, yielding the same point index
within `$031a-$0349`. There is no point-index permutation between the two
sets.

`$c423` loads the near-set bytes in the order `$032a,$031a,$034a,$033a`;
`$c43c` loads the far-set bytes in the corresponding order
`$036a,$035a,$038a,$037a`. The local `vapp_to(X)` comment at `$c765`
describes the four-byte block as a DVG point (X=`DDCC`, Y=`BBAA`, with the
high three bits dropped). `$c3ba` differences each requested point against
the current point and `$df92` serializes that vector using the current
`draw_z`. These are DVG deltas, but the current-point state resolves them to
the indexed endpoints below.

### Producers and consumers of `$035a-$038a`

- **Producer:** `$c473`, through stores based at `$031a/$032a/$033a/$034a`
  and caller-selected offsets; the far call uses `$4f` through `$40`.
- **Tube-geometry consumer:** `$c43c`, called from `$c3ee`, loads the far
  point at the current rail index and again at the predecessor index.
- **Adjacent-point/effect consumer:** `$c66d-$c6a0` combines neighboring
  far-set coordinates using `(index+1)&$0f` to form an intermediate
  coordinate for the spike/effect path.
- **Pending-segment consumer:** `$b4d8-$b4fc` loads far-set bytes selected
  by `pending_seg` while constructing a pending vector.

These latter consumers do not change the normal tube segment graph.

## Atari segment reconstruction

The `$c342-$c356` loop calls `$c3ee` for indices `i=15,14,...,0` when the
far-set status `$0110` is zero and the rail-enable value `$0113` is nonzero.
For each `i`, `$c3ee`:

1. Loads `Far[i]` via `$c43c`; `$c772` emits a move-to at that point with
   Z off.
2. Calls `$c423`, which loads `Near[i]` and branches to `$c3ba`. With
   `draw_z=$c0`, this emits `Far[i] -> Near[i]`.
3. Decrements the index, wrapping 0 to 15, sets `draw_z=0`, and calls
   `$c423` for `Near[i-1]`. This emits the zero-Z transition
   `Near[i] -> Near[i-1]` and updates the current point.
4. Restores `draw_z=$c0`, loads `Far[i-1]` via `$c43c`, and calls `$c3ba`,
   emitting `Near[i-1] -> Far[i-1]`.

Consequently, across the full 16-iteration closed traversal, every indexed
rail is emitted twice: `Far[i] -> Near[i]` in its own iteration and
`Near[i] -> Far[i]` as the predecessor rail in the preceding iteration
(with wraparound). The intermediate near-neighbor command has Z zero; it is
not an additional visible rim edge.

The rim path at `$c361-$c36e` separately calls `$c36e` first for the far
set (start index `$4f`) and then for the near set (start index `$0f`).
Each pass is gated by its corresponding projection status (`$0110` or
`$010f`). It moves to point 15, then traverses descending indices. The
`open_level` branch at `$c395-$c39a` uses 15 edges for an open tube; for a
closed tube it uses 16 and wraps 0 to 15. Record 06 is treated as closed in
the supplied task context and C# catalog; its ROM metadata value was not
re-investigated because that was explicitly out of scope.

For the normal, valid endpoint-set passes, the normalized **visible**
Atari segment multiset is:

- Far rim: `Far[i] -- Far[(i-1) mod 16]`, once each.
- Near rim: `Near[i] -- Near[(i-1) mod 16]`, once each.
- Rails: `Far[i] -- Near[i]`, twice each (the two emissions have opposite
  direction).
- Additional non-visible commands: one zero-Z `Near[i] -> Near[i-1]`
  transition per rail-loop iteration, plus move-to commands.

The routine gates above are established by the code. Their per-frame
projection status values were not recomputed as part of this connectivity-
only investigation.

## C# construction and comparison

`AtariTubeVectorListBuilder.Build` projects the same source index into the
near and far arrays, adds one `Near[i] -> Far[i]` rail, and for a closed
record adds both cyclic rims as `i -> (i+1) mod 16`. The tests assert these
relationships. `VectorRenderer.DrawAtariTubePrototype` emits rails, then
near rims, then far rims.

| Question | Finding |
|---|---|
| Same near-rim adjacency? | Yes. Atari walks `i -> i-1`; C# walks `i -> i+1`. These are the same undirected cycle. |
| Same far-rim adjacency? | Yes, for the same reason. |
| Same-index rail pairing? | Yes. Atari’s `$c43c`/`$c423` use the same `i` and predecessor index; its rail is `Far[i]--Near[i]`. |
| Any visible Atari segment omitted? | No unique visible edge is omitted. C# emits one copy rather than Atari’s two rail copies. |
| Any extra visible C# segment? | No. |
| Any C# duplication where Atari emits once? | No. Atari duplicates each rail; C# does not. |
| Rim closure mismatch? | No for closed record 06. Both have all 16 rim edges. |
| Intermediate/reordered logical endpoint? | No. Atari’s zero-Z transition and move-to commands are command sequencing, not additional projected tube points. |
| Traversal direction consequence? | None for the undirected segment graph; order and vector direction differ. |
| Exact graph isomorphism ignoring order/display mapping? | Yes as a simple graph; no as an edge-multiset comparison because Atari doubles every rail. |

For the rim proof, substitute `j=i-1 mod 16`: Atari’s edge set
`{(i,i-1)}` is exactly C#’s `{(j,j+1)}`. For rails, both connect only
`Near[i]` and `Far[i]`. Thus there is no index, adjacency, closure, or
pairing mismatch. The only visible edge-multiplicity difference is Atari’s
second emission of each rail.

## Conclusion and remaining boundary

Segment connectivity is **not** a supported cause of the C# tube’s different
shape. The C# builder has the correct points arranged into the same indexed
rim cycles and same-index rails. The duplicate Atari rail commands could
affect line persistence/brightness, but that visual effect is an inference
and does not change the segment graph or explain a different silhouette.
Projection, scaling, placement, and display mapping were excluded and remain
unassessed here.

There is enough evidence for a separate, bounded **visible emission**
candidate if authorized: emit each indexed rail twice, in opposite
directions, while retaining one visible edge per rim pair. The zero-Z
transitions are command-stream details, not visible geometry; reproducing
the entire Atari command stream would be broader than a shape/connectivity
correction. No point reordering, rail remapping, or connectivity correction
is justified by this investigation.

## Validation and files

- Automated tests: not run; this was a read-only source reconstruction and
  made no project-code changes.
- Manual acceptance: not requested or performed.
- Git/source state: no project files, tests, configuration, durable docs,
  protected material, staging area, or commits were changed. Only the
  required ignored AI log files were written.
- Saved prompt:
  `AI-Logs/Prompts/2026-11-16-level7-record06-segment-reconstruction-prompt.md`
- Saved report:
  `AI-Logs/Reports/2026-11-16-level7-record06-segment-reconstruction-report.md`
