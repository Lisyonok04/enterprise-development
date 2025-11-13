namespace Airline.Application.Contracts.PlaneModel;

/// <summary>
/// DTO for creating a new plane model.
/// </summary>
/// <param name="ModelName">Name of the plane model.</param>
/// <param name="PlaneFamilyId">ID of the associated model family.</param>
/// <param name="MaxRange">Maximum flight range (km).</param>
/// <param name="PassengerCapacity">Passenger capacity.</param>
/// <param name="CargoCapacity">Cargo capacity (tons).</param>
public record CreatePlaneModelDto(
    string ModelName,
    string PlaneFamilyId,
    double MaxRange,
    double PassengerCapacity,
    double CargoCapacity);