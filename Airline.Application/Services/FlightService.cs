using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class FlightService(
    IRepository<Flight, int> flightRepository,
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IFlightService
{
    public async Task<FlightDto> CreateAsync(CreateFlightDto dto)
    {
        if (await planeModelRepository.GetAsync(dto.ModelId) == null)
            throw new KeyNotFoundException($"Plane model '{dto.ModelId}' not found.");
        var flight = mapper.Map<Flight>(dto);
        var created = await flightRepository.CreateAsync(flight);
        return mapper.Map<FlightDto>(created);
    }

    public async Task<FlightDto?> GetByIdAsync(int id)
    {
        var entity = await flightRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<FlightDto>(entity);
    }

    public async Task<IList<FlightDto>> GetAllAsync() =>
        (await flightRepository.GetAllAsync()).Select(mapper.Map<FlightDto>).ToList();

    public async Task<FlightDto> UpdateAsync(CreateFlightDto dto, int id)
    {
        var existing = await flightRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Flight '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await flightRepository.UpdateAsync(existing);
        return mapper.Map<FlightDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await flightRepository.DeleteAsync(id);

    public async Task<PlaneModelDto> GetPlaneModelAsync(int flightId)
    {
        var flight = await flightRepository.GetAsync(flightId)
            ?? throw new KeyNotFoundException($"Flight '{flightId}' not found.");
        var model = await planeModelRepository.GetAsync(flight.ModelId)
            ?? throw new InvalidOperationException($"Model '{flight.ModelId}' missing.");
        return mapper.Map<PlaneModelDto>(model);
    }

    public async Task<IList<TicketDto>> GetTicketsAsync(int flightId)
    {
        var all = await ticketRepository.GetAllAsync();
        return all.Where(t => t.FlightId == flightId)
                  .Select(mapper.Map<TicketDto>).ToList();
    }

    public async Task<IList<PassengerDto>> GetPassengersAsync(int flightId)
    {
        var ticketDtos = await GetTicketsAsync(flightId);
        var passengerIds = ticketDtos.Select(t => t.PassengerId).ToHashSet();
        var allPassengers = await passengerRepository.GetAllAsync();
        return allPassengers.Where(p => passengerIds.Contains(p.Id))
                            .Select(mapper.Map<PassengerDto>).ToList();
    }
}