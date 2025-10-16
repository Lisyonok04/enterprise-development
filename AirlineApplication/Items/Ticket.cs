namespace AirlineApplication.Items

/// <summary>
/// The class for information about ticket.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Unique ticket's ID.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The connection between the ticket and the flight.
    /// </summary>
    public required Flight Flight { get; set; }

    /// <summary>
    /// The connection between the ticket and the passenger.
    /// </summary>
    public required Passenger Passenger{ get; set; }

    /// <summary>
    /// The passenger's seat number.
    /// </summary>
    public required string SeatNumber { get; set; }

    /// <summary>
    /// The flag to indicate if there is a hand luggage.
    /// </summary>
    public required bool HandLuggage { get; set; }

    /// <summary>
    /// Total baggage weight. (kilograms)
    /// </summary>
    public double? BaggageWeight { get; set; }
}
