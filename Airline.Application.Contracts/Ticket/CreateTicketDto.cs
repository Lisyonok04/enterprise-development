namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// Data Transfer Object (DTO) for creating a new ticket.
/// Contains all required fields for ticket creation in the airline system.
/// </summary>
/// <param name="FlightId">The unique identifier of the associated flight.</param>
/// <param name="PassengerId">The unique identifier of the associated passenger.</param>
/// <param name="SeatNumber">The assigned seat number (e.g., "12A").</param>
/// <param name="HandLuggage">Indicates whether hand luggage is carried.</param>
/// <param name="BaggageWeight">The total checked baggage weight in kilograms (null if no baggage).</param>
public record CreateTicketDto(
    int FlightId,
    int PassengerId,
    string SeatNumber,
    bool HandLuggage,
    double? BaggageWeight
);