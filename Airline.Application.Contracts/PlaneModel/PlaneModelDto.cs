namespace Airline.Application.Contracts.PlaneModel;

/// <summary>
/// Data Transfer Object (DTO) representing an aircraft model in the airline system.
/// Used for read operations and data exchange with clients.
/// </summary>
/// <param name="Id">The unique identifier of the aircraft model.</param>
/// <param name="ModelName">The name of the aircraft model (e.g., "A320").</param>
/// <param name="ModelFamilyId">The unique identifier of the associated model family.</param>
/// <param name="MaxRange">The maximum flight range in kilometers.</param>
/// <param name="PassengerCapacity">The number of passenger seats.</param>
/// <param name="CargoCapacity">The cargo capacity in tons.</param>
public record PlaneModelDto(
    int Id,
    string ModelName,
    int ModelFamilyId,
    double MaxRange,
    double PassengerCapacity,
    double CargoCapacity
);