namespace Airline.Application.Contracts.ModelFamily;

/// <summary>
/// Data Transfer Object (DTO) for creating a new aircraft model family.
/// Represents a group of aircraft models with common design features.
/// </summary>
/// <param name="NameOfFamily">The name of the aircraft model family (e.g., "A320 Family").</param>
/// <param name="ManufacturerName">The name of the manufacturer (e.g., "Airbus", "Boeing").</param>
public record CreateModelFamilyDto(
    string NameOfFamily,
    string ManufacturerName
);