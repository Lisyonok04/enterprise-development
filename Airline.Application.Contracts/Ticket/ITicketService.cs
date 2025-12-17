using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

namespace Airline.Application.Contracts.Ticket;

/// <summary>
/// Service contract for managing tickets in the airline system.
/// Extends the generic CRUD interface with ticket-specific analytical operations.
/// </summary>
public interface ITicketService : IApplicationService<TicketDto, CreateTicketDto, int>
{
    /// <summary>
    /// Retrieves the flight associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The flight DTO linked to the ticket.</returns>
    public Task<FlightDto> GetFlightAsync(int ticketId);

    /// <summary>
    /// Retrieves the passenger associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The passenger DTO linked to the ticket.</returns>
    public Task<PassengerDto> GetPassengerAsync(int ticketId);
}