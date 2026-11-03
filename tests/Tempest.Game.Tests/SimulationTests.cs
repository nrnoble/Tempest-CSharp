using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Tempest.Game.Rendering;
using Raylib_cs;
using System.Numerics;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class SimulationTests
{
    // --- Incitti rimit: end-of-tick latch, confined to the direct development waves ---

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HurryUpIsFalseWhileReserveEggsRemain(bool pulsar)
    {
        TempestGame game = pulsar ? HatchDevelopmentPulsar() : HatchDevelopmentFuseball();
        Assert.True(game.CurrentLevel.ReserveEggs > 0);
        while (game.Eggs.Count > 0)
            ResolveRemainingHurryUpEgg(game);
        Assert.Empty(game.Eggs);
        Assert.False(game.HurryUp);
        game.RunOneStep();
        Assert.False(game.HurryUp);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HurryUpIsFalseWhileAnActiveEggRemainsWithoutReserve(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Single(game.Eggs);
        Assert.Single(HurryUpEnemies(game));
        Assert.False(game.HurryUp);
        game.RunOneStep();
        Assert.False(game.HurryUp);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HurryUpDoesNotActivateWithoutActiveEnemies(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar);
        ResolveRemainingHurryUpEgg(game);
        Assert.Empty(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(HurryUpEnemies(game));
        game.RunOneStep();
        Assert.False(game.HurryUp);
        Assert.Equal(LevelState.ExitDelay, game.State);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AllRimCompletionPrecedesNewHurryUpWhenAllActiveEnemiesReachTheRim(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Enemy active = Assert.Single(HurryUpEnemies(game));
        AdvanceHurryUpEnemyToRim(active, game.CurrentLevel);
        ResolveRemainingHurryUpEgg(game);
        game.RunOneStep();
        Assert.Equal(Level.Depth, active.Height);
        Assert.False(game.HurryUp);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(HurryUpEnemies(game));
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.CurrentLevel.RemainingEnemies);
        if (!pulsar)
            Assert.Empty(game.FuseballScorePopups);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void AllRimCompletionClearsMultipleFlippersWithoutScoringOrDestroyingThem()
    {
        TempestGame game = new();
        for (int unit = 0; unit < Level.EnemyTotal - 2; unit++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(2, game.Flippers.Count);
        Assert.Equal(8, game.CurrentLevel.EnemiesDestroyed);
        game.Player.Move(3 - game.Player.EdgeIndex, game.CurrentLevel.Definition.Points.Count);

        foreach (Flipper flipper in game.Flippers)
        {
            for (int tick = 0; tick < 300 && flipper.Height < Level.Depth; tick++)
                flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
            Assert.Equal(Level.Depth, flipper.Height);
        }

        game.Score.Add(17);
        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Flippers);
        Assert.Equal(8, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(2, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(17, game.Score.Score);

        game.RunOneStep();
        Assert.Equal(8, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(2, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(17, game.Score.Score);
    }

    [Fact]
    public void ReserveEggsBlockAllRimCompletion()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        for (int tick = 0; tick < 300 && flipper.Height < Level.Depth; tick++)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.True(game.CurrentLevel.ReserveEggs > 0);

        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Single(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HurryUpActivatesOnTheLastEggHatchTickAfterItsFirstEnemyUpdate(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar);
        Egg last = Assert.Single(game.Eggs);
        game.Score.Add(17);
        last.BeginApproach();
        Step(game, 39);
        Assert.Equal(EggState.Approaching, last.State);
        Assert.Equal(-2, last.Height);
        Assert.Equal(7, last.UpdateCounter);
        Assert.False(game.HurryUp);
        Assert.Empty(HurryUpEnemies(game));
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);

        game.RunOneStep(); // Approach update 40 removes the last Egg and updates its hatchling.
        Assert.Equal(EggState.ReadyToHatch, last.State);
        Assert.Empty(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Enemy active = Assert.Single(HurryUpEnemies(game));
        Assert.True(game.HurryUp);
        Assert.Equal(pulsar ? 1f : 0f, active.Height);
        if (active is Fuseball fuseball)
        {
            Assert.Equal(1, fuseball.StateTicks);
            Assert.Equal(0, fuseball.W);
        }
        else
            Assert.Equal(1, Assert.IsType<Pulsar>(active).TimeToFlip);
        Assert.Equal(17, game.Score.Score);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.IsExiting);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpFuseballUsesOrdinaryClimbOnTriggerTickThenDoubleClimbOnNextTick()
    {
        TempestGame game = PrepareHurryUpEggs(pulsar: false, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Fuseball active = Assert.Single(game.Fuseballs);
        AdvanceFuseball(active, game.CurrentLevel, 71);
        Assert.Equal(FuseballState.Climbing, active.State);
        Assert.Equal(0, active.Height);
        Assert.False(game.HurryUp);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);

        game.RunOneStep();
        Assert.Equal(3.3f, active.Height);
        Assert.True(game.HurryUp);
        Assert.Equal(1, game.Fuseballs[1].StateTicks);
        game.RunOneStep();
        Assert.Equal(3.3f + 3.3f + 3.3f, active.Height);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpPulsarUsesOrdinaryTopDecisionOnTriggerTickNotTheNewLatch()
    {
        TempestGame game = PrepareHurryUpEggs(pulsar: true, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Pulsar active = Assert.Single(game.Pulsars);
        AdvanceHurryUpEnemyToRim(active, game.CurrentLevel);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);

        game.RunOneStep();
        Assert.True(game.HurryUp);
        Assert.Equal(PulsarState.RimPause, active.State);
        Assert.Equal(PulsarState.Descending, active.ResumeState);
        Assert.Equal(14, active.PauseTicks);
        Assert.Equal(1, game.Pulsars[1].Height);
        Step(game, 14);
        Assert.Equal(PulsarState.Descending, active.State);
        Assert.Equal(Level.Depth, active.Height);
        game.RunOneStep();
        Assert.Equal(Level.Depth - 1, active.Height);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AllRimCompletionUsesPostUpdateHeightWhenHurryUpSurvivorReachesRim(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Enemy active = Assert.Single(HurryUpEnemies(game));
        if (active is Fuseball fuseball)
            AdvanceFuseballToLastClimbBelowRim(fuseball, game.CurrentLevel);
        else
            AdvancePulsarToHeight(Assert.IsType<Pulsar>(active), game.CurrentLevel, Level.Depth - 1);
        Assert.True(active.Height < Level.Depth);
        ResolveRemainingHurryUpEgg(game);
        Assert.False(game.HurryUp);
        game.RunOneStep();
        Assert.Equal(Level.Depth, active.Height);
        Assert.False(game.HurryUp); // Pre-update height would incorrectly latch on this update.
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(HurryUpEnemies(game));
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        if (!pulsar)
            Assert.Empty(game.FuseballScorePopups);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PreviouslyLatchedHurryUpDoesNotBlockAllRimCompletion(bool pulsar)
    {
        TempestGame game = HatchFinalHurryUpEnemy(pulsar);
        Enemy active = Assert.Single(HurryUpEnemies(game));
        AdvanceHurryUpEnemyToRim(active, game.CurrentLevel);
        game.Player.Move(game.CurrentLevel.NextEdge(active.EdgeIndex) - game.Player.EdgeIndex, 16);
        game.Score.Add(17);
        game.RunOneStep();
        Assert.True(game.HurryUp);
        Assert.Equal(Level.Depth, active.Height);
        Assert.Equal(17, game.Score.Score);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Empty(HurryUpEnemies(game));
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.True(game.IsExiting);
        if (!pulsar)
            Assert.Empty(game.FuseballScorePopups);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpFuseballForcesClimbWithTheSameDescendingRollAndLateralCadence()
    {
        Level level = new();
        Fuseball ordinary = CreateHurryUpLateralFuseball(level);
        Fuseball hurried = CreateHurryUpLateralFuseball(level);
        float initialHeight = hurried.Height;
        List<(int, int)> ordinaryRolls = [];
        List<(int, int)> hurriedRolls = [];
        int Roll(List<(int, int)> rolls, int min, int max)
        {
            rolls.Add((min, max));
            return max == 2 ? 2 : min;
        }
        for (int tick = 1; tick <= 72; tick++)
        {
            ordinary.Update(level, 8, (min, max) => Roll(ordinaryRolls, min, max));
            hurried.Update(level, 8, (min, max) => Roll(hurriedRolls, min, max), hurryUp: true);
            Assert.Equal(ordinary.W, hurried.W);
            Assert.Equal(ordinary.EdgeIndex, hurried.EdgeIndex);
            Assert.Equal(ordinary.Direction, hurried.Direction);
            Assert.Equal(ordinary.StateTicks, hurried.StateTicks);
            Assert.Equal(tick / 9 % 8, hurried.W);
            if (tick < 72)
                Assert.Equal(FuseballState.Lateral, hurried.State);
        }
        Assert.Equal(ordinaryRolls, hurriedRolls);
        Assert.Equal(9, hurriedRolls.Count);
        Assert.Equal(FuseballState.Descending, ordinary.State);
        Assert.Equal(FuseballState.Climbing, hurried.State);
        Assert.Equal(initialHeight, hurried.Height);
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(3.3f)]
    [InlineData(7.25f)]
    public void HurryUpFuseballAddsTheExistingClimbTermExactlyTwice(float speed)
    {
        Level level = new();
        Fuseball fuseball = new(climbSpeed: speed);
        AdvanceFuseball(fuseball, level, 72);
        fuseball.Update(level, 8, (_, _) => 3, hurryUp: true);
        Assert.Equal(speed + speed, fuseball.Height);
        if (speed == 3.3f)
            Assert.Equal(6.6f, fuseball.Height);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.Equal(1, fuseball.StateTicks);
    }

    [Fact]
    public void HurryUpFuseballClampsAtRimAndPreservesCollisionScoreAndLateralResume()
    {
        Level level = new();
        Fuseball fuseball = new(3);
        AdvanceFuseballToLastClimbBelowRim(fuseball, level);
        fuseball.Update(level, 8, (_, _) => 3, hurryUp: true);
        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.False(fuseball.HitsPlayer(new Player()));
        fuseball.Update(level, 8, (_, _) => throw new InvalidOperationException(), hurryUp: true);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.Equal(0, fuseball.StateTicks);
        for (int tick = 0; tick < 18; tick++)
            fuseball.Update(level, 8, (_, _) => 0, hurryUp: true);
        Assert.Equal(2, fuseball.W);
        Player player = new();
        player.Move(fuseball.EdgeIndex - player.EdgeIndex, 16);
        Assert.True(fuseball.HitsPlayer(player));
        Assert.True(fuseball.IsHitBy(new Shot { EdgeIndex = fuseball.EdgeIndex, Height = 399 }));
        Assert.False(fuseball.IsHitBy(new Shot { EdgeIndex = fuseball.EdgeIndex, Height = 384 }));
        Assert.Equal(250, fuseball.KillScore);
    }

    [Fact]
    public void HurryUpFuseballDoesNotChangeExistingDescentOrDurationDecision()
    {
        Level level = new();
        Fuseball ordinary = CreateHurryUpLateralFuseball(level);
        Fuseball hurried = CreateHurryUpLateralFuseball(level);
        float initialHeight = hurried.Height;
        foreach (Fuseball fuseball in new[] { ordinary, hurried })
            AdvanceFuseball(fuseball, level, 72, (_, max) => max == 2 ? 2 : 0);
        Assert.Equal(FuseballState.Descending, hurried.State);
        for (int tick = 1; tick <= 26; tick++)
        {
            ordinary.Update(level, 8, (_, _) => 1);
            hurried.Update(level, 8, (_, _) => 1, hurryUp: true);
            Assert.Equal(ordinary.Height, hurried.Height);
            Assert.Equal(ordinary.StateTicks, hurried.StateTicks);
            Assert.Equal(ordinary.State, hurried.State);
            if (tick == 1)
                Assert.Equal(initialHeight - 3.3f, hurried.Height);
        }
        Assert.Equal(FuseballState.Lateral, hurried.State);
    }

    [Fact]
    public void HurryUpPulsarForcesTopFlipWithTheSameZeroRollAndPreservesPauseAndTransfer()
    {
        Level level = new();
        Pulsar ordinary = new(3, Level.Depth);
        Pulsar hurried = new(3, Level.Depth);
        int ordinaryRolls = 0;
        int hurriedRolls = 0;
        ordinary.Update(level, 8, (_, _) => { ordinaryRolls++; return 0; });
        hurried.Update(level, 8, (_, _) => { hurriedRolls++; return 0; }, hurryUp: true);
        Assert.Equal(ordinaryRolls, hurriedRolls);
        Assert.Equal(1, hurriedRolls);
        Assert.Equal(PulsarState.RimPause, ordinary.State);
        Assert.Equal(PulsarState.Descending, ordinary.ResumeState);
        Assert.Equal(PulsarState.LanePause, hurried.State);
        Assert.Equal(PulsarState.Flipping, hurried.ResumeState);
        Assert.Equal(18, hurried.PauseTicks);
        for (int tick = 0; tick < 17; tick++)
            hurried.Update(level, 8, (_, _) => throw new InvalidOperationException(), hurryUp: true);
        Assert.Equal(1, hurried.PauseTicks);
        Assert.Equal(3, hurried.EdgeIndex);
        Assert.Equal(Level.Depth, hurried.Height);
        hurried.Update(level, 8, (_, _) => throw new InvalidOperationException(), hurryUp: true);
        Assert.Equal(4, hurried.EdgeIndex);
        Assert.Equal(PulsarState.Flipping, hurried.State);
        Assert.Equal(0, hurried.TimeToFlip);
        Assert.Equal(0, hurried.TurnAngle);
        int flipTicks = (int)MathF.Ceiling(hurried.TotalTurnAngle / Pulsar.PulseFlipSpeed);
        for (int tick = 0; tick < flipTicks; tick++)
            hurried.Update(level, 8, (_, _) => throw new InvalidOperationException(), hurryUp: true);
        Assert.Equal(Level.Depth, hurried.Height);
        Assert.Equal(PulsarState.RimPause, hurried.State);
        Assert.Equal(PulsarState.Climbing, hurried.ResumeState);
        Assert.Equal(14, hurried.PauseTicks);
    }

    [Fact]
    public void HurryUpPulsarPreservesOneUnitClimbDescentAndMidTubeDecisions()
    {
        Level level = new();
        Pulsar ordinary = new(3, 100);
        Pulsar hurried = new(3, 100);
        for (int tick = 1; tick <= 51; tick++)
        {
            ordinary.Update(level, 8, (_, _) => 0);
            hurried.Update(level, 8, (_, _) => 0, hurryUp: true);
            Assert.Equal(100 + tick, hurried.Height);
            Assert.Equal(ordinary.Height, hurried.Height);
            Assert.Equal(ordinary.State, hurried.State);
            Assert.Equal(ordinary.PauseTicks, hurried.PauseTicks);
        }
        Assert.Equal(PulsarState.RimPause, hurried.State);
        Pulsar descending = new(3, Level.Depth);
        descending.Update(level, 8, (_, _) => 0);
        AdvancePulsar(descending, level, 14, 8, (_, _) => 0);
        descending.Update(level, 8, (_, _) => 0, hurryUp: true);
        Assert.Equal(Level.Depth - 1, descending.Height);
        Assert.Equal(PulsarState.Descending, descending.State);

        Pulsar flipping = new(3, 100);
        AdvancePulsar(flipping, level, 51, 8, (_, _) => 1);
        AdvancePulsar(flipping, level, 18, 8, (_, _) => 0);
        Assert.Equal(PulsarState.Flipping, flipping.State);
        flipping.Update(level, 8, (_, _) => 0, hurryUp: true);
        Assert.Equal(152, flipping.Height);
    }

    [Fact]
    public void PlayerDeathPreventsSameTickHurryUpAndFreezesExistingState()
    {
        TempestGame game = PrepareHurryUpEggs(pulsar: true, survivors: 2);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Pulsar rim = Assert.Single(game.Pulsars);
        AdvanceHurryUpEnemyToRim(rim, game.CurrentLevel);
        game.Player.Move(rim.EdgeIndex - game.Player.EdgeIndex, 16);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        Assert.False(game.HurryUp);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.False(game.HurryUp);
        Assert.Empty(game.Eggs);
        Assert.Equal(1, game.Pulsars[1].Height);
        game.RunOneStep();
        Assert.False(game.HurryUp);
        Assert.Equal(1, game.Pulsars[1].Height);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpPulsarKeepsTheSharedOscillatorAndExistingRenderSubmissionsForAFullCycle()
    {
        TempestGame game = HatchFinalHurryUpEnemy(pulsar: true);
        Pulsar pulsar = Assert.Single(game.Pulsars);
        game.Player.Move(0 - game.Player.EdgeIndex, 16);
        VectorRenderer renderer = new(game);
        for (int tick = 0; tick < 181; tick++)
        {
            int phase = (tick + 1) % 181;
            bool pulsing = phase > 70 && phase < 110;
            float amplitude = MathF.Abs(phase % 90 - 45) / 4f;
            Assert.True(game.HurryUp);
            Assert.Equal(LevelState.Playing, game.State);
            Assert.Equal(phase, game.PulsePhase);
            Assert.Equal(pulsing, game.Pulsing);
            Assert.Equal(amplitude < 2 ? 0 : MathF.Min(amplitude, 12), game.PulseZh);
            List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
            renderer.DrawPulsars((start, end, color) => lines.Add((start, end, color)));
            Assert.Equal(6, lines.Count);
            Assert.All(lines, line => { AssertFinite(line.Start); AssertFinite(line.End); });
            Assert.All(lines, line => Assert.Equal(pulsing ? Color.White : new Color(0, 255, 255, 255),
                line.Color));
            float depth = Level.Position + Level.Depth - pulsar.Height;
            Vector2 firstPoint = game.CurrentLevel.Definition.Points[pulsar.EdgeIndex];
            Vector2 nextPoint = game.CurrentLevel.Definition.Points[
                game.CurrentLevel.NextEdge(pulsar.EdgeIndex)];
            Vector2 start = Projection.Project(firstPoint.X, firstPoint.Y, depth,
                game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
            Vector2 end = Projection.Project(nextPoint.X, nextPoint.Y, depth,
                game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
            Vector2 delta = start - end;
            Vector2 normal = new(-delta.Y / delta.Length(), delta.X / delta.Length());
            Assert.Equal(start, lines[0].Start);
            Assert.Equal(Vector2.Lerp(start, end, .2f) - normal * game.PulseZh, lines[0].End);
            bool lanePulsing = Pulsar.IsLanePulsing(pulsar.Height, pulsing);
            AssertPulsarTubeGeometry(game, lanePulsing ? [pulsar.EdgeIndex] : []);
            int overlays = 0;
            renderer.DrawPulsingLaneBoundaries((_, _, color) =>
            {
                overlays++;
                Assert.Equal(phase % 8 > 3 ? new Color(0, 255, 255, 255) : Color.White, color);
            });
            Assert.Equal(lanePulsing ? 2 : 0, overlays);
            game.RunOneStep();
        }
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpPulsingLaneCollisionStillStartsOnPhaseSeventyOne()
    {
        TempestGame game = HatchFinalHurryUpEnemy(pulsar: true);
        Pulsar pulsar = Assert.Single(game.Pulsars);
        game.Player.Move(pulsar.EdgeIndex - game.Player.EdgeIndex, 16);
        Step(game, 69);
        Assert.Equal(70, game.PulsePhase);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(game.HurryUp);
        game.RunOneStep();
        Assert.Equal(71, game.PulsePhase);
        Assert.True(game.Pulsing);
        Assert.True(pulsar.Height < Level.Depth - 8);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.Score.Score);
        AssertHurryUpAccounting(game);
    }

    [Fact]
    public void HurryUpFuseballKeepsAllFourAnimationFramesAndFiveColorGeometry()
    {
        TempestGame game = HatchFinalHurryUpEnemy(pulsar: false);
        VectorRenderer renderer = new(game);
        Vector2[] firstArmPoints = [new(7, -5), new(6, -8), new(5, -8), new(6, -6)];
        HashSet<int> frames = [];
        for (int tick = 0; tick < 16; tick++)
        {
            Assert.True(game.HurryUp);
            Assert.Equal((tick + 1) % 16 / 4, game.FuseballAnimationFrame);
            frames.Add(game.FuseballAnimationFrame);
            List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
            renderer.DrawFuseballs((start, end, color) => lines.Add((start, end, color)));
            Assert.Equal(25, lines.Count);
            Assert.Equal(5, lines.Select(line => line.Color).Distinct().Count());
            Assert.All(lines, line => { AssertFinite(line.Start); AssertFinite(line.End); });
            Fuseball fuseball = Assert.Single(game.Fuseballs);
            float scale = (8 / ((Level.Position + Level.Depth - fuseball.Height + Projection.ZOffset) /
                Projection.K) + .25f) / 6;
            Vector2 expectedTip = firstArmPoints[game.FuseballAnimationFrame] * scale;
            Assert.InRange(Vector2.Distance(expectedTip, lines[0].End - lines[0].Start), 0, .0001f);
            game.RunOneStep();
        }
        Assert.Equal(4, frames.Count);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false, 10)]
    [InlineData(true, 10)]
    [InlineData(false, 2)]
    [InlineData(true, 2)]
    public void HurryUpFreezesThroughDeathAndPersistsThroughFullSameLevelReconstruction(
        bool pulsar, int survivors)
    {
        TempestGame game = HatchHurryUpWave(pulsar, survivors: survivors);
        Enemy old = HurryUpEnemies(game).First();
        game.Score.Add(17);
        BeginHurryUpDeath(game, old);
        int phase = game.PulsePhase;
        float zh = game.PulseZh;
        bool pulsing = game.Pulsing;
        int frame = game.FuseballAnimationFrame;
        float[] heights = HurryUpEnemies(game).Select(enemy => enemy.Height).ToArray();
        Step(game, 59);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(1, game.DeathTicks);
        Assert.True(game.HurryUp);
        Assert.Equal(phase, game.PulsePhase);
        Assert.Equal(zh, game.PulseZh);
        Assert.Equal(pulsing, game.Pulsing);
        Assert.Equal(frame, game.FuseballAnimationFrame);
        Assert.Equal(heights, HurryUpEnemies(game).Select(enemy => enemy.Height).ToArray());
        Assert.Empty(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        AssertHurryUpAccounting(game);

        game.RunOneStep();
        Assert.True(game.HurryUp);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(17, game.Score.Score);
        Assert.Equal(10 - survivors, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(HurryUpEnemies(game));
        Assert.Equal(Math.Min(4, survivors), game.Eggs.Count);
        Assert.Equal(Math.Max(0, survivors - 4), game.CurrentLevel.ReserveEggs);
        Assert.Equal(phase, game.PulsePhase);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Enemy fresh = Assert.Single(HurryUpEnemies(game));
        Assert.NotSame(old, fresh);
        Assert.True(game.HurryUp);
        Assert.NotEmpty(game.Eggs);
        Assert.Equal(pulsar ? 1f : 0f, fresh.Height);
        if (fresh is Fuseball fuseball)
        {
            Assert.Equal(1, fuseball.StateTicks);
            Step(game, 71);
            Assert.Equal(FuseballState.Climbing, fuseball.State);
            Assert.Equal(0, fuseball.Height);
            game.RunOneStep(); // First climbing update after fresh-Egg re-entry.
            Assert.Equal(6.6f, fuseball.Height);
        }
        else
        {
            AdvanceHurryUpEnemyToRim(fresh, game.CurrentLevel);
            game.RunOneStep(); // First eligible top-end decision after re-entry.
            Pulsar restored = Assert.IsType<Pulsar>(fresh);
            Assert.Equal(PulsarState.LanePause, restored.State);
            Assert.Equal(PulsarState.Flipping, restored.ResumeState);
        }
        if (survivors == 10)
        {
            Assert.True(game.CurrentLevel.ReserveEggs > 0);
            Assert.NotEmpty(game.Eggs);
        }
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HurryUpFullGameRestartClearsLatchAndRestoresInitialSupply(bool pulsar)
    {
        TempestGame game = HatchHurryUpWave(pulsar, startingLives: 1);
        BeginHurryUpDeath(game, HurryUpEnemies(game).First());
        Step(game, 60);
        Assert.True(game.GameOver);
        Assert.True(game.HurryUp);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.HurryUp);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false, 1000)]
    [InlineData(true, 200)]
    public void HurryUpStillRequiresFinalKillAndReplayClearsLatch(bool pulsar, int killScore)
    {
        TempestGame game = HatchFinalHurryUpEnemy(pulsar);
        Enemy active = Assert.Single(HurryUpEnemies(game));
        if (active is Fuseball fuseball)
        {
            AdvanceFuseball(fuseball, game.CurrentLevel, 71);
            AdvanceFuseball(fuseball, game.CurrentLevel, 26);
            AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        }
        else
            AdvancePulsarToHeight(Assert.IsType<Pulsar>(active), game.CurrentLevel, 100);
        game.Score.Add(17);
        game.Player.Move(active.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.True(game.HurryUp);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(17, game.Score.Score);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Single(game.Shots).Height = active.Height + 5;
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.True(game.HurryUp);
        Assert.Equal(17 + killScore, game.Score.Score);
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(HurryUpEnemies(game));
        AssertHurryUpAccounting(game);
        FinishTerminalExit(game);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.HurryUp);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(17 + killScore, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertHurryUpAccounting(game);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void HurryUpIsNotEnabledForNormalSpikerOrTankerScenarios(bool spiker, bool tanker)
    {
        TempestGame game = new(spikerDevelopmentWave: spiker, tankerTest: tanker,
            random: new MinimumRandom());
        while (game.CurrentLevel.ReserveEggs > 0 || game.Eggs.Count > 0)
        {
            if (game.Eggs.Count == 0)
                for (int tick = 0; tick < 50; tick++)
                    game.CurrentLevel.UpdateEggRelease();
            foreach (Egg egg in game.Eggs)
                MakeReady(egg, game.CurrentLevel);
            game.RunOneStep();
        }
        Assert.NotEmpty(game.Flippers);
        Assert.Contains(game.Flippers, enemy => enemy.Height < Level.Depth);
        Assert.False(game.HurryUp);
        Assert.Equal(LevelState.Playing, game.State);
        if (tanker)
            AssertTankerAccounting(game);
        else
            AssertSpikerAccounting(game);
    }

    private static TempestGame PrepareHurryUpEggs(bool pulsar, int survivors = 1)
    {
        TempestGame game = new(fuseballTest: !pulsar, pulsarTest: pulsar, random: new MinimumRandom());
        for (int unit = survivors; unit < Level.EnemyTotal; unit++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(survivors, game.Eggs.Count);
        return game;
    }

    private static TempestGame HatchFinalHurryUpEnemy(bool pulsar)
    {
        TempestGame game = PrepareHurryUpEggs(pulsar);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Assert.True(game.HurryUp);
        return game;
    }

    private static TempestGame HatchHurryUpWave(bool pulsar, int startingLives = 3, int survivors = 10)
    {
        TempestGame game = new(fuseballTest: !pulsar, pulsarTest: pulsar,
            startingLives: startingLives, random: new MinimumRandom());
        for (int unit = survivors; unit < Level.EnemyTotal; unit++)
            game.CurrentLevel.RecordEnemyDestroyed();
        if (survivors < Level.EnemyTotal)
            game.CurrentLevel.ResetAfterDeath();
        while (game.CurrentLevel.ReserveEggs > 0 || game.Eggs.Count > 0)
        {
            if (game.Eggs.Count == 0)
                for (int tick = 0; tick < 50; tick++)
                    game.CurrentLevel.UpdateEggRelease();
            foreach (Egg egg in game.Eggs)
                MakeReady(egg, game.CurrentLevel);
            game.RunOneStep();
        }
        Assert.True(game.HurryUp);
        Assert.Equal(survivors, HurryUpEnemies(game).Count());
        AssertHurryUpAccounting(game);
        return game;
    }

    private static IEnumerable<Enemy> HurryUpEnemies(TempestGame game) =>
        game.Fuseballs.Concat<Enemy>(game.Pulsars);

    private static void AssertHurryUpAccounting(TempestGame game) =>
        Assert.Equal(Level.EnemyTotal, game.CurrentLevel.EnemiesDestroyed +
            game.CurrentLevel.EnemiesRimCleared + game.CurrentLevel.ReserveEggs +
            game.Eggs.Count + HurryUpEnemies(game).Count());

    private static void ResolveRemainingHurryUpEgg(TempestGame game)
    {
        MakeReady(game.Eggs[0], game.CurrentLevel);
        Assert.NotNull(game.CurrentLevel.HatchReadyEnemy(lane => new Pulsar(lane)));
        game.CurrentLevel.RecordEnemyDestroyed();
    }

    private static void BeginHurryUpDeath(TempestGame game, Enemy enemy)
    {
        AdvanceHurryUpEnemyToRim(enemy, game.CurrentLevel);
        if (enemy is Fuseball fuseball)
            AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        game.Player.Move(enemy.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.True(game.HurryUp);
    }

    private static Fuseball CreateHurryUpLateralFuseball(Level level)
    {
        Fuseball fuseball = new(3);
        AdvanceFuseball(fuseball, level, 72);
        AdvanceFuseball(fuseball, level, 26);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.True(fuseball.Height > 0);
        return fuseball;
    }

    private static void AdvanceFuseballToLastClimbBelowRim(Fuseball fuseball, Level level)
    {
        for (int tick = 0; tick < 2000; tick++)
        {
            if (fuseball.State == FuseballState.Climbing && fuseball.Height < Level.Depth &&
                fuseball.Height + fuseball.ClimbSpeed >= Level.Depth)
                return;
            fuseball.Update(level, 8, (min, max) => max == 3 ? 3 : min);
        }
        Assert.Fail("Fuseball did not reach its last climbing update below the rim.");
    }

    private static void AdvanceHurryUpEnemyToRim(Enemy enemy, Level level)
    {
        if (enemy is Pulsar pulsar)
        {
            AdvancePulsarToHeight(pulsar, level, Level.Depth);
            return;
        }
        Fuseball fuseball = Assert.IsType<Fuseball>(enemy);
        for (int tick = 0; tick < 2000 &&
            !(fuseball.Height == Level.Depth && fuseball.State == FuseballState.Lateral); tick++)
            fuseball.Update(level, 8, (min, max) => max == 3 ? 3 : min);
        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
    }

    private static void AdvancePulsarToHeight(Pulsar pulsar, Level level, float height)
    {
        for (int tick = 0; tick < 1000 && pulsar.Height < height; tick++)
            pulsar.Update(level, 8, (_, _) => 0);
        Assert.Equal(height, pulsar.Height);
    }

    // --- Direct Pulsars: source behavior in an isolated ten-unit development wave ---

    [Fact]
    public void PulsarScenarioIsExclusiveComposesWithLivesAndHatchesHeightZeroUnits()
    {
        string[] args = ["--pulsar-test", "--lives", "10", "--enemy-bullets-test"];
        Assert.True(Tempest.Game.Debug.DebugController.UsePulsarTest(args));
        Assert.False(Tempest.Game.Debug.DebugController.UsePulsarTest([]));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(args));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(args));
        foreach (string other in new[] { "--spiker-test", "--tanker-test", "--fuseball-test" })
            Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(["--pulsar-test", other]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, pulsarTest: true));
        Assert.Throws<ArgumentException>(() => new TempestGame(tankerTest: true, pulsarTest: true));
        Assert.Throws<ArgumentException>(() => new TempestGame(fuseballTest: true, pulsarTest: true));

        Level level = new();
        Egg egg = level.Eggs[0];
        MakeReady(egg, level);
        Pulsar directHatch = Assert.IsType<Pulsar>(level.HatchReadyEnemy(lane => new Pulsar(lane, 0)));
        Assert.Equal(egg.EdgeIndex, directHatch.EdgeIndex);
        Assert.Equal(0, directHatch.Height);
        Assert.Equal(PulsarState.Climbing, directHatch.State);
        Assert.Equal(10, level.RemainingEnemies);
        Assert.Equal(10, level.ReserveEggs + level.Eggs.Count + 1);

        TempestGame game = HatchDevelopmentPulsar(enemyBulletsTest: true);
        Pulsar active = Assert.Single(game.Pulsars);
        Assert.Equal(12, active.EdgeIndex);
        Assert.Equal(1, active.Height); // It hatches at zero, then receives the normal same-tick update.
        Assert.Equal(PulsarState.Climbing, active.State);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(Player.DefaultStartingLives, game.Player.Lives);
        AssertPulsarAccounting(game);
    }

    [Fact]
    public void PulsarMovesExactlyOneUnitPerClimbAndClampsAtTheTop()
    {
        Level level = new();
        Pulsar pulsar = new(3, 10);
        pulsar.Update(level, 8, (_, _) => throw new InvalidOperationException());
        Assert.Equal(11, pulsar.Height);
        pulsar = new Pulsar(3, Level.Depth - 1);
        pulsar.Update(level, 8, (_, _) => throw new InvalidOperationException());
        Assert.Equal(Level.Depth, pulsar.Height);
        Assert.Equal(PulsarState.Climbing, pulsar.State);
    }

    [Fact]
    public void PulsarDecisionUsesUpdateFiftyOneAndNotFifty()
    {
        Pulsar pulsar = new(3);
        int rolls = 0;
        int Roll(int minimum, int maximum)
        {
            Assert.Equal(0, minimum);
            Assert.Equal(1, maximum);
            rolls++;
            return 1;
        }

        AdvancePulsar(pulsar, new Level(), 50, playerEdge: 8, randomInclusive: Roll);
        Assert.Equal(50, pulsar.Height);
        Assert.Equal(PulsarState.Climbing, pulsar.State);
        Assert.Equal(0, rolls);
        pulsar.Update(new Level(), 8, Roll);
        Assert.Equal(51, pulsar.Height);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        Assert.Equal(PulsarState.Flipping, pulsar.ResumeState);
        Assert.Equal(Pulsar.LanePauseDuration, pulsar.PauseTicks);
        Assert.Equal(0, pulsar.TimeToFlip);
        Assert.Equal(1, rolls);
    }

    [Fact]
    public void PulsarDecisionZeroChoosesSameLanePauseOnUpdateFiftyOne()
    {
        Pulsar pulsar = new(3);
        AdvancePulsar(pulsar, new Level(), 51, 8, (_, _) => 0);
        Assert.Equal(PulsarState.RimPause, pulsar.State);
        Assert.Equal(PulsarState.Climbing, pulsar.ResumeState);
        Assert.Equal(Pulsar.RimPauseDuration, pulsar.PauseTicks);
        Assert.Equal(51, pulsar.Height);
    }

    [Fact]
    public void PulsarLanePauseMovesOneAdjacentEdgeOnlyOnItsExpirationUpdate()
    {
        Level level = new();
        Pulsar pulsar = new(0, 100);
        int rolls = 0;
        int Roll(int minimum, int maximum) => rolls++ == 0 ? 1 : 0;
        AdvancePulsar(pulsar, level, 51, 2, Roll);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        Assert.Equal(0, pulsar.EdgeIndex);
        Assert.Equal(151, pulsar.Height);
        AdvancePulsar(pulsar, level, Pulsar.LanePauseDuration - 1, 2, Roll);
        Assert.Equal(1, pulsar.PauseTicks);
        Assert.Equal(0, pulsar.EdgeIndex);
        Assert.Equal(151, pulsar.Height);
        pulsar.Update(level, 2, Roll);
        Assert.Equal(PulsarState.Flipping, pulsar.State);
        Assert.Equal(1, pulsar.EdgeIndex);
        Assert.True(pulsar.TotalTurnAngle > 0);
        Assert.Equal(0, pulsar.TimeToFlip);
    }

    [Fact]
    public void PulsarRimPauseExpiresBeforeVerticalMovementResumes()
    {
        Pulsar pulsar = new(3);
        AdvancePulsar(pulsar, new Level(), 51, 8, (_, _) => 0);
        AdvancePulsar(pulsar, new Level(), Pulsar.RimPauseDuration - 1, 8, (_, _) => 0);
        Assert.Equal(1, pulsar.PauseTicks);
        Assert.Equal(PulsarState.RimPause, pulsar.State);
        Assert.Equal(51, pulsar.Height);
        pulsar.Update(new Level(), 8, (_, _) => 0);
        Assert.Equal(PulsarState.Climbing, pulsar.State);
        Assert.Equal(51, pulsar.Height);
        pulsar.Update(new Level(), 8, (_, _) => 0);
        Assert.Equal(52, pulsar.Height);
    }

    [Fact]
    public void PulsarTopAndBottomChooseFlipOrOppositeVerticalTravel()
    {
        Level level = new();
        Pulsar topFlip = new(0, Level.Depth);
        topFlip.Update(level, 8, (_, _) => 1);
        Assert.Equal(PulsarState.LanePause, topFlip.State);
        Assert.Equal(PulsarState.Flipping, topFlip.ResumeState);

        Pulsar topDescend = new(0, Level.Depth);
        topDescend.Update(level, 8, (_, _) => 0);
        Assert.Equal(PulsarState.RimPause, topDescend.State);
        Assert.Equal(PulsarState.Descending, topDescend.ResumeState);
        AdvancePulsar(topDescend, level, Pulsar.RimPauseDuration, 8, (_, _) => 0);
        topDescend.Update(level, 8, (_, _) => 0);
        Assert.Equal(Level.Depth - 1, topDescend.Height);
    }

    [Fact]
    public void PulsarBottomReversesToClimbingAfterRimPause()
    {
        Level level = new();
        Pulsar pulsar = new(0, Level.Depth);
        int ticks = 0;
        while (!(pulsar.State == PulsarState.RimPause &&
                 pulsar.ResumeState == PulsarState.Climbing && pulsar.Height == 0) && ticks++ < 1000)
            pulsar.Update(level, 8, (_, _) => 0);

        Assert.True(ticks < 1000);
        Assert.Equal(0, pulsar.Height);
        Assert.Equal(Pulsar.RimPauseDuration, pulsar.PauseTicks);
        AdvancePulsar(pulsar, level, Pulsar.RimPauseDuration, 8, (_, _) => 0);
        pulsar.Update(level, 8, (_, _) => 0);
        Assert.Equal(1, pulsar.Height);
        Assert.Equal(PulsarState.Climbing, pulsar.State);
    }

    [Theory]
    [InlineData(0, 0, 15, 1)]
    [InlineData(15, 1, 0, -1)]
    public void PulsarClosedTubeWrapsInBothDirections(int edge, int tieRoll, int expectedEdge, int expectedDirection)
    {
        Level level = new();
        Pulsar pulsar = new(edge, Level.Depth);
        int calls = 0;
        int Roll(int _, int __) => calls++ == 0 ? 1 : tieRoll;
        pulsar.Update(level, edge, Roll);
        AdvancePulsar(pulsar, level, Pulsar.LanePauseDuration, edge, Roll);
        Assert.Equal(expectedEdge, pulsar.EdgeIndex);
        Assert.Equal(expectedDirection, pulsar.Direction);
    }

    [Fact]
    public void PulsarOpenTubeReflectsAtEndpointWithoutWrapping()
    {
        Level level = CreateTopologyLevel(continuous: false);
        Pulsar pulsar = new(0, Level.Depth);
        int calls = 0;
        int Roll(int _, int __) => calls++ == 0 ? 1 : 0;
        pulsar.Update(level, 0, Roll);
        AdvancePulsar(pulsar, level, Pulsar.LanePauseDuration, 0, Roll);
        Assert.Equal(1, pulsar.EdgeIndex);
        Assert.Equal(-1, pulsar.Direction);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void PulsarSameEdgeDirectionTieUsesBothRandomOutcomes(int roll, int expectedDirection)
    {
        Pulsar pulsar = new(4, 1);
        pulsar.Update(new Level(), 4, (_, _) => roll);
        Assert.Equal(expectedDirection, pulsar.Direction);
    }

    [Theory]
    [InlineData(0, 15, 1)]
    [InlineData(0, 1, -1)]
    [InlineData(0, 8, -1)]
    public void PulsarChoosesTheSourceShortestDirectionTowardThePlayer(
        int edge, int playerEdge, int expectedDirection)
    {
        Pulsar pulsar = new(edge, 1);
        pulsar.Update(new Level(), playerEdge, (_, _) => throw new InvalidOperationException());
        Assert.Equal(expectedDirection, pulsar.Direction);
    }

    [Fact]
    public void PulsarFlipAdvancesAngleAndHeightThenPausesWithoutEnteringStateTwo()
    {
        Level level = new();
        Pulsar pulsar = new(0, Level.Depth);
        int calls = 0;
        int Roll(int _, int __) => calls++ == 0 ? 1 : 0;
        pulsar.Update(level, 2, Roll);
        AdvancePulsar(pulsar, level, Pulsar.LanePauseDuration, 2, Roll);
        Assert.Equal(PulsarState.Flipping, pulsar.State);
        float initialHeight = pulsar.Height;
        pulsar.Update(level, 2, Roll);
        Assert.Equal(Pulsar.PulseFlipSpeed, pulsar.TurnAngle);
        Assert.Equal(initialHeight, pulsar.Height);
        pulsar.Update(level, 2, Roll);
        Assert.Equal(PulsarState.Flipping, pulsar.State);
        Assert.Equal(Pulsar.PulseFlipSpeed * 2, pulsar.TurnAngle);
        int flipTicks = (int)MathF.Ceiling(pulsar.TotalTurnAngle / Pulsar.PulseFlipSpeed);
        int actualTicks = 2;
        while (pulsar.State == PulsarState.Flipping)
        {
            pulsar.Update(level, 2, Roll);
            actualTicks++;
        }
        Assert.Equal(PulsarState.RimPause, pulsar.State);
        Assert.Equal(Pulsar.RimPauseDuration, pulsar.PauseTicks);
        Assert.Equal(PulsarState.Climbing, pulsar.ResumeState);
        Assert.Equal(flipTicks, actualTicks);
    }

    [Fact]
    public void PulsarNormalStateMachineNeverEntersSourceStateTwo()
    {
        Pulsar pulsar = new();
        Level level = new();
        for (int tick = 0; tick < 1000; tick++)
        {
            pulsar.Update(level, 8, (_, _) => 0);
            Assert.NotEqual(PulsarState.FlippingWithoutClimb, pulsar.State);
        }
    }

    [Theory]
    [InlineData(70, false)]
    [InlineData(71, true)]
    [InlineData(109, true)]
    [InlineData(110, false)]
    [InlineData(180, false)]
    [InlineData(181, false)]
    public void SharedPulsarClockUsesLiteralWindowAndResetBoundaries(int ticks, bool pulsing)
    {
        TempestGame game = new(random: new MinimumRandom());
        Step(game, ticks);
        Assert.Equal(ticks == 181 ? 0 : ticks, game.PulsePhase);
        Assert.Equal(pulsing, game.Pulsing);
        Assert.Equal(ticks == 181 ? 11.25f :
            MathF.Abs((ticks % 90) - 45) / 4f < 2 ? 0 : MathF.Abs((ticks % 90) - 45) / 4f,
            game.PulseZh);
    }

    [Fact]
    public void SharedPulsarClockRepeatsAfterExactly181UpdatesAndUsesStrictAmplitudeDeadZone()
    {
        TempestGame game = new(random: new MinimumRandom());
        Step(game, 180);
        Assert.Equal(180, game.PulsePhase);
        Assert.Equal(11.25f, game.PulseZh);
        game.RunOneStep();
        Assert.Equal(0, game.PulsePhase);
        Assert.Equal(11.25f, game.PulseZh);

        game = new TempestGame(random: new MinimumRandom());
        Step(game, 37);
        Assert.Equal(2, game.PulseZh);
        game.RunOneStep();
        Assert.Equal(0, game.PulseZh);
    }

    [Fact]
    public void PulsarsUseOneSharedPulseWindow()
    {
        TempestGame game = new(pulsarTest: true, random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        MakeReady(game.Eggs[1], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(2, game.Pulsars.Count);
        Step(game, 70);
        Assert.Equal(71, game.PulsePhase);
        Assert.True(game.Pulsing);
        Assert.All(game.Pulsars.Take(2),
            pulsar => Assert.True(Pulsar.IsLanePulsing(pulsar.Height, game.Pulsing)));
    }

    [Fact]
    public void SharedPulseClockFreezesDuringTheExistingPlayerDeathFreeze()
    {
        TempestGame game = HatchDevelopmentPulsar();
        game.Player.Move(game.Pulsars[0].EdgeIndex - game.Player.EdgeIndex, 16);
        Step(game, 70);
        Assert.Equal(71, game.PulsePhase);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Step(game, 12);
        Assert.Equal(71, game.PulsePhase);
        Assert.Equal(48, game.DeathTicks);
    }

    [Fact]
    public void PulsarGlyphSubmitsSixSourceInterpolatedSegmentsAndPulseColors()
    {
        TempestGame game = HatchDevelopmentPulsar();
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawPulsars((start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(6, lines.Count);
        Assert.All(lines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
            Assert.Equal(new Color(0, 255, 255, 255), line.Color);
        });

        Pulsar pulsar = game.Pulsars[0];
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        int next = game.CurrentLevel.NextEdge(pulsar.EdgeIndex);
        float depth = Level.Position + Level.Depth - pulsar.Height;
        Vector2 expectedStart = Projection.Project(points[pulsar.EdgeIndex].X, points[pulsar.EdgeIndex].Y,
            depth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        Vector2 expectedEnd = Projection.Project(points[next].X, points[next].Y,
            depth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        Assert.Equal(expectedStart, lines[0].Start);
        Assert.Equal(expectedEnd, lines[^1].End);
        Vector2 delta = expectedStart - expectedEnd;
        Vector2 normal = new(-delta.Y / delta.Length(), delta.X / delta.Length());
        Vector2 expectedInnerVertex = Vector2.Lerp(expectedStart, expectedEnd, .2f) -
            normal * game.PulseZh;
        Assert.Equal(expectedInnerVertex, lines[0].End);

        Step(game, 70);
        lines.Clear();
        renderer.DrawPulsars((start, end, color) => lines.Add((start, end, color)));
        Assert.All(lines, line => Assert.Equal(Color.White, line.Color));
        Vector2 unpulsedInnerVertex = lines[0].End;
        lines.Clear();
        game.RunOneStep();
        renderer.DrawPulsars((start, end, color) => lines.Add((start, end, color)));
        Assert.NotEqual(unpulsedInnerVertex, lines[0].End);
    }

    [Fact]
    public void PulsarRendererUsesItsActiveDirectedTransitionDuringTheSweep()
    {
        TempestGame game = HatchDevelopmentPulsar();
        Pulsar pulsar = Assert.Single(game.Pulsars);
        Level level = game.CurrentLevel;
        for (int tick = 0; tick < 51; tick++)
            pulsar.Update(level, playerEdge: 8, (_, _) => 1);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            pulsar.Update(level, playerEdge: 8, (_, _) =>
                throw new InvalidOperationException("A pause update must not request randomness."));
        Assert.Equal(PulsarState.Flipping, pulsar.State);

        pulsar.Update(level, playerEdge: 8, (_, _) =>
            throw new InvalidOperationException("A flip update must not request randomness."));
        DirectedLaneTransition transition = pulsar.ActiveTransition!.Value;
        float progress = pulsar.TurnAngle / pulsar.TotalTurnAngle;
        (Vector2 expectedStart, Vector2 expectedEnd) =
            level.CalculateFlipperPivotEndpoints(transition, progress);
        float depth = level.Configuration.Position + level.Configuration.Depth - pulsar.Height;
        expectedStart = Projection.Project(expectedStart.X, expectedStart.Y, depth,
            level.Definition.CenterY, level.Definition.YOffset);
        expectedEnd = Projection.Project(expectedEnd.X, expectedEnd.Y, depth,
            level.Definition.CenterY, level.Definition.YOffset);

        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        new VectorRenderer(game).DrawPulsars((start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(6, lines.Count);
        Assert.Equal(expectedStart, lines[0].Start);
        Assert.Equal(expectedEnd, lines[^1].End);
    }

    [Fact]
    public void PulsingLaneOverlayAffectsOnlyPulsarsAboveHeightSixteen()
    {
        Assert.False(Pulsar.IsLanePulsing(16, true));
        Assert.True(Pulsar.IsLanePulsing(16.001f, true));
        Assert.False(Pulsar.IsLanePulsing(17, false));

        TempestGame game = HatchDevelopmentPulsar();
        Step(game, 70);
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawPulsingLaneBoundaries((start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(2, lines.Count);
        Assert.All(lines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
            Assert.Equal(new Color(0, 255, 255, 255), line.Color);
        });
        game.RunOneStep();
        lines.Clear();
        renderer.DrawPulsingLaneBoundaries((start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(2, lines.Count);
        Assert.All(lines, line => Assert.Equal(Color.White, line.Color));
    }

    [Fact]
    public void PulsarTubeKeepsOrdinaryGeometryAndRestoresNearRimAfterPulse()
    {
        TempestGame ordinary = new();
        AssertPulsarTubeGeometry(ordinary, []);

        TempestGame game = HatchDevelopmentPulsar();
        AssertPulsarTubeGeometry(game, []);
        int lane = game.Pulsars[0].EdgeIndex;
        Step(game, 70);
        Assert.Equal(71, game.PulsePhase);
        Assert.True(game.Pulsing);
        AssertPulsarTubeGeometry(game, [lane]);
        List<(Vector2 Start, Vector2 End)> overlays = [];
        new VectorRenderer(game).DrawPulsingLaneBoundaries(
            (start, end, _) => overlays.Add((start, end)));
        Assert.Equal(2, overlays.Count);
        Assert.Contains((ProjectTubePoint(game, lane, Level.Position),
            ProjectTubePoint(game, lane, Level.Position + Level.Depth)), overlays);
        int next = game.CurrentLevel.NextEdge(lane);
        Assert.Contains((ProjectTubePoint(game, next, Level.Position),
            ProjectTubePoint(game, next, Level.Position + Level.Depth)), overlays);

        Step(game, 39);
        Assert.Equal(110, game.PulsePhase);
        Assert.False(game.Pulsing);
        AssertPulsarTubeGeometry(game, []);
        overlays.Clear();
        new VectorRenderer(game).DrawPulsingLaneBoundaries(
            (start, end, _) => overlays.Add((start, end)));
        Assert.Empty(overlays);
    }

    [Fact]
    public void PulsarTubeSuppressesExactlyMultipleAffectedLanesAndKeepsTheirOverlays()
    {
        TempestGame game = new(pulsarTest: true, random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        MakeReady(game.Eggs[1], game.CurrentLevel);
        game.RunOneStep();
        int[] lanes = game.Pulsars.Select(pulsar => pulsar.EdgeIndex).ToArray();
        Assert.Equal(2, lanes.Length);
        Step(game, 70);
        Assert.True(game.Pulsing);
        Assert.All(game.Pulsars.Take(2), pulsar => Assert.True(pulsar.Height > 16));
        Assert.All(game.Pulsars.Skip(2), pulsar => Assert.True(pulsar.Height <= 16));
        AssertPulsarTubeGeometry(game, lanes);

        List<(Vector2 Start, Vector2 End)> overlays = [];
        new VectorRenderer(game).DrawPulsingLaneBoundaries(
            (start, end, _) => overlays.Add((start, end)));
        Assert.Equal(4, overlays.Count);
        Assert.Equal(4, overlays.Distinct().Count());
        foreach (int lane in lanes)
        {
            foreach (int boundary in new[] { lane, game.CurrentLevel.NextEdge(lane) })
                Assert.Contains((ProjectTubePoint(game, boundary, Level.Position),
                    ProjectTubePoint(game, boundary, Level.Position + Level.Depth)), overlays);
        }
    }

    [Theory]
    [InlineData(16, false)]
    [InlineData(17, true)]
    public void PulsarTubeSuppressionAndOverlayShareStrictVisualHeightEligibility(int height, bool affected)
    {
        TempestGame game = new(pulsarTest: true, random: new MinimumRandom());
        Step(game, 70);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(71, game.PulsePhase);
        Pulsar pulsar = Assert.Single(game.Pulsars, candidate => candidate.EdgeIndex == egg.EdgeIndex);
        Assert.All(game.Pulsars.Where(candidate => candidate != pulsar),
            candidate => Assert.True(candidate.Height <= 16));
        AdvancePulsar(pulsar, game.CurrentLevel, height - 1, 7, (_, _) => 0);
        Assert.Equal(height, pulsar.Height);
        AssertPulsarTubeGeometry(game, affected ? [pulsar.EdgeIndex] : []);
        int overlays = 0;
        new VectorRenderer(game).DrawPulsingLaneBoundaries((_, _, _) => overlays++);
        Assert.Equal(affected ? 2 : 0, overlays);
    }

    private static void AssertPulsarTubeGeometry(TempestGame game, int[] suppressedLanes)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        new VectorRenderer(game).DrawTube((start, end, color) => lines.Add((start, end, color)));
        int count = game.CurrentLevel.Definition.Points.Count;
        Assert.Equal(count * 3 - suppressedLanes.Length, lines.Count);
        Assert.Equal(lines.Count, lines.Distinct().Count());
        for (int lane = 0; lane < count; lane++)
        {
            int next = game.CurrentLevel.NextEdge(lane);
            Vector2 near = ProjectTubePoint(game, lane, Level.Position);
            Vector2 far = ProjectTubePoint(game, lane, Level.Position + Level.Depth);
            Vector2 nearNext = ProjectTubePoint(game, next, Level.Position);
            Vector2 farNext = ProjectTubePoint(game, next, Level.Position + Level.Depth);
            Color railColor = lane == game.Player.EdgeIndex ||
                lane == game.CurrentLevel.NextEdge(game.Player.EdgeIndex)
                ? new Color(255, 255, 0, 255)
                : Color.Blue;
            Assert.Contains((near, far, railColor), lines);
            Assert.Contains((far, farNext, Color.Blue), lines);
            Assert.Equal(!suppressedLanes.Contains(lane), lines.Contains((near, nearNext, Color.Blue)));
        }
    }

    private static Vector2 ProjectTubePoint(TempestGame game, int index, float depth)
    {
        Vector2 point = game.CurrentLevel.Definition.Points[index];
        return Projection.Project(point.X, point.Y, depth, game.CurrentLevel.Definition.CenterY,
            game.CurrentLevel.Definition.YOffset);
    }

    [Theory]
    [InlineData(7, PulsarCollision.Rim)]
    [InlineData(8, PulsarCollision.PulsingLane)]
    public void PulsarNearRimBoundaryIsStrictAndPulseBranchStartsAtEight(int rimDistance,
        PulsarCollision expected)
    {
        Pulsar pulsar = new(4, Level.Depth - rimDistance);
        Player player = new();
        player.Move(4 - player.EdgeIndex, 16);
        Assert.Equal(expected, pulsar.CheckPlayerCollision(player, pulsing: true));
    }

    [Fact]
    public void PulsingLaneCollisionRequiresSameEdgeWindowAndVerticalMovementState()
    {
        Pulsar pulsar = new(4, 100);
        Player player = new();
        player.Move(4 - player.EdgeIndex, 16);
        Assert.Equal(PulsarCollision.None, pulsar.CheckPlayerCollision(player, pulsing: false));
        Assert.Equal(PulsarCollision.PulsingLane, pulsar.CheckPlayerCollision(player, pulsing: true));
        player.Move(1, 16);
        Assert.Equal(PulsarCollision.None, pulsar.CheckPlayerCollision(player, pulsing: true));

        pulsar = new(4, 100);
        int calls = 0;
        int Roll(int _, int __) => calls++ == 50 ? 1 : 0;
        AdvancePulsar(pulsar, new Level(), 51, 4, Roll);
        player.Move(4 - player.EdgeIndex, 16);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        Assert.Equal(PulsarCollision.None, pulsar.CheckPlayerCollision(player, pulsing: true));
        AdvancePulsar(pulsar, new Level(), Pulsar.LanePauseDuration, 4, Roll);
        Assert.Equal(PulsarState.Flipping, pulsar.State);
        player.Move(pulsar.EdgeIndex - player.EdgeIndex, 16);
        Assert.Equal(PulsarCollision.None, pulsar.CheckPlayerCollision(player, pulsing: true));

        Pulsar descending = new(4, Level.Depth);
        int DescendRoll(int _, int __) => 0;
        descending.Update(new Level(), 4, DescendRoll);
        AdvancePulsar(descending, new Level(), Pulsar.RimPauseDuration, 4, DescendRoll);
        AdvancePulsar(descending, new Level(), 10, 4, DescendRoll);
        player.Move(descending.EdgeIndex - player.EdgeIndex, 16);
        Assert.Equal(PulsarState.Descending, descending.State);
        Assert.Equal(PulsarCollision.PulsingLane, descending.CheckPlayerCollision(player, pulsing: true));
    }

    [Theory]
    [InlineData(29.999f, true)]
    [InlineData(30, false)]
    [InlineData(30.001f, false)]
    public void PulsarFlipCollisionUsesStrictThirtyDegreeBoundary(float difference, bool lethal) =>
        Assert.Equal(lethal, Pulsar.IsWithinFlipKillAngle(difference, 0));

    [Theory]
    [InlineData(0, false)]
    [InlineData(0.001f, true)]
    [InlineData(15.999f, true)]
    [InlineData(16, false)]
    public void PulsarShotCollisionUsesStrictHeightBand(float separation, bool hit)
    {
        Pulsar pulsar = new(4, 100);
        Assert.Equal(hit, pulsar.IsHitBy(new Shot { EdgeIndex = 4, Height = 100 - separation }));
        Assert.False(pulsar.IsHitBy(new Shot { EdgeIndex = 5, Height = 99 }));
    }

    [Fact]
    public void PulsarShotAwardsTwoHundredThenSurvivorsReenterAsEggsAfterDeath()
    {
        TempestGame game = HatchDevelopmentPulsar();
        Pulsar first = Assert.Single(game.Pulsars);
        game.Player.Move(first.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        int ticks = 0;
        while (game.Pulsars.Count > 0 && ticks++ < 70)
            game.RunOneStep();
        Assert.Empty(game.Pulsars);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(9, game.CurrentLevel.RemainingEnemies);
        AssertPulsarAccounting(game);

        while (game.PulsePhase < 70)
            game.RunOneStep();
        Egg nextEgg = game.Eggs[0];
        MakeReady(nextEgg, game.CurrentLevel);
        game.Player.Move(nextEgg.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(71, game.PulsePhase);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Pulsar retracting = game.Pulsars.First();
        float pulsarHeight = retracting.Height;
        PulsarState pulsarState = retracting.State;
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> beforeRetraction = [];
        renderer.DrawPulsars((start, end, color) =>
            beforeRetraction.Add((start, end, color)));
        game.RunOneStep();
        List<(Vector2 Start, Vector2 End, Color Color)> duringRetraction = [];
        renderer.DrawPulsars((start, end, color) =>
            duringRetraction.Add((start, end, color)));
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.NotEqual(beforeRetraction[0].Start, duringRetraction[0].Start);
        Assert.Equal(pulsarHeight, retracting.Height);
        Assert.Equal(pulsarState, retracting.State);
        Step(game, TempestGame.PlayerDeathTicks - 1);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(9, game.CurrentLevel.RemainingEnemies);
        Assert.Empty(game.Pulsars);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(5, game.CurrentLevel.ReserveEggs);
        AssertPulsarAccounting(game);
    }

    [Fact]
    public void PulsarCompletionWaitsForFinalActiveUnitThenUsesExistingExit()
    {
        TempestGame activeGame = HatchDevelopmentPulsar();
        Level activeLevel = activeGame.CurrentLevel;
        while (activeLevel.Eggs.Count > 0)
        {
            foreach (Egg egg in activeLevel.Eggs)
                MakeReady(egg, activeLevel);
            while (activeLevel.HatchReadyEnemy(lane => new Pulsar(lane, 0)) is not null) { }
        }
        while (activeLevel.ReserveEggs > 0)
        {
            for (int tick = 0; tick < 50; tick++)
                activeLevel.UpdateEggRelease();
            foreach (Egg egg in activeLevel.Eggs)
                MakeReady(egg, activeLevel);
            while (activeLevel.HatchReadyEnemy(lane => new Pulsar(lane, 0)) is not null) { }
        }
        for (int index = 0; index < 9; index++)
            activeLevel.RecordEnemyDestroyed();
        activeGame.RunOneStep();
        Assert.Single(activeGame.Pulsars);
        Assert.Equal(LevelState.Playing, activeGame.State);
        AssertPulsarAccounting(activeGame);

        Pulsar finalPulsar = activeGame.Pulsars[0];
        activeGame.Player.Move(finalPulsar.EdgeIndex - activeGame.Player.EdgeIndex, 16);
        activeGame.RunOneStep(new PlayerInput(true, 0, false, false));
        int ticks = 0;
        while (!activeGame.IsExiting && ticks++ < 80)
            activeGame.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, activeGame.State);
        Assert.Equal(200, activeGame.Score.Score);
        Assert.Equal(Level.EnemyTotal, activeGame.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(activeGame.Pulsars);
        Assert.Empty(activeGame.Eggs);
        Assert.Equal(0, activeGame.CurrentLevel.ReserveEggs);
        AssertPulsarAccounting(activeGame);
    }

    // --- Direct Fuseballs: source behavior in an isolated ten-unit development wave ---

    [Fact]
    public void FuseballScenarioIsExplicitMutuallyExclusiveAndHatchesOneOrdinaryUnit()
    {
        string[] args = ["--fuseball-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseFuseballTest(args));
        Assert.False(Tempest.Game.Debug.DebugController.UseFuseballTest([]));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(args));
        foreach (string other in new[] { "--spiker-test", "--tanker-test" })
            Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(["--fuseball-test", other]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, fuseballTest: true));
        Assert.Throws<ArgumentException>(() => new TempestGame(tankerTest: true, fuseballTest: true));
        Assert.Empty(new TempestGame().Fuseballs);
        Assert.False(new TempestGame().FuseballTest);

        TempestGame game = HatchDevelopmentFuseball();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        Assert.Equal(12, fuseball.EdgeIndex);
        Assert.Equal(0, fuseball.Height);
        Assert.Equal(0, fuseball.W);
        Assert.Equal(1, fuseball.Direction);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.Equal(1, fuseball.StateTicks);
        Assert.Equal(3.3f, fuseball.ClimbSpeed);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(10, game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Fuseballs.Count);
    }

    [Fact]
    public void FuseballLateralMovementUsesLiteralSourceCounterAndForcesClimbAtZero()
    {
        Level level = new();
        Fuseball fuseball = new(3);
        for (int tick = 0; tick < 8; tick++)
            fuseball.Update(level, 0, (_, _) => throw new InvalidOperationException());
        Assert.Equal(0, fuseball.W);
        fuseball.Update(level, 0, (_, maximum) => maximum == 100 ? 90 : 2);
        Assert.Equal(1, fuseball.W);
        Assert.Equal(1, fuseball.Direction);
        AdvanceFuseball(fuseball, level, 63, (_, maximum) => maximum == 100 ? 0 : 2);
        Assert.Equal(4, fuseball.EdgeIndex);
        Assert.Equal(0, fuseball.W);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.Equal(0, fuseball.Height);
        Assert.Equal(0, fuseball.StateTicks);
    }

    [Theory]
    [InlineData(true, 0, 0, 1)]
    [InlineData(false, 14, 8, -1)]
    public void FuseballForwardWrapOrOpenEndpointBounce(bool continuous, int edge, int w, int direction)
    {
        Level level = CreateTopologyLevel(continuous);
        Fuseball fuseball = new(continuous ? 15 : 14);
        AdvanceFuseball(fuseball, level, 72);
        Assert.Equal(edge, fuseball.EdgeIndex);
        Assert.Equal(w, fuseball.W);
        Assert.Equal(direction, fuseball.Direction);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
    }

    [Theory]
    [InlineData(true, 15, 8)]
    [InlineData(false, 0, 0)]
    public void FuseballReverseWrapOrOpenEndpointBounce(bool continuous, int edge, int w)
    {
        Level level = CreateTopologyLevel(continuous);
        Fuseball fuseball = new();
        // Same-edge retarget uses its own random tie roll: 0 chooses direction -1.
        AdvanceFuseball(fuseball, level, 9, (minimum, maximum) => maximum == 100 ? 91 : minimum);
        Assert.Equal(edge, fuseball.EdgeIndex);
        Assert.Equal(w, fuseball.W);
        Assert.Equal(continuous ? -1 : 1, fuseball.Direction);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
    }

    [Theory]
    [InlineData(true, 15, 1, 15, 1)]
    [InlineData(false, 14, 1, 13, 8)]
    [InlineData(true, 0, 8, 0, 1)]
    public void FuseballRetargetRespectsShortRouteOpenTubeAndOppositeTie(
        bool continuous, int edge, int playerEdge, int expectedEdge, int expectedW)
    {
        Level level = CreateTopologyLevel(continuous);
        Fuseball fuseball = new(edge);
        for (int tick = 0; tick < 9; tick++)
            fuseball.Update(level, playerEdge, (minimum, maximum) => maximum == 100 ? 91 : minimum);
        Assert.Equal(expectedEdge, fuseball.EdgeIndex);
        Assert.Equal(expectedW, fuseball.W);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 51)]
    [InlineData(3, 76)]
    public void FuseballVerticalDurationUsesStrictRerolledSourceThreshold(int durationRoll, int duration)
    {
        Level level = new();
        Fuseball fuseball = new();
        AdvanceFuseball(fuseball, level, 72);
        int rolls = 0;
        int Roll(int minimum, int maximum)
        {
            Assert.Equal(1, minimum);
            Assert.Equal(3, maximum);
            rolls++;
            return durationRoll;
        }
        AdvanceFuseball(fuseball, level, duration - 1, Roll);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        fuseball.Update(level, 0, Roll);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.Equal(0, fuseball.StateTicks);
        Assert.Equal(duration, rolls);
        Assert.InRange(fuseball.Height, duration * 3.3f - .001f, duration * 3.3f + .001f);
    }

    [Fact]
    public void FuseballDescendingClampsAtZeroAndWaitsForDurationDecision()
    {
        Level level = new();
        Fuseball fuseball = new(0, climbSpeed: 1);
        AdvanceFuseball(fuseball, level, 72);
        AdvanceFuseball(fuseball, level, 26);
        Assert.Equal(26, fuseball.Height);
        AdvanceFuseball(fuseball, level, 72, (_, maximum) => maximum == 2 ? 2 : 0);
        Assert.Equal(FuseballState.Descending, fuseball.State);
        AdvanceFuseball(fuseball, level, 26, (_, _) => 3);
        Assert.Equal(0, fuseball.Height);
        Assert.Equal(FuseballState.Descending, fuseball.State);
        fuseball.Update(level, 0, (_, _) => 1);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.Equal(0, fuseball.StateTicks);
    }

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(15, true)]
    [InlineData(16, false)]
    public void FuseballShotHeightBandIsStrict(float separation, bool hit)
    {
        Level level = new();
        Fuseball fuseball = CreateRimFuseball(level);
        AdvanceFuseball(fuseball, level, 18);
        Assert.Equal(hit, fuseball.IsHitBy(new Shot { EdgeIndex = fuseball.EdgeIndex, Height = 400 - separation }));
        Assert.False(fuseball.IsHitBy(new Shot { EdgeIndex = 7, Height = 399 }));
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(6, true)]
    [InlineData(7, false)]
    [InlineData(8, false)]
    public void FuseballShotAndRimPlayerCollisionRequireInteriorW(int w, bool hit)
    {
        Level level = new();
        Fuseball fuseball = CreateRimFuseball(level);
        if (w == 8)
        {
            AdvanceFuseball(fuseball, level, 9, (minimum, maximum) => maximum == 100 ? 91 : minimum);
            fuseball.Update(level, 0, (minimum, _) => minimum);
        }
        else
            AdvanceFuseball(fuseball, level, w * 9);
        Assert.Equal(w, fuseball.W);
        Assert.Equal(hit, fuseball.IsHitBy(new Shot { EdgeIndex = fuseball.EdgeIndex, Height = 399 }));
        Player player = new();
        player.Move(fuseball.EdgeIndex - player.EdgeIndex, 16);
        Assert.Equal(hit, fuseball.HitsPlayer(player));
        player.Move(.4f, 16);
        Assert.Equal(hit, fuseball.HitsPlayer(player));
        player.Move(1, 16);
        Assert.False(fuseball.HitsPlayer(player));
    }

    [Fact]
    public void FuseballClimbingRimArrivalIsNotItselfLethal()
    {
        Level level = new();
        Fuseball fuseball = new(6, Level.Depth);
        AdvanceFuseball(fuseball, level, 73);
        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.False(fuseball.HitsPlayer(new Player()));
        fuseball.Update(level, 7, (_, _) => 1);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.False(fuseball.HitsPlayer(new Player()));
        AdvanceFuseball(fuseball, level, 18);
        Assert.True(fuseball.HitsPlayer(new Player()));
        fuseball.BeginPlayerKill();
        Assert.Equal(FuseballState.KillingPlayer, fuseball.State);
        Assert.Equal(3, fuseball.W);
        Assert.False(fuseball.HitsPlayer(new Player()));
    }

    [Theory]
    [InlineData(0, 1250)]
    [InlineData(.01f, 1000)]
    [InlineData(99.99f, 1000)]
    [InlineData(100, 1000)]
    [InlineData(100.01f, 750)]
    [InlineData(200, 750)]
    [InlineData(200.01f, 500)]
    [InlineData(300, 500)]
    [InlineData(300.01f, 250)]
    [InlineData(400, 250)]
    public void FuseballScoreUsesSourceHeightSteps(float height, int score)
    {
        Fuseball fuseball = new(0, height == 0 ? 1 : height);
        if (height > 0)
            AdvanceFuseball(fuseball, new Level(), 73);
        Assert.Equal(height, fuseball.Height);
        Assert.Equal(score, fuseball.KillScore);
    }

    [Fact]
    public void FuseballKillScoresOneUnitAndCreatesOnlyItsShortLivedPopup()
    {
        TempestGame game = HatchDevelopmentFuseball();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        AdvanceFuseball(fuseball, game.CurrentLevel, 71);
        AdvanceFuseball(fuseball, game.CurrentLevel, 26);
        AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        Assert.Equal(2, fuseball.W);
        // Test the production shot path without waiting for a shot to travel the entire tube.
        game.Player.Move(fuseball.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = fuseball.Height + 5;
        game.RunOneStep();
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Shots);
        Assert.Equal(1000, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(9, game.CurrentLevel.RemainingEnemies);
        FuseballScorePopup popup = Assert.Single(game.FuseballScorePopups);
        Assert.Equal(1000, popup.Points);
        Assert.Equal(fuseball.Height, popup.Height);
        VectorRenderer renderer = new(game);
        int labels = 0;
        renderer.DrawFuseballScorePopups((text, position, scale) =>
        {
            Assert.Equal("1000", text);
            AssertFinite(position);
            Assert.Equal(popup.Height / 64 + .5f, scale);
            labels++;
        });
        Assert.Equal(1, labels);
        Step(game, 29);
        Assert.Single(game.FuseballScorePopups);
        game.RunOneStep();
        Assert.Empty(game.FuseballScorePopups);
    }

    [Fact]
    public void FuseballPlayerDeathReconstructsSurvivorAsFreshEggAndRetainsScore()
    {
        TempestGame game = HatchDevelopmentFuseball();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        // Advance only this entity to the rim; the game still owns collision and life loss.
        for (int tick = 0; tick < 1000 &&
            !(fuseball.State == FuseballState.Lateral && fuseball.Height == Level.Depth); tick++)
            fuseball.Update(game.CurrentLevel, 0, (_, maximum) => maximum == 3 ? 3 : 1);
        Assert.Equal(Level.Depth, fuseball.Height);
        AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        game.Player.Move(fuseball.EdgeIndex - game.Player.EdgeIndex, 16);
        game.Score.Add(500);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(FuseballState.KillingPlayer, fuseball.State);
        Assert.Equal(60, game.DeathTicks);
        VectorRenderer renderer = new(game);
        Vector2 centerBeforeRetraction = renderer.ProjectFuseballCenter(fuseball);
        float height = fuseball.Height;
        game.RunOneStep();
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.NotEqual(centerBeforeRetraction, renderer.ProjectFuseballCenter(fuseball));
        Assert.Equal(height, fuseball.Height);
        Assert.Equal(FuseballState.KillingPlayer, fuseball.State);
        Step(game, TempestGame.PlayerDeathTicks - 1);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(500, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Fuseball fresh = Assert.Single(game.Fuseballs);
        Assert.NotSame(fuseball, fresh);
        Assert.Equal(0, fresh.Height);
        Assert.Equal(0, fresh.W);
        Assert.Equal(1, fresh.Direction);
    }

    [Fact]
    public void FuseballBlocksCompletionUntilItsUnitIsResolved()
    {
        TempestGame game = HatchDevelopmentFuseball();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        AdvanceFuseball(fuseball, game.CurrentLevel, 71);
        AdvanceFuseball(fuseball, game.CurrentLevel, 26);
        AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        // Resolve the other nine Egg units without introducing extra runtime enemies.
        for (int unit = 0; unit < 9; unit++)
        {
            if (game.Eggs.Count == 0)
                for (int tick = 0; tick < 50; tick++)
                    game.CurrentLevel.UpdateEggRelease();
            MakeReady(game.Eggs[0], game.CurrentLevel);
            Assert.NotNull(game.CurrentLevel.HatchReadyEnemy(lane => new Fuseball(lane)));
            game.CurrentLevel.RecordEnemyDestroyed();
        }
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.CurrentLevel.RemainingEnemies);
        game.Player.Move(fuseball.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Single(game.Shots).Height = fuseball.Height + 5;
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void FuseballAnimationHasFourSharedFramesFiveColoredArmsAndFiniteProjection()
    {
        TempestGame game = HatchDevelopmentFuseball();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        VectorRenderer renderer = new(game);
        HashSet<int> frames = [];
        Vector2[] firstArmPoints = [new(7, -5), new(6, -8), new(5, -8), new(6, -6)];
        Raylib_cs.Color[] colors =
        [
            new(255, 255, 0, 255), new(0, 255, 0, 255), new(255, 0, 255, 255),
            new(0, 255, 255, 255), new(255, 0, 0, 255)
        ];
        for (int tick = 0; tick < 16; tick++)
        {
            Assert.Equal((tick + 1) % 16 / 4, game.FuseballAnimationFrame);
            frames.Add(game.FuseballAnimationFrame);
            List<(Vector2 Start, Vector2 End, Raylib_cs.Color Color)> lines = [];
            renderer.DrawFuseballs((start, end, color) => lines.Add((start, end, color)));
            Assert.Equal(25, lines.Count);
            Assert.Equal(5, lines.Select(line => line.Color).Distinct().Count());
            Assert.All(lines, line => { AssertFinite(line.Start); AssertFinite(line.End); });
            for (int arm = 0; arm < 5; arm++)
                Assert.Equal(colors[arm], lines[arm * 5].Color);
            float scale = (8 / ((Level.Position + Level.Depth - fuseball.Height + Projection.ZOffset) /
                Projection.K) + .25f) / 6;
            Vector2 expectedTip = firstArmPoints[game.FuseballAnimationFrame] * scale;
            Assert.InRange(Vector2.Distance(expectedTip, lines[0].End - lines[0].Start), 0, .0001f);
            Vector2 point = Vector2.Lerp(game.CurrentLevel.Definition.Points[fuseball.EdgeIndex],
                game.CurrentLevel.Definition.Points[game.CurrentLevel.NextEdge(fuseball.EdgeIndex)],
                fuseball.W / 8f);
            Assert.Equal(Projection.Project(point.X, point.Y, Level.Position + Level.Depth - fuseball.Height,
                game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset),
                renderer.ProjectFuseballCenter(fuseball));
            game.RunOneStep();
        }
        Assert.Equal(4, frames.Count);
    }

    private static TempestGame HatchDevelopmentFuseball()
    {
        TempestGame game = new(fuseballTest: true, random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame HatchDevelopmentPulsar(bool enemyBulletsTest = false)
    {
        TempestGame game = new(pulsarTest: true, enemyBulletsTest: enemyBulletsTest,
            random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static void AdvancePulsar(Pulsar pulsar, Level level, int ticks, int playerEdge,
        Func<int, int, int> randomInclusive)
    {
        for (int tick = 0; tick < ticks; tick++)
            pulsar.Update(level, playerEdge, randomInclusive);
    }

    private static void AssertPulsarAccounting(TempestGame game)
    {
        Assert.Equal(Level.EnemyTotal, game.CurrentLevel.EnemiesDestroyed +
            game.CurrentLevel.EnemiesRimCleared + game.CurrentLevel.ReserveEggs +
            game.Eggs.Count + game.Pulsars.Count);
    }

    private static Fuseball CreateRimFuseball(Level level)
    {
        Fuseball fuseball = new(0, Level.Depth);
        AdvanceFuseball(fuseball, level, 74);
        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        return fuseball;
    }

    private static void AdvanceFuseball(Fuseball fuseball, Level level, int ticks,
        Func<int, int, int>? randomInclusive = null)
    {
        for (int tick = 0; tick < ticks; tick++)
            fuseball.Update(level, 0, randomInclusive ?? ((minimum, _) => minimum));
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }

    // --- Tanker development scenario (--tanker-test): flags, slot identity, climb and release ---

    [Fact]
    public void TankerTestFlagKeepsNormalAndSpikerModesSeparateAndRejectsConflict()
    {
        string[] tankerArgs = ["--tanker-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseTankerTest(tankerArgs));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(tankerArgs));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(tankerArgs));
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(
            ["--tanker-test", "--enemy-bullets-test"]));
        Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(
            ["--tanker-test", "--spiker-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, tankerTest: true));

        TempestGame normal = new();
        Assert.False(normal.TankerTest);
        Assert.False(normal.CurrentLevel.TankerDevelopmentWave);
        Assert.Empty(normal.Tankers);
        Assert.Empty(normal.Spikes);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.TankerTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);
        TempestGame tankerGame = new(startingLives: 10, tankerTest: true);
        Assert.True(tankerGame.TankerTest);
        Assert.Equal(10, tankerGame.Player.Lives);
        Assert.Equal(4, tankerGame.Eggs.Count);
        Assert.Equal(6, tankerGame.CurrentLevel.ReserveEggs);
        Assert.Empty(tankerGame.Tankers);
        Assert.Equal(10, tankerGame.TankerOriginalSlotsRemaining);
    }

    [Fact]
    public void CombinedTankerAndBulletFlagsArmBothTankerAndFlippers()
    {
        // Tanker firing remains on its independent ammunition/timer implementation.
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(2, [64, 3]));
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(2, tanker.BulletAmmunition);
        // 16 + 64 source-backed initial timer, minus the hatch-tick countdown.
        Assert.Equal(79, tanker.BulletTimer);
        Egg ordinaryEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 1);
        MakeReady(ordinaryEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper ordinary = Assert.Single(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.Equal(0, ordinary.ShotDelayTicks);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(78, tanker.BulletTimer);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(2, tanker.LatentCargoCount);
    }

    // --- Tanker firing (--tanker-test combined with --enemy-bullets-test) ---

    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(1, 1, -1)]
    [InlineData(1, 64, 0)]
    [InlineData(1, 64, 1)]
    [InlineData(1, 64, -1)]
    [InlineData(2, 1, 0)]
    [InlineData(2, 1, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(2, 64, 0)]
    [InlineData(2, 64, 1)]
    [InlineData(2, 64, 2)]
    public void TankerWholeClimbProtectsReferenceSpeedAndRandomizedShotCounts(
        int ammunition, int initialOffset, int successfulDecisions)
    {
        int availableDecisions = initialOffset == 1 ? 12 : 8;
        int expectedShots = successfulDecisions < 0
            ? ammunition
            : Math.Min(ammunition, successfulDecisions);
        int expectedDecisions = expectedShots == ammunition ? ammunition : availableDecisions;
        int[] rolls = Enumerable.Range(0, expectedDecisions)
            .Select(index => successfulDecisions < 0 || index < successfulDecisions ? 76 : 75)
            .ToArray();
        StrictTankerRandom random = new(
            (1, 3, [ammunition]), (1, 65, [initialOffset]), (0, 101, rolls));
        int initialAmmunition = random.NextInclusive(1, 2);
        int initialTimer = 16 + random.NextInclusive(1, 64);
        Tanker tanker = new(5, 0, bulletAmmunition: initialAmmunition,
            bulletTimer: initialTimer);
        Assert.Equal(10, tanker.Height);
        Assert.Equal(1.75f, tanker.ClimbSpeed);
        Assert.Equal(ammunition, tanker.BulletAmmunition);
        Assert.Equal(16 + initialOffset, tanker.BulletTimer);

        int eligibleUpdates = 0;
        List<int> decisionUpdates = [];
        List<int> shotUpdates = [];
        for (int update = 1; update <= 215; update++)
        {
            int previousTimer = tanker.BulletTimer;
            int previousAmmo = tanker.BulletAmmunition;
            int previousDecisions = random.DecisionDrawCount;
            TankerUpdateResult result = tanker.UpdateForDevelopment(
                random.NextInclusive, out EnemyBullet? bullet);

            Assert.Equal(10 + update * 1.75f, tanker.Height);
            Assert.Equal(5, tanker.EdgeIndex);
            Assert.Equal(update == 215
                ? TankerUpdateResult.NaturalCargoRelease
                : TankerUpdateResult.Active, result);
            if (tanker.Height < 380)
            {
                eligibleUpdates++;
                if (previousAmmo > 0)
                    Assert.Equal(previousTimer == 0 ? 16 : previousTimer - 1, tanker.BulletTimer);
            }
            if (previousAmmo == 0 || tanker.Height >= 380)
            {
                Assert.Equal(previousTimer, tanker.BulletTimer);
                Assert.Equal(previousDecisions, random.DecisionDrawCount);
                Assert.Null(bullet);
            }
            if (random.DecisionDrawCount != previousDecisions)
            {
                Assert.Equal(previousDecisions + 1, random.DecisionDrawCount);
                decisionUpdates.Add(update);
                Assert.Equal(16, tanker.BulletTimer);
            }
            if (bullet is not null)
            {
                shotUpdates.Add(update);
                Assert.Equal(tanker.EdgeIndex, bullet.EdgeIndex);
                Assert.Equal(tanker.Height, bullet.Height);
            }
            Assert.Equal(previousAmmo - (bullet is null ? 0 : 1), tanker.BulletAmmunition);
            if (update == 1)
                Assert.Equal(11.75f, tanker.Height);
            if (update == 211)
                Assert.Equal(379.25f, tanker.Height);
            if (update == 212)
                Assert.Equal(381, tanker.Height);
        }

        Assert.Equal(211, eligibleUpdates);
        Assert.Equal(386.25f, tanker.Height);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
        Assert.Equal(expectedShots, shotUpdates.Count);
        Assert.Equal(ammunition - expectedShots, tanker.BulletAmmunition);
        int[] expectedUpdates = Enumerable.Range(0, expectedDecisions)
            .Select(index => initialTimer + 1 + index * 17).ToArray();
        Assert.Equal(expectedUpdates, decisionUpdates);
        Assert.Equal(expectedUpdates.Take(expectedShots), shotUpdates);
        random.AssertComplete();

        int finalTimer = tanker.BulletTimer;
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease,
            tanker.UpdateForDevelopment(random.NextInclusive, out EnemyBullet? pendingBullet));
        Assert.Null(pendingBullet);
        tanker.ReleaseFlipperCargo(new Level(), 1, TankerReleaseKind.Natural);
        Assert.Equal(TankerUpdateResult.Active,
            tanker.UpdateForDevelopment(random.NextInclusive, out EnemyBullet? releasedBullet));
        Assert.Null(releasedBullet);
        Assert.Equal(386.25f, tanker.Height);
        Assert.Equal(finalTimer, tanker.BulletTimer);
        random.AssertComplete();
    }

    [Fact]
    public void TankerLateDecisionWindowsPermitTwoShotsBeforeNaturalRelease()
    {
        StrictTankerRandom random = new((0, 101, [75, 75, 75, 75, 75, 75, 76, 76]));
        Tanker tanker = new(4, 0, bulletAmmunition: 2, bulletTimer: 80);
        List<int> decisions = [];
        List<int> shots = [];
        for (int update = 1; update <= 215; update++)
        {
            int previousDecisions = random.DecisionDrawCount;
            TankerUpdateResult result = tanker.UpdateForDevelopment(
                random.NextInclusive, out EnemyBullet? bullet);
            if (random.DecisionDrawCount != previousDecisions)
                decisions.Add(update);
            if (bullet is not null)
            {
                shots.Add(update);
                Assert.Equal(10 + update * 1.75f, bullet.Height);
                Assert.Equal(4, bullet.EdgeIndex);
            }
            Assert.Equal(update == 215
                ? TankerUpdateResult.NaturalCargoRelease
                : TankerUpdateResult.Active, result);
        }
        Assert.Equal(new[] { 81, 98, 115, 132, 149, 166, 183, 200 }, decisions);
        Assert.Equal(new[] { 183, 200 }, shots);
        Assert.Equal(0, tanker.BulletAmmunition);
        Assert.Equal(386.25f, tanker.Height);
        random.AssertComplete();
    }

    [Fact]
    public void TankerTimedGameShotsKeepClimbingAndSurviveNaturalReleaseIndependently()
    {
        StrictTankerRandom random = new(
            (1, 3, [2]), (1, 65, [7]), (0, 101, []));
        TempestGame game = CreateTankerWave(enemyBulletsTest: true, random: random);
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(11.75f, tanker.Height);
        Assert.Equal(22, tanker.BulletTimer);
        List<(int Update, EnemyBullet Bullet, float Origin)> creations = [];
        for (int update = 2; update <= 215; update++)
        {
            // Ordinary Flippers remain below the rim; script their decisions as failures
            // before the Tanker's decision, matching the established game update order.
            Assert.All(game.Flippers, flipper => Assert.Equal(FlipperState.Climbing, flipper.State));
            List<int> rolls = [];
            if (update >= 24 && update <= 211 && (update - 24) % 17 == 0)
                rolls.Add(update is 194 or 211 ? 76 : 75);
            random.ExpectDecisions(rolls);
            HashSet<EnemyBullet> previousBullets = game.EnemyBullets.ToHashSet();
            game.RunOneStep();
            random.AssertDecisionsComplete();
            Assert.Equal(LevelState.Playing, game.State);
            Assert.Equal(3, game.Player.Lives);
            Assert.Equal(10 + update * 1.75f, tanker.Height);
            Assert.Empty(game.Shots);
            Assert.Equal(0, game.Score.Score);
            EnemyBullet[] newBullets = game.EnemyBullets
                .Where(bullet => !previousBullets.Contains(bullet)).ToArray();
            if (update is 194 or 211)
            {
                EnemyBullet bullet = Assert.Single(newBullets);
                Assert.Equal(tanker.EdgeIndex, bullet.EdgeIndex);
                Assert.Equal(tanker.Height, bullet.Height);
                creations.Add((update, bullet, bullet.Height));
            }
            else
                Assert.Empty(newBullets);
            foreach ((int createdAt, EnemyBullet bullet, float origin) in creations)
            {
                int lifetimeUpdates = (int)MathF.Floor((400 - origin) / 3) + 1;
                Assert.Equal(origin + 3 * Math.Min(update - createdAt, lifetimeUpdates), bullet.Height);
                if (bullet.Height <= 400)
                    Assert.Contains(bullet, game.EnemyBullets);
                else
                    Assert.DoesNotContain(bullet, game.EnemyBullets);
            }
            Assert.Equal(update == 215 ? 12 : 10, game.TankerAccountingTotal);
            AssertTankerAccounting(game);
        }
        Assert.Equal(new[] { 194, 211 }, creations.Select(creation => creation.Update));
        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerLatentCargoUnits);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        EnemyBullet survivingBullet = Assert.Single(game.EnemyBullets);
        Assert.Same(creations[1].Bullet, survivingBullet);
        Assert.Equal(391.25f, survivingBullet.Height);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(new[] { 0, 1 }, cargo.Select(flipper => flipper.CargoUnitId));
        Assert.Equal(game.CurrentLevel.PreviousEdge(tanker.EdgeIndex), cargo[0].SourceEdge);
        Assert.Equal(game.CurrentLevel.NextEdge(tanker.EdgeIndex), cargo[1].SourceEdge);
        Assert.Equal(game.CurrentLevel.PreviousEdge(cargo[0].SourceEdge), cargo[0].DestinationEdge);
        Assert.Equal(game.CurrentLevel.NextEdge(cargo[1].SourceEdge), cargo[1].DestinationEdge);
        Assert.Equal(1, cargo[0].Direction);
        Assert.Equal(-1, cargo[1].Direction);
        Assert.All(cargo, flipper =>
        {
            Assert.Equal(FlipperState.Flipping, flipper.State);
            Assert.Equal(386.25f, flipper.Height);
            Assert.Equal(0, flipper.TurnAngle);
            Assert.Equal(0, flipper.PendingZapTicks);
        });
        random.AssertComplete();

        game.RunOneStep();
        random.AssertDecisionsComplete();
        Assert.Contains(survivingBullet, game.EnemyBullets);
        Assert.Equal(394.25f, survivingBullet.Height);
        Assert.All(cargo, flipper => Assert.Equal(388.25f, flipper.Height));
        Assert.Equal(0, game.Score.Score);
        AssertTankerAccounting(game);
        random.AssertComplete();
    }

    [Fact]
    public void ArmedTankerPendingZapAllowsFireButExpirySkipsReadyDecisionAndKeepsBullet()
    {
        StrictTankerRandom random = new(
            (1, 3, [2]), (1, 65, [1]), (0, 101, [76]));
        TempestGame game = CreateTankerWave(enemyBulletsTest: true, random: random);
        Tanker tanker = Assert.Single(game.Tankers);
        Step(game, 16);
        Assert.Equal(0, tanker.BulletTimer);
        Assert.True(tanker.TryZap(18));

        float beforeFire = tanker.Height;
        game.RunOneStep(); // Tanker update 18: pending zap does not suppress the shot.
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(beforeFire + 1.75f, tanker.Height);
        Assert.Equal(tanker.Height, bullet.Height);
        Assert.Equal(17, tanker.PendingZapTicks);
        Assert.Equal(1, tanker.BulletAmmunition);
        Step(game, 16);
        Assert.Equal(0, tanker.BulletTimer);
        Assert.Equal(1, tanker.PendingZapTicks);
        float beforeExpiry = tanker.Height;
        float bulletBeforeExpiry = bullet.Height;

        game.RunOneStep(); // Update 35 would otherwise be the next firing decision.
        Assert.Empty(game.Tankers);
        Assert.Equal(beforeExpiry, tanker.Height);
        Assert.Equal(0, tanker.BulletTimer);
        Assert.Equal(1, tanker.BulletAmmunition);
        Assert.Equal(1, random.DecisionDrawCount);
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(bulletBeforeExpiry + 3, bullet.Height);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.Equal(0, game.TankerLatentCargoUnits);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(200, game.Score.Score);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(new[] { 0, 1 }, cargo.Select(flipper => flipper.CargoUnitId));
        Assert.All(cargo, flipper =>
        {
            Assert.Equal(beforeExpiry, flipper.Height);
            Assert.Equal(8, flipper.PendingZapTicks);
            Assert.Equal(FlipperState.Flipping, flipper.State);
        });
        AssertTankerAccounting(game);
        random.AssertComplete();

        Step(game, 8);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.Equal(500, game.Score.Score);
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(3, game.Player.Lives);
        AssertTankerAccounting(game);
        random.AssertComplete();
    }

    [Fact]
    public void TankerTestAloneKeepsTankerUnarmedAndSilent()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(0, tanker.BulletAmmunition);
        Assert.Equal(Tanker.BulletTimerResetTicks, tanker.BulletTimer);
        Step(game, 60);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, tanker.BulletAmmunition);
        Assert.Equal(Tanker.BulletTimerResetTicks, tanker.BulletTimer);
    }

    [Fact]
    public void TankerFiringTimerDecrementsResetsAndUsesStrictDecisionBoundary()
    {
        Tanker tanker = new(5, 0, climbSpeed: 1, bulletAmmunition: 1, bulletTimer: 2);
        Assert.Equal(1, tanker.BulletAmmunition);
        Assert.Equal(2, tanker.BulletTimer);

        int decision = 75;
        Assert.Equal(TankerUpdateResult.Active,
            tanker.UpdateForDevelopment((_, _) => decision, out EnemyBullet? bullet));
        Assert.Null(bullet);
        Assert.Equal(1, tanker.BulletTimer);
        tanker.UpdateForDevelopment((_, _) => decision, out bullet);
        Assert.Null(bullet);
        Assert.Equal(0, tanker.BulletTimer);

        // Expiry resets to exactly 16; a decision of 75 fails and costs no ammunition.
        tanker.UpdateForDevelopment((_, _) => decision, out bullet);
        Assert.Null(bullet);
        Assert.Equal(16, tanker.BulletTimer);
        Assert.Equal(1, tanker.BulletAmmunition);

        // A decision of 76 fires on the next expiry, consuming exactly one ammunition unit.
        decision = 76;
        for (int tick = 0; tick < 16; tick++)
        {
            tanker.UpdateForDevelopment((_, _) => decision, out bullet);
            Assert.Null(bullet);
        }
        tanker.UpdateForDevelopment((_, _) => decision, out bullet);
        Assert.NotNull(bullet);
        Assert.Equal(5, bullet!.EdgeIndex);
        Assert.Equal(tanker.Height, bullet.Height);
        Assert.Equal(0, tanker.BulletAmmunition);
        Assert.Equal(16, tanker.BulletTimer);

        // Exhausted ammunition freezes the timer and never fires again.
        for (int tick = 0; tick < 40; tick++)
        {
            tanker.UpdateForDevelopment((_, _) => 100, out bullet);
            Assert.Null(bullet);
        }
        Assert.Equal(16, tanker.BulletTimer);
    }

    [Fact]
    public void UnarmedTankerNeverRunsTimerOrDrawsDecisions()
    {
        Tanker tanker = new(3, 0, climbSpeed: 1);
        Assert.Equal(0, tanker.BulletAmmunition);
        for (int tick = 0; tick < 50; tick++)
        {
            tanker.UpdateForDevelopment(
                (_, _) => throw new InvalidOperationException("unarmed Tanker must not draw"),
                out EnemyBullet? bullet);
            Assert.Null(bullet);
        }
        Assert.Equal(Tanker.BulletTimerResetTicks, tanker.BulletTimer);
    }

    [Theory]
    [InlineData(369, true)]   // post-climb 379: strictly below the ceiling, eligible
    [InlineData(370, false)]  // post-climb exactly 380: not eligible
    [InlineData(371, false)]  // post-climb 381: above the ceiling, not eligible
    public void TankerFiringRequiresPostClimbHeightStrictlyBelow380(float climbSpeed, bool fires)
    {
        Tanker tanker = new(4, 0, climbSpeed: climbSpeed, bulletAmmunition: 1, bulletTimer: 0);
        Assert.Equal(TankerUpdateResult.Active,
            tanker.UpdateForDevelopment((_, _) => 100, out EnemyBullet? bullet));
        Assert.Equal(10 + climbSpeed, tanker.Height);
        if (fires)
        {
            Assert.NotNull(bullet);
            // Climb advances before the firing evaluation: the bullet starts at the post-climb height.
            Assert.Equal(379, bullet!.Height);
            Assert.Equal(4, bullet.EdgeIndex);
            Assert.Equal(0, tanker.BulletAmmunition);
        }
        else
        {
            Assert.Null(bullet);
            Assert.Equal(1, tanker.BulletAmmunition);
            // The height gate skips the countdown entirely, matching Incitti's outer height check.
            Assert.Equal(0, tanker.BulletTimer);
        }
    }

    [Fact]
    public void TankerCannotFireOncePendingOrAfterCargoRelease()
    {
        // One oversized climb jumps straight past both the firing ceiling and the release threshold.
        Tanker tanker = new(6, 0, climbSpeed: 376, bulletAmmunition: 2, bulletTimer: 0);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease,
            tanker.UpdateForDevelopment((_, _) => 100, out EnemyBullet? bullet));
        Assert.Null(bullet);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease,
            tanker.UpdateForDevelopment((_, _) => 100, out bullet));
        Assert.Null(bullet);

        tanker.ReleaseFlipperCargo(new Level(), 1, TankerReleaseKind.Natural);
        Assert.Equal(TankerState.CargoReleased, tanker.State);
        Assert.Equal(TankerUpdateResult.Active,
            tanker.UpdateForDevelopment((_, _) => 100, out bullet));
        Assert.Null(bullet);
        Assert.Equal(2, tanker.BulletAmmunition);
        Assert.Equal(0, tanker.BulletTimer);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ArmedTankerUsesControlledInclusiveAmmunitionAndTimerOffset(int ammunition)
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(ammunition, [7]));
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(ammunition, tanker.BulletAmmunition);
        Assert.InRange(tanker.BulletAmmunition, 1, 2);
        // 16 + controlled offset 7, minus the hatch-tick countdown.
        Assert.Equal(22, tanker.BulletTimer);
    }

    [Fact]
    public void TankerDecisionBoundaryMatchesFlipperConventionInGame()
    {
        // Queue: ammunition 1, offset 1, then decisions 75 (fail) and 76 (fire).
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(1, [1, 75, 76]));
        Tanker tanker = Assert.Single(game.Tankers);
        Step(game, 17);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(1, tanker.BulletAmmunition);
        Step(game, 16);
        Assert.Empty(game.EnemyBullets);
        game.RunOneStep();
        Assert.Single(game.EnemyBullets);
        Assert.Equal(0, tanker.BulletAmmunition);
    }

    [Fact]
    public void TankerFiresFromLaneAtPostClimbHeightThroughGameUpdate()
    {
        TempestGame game = PrepareTankerBulletGame();
        Tanker tanker = Assert.Single(game.Tankers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(tanker.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(tanker.Height, bullet.Height);
        Assert.Equal(16, tanker.BulletTimer);
        // The bullet is a projectile, not an enemy: ledger totals are untouched by its creation.
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(0, game.Score.Score);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerBulletSurvivesTankerShotDestructionAndIsWorthTwentyFive()
    {
        TempestGame game = PrepareTankerBulletGame();
        Tanker tanker = Assert.Single(game.Tankers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        ShootTanker(game, tanker);
        // Destroying the Tanker never removes an already-fired bullet.
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);

        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = bullet.Height - 1;
        game.RunOneStep();
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(225, game.Score.Score);
        // Shooting the bullet resolves no enemy unit: slot and cargo accounting are unchanged.
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerCargoUnitsResolved);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerBulletSurvivesNaturalCargoRelease()
    {
        TempestGame game = PrepareTankerBulletGame();
        Tanker tanker = Assert.Single(game.Tankers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        // Fast-forward the Tanker to its release point outside the game loop (established test
        // pattern); the parameterless update never draws or fires.
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        game.RunOneStep();
        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        // Natural release never removes an already-fired bullet.
        Assert.Contains(bullet, game.EnemyBullets);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void LifeLossClearsTankerBulletsAndRearmsFreshTankerWithNewValues()
    {
        // Queue: first Tanker ammo 1 and offset 1, decision 100 (fire), then fresh ammo 2 and
        // offset 5 for the Tanker rebuilt after the life loss.
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(1, [1, 100, 2, 5]));
        Tanker firstTanker = Assert.Single(game.Tankers);
        Step(game, 16);
        game.RunOneStep();
        Assert.Single(game.EnemyBullets);

        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Single(game.EnemyBullets);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        // Active bullets clear through the established life-reset behavior.
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Tankers);

        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        Tanker freshTanker = Assert.Single(game.Tankers);
        // The rebuilt Tanker draws fresh source-backed values; firing state is never preserved.
        Assert.NotSame(firstTanker, freshTanker);
        Assert.Equal(2, freshTanker.BulletAmmunition);
        Assert.Equal(20, freshTanker.BulletTimer);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CombinedModeCompletesWithBulletsAndClearsResidualsAtExit()
    {
        TempestGame game = new(startingLives: 100, tankerTest: true, enemyBulletsTest: true,
            random: new TankerBulletModeRandom());
        bool bulletSeen = false;
        for (int tick = 0; tick < 30000 && !game.LevelComplete && !game.GameOver; tick++)
        {
            Enemy? target = game.Tankers.Cast<Enemy>().Concat(game.Flippers)
                .OrderByDescending(enemy => enemy.Height).FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            bulletSeen |= game.EnemyBullets.Count > 0;
            AssertTankerAccounting(game);
        }

        Assert.True(game.LevelComplete,
            $"state={game.State}, lives={game.Player.Lives}, original={game.TankerOriginalSlotsRemaining}, " +
            $"cargo={game.TankerCargoUnitsRemaining}, bullets={game.EnemyBullets.Count}");
        Assert.True(bulletSeen);
        // Residual bullets never block completion and clear through the established exit path.
        Assert.Empty(game.EnemyBullets);
    }

    [Fact]
    public void TankerWaveHatchesStableOriginalSlotZeroExactlyOnce()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(0, tanker.OriginalSlotId);
        Assert.InRange(tanker.EdgeIndex, 0, 15);
        Assert.Equal(11.75f, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        Assert.Equal(1, game.ActiveTankerOriginalEnemies);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Empty(game.Spikers);
    }

    [Fact]
    public void NonTankerOriginalEggSlotsContinueToHatchOrdinaryFlippers()
    {
        TempestGame game = CreateTankerWave();
        Egg ordinaryEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 1);
        MakeReady(ordinaryEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers, candidate => !candidate.IsTankerCargo);
        Assert.Equal(1, flipper.OriginalSlotId);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Single(game.Tankers);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(10, game.TankerAccountingTotal);
    }

    [Fact]
    public void TankerFixedSpeedKeepsLaneAndReleasesStrictlyAbove385()
    {
        Tanker tanker = new(7, 0, climbSpeed: 1);
        Assert.Equal(10, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        for (int tick = 0; tick < 375; tick++)
            Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(385, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(7, tanker.EdgeIndex);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease, tanker.UpdateForDevelopment());
        Assert.Equal(386, tanker.Height);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
    }

    [Fact]
    public void TankerUsesDocumentedDevelopmentSpeedAndNeverDirectlyHitsPlayer()
    {
        Tanker tanker = new(7, 0);
        Assert.Equal(1.75f, tanker.ClimbSpeed);
        Assert.False(tanker.HitsPlayer(new Player()));
        Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(11.75f, tanker.Height);
        Assert.Equal(7, tanker.EdgeIndex);
    }

    [Theory]
    [InlineData(0, 1, 15, 1, 14, 2, 1, -1)]
    [InlineData(15, 1, 14, 0, 13, 1, 1, -1)]
    [InlineData(6, 1, 5, 7, 4, 8, 1, -1)]
    [InlineData(0, -1, 1, 15, 2, 14, -1, 1)]
    [InlineData(15, -1, 0, 14, 1, 13, -1, 1)]
    public void TankerCargoUsesOppositeAdjacentLanesAndDirectionsOnContinuousTube(
        int tankerLane, int firstSideDirection, int expectedFirstLane, int expectedSecondLane,
        int expectedFirstDestination, int expectedSecondDestination,
        int expectedFirstDirection, int expectedSecondDirection)
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(tankerLane);
        TankerReleaseResult result = tanker.ReleaseFlipperCargo(
            level, firstSideDirection, TankerReleaseKind.Natural);
        Assert.Equal(TankerReleaseKind.Natural, result.Kind);
        Assert.Equal(2, result.Cargo.Count);
        Assert.Equal(expectedFirstLane, result.Cargo[0].SourceLane);
        Assert.Equal(expectedSecondLane, result.Cargo[1].SourceLane);
        Assert.Equal(expectedFirstDestination, result.Cargo[0].DestinationLane);
        Assert.Equal(expectedSecondDestination, result.Cargo[1].DestinationLane);
        Assert.Equal(expectedFirstDirection, result.Cargo[0].Direction);
        Assert.Equal(expectedSecondDirection, result.Cargo[1].Direction);
        Assert.Equal(tanker.Height, result.Cargo[0].Height);
        Assert.Equal(tanker.Height, result.Cargo[1].Height);
        Assert.Equal(TankerState.CargoReleased, tanker.State);
        Assert.Equal(0, tanker.LatentCargoCount);
        Assert.Empty(tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.PlayerShotDestruction).Cargo);
    }

    [Fact]
    public void TankerCargoFlippersStartInCalculatedPivotAndPreserveOrdinaryConstructor()
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(0);
        TankerCargoSpawn[] cargo = tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.Natural).Cargo.ToArray();
        Flipper first = Flipper.CreateTankerCargo(level, cargo[0], 10);
        Flipper second = Flipper.CreateTankerCargo(level, cargo[1], 11);
        Assert.Equal(FlipperState.Flipping, first.State);
        Assert.Equal(FlipperState.Flipping, second.State);
        Assert.Equal(cargo[0].SourceLane, first.EdgeIndex);
        Assert.Equal(cargo[0].SourceLane, first.SourceEdge);
        Assert.Equal(cargo[0].DestinationLane, first.DestinationEdge);
        Assert.Equal(cargo[0].Direction, first.Direction);
        Assert.Equal(first.ActiveTransition!.Value.SweepMagnitudeDegrees, first.TotalTurnAngle);
        Assert.Equal(0, first.TurnAngle);
        Assert.Equal(0, second.TurnAngle);
        Assert.Equal(-first.Direction, second.Direction);
        Assert.Equal(tanker.Height, first.Height);
        Assert.Equal(tanker.Height, second.Height);
        Assert.Equal(10, first.CargoUnitId);
        Assert.Equal(11, second.CargoUnitId);
        Assert.Equal(15, first.SourceEdge);
        Assert.Equal(1, second.SourceEdge);
        Assert.Equal(14, first.DestinationEdge);
        Assert.Equal(2, second.DestinationEdge);
        Assert.DoesNotContain(tanker.EdgeIndex, new[] { first.SourceEdge, first.DestinationEdge });
        Assert.DoesNotContain(tanker.EdgeIndex, new[] { second.SourceEdge, second.DestinationEdge });
        Assert.Equal(1, first.Direction);
        Assert.Equal(-1, second.Direction);
        Assert.Equal(FlipperState.Climbing, new Flipper(4, 0).State);

        float height = first.Height;
        first.Update(level, 8);
        Assert.Equal(height + 2, first.Height);
        Assert.Equal(8, first.TurnAngle);
    }

    [Fact]
    public void TankerCargoKeepsItsDirectionThroughBelowRimPivotAndPauseCycles()
    {
        Level level = new();
        Tanker tanker = new(0, 0, climbSpeed: 1);
        TankerCargoSpawn cargo = tanker.ReleaseFlipperCargo(
            level, 1, TankerReleaseKind.Natural).Cargo[0];
        Flipper flipper = Flipper.CreateTankerCargo(level, cargo, 10);
        int playerEdge = 8;
        int firstPivotTicks = 0;

        while (flipper.State == FlipperState.Flipping)
        {
            flipper.Update(level, playerEdge);
            playerEdge = (playerEdge + 5) % level.Definition.Points.Count;
            firstPivotTicks++;
        }

        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(14, flipper.EdgeIndex);
        Assert.Equal(cargo.Height + 2 * firstPivotTicks, flipper.Height);
        Assert.Equal(1, flipper.Direction);
        Assert.False(flipper.CompletedIntoPlayer);

        float heightAtPause = flipper.Height;
        int pauseTicks = flipper.PauseTicks;
        for (int tick = 0; tick < pauseTicks; tick++)
        {
            flipper.Update(level, playerEdge);
            playerEdge = (playerEdge + 7) % level.Definition.Points.Count;
        }

        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(heightAtPause + pauseTicks, flipper.Height);
        Assert.Equal(14, flipper.SourceEdge);
        Assert.Equal(13, flipper.DestinationEdge);
        Assert.Equal(1, flipper.Direction);
        Assert.False(flipper.CompletedIntoPlayer);

        int secondPivotTicks = 0;
        while (flipper.State == FlipperState.Flipping)
        {
            flipper.Update(level, playerEdge);
            playerEdge = (playerEdge + 3) % level.Definition.Points.Count;
            secondPivotTicks++;
        }

        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(13, flipper.EdgeIndex);
        Assert.Equal(heightAtPause + pauseTicks + 2 * secondPivotTicks, flipper.Height);
        Assert.Equal(1, flipper.Direction);
        Assert.False(flipper.CompletedIntoPlayer);
    }

    [Fact]
    public void TankerCargoResumesPlayerPursuitAtTheRimAndKeepsCompletionCollision()
    {
        Level level = new();
        Tanker tanker = new(0, 0, climbSpeed: 1);
        for (int tick = 0; tick < 370; tick++)
            tanker.UpdateForDevelopment();
        TankerCargoSpawn cargo = tanker.ReleaseFlipperCargo(
            level, 1, TankerReleaseKind.Natural).Cargo[0];
        Flipper flipper = Flipper.CreateTankerCargo(level, cargo, 10);

        while (flipper.State == FlipperState.Flipping)
            flipper.Update(level, 15);
        Assert.Equal(14, flipper.EdgeIndex);
        Assert.Equal(Level.Depth, flipper.Height);

        int pauseTicks = flipper.PauseTicks;
        for (int tick = 0; tick < pauseTicks; tick++)
            flipper.Update(level, 15);

        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(15, flipper.DestinationEdge);

        for (int tick = 0; !flipper.CompletedIntoPlayer; tick++)
            flipper.Update(level, 15);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(15, flipper.EdgeIndex);
    }

    [Fact]
    public void OrdinaryFlipperIgnoresPlayerBelowRimAndStillPursuesAtRim()
    {
        Level level = new();
        Flipper flipper = new(4, 0);

        for (int tick = 0; tick < 100; tick++)
            flipper.Update(level, tick % level.Definition.Points.Count);

        Assert.Equal(4, flipper.EdgeIndex);
        Assert.Equal(200, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);

        while (flipper.Height < Level.Depth - 2)
            flipper.Update(level, 7);
        flipper.Update(level, 7);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(4, flipper.EdgeIndex);
        Assert.Equal(FlipperState.Pausing, flipper.State);

        int pauseTicks = flipper.PauseTicks;
        for (int tick = 0; tick < pauseTicks - 1; tick++)
            flipper.Update(level, 7);
        flipper.Update(level, 7);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(5, flipper.DestinationEdge);
    }

    [Fact]
    public void CargoPivotDoesNotKillPlayerBeforeCargoReachesTheRim()
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(0);
        TankerCargoSpawn cargo = tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.Natural).Cargo[0];
        Flipper flipper = Flipper.CreateTankerCargo(level, cargo, 0);
        while (flipper.State == FlipperState.Flipping && flipper.Height < Level.Depth)
            flipper.Update(level, cargo.DestinationLane);
        Assert.False(flipper.CompletedIntoPlayer);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        while (flipper.State == FlipperState.Flipping)
            flipper.Update(level, cargo.DestinationLane);
        Assert.True(flipper.CompletedIntoPlayer);
    }

    // --- Flipper rim-arrival collision/timing regressions (direct climb into the player lane) ---

    [Fact]
    public void ClimbingFlipperReachingRimInPlayerLaneSignalsLethalCollision()
    {
        Level level = new();
        Flipper flipper = new(5, Level.Depth - 1);
        flipper.Update(level, 5);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.NotEqual(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void ClimbingFlipperReachingRimInAnotherLaneStillPauses()
    {
        Level level = new();
        Flipper flipper = new(5, Level.Depth - 1);
        flipper.Update(level, 6);
        Assert.False(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void DirectRimArrivalKillsPlayerOnceAndShotOnFinalTickWinsInstead()
    {
        TempestGame game = CreateTankerWave();
        Flipper flipper = HatchOneOrdinaryTankerWaveFlipper(game);
        int lives = game.Player.Lives;
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        ForceClimbToJustBelowRim(flipper, game);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);

        game = CreateTankerWave();
        flipper = HatchOneOrdinaryTankerWaveFlipper(game);
        lives = game.Player.Lives;
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        ForceClimbToJustBelowRim(flipper, game);
        shot.Height = flipper.Height + 5;
        game.RunOneStep();
        Assert.DoesNotContain(flipper, game.Flippers);
        Assert.NotEqual(LevelState.PlayerDying, game.State);
        Assert.Equal(lives, game.Player.Lives);
    }

    private static void ForceClimbToJustBelowRim(Flipper flipper, TempestGame game)
    {
        for (int tick = 0; flipper.Height < Level.Depth - 2 && tick < 1000; tick++)
            flipper.Update(game.CurrentLevel, -1);
    }

    [Fact]
    public void DirectRimArrivalRunsFullDeathLifecycleAndRestartsUnresolvedEnemiesAsFreshEggs()
    {
        TempestGame game = CreateTankerWave();
        DestroyOriginalFlipperSlot(game, 1);
        Egg egg = FindOrReleaseOriginalEgg(game, 2)!;
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers, candidate => candidate.OriginalSlotId == 2);
        Egg[] oldEggs = game.Eggs.ToArray();
        Flipper[] oldFlippers = game.Flippers.ToArray();
        int score = game.Score.Score;
        int lives = game.Player.Lives;
        Assert.True(score > 0);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);

        ForceClimbToJustBelowRim(flipper, game);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathTicks);
        Assert.Equal(lives, game.Player.Lives);

        Step(game, 59);
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(lives, game.Player.Lives);
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(lives - 1, game.Player.Lives);

        Assert.Equal(score, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.DoesNotContain(game.Eggs, fresh => oldEggs.Contains(fresh));
        Assert.Equal(game.Eggs.Count, game.Eggs.Select(fresh => fresh.OriginalSlotId).Distinct().Count());
        Assert.DoesNotContain(game.Eggs, fresh => fresh.OriginalSlotId == 1);
        Assert.Contains(game.Eggs, fresh => fresh.OriginalSlotId == 0);
        Assert.All(game.Eggs, fresh =>
        {
            Assert.Equal(-10, fresh.Height);
            Assert.Equal(EggState.Roaming, fresh.State);
        });
        Assert.DoesNotContain(oldFlippers, game.Flippers.Contains);
        Assert.Equal(9, game.Eggs.Count + game.CurrentLevel.ReserveEggs);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        AssertTankerAccounting(game);
    }

    // --- Tanker shot/natural release, cargo ledger, death/restart and rendering regressions ---

    [Theory]
    [InlineData(7, 385, true)]
    [InlineData(7, 386, false)]
    [InlineData(6, 385, false)]
    public void TankerShotRequiresSameLaneAndStrictShotDepthBelowTanker(int shotLane, float shotHeight, bool expected)
    {
        Tanker tanker = CreatePendingTanker(7);
        Assert.Equal(expected, tanker.IsHitBy(new Shot { EdgeIndex = shotLane, Height = shotHeight }));
    }

    [Fact]
    public void TankerPlayerShotScores200AndReleasesExactlyTwoLiveCargoFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(2, game.TankerMaterializedCargoUnits);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo), flipper =>
        {
            Assert.Equal(FlipperState.Flipping, flipper.State);
            Assert.Equal(tanker.Height, flipper.Height);
        });
        Level shotLevel = game.CurrentLevel;
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.Equal(new[] { shotLevel.PreviousEdge(tanker.EdgeIndex), shotLevel.NextEdge(tanker.EdgeIndex) },
            cargo.Select(flipper => flipper.EdgeIndex));
        Assert.Equal(new[] {
                game.CurrentLevel.PreviousEdge(game.CurrentLevel.PreviousEdge(tanker.EdgeIndex)),
                game.CurrentLevel.NextEdge(game.CurrentLevel.NextEdge(tanker.EdgeIndex))
            },
            cargo.Select(flipper => flipper.DestinationEdge));
        Assert.Equal(2, cargo.Select(flipper => flipper.CargoUnitId).Distinct().Count());
        Assert.All(cargo, child => Assert.DoesNotContain(tanker.EdgeIndex,
            new[] { child.SourceEdge, child.DestinationEdge }));
        Assert.Empty(game.Spikers);
    }

    [Fact]
    public void TankerNaturalReleaseEscapesWithoutCarrierScoreAndCreatesTwoCargoFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        float releaseHeight = tanker.Height;
        game.RunOneStep();
        Assert.Empty(game.Tankers);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(releaseHeight, flipper.Height));

        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.NotSame(cargo[0], cargo[1]);
        Assert.NotEqual(cargo[0].CargoUnitId, cargo[1].CargoUnitId);
        Level level = game.CurrentLevel;
        int lane = tanker.EdgeIndex;
        Assert.Equal(new[] { level.PreviousEdge(lane), level.NextEdge(lane) },
            cargo.Select(child => child.EdgeIndex));
        Assert.Equal(new[]
        {
            level.PreviousEdge(level.PreviousEdge(lane)),
            level.NextEdge(level.NextEdge(lane))
        }, cargo.Select(child => child.DestinationEdge));
        foreach (Flipper child in cargo)
            for (int tick = 0; child.State == FlipperState.Flipping; tick++)
                child.Update(level, 0);

        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(new[]
        {
            level.PreviousEdge(level.PreviousEdge(lane)),
            level.NextEdge(level.NextEdge(lane))
        }, cargo.Select(child => child.EdgeIndex));
        Assert.True(cargo[0].IsHitBy(new Shot { EdgeIndex = cargo[0].EdgeIndex, Height = cargo[0].Height - 1 }));
        Assert.True(cargo[1].IsHitBy(new Shot { EdgeIndex = cargo[1].EdgeIndex, Height = cargo[1].Height - 1 }));
    }

    [Fact]
    public void TankerHitConsumesShotBeforeCargoCanBeHitByThatShot()
    {
        TempestGame game = CreateTankerWave();
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Assert.Empty(game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(0, game.TankerCargoUnitsResolved);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(tanker.Height, flipper.Height));
    }

    [Fact]
    public void PreexistingShotsInFormerTankerLaneCannotDestroyReleasedCargo()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot firstShot = Assert.Single(game.Shots);
        game.RunOneStep();
        game.RunOneStep();
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot[] activeShots = game.Shots.ToArray();
        Assert.Equal(2, activeShots.Length);
        Shot secondShot = Assert.Single(activeShots, shot => !ReferenceEquals(shot, firstShot));
        foreach (Shot shot in activeShots)
            shot.Height = tanker.Height + 5;

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Single(game.Shots);
        Assert.DoesNotContain(secondShot, game.Shots);
        Assert.Contains(firstShot, game.Shots);
        Assert.Equal(200, game.Score.Score);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.Equal(2, cargo.Length);
        Assert.Equal(2, cargo.Select(child => child.CargoUnitId).Distinct().Count());
        Assert.All(cargo, child => Assert.DoesNotContain(tanker.EdgeIndex,
            new[] { child.SourceEdge, child.DestinationEdge }));
        Assert.Equal(0, game.TankerCargoUnitsResolved);

        game.RunOneStep();

        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Contains(firstShot, game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(0, game.TankerCargoUnitsResolved);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);

        for (int tick = 0; game.Shots.Count > 0 && tick < 4; tick++)
        {
            game.RunOneStep();
            Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
            Assert.Equal(200, game.Score.Score);
            Assert.Equal(0, game.TankerCargoUnitsResolved);
        }

        Assert.Empty(game.Shots);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(0, game.TankerCargoUnitsResolved);
        Assert.Equal(12, game.TankerAccountingTotal);
    }

    [Fact]
    public void TankerAndCargoCanBothScoreWhenShotThenChildrenAreDestroyed()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Flipper cargo = game.Flippers.First(flipper => flipper.IsTankerCargo);
        Step(game, 1);
        game.Player.Move(cargo.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        shot = Assert.Single(game.Shots);
        shot.Height = cargo.Height - 5;
        game.RunOneStep();
        Assert.Equal(350, game.Score.Score);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
    }

    [Fact]
    public void TankerDeathBeforeReleaseRestoresOneStableOriginalSlotEggAndLatentCargo()
    {
        TempestGame game = CreateTankerWave();
        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Empty(game.Tankers);
        Assert.Contains(game.Eggs, egg => egg.OriginalSlotId == 0);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(0, game.TankerMaterializedCargoUnits);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void DeathWithThreeUnresolvedOriginalSlotsRebuildsOnlyFreshEggEntrants()
    {
        TempestGame game = CreateTankerWave(startingLives: 100);
        Tanker tanker = Assert.Single(game.Tankers);
        ShootTanker(game, tanker);
        foreach (Flipper cargo in game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray())
            ShootDevelopmentFlipper(game, cargo);

        for (int slotId = 1; slotId <= 6; slotId++)
            DestroyOriginalFlipperSlot(game, slotId);

        Egg survivingEgg = FindOrReleaseOriginalEgg(game, 7)!;
        MakeReady(survivingEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper survivor = Assert.Single(game.Flippers, flipper => flipper.OriginalSlotId == 7);
        Step(game, 15);
        Assert.InRange(survivor.Height, 2, Level.Depth - 1);
        Egg[] oldEggs = game.Eggs.ToArray();
        Flipper[] oldFlippers = game.Flippers.ToArray();
        int score = game.Score.Score;
        Assert.Equal(3, game.TankerOriginalSlotsRemaining);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(100, game.Player.Lives);

        ForceFlipperPivotIntoPlayer(game, survivor);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);

        Assert.Equal(99, game.Player.Lives);
        Assert.Equal(score, game.Score.Score);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Flippers);
        Assert.Equal(3, game.Eggs.Count);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(new[] { 7, 8, 9 }, game.Eggs.Select(egg => egg.OriginalSlotId).Order());
        Assert.Equal(7, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.All(game.Eggs, egg =>
        {
            Assert.DoesNotContain(egg, oldEggs);
            Assert.DoesNotContain(oldFlippers, flipper => flipper.EdgeIndex == egg.EdgeIndex &&
                Math.Abs(flipper.Height - egg.Height) < 1);
            Assert.Equal(-10, egg.Height);
            Assert.Equal(0, egg.UpdateCounter);
            Assert.Equal(EggState.Roaming, egg.State);
        });
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CargoEggIdentitySurvivesASecondDeathBeforeHatching()
    {
        TempestGame game = CreateTankerWave(startingLives: 5);
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        game.RunOneStep();
        Flipper cargo = game.Flippers.First(flipper => flipper.IsTankerCargo);
        ShootDevelopmentFlipper(game, cargo);
        Flipper survivor = game.Flippers.Single(flipper => flipper.IsTankerCargo);
        ForceFlipperPivotIntoPlayer(game, survivor);
        game.RunOneStep();
        Step(game, 60);

        Egg firstLifeCargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        int cargoId = firstLifeCargoEgg.CargoUnitId;
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        AssertTankerAccounting(game);

        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);

        Egg secondLifeCargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.NotSame(firstLifeCargoEgg, secondLifeCargoEgg);
        Assert.Equal(cargoId, secondLifeCargoEgg.CargoUnitId);
        Assert.Equal(-10, secondLifeCargoEgg.Height);
        Assert.Equal(EggState.Roaming, secondLifeCargoEgg.State);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CargoLifeResetRestoresSurvivingCargoWithoutReturningResolvedUnits()
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(2, [3, 2]));
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        game.RunOneStep();
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Empty(game.Tankers);

        Flipper cargoToDestroy = game.Flippers.First(flipper => flipper.IsTankerCargo);
        ShootDevelopmentFlipper(game, cargoToDestroy);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Flipper survivingCargo = Assert.Single(game.Flippers, flipper => flipper.IsTankerCargo);
        float formerHeight = survivingCargo.Height;
        int formerLane = survivingCargo.EdgeIndex;
        ForceFlipperPivotIntoPlayer(game, survivingCargo);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Empty(game.Flippers);
        Assert.Equal(0, game.ActiveTankerCargoFlippers);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        Egg cargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.Equal(-10, cargoEgg.Height);
        Assert.Equal(0, cargoEgg.UpdateCounter);
        Assert.Equal(EggState.Roaming, cargoEgg.State);
        Assert.NotEqual(formerLane, cargoEgg.EdgeIndex);
        Assert.NotEqual((int)formerHeight, (int)cargoEgg.Height);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.DoesNotContain(game.Eggs, egg => egg.OriginalSlotId == 0);
        Assert.Empty(game.Tankers);
        Assert.Equal(150, game.Score.Score);

        MakeReady(cargoEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper freshCargo = Assert.Single(game.Flippers, flipper => flipper.IsTankerCargo);
        Assert.NotSame(survivingCargo, freshCargo);
        Assert.Equal(FlipperState.Climbing, freshCargo.State);
        Assert.Equal(2, freshCargo.Height);
        Assert.Equal(survivingCargo.CargoUnitId, freshCargo.CargoUnitId);
        Assert.Equal(0, freshCargo.ShotDelayTicks);
        Assert.NotEqual((int)formerHeight, (int)freshCargo.Height);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerDevelopmentOriginalEggAndCargoAccountingRemainCoherent()
    {
        TempestGame game = CreateTankerWave();
        AssertTankerAccounting(game);
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        AssertTankerAccounting(game);
        Assert.False(game.LevelComplete);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
    }

    [Fact]
    public void MixedKilledAndRimClearedTankerCargoKeepDistinctLedgerOutcomes()
    {
        TempestGame game = CreateTankerWave(startingLives: 100);
        ShootTanker(game, Assert.Single(game.Tankers));
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(2, cargo.Length);
        ShootDevelopmentFlipper(game, cargo[0]);

        for (int slotId = 1; slotId < Level.EnemyTotal; slotId++)
            DestroyOriginalFlipperSlot(game, slotId);

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Flippers);
        Assert.Equal(10, game.TankerOriginalSlotsDestroyed + game.TankerOriginalSlotsEscaped);
        Assert.Equal(0, game.TankerOriginalSlotsRimCleared);
        Assert.Equal(1, game.TankerCargoUnitsRimCleared);
        Assert.Equal(1, game.TankerCargoUnitsKilled);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(1700, game.Score.Score);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CompleteTankerScenarioResolvesDynamicOriginalAndCargoLedgerThenReplays()
    {
        TempestGame game = new(startingLives: 100, tankerTest: true);
        int previousResolvedCargo = 0;
        for (int tick = 0; tick < 30000 && !game.LevelComplete && !game.GameOver; tick++)
        {
            Enemy? target = game.Tankers.Cast<Enemy>()
                .Concat(game.Flippers)
                .OrderByDescending(enemy => enemy.Height)
                .FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);

            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertTankerAccounting(game);
            Assert.True(game.TankerCargoUnitsResolved >= previousResolvedCargo);
            previousResolvedCargo = game.TankerCargoUnitsResolved;
        }

        Assert.True(game.LevelComplete,
            $"state={game.State}, lives={game.Player.Lives}, original={game.TankerOriginalSlotsRemaining}, " +
            $"cargo={game.TankerCargoUnitsRemaining}, activeTankers={game.Tankers.Count}, " +
            $"activeFlippers={game.Flippers.Count}, eggs={game.Eggs.Count}, reserve={game.CurrentLevel.ReserveEggs}");
        Assert.Equal(10, game.TankerOriginalSlotsResolved);
        Assert.Equal(10, game.TankerOriginalSlotsDestroyed + game.TankerOriginalSlotsEscaped);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);

        int score = game.Score.Score;
        int lives = game.Player.Lives;
        game.RunOneStep(restartPressed: true);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(score, game.Score.Score);
        Assert.Equal(lives, game.Player.Lives);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(0, game.TankerMaterializedCargoUnits);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerScenarioGameOverRestartRestoresInitialLedger()
    {
        TempestGame game = CreateTankerWave(startingLives: 1);
        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Step(game, 60);
        Assert.True(game.GameOver);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(1, game.Player.Lives);
    }

    [Fact]
    public void TankerProjectionDrawsStableFiniteMagentaGlyphAndCargoUsesFlipperRendering()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        VectorRenderer renderer = new(game);
        Vector2 expected = ProjectTanker(game, tanker);
        Assert.Equal(expected, renderer.ProjectTankerCenter(tanker));
        List<(Vector2 Start, Vector2 End)> first = [];
        List<(Vector2 Start, Vector2 End)> second = [];
        renderer.DrawTankers((start, end) => first.Add((start, end)));
        renderer.DrawTankers((start, end) => second.Add((start, end)));
        Assert.Equal(16, first.Count);
        Assert.Equal(first, second);
        foreach ((Vector2 start, Vector2 end) in first)
        {
            Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
            Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
        }

        TankerCargoSpawn[] cargo = tanker.ReleaseFlipperCargo(game.CurrentLevel, 1, TankerReleaseKind.Natural).Cargo.ToArray();
        Flipper released = Flipper.CreateTankerCargo(game.CurrentLevel, cargo[0], 0);
        Assert.Equal(FlipperState.Flipping, released.State);
    }

    [Fact]
    public void TankerScenarioFlagsAreIndependentAndMutuallyExclusiveWithSpikerMode()
    {
        string[] tankerArgs = ["--tanker-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseTankerTest(tankerArgs));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(tankerArgs));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(tankerArgs));
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(
            ["--tanker-test", "--enemy-bullets-test"]));
        Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(
            ["--tanker-test", "--spiker-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, tankerTest: true));

        TempestGame normal = new();
        Assert.False(normal.TankerTest);
        Assert.False(normal.CurrentLevel.TankerDevelopmentWave);
        Assert.Empty(normal.Tankers);
        Assert.Empty(normal.CurrentLevel.Spikes);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.TankerTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);
        TempestGame tanker = new(startingLives: 10, tankerTest: true);
        Assert.True(tanker.TankerTest);
        Assert.False(tanker.CurrentLevel.SpikerDevelopmentWave);
        Assert.Equal(10, tanker.Player.Lives);
        Assert.Equal(4, tanker.Eggs.Count);
        Assert.Equal(6, tanker.CurrentLevel.ReserveEggs);
        Assert.Empty(tanker.Tankers);
        Assert.Equal(10, tanker.TankerOriginalSlotsRemaining);
    }

    [Fact]
    public void TankerStartsAtHeightTenOnStableOriginalEggLaneAndCarriesTwoLatentFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(0, tanker.OriginalSlotId);
        Assert.InRange(tanker.EdgeIndex, 0, 15);
        Assert.Equal(11.75f, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        Assert.Empty(game.Spikers);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(1, game.ActiveTankerOriginalEnemies);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.False(tanker.HitsPlayer(game.Player));
    }

    [Fact]
    public void TankerMovesAtFixedSpeedOnItsLaneAndDoesNotReleaseAt385()
    {
        Tanker tanker = new(4, 0, climbSpeed: 1);
        Assert.Equal(10, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        for (int tick = 0; tick < 375; tick++)
            Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(385, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease, tanker.UpdateForDevelopment());
        Assert.Equal(386, tanker.Height);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
        Assert.Equal(4, tanker.EdgeIndex);
    }

    [Fact]
    public void TankerUsesEarlyReferenceTierSpeedAndCannotDirectlyHitPlayer()
    {
        Tanker tanker = new(7, 0);
        Assert.Equal(1.75f, tanker.ClimbSpeed);
        Assert.False(tanker.HitsPlayer(new Player()));
        Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(11.75f, tanker.Height);
        Assert.Equal(7, tanker.EdgeIndex);
    }


    [Fact]
    public void TankerRendererUsesFiniteMagentaGlyphAtProjectedLaneDepth()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        VectorRenderer renderer = new(game);
        Vector2 expected = Projection.Project(
            (game.CurrentLevel.Definition.Points[tanker.EdgeIndex].X +
             game.CurrentLevel.Definition.Points[(tanker.EdgeIndex + 1) % 16].X) / 2,
            (game.CurrentLevel.Definition.Points[tanker.EdgeIndex].Y +
             game.CurrentLevel.Definition.Points[(tanker.EdgeIndex + 1) % 16].Y) / 2,
            Level.Position + Level.Depth - tanker.Height, 400, -80);
        Assert.Equal(expected, renderer.ProjectTankerCenter(tanker));
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTankers((start, end) => lines.Add((start, end)));
        Assert.Equal(16, lines.Count);
        Assert.All(lines, line =>
        {
            Assert.True(float.IsFinite(line.Start.X) && float.IsFinite(line.Start.Y));
            Assert.True(float.IsFinite(line.End.X) && float.IsFinite(line.End.Y));
        });
    }

    // --- Flipper enemy-bullet behavior ---

    [Fact]
    public void EnemyBulletTestFlagIsIndependentFromProductionFiring()
    {
        Assert.False(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest([]));
        string[] bulletArguments = ["--enemy-bullets-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(bulletArguments));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(bulletArguments));
        string[] combinedArguments = ["--spiker-test", "--enemy-bullets-test", "--lives", "10"];
        TempestGame combined = new(
            Tempest.Game.Debug.DebugController.UseSpikerWave(combinedArguments),
            startingLives: Tempest.Game.Debug.DebugController.GetStartingLives(combinedArguments),
            enemyBulletsTest: Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(combinedArguments));
        Assert.True(combined.CurrentLevel.SpikerDevelopmentWave);
        Assert.True(combined.EnemyBulletsTest);
        Assert.Equal(10, combined.Player.Lives);

        TempestGame normal = new();
        Assert.False(normal.EnemyBulletsTest);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.EnemyBulletsTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);

        Flipper ordinary = new(2, 0);
        Assert.Equal(0, ordinary.ShotDelayTicks);
    }

    [Fact]
    public void NewFlipperStartsReadyForTheSeparateArcadeFiringPhase()
    {
        Flipper flipper = new(4);
        Assert.Equal(0, flipper.ShotDelayTicks);
        flipper.Update(new Level(), 7);
        Assert.Equal(0, flipper.ShotDelayTicks);
    }

    [Fact]
    public void EnemyBulletAdvancesThreeAndExpiresOnlyAfterPassingDepth()
    {
        EnemyBullet bullet = new(3, 394);
        bullet.Advance();
        Assert.Equal(397, bullet.Height);
        Assert.False(bullet.IsExpired);
        Assert.Equal(15, bullet.Rotation);
        bullet = new EnemyBullet(3, 397);
        bullet.Advance();
        Assert.Equal(400, bullet.Height);
        Assert.False(bullet.IsExpired);
        bullet.Advance();
        Assert.Equal(403, bullet.Height);
        Assert.True(bullet.IsExpired);
    }

    [Theory]
    [InlineData(392, false)]
    [InlineData(393, true)]
    [InlineData(400, true)]
    public void EnemyBulletPlayerHitUsesSameLaneAndStrictEightUnitBoundary(float height, bool expected)
    {
        Player player = new();
        Assert.Equal(expected, new EnemyBullet(7, height).HitsPlayer(player));
        Assert.False(new EnemyBullet(6, height).HitsPlayer(player));
    }

    [Theory]
    [InlineData(99, 100, true)]
    [InlineData(100, 100, false)]
    [InlineData(101, 100, false)]
    public void PlayerShotHitsEnemyBulletOnlyBelowItOnTheSameLane(float shotDepth, float bulletDepth, bool expected)
    {
        EnemyBullet bullet = new(8, bulletDepth);
        Assert.Equal(expected, bullet.IsHitBy(new Shot { EdgeIndex = 8, Height = shotDepth }));
        Assert.False(bullet.IsHitBy(new Shot { EdgeIndex = 9, Height = shotDepth }));
    }

    [Fact]
    public void EnemyBulletFlagUsesTheSeparatePhaseAndCreatesBulletAtLaneAndHeight()
    {
        TempestGame game = new(enemyBulletsTest: true, random: new BulletRandom(1, [76]));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(flipper.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(flipper.Height, bullet.Height);
        Assert.Equal(2, bullet.Height);
        Assert.Equal(80, flipper.ShotDelayTicks);
    }

    [Fact]
    public void ProductionFlipperFiresImmediatelyThenReloadsArcadeLevelOneHoldoff()
    {
        BulletRandom random = new(2, [76]);
        TempestGame game = new(random: random);
        Assert.False(game.EnemyBulletsTest);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();

        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(80, flipper.ShotDelayTicks);
        Assert.Single(game.EnemyBullets);
        Assert.Equal(new[] { (0, 256) }, random.Calls);

        Step(game, 16);
        Assert.Single(game.EnemyBullets);
        Assert.Equal(64, flipper.ShotDelayTicks);
        Assert.Equal(new[] { (0, 256) }, random.Calls);
    }

    [Fact]
    public void PlayerCanShootEnemyBulletForTwentyFiveWithoutEnemyAccountingChange()
    {
        TempestGame game = PrepareProductionBulletGame(1, 76);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 80 && game.EnemyBullets.Count > 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(25, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void ProductionPlayerShotCanDestroyFlipperBulletWithoutEnemyBulletFlag()
    {
        TempestGame game = PrepareProductionBulletGame(1, 76);
        Flipper flipper = Assert.Single(game.Flippers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Step(game, 40);
        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex, 16);

        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = bullet.Height - 15;
        game.RunOneStep();

        Assert.Empty(game.EnemyBullets);
        Assert.Contains(flipper, game.Flippers);
        Assert.Equal(25, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void ProductionLifeResetClearsBulletsAndRearmsFreshFlipper()
    {
        TempestGame game = new(random: new BulletRandom(1, [76, 1]));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper previous = Assert.Single(game.Flippers);
        Step(game, 16);
        Assert.Single(game.EnemyBullets);

        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();
        Assert.Empty(game.EnemyBullets);
        Assert.DoesNotContain(previous, game.Flippers);

        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper fresh = Assert.Single(game.Flippers);
        Assert.NotSame(previous, fresh);
        Assert.Equal(80, fresh.ShotDelayTicks);
    }

    [Fact]
    public void FlipperAndBulletCollisionPriorityUsesDevelopmentInsertionOrder()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        Flipper flipper = Assert.Single(game.Flippers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height + 5;
        game.RunOneStep();
        Assert.Empty(game.Flippers);
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void EnemyBulletPlayerHitUsesNormalDeathAndSingleLifeLoss()
    {
        TempestGame game = CreateGameWithFourReadyFlippers();
        EnemyBullet firstBullet = game.EnemyBullets[0];
        game.Player.Move(firstBullet.EdgeIndex - game.Player.EdgeIndex, 16);
        int ticks = 0;
        while (game.State == LevelState.Playing && ticks++ < 150)
            game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.Single(game.EnemyBullets);
        game.Score.Add(125);
        Step(game, 59);
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);

        Assert.Equal(125, game.Score.Score);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);

        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void EnemyBulletsDoNotBlockCompletionAndAreClearedBeforeExit()
    {
        TempestGame game = PrepareProductionBulletGame(1, 76);
        Flipper flipper = Assert.Single(game.Flippers);

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height - 5;
        game.RunOneStep();
        Assert.Contains(bullet, game.EnemyBullets);
        for (int index = 0; index < 9; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ReturnSurvivorsToReserve();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.EnemyBullets);

        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]

    public void SpikerOnlyScenarioRemainsSilentWithoutEnemyBulletFlag()
    {
        TempestGame spikerOnly = new(true, new SpikerRandom { ChooseSpikers = false });
        MakeReady(spikerOnly.Eggs[0], spikerOnly.CurrentLevel);
        spikerOnly.RunOneStep();
        Assert.Equal(0, Assert.Single(spikerOnly.Flippers).ShotDelayTicks);
        Assert.False(spikerOnly.EnemyBulletsTest);
    }

    [Fact]
    public void EnemyBulletRenderUsesFiniteLaneDepthAndEightDistinctRadials()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        VectorRenderer renderer = new(game);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        List<(Vector2 Start, Vector2 End)> lines = [];

        renderer.DrawEnemyBullets((start, end) => lines.Add((start, end)));
        Assert.Equal(8, lines.Count);
        foreach ((Vector2 start, Vector2 end) in lines)
        {
            Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
            Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
        }
        float projectedDepth = Level.Position + Level.Depth - bullet.Height;
        float expectedDiameter = 12 / ((projectedDepth + Projection.ZOffset) / Projection.K);
        Assert.InRange(Vector2.Distance(lines[0].Start, lines[0].End),
            expectedDiameter - 0.001f, expectedDiameter + 0.001f);
        Assert.Equal(2, bullet.Height);
        Assert.InRange(renderer.ProjectEnemyBullet(bullet).X, 0, 800);
        Assert.InRange(renderer.ProjectEnemyBullet(bullet).Y, 0, 600);
    }

    // --- Configurable starting lives (--lives) ---

    [Fact]
    public void StartingLivesDefaultRemainsThree()
    {
        Assert.Equal(3, Player.DefaultStartingLives);
        Assert.Equal(3, new Player().Lives);
        Assert.Equal(3, new TempestGame().Player.Lives);
        Assert.Equal(3, Tempest.Game.Debug.DebugController.GetStartingLives([]));
    }

    [Fact]
    public void LivesCommandLineOptionWorksAloneAndWithSpikerWave()
    {
        string[] livesArguments = ["--lives", "10"];
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(livesArguments));
        Assert.Equal(10, new TempestGame(startingLives:
            Tempest.Game.Debug.DebugController.GetStartingLives(livesArguments)).Player.Lives);

        string[] combinedArguments = ["--spiker-test", "--lives", "20"];
        TempestGame game = new(
            Tempest.Game.Debug.DebugController.UseSpikerWave(combinedArguments),
            startingLives: Tempest.Game.Debug.DebugController.GetStartingLives(combinedArguments));
        Assert.Equal(20, game.Player.Lives);
        Assert.True(game.CurrentLevel.SpikerDevelopmentWave);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("0")]
    [InlineData("-3")]
    [InlineData("999999999999999999")]
    public void InvalidLivesCommandLineValuesFallBackToThree(string value)
    {
        string[] arguments = ["--lives", value];
        Assert.Equal(Player.DefaultStartingLives,
            Tempest.Game.Debug.DebugController.GetStartingLives(arguments));
        Assert.Equal(Player.DefaultStartingLives,
            new TempestGame(startingLives:
                Tempest.Game.Debug.DebugController.GetStartingLives(arguments)).Player.Lives);
    }

    [Fact]
    public void MissingLivesValueFallsBackToThree()
    {
        string[] arguments = ["--lives"];
        Assert.Equal(Player.DefaultStartingLives,
            Tempest.Game.Debug.DebugController.GetStartingLives(arguments));
        Assert.Equal(Player.DefaultStartingLives,
            new TempestGame(startingLives:
                Tempest.Game.Debug.DebugController.GetStartingLives(arguments)).Player.Lives);
    }

    [Fact]
    public void GameOverRestartRestoresSelectedStartingLives()
    {
        TempestGame game = PreparePivotWithStartingLives(10);
        game.Player.Lives = 1;
        Step(game, GetFlipperPivotTicks(game) + TempestGame.PlayerDeathTicks);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
    }

    [Fact]
    public void LifeLossStillSubtractsExactlyOneFromSelectedStartingLives()
    {
        TempestGame game = PreparePivotWithStartingLives(10);
        game.Score.Add(125);
        Step(game, GetFlipperPivotTicks(game) + TempestGame.PlayerDeathTicks);
        Assert.False(game.GameOver);

        Assert.Equal(9, game.Player.Lives);
        Assert.Equal(125, game.Score.Score);
        Assert.Equal(LevelState.Playing, game.State);
    }

    // --- Spiker/spike development scenario (--spiker-test) and spike-exit regressions ---

    [Fact]
    public void FullDevelopmentWaveCanBeDefeatedAndExitedWithEnemyAccountingIntact()
    {
        SpikerRandom random = new();
        TempestGame game = new(true, random);
        bool sawSpiker = false;
        bool sawFlipper = false;

        for (int tick = 0; tick < 12000 && !game.GameOver && !game.LevelComplete; tick++)
        {
            sawSpiker |= game.Spikers.Count > 0;
            sawFlipper |= game.Flippers.Count > 0;
            if (sawSpiker)
                random.ChooseSpikers = false;
            Enemy? target = game.Flippers.Cast<Enemy>().Concat(game.Spikers)
                .OrderByDescending(enemy => enemy.Height).FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);
            else if (game.IsExiting)
            {
                int safeLane = Enumerable.Range(0, 16)
                    .OrderBy(lane => game.Spikes.FirstOrDefault(spike => spike.EdgeIndex == lane)?.Height ?? 0)
                    .First();
                game.Player.Move(safeLane - game.Player.EdgeIndex, 16);
            }
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertSpikerAccounting(game);
            Assert.True(game.Score.Score >= game.CurrentLevel.EnemiesDestroyed * 50);
        }
        Assert.True(sawSpiker);
        Assert.True(sawFlipper);
        Assert.True(game.LevelComplete,
            $"State={game.State}, lives={game.Player.Lives}, destroyed={game.CurrentLevel.EnemiesDestroyed}, " +
            $"reserve={game.CurrentLevel.ReserveEggs}, eggs={game.Eggs.Count}, flippers={game.Flippers.Count}, spikers={game.Spikers.Count}");
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void SpikerDevelopmentSelectionIsExplicitAndDefaultLevelOneIsUnchanged()
    {
        Assert.False(Tempest.Game.Debug.DebugController.UseSpikerWave([]));
        Assert.True(Tempest.Game.Debug.DebugController.UseSpikerWave(["--spiker-test"]));
        TempestGame normal = new();
        Assert.Empty(normal.Spikes);
        Assert.Empty(normal.Spikers);
        TempestGame development = new(true, new SpikerRandom());
        Assert.Equal(16, development.Spikes.Count);
        Assert.All(development.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(4, development.Eggs.Count);
        Assert.Equal(6, development.CurrentLevel.ReserveEggs);
        AssertSpikerAccounting(development);
    }

    [Fact]
    public void SpikerHatchesOnEggLaneWithNoMovementOnFirstUpdate()
    {
        TempestGame game = new(true, new SpikerRandom());
        Egg egg = game.Eggs[0];
        int lane = egg.EdgeIndex;
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        Assert.Equal(lane, spiker.EdgeIndex);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(SpikerState.Building, spiker.State);
        game.RunOneStep();
        Assert.Equal(3, spiker.Height);
        Assert.Equal(lane, spiker.EdgeIndex);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void SpikerInitialUpdateDoesNotCreateSpikeAndNextUpdateCreatesAtOwnHeight()
    {
        Level level = new();
        Spiker spiker = new(3, 5);
        Assert.Equal(SpikerState.Initializing, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(0, spiker.Height);
        Assert.Empty(level.Spikes);
        spiker.Update(level, () => false);
        Spike spike = Assert.Single(level.Spikes);
        Assert.Equal(3, spike.EdgeIndex);
        Assert.Equal(5, spike.Height);
        spiker.Update(level, () => false);
        Assert.Same(spike, Assert.Single(level.Spikes));
        Assert.Equal(10, spiker.Height);
        Assert.Equal(6, spike.Height);
    }

    [Fact]
    public void SpikerExistingSpikeAssignmentCanShortenAndDoesNotAlwaysGrow()
    {
        Level level = new();
        Spike spike = level.CreateSpike(2, 60);
        Spiker spiker = new(2, 3);
        for (int tick = 0; tick < 21; tick++)
            spiker.Update(level, () => false);
        Assert.Equal(60, spiker.Height);
        Assert.Equal(60, spike.Height);
        spiker.Update(level, () => false);
        Assert.Same(spike, Assert.Single(level.Spikes));
        Assert.Equal(63, spiker.Height);
        Assert.Equal(59, spike.Height);
        Assert.Equal(3, spiker.Growth);
    }

    [Theory]
    [InlineData(true, SpikerState.Retreating)]
    [InlineData(false, SpikerState.Building)]
    public void SpikerRetreatDecisionOccursOnlyAfterMoreThanOneHundredGrowth(bool retreat, SpikerState expected)
    {
        Level level = new();
        level.CreateSpike(0, 0);
        Spiker spiker = new(0, 5);
        spiker.Update(level, () => throw new InvalidOperationException());
        for (int tick = 0; tick < 20; tick++)
            spiker.Update(level, () => throw new InvalidOperationException());
        Assert.Equal(100, spiker.Growth);
        int decisions = 0;
        spiker.Update(level, () => { decisions++; return retreat; });
        Assert.Equal(1, decisions);
        Assert.Equal(105, spiker.Growth);
        Assert.Equal(expected, spiker.State);
    }

    [Theory]
    [InlineData(70, false)]
    [InlineData(71, true)]
    public void SpikerFiresAtMostOnceOnlyOnSuccessfulGrowthRetreatDecision(int decision, bool fires)
    {
        Level level = new();
        level.CreateSpike(4, 0);
        Spiker spiker = new(4, 5);
        spiker.Update(level, () => false);
        for (int tick = 0; tick < 20; tick++)
            spiker.Update(level, () => false);

        int firingDecisions = 0;
        EnemyBullet? bullet = spiker.Update(level, () => true, (minimum, maximum) =>
        {
            Assert.Equal(0, minimum);
            Assert.Equal(100, maximum);
            firingDecisions++;
            return decision;
        });

        Assert.Equal(SpikerState.Retreating, spiker.State);
        Assert.Equal(1, firingDecisions);
        if (fires)
        {
            Assert.NotNull(bullet);
            Assert.Equal(4, bullet!.EdgeIndex);
            Assert.Equal(105, bullet.Height);
        }
        else
        {
            Assert.Null(bullet);
        }

        for (int tick = 0; tick < 10; tick++)
            Assert.Null(spiker.Update(level, () => throw new InvalidOperationException(),
                (_, _) => throw new InvalidOperationException()));
        Assert.Equal(1, firingDecisions);
    }

    [Fact]
    public void SpikerTestAloneStaysSilentAtRandomRetreat()
    {
        (TempestGame game, SpikerBulletModeRandom random) =
            PrepareSpikerRetreatTransition(decision: 100, enemyBulletsTest: false);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, random.FiringDecisionDraws);
    }

    [Fact]
    public void CombinedSpikerAndEnemyBulletFlagsRegisterOneIndependentRetreatBullet()
    {
        (TempestGame game, SpikerBulletModeRandom random) =
            PrepareSpikerRetreatTransition(decision: 71, enemyBulletsTest: true);
        Spiker spiker = Assert.Single(game.Spikers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(spiker.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(spiker.Height, bullet.Height);
        Assert.Equal(1, random.FiringDecisionDraws);

        float creationHeight = bullet.Height;
        game.RunOneStep();
        Assert.Equal(creationHeight + 3, bullet.Height);

        for (int tick = 0; tick < 80 && game.Spikers.Contains(spiker); tick++)
            game.RunOneStep();
        Assert.DoesNotContain(spiker, game.Spikers);
        Assert.Contains(bullet, game.EnemyBullets);

        int destroyedBeforeShot = game.CurrentLevel.EnemiesDestroyed;
        int remainingBeforeShot = game.CurrentLevel.RemainingEnemies;
        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = bullet.Height - 7;
        game.RunOneStep();
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(25, game.Score.Score);
        Assert.Equal(destroyedBeforeShot, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(remainingBeforeShot, game.CurrentLevel.RemainingEnemies);
    }

    [Fact]
    public void SpikerRetreatBulletUsesExistingLifeResetCleanup()
    {
        TempestGame game = PrepareSpikerRetreatTransition(decision: 71, enemyBulletsTest: true).Game;
        Assert.Single(game.EnemyBullets);
        // Enter the established reset boundary directly so the Spiker-created bullet is still active.
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);
        Assert.Empty(game.EnemyBullets);
    }

    [Fact]
    public void SpikerUpperThresholdRetreatsOnFollowingUpdateAndRemovesAfterCrossingZero()
    {
        Level level = new();
        Spiker spiker = new(1, 3);
        spiker.Update(level, () => false);
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(level, () => false);
        Assert.Equal(396, spiker.Height);
        Assert.Equal(SpikerState.Building, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(396, spiker.Height);
        Assert.Equal(SpikerState.Retreating, spiker.State);
        Spike spike = Assert.Single(level.Spikes);
        float spikeHeight = spike.Height;
        spiker.Update(level, () => false);
        Assert.InRange(spiker.Height, 393.299f, 393.301f);
        while (spiker.Height > 0)
            spiker.Update(level, () => false);
        Assert.Equal(SpikerState.Retreating, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(SpikerState.ReturnedToReserve, spiker.State);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(spikeHeight, spike.Height);
    }

    [Fact]
    public void SpikerRuntimeRetreatReturnsExactlyOneReserveEnemyAndPreservesSpike()
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = Assert.Single(game.Spikers);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        for (int tick = 0; tick < 133; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        while (spiker.Height > 0)
            spiker.Update(game.CurrentLevel, () => false);
        int reserve = game.CurrentLevel.ReserveEggs;
        game.RunOneStep();
        Assert.Empty(game.Spikers);
        Assert.Equal(reserve + 1, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Contains(spike, game.Spikes);
        AssertSpikerAccounting(game);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, true)]
    [InlineData(1, false)]
    public void SpikerAndSpikeUseSameLaneStrictDepthComparisons(float difference, bool expected)
    {
        Level level = new();
        Spiker spiker = new(4, 3);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        Spike spike = Assert.Single(level.Spikes);
        Shot shot = new() { EdgeIndex = 4, Height = spiker.Height + difference };
        Assert.Equal(expected, spiker.IsHitBy(shot));
        Assert.Equal(expected, spike.IsHitBy(shot));
        Shot wrongLane = new() { EdgeIndex = 5, Height = 0 };
        Assert.False(spiker.IsHitBy(wrongLane));
        Assert.False(spike.IsHitBy(wrongLane));
    }

    [Fact]
    public void SpikerShotConsumesShotAwardsFiftyAndKeepsSpike()
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = game.Spikers[0];
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 60;
        game.Player.Move(spiker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikers);
        Assert.Equal(50, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(60, spike.Height);
        AssertSpikerAccounting(game);
    }

    [Theory]
    [InlineData(400, 390, false)]
    [InlineData(10, 0, true)]
    [InlineData(5, -5, true)]
    public void SpikeShotShortensExactlyTenAwardsOneAndDeletesWhenDepleted(float height, float expected, bool deleted)
    {
        TempestGame game = new(true, new SpikerRandom());
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = height;
        float initialDepth = height > 394 ? 400 : height + 5;
        game.Player.AdvanceExit(400 - initialDepth);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(expected, spike.Height);
        Assert.Equal(1, game.Score.Score);
        Assert.Empty(game.Shots);
        Assert.Equal(deleted, !game.Spikes.Contains(spike));
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SpikerVersusSpikePriorityFollowsInsertionNotProximity(bool replacement)
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = game.Spikers[0];
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        Spike initial = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        initial.Height = replacement ? 1 : 395;
        game.Player.Move(spiker.EdgeIndex - game.Player.EdgeIndex, 16);
        if (replacement)
        {
            // Deplete the initial spike through production shots, then recreate after the Spiker.
            game.Player.AdvanceExit(394);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            game.Player.ResetDepth();
            game.CurrentLevel.CreateSpike(spiker.EdgeIndex, 395);
            Step(game, 2);
            game.Player.AdvanceExit(400 - (spiker.Height + 5));
        }
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.Shots);
        if (replacement)
        {
            Assert.Empty(game.Spikers);
            Assert.Equal(51, game.Score.Score);
        }
        else
        {
            Assert.Single(game.Spikers);
            Assert.Equal(1, game.Score.Score);
            Assert.Equal(385, initial.Height);
        }
    }

    [Fact]
    public void DevelopmentWaveCanContainBothTypesAndKeepsAccountingAcrossDeath()
    {
        SpikerRandom random = new();
        TempestGame game = new(true, random);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        random.ChooseSpikers = false;
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Spikers);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
        Spike spike = game.Spikes[0];
        spike.Height = 100;
        game.Player.Move(spike.EdgeIndex - game.Player.EdgeIndex, 16);
        game.Player.AdvanceExit(350);
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
        AssertSpikerAccounting(game);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(100, spike.Height);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void ActiveSpikerRetractsWhileItsPersistentSpikeStaysFixedDuringPlayerDeath()
    {
        SpikerRandom random = new();
        TempestGame game = new(true, random);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        random.ChooseSpikers = false;
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        Assert.Single(game.Flippers);
        Spike spike = game.Spikes.First(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        game.Player.Move(game.Flippers[0].EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 500 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);

        float spikeHeight = spike.Height;
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End)> beforeRetraction = [];
        renderer.DrawSpikers((start, end) => beforeRetraction.Add((start, end)));
        List<(Vector2 Start, Vector2 End)> spikeLines = [];
        renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), _ => { });
        float spikerHeight = spiker.Height;
        SpikerState spikerState = spiker.State;

        game.RunOneStep();

        List<(Vector2 Start, Vector2 End)> duringRetraction = [];
        renderer.DrawSpikers((start, end) => duringRetraction.Add((start, end)));
        List<(Vector2 Start, Vector2 End)> spikesDuringRetraction = [];
        renderer.DrawSpikes((start, end) => spikesDuringRetraction.Add((start, end)), _ => { });
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.NotEqual(beforeRetraction[0].Start, duringRetraction[0].Start);
        Assert.Equal(spikerHeight, spiker.Height);
        Assert.Equal(spikerState, spiker.State);
        Assert.Equal(spikeHeight, spike.Height);
        Assert.Equal(spikeLines, spikesDuringRetraction);
    }

    [Fact]
    public void DevelopmentLastEnemiesAreFlippersSoRetreatCannotPreventCompletion()
    {
        TempestGame game = new(true, new SpikerRandom());
        for (int index = 0; index < 8; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Empty(game.Spikers);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void ResidualSpikesAllowClearAndAvoidanceLastsOneHundredFiftyOneUpdates()
    {
        TempestGame game = BeginSpikerExit();
        Assert.Equal(16, game.Spikes.Count);
        Step(game, 150);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Step(game, 20);
        Assert.Equal(LevelState.ExitDelay, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void SpikerDiagnosticRetainsFiringDuringItsAvoidanceInterval()
    {
        TempestGame game = BeginSpikerExit();

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Single(game.Shots);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SpikeExitAllowsMovementAndShotsFromTravelDepth(bool zoom)
    {
        TempestGame game = BeginSpikerExit();
        if (zoom)
            Step(game, 151 + 21 + 10);
        float depth = game.Player.Depth;
        int lane = game.Player.EdgeIndex;
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.True(game.SpikeExitControlsEnabled);
    }

    [Fact]
    public void SpikeTravelCollisionSweepsDepthAndLaneAndCostsOneLifeBeforeRetry()
    {
        TempestGame game = BeginSpikerExit();
        Step(game, 151 + 21);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 397;
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(397, game.Player.Depth);
        Assert.Equal(60, game.DeathTicks);
        Step(game, 59);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(1, game.DeathTicks);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(400, game.Player.Depth);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(397, spike.Height);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        AssertSpikerAccounting(game);
        game.Player.Move(1, 16);
        foreach (Spike other in game.Spikes.Where(other => other != spike))
            other.Height = 0;
        for (int tick = 0; tick < 400 && !game.LevelComplete; tick++)
            game.RunOneStep();
        Assert.True(game.LevelComplete);
        Assert.Equal(2, game.Player.Lives);
    }

    [Fact]
    public void SpikeReverseZoomKeepsDeathDepthUntilOffsetRestoresAndHandlesGameOver()
    {
        TempestGame game = BeginSpikerExit();
        game.Player.Lives = 1;
        Step(game, 151 + 21 + 100);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = game.Player.Depth;
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        float depth = game.Player.Depth;
        Step(game, 46);
        Assert.Equal(depth, game.Player.Depth);
        game.RunOneStep();
        Assert.Equal(400, game.Player.Depth);
        Step(game, 13);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Step(game, 100);
        Assert.Equal(0, game.Player.Lives);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(3, game.Player.Lives);
        Assert.All(game.Spikes, candidate => Assert.Equal(60, candidate.Height));
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void DevelopmentReplayRestoresInitialSpikesAndEggsButPreservesScoreAndLives()
    {
        TempestGame game = BeginSpikerExit();
        foreach (Spike spike in game.Spikes)
            spike.Height = 0;
        game.Score.Add(501);
        game.Player.Lives = 2;
        Step(game, 400);
        Assert.True(game.LevelComplete);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(501, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Player.Depth);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void SpikerAndSpikeRenderSubmissionsAreFiniteRotateAndPersistDuringExit()
    {
        TempestGame game = HatchDevelopmentSpiker();
        VectorRenderer renderer = new(game);
        List<(Vector2, Vector2)> first = [];
        renderer.DrawSpikers((start, end) => first.Add((start, end)));
        Assert.Equal(14, first.Count);
        game.RunOneStep();
        List<(Vector2, Vector2)> second = [];
        renderer.DrawSpikers((start, end) => second.Add((start, end)));
        Assert.NotEqual(first[1], second[1]);
        foreach ((Vector2 start, Vector2 end) in first.Concat(second))
        {
            AssertFinite(start);
            AssertFinite(end);
        }
        game = BeginSpikerExit();
        renderer = new VectorRenderer(game);
        for (int tick = 0; tick < 400 && !game.GameOver; tick++)
        {
            int lines = 0;
            int tips = 0;
            renderer.DrawSpikes((start, end) => { AssertFinite(start); AssertFinite(end); lines++; },
                tip => { AssertFinite(tip); tips++; });
            Assert.Equal(16, lines);
            Assert.Equal(16, tips);
            game.RunOneStep();
        }
    }

    // --- Shared helpers and deterministic Random test doubles ---

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private static TempestGame CreateTankerWave(
        int startingLives = 3, bool enemyBulletsTest = false, Random? random = null)
    {
        TempestGame game = new(startingLives: startingLives, random: random,
            enemyBulletsTest: enemyBulletsTest, tankerTest: true);
        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Tankers);
        return game;
    }

    private static Tanker CreatePendingTanker(int lane)
    {
        Tanker tanker = new(lane, 0, climbSpeed: 1);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        Assert.Equal(386, tanker.Height);
        return tanker;
    }

    private static Egg? FindOrReleaseOriginalEgg(TempestGame game, int slotId)
    {
        for (int tick = 0; tick < 5000; tick++)
        {
            Egg? egg = game.Eggs.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            if (egg is not null)
                return egg;
            game.RunOneStep();
        }
        return null;
    }

    private static void DestroyOriginalFlipperSlot(TempestGame game, int slotId)
    {
        int remainingBefore = game.TankerOriginalSlotsRemaining;
        for (int attempt = 0; attempt < 4 && game.TankerOriginalSlotsRemaining == remainingBefore; attempt++)
        {
            Flipper? flipper = game.Flippers.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            if (flipper is null)
            {
                Egg? egg = FindOrReleaseOriginalEgg(game, slotId);
                Assert.NotNull(egg);
                MakeReady(egg!, game.CurrentLevel);
                game.RunOneStep();
                flipper = game.Flippers.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            }
            if (flipper is not null)
                ShootDevelopmentFlipper(game, flipper);
        }
        Assert.Equal(remainingBefore - 1, game.TankerOriginalSlotsRemaining);
    }

    private static void ShootTanker(TempestGame game, Tanker tanker)
    {
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Assert.DoesNotContain(tanker, game.Tankers);
    }

    private static Flipper HatchOneOrdinaryTankerWaveFlipper(TempestGame game)
    {
        Egg egg = Assert.Single(game.Eggs, candidate => candidate.OriginalSlotId == 1);
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        return Assert.Single(game.Flippers, flipper => !flipper.IsTankerCargo);
    }

    private static void ForceFlipperPivotIntoPlayer(TempestGame game, Flipper flipper)
    {
        int ticks = 0;
        while (!flipper.CompletedIntoPlayer && ticks++ < 2000)
        {
            if (flipper.State == FlipperState.Flipping)
                game.Player.Move(flipper.DestinationEdge - game.Player.EdgeIndex, 16);
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        }
        Assert.True(flipper.CompletedIntoPlayer);
    }

    private static void ShootDevelopmentFlipper(TempestGame game, Flipper flipper)
    {
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height + 5;
        game.RunOneStep();
        Assert.DoesNotContain(flipper, game.Flippers);
    }

    private static void AssertTankerAccounting(TempestGame game)
    {
        Assert.Equal(Level.EnemyTotal, game.TankerOriginalSlotsDestroyed + game.TankerOriginalSlotsEscaped +
            game.TankerOriginalSlotsRimCleared + game.TankerOriginalSlotsRemaining);
        Assert.Equal(Level.EnemyTotal + game.TankerMaterializedCargoUnits, game.TankerAccountingTotal);
        Assert.Equal(game.TankerCargoUnitsResolved + game.TankerCargoUnitsRemaining,
            game.TankerMaterializedCargoUnits + game.TankerLatentCargoUnits);
        Assert.InRange(game.CurrentLevel.ReserveEggs, 0, Level.EnemyTotal);
        Assert.InRange(game.TankerOriginalSlotsRemaining, 0, Level.EnemyTotal);
        Assert.InRange(game.TankerCargoUnitsRemaining, 0, 2);
    }

    private static Vector2 ProjectTanker(TempestGame game, Tanker tanker)
    {
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        Vector2 center = (points[tanker.EdgeIndex] + points[(tanker.EdgeIndex + 1) % points.Count]) / 2;
        return Projection.Project(center.X, center.Y, Level.Position + Level.Depth - tanker.Height, 400, -80);
    }

    private static TempestGame PrepareBulletGame(int ammunition, params int[] firingDecisions)
    {
        TempestGame game = new(enemyBulletsTest: true,
            random: new BulletRandom(ammunition, firingDecisions));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Flippers);
        Assert.Single(game.EnemyBullets);
        return game;
    }

    private static TempestGame CreateGameWithFourReadyFlippers()
    {
        TempestGame game = new(enemyBulletsTest: true,
            random: new BulletRandom(1, [255, 255, 255, 255]));
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(2, game.EnemyBullets.Count);
        return game;
    }

    private static TempestGame PrepareProductionBulletGame(int ammunition, params int[] firingDecisions)
    {
        TempestGame game = new(random: new BulletRandom(ammunition, firingDecisions));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Flippers);
        Step(game, 16);
        Assert.Single(game.EnemyBullets);
        Assert.False(game.EnemyBulletsTest);
        return game;
    }

    private static void AssertSpikerAccounting(TempestGame game) =>
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.EnemiesRimCleared +
            game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Flippers.Count + game.Spikers.Count);

    [Fact]
    public void ForcedRimSpikerIsNonKillClearedAndPersistentSpikeRetriesWithoutReconstruction()
    {
        TempestGame game = new(true, new SpikerRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);

        for (int unit = 0; unit < Level.EnemyTotal - 1; unit++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ReturnSurvivorsToReserve();
        for (int tick = 0; tick < 50; tick++)
            game.CurrentLevel.UpdateEggRelease();
        Egg discardedEntry = Assert.Single(game.Eggs);
        MakeReady(discardedEntry, game.CurrentLevel);
        Assert.NotNull(game.CurrentLevel.HatchReadyEnemy(lane => new Flipper(lane)));

        typeof(Enemy).GetProperty(nameof(Enemy.Height))!.SetValue(spiker, Level.Depth);
        int spikeCount = game.Spikes.Count;

        game.RunOneStep();

        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Empty(game.Spikers);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(spikeCount, game.Spikes.Count);

        Spike persistentSpike = Assert.Single(game.Spikes, spike => spike.EdgeIndex == game.Player.EdgeIndex);
        persistentSpike.Height = game.Player.Depth + 1;
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        for (int tick = 0; tick < 120 && game.State == LevelState.ReverseZoom; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Spikers);
        Assert.Equal(spikeCount, game.Spikes.Count);
        Assert.Equal(0, game.Score.Score);
        AssertSpikerAccounting(game);
    }

    private static TempestGame HatchDevelopmentSpiker()
    {
        TempestGame game = new(true, new SpikerRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Spikers);
        return game;
    }

    private static (TempestGame Game, SpikerBulletModeRandom Random) PrepareSpikerRetreatTransition(
        int decision, bool enemyBulletsTest)
    {
        SpikerBulletModeRandom random = new(decision);
        TempestGame game = new(true, random, enemyBulletsTest: enemyBulletsTest);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        for (int tick = 0; tick < 100 && spiker.State == SpikerState.Building; tick++)
            game.RunOneStep();
        Assert.Equal(SpikerState.Retreating, spiker.State);
        return (game, random);
    }

    private static TempestGame BeginSpikerExit()
    {
        TempestGame game = new(true, new SpikerRandom());
        for (int index = 0; index < 10; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        return game;
    }

    // Hatches the development Tanker armed with exactly one bullet and the minimal timer offset
    // (16 + 1), then steps until the first firing decision succeeds.
    private static TempestGame PrepareTankerBulletGame()
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(1, [1, 100]));
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(1, tanker.BulletAmmunition);
        Assert.Equal(16, tanker.BulletTimer);
        Step(game, 16);
        Assert.Empty(game.EnemyBullets);
        game.RunOneStep();
        Assert.Single(game.EnemyBullets);
        Assert.Equal(0, tanker.BulletAmmunition);
        return game;
    }

    private sealed class SpikerRandom : Random
    {
        public bool ChooseSpikers { get; set; } = true;
        public override int Next(int minValue, int maxValue) => ChooseSpikers ? minValue : maxValue - 1;
        public override double NextDouble() => 0;
    }

    private sealed class SpikerBulletModeRandom(int firingDecision) : Random
    {
        private int spikerSelections;

        public int FiringDecisionDraws { get; private set; }

        public override int Next(int minValue, int maxValue)
        {
            if (minValue == -6 && maxValue == 2)
                return spikerSelections++ == 0 ? minValue : maxValue - 1;

            if (minValue == 0 && maxValue == 101)
            {
                FiringDecisionDraws++;
                return firingDecision;
            }

            return minValue;
        }

        public override double NextDouble() => .75;
    }

    private sealed class StrictTankerRandom : Random
    {
        private readonly Dictionary<(int Minimum, int MaximumExclusive), Queue<int>> draws;

        public StrictTankerRandom(params (int Minimum, int MaximumExclusive, int[] Values)[] scripts)
        {
            draws = scripts.ToDictionary(script => (script.Minimum, script.MaximumExclusive),
                script => new Queue<int>(script.Values));
        }

        public int DecisionDrawCount { get; private set; }

        public int NextInclusive(int minimum, int maximum) => Next(minimum, maximum + 1);

        public void ExpectDecisions(IEnumerable<int> values)
        {
            AssertDecisionsComplete();
            foreach (int value in values)
                draws[(0, 101)].Enqueue(value);
        }

        public void AssertDecisionsComplete() => Assert.Empty(draws[(0, 101)]);

        public override int Next(int minValue, int maxValue)
        {
            if (!draws.TryGetValue((minValue, maxValue), out Queue<int>? values) || values.Count == 0)
                throw new InvalidOperationException($"Unexpected random draw [{minValue}, {maxValue}).");
            int value = values.Dequeue();
            Assert.InRange(value, minValue, maxValue - 1);
            if (minValue == 0 && maxValue == 101)
                DecisionDrawCount++;
            return value;
        }

        public override int Next() => throw new InvalidOperationException("Unexpected unbounded random draw.");
        public override int Next(int maxValue) =>
            throw new InvalidOperationException($"Unexpected one-bound random draw [0, {maxValue}).");
        public override double NextDouble() =>
            throw new InvalidOperationException("Unexpected floating-point random draw.");

        public void AssertComplete() => Assert.All(draws.Values, values => Assert.Empty(values));
    }

    private sealed class BulletRandom : Random
    {
        private readonly Queue<int> values;

        public List<(int Minimum, int Maximum)> Calls { get; } = [];

        public BulletRandom(int ammunition, IEnumerable<int> firingDecisions)
        {
            values = new Queue<int>(new[] { ammunition }.Concat(firingDecisions));
        }

        public override int Next(int minValue, int maxValue)
        {
            Calls.Add((minValue, maxValue));
            if (values.Count == 0)
                return minValue;

            return Math.Clamp(values.Dequeue(), minValue, maxValue - 1);
        }
    }

    // Deterministic double for the combined Tanker/bullet mode. Each known draw site has a
    // distinct exclusive upper bound, so dispatching on maxValue keeps full-run tests simple:
    // Tanker ammo Next(1,3), timer offset Next(1,65), Flipper ammo Next(1,4), decision Next(0,101).
    private sealed class TankerBulletModeRandom : Random
    {
        public override int Next(int minValue, int maxValue) => maxValue switch
        {
            101 => 100, // every firing decision succeeds
            65 => 1,    // minimal initial timer offset
            _ => minValue,
        };
    }

    // --- Normal Level 1 exit/zoom, projection and production rendering regressions ---

    [Fact]
    public void NormalPlayUsesEstablishedProjectionAndNearRimDepth()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(50, renderer.PlayerProjectedDepth);
        Assert.Equal(20, renderer.PlayerClawHeight);
        Step(game, 100);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(400, game.Player.Depth);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ExitCannotStartWithReserveOrActiveEggs(bool reserveOnly)
    {
        TempestGame game = new();
        if (reserveOnly)
            game.CurrentLevel.ReturnSurvivorsToReserve();
        Step(game, 49);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.LevelComplete);
        Assert.Equal(400, game.Player.Depth);
        if (reserveOnly)
            Assert.Equal(10, game.CurrentLevel.ReserveEggs);
        else
            Assert.Equal(4, game.Eggs.Count);
    }

    [Fact]
    public void ExitCannotStartWithTheFinalActiveFlipper()
    {
        TempestGame game = PrepareFinalEnemy();
        Assert.Single(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Step(game, 50);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void ExitPhaseOneChangesOffsetAndDepthForExactlyTwentyThreeTicks()
    {
        TempestGame game = BeginExit();
        Step(game, 21);
        Assert.Equal(LevelState.ExitZoom, game.State);
        for (int tick = 1; tick <= 23; tick++)
        {
            game.RunOneStep();
            Assert.Equal(5 - tick * 2, Projection.ZOffset);
            Assert.Equal(400 - tick * 3, game.Player.Depth);
            Assert.Equal(70, Projection.K);
        }
        Assert.Equal(-41, Projection.ZOffset);
        game.RunOneStep();
        Assert.Equal(-41, Projection.ZOffset);
        Assert.Equal(72, Projection.K);
        Assert.InRange(game.Player.Depth, 327.799f, 327.801f);
    }

    [Fact]
    public void ExitPhaseTwoAcceleratesKAfterOneHundred()
    {
        TempestGame game = BeginExit();
        Step(game, 21 + 23);
        for (int tick = 1; tick <= 15; tick++)
        {
            game.RunOneStep();
            Assert.Equal(70 + tick * 2, Projection.K);
            Assert.InRange(game.Player.Depth, 331 - tick * 3.2f - .001f, 331 - tick * 3.2f + .001f);
        }
        Assert.Equal(100, Projection.K);
        game.RunOneStep();
        Assert.Equal(104, Projection.K);
        game.RunOneStep();
        Assert.Equal(108, Projection.K);
        Assert.Equal(-41, Projection.ZOffset);
    }

    [Fact]
    public void ExitZoomIgnoresInputAndCommitsOnlyAfterSuccessfulTravel()
    {
        TempestGame game = BeginExit();
        Level outgoingLevel = game.CurrentLevel;
        int edge = game.Player.EdgeIndex;
        float position = game.Player.EdgePosition;
        PlayerInput input = new(true, 800, true, true);
        for (int tick = 0; tick < 21 + 123; tick++)
        {
            game.RunOneStep(input, restartPressed: true);
            Assert.Equal(edge, game.Player.EdgeIndex);
            Assert.Equal(position, game.Player.EdgePosition);
            Assert.Empty(game.Shots);
            Assert.Empty(game.Eggs);
            Assert.Empty(game.Flippers);
            Assert.Equal(0, game.CurrentLevel.ReserveEggs);
            Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
            Assert.Equal(0, game.DeathTicks);
            Assert.False(game.GameOver);
            Assert.Equal(3, game.Player.Lives);
            Assert.Same(outgoingLevel, game.CurrentLevel);
        }
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);

        game.RunOneStep(input, restartPressed: true);

        Assert.NotSame(outgoingLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(LevelApproach.InitialDepthOffset, game.CurrentApproach!.DepthOffset);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void ZoomCommitsBelowTenIntoFreshLevel2AndRestoresProjection()
    {
        TempestGame game = BeginExit();
        Step(game, 21 + 23 + 100);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);
        Level outgoingLevel = game.CurrentLevel;
        game.RunOneStep();
        Assert.NotSame(outgoingLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(LevelApproach.InitialDepthOffset, game.CurrentApproach!.DepthOffset);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(12, game.CurrentLevel.RemainingEnemies);
    }

    [Fact]
    public void SuccessfulExitCarriesSessionAndInstallsFreshAccounting()
    {
        TempestGame game = BeginExit();
        game.Score.Add(1500);
        game.Player.Lives = 2;
        FinishLevel1Exit(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.True(game.Score.Score >= 1500);
        Assert.Equal(0, (game.Score.Score - 1500) % 25);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(12, game.CurrentLevel.RemainingEnemies);
    }

    [Fact]
    public void UndestroyedFinalFlipperStillKillsPlayerAtCompletedPivot()
    {
        TempestGame game = PrepareFinalEnemy();
        game.Player.Move(-1, 16);
        Flipper flipper = Assert.Single(game.Flippers);
        while (flipper.Height < Level.Depth)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        while (flipper.State == FlipperState.Pausing)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        while (flipper.TurnAngle < flipper.TotalTurnAngle - 8)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);

        // Keep the explicit lethal-pivot route isolated from earlier all-rim exit ticks.
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.False(game.IsExiting);
        AssertAccounting(game);
        Step(game, 60);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Player.Depth);
        Assert.Single(game.Eggs);
        AssertAccounting(game);
    }

    [Fact]
    public void FinalPivotTickKillStartsExitInsteadOfKillingPlayer()
    {
        TempestGame game = PrepareFinalEnemy();
        game.Player.Move(-1, 16);
        Flipper flipper = Assert.Single(game.Flippers);
        while (flipper.Height < Level.Depth)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        while (flipper.State == FlipperState.Pausing)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        while (flipper.TurnAngle < flipper.TotalTurnAngle - 8)
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(game.Player.EdgeIndex, flipper.DestinationEdge);

        // Drive the enemy to its last pivot tick directly so the test isolates the shot-before-
        // collision kill route instead of naturally resolving earlier through all-rim completion.
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void ExitRendererSwitchesFromCircleToSquareAtTheProgressionCommit()
    {
        TempestGame game = BeginExit();
        VectorRenderer renderer = new(game);
        Level outgoingLevel = game.CurrentLevel;
        int ticks = 0;
        while (ReferenceEquals(outgoingLevel, game.CurrentLevel) && ticks++ < 200)
        {
            IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
            float expectedDepth = Level.Position + Level.Depth - game.Player.Depth;
            Assert.Equal(expectedDepth, renderer.PlayerProjectedDepth);
            renderer.GetPlayerEdge(points, out Vector2 first, out Vector2 second);
            Assert.Equal(Projection.Project(points[game.Player.EdgeIndex].X,
                points[game.Player.EdgeIndex].Y, expectedDepth, 400, -80), first);
            Assert.True(float.IsFinite(first.X) && float.IsFinite(first.Y));
            Assert.True(float.IsFinite(second.X) && float.IsFinite(second.Y));
            Assert.True(float.IsFinite(renderer.PlayerClawHeight));
            Assert.True(renderer.PlayerClawHeight > 0);
            foreach (Vector2 point in points)
            {
                Vector2 near = Projection.Project(point.X, point.Y, Level.Position, 400, -80);
                Vector2 far = Projection.Project(point.X, point.Y, Level.Position + Level.Depth, 400, -80);
                Assert.True(float.IsFinite(near.X) && float.IsFinite(near.Y));
                Assert.True(float.IsFinite(far.X) && float.IsFinite(far.Y));
            }
            game.RunOneStep();
        }
        Assert.NotSame(outgoingLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(LevelApproach.InitialDepthOffset, renderer.ApproachDepthOffset);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        IReadOnlyList<Vector2> squarePoints = game.CurrentLevel.Definition.Points;
        renderer.GetPlayerEdge(squarePoints, out Vector2 squareStart, out Vector2 squareEnd);
        Assert.True(float.IsFinite(squareStart.X) && float.IsFinite(squareStart.Y));
        Assert.True(float.IsFinite(squareEnd.X) && float.IsFinite(squareEnd.Y));
        Assert.Equal(Level.Position + Level.Depth - game.Player.Depth, renderer.PlayerProjectedDepth);
        foreach (Vector2 point in squarePoints)
        {
            Vector2 projected = Projection.Project(point.X, point.Y, Level.Position,
                game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
            Assert.True(float.IsFinite(projected.X) && float.IsFinite(projected.Y));
        }
    }

    [Fact]
    public void ExitDelayWaitsTwentyOneUpdatesAndIgnoresInput()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.LevelComplete);
        PlayerInput input = new(true, 80, true, true);
        for (int tick = 0; tick < 20; tick++)
        {
            game.RunOneStep(input, restartPressed: true);
            Assert.Equal(LevelState.ExitDelay, game.State);
            Assert.Equal(7, game.Player.EdgeIndex);
            Assert.Equal(0.5f, game.Player.EdgePosition);
            Assert.Equal(400, game.Player.Depth);
            Assert.Empty(game.Shots);
        }
        game.RunOneStep(input, restartPressed: true);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        game.RunOneStep(input, restartPressed: true);
        Assert.Equal(397, game.Player.Depth);
        Assert.Equal(3, Projection.ZOffset);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void ProductionInitializationAndFirstTickRetainRenderFacingEggs()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.Same(game.CurrentLevel.Eggs, game.Eggs);
        Assert.Same(game.Eggs, renderer.Eggs);
        Egg[] initial = game.Eggs.ToArray();
        game.RunOneStep();
        Assert.Equal(initial, renderer.Eggs);
        Assert.All(renderer.Eggs, egg =>
        {
            Assert.Equal(EggState.Roaming, egg.State);
            Assert.Equal(1, egg.UpdateCounter);
            Assert.Equal(-10, egg.Height);
        });
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void ProductionEggProjectionAndDrawSubmissionAreInsideWindow()
    {
        float previousK = Projection.K;
        float previousZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            TempestGame game = new();
            VectorRenderer renderer = new(game);
            List<Vector2> submitted = [];
            renderer.DrawEggs((point, size) =>
            {
                submitted.Add(point);
                Assert.Equal(new Vector2(3, 3), size);
                Assert.InRange(point.X + size.X, 0, 800);
                Assert.InRange(point.Y + size.Y, 0, 600);
            });
            Assert.Equal(4, submitted.Count);
            for (int eggIndex = 0; eggIndex < game.Eggs.Count; eggIndex++)
            {
                Egg egg = game.Eggs[eggIndex];
                IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
                Vector2 midpoint = (points[egg.EdgeIndex] + points[(egg.EdgeIndex + 1) % points.Count]) / 2;
                Vector2 expected = Projection.Project(midpoint.X, midpoint.Y, 650, 400, -80);
                Assert.Equal(expected, renderer.ProjectEgg(egg));
                Assert.Equal(expected - Vector2.One, submitted[eggIndex]);
                Assert.True(float.IsFinite(expected.X) && float.IsFinite(expected.Y));
                Assert.InRange(expected.X, 0, 799);
                Assert.InRange(expected.Y, 0, 599);
            }
        }
        finally
        {
            Projection.K = previousK;
            Projection.ZOffset = previousZOffset;
        }
    }

    [Fact]
    public void ProductionTimerCannotSpawnFlipperBeforeReadyEggConversion()
    {
        TempestGame game = new();
        for (int tick = 1; tick <= 64; tick++)
        {
            game.RunOneStep();
            Assert.Empty(game.Flippers);
            Assert.Equal(4, game.Eggs.Count);
            Assert.DoesNotContain(game.Eggs, egg => egg.State == EggState.ReadyToHatch);
        }
        Egg approaching = Assert.Single(game.Eggs, egg => egg.State == EggState.Approaching);
        Assert.Equal(-2, approaching.Height);
        Assert.Equal(7, approaching.UpdateCounter);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.DoesNotContain(approaching, game.Eggs);
        Assert.Equal(EggState.ReadyToHatch, approaching.State);
        Assert.Equal(0, approaching.Height);
        Assert.Equal(approaching.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(2, flipper.Height);
    }

    // --- Player movement, input and player-shot basics ---

    [Fact]
    public void PlayerPreservesExactBoundaryAndWrapsBothDirections()
    {
        Player player = new();
        player.Move(0.5f, 16);
        Assert.Equal(7, player.EdgeIndex);
        Assert.Equal(1, player.EdgePosition);
        player.Move(0.25f, 16);
        Assert.Equal(8, player.EdgeIndex);
        Assert.Equal(0.25f, player.EdgePosition);
        player.Move(-9, 16);
        Assert.Equal(15, player.EdgeIndex);
        Assert.Equal(0.25f, player.EdgePosition);
    }

    [Fact]
    public void FiringPrecedesMovementAndNewShotMovesImmediately()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Shot shot = Assert.Single(game.Shots);
        Assert.Equal(7, shot.EdgeIndex);
        Assert.Equal(394, shot.Height);
        Assert.Equal(15, shot.Rotation);
        Assert.Equal(8, game.Player.EdgeIndex);
        float boundaryAngle = game.CurrentLevel.CalculateLaneTransitionAngle(7, 8);
        float nextLaneAngle = game.CurrentLevel.CalculateLaneTransitionAngle(8, 9);
        Assert.Equal((180 - boundaryAngle * 0.5f) / nextLaneAngle, game.Player.EdgePosition, 5);
    }

    [Fact]
    public void KeyboardAndSpinnerAccumulateWithOriginalSigns()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(false, -8, true, false));
        float laneAngle = game.CurrentLevel.CalculateLaneTransitionAngle(7, 8);
        Assert.Equal(0.5f - 27 / laneAngle, game.Player.EdgePosition, 5);
        game.RunOneStep(new PlayerInput(false, 8, false, true));
        Assert.Equal(0.5f, game.Player.EdgePosition);
    }

    [Fact]
    public void FiringUsesThreeTickCooldownAndTenShotLimit()
    {
        TempestGame game = new();
        PlayerInput firing = new(true, 0, false, false);
        game.RunOneStep(firing);
        Step(game, 2, firing);
        Assert.Single(game.Shots);
        game.RunOneStep(firing);
        Assert.Equal(2, game.Shots.Count);
        Step(game, 30, firing);
        Assert.Equal(10, game.Shots.Count);
    }

    [Fact]
    public void ShotsRotateAndExpireOnlyBelowZero()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Step(game, 65);
        Assert.Equal(4, Assert.Single(game.Shots).Height);
        game.RunOneStep();
        Assert.Empty(game.Shots);
    }

    // --- Normal Level 1 spawn sequence, collision, death and replay regressions ---

    [Fact]
    public void SpawnSequenceAndIntervalAreDeterministic()
    {
        Level level = new();
        Assert.Equal(new[] { 12, 4, 9, 1 }, level.Eggs.Select(egg => egg.EdgeIndex));
        foreach (Egg egg in level.Eggs)
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
        }
        while (level.HatchReadyEgg() is not null) { }
        int[] expected = [14, 6, 10, 2, 15, 7];
        foreach (int lane in expected)
        {
            int released = level.EnemiesSpawned;
            for (int tick = 0; tick < 49; tick++)
                level.UpdateEggRelease();
            Assert.Equal(released, level.EnemiesSpawned);
            level.UpdateEggRelease();
            Egg egg = Assert.Single(level.Eggs);
            Assert.Equal(lane, egg.EdgeIndex);
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
            Assert.NotNull(level.HatchReadyEgg());
        }
        Assert.Equal(10, level.EnemiesSpawned);
        Assert.Equal(0, level.ReserveEggs);
    }

    [Fact]
    public void EggReleaseRespectsActiveLimitAndDeathRestoresPendingPool()
    {
        Level level = new();
        for (int tick = 0; tick < 100; tick++)
            level.UpdateEggRelease();
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(6, level.ReserveEggs);
        level.RecordEnemyDestroyed();
        level.RecordEnemyDestroyed();
        level.ResetAfterDeath();
        Assert.Equal(6, level.EnemiesSpawned);
        Assert.Equal(4, level.ReserveEggs);
        Assert.Equal(new[] { 9, 1, 14, 6 }, level.Eggs.Select(egg => egg.EdgeIndex));
    }

    [Fact]
    public void HatchedFlipperClimbsOnItsHatchTick()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        Assert.Empty(game.Flippers);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(egg.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(2, flipper.Height);
        Assert.DoesNotContain(egg, game.Eggs);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(15, true)]
    [InlineData(16, false)]
    public void ShotCollisionPreservesStrictDepthBoundaries(float separation, bool hit)
    {
        Flipper flipper = new(6, 400);
        Assert.Equal(hit, flipper.IsHitBy(new Shot { EdgeIndex = 6, Height = 400 - separation }));
        Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 5, Height = 400 - separation }));
    }

    [Fact]
    public void FlipperRemainsShootableOnBothLanesThroughoutVisiblePivot()
    {
        Level level = new();
        Flipper flipper = new(6, 400);
        flipper.Update(level, 7);
        Assert.Equal(24, flipper.PauseTicks);
        for (int tick = 0; tick < 23; tick++)
            flipper.Update(level, 7);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        flipper.Update(level, 7);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(7, flipper.DestinationEdge);

        int visibleTicks = 0;
        int expectedVisibleTicks = (int)MathF.Ceiling(
            flipper.TotalTurnAngle / level.Configuration.Flipper.TurnSpeedDegrees);
        while (!flipper.CompletedIntoPlayer)
        {
            Assert.True(flipper.IsHitBy(new Shot { EdgeIndex = 6, Height = 394 }));
            Assert.True(flipper.IsHitBy(new Shot { EdgeIndex = 7, Height = 394 }));
            Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 8, Height = 394 }));
            flipper.Update(level, 7);
            visibleTicks++;
        }
        Assert.Equal(expectedVisibleTicks, visibleTicks);
        Assert.True(flipper.CompletedIntoPlayer);
    }

    [Fact]
    public void PlayerCanMoveUnderPivotAndLeaveBeforeCompletion()
    {
        TempestGame game = PreparePivot();
        Flipper flipper = game.Flippers[0];
        game.Player.Move(1, 16);
        game.RunOneStep(new PlayerInput(false, -80, false, false));
        Assert.Equal(flipper.DestinationEdge, game.Player.EdgeIndex);
        Assert.Equal(0, game.DeathTicks);
        game.RunOneStep(new PlayerInput(false, 80, false, false));
        int pivotTicks = (int)MathF.Ceiling(
            flipper.TotalTurnAngle / game.CurrentLevel.Configuration.Flipper.TurnSpeedDegrees);
        for (int tick = 0; flipper.State == FlipperState.Flipping && tick < pivotTicks; tick++)
            game.RunOneStep();
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void CollisionUsesPlayerLaneOnCompletionTick()
    {
        TempestGame game = PreparePivot();
        int pivotTicks = GetFlipperPivotTicks(game);
        Step(game, pivotTicks - 1);
        Assert.Equal(0, game.DeathTicks);
        game.RunOneStep(new PlayerInput(false, 80, false, false));
        Assert.Equal(0, game.DeathTicks);

        game = PreparePivot();
        game.Player.Move(1, 16);
        pivotTicks = GetFlipperPivotTicks(game);
        Step(game, pivotTicks - 1);
        game.RunOneStep(new PlayerInput(false, -80, false, false));
        Assert.Equal(60, game.DeathTicks);
    }

    [Fact]
    public void FinalPivotTickShotWinsBeforePlayerCollisionAndScoresOnce()
    {
        TempestGame game = PreparePivot();
        Flipper flipper = game.Flippers[0];
        Step(game, GetFlipperPivotTicks(game, flipper) - 1);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.DoesNotContain(flipper, game.Flippers);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void DeathFreezesSimulationForSixtyTicksThenResetsPendingEnemies()
    {
        TempestGame game = PreparePivot();
        Step(game, GetFlipperPivotTicks(game));
        Assert.Equal(60, game.DeathTicks);
        float height = game.Flippers[1].Height;
        Step(game, 59, new PlayerInput(true, 80, false, false));
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(height, game.Flippers[1].Height);
        Assert.Empty(game.Shots);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Empty(game.Flippers);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Step(game, 50);
        Assert.Empty(game.Flippers);
        Assert.Equal(EggState.Approaching, game.Eggs[0].State);
    }

    [Fact]
    public void GameOverFreezesUntilRestartAndResetsScoreAndLives()
    {
        TempestGame game = PreparePivot();
        game.Player.Lives = 1;
        game.Score.Add(150);
        Step(game, GetFlipperPivotTicks(game) + TempestGame.PlayerDeathTicks);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Assert.Empty(game.Flippers);
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Empty(game.Shots);
        Assert.Equal(150, game.Score.Score);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(7, game.Player.EdgeIndex);
    }

    [Fact]
    public void DestroyingLastEnemyStartsExitDelayOnTheSameTick()
    {
        TempestGame game = new(random: new MinimumRandom());
        for (int enemyIndex = 0; enemyIndex < 9; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        Assert.Equal(7, Assert.Single(game.Eggs).EdgeIndex);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(7, Assert.Single(game.Flippers).EdgeIndex);

        int ticks = 0;
        while (!game.IsExiting && ticks < 100)
        {
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            ticks++;
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed == 10, game.State == LevelState.ExitDelay);
        }

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.LevelComplete);
        Assert.Empty(game.Flippers);
        Assert.Equal(175, game.Score.Score);
        Assert.Equal(0, game.DeathTicks);
    }

    [Fact]
    public void NaturalLevel3CompletionEntersTheLevel4ApproachWithCampaignState()
    {
        TempestGame game = new(startingLives: 5);
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.Score.Add(1500);
        game.Player.Lives = 2;
        game.RunOneStep();
        FinishLevel1Exit(game);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);

        for (int enemyIndex = 0; enemyIndex < game.CurrentLevel.EnemySupply; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        FinishLevel2Exit(game);

        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);

        Level level3 = game.CurrentLevel;
        Level3WaveLedger ledger = level3.Level3Ledger!;
        foreach (int id in ledger.UnresolvedOriginalIds.Order())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }
        level3.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(ledger.UnresolvedOriginalIds);

        for (int tick = 0; tick < 300 && ReferenceEquals(level3, game.CurrentLevel); tick++)
            game.RunOneStep();

        Assert.NotSame(level3, game.CurrentLevel);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(13, game.CurrentLevel.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
    }

    [Theory]
    [InlineData(0, 8, 1)]
    [InlineData(0, 0, 1)]
    [InlineData(15, 0, -1)]
    [InlineData(0, 15, 1)]
    public void ShortestDirectionPreservesTieAndWrapRules(int source, int target, int expected)
    {
        Assert.Equal(expected, new Level().FindShortestDirection(source, target));
    }

    // --- Egg lifecycle and normal ten-unit accounting regressions ---

    [Fact]
    public void NewEggStartsRoamingBehindTheWeb()
    {
        Egg egg = new(12, -1);
        Assert.Equal(12, egg.EdgeIndex);
        Assert.Equal(-10, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
        Assert.Equal(-1, egg.MovementDirection);
        Assert.Equal(1, egg.VerticalDirection);
        Assert.Equal(EggState.Roaming, egg.State);
    }

    [Theory]
    [InlineData(15, 1, 0)]
    [InlineData(0, -1, 15)]
    [InlineData(4, 1, 5)]
    public void EggRoamsAndWrapsOnlyOnTheEighthUpdate(int lane, int direction, int expected)
    {
        Level level = new();
        Egg egg = new(lane, direction);
        for (int tick = 0; tick < 7; tick++)
            egg.Update(level);
        Assert.Equal(lane, egg.EdgeIndex);
        Assert.Equal(-10, egg.Height);
        Assert.Equal(7, egg.UpdateCounter);
        egg.Update(level);
        Assert.Equal(expected, egg.EdgeIndex);
        Assert.Equal(-9, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
    }

    [Fact]
    public void RoamingEggReversesAtMinusOneAndMinusTwenty()
    {
        Level level = new();
        Egg egg = new();
        for (int tick = 0; tick < 9 * 8; tick++)
            egg.Update(level);
        Assert.Equal(-1, egg.Height);
        Assert.Equal(-1, egg.VerticalDirection);
        for (int tick = 0; tick < 19 * 8; tick++)
            egg.Update(level);
        Assert.Equal(-20, egg.Height);
        Assert.Equal(1, egg.VerticalDirection);
        for (int tick = 0; tick < 1000; tick++)
        {
            egg.Update(level);
            Assert.InRange(egg.Height, -20, -1);
        }
    }

    [Fact]
    public void ApproachingEggStepsByTwoEveryEightUpdatesThenClampsAtZero()
    {
        Level level = new();
        Egg egg = new(4);
        for (int tick = 0; tick < 8; tick++)
            egg.Update(level);
        Assert.Equal(-9, egg.Height);
        egg.BeginApproach();
        for (int tick = 0; tick < 7; tick++)
            egg.Update(level);
        Assert.Equal(-9, egg.Height);
        egg.Update(level);
        Assert.Equal(-7, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
        Assert.Equal(5, egg.EdgeIndex);
        for (int tick = 0; tick < 32; tick++)
            egg.Update(level);
        Assert.Equal(0, egg.Height);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        egg.Update(level);
        Assert.Equal(0, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
    }

    [Fact]
    public void LevelStartsWithTenEnemiesFourActiveEggsAndSixReserveEggs()
    {
        TempestGame game = new();
        Assert.Equal(10, Level.EnemyTotal);
        Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.All(game.Eggs, egg =>
        {
            Assert.Equal(-10, egg.Height);
            Assert.Equal(0, egg.UpdateCounter);
            Assert.Equal(EggState.Roaming, egg.State);
            Assert.InRange(egg.EdgeIndex, 0, 15);
        });
        Assert.Equal(new[] { 1, -1, 1, -1 }, game.Eggs.Select(egg => egg.MovementDirection));
        AssertAccounting(game);
    }

    [Fact]
    public void HatchSelectionIsStaggeredAndNeverExceedsTwoEggsInTransit()
    {
        Level level = new();
        foreach (Egg egg in level.Eggs)
            for (int tick = 0; tick < 28 * 8; tick++)
                egg.Update(level);

        for (int tick = 0; tick < 49; tick++)
            level.UpdateEggs();
        Assert.All(level.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
        level.UpdateEggs();
        Assert.Single(level.Eggs, egg => egg.State != EggState.Roaming);
        for (int tick = 0; tick < 50; tick++)
            level.UpdateEggs();
        Assert.Equal(2, level.Eggs.Count(egg => egg.State != EggState.Roaming));
        for (int tick = 0; tick < 300; tick++)
        {
            level.UpdateEggs();
            Assert.InRange(level.Eggs.Count(egg => egg.State != EggState.Roaming), 0, 2);
        }
    }

    [Fact]
    public void ReadyEggConvertsToOneFlipperAtZeroWithoutChangingRemainingCount()
    {
        Level level = new();
        Egg egg = level.Eggs[0];
        MakeReady(egg, level);
        int remaining = level.RemainingEnemies;
        Flipper flipper = Assert.IsType<Flipper>(level.HatchReadyEgg());
        Assert.Equal(egg.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(0, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Equal(3, level.Eggs.Count);
        Assert.DoesNotContain(egg, level.Eggs);
        Assert.Null(level.HatchReadyEgg());
        Assert.Equal(remaining, level.RemainingEnemies);
        Assert.Equal(10, level.ReserveEggs + level.Eggs.Count + 1);
    }

    [Fact]
    public void ReserveReleaseOccursOnFiftiethTickAndCreatesAnEggNotAFlipper()
    {
        TempestGame game = new();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(3, game.Eggs.Count);
        Step(game, 48);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Equal(3, game.Eggs.Count);
        game.RunOneStep();
        Assert.Equal(5, game.CurrentLevel.ReserveEggs);
        Assert.Equal(4, game.Eggs.Count);
        Egg released = game.Eggs[^1];
        Assert.Equal(14, released.EdgeIndex);
        Assert.Equal(-10, released.Height);
        Assert.Equal(EggState.Roaming, released.State);
        Assert.Single(game.Flippers);
        AssertAccounting(game);
    }

    [Fact]
    public void LevelCannotCompleteWithReserveEggsActiveEggsOrFlippers()
    {
        Level level = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            level.RecordEnemyDestroyed();
        Assert.False(level.IsComplete(0));
        level.ReturnSurvivorsToReserve();
        Assert.True(level.IsComplete(0));
        Assert.False(level.IsComplete(1));

        level.Reset();
        level.ReturnSurvivorsToReserve();
        Assert.Empty(level.Eggs);
        Assert.Equal(10, level.ReserveEggs);
        Assert.False(level.IsComplete(0));

        level.Reset();
        while (level.ReserveEggs > 0)
        {
            foreach (Egg egg in level.Eggs)
                MakeReady(egg, level);
            while (level.HatchReadyEgg() is not null) { }
            for (int tick = 0; tick < 50; tick++)
                level.UpdateEggRelease();
        }
        Assert.Equal(0, level.ReserveEggs);
        Assert.NotEmpty(level.Eggs);
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            level.RecordEnemyDestroyed();
        Assert.False(level.IsComplete(0));

        TempestGame game = new();
        Step(game, 49);
        Assert.Empty(game.Flippers);
        Assert.False(game.LevelComplete);
    }

    [Fact]
    public void AccountingSurvivesNaturalHatchesKillsDeathsAndRestart()
    {
        TempestGame game = new();
        bool sawHatch = false;
        bool sawDeath = false;
        for (int tick = 0; tick < 4000 && !game.GameOver; tick++)
        {
            game.RunOneStep();
            sawHatch |= game.Flippers.Count > 0;
            sawDeath |= game.DeathTicks > 0;
            AssertAccounting(game);
            Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
            Assert.False(game.LevelComplete);
        }
        Assert.True(sawHatch);
        Assert.True(sawDeath);
        Assert.True(game.GameOver);
        game.RunOneStep(restartPressed: true);
        AssertAccounting(game);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void DefeatedEnemiesAndScoreStayDefeatedAfterLosingALife()
    {
        TempestGame game = PreparePivot();
        Step(game, 2);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        AssertAccounting(game);
        int ticks = 0;
        while (game.Player.Lives == 3 && ticks++ < 2000)
        {
            game.RunOneStep();
            AssertAccounting(game);
        }
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(9, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(5, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void EntireEggWaveCanBeDestroyedWithSameTickExitThenFreshLevel2()
    {
        TempestGame game = new(random: new MinimumRandom());
        int previousDestroyed = 0;
        for (int tick = 0; tick < 5000 && !game.IsExiting && !game.GameOver; tick++)
        {
            Flipper? target = game.Flippers.OrderByDescending(flipper => flipper.Height).FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertAccounting(game);
            int flipperScore = game.CurrentLevel.EnemiesDestroyed * 150;
            Assert.True(game.Score.Score >= flipperScore);
            Assert.Equal(0, (game.Score.Score - flipperScore) % 25);
            Assert.Equal(game.CurrentLevel.RemainingEnemies,
                game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Flippers.Count);
            Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
            Assert.True(game.CurrentLevel.EnemiesDestroyed >= previousDestroyed);
            previousDestroyed = game.CurrentLevel.EnemiesDestroyed;
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed == 10, game.State == LevelState.ExitDelay);
        }
        Assert.Equal(LevelState.ExitDelay, game.State);
        FinishLevel1Exit(game);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(game.Score.Score >= 1500);
        Assert.Equal(0, (game.Score.Score - 1500) % 25);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Empty(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(12, game.CurrentLevel.RemainingEnemies);
    }

    private static TempestGame PrepareFinalEnemy()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 9; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame BeginExit()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        return game;
    }

    private static void FinishLevel1Exit(TempestGame game)
    {
        for (int tick = 0; tick < 200 &&
             !ReferenceEquals(game.CurrentLevel.Configuration, LevelConfiguration.Level2Square); tick++)
            game.RunOneStep();
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }

    private static void FinishLevel2Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 200 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }

    private static void FinishTerminalExit(TempestGame game)
    {
        for (int tick = 0; tick < 200 && !game.LevelComplete; tick++)
            game.RunOneStep();
        Assert.True(game.LevelComplete);
    }

    private static void AssertAccounting(TempestGame game)
    {
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.EnemiesRimCleared +
            game.CurrentLevel.ReserveEggs
            + game.Eggs.Count + game.Flippers.Count);
        Assert.InRange(game.CurrentLevel.ReserveEggs, 0, 10);
        Assert.InRange(game.Eggs.Count, 0, 4);
        Assert.InRange(game.Eggs.Count(egg => egg.State != EggState.Roaming), 0, 2);
    }

    private static TempestGame PreparePivot()
        => PreparePivotWithStartingLives(Player.DefaultStartingLives);

    private static int GetFlipperPivotTicks(TempestGame game, Flipper? flipper = null) =>
        (int)MathF.Ceiling(
            (flipper ?? game.Flippers[0]).TotalTurnAngle /
            game.CurrentLevel.Configuration.Flipper.TurnSpeedDegrees);

    private static TempestGame PreparePivotWithStartingLives(int startingLives)
    {
        TempestGame game = new(startingLives: startingLives);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        game.Player.Move(egg.EdgeIndex - 1 - game.Player.EdgeIndex, 16);
        Step(game, 199);
        Assert.Equal(FlipperState.Pausing, game.Flippers[0].State);
        Step(game, 24);
        Assert.Equal(FlipperState.Flipping, game.Flippers[0].State);
        Assert.Equal(11, game.Flippers[0].DestinationEdge);
        return game;
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void Step(TempestGame game, int ticks, PlayerInput input = default)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep(input);
    }

    private static Level CreateTopologyLevel(bool continuous)
    {
        LevelConfiguration level1 = LevelConfiguration.Level1;
        LevelConfiguration fixture = level1 with
        {
            Definition = level1.Definition with { Continuous = continuous }
        };
        return new Level(fixture);
    }
}
