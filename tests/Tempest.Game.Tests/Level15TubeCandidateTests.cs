using System.Reflection;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level15TubeCandidateTests
{
    [Fact]
    public void SeededDirectStartCandidateWaveConservesEveryIdentityBeyondStartup()
    {
        TempestGame game = new(LevelConfiguration.Level15DirectStartCandidate, random: new Random(4));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int tick = 0; tick < 126; tick++)
            game.RunOneStep();

        Egg egg = Assert.Single(game.Eggs, e => e.OriginalSlotId == 4);
        Assert.Equal(EggState.Approaching, egg.State);
        Assert.Equal(4, egg.EdgeIndex);
        Assert.Equal(-2, egg.Height);
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(4));
        Assert.Equal(2, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Assert.Contains(game.Spikes, spike => spike.EdgeIndex == 4);

        game.RunOneStep();

        Fuseball fuseball = Assert.Single(game.Fuseballs);
        Assert.Equal(4, fuseball.OriginalSlotId);
        Assert.Equal(4, fuseball.EdgeIndex);
        Assert.Equal(Level3OriginalType.Fuseball, ledger.GetOriginalType(4));
        Assert.Equal(Level3OriginalState.ActiveFuseball, ledger.GetOriginalState(4));
        Assert.DoesNotContain(game.Eggs, e => e.OriginalSlotId == 4);
        Assert.Equal(27, ledger.OriginalCount);
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(game.Tankers[0].OriginalSlotId).Count);
        Assert.False(game.CurrentLevel.IsComplete(0));

        int playingTicks = 0;
        for (int tick = 0; tick < 1000; tick++)
        {
            if (game.State == LevelState.Playing)
                playingTicks++;
            game.RunOneStep();
            Assert.InRange(game.Flippers.Count + game.Tankers.Count +
                game.Spikers.Count + game.Fuseballs.Count, 0,
                Level11WaveSelection.MaximumPhysicalEnemySlots);
            Assert.All(game.Fuseballs, f =>
                Assert.InRange(f.EdgeIndex, 0, game.CurrentLevel.PlayableLaneCount - 1));
        }
        Assert.True(playingTicks > 300);
    }

    [Fact]
    public void DirectFuseballZapResolvesItsOriginalIdentity()
    {
        TempestGame game = new(LevelConfiguration.Level15DirectStartCandidate, random: new Random(4));
        for (int tick = 0; tick < 127; tick++)
            game.RunOneStep();

        Assert.Equal(4, Assert.Single(game.Fuseballs).OriginalSlotId);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        for (int tick = 0; tick < 30; tick++)
            game.RunOneStep();

        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(4));
        Assert.Equal(27, game.CurrentLevel.Level3Ledger.OriginalCount);
    }

    [Fact]
    public void FullCapacityDefersEggUntilAnExistingIdentityResolves()
    {
        TempestGame game = new(LevelConfiguration.Level15DirectStartCandidate, random: new Random(4));
        for (int tick = 0; tick < 50; tick++)
            game.CurrentLevel.UpdateEggRelease();
        foreach (Egg egg in game.Eggs)
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 160 && egg.State != EggState.ReadyToHatch; tick++)
                egg.Update(game.CurrentLevel);
            Assert.Equal(EggState.ReadyToHatch, egg.State);
        }

        game.RunOneStep();
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Egg deferred = game.Eggs.First(e => e.State == EggState.ReadyToHatch);
        int id = deferred.OriginalSlotId;
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(id));
        Assert.Single(game.Eggs, e => e.OriginalSlotId == id);
        Assert.InRange(game.Flippers.Count + game.Tankers.Count + game.Spikers.Count +
            game.Fuseballs.Count + game.Tankers.Count, 0,
            Level11WaveSelection.MaximumPhysicalEnemySlots);

        Assert.Single(game.Fuseballs).TryZap(1);
        game.RunOneStep();
        Assert.Same(deferred, Assert.Single(game.Eggs, e => e.OriginalSlotId == id));
        for (int tick = 0; tick < 50 && ledger.GetOriginalState(id) == Level3OriginalState.Egg; tick++)
            game.RunOneStep();

        Assert.Contains(ledger.GetOriginalState(id), new[]
        {
            Level3OriginalState.ActiveFlipper, Level3OriginalState.ActiveTanker,
            Level3OriginalState.ActiveSpiker, Level3OriginalState.ActiveFuseball
        });
        int representations = game.Flippers.Count(f => f.OriginalSlotId == id) +
            game.Tankers.Count(t => t.OriginalSlotId == id) +
            game.Spikers.Count(s => s.OriginalSlotId == id) +
            game.Fuseballs.Count(f => f.OriginalSlotId == id);
        Assert.Equal(1, representations);
        Assert.DoesNotContain(game.Eggs, e => e.OriginalSlotId == id);
        Assert.Equal(27, ledger.OriginalCount);
    }

    [Fact]
    public void DirectFuseballReconstructsAsTheSameGenericOriginalAfterDeath()
    {
        TempestGame game = new(LevelConfiguration.Level15DirectStartCandidate, random: new Random(4));
        for (int tick = 0; tick < 127; tick++)
            game.RunOneStep();
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.Equal(4, Assert.Single(game.Fuseballs).OriginalSlotId);
        int[] cargo = ledger.GetCargoIdsForOriginal(Assert.Single(game.Tankers).OriginalSlotId).ToArray();
        Assert.Equal(2, cargo.Length);

        typeof(TempestGame).GetMethod("KillPlayer", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Empty(game.Fuseballs);
        Assert.Equal(27, ledger.OriginalCount);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(4));
        Assert.Contains(ledger.GetOriginalState(4),
            new[] { Level3OriginalState.Reserve, Level3OriginalState.Egg });
        Assert.Equal(27, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
    }

    [Fact]
    public void DirectFuseballShotResolvesItsOriginalIdentity()
    {
        TempestGame game = new(LevelConfiguration.Level15DirectStartCandidate, random: new Random(4));
        for (int tick = 0; tick < 127; tick++)
            game.RunOneStep();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        for (int tick = 0; tick < 18; tick++)
            fuseball.Update(game.CurrentLevel, game.Player.EdgeIndex, (min, _) => min);
        Assert.Equal(2, fuseball.W);
        typeof(Enemy).GetProperty(nameof(Enemy.Height))!.SetValue(fuseball, 350f);

        Shot shot = new() { EdgeIndex = fuseball.EdgeIndex, Height = fuseball.Height - 8 };
        bool hit = (bool)typeof(TempestGame).GetMethod(
            "CheckShotAgainstDevelopmentTargets", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, [shot])!;

        Assert.True(hit);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(4));
        game.RunOneStep();
    }

    [Fact]
    public void DirectFuseballAtRimResolvesOnlyWhenAllObligationsAreAtRim()
    {
        LevelConfiguration fixture = LevelConfiguration.Level15DirectStartCandidate with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1
        };
        TempestGame game = new(fixture, random: new Random(4));
        Egg egg = Assert.Single(game.Eggs);
        egg.BeginApproach();
        for (int tick = 0; tick < 160 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        game.CurrentLevel.HatchReadyMixedEnemies(e =>
        {
            ledger.CommitOriginalAsFuseball(e.OriginalSlotId);
            return new Fuseball(e.EdgeIndex, originalSlotId: e.OriginalSlotId);
        }, enemy =>
        {
            ((List<Fuseball>)game.Fuseballs).Add((Fuseball)enemy);
            ((List<object>)typeof(TempestGame).GetField(
                "developmentCollisionOrder", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(game)!).Add(enemy);
        });
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        typeof(Enemy).GetProperty(nameof(Enemy.Height))!.SetValue(fuseball, (float)Level.Depth);

        bool completed = (bool)typeof(TempestGame).GetMethod(
            "TryCompleteAllRim", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null)!;

        Assert.True(completed);
        Assert.Equal(Level3OriginalState.RimCleared, ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Empty(game.Fuseballs);
        Assert.Empty(ledger.UnresolvedOriginalIds);
    }

    [Theory]
    [InlineData(TankerCargoMovementProgram.Repeating24)]
    [InlineData(TankerCargoMovementProgram.Straight07)]
    public void Level11ParameterCargoProgramsRemainAvailableThroughTheGenericFactory(
        TankerCargoMovementProgram program)
    {
        Level level = new(LevelConfiguration.Level9Production, randomInclusive: (min, _) => min);
        Tanker tanker = new(0, 0);
        TankerCargoSpawn spawn = tanker.ReleaseFlipperCargo(
            level, 1, TankerReleaseKind.PlayerShotDestruction).Cargo[0];
        Flipper cargo = Flipper.CreateTankerCargo(level, spawn, 17,
            tankerCargoMovementProgram: program);
        for (int tick = 0; tick < 50; tick++)
            cargo.Update(level, 7, (min, _) => min);
        Assert.Equal(17, cargo.CargoUnitId);
        Assert.Equal(program, cargo.CargoMovementProgram);
        Assert.InRange(cargo.EdgeIndex, 0, 14);
    }

    [Fact]
    public void LevelFifteenCandidatePreservesTheManuallyIdentifiedRowTenTube()
    {
        LevelConfiguration configuration = LevelConfiguration.Level15DirectStartCandidate;
        LevelDefinition definition = configuration.Definition;
        RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(15);

        Assert.Equal(14, LevelLoader.Level15RomEvidence.LookupIndex);
        Assert.Equal(10, LevelLoader.Level15RomEvidence.ShapeRemap);
        Assert.Equal(15, configuration.DisplayedLevel);
        Assert.Equal(14, configuration.BoardIndex);
        Assert.Equal(10, metadata.TubeTableIndex);
        Assert.Equal("Level 15 - Rev 2 open shape 10", definition.Comment);
        Assert.False(definition.Continuous);
        Assert.Equal(16, definition.VertexCount);
        Assert.Equal(15, definition.PlayableLaneCount);
        Assert.Equal(0x10, LevelLoader.Level15RomEvidence.Scale);
        Assert.Equal(0x90, LevelLoader.Level15RomEvidence.Y3d);
        Assert.Equal((short)0x0040, LevelLoader.Level15RomEvidence.PresentationMetadata.ScreenYOffsetSigned);
        Assert.Equal(0xFF, LevelLoader.Level15RomEvidence.Open);
        Assert.Equal((0x50, 0x04, 0x0250),
            (LevelLoader.Level15RomEvidence.FlipperScaleLow,
             LevelLoader.Level15RomEvidence.FlipperScaleHigh,
             LevelLoader.Level15RomEvidence.FlipperScale));
        Assert.Equal(27, configuration.EnemySupply);
        Assert.Equal(15, new Level(configuration).PlayableLaneCount);
        AtariTubeRecord record = AtariTubeRecordCatalog.Level15Record10;
        Assert.Equal(10, record.RecordIndex);
        Assert.Equal(LevelLoader.Level15RomEvidence.X, record.RawXPoints);
        Assert.Equal(LevelLoader.Level15RomEvidence.Y, record.RawYPoints);
    }

    [Fact]
    public void LevelFifteenCandidatePreservesOrderedXyAndAngleBytes()
    {
        byte[] x =
        [
            0x10, 0x24, 0x30, 0x36, 0x3E, 0x49, 0x5A, 0x75,
            0x94, 0xA4, 0xAC, 0xBA, 0xDA, 0xE2, 0xEA, 0xF0
        ];
        byte[] y =
        [
            0xC0, 0xA6, 0x8A, 0x6A, 0x4A, 0x2F, 0x14, 0x24,
            0x20, 0x39, 0x59, 0x75, 0x72, 0x90, 0xB0, 0xD0
        ];
        byte[] angles =
        [
            0x0E, 0x0D, 0x0C, 0x0D, 0x0D, 0x0D, 0x01, 0x0F,
            0x02, 0x03, 0x03, 0x00, 0x03, 0x03, 0x03, 0x00
        ];

        Assert.Equal(x, LevelLoader.Level15RomEvidence.X);
        Assert.Equal(y, LevelLoader.Level15RomEvidence.Y);
        Assert.Equal(angles, LevelLoader.Level15RomEvidence.Angles);
    }

    [Fact]
    public void LevelElevenParameterEvidenceIsNotAttachedToTheLevelFifteenTube()
    {
        LevelConfiguration configuration = LevelConfiguration.Level15DirectStartCandidate;

        Assert.Equal(27, Level11WaveSelection.OrdinaryEnemySupply);
        Assert.Equal(30, Level11WaveSelection.HardDifficultyEnemySupply);
        Assert.Equal(7, Level11WaveSelection.MaximumPhysicalEnemySlots);
        Assert.Equal((0, 5), (Level11WaveSelection.MinimumFlippers,
            Level11WaveSelection.MaximumFlippers));
        Assert.Equal((1, 2), (Level11WaveSelection.MinimumTankers,
            Level11WaveSelection.MaximumTankers));
        Assert.Equal((2, 3), (Level11WaveSelection.MinimumSpikers,
            Level11WaveSelection.MaximumSpikers));
        Assert.Equal((1, 1), (Level11WaveSelection.MinimumFuseballs,
            Level11WaveSelection.MaximumFuseballs));
        Assert.Equal((0, 0), (Level11WaveSelection.MinimumPulsars,
            Level11WaveSelection.MaximumPulsars));
        Assert.Equal(0x24, Level11WaveSelection.FlipperMovementProgram);
        Assert.Equal(0x07, Level11WaveSelection.StraightTankerCargoProgram);
        Assert.Equal(0x20, Level11WaveSelection.DeepTankerCargoRomAlongThreshold);
        Assert.Equal(0xB0, Level11WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal(15, configuration.DisplayedLevel);
        Assert.Equal(11, configuration.EffectiveGameplayParameterLevel);
        Assert.False(configuration.UsesLevel11RomWavePolicy);
        Assert.True(configuration.UsesLevel11ParameterDevelopmentBaseline);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
    }

    [Fact]
    public void LevelElevenGeometryEvidenceIsDistinctAndProductionMapped()
    {
        byte[] expectedX =
        [
            0x10, 0x1E, 0x2D, 0x3C, 0x4B, 0x5A, 0x69, 0x78,
            0x87, 0x96, 0xA5, 0xB4, 0xC3, 0xD2, 0xE1, 0xF0
        ];
        byte[] expectedY =
        [
            0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40,
            0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40
        ];
        byte[] expectedAngles = new byte[16];

        Assert.Equal(new[] { 8, 12, 14, 15, 10, 11 },
            RomTubeMappingCatalog.DisplayedLevels1To16.Skip(10));
        for (int displayedLevel = 11; displayedLevel <= 16; displayedLevel++)
            Assert.Equal(RomTubeMappingCatalog.DisplayedLevels1To16[displayedLevel - 1],
                RomTubeMappingCatalog.ShapeRowForDisplayedLevel(displayedLevel));
        Assert.Equal(10, LevelLoader.Level11RomEvidence.LookupIndex);
        Assert.Equal(8, LevelLoader.Level11RomEvidence.ShapeRemap);
        Assert.Equal(expectedX, LevelLoader.Level11RomEvidence.X);
        Assert.Equal(expectedY, LevelLoader.Level11RomEvidence.Y);
        Assert.Equal(expectedAngles, LevelLoader.Level11RomEvidence.Angles);
        Assert.Equal(((byte)0x0A, (byte)0xA0, (short)0x0100, (byte)0xFF),
            (LevelLoader.Level11RomEvidence.Scale,
             LevelLoader.Level11RomEvidence.Y3d,
             LevelLoader.Level11RomEvidence.PresentationMetadata.ScreenYOffsetSigned,
             LevelLoader.Level11RomEvidence.Open));
        Assert.Equal(((byte)0x50, (byte)0x05, (ushort)0x02D0),
            (LevelLoader.Level11RomEvidence.FlipperScaleLow,
             LevelLoader.Level11RomEvidence.FlipperScaleHigh,
             LevelLoader.Level11RomEvidence.FlipperScale));
        Assert.Equal(15, LevelConfiguration.Level11Production.PlayableLaneCount);
        Assert.Equal(16, LevelConfiguration.Level11Production.VertexCount);
        Assert.NotSame(LevelConfiguration.Level11Production.Definition,
            LevelConfiguration.Level15DirectStartCandidate.Definition);
        Assert.NotEqual(LevelConfiguration.Level11Production.Definition.Points,
            LevelConfiguration.Level15DirectStartCandidate.Definition.Points);
        Assert.Equal(8, AtariTubeRecordCatalog.Level11Record08.RecordIndex);
        Assert.Equal(expectedX,
            AtariTubeRecordCatalog.Level11Record08.RawXPoints);
        Assert.Equal(expectedY,
            AtariTubeRecordCatalog.Level11Record08.RawYPoints);
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetProductionLevel(11));
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetDirectStartLevel(11));
        Assert.Same(LevelConfiguration.Level12Production,
            LevelProgression.GetProductionLevel(12));
        Assert.Null(LevelProgression.GetProductionLevel(14));
        Assert.Null(LevelProgression.GetProductionLevel(15));
        Assert.Null(LevelProgression.GetProductionLevel(16));
        Assert.Same(LevelConfiguration.Level12Production, LevelProgression.GetLastProductionLevel());
    }
}
