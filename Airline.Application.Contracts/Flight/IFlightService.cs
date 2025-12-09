using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;

namespace Airline.Application.Contracts.Flight;

public interface IFlightService : IApplicationService<FlightDto, CreateFlightDto, int>
{

    public Task<PlaneModelDto> GetPlaneModelAsync(int flightId);
    public Task<IList<TicketDto>> GetTicketsAsync(int flightId);
    public Task<IList<PassengerDto>> GetPassengersAsync(int flightId);
}
