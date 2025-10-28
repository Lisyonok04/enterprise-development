namespace Airline.Domain.Items;

/// <summary>
/// The class for information about a model family.
/// </summary>
public class ModelFamily
{
    /// <summary>
    /// Unique model's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of model's family.
    /// </summary>
    public required string NameOfFamily { get; set; }

    /// <summary>
    /// The name of the model manufacturer.
    /// </summary>
    public required string ManufacturerName { get; set; }
}