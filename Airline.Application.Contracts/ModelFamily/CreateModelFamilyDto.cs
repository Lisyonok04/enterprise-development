namespace Airline.Application.Contracts.ModelFamily;

/// <summary>
/// DTO for creating a new model family.
/// </summary>
/// <param name="NameOfFamily">Name of the model family.</param>
/// <param name="ManufacturerName">Manufacturer of the model family.</param>
public record CreateModelFamilyDto(string NameOfFamily, string ManufacturerName);