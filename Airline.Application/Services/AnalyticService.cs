using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class AnalyticsService(
    IRepository<Flight, int> flightRepository,
    IRepository<Passenger, int> passengerRepository,
    IRepository<Ticket, int> ticketRepository,
    IMapper mapper
) : IAnalyticsService
{
    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5)
    {
        var flights = await flightRepository.GetAllAsync();
        return flights.Take(top).Select(f => mapper.Map<FlightDto>(f)).ToList();
    }

    public async Task<List<FlightDto>> GetFlightsWithMinTravelTimeAsync()
    {
        var flights = await flightRepository.GetAllAsync();
        var minTime = flights.Min(f => f.ArrivalDateTime - f.DepartureDateTime);
        return flights
            .Where(f => f.ArrivalDateTime - f.DepartureDateTime == minTime)
            .Select(f => mapper.Map<FlightDto>(f))
            .ToList();
    }

    public async Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival)
    {
        var flights = await flightRepository.GetAllAsync();
        return flights
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .Select(f => mapper.Map<FlightDto>(f))
            .ToList();
    }

    public async Task<IList<FlightDto>> GetFlightsOfModelWithinPeriod(string planeModelId, DateTime from, DateTime to)
    {
        var flights = await flightRepository.GetAllAsync();

        var result = flights
            .Where(f => f.ModelId == planeModelId
                        && f.DepartureDateTime >= from
                        && f.DepartureDateTime <= to)
            .ToList();

        return mapper.Map<IList<FlightDto>>(result);
    }
}
