using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;

namespace Airline.Application.Contracts.Flight;

/// <summary>
/// Service contract for managing flights in the airline system.
/// Extends the generic CRUD interface with flight-specific analytical operations.
/// </summary>
public interface IFlightService : IApplicationService<FlightDto, CreateFlightDto, int>
{
    /// <summary>
    /// Retrieves the aircraft model associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>The aircraft model DTO linked to the flight.</returns>
    public Task<PlaneModelDto> GetPlaneModelAsync(int flightId);

    /// <summary>
    /// Retrieves all tickets associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of ticket DTOs linked to the flight.</returns>
    public Task<IList<TicketDto>> GetTicketsAsync(int flightId);

    /// <summary>
    /// Retrieves all passengers associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of passenger DTOs linked to the flight.</returns>
    public Task<IList<PassengerDto>> GetPassengersAsync(int flightId);
}