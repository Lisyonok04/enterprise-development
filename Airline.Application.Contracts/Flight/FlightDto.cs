namespace Airline.Application.Contracts.Flight;

/// <summary>
/// DTO representing a flight.
/// </summary>
/// <param name="Id">Unique identifier of the flight.</param>
/// <param name="FlightCode">Flight code (e.g., SU101).</param>
/// <param name="DepartureCity">City of departure.</param>
/// <param name="ArrivalCity">City of arrival.</param>
/// <param name="DepartureDateTime">Date and time of departure.</param>
/// <param name="ArrivalDateTime">Date and time of arrival.</param>
/// <param name="ModelId">ID of the plane model used for the flight.</param>
public record FlightDto(
    int Id,
    string FlightCode,
    string DepartureCity,
    string ArrivalCity,
    DateTime DepartureDateTime,
    DateTime ArrivalDateTime,
    int ModelId);