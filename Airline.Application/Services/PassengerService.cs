using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class PassengerService(
    IRepository<Passenger, int> passengerRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IMapper mapper
) : IPassengerService
{
    public async Task<PassengerDto> CreateAsync(CreatePassengerDto dto)
    {
        var passenger = mapper.Map<Passenger>(dto);
        var created = await passengerRepository.CreateAsync(passenger);
        return mapper.Map<PassengerDto>(created);
    }

    public async Task<PassengerDto?> GetByIdAsync(int id)
    {
        var entity = await passengerRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<PassengerDto>(entity);
    }

    public async Task<IList<PassengerDto>> GetAllAsync() =>
        (await passengerRepository.GetAllAsync()).Select(mapper.Map<PassengerDto>).ToList();

    public async Task<PassengerDto> UpdateAsync(CreatePassengerDto dto, int id)
    {
        var existing = await passengerRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Passenger '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await passengerRepository.UpdateAsync(existing);
        return mapper.Map<PassengerDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await passengerRepository.DeleteAsync(id);

    public async Task<IList<TicketDto>> GetTicketsAsync(int passengerId)
    {
        var all = await ticketRepository.GetAllAsync();
        return all.Where(t => t.PassengerId == passengerId)
                  .Select(mapper.Map<TicketDto>).ToList();
    }

    public async Task<IList<FlightDto>> GetFlightsAsync(int passengerId)
    {
        var ticketDtos = await GetTicketsAsync(passengerId);
        var flightIds = ticketDtos.Select(t => t.FlightId).ToHashSet();
        var allFlights = await flightRepository.GetAllAsync();
        return allFlights.Where(f => flightIds.Contains(f.Id))
                         .Select(mapper.Map<FlightDto>).ToList();
    }
}