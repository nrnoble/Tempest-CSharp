using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level3MixedAdmissionPolicyTests
{
    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 1)]
    [InlineData(2, 0)]
    public void OneActiveTankerAppliesWrappingCargoAvailability(
        int activeFlippers,
        int expectedAvailability)
    {
        Level3MixedAdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: activeFlippers,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);

        Assert.Equal(expectedAvailability,
            Level3MixedAdmissionPolicy.GetFlipperAvailability(snapshot));
        Assert.Equal(
            expectedAvailability > 0 ? Level3OriginalType.Flipper : null,
            Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(snapshot));
        Assert.Equal(0, Level3MixedAdmissionPolicy.GetTankerAvailability(snapshot));
    }

    [Theory]
    [InlineData(3, 1, Level3OriginalType.Tanker)]
    [InlineData(4, 1, null)]
    public void TankerMinimumRequiresSelectableFlipperCargo(
        int activeFlippers,
        int expectedTankerAvailability,
        Level3OriginalType? expectedType)
    {
        Level3MixedAdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: activeFlippers);

        Assert.Equal(expectedTankerAvailability,
            Level3MixedAdmissionPolicy.GetTankerAvailability(snapshot));
        Assert.Equal(expectedType,
            Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(snapshot));
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    public void TankerCargoByteUnderflowReopensFlipperAvailabilityWithinPhysicalCapacity(
        int activeFlippers,
        int expectedAvailability)
    {
        Level3MixedAdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: activeFlippers,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);

        Assert.Equal(expectedAvailability,
            Level3MixedAdmissionPolicy.GetFlipperAvailability(snapshot));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(snapshot));
    }

    [Fact]
    public void ActiveSixthEnemyCanAdmitOneMoreButSeventhClosesAllOrdinaryAdmission()
    {
        Level3MixedAdmissionSnapshot sixActive = Snapshot(
            ordinaryFlippers: 5,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);
        Level3MixedAdmissionSnapshot sevenActive = Snapshot(
            ordinaryFlippers: 6,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);

        Assert.Equal(1, Level3MixedAdmissionPolicy.GetOpenPhysicalSlots(sixActive));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(sixActive));
        Assert.Equal(0, Level3MixedAdmissionPolicy.GetOpenPhysicalSlots(sevenActive));
        Assert.Null(Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(sevenActive));
    }

    [Fact]
    public void SelectedTankerWaitsWithoutChangingTypeAndSpikerIsNotAdmitted()
    {
        Level3MixedAdmissionSnapshot oneTanker = Snapshot(
            ordinaryFlippers: 3,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);

        Assert.Null(Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(
            oneTanker, Level3OriginalType.Tanker));
        Assert.Null(Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(
            Snapshot(), Level3OriginalType.Spiker));
    }

    [Fact]
    public void MaterializedShotCargoAndRimOrTransitionFlippersOccupyPhysicalPlaces()
    {
        Level3MixedAdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: 2,
            releasedCargoFlippers: 1,
            pendingShotCargoFlippers: 2,
            activeTankers: 1,
            latentCargo: Tanker.CarriedFlipperCount);

        Assert.Equal(6, snapshot.ActiveEnemyCount);
        Assert.Equal(1, Level3MixedAdmissionPolicy.GetOpenPhysicalSlots(snapshot));
        Assert.True(Level3MixedAdmissionPolicy.CanAdmitCargoReentry(snapshot));
        Assert.False(Level3MixedAdmissionPolicy.CanAdmitCargoReentry(
            snapshot with { OrdinaryFlippers = 3 }));
    }

    [Fact]
    public void CargoReleaseMayExceedOrdinaryCapacityButDoesNotReopenHatchAdmission()
    {
        // The accepted complete-pair release can produce eight live Flippers from a
        // previously full seven-object population; only later ordinary hatches are blocked.
        Level3MixedAdmissionSnapshot afterCompletePairRelease = Snapshot(
            releasedCargoFlippers: 8);

        Assert.Equal(0,
            Level3MixedAdmissionPolicy.GetOpenPhysicalSlots(afterCompletePairRelease));
        Assert.False(Level3MixedAdmissionPolicy.CanAdmitCargoReentry(
            afterCompletePairRelease));
        Assert.Null(Level3MixedAdmissionPolicy.SelectAdmissibleOriginalType(
            afterCompletePairRelease));
    }

    private static Level3MixedAdmissionSnapshot Snapshot(
        int ordinaryFlippers = 0,
        int releasedCargoFlippers = 0,
        int pendingShotCargoFlippers = 0,
        int activeTankers = 0,
        int activeOtherEnemies = 0,
        int latentCargo = 0) =>
        new(ordinaryFlippers, releasedCargoFlippers, pendingShotCargoFlippers,
            activeTankers, activeOtherEnemies, latentCargo);
}
