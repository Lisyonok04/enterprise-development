using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class TicketService(
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : ITicketService
{
    public async Task<TicketDto> CreateAsync(CreateTicketDto dto)
    {
        if (await flightRepository.GetAsync(dto.FlightId) == null)
            throw new KeyNotFoundException($"Flight '{dto.FlightId}' not found.");
        if (await passengerRepository.GetAsync(dto.PassengerId) == null)
            throw new KeyNotFoundException($"Passenger '{dto.PassengerId}' not found.");

        var ticket = mapper.Map<Ticket>(dto);
        var created = await ticketRepository.CreateAsync(ticket);
        return mapper.Map<TicketDto>(created);
    }

    public async Task<TicketDto?> GetByIdAsync(int id)
    {
        var entity = await ticketRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<TicketDto>(entity);
    }

    public async Task<IList<TicketDto>> GetAllAsync() =>
        (await ticketRepository.GetAllAsync()).Select(mapper.Map<TicketDto>).ToList();

    public async Task<TicketDto> UpdateAsync(CreateTicketDto dto, int id)
    {
        var existing = await ticketRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Ticket '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await ticketRepository.UpdateAsync(existing);
        return mapper.Map<TicketDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await ticketRepository.DeleteAsync(id);

    public async Task<FlightDto> GetFlightAsync(int ticketId)
    {
        var ticket = await ticketRepository.GetAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");
        var flight = await flightRepository.GetAsync(ticket.FlightId)
            ?? throw new InvalidOperationException($"Flight '{ticket.FlightId}' missing.");
        return mapper.Map<FlightDto>(flight);
    }

    public async Task<PassengerDto> GetPassengerAsync(int ticketId)
    {
        var ticket = await ticketRepository.GetAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");
        var passenger = await passengerRepository.GetAsync(ticket.PassengerId)
            ?? throw new InvalidOperationException($"Passenger '{ticket.PassengerId}' missing.");
        return mapper.Map<PassengerDto>(passenger);
    }
}