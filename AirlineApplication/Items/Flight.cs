namespace AirlineApplication.Items

/// <summary>
/// The class for flight description and information about it.
/// </summary>
public class Flight
{
    /// <summary>
    /// Unique flight's ID.
    /// </summary>
    public required int ID { get; set; }

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
    /// Date of the departure.
    /// </summary>
    public DateTime? DepartureDate { get; set; }

    /// <summary>
    /// Date of the arrival.
    /// </summary>
    public DateTime? ArrivalDate { get; set; }

    /// <summary>
    /// Flight's eparture time.
    /// </summary>
    public TimeSpan? DepartureTime { get; set; };

    /// <summary>
    /// Flight's travel time.
    /// </summary>
    public TimeSpan? TravelTime { get; set; };

    /// <summary>
    /// The type of plane for the flight.
    /// </summary>
    public required PlaneModel Model { get; set; } 
}
