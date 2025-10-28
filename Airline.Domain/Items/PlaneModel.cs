namespace Airline.Domain.Items;

/// <summary>
/// The class for information about a plane model.
/// </summary>
public class PlaneModel
{
    /// <summary>
    /// Unique plane model's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of plane model.
    /// </summary>
    public required string ModelName { get; set; }

    /// <summary>
    /// The model family of the plane.
    /// </summary>
    public required ModelFamily PlaneFamily { get; set; }

    /// <summary>
    /// The max flight range of the plane model.
    /// </summary>
    public required double MaxRange { get; set; }

    /// <summary>
    /// The passenger capacity of the plane model.
    /// </summary>
    public required int PassengerCapacity { get; set; }

    /// <summary>
    /// The cargo capacity of the plane model (tons).
    /// </summary>
    public required double CargoCapacity { get; set; }
}