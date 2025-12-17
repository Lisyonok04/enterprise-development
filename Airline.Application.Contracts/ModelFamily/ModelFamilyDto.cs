namespace Airline.Application.Contracts.ModelFamily;

/// <summary>
/// Data Transfer Object (DTO) representing an aircraft model family in the airline system.
/// Used for read operations and data exchange with clients.
/// </summary>
/// <param name="Id">The unique identifier of the model family.</param>
/// <param name="NameOfFamily">The name of the aircraft model family (e.g., "A320 Family").</param>
/// <param name="ManufacturerName">The name of the manufacturer (e.g., "Airbus", "Boeing").</param>
public record ModelFamilyDto(
    int Id,
    string NameOfFamily,
    string ManufacturerName
);