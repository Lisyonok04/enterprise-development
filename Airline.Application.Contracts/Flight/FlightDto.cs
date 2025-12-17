namespace Airline.Application.Contracts.Flight;

/// <summary>
/// Data Transfer Object (DTO) representing a flight in the airline system.
/// Used for read operations and data exchange with clients.
/// </summary>
/// <param name="Id">The unique identifier of the flight.</param>
/// <param name="FlightCode">The flight code (e.g., "SU101").</param>
/// <param name="DepartureCity">The city of departure.</param>
/// <param name="ArrivalCity">The city of arrival.</param>
/// <param name="DepartureDateTime">The scheduled departure date and time (local time).</param>
/// <param name="ArrivalDateTime">The scheduled arrival date and time (local time).</param>
/// <param name="ModelId">The unique identifier of the associated aircraft model.</param>
public record FlightDto(
    int Id,
    string FlightCode,
    string DepartureCity,
    string ArrivalCity,
    DateTime DepartureDateTime,
    DateTime ArrivalDateTime,
    int ModelId
);