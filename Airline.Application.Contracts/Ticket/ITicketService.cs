using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

namespace Airline.Application.Contracts.Ticket;

public interface ITicketService : IApplicationService<TicketDto, CreateTicketDto, int>
{
    public Task<FlightDto> GetFlightAsync(int ticketId);
    public Task<PassengerDto> GetPassengerAsync(int ticketId);
}
