namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// DTO for creating a new ticket.
/// </summary>
/// <param name="FlightId">ID of the associated flight.</param>
/// <param name="PassengerId">ID of the associated passenger.</param>
/// <param name="SeatNumber">Seat number (e.g., 12A).</param>
/// <param name="HandLuggage">Indicates if hand luggage is present.</param>
/// <param name="BaggageWeight">Total baggage weight in kilograms (null if no baggage).</param>
public record CreateTicketDto
(
    int FlightId,
    int PassengerId,
    string SeatNumber,
    bool HandLuggage,
    double? BaggageWeight
    );