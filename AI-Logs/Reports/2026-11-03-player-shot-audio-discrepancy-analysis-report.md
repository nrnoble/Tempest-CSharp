# Player Blaster-shot audio discrepancy: analysis only

Date: 2026-11-03

## Finding

The reference does not merely call PlaySound with an automatically allocated
voice. It sends every player shot through one persistent, preallocated channel:
`PlaySound(shotsfx, schan)`.

Classic FreeAudio replaces the playback on that channel, stops its previous
output, and starts the new playback at sample zero. The current C# AudioSystem
instead selects an idle voice from four independently playing voices. It does
not interrupt an earlier player shot. This is a concrete implementation
discrepancy, not evidence that the WAV selection is wrong.

**Most likely explanation:** during sustained firing, the reference repeatedly
replaces the preceding shot before the approximately 169 ms WAV finishes.
Our implementation overlaps complete copies, exposing and combining substantially
more of the waveform's later content. Direct Windows playback also exposes the
whole waveform, without reference-style retriggering.

**Important limit:** a genuinely isolated reference shot, or the last shot after
release, is allowed to finish. Channel reuse cannot by itself explain a difference
between isolated, uninterrupted playback in Incitti and Windows. That distinction
must remain explicit; the reported listening comparison did not establish a
controlled isolated-tap versus sustained-fire comparison.

No tuning, acceptance session, replacement search, playback experiment, or
application/asset edit was performed during this investigation.

## Scope and evidence strength

Neal reports that the other implemented sounds and their timing sound correct,
but player firing sounds wrong in C# and in direct Windows WAV playback, while
Incitti firing sounds correct. Those are supplied observations, not new listening
results from this investigation.

The entire checked-in [Incitti reference](../../Reference/incitti/) was searched:
all four BMX source files, Readme, and seven levelsets, not just sfx.bmx.
Searches included standalone shotsfx references, sound loading/play/cue calls,
all channel names and allocation, stop/pause/resume, rate/volume/pan/depth,
position/seek/time controls, driver selection, and direct object-method variants.

Game-level findings below are established from the checked-in source. Native
channel semantics are corroborated by public classic BlitzMax/FreeAudio source;
the exact compiler/runtime/backend embedded in Neal's historical executable has
not been identified or instrumented. No assertion is made that that binary's
runtime is byte-identical to the public source inspected.

## Complete reference player-shot path

| Location | Operation | Consequence |
|---|---|---|
| [sfx.bmx:3](../../Reference/incitti/sfx.bmx#L3) | Import BRL.FreeAudioAudio | FreeAudio driver integration. |
| [sfx.bmx:4](../../Reference/incitti/sfx.bmx#L4) | Import BRL.Wavloader | WAV decoding. |
| [sfx.bmx:10](../../Reference/incitti/sfx.bmx#L10) | Global shotsfx:TSound | Shared loaded sound resource. |
| [sfx.bmx:34](../../Reference/incitti/sfx.bmx#L34) | Incbin "sfx/shot.wav" | The WAV is embedded, not selected dynamically. |
| [sfx.bmx:49](../../Reference/incitti/sfx.bmx#L49) | shotsfx = LoadSound("incbin::sfx/shot.wav") | No loop/hardware flags or sound modification requested. |
| [tempest.bmx:195](../../Reference/incitti/tempest.bmx#L195) | LoadSfx() | Startup loading before gameplay. |
| [tempest.bmx:200](../../Reference/incitti/tempest.bmx#L200) | MainPlayer = Player.Create() | Creates the persistent player. |
| [tempest.bmx:605](../../Reference/incitti/tempest.bmx#L605) | Field schan:TChannel = Null | Comment explicitly says "send all shot audio through this channel". |
| [tempest.bmx:983](../../Reference/incitti/tempest.bmx#L983) | p.schan = AllocChannel() | One preallocated channel, not one allocation per shot. |
| [tempest.bmx:612](../../Reference/incitti/tempest.bmx#L612) | Player.AddShot -> Shot.Create | Creates a shot; no audio operation inside Shot.Create. |
| [tempest.bmx:650](../../Reference/incitti/tempest.bmx#L650) | PlaySound(shotsfx, schan) | The only player shotsfx playback call. |

The only standalone shotsfx references are its declaration, its LoadSound
assignment, and the playback at line 650. The only schan references are its
field, that playback, and its allocation. It is not reassigned per shot/life/level.

At [Player.Update](../../Reference/incitti/tempest.bmx#L638), firing requires:

- Left Ctrl, right Ctrl, or mouse button 1 held.
- shottimer > 2, deathcount = 0, and shot.count < 10.
- Self.AddShot(), immediately followed by PlaySound(shotsfx, schan).
- Reset shottimer to zero, then increment it once per update.

There is no independent audio timer, ChannelPlaying gate, cue/resume phase,
delayed start in game code, scheduled tail stop, collision-driven shot-channel
stop, or sound-only trigger. Actual shot creation and sound triggering share
the same accepted firing branch.

MainPlayer.Update is called at
[tempest.bmx:3755](../../Reference/incitti/tempest.bmx#L3755), unless the level
is in the vortex. The loop budgets 16 ms before remaining work and Flip2
([line 3808](../../Reference/incitti/tempest.bmx#L3808)).
[Flip2](../../Reference/incitti/tempest.bmx#L209) calls Flip with sync=-1 by
default and handles window/application state; it does not manipulate audio.
Three updates therefore suggest roughly 48 ms or longer between accepted
sustained shots under nominal pacing, not a measured or guaranteed 48 ms.
Shot limits, death/state gates, scheduling, rendering, and display waits can
increase the interval.

## What FreeAudio actually does on channel reuse

Public upstream source inspected on 2026-11-03:

1. [BRL.Audio](https://github.com/blitz-research/blitzmax/blob/master/mod/brl.mod/audio.mod/audio.bmx):
   LoadSound defaults flags to zero; PlaySound(sound, channel) delegates to
   sound.Play(channel).
2. [BRL.FreeAudioAudio](https://github.com/blitz-research/blitzmax/blob/master/mod/brl.mod/freeaudioaudio.mod/freeaudioaudio.bmx):
   TFreeAudioSound.Play forwards the supplied channel handle to
   fa_PlaySound(fa_sound, False, fa_channel). CreateSound passes the decoded
   sample's length, bit depth, channel count, and hertz to fa_CreateSound.
   The plain LoadSound call does not request looping.
3. [freeaudioglue.cpp:25](https://github.com/blitz-research/blitzmax/blob/master/mod/pub.mod/freeaudio.mod/freeaudioglue.cpp#L25):
   fa_PlaySound retrieves the existing channel output, creates playback through
   audio->play, then calls io->setchannel(ch, out).
4. [freeaudio.h:284](https://github.com/blitz-research/blitzmax/blob/d508336422aeaae76c62574929c3e7b46271798a/mod/pub.mod/freeaudio.mod/freeaudio.h#L284):
   systemio::setchannel obtains the old output, calls old->stop(0), and installs
   the new output on the same logical channel.
5. [freeaudio.cpp:68](https://github.com/blitz-research/blitzmax/blob/master/mod/pub.mod/freeaudio.mod/freeaudio.cpp#L68):
   mixer::play sets the new sample position and mix position to zero and queues
   the new playback. It does not resume the old position or queue the new WAV
   to play after the preceding WAV completes.
6. [freeaudio.cpp:41](https://github.com/blitz-research/blitzmax/blob/master/mod/pub.mod/freeaudio.mod/freeaudio.cpp#L41):
   defaults are rate 65536 (unity fixed-point), volume VOLUMEONE, centered pan,
   and zero depth; reuse inherits the channel's target settings. Incitti never
   changes those settings.

The game itself contains no StopChannel call, but the backend stops the old
playback as part of replacing the channel. This distinction is essential.

This is a logical single-channel replacement, not necessarily in-place reuse of
one native playback object. Hardware/mixer buffers already submitted cannot be
assumed to disappear at precisely the trigger timestamp. The inspected FreeAudio
mixer also has a nominal 20 ms start-time scheduling calculation
([freeaudio.cpp:391](https://github.com/blitz-research/blitzmax/blob/master/mod/pub.mod/freeaudio.mod/freeaudio.cpp#L391)).
Consequently, do not interpret a nominal 48-50 ms firing interval as proof of
an exact sample-accurate 48-50 ms audible prefix.

## Answers to the ten requested questions

1. **Entire WAV normally?** Yes when uninterrupted: loaded as a non-looping
   sound at its original sample rate, starting at sample zero. There is no
   explicit cropped region. No guarantee of full playback during repeated fire.
2. **Stopped/replaced before completion?** Yes, if another player shot is
   accepted before completion. Reusing schan replaces/stops the previous output.
   There is no separate fixed-duration stop.
3. **Repeated firing retriggers/interferes?** Yes: repeated starts from zero
   on the same channel, rather than overlapping complete copies. The leading
   portion becomes recurrent, while later portions of interrupted copies do
   not continue as independent voices. The last copy can finish.
4. **Persistent/preallocated channel?** Yes: Player.schan, allocated once in
   Player.Create. It is distinct from the separate global zapchan at
   [tempest.bmx:161](../../Reference/incitti/tempest.bmx#L161).
5. **Rate/pitch modification?** None in the game source. No SetChannelRate or
   equivalent shot-rate manipulation. Original sample hertz is passed to the
   backend; backend sample-rate conversion is not an intentional pitch change.
6. **Volume modification?** None in the game source. No channel-volume setting,
   envelope command, or player-shot gain adjustment.
7. **Additional timing condition beyond shot creation?** No independent
   audio-specific condition. Input/timer/death/shot-count and level-update gates
   govern actual shot creation; sound immediately accompanies that creation.
8. **Another sound masking/combining?** Other sounds can coincide conditionally:
   movement ticks at lines 618/629, score bonus at 745, and enemy/collision/pulse
   events elsewhere. None is unconditionally layered with every player shot.
   Other effects do not use schan, so they do not replace it. Perceptual masking
   in Neal's particular comparison is unmeasured, not established or ruled out.
9. **Intentionally compound WAV dependent on playback?** The waveform has
   repeated and evolving internal structure (below), but authorial intent and
   the meaning "several shots" cannot be inferred from peaks. Reference-style
   retriggering demonstrably changes which parts persist during sustained fire.
   No evidence justifies replacing or destructively editing this asset.
10. **Backend difference that materially explains this?** Channel policy is
    the concrete difference: FreeAudio single-channel replacement versus our
    multi-voice use of Raylib. Raylib itself supports restarting the same Sound;
    this is not a missing capability. Mixer interpolation, resampling, buffering,
    and device latency also differ, but have not been shown to explain the
    reported perceptual difference independently of that channel-policy mismatch.

No CueSound, SetChannelRate/Volume/Pan/Depth, StopChannel, ResumeChannel,
PauseChannel, channel seek, or direct equivalent was found in the Incitti game
source. The only explicitly allocated channels are schan and zapchan.

## Read-only WAV inspection

Asset: [sfx/shot.wav](../../sfx/shot.wav). Measurements read its bytes directly
with PowerShell/.NET; no decoder rewrite, export, trimming, conversion, or playback.
Sample offsets below are zero-based; time = sample index / 22050.

| Property | Measured value |
|---|---|
| File size / RIFF declared payload | 7,484 / 7,476 bytes |
| Chunks | fmt at byte 12 (16-byte payload); data at byte 36 |
| PCM payload starts / length | byte 44 / 7,440 bytes |
| Format | PCM format 1, mono, signed 16-bit little-endian |
| Sample rate / byte rate / block alignment | 22,050 Hz / 44,100 bytes/s / 2 |
| Frames / duration | 3,720 / 168.707483 ms |
| Extra cue/loop/position chunks | None; only fmt and data |
| Minimum / maximum sample value | -177 / 1,911; no full-scale clipping |
| Global RMS | 882.483 integer PCM units |
| Absolute maximum | sample 1,135, 51.473923 ms |
| SHA256 | 1375E27CE3F9B39C81EE2A65FD22D1BBE9BFE0F76BF8ABC1B1751205FBFEB744 |

### Internal timing, without labeling peaks as shots

There is a particularly strong repeated motif: samples [0,760) and [760,1520)
cover 0-34.467120 ms and 34.467120-68.934240 ms. Corresponding samples differ
by at most 2 PCM units; root-mean-square difference is 0.701502 and normalized
dot-product similarity is 0.9999996543. That is strong evidence for an internal
repeated waveform motif, not merely two unrelated amplitude peaks. It is not
proof that either segment was intended to be a separate player shot.

The following representative envelope observations are reproducible using
1 ms bins with RMS >= 1,000 PCM units. Bins begin at ceil(ms * 22.05), and end
offsets are exclusive. The threshold is descriptive, not a shot detector.

| Sample interval | Time interval (ms) | Observation |
|---|---|---|
| [111,177) | 5.034-8.027 | Early high-energy cluster |
| [287,353) | 13.016-16.009 | Another early cluster |
| [662,684), [706,750) | 30.023-31.020; 32.018-34.014 | Clusters near end of first motif |
| [860,949) | 39.002-43.039 | Early cluster in repeated motif |
| [1037,1081), [1103,1125) | 47.029-49.025; 50.023-51.020 | Further repeated-motif energy |
| [1412,1456), [1478,1500) | 64.036-66.032; 67.029-68.027 | Late repeated-motif energy |
| [2536,2602) | 115.011-118.005 | Later distinct envelope cluster |
| [2691,2735) | 122.041-124.036 | Later cluster |
| [2933,2977) | 133.016-135.011 | Later cluster |
| [3087,3154) | 140.000-143.039 | Later cluster |
| [3242,3330) | 147.029-151.020 | Later cluster |
| [3418,3506) | 155.011-159.002 | Later cluster |
| [3617,3683) | 164.036-167.029 | Final substantial cluster |

For examples with exact threshold crossings rather than bin boundaries,
contiguous runs with abs(sample) >= 1,000 include [2546,2605)
(115.465-118.141 ms), [3090,3156) (140.136-143.129 ms), and [3616,3683)
(163.991-167.029 ms). These are high-amplitude waveform lobes, not independently
identified acoustic shot onsets.

There are no long quiet separators: the longest contiguous run with
abs(sample) <= 64 is 87 samples, starting at 3,503 (158.866 ms), lasting
3.945578 ms. The waveform remains energetic late in the file; it is not simply
one initial click followed by a silent remainder. Individual oscillator lobes
and envelope clusters must not be counted as separate gameplay shots.

The measurements establish internal repetition and later energy consistent with
the user's impression of a compound sound when played completely. They do not
establish why the author chose that structure or reproduce Neal's listening test.

## Concrete comparison with the current C# implementation

[AudioSystem](../../src/Tempest.Game/Systems/AudioSystem.cs#L7) uses
VoicesPerSound = 4 for every sound, including PlayerShot:

- One loaded source plus three LoadSoundAlias voices.
- [Play](../../src/Tempest.Game/Systems/AudioSystem.cs#L44) skips voices already
  playing and starts the first idle one.
- Previous player-shot voices continue to their ends; there is no replacement.
- If all voices are busy, a new event is skipped, rather than replacing one.
- No shot-specific rate, volume, pan, depth, or truncation setting.

[UpdatePlayerFiring](../../src/Tempest.Game/TempestGame.cs#L572) emits PlayerShot
only after actual shot creation. Normal cooldown is three updates, spike-exit
cooldown four. [Program](../../src/Tempest.Game/Program.cs#L40) targets 60 FPS:
nominal spacing is 50 ms normally and 66.667 ms in the exit case, not a measured
wall-clock guarantee.

At nominal 50 ms spacing and a 168.707 ms WAV, C# can mix four copies at once.
The first copy retains approximately 118.707 ms of playback after the second
trigger. The reference replaces that first copy rather than retaining its full
tail. At slower firing or after release, fewer copies overlap in C#, and the
reference's last copy is permitted to finish.

The installed Raylib-cs 8.1.0 package README explicitly identifies native
**Raylib 6.0**, not 5.5. Matching public
[Raylib 6.0 raudio.c](https://github.com/raysan5/raylib/blob/dbc56a87da87d973a9c5baa4e7438a9d20121d28/src/raudio.c)
confirms:

- LoadSoundFromWave (line 944): converts source frames to device format/rate
  at load time, rather than intentionally changing pitch.
- LoadSoundAlias (line 985): shared PCM data, separate playback buffers/state.
- PlaySound (line 1193): calls PlayAudioBuffer.
- PlayAudioBuffer (line 662): explicitly restarts the buffer, setting its
  frame cursor to zero even if already playing.
- New buffers/aliases default to unity volume and pitch.

Thus a single persistent Raylib Sound could reproduce the key restart policy
without changing the asset, firing cadence, volume, or pitch. The current code
deliberately avoids calling PlaySound on that already-playing voice.

FreeAudio uses its own mixer/linear interpolation and Windows driver paths;
Raylib uses miniaudio and device-format conversion. The exact active historical
Windows backend and actual output sample rates/latencies were not measured.
There is no evidence here of a hidden Incitti pitch/gain transform or a special
single-shot WAV region. If isolated taps still differ after controlling channel
policy, runtime/embedded-asset identity and output/backend behavior remain
separate diagnostic questions, not grounds for speculative tuning now.

## Smallest proposed experiment -- NOT implemented

After authorization, change only PlayerShot playback policy to reuse the same
loaded Sound for every existing PlayerShot event and call Raylib.PlaySound on
it even while playing. Raylib already restarts it from zero; no extra rate,
gain, cadence, WAV edit, or explicit timed stop is needed. Leave other eleven
sounds and all gameplay behavior unchanged. Existing aliases may remain
allocated for this small experiment; resource-allocation cleanup is not needed
to test the hypothesis.

Compare the current and single-voice policies against Incitti under:

1. A stationary isolated tap, then wait more than 169 ms.
2. Stationary sustained fire, avoiding conditional movement/collision effects
   as far as the existing gameplay permits.
3. Release after sustained fire and listen specifically to the final tail.

Expected discriminator: the channel-policy change primarily affects sustained
fire and overlap, not the uninterrupted isolated WAV or final full tail.
If only sustained fire converges, the mechanism is supported. If an isolated
tap still differs, the current evidence does not fully explain the observation;
verify executable/embedded-WAV identity and runtime output before tuning.
This is a proposed later diagnostic, not permission to begin manual acceptance.

## Preservation and validation

- Only this ignored follow-up report was added by this analysis.
- Application code, firing cadence, audio settings, and all WAVs were not edited.
- Recorded shot.wav SHA256 matched again after analysis.
- The tracked working diff fingerprint before/after report creation matched:
  E7FD2910545B3F21F72B7A77A0D8BF46BE277D9CD2D6FF91161D35024EEAEA9C.
  This hashes UTF-8 bytes of git diff --binary joined with LF.
- Existing implementation edits and untracked assets/tests remain unstaged;
  git diff --cached --name-only is empty.
- The report is ignored under AI-Logs. TEMP was not touched.
- No build/tests were rerun: this is read-only investigation plus documentation,
  not a playback or code change. Previous implementation test results are not
  evidence of perceptual equivalence.

TEMPEST_PLAYER_SHOT_AUDIO_DISCREPANCY_ANALYZED
