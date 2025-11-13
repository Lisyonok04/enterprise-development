namespace Airline.Application.Contracts.PlaneModel;

/// <summary>
/// DTO representing a plane model.
/// </summary>
/// <param name="Id">Unique identifier of the plane model.</param>
/// <param name="ModelName">Name of the plane model.</param>
/// <param name="PlaneFamilyId">ID of the associated model family.</param>
/// <param name="MaxRange">Maximum flight range (km).</param>
/// <param name="PassengerCapacity">Passenger capacity.</param>
/// <param name="CargoCapacity">Cargo capacity (tons).</param>
public record PlaneModelDto(
    string Id,
    string ModelName,
    string PlaneFamilyId,
    double MaxRange,
    double PassengerCapacity,
    double CargoCapacity);