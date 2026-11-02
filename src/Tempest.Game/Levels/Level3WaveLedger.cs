namespace Tempest.Game.Levels;

public enum Level3OriginalType
{
    Unselected,
    Flipper,
    Tanker,
    Spiker
}

public enum Level3OriginalState
{
    Reserve,
    Egg,
    ActiveFlipper,
    ActiveTanker,
    ActiveSpiker,
    Destroyed,
    Escaped,
    RimCleared
}

public enum Level3CargoState
{
    Latent,
    PendingShotActivation,
    ReentryReserve,
    Egg,
    Active,
    Destroyed,
    RimCleared
}

public enum Level3IdentityKind
{
    Original,
    Cargo
}

public readonly record struct Level3Identity(
    Level3IdentityKind Kind,
    int Id);

public enum Level3RepresentationLocation
{
    Reserve,
    Egg,
    ActiveEnemy,
    PendingActivation
}

public readonly record struct Level3WaveRepresentation(
    Level3Identity Identity,
    Level3RepresentationLocation Location);

public sealed class Level3WaveLedger
{
    private sealed class OriginalEntry
    {
        public Level3OriginalType Type { get; set; }
        public Level3OriginalState State { get; set; }

        public OriginalEntry(Level3OriginalState state)
        {
            State = state;
        }
    }

    private sealed class CargoEntry(int ownerOriginalId, int ordinal)
    {
        public int OwnerOriginalId { get; } = ownerOriginalId;
        public int Ordinal { get; } = ordinal;
        public Level3CargoState State { get; set; } = Level3CargoState.Latent;
    }

    private readonly Dictionary<int, OriginalEntry> originals = [];
    private readonly Dictionary<int, CargoEntry> cargo = [];

    public Level3WaveLedger(int originalCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(originalCount);

        for (int id = 0; id < originalCount; id++)
            originals.Add(id, new OriginalEntry(Level3OriginalState.Reserve));
    }

    public int OriginalCount => originals.Count;
    public int CargoCount => cargo.Count;
    public int TotalIdentityCount => originals.Count + cargo.Count;
    public bool HasLatentCargo =>
        cargo.Values.Any(entry => entry.State == Level3CargoState.Latent);
    public bool HasPendingShotActivation =>
        cargo.Values.Any(entry => entry.State == Level3CargoState.PendingShotActivation);
    public IReadOnlySet<int> UnresolvedOriginalIds => originals
        .Where(pair => !IsTerminal(pair.Value.State))
        .Select(pair => pair.Key)
        .ToHashSet();
    public IReadOnlySet<int> UnresolvedCargoIds => cargo
        .Where(pair => !IsTerminal(pair.Value.State))
        .Select(pair => pair.Key)
        .ToHashSet();

    public Level3OriginalType GetOriginalType(int id) =>
        GetOriginal(id).Type;

    public Level3OriginalState GetOriginalState(int id) =>
        GetOriginal(id).State;

    public Level3CargoState GetCargoState(int id) =>
        GetCargo(id).State;

    public int GetCargoOwner(int id) => GetCargo(id).OwnerOriginalId;

    public int GetCargoOrdinal(int id) => GetCargo(id).Ordinal;

    public void ValidateCargoOwner(int cargoId, int ownerOriginalId)
    {
        if (GetCargoOwner(cargoId) != ownerOriginalId)
            throw new InvalidOperationException(
                $"Cargo identity {cargoId} belongs to original {GetCargoOwner(cargoId)}, not {ownerOriginalId}.");
    }

    public void ValidateOriginalRuntimeType(int id, Level3OriginalType runtimeType)
    {
        if (runtimeType is not (Level3OriginalType.Flipper or Level3OriginalType.Tanker or Level3OriginalType.Spiker))
            throw new ArgumentOutOfRangeException(nameof(runtimeType));
        if (GetOriginalType(id) != runtimeType)
            throw new InvalidOperationException(
                $"Original {id} is selected as {GetOriginalType(id)} but is represented as {runtimeType}.");
    }

    public IReadOnlyList<int> GetCargoIdsForOriginal(int originalId)
    {
        GetOriginal(originalId);
        return cargo.Where(pair => pair.Value.OwnerOriginalId == originalId)
            .OrderBy(pair => pair.Value.Ordinal)
            .Select(pair => pair.Key)
            .ToArray();
    }

    public IReadOnlyList<int> OriginalIdsInReserve() => originals
        .Where(pair => pair.Value.State == Level3OriginalState.Reserve)
        .Select(pair => pair.Key)
        .Order()
        .ToArray();

    public IReadOnlyList<int> CargoIdsInReentryReserve() => cargo
        .Where(pair => pair.Value.State == Level3CargoState.ReentryReserve)
        .Select(pair => pair.Key)
        .Order()
        .ToArray();

    public void MarkOriginalEgg(int id) =>
        TransitionOriginal(id, Level3OriginalState.Reserve, Level3OriginalState.Egg);

    public void CommitOriginalAsFlipper(int id)
    {
        OriginalEntry entry = GetOriginal(id);
        if (entry.State != Level3OriginalState.Egg)
            throw InvalidTransition("original", id, entry.State, Level3OriginalState.ActiveFlipper);
        if (entry.Type is Level3OriginalType.Tanker or Level3OriginalType.Spiker)
            throw new InvalidOperationException($"Original {id} is already committed as {entry.Type}.");
        if (GetCargoIdsForOriginal(id).Count != 0)
            throw new InvalidOperationException($"Flipper original {id} cannot own cargo.");

        entry.Type = Level3OriginalType.Flipper;
        entry.State = Level3OriginalState.ActiveFlipper;
    }

    public void CommitOriginalAsTanker(int id)
    {
        OriginalEntry entry = GetOriginal(id);
        if (entry.State != Level3OriginalState.Egg)
            throw InvalidTransition("original", id, entry.State, Level3OriginalState.ActiveTanker);
        if (entry.Type is Level3OriginalType.Flipper or Level3OriginalType.Spiker)
            throw new InvalidOperationException($"Original {id} is already committed as {entry.Type}.");

        if (entry.Type == Level3OriginalType.Unselected)
        {
            if (GetCargoIdsForOriginal(id).Count != 0)
                throw new InvalidOperationException($"Unselected original {id} already owns cargo.");

            entry.Type = Level3OriginalType.Tanker;
            cargo.Add(CargoUnitId(id, 0), new CargoEntry(id, 0));
            cargo.Add(CargoUnitId(id, 1), new CargoEntry(id, 1));
        }
        else if (GetCargoIdsForOriginal(id).Count != 2)
        {
            throw new InvalidOperationException($"Selected Tanker {id} must retain its original cargo pair.");
        }

        entry.State = Level3OriginalState.ActiveTanker;
    }

    public void CommitOriginalAsSpiker(int id)
    {
        OriginalEntry entry = GetOriginal(id);
        if (entry.State != Level3OriginalState.Egg)
            throw InvalidTransition("original", id, entry.State, Level3OriginalState.ActiveSpiker);
        if (entry.Type is Level3OriginalType.Flipper or Level3OriginalType.Tanker)
            throw new InvalidOperationException($"Original {id} is already committed as {entry.Type}.");
        if (GetCargoIdsForOriginal(id).Count != 0)
            throw new InvalidOperationException($"Spiker original {id} cannot own cargo.");

        entry.Type = Level3OriginalType.Spiker;
        entry.State = Level3OriginalState.ActiveSpiker;
    }

    public void ReturnSpikerToReserve(int id)
    {
        OriginalEntry entry = GetOriginal(id);
        if (entry.Type != Level3OriginalType.Spiker ||
            entry.State != Level3OriginalState.ActiveSpiker)
            throw new InvalidOperationException($"Only an active Spiker original can return to reserve.");
        entry.Type = Level3OriginalType.Unselected;
        entry.State = Level3OriginalState.Reserve;
    }

    public void MarkOriginalDestroyed(int id) =>
        MarkOriginalTerminal(id, Level3OriginalState.Destroyed);

    public void MarkOriginalEscaped(int id) =>
        MarkOriginalTerminal(id, Level3OriginalState.Escaped);

    public void MarkOriginalRimCleared(int id) =>
        MarkOriginalTerminal(id, Level3OriginalState.RimCleared);

    public IReadOnlyList<int> MaterializeLiveCargo(int ownerOriginalId) =>
        MaterializeCargo(ownerOriginalId, Level3CargoState.Active);

    public IReadOnlyList<int> QueueCargoForShotActivation(int ownerOriginalId) =>
        MaterializeCargo(ownerOriginalId, Level3CargoState.PendingShotActivation);

    public IReadOnlyList<int> MaterializeStillbornCargo(int ownerOriginalId) =>
        MaterializeCargo(ownerOriginalId, Level3CargoState.Active);

    public void ActivateShotCargo(int cargoId) =>
        TransitionCargo(cargoId, Level3CargoState.PendingShotActivation, Level3CargoState.Active);

    public void MarkCargoDestroyed(int cargoId) =>
        MarkCargoTerminal(cargoId, Level3CargoState.Destroyed);

    public void MarkCargoRimCleared(int cargoId) =>
        MarkCargoTerminal(cargoId, Level3CargoState.RimCleared);

    public void MarkCargoEgg(int cargoId) =>
        TransitionCargo(cargoId, Level3CargoState.ReentryReserve, Level3CargoState.Egg);

    public void MarkCargoActive(int cargoId) =>
        TransitionCargo(cargoId, Level3CargoState.Egg, Level3CargoState.Active);

    public void RebuildAfterDeath()
    {
        HashSet<int> unresolvedOriginals = UnresolvedOriginalIds.ToHashSet();
        HashSet<int> unresolvedCargo = UnresolvedCargoIds.ToHashSet();
        Dictionary<int, Level3OriginalType> selectedTypes = originals
            .Where(pair => pair.Value.Type != Level3OriginalType.Unselected)
            .ToDictionary(pair => pair.Key, pair => pair.Value.Type);
        Dictionary<int, Level3OriginalState> terminalOriginals = originals
            .Where(pair => IsTerminal(pair.Value.State))
            .ToDictionary(pair => pair.Key, pair => pair.Value.State);
        Dictionary<int, Level3CargoState> terminalCargo = cargo
            .Where(pair => IsTerminal(pair.Value.State))
            .ToDictionary(pair => pair.Key, pair => pair.Value.State);
        HashSet<(int Owner, int Ordinal)> latentCargo = cargo.Values
            .Where(entry => entry.State == Level3CargoState.Latent)
            .Select(entry => (entry.OwnerOriginalId, entry.Ordinal))
            .ToHashSet();

        // Keep ownership and terminal outcomes; only unresolved materialized units re-enter.
        foreach (OriginalEntry entry in originals.Values)
        {
            if (!IsTerminal(entry.State))
                entry.State = Level3OriginalState.Reserve;
        }

        foreach (CargoEntry entry in cargo.Values)
        {
            if (entry.State != Level3CargoState.Latent &&
                !IsTerminal(entry.State))
                entry.State = Level3CargoState.ReentryReserve;
        }

        ValidateState();
        if (!unresolvedOriginals.SetEquals(UnresolvedOriginalIds) ||
            !unresolvedCargo.SetEquals(UnresolvedCargoIds) ||
            !selectedTypes.All(pair => GetOriginalType(pair.Key) == pair.Value) ||
            !terminalOriginals.All(pair => GetOriginalState(pair.Key) == pair.Value) ||
            !terminalCargo.All(pair => GetCargoState(pair.Key) == pair.Value) ||
            !latentCargo.SetEquals(cargo.Values
                .Where(entry => entry.State == Level3CargoState.Latent)
                .Select(entry => (entry.OwnerOriginalId, entry.Ordinal))))
            throw new InvalidOperationException("Level 3 death reconstruction changed a committed identity set.");
    }

    public void ValidateRepresentation(IEnumerable<Level3Identity> represented)
    {
        ArgumentNullException.ThrowIfNull(represented);
        ValidateState();

        HashSet<Level3Identity> actual = [];
        foreach (Level3Identity identity in represented)
        {
            if (!actual.Add(identity))
                throw new InvalidOperationException($"Identity {identity} is represented more than once.");

            if (identity.Kind == Level3IdentityKind.Original)
                _ = GetOriginal(identity.Id);
            else if (identity.Kind == Level3IdentityKind.Cargo)
                _ = GetCargo(identity.Id);
            else
                throw new InvalidOperationException($"Unknown identity kind {identity.Kind}.");
        }

        HashSet<Level3Identity> expected = [];
        foreach ((int id, OriginalEntry entry) in originals)
        {
            if (!IsTerminal(entry.State))
                expected.Add(new Level3Identity(Level3IdentityKind.Original, id));
        }

        foreach ((int id, CargoEntry entry) in cargo)
        {
            if (!IsTerminal(entry.State) && entry.State != Level3CargoState.Latent)
                expected.Add(new Level3Identity(Level3IdentityKind.Cargo, id));
        }

        if (!expected.SetEquals(actual))
        {
            Level3Identity[] missing = expected.Except(actual).OrderBy(identity => identity.Kind)
                .ThenBy(identity => identity.Id).ToArray();
            Level3Identity[] extraneous = actual.Except(expected).OrderBy(identity => identity.Kind)
                .ThenBy(identity => identity.Id).ToArray();
            throw new InvalidOperationException(
                $"Level 3 representation mismatch. Missing: [{string.Join(", ", missing)}]; " +
                $"extraneous: [{string.Join(", ", extraneous)}].");
        }
    }

    public void ValidateRuntimeRepresentations(
        IEnumerable<Level3WaveRepresentation> represented)
    {
        ArgumentNullException.ThrowIfNull(represented);
        ValidateState();

        Dictionary<Level3Identity, Level3RepresentationLocation> actual = [];
        foreach (Level3WaveRepresentation representation in represented)
        {
            Level3Identity identity = representation.Identity;
            if (!actual.TryAdd(identity, representation.Location))
                throw new InvalidOperationException($"Identity {identity} is represented more than once.");

            if (identity.Kind == Level3IdentityKind.Original)
                _ = GetOriginal(identity.Id);
            else if (identity.Kind == Level3IdentityKind.Cargo)
                _ = GetCargo(identity.Id);
            else
                throw new InvalidOperationException($"Unknown identity kind {identity.Kind}.");
        }

        Dictionary<Level3Identity, Level3RepresentationLocation> expected = [];
        foreach ((int id, OriginalEntry entry) in originals)
        {
            if (IsTerminal(entry.State))
                continue;

            Level3RepresentationLocation location = entry.State switch
            {
                Level3OriginalState.Reserve => Level3RepresentationLocation.Reserve,
                Level3OriginalState.Egg => Level3RepresentationLocation.Egg,
                Level3OriginalState.ActiveFlipper or Level3OriginalState.ActiveTanker or
                    Level3OriginalState.ActiveSpiker =>
                    Level3RepresentationLocation.ActiveEnemy,
                _ => throw new InvalidOperationException($"Unsupported original state {entry.State}.")
            };
            expected.Add(new Level3Identity(Level3IdentityKind.Original, id), location);
        }

        foreach ((int id, CargoEntry entry) in cargo)
        {
            if (IsTerminal(entry.State) || entry.State == Level3CargoState.Latent)
                continue;

            Level3RepresentationLocation location = entry.State switch
            {
                Level3CargoState.PendingShotActivation =>
                    Level3RepresentationLocation.PendingActivation,
                Level3CargoState.ReentryReserve => Level3RepresentationLocation.Reserve,
                Level3CargoState.Egg => Level3RepresentationLocation.Egg,
                Level3CargoState.Active => Level3RepresentationLocation.ActiveEnemy,
                _ => throw new InvalidOperationException($"Unsupported cargo state {entry.State}.")
            };
            expected.Add(new Level3Identity(Level3IdentityKind.Cargo, id), location);
        }

        if (expected.Count != actual.Count ||
            expected.Any(pair => !actual.TryGetValue(pair.Key, out Level3RepresentationLocation location) ||
                                 location != pair.Value))
        {
            string expectedText = string.Join(", ", expected.OrderBy(pair => pair.Key.Kind)
                .ThenBy(pair => pair.Key.Id).Select(pair => $"{pair.Key}@{pair.Value}"));
            string actualText = string.Join(", ", actual.OrderBy(pair => pair.Key.Kind)
                .ThenBy(pair => pair.Key.Id).Select(pair => $"{pair.Key}@{pair.Value}"));
            throw new InvalidOperationException(
                $"Level 3 runtime representation mismatch. Expected [{expectedText}], actual [{actualText}].");
        }
    }

    public void ValidateState()
    {
        if (originals.Count == 0 ||
            !originals.Keys.Order().SequenceEqual(Enumerable.Range(0, originals.Count)))
            throw new InvalidOperationException("Original identity set is not the configured contiguous namespace.");

        foreach ((int id, OriginalEntry entry) in originals)
        {
            int[] ownedCargo = GetCargoIdsForOriginal(id).ToArray();
            if (entry.Type == Level3OriginalType.Tanker && ownedCargo.Length != 2)
                throw new InvalidOperationException($"Tanker original {id} must own exactly two cargo identities.");
            if (entry.Type != Level3OriginalType.Tanker && ownedCargo.Length != 0)
                throw new InvalidOperationException($"Non-Tanker original {id} cannot own cargo identities.");
            if (entry.State is Level3OriginalState.Destroyed or
                Level3OriginalState.Escaped or Level3OriginalState.RimCleared &&
                ownedCargo.Any(cargoId => GetCargoState(cargoId) == Level3CargoState.Latent))
                throw new InvalidOperationException($"Terminal carrier {id} cannot retain latent cargo.");
            if (entry.State is Level3OriginalState.ActiveFlipper &&
                entry.Type != Level3OriginalType.Flipper)
                throw new InvalidOperationException($"Active Flipper original {id} has inconsistent selected type.");
            if (entry.State is Level3OriginalState.ActiveTanker &&
                entry.Type != Level3OriginalType.Tanker)
                throw new InvalidOperationException($"Active Tanker original {id} has inconsistent selected type.");
            if (entry.State is Level3OriginalState.ActiveSpiker &&
                entry.Type != Level3OriginalType.Spiker)
                throw new InvalidOperationException($"Active Spiker original {id} has inconsistent selected type.");
        }

        foreach ((int id, CargoEntry entry) in cargo)
        {
            if (id != CargoUnitId(entry.OwnerOriginalId, entry.Ordinal) ||
                entry.Ordinal is < 0 or > 1 ||
                GetOriginalType(entry.OwnerOriginalId) != Level3OriginalType.Tanker)
                throw new InvalidOperationException($"Cargo identity {id} has invalid ownership.");

            if (entry.State == Level3CargoState.Latent &&
                IsTerminal(GetOriginalState(entry.OwnerOriginalId)))
                throw new InvalidOperationException($"Latent cargo {id} belongs to a terminal carrier.");
        }

        foreach (IGrouping<int, CargoEntry> pair in cargo.Values.GroupBy(entry => entry.OwnerOriginalId))
        {
            if (pair.Count() != 2 ||
                !pair.Select(entry => entry.Ordinal).Order().SequenceEqual(new[] { 0, 1 }))
                throw new InvalidOperationException($"Tanker original {pair.Key} does not own one unique cargo pair.");
        }
    }

    public static int CargoUnitId(int ownerOriginalId, int ordinal)
    {
        // The numeric adapter is unique inside the cargo namespace; ownership stays explicit.
        ArgumentOutOfRangeException.ThrowIfNegative(ownerOriginalId);
        if (ordinal is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(ordinal));
        return checked(2 * ownerOriginalId + ordinal);
    }

    private IReadOnlyList<int> MaterializeCargo(
        int ownerOriginalId,
        Level3CargoState destination)
    {
        if (GetOriginalType(ownerOriginalId) != Level3OriginalType.Tanker)
            throw new InvalidOperationException($"Original {ownerOriginalId} is not a selected Tanker.");

        List<int> ids = GetCargoIdsForOriginal(ownerOriginalId).ToList();
        if (ids.Count != 2)
            throw new InvalidOperationException($"Tanker original {ownerOriginalId} must own exactly two cargo identities.");

        foreach (int id in ids)
            TransitionCargo(id, Level3CargoState.Latent, destination);
        return ids;
    }

    private void MarkOriginalTerminal(int id, Level3OriginalState destination)
    {
        OriginalEntry entry = GetOriginal(id);
        if (IsTerminal(entry.State))
            throw new InvalidOperationException($"Original {id} is already terminal as {entry.State}.");
        if (destination == Level3OriginalState.Escaped &&
            (entry.Type != Level3OriginalType.Tanker ||
             entry.State != Level3OriginalState.ActiveTanker))
            throw new InvalidOperationException($"Only an active Tanker original can escape naturally.");
        if (destination == Level3OriginalState.Destroyed &&
            entry.State is not (Level3OriginalState.ActiveFlipper or Level3OriginalState.ActiveTanker or
                Level3OriginalState.ActiveSpiker))
            throw new InvalidOperationException($"Only an active original can be destroyed.");
        if (destination == Level3OriginalState.RimCleared &&
            entry.State is not (Level3OriginalState.ActiveFlipper or Level3OriginalState.ActiveTanker or
                Level3OriginalState.ActiveSpiker))
            throw new InvalidOperationException($"Only an active original can be rim-cleared.");
        if (entry.Type == Level3OriginalType.Tanker &&
            GetCargoIdsForOriginal(id).Any(cargoId => GetCargoState(cargoId) == Level3CargoState.Latent))
            throw new InvalidOperationException($"Tanker original {id} cannot become terminal before its cargo materializes.");
        entry.State = destination;
    }

    private void MarkCargoTerminal(int id, Level3CargoState destination)
    {
        CargoEntry entry = GetCargo(id);
        if (IsTerminal(entry.State))
            throw new InvalidOperationException($"Cargo identity {id} is already terminal as {entry.State}.");
        if (entry.State != Level3CargoState.Active)
            throw new InvalidOperationException($"Only active cargo identity {id} can become terminal.");
        entry.State = destination;
    }

    private void TransitionOriginal(
        int id,
        Level3OriginalState expected,
        Level3OriginalState destination)
    {
        OriginalEntry entry = GetOriginal(id);
        if (entry.State != expected)
            throw InvalidTransition("original", id, entry.State, destination);
        entry.State = destination;
    }

    private void TransitionCargo(
        int id,
        Level3CargoState expected,
        Level3CargoState destination)
    {
        CargoEntry entry = GetCargo(id);
        if (entry.State != expected)
            throw InvalidTransition("cargo", id, entry.State, destination);
        entry.State = destination;
    }

    private OriginalEntry GetOriginal(int id) =>
        originals.TryGetValue(id, out OriginalEntry? entry)
            ? entry
            : throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown original identity.");

    private CargoEntry GetCargo(int id) =>
        cargo.TryGetValue(id, out CargoEntry? entry)
            ? entry
            : throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown cargo identity.");

    private static bool IsTerminal(Level3OriginalState state) =>
        state is Level3OriginalState.Destroyed or
            Level3OriginalState.Escaped or Level3OriginalState.RimCleared;

    private static bool IsTerminal(Level3CargoState state) =>
        state is Level3CargoState.Destroyed or Level3CargoState.RimCleared;

    private static InvalidOperationException InvalidTransition(
        string identityType,
        int id,
        Enum current,
        Enum destination) =>
        new($"Cannot transition {identityType} {id} from {current} to {destination}.");
}
