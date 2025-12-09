using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Ticket;

namespace Airline.Application.Contracts.Passenger;

public interface IPassengerService : IApplicationService<PassengerDto, CreatePassengerDto, int>
{
    public Task<IList<TicketDto>> GetTicketsAsync(int passengerId);
    public Task<IList<FlightDto>> GetFlightsAsync(int passengerId);
}