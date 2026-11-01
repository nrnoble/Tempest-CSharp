using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level3WaveLedgerTests
{
    [Fact]
    public void TankerCommitAllocatesOneOwnedLatentPairAndPreservesNamespaces()
    {
        Level3WaveLedger ledger = new(originalCount: 3);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(0));
        Assert.Equal(new[] { 0, 1 }, ledger.GetCargoIdsForOriginal(0));
        Assert.Equal(0, ledger.GetCargoOwner(0));
        Assert.Equal(1, ledger.GetCargoOrdinal(1));
        Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(0));
        Assert.True(ledger.HasLatentCargo);
        Assert.Equal(new[] { 0, 1 }, ledger.UnresolvedCargoIds.Order());
        Assert.Equal(5, ledger.TotalIdentityCount);
        Assert.Throws<InvalidOperationException>(() => ledger.CommitOriginalAsTanker(0));
        ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0),
            new(Level3IdentityKind.Original, 1),
            new(Level3IdentityKind.Original, 2)
        ]);
    }

    [Fact]
    public void NaturalReleaseKeepsCargoIdentityAndCarrierOutcomeDistinct()
    {
        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        IReadOnlyList<int> children = ledger.MaterializeLiveCargo(0);
        ledger.MarkOriginalEscaped(0);

        Assert.Equal(Level3OriginalState.Escaped, ledger.GetOriginalState(0));
        Assert.Equal(new[] { Level3CargoState.Active, Level3CargoState.Active },
            children.Select(ledger.GetCargoState));
        ledger.ValidateRepresentation(children
            .Select(id => new Level3Identity(Level3IdentityKind.Cargo, id)));

        ledger.MarkCargoDestroyed(children[0]);
        ledger.MarkCargoRimCleared(children[1]);
        Assert.Throws<InvalidOperationException>(() => ledger.MarkCargoDestroyed(children[0]));
        ledger.ValidateState();
    }

    [Fact]
    public void ShotActivationAndDeathReconstructionRetainUnresolvedIdentitySets()
    {
        Level3WaveLedger ledger = new(originalCount: 2);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);
        IReadOnlyList<int> children = ledger.QueueCargoForShotActivation(0);
        ledger.MarkOriginalDestroyed(0);
        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsFlipper(1);

        ledger.ActivateShotCargo(children[0]);
        ledger.MarkCargoDestroyed(children[0]);
        HashSet<int> unresolvedOriginals = ledger.UnresolvedOriginalIds.ToHashSet();
        HashSet<int> unresolvedCargo = ledger.UnresolvedCargoIds.ToHashSet();

        ledger.RebuildAfterDeath();

        Assert.Equal(unresolvedOriginals, ledger.UnresolvedOriginalIds);
        Assert.Equal(unresolvedCargo, ledger.UnresolvedCargoIds);
        Assert.Equal(Level3CargoState.ReentryReserve, ledger.GetCargoState(children[1]));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(children[0]));
        Assert.Equal(Level3OriginalType.Flipper, ledger.GetOriginalType(1));
        ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 1),
            new(Level3IdentityKind.Cargo, children[1])
        ]);
    }

    [Fact]
    public void DeathResetReturnsBothPendingShotCargoIdentitiesAsEggs()
    {
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsTanker(0);
        IReadOnlyList<int> cargo = ledger.QueueCargoForShotActivation(0);
        ledger.MarkOriginalDestroyed(0);

        Assert.All(cargo, id =>
            Assert.Equal(Level3CargoState.PendingShotActivation, ledger.GetCargoState(id)));
        int cargoCount = ledger.CargoCount;
        int totalIdentityCount = ledger.TotalIdentityCount;

        level.ResetAfterDeath();

        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(0));
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(new[] { Level3CargoState.Egg, Level3CargoState.Egg },
            cargo.Select(ledger.GetCargoState));
        Assert.Equal(cargo.Order(), level.Eggs
            .Where(egg => egg.CargoUnitId >= 0)
            .Select(egg => egg.CargoUnitId).Order());
        Assert.All(cargo, id =>
        {
            Assert.Equal(1, level.Eggs.Count(egg => egg.CargoUnitId == id));
            Assert.Equal(0, ledger.GetCargoOwner(id));
        });
        Assert.Equal(new[] { 0, 1 }, cargo.Select(ledger.GetCargoOrdinal));
        Assert.Equal(cargoCount, ledger.CargoCount);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(0, level.Level3CargoReserveEggs);

        Level3Identity[] represented = ledger.OriginalIdsInReserve()
            .Select(id => new Level3Identity(Level3IdentityKind.Original, id))
            .Concat(ledger.CargoIdsInReentryReserve()
                .Select(id => new Level3Identity(Level3IdentityKind.Cargo, id)))
            .Concat(level.Eggs.Select(egg => egg.CargoUnitId >= 0
                ? new Level3Identity(Level3IdentityKind.Cargo, egg.CargoUnitId)
                : new Level3Identity(Level3IdentityKind.Original, egg.OriginalSlotId)))
            .ToArray();
        ledger.ValidateRepresentation(represented);
    }

    [Fact]
    public void UnreleasedTankerKeepsItsLatentPairWhenReconstructedAsReserve()
    {
        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        ledger.RebuildAfterDeath();

        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(0));
        Assert.Equal(new[] { Level3CargoState.Latent, Level3CargoState.Latent },
            new[] { 0, 1 }.Select(ledger.GetCargoState));
        ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0)
        ]);
    }

    [Fact]
    public void RepresentationValidationRejectsDuplicateMissingAndExtraneousIdentities()
    {
        Level3WaveLedger ledger = new(originalCount: 2);

        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0),
            new(Level3IdentityKind.Original, 0)
        ]));
        Assert.Throws<ArgumentOutOfRangeException>(() => ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0),
            new(Level3IdentityKind.Original, 3)
        ]));
        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0)
        ]));
    }

    [Fact]
    public void RepresentationValidationRejectsEqualCountsWithWrongNamespaceAndCargoOwner()
    {
        Level3WaveLedger ledger = new(originalCount: 2);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRepresentation(
        [
            new(Level3IdentityKind.Original, 0),
            new(Level3IdentityKind.Cargo, 0)
        ]));
        Assert.Throws<InvalidOperationException>(() => ledger.ValidateCargoOwner(
            cargoId: 0, ownerOriginalId: 1));
    }

    [Fact]
    public void RuntimeValidationRejectsDuplicateLatentAndEqualCountExtraneousCargo()
    {
        Level3WaveLedger ledger = new(originalCount: 2);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);
        Level3WaveRepresentation original = new(
            new(Level3IdentityKind.Original, 0), Level3RepresentationLocation.ActiveEnemy);
        Level3WaveRepresentation reserve = new(
            new(Level3IdentityKind.Original, 1), Level3RepresentationLocation.Reserve);
        ledger.ValidateRuntimeRepresentations([original, reserve]);
        Assert.Throws<InvalidOperationException>(() =>
            ledger.ValidateRuntimeRepresentations([reserve]));
        Assert.Throws<InvalidOperationException>(() => ledger.MarkOriginalDestroyed(0));
        Assert.True(ledger.HasLatentCargo);
        Assert.Equal(new[] { 0, 1 }, ledger.UnresolvedCargoIds.Order());

        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRuntimeRepresentations(
        [
            original,
            original,
            reserve
        ]));
        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRuntimeRepresentations(
        [
            original,
            new(new(Level3IdentityKind.Cargo, 0), Level3RepresentationLocation.ActiveEnemy)
        ]));
        Assert.Throws<InvalidOperationException>(() => ledger.ValidateRuntimeRepresentations(
        [
            original,
            new(new(Level3IdentityKind.Cargo, 0), Level3RepresentationLocation.Reserve)
        ]));
    }

    [Fact]
    public void RuntimeValidationRejectsCargoDuplicateAndTerminalOriginalStillActive()
    {
        Level3WaveLedger cargoLedger = new(originalCount: 1);
        cargoLedger.MarkOriginalEgg(0);
        cargoLedger.CommitOriginalAsTanker(0);
        IReadOnlyList<int> cargo = cargoLedger.MaterializeLiveCargo(0);
        cargoLedger.MarkOriginalEscaped(0);

        Level3WaveRepresentation firstCargo = new(
            new(Level3IdentityKind.Cargo, cargo[0]), Level3RepresentationLocation.ActiveEnemy);
        Level3WaveRepresentation secondCargo = new(
            new(Level3IdentityKind.Cargo, cargo[1]), Level3RepresentationLocation.ActiveEnemy);
        cargoLedger.ValidateRuntimeRepresentations([firstCargo, secondCargo]);
        Assert.Throws<InvalidOperationException>(() =>
            cargoLedger.ValidateRuntimeRepresentations(
                [firstCargo, firstCargo, secondCargo]));
        Assert.Throws<InvalidOperationException>(() =>
            cargoLedger.ValidateCargoOwner(cargo[0], ownerOriginalId: 1));

        Level3WaveLedger terminalLedger = new(originalCount: 2);
        terminalLedger.MarkOriginalEgg(0);
        terminalLedger.CommitOriginalAsFlipper(0);
        terminalLedger.MarkOriginalDestroyed(0);
        Assert.Throws<InvalidOperationException>(() =>
            terminalLedger.ValidateRuntimeRepresentations(
            [
                new(new(Level3IdentityKind.Original, 0), Level3RepresentationLocation.ActiveEnemy),
                new(new(Level3IdentityKind.Original, 1), Level3RepresentationLocation.Reserve)
            ]));
    }

    [Theory]
    [InlineData(Level3CargoState.PendingShotActivation, Level3RepresentationLocation.PendingActivation)]
    [InlineData(Level3CargoState.ReentryReserve, Level3RepresentationLocation.Reserve)]
    [InlineData(Level3CargoState.Egg, Level3RepresentationLocation.Egg)]
    public void RuntimeValidationRequiresExactCargoRepresentationBesideTheSameNumberedOriginal(
        Level3CargoState state, Level3RepresentationLocation location)
    {
        Level3WaveLedger ledger = new(originalCount: 2);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);
        ledger.QueueCargoForShotActivation(0);
        ledger.MarkOriginalDestroyed(0);
        ledger.ActivateShotCargo(0);
        ledger.MarkCargoDestroyed(0);
        if (state != Level3CargoState.PendingShotActivation)
            ledger.RebuildAfterDeath();
        if (state == Level3CargoState.Egg)
            ledger.MarkCargoEgg(1);

        Level3WaveRepresentation original = new(
            new(Level3IdentityKind.Original, 1), Level3RepresentationLocation.Reserve);
        Level3WaveRepresentation cargo = new(new(Level3IdentityKind.Cargo, 1), location);
        Assert.Equal(state, ledger.GetCargoState(1));
        Assert.Equal(new[] { 1 }, ledger.UnresolvedOriginalIds);
        Assert.Equal(new[] { 1 }, ledger.UnresolvedCargoIds);
        Assert.Equal(4, ledger.TotalIdentityCount);
        ledger.ValidateRuntimeRepresentations([original, cargo]);

        InvalidOperationException missing = Assert.Throws<InvalidOperationException>(
            () => ledger.ValidateRuntimeRepresentations([original]));
        Assert.Contains("runtime representation mismatch", missing.Message);
        Assert.Contains("Cargo, Id = 1", missing.Message);
        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(
            () => ledger.ValidateRuntimeRepresentations([original, original]));
        Assert.Contains("represented more than once", duplicate.Message);
        InvalidOperationException extraneous = Assert.Throws<InvalidOperationException>(
            () => ledger.ValidateRuntimeRepresentations(
                [original, new(new(Level3IdentityKind.Original, 0), location)]));
        Assert.Contains("runtime representation mismatch", extraneous.Message);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ledger.ValidateRuntimeRepresentations(
                [original, new(new(Level3IdentityKind.Cargo, 99), location)]));
        InvalidOperationException wrongLocation = Assert.Throws<InvalidOperationException>(
            () => ledger.ValidateRuntimeRepresentations(
                [original, cargo with { Location = Level3RepresentationLocation.ActiveEnemy }]));
        Assert.Contains("runtime representation mismatch", wrongLocation.Message);
    }

    [Fact]
    public void DeathRebuildRetainsEveryIdentityTypeOwnerAndTerminalOutcome()
    {
        Level3WaveLedger ledger = new(originalCount: 3);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);
        IReadOnlyList<int> cargo = ledger.QueueCargoForShotActivation(0);
        ledger.ActivateShotCargo(cargo[0]);
        ledger.MarkCargoDestroyed(cargo[0]);
        ledger.MarkOriginalDestroyed(0);
        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsFlipper(1);
        ledger.MarkOriginalDestroyed(1);

        int totalIdentityCount = ledger.TotalIdentityCount;
        HashSet<int> unresolvedOriginals = ledger.UnresolvedOriginalIds.ToHashSet();
        HashSet<int> unresolvedCargo = ledger.UnresolvedCargoIds.ToHashSet();

        ledger.RebuildAfterDeath();
        ledger.RebuildAfterDeath();

        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(5, totalIdentityCount);
        Assert.Equal(unresolvedOriginals, ledger.UnresolvedOriginalIds);
        Assert.Equal(unresolvedCargo, ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalType.Flipper, ledger.GetOriginalType(1));
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(0));
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(1));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargo[0]));
        Assert.Equal(Level3CargoState.ReentryReserve, ledger.GetCargoState(cargo[1]));
        Assert.Equal(0, ledger.GetCargoOwner(cargo[0]));
        Assert.Equal(1, ledger.GetCargoOrdinal(cargo[1]));
    }
}
