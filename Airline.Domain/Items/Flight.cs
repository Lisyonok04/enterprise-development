namespace Airline.Domain.Items;

/// <summary>
/// The class for flight description and information about it.
/// </summary>
public class Flight
{
    /// <summary>
    /// Unique flight's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Flight's code.
    /// </summary>
    public required string FlightCode { get; set; }

    /// <summary>
    /// The place of departure.
    /// </summary>
    public required string DepartureCity { get; set; }

    /// <summary>
    /// The place of arrival.
    /// </summary>
    public required string ArrivalCity { get; set; }

    /// <summary>
    /// Date and time of the arrival.
    /// </summary>
    public DateTime? ArrivalDateTime { get; set; }

    /// <summary>
    /// Flight's departure date and time.
    /// </summary>
    public DateTime? DepartureDateTime { get; set; }

    /// <summary>
    /// Flight's travel time.
    /// </summary>
    public TimeSpan? TravelTime { get; set; }

    /// <summary>
    /// The model of plane.
    /// </summary>
    public required PlaneModel? Model { get; set; }

    /// <summary>
    /// The id of the model.
    /// </summary>
    public int ModelId { get; set; } // ← ссылка на PlaneModel
}