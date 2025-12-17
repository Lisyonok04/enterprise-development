namespace Airline.Application.Contracts.PlaneModel;

/// <summary>
/// Data Transfer Object (DTO) for creating a new aircraft model.
/// Contains technical specifications and references to its model family.
/// </summary>
/// <param name="ModelName">The name of the aircraft model (e.g., "A320").</param>
/// <param name="ModelFamilyId">The unique identifier of the associated model family.</param>
/// <param name="MaxRange">The maximum flight range in kilometers (must be non-negative).</param>
/// <param name="PassengerCapacity">The number of passenger seats (must be non-negative).</param>
/// <param name="CargoCapacity">The cargo capacity in tons (must be non-negative).</param>
public record CreatePlaneModelDto(
    string ModelName,
    int ModelFamilyId,
    double MaxRange,
    double PassengerCapacity,
    double CargoCapacity
);