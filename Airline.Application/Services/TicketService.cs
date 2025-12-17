using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Provides business logic for managing tickets in the airline system.
/// Handles CRUD operations, validation, and retrieval of related entities (flight, passenger).
/// </summary>
public class TicketService(
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : ITicketService
{
    /// <summary>
    /// Creates a new ticket for a passenger on a specific flight.
    /// Validates that the flight and passenger exist and that baggage weight is non-negative.
    /// </summary>
    /// <param name="dto">The ticket creation data transfer object.</param>
    /// <returns>The created ticket DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the specified flight or passenger does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if baggage weight is negative.
    /// </exception>
    public async Task<TicketDto> CreateAsync(CreateTicketDto dto)
    {
        if (await flightRepository.GetAsync(dto.FlightId) == null)
            throw new KeyNotFoundException($"Flight '{dto.FlightId}' not found.");
        if (await passengerRepository.GetAsync(dto.PassengerId) == null)
            throw new KeyNotFoundException($"Passenger '{dto.PassengerId}' not found.");

        var ticket = mapper.Map<Ticket>(dto);
        var maxId = 0;
        var last = await ticketRepository.GetAllAsync();
        if (last.Any())
        {
            maxId = last.Max(t => t.Id);
        }
        ticket.Id = maxId + 1;
        if (dto.BaggageWeight < 0)
            throw new ArgumentException("BaggageWeight cannot be negative.");
        var created = await ticketRepository.CreateAsync(ticket);
        return mapper.Map<TicketDto>(created);
    }

    /// <summary>
    /// Retrieves a ticket by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <returns>The ticket DTO if found; otherwise, null.</returns>
    public async Task<TicketDto?> GetByIdAsync(int id)
    {
        var entity = await ticketRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<TicketDto>(entity);
    }

    /// <summary>
    /// Retrieves all tickets in the system.
    /// </summary>
    /// <returns>A list of all ticket DTOs.</returns>
    public async Task<IList<TicketDto>> GetAllAsync() =>
        (await ticketRepository.GetAllAsync()).Select(mapper.Map<TicketDto>).ToList();

    /// <summary>
    /// Updates an existing ticket with new data.
    /// Validates that baggage weight is non-negative.
    /// </summary>
    /// <param name="dto">The updated ticket data transfer object.</param>
    /// <param name="id">The unique identifier of the ticket to update.</param>
    /// <returns>The updated ticket DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the ticket does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if baggage weight is negative.
    /// </exception>
    public async Task<TicketDto> UpdateAsync(CreateTicketDto dto, int id)
    {
        var existing = await ticketRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Ticket '{id}' not found.");
        mapper.Map(dto, existing);
        if (dto.BaggageWeight < 0)
            throw new ArgumentException("BaggageWeight cannot be negative.");
        var updated = await ticketRepository.UpdateAsync(existing);
        return mapper.Map<TicketDto>(updated);
    }

    /// <summary>
    /// Deletes a ticket by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket to delete.</param>
    /// <returns>True if the ticket was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id) => await ticketRepository.DeleteAsync(id);

    /// <summary>
    /// Retrieves the flight associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The flight DTO associated with the ticket.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the ticket does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the associated flight is missing (data integrity issue).
    /// </exception>
    public async Task<FlightDto> GetFlightAsync(int ticketId)
    {
        var ticket = await ticketRepository.GetAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");
        var flight = await flightRepository.GetAsync(ticket.FlightId)
            ?? throw new InvalidOperationException($"Flight '{ticket.FlightId}' missing.");
        return mapper.Map<FlightDto>(flight);
    }

    /// <summary>
    /// Retrieves the passenger associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The passenger DTO associated with the ticket.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the ticket does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the associated passenger is missing (data integrity issue).
    /// </exception>
    public async Task<PassengerDto> GetPassengerAsync(int ticketId)
    {
        var ticket = await ticketRepository.GetAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket '{ticketId}' not found.");
        var passenger = await passengerRepository.GetAsync(ticket.PassengerId)
            ?? throw new InvalidOperationException($"Passenger '{ticket.PassengerId}' missing.");
        return mapper.Map<PassengerDto>(passenger);
    }
}