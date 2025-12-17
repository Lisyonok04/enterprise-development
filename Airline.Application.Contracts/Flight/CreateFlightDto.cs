namespace Airline.Application.Contracts.Flight;

/// <summary>
/// Data Transfer Object (DTO) for creating a new flight.
/// Contains scheduling and routing information for an airline flight.
/// </summary>
/// <param name="FlightCode">The flight code (e.g., "SU101").</param>
/// <param name="DepartureCity">The city of departure.</param>
/// <param name="ArrivalCity">The city of arrival.</param>
/// <param name="DepartureDateTime">The scheduled departure date and time (local time).</param>
/// <param name="ArrivalDateTime">The scheduled arrival date and time (local time).</param>
/// <param name="ModelId">The unique identifier of the associated aircraft model.</param>
public record CreateFlightDto(
    string FlightCode,
    string DepartureCity,
    string ArrivalCity,
    DateTime DepartureDateTime,
    DateTime ArrivalDateTime,
    int ModelId
);