namespace Airline.Application.Contracts.ModelFamily;

/// <summary>
/// DTO representing a model family.
/// Contains basic information about the family.
/// </summary>
/// <param name="Id">Unique identifier of the model family.</param>
/// <param name="NameOfFamily">Name of the model family.</param>
/// <param name="ManufacturerName">Manufacturer of the model family.</param>
public record ModelFamilyDto(int Id, string NameOfFamily, string ManufacturerName);