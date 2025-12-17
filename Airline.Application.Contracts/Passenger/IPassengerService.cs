using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Ticket;

namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// Service contract for managing passengers in the airline system.
/// Extends the generic CRUD interface with passenger-specific analytical operations.
/// </summary>
public interface IPassengerService : IApplicationService<PassengerDto, CreatePassengerDto, int>
{
    /// <summary>
    /// Retrieves all tickets associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of ticket DTOs linked to the passenger.</returns>
    public Task<IList<TicketDto>> GetTicketsAsync(int passengerId);

    /// <summary>
    /// Retrieves all flights associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of flight DTOs linked to the passenger.</returns>
    public Task<IList<FlightDto>> GetFlightsAsync(int passengerId);
}