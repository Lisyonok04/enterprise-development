namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// Data Transfer Object (DTO) representing a ticket in the airline system.
/// Used for read operations and data exchange with clients.
/// </summary>
/// <param name="Id">The unique identifier of the ticket.</param>
/// <param name="FlightId">The unique identifier of the associated flight.</param>
/// <param name="PassengerId">The unique identifier of the associated passenger.</param>
/// <param name="SeatNumber">The assigned seat number (e.g., "12A").</param>
/// <param name="HandLuggage">Indicates whether hand luggage is carried.</param>
/// <param name="BaggageWeight">The total checked baggage weight in kilograms (null if no baggage).</param>
public record TicketDto(
    int Id,
    int FlightId,
    int PassengerId,
    string SeatNumber,
    bool HandLuggage,
    double? BaggageWeight
);