using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;
using System.Text.RegularExpressions;

namespace Airline.Application.Services;

/// <summary>
/// Provides business logic for managing passengers in the airline system.
/// Handles CRUD operations, validation of personal data (name must not contain digits),
/// and retrieval of associated tickets and flights.
/// </summary>
public class PassengerService(
    IRepository<Passenger, int> passengerRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IMapper mapper
) : IPassengerService
{
    /// <summary>
    /// Creates a new passenger.
    /// Validates that the passenger name is not empty and does not contain digits.
    /// </summary>
    /// <param name="dto">The passenger creation data transfer object.</param>
    /// <returns>The created passenger DTO.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the passenger name is empty, whitespace, or contains digits.
    /// </exception>
    public async Task<PassengerDto> CreateAsync(CreatePassengerDto dto)
    {
        var passenger = mapper.Map<Passenger>(dto);
        var maxId = 0;
        var last = await passengerRepository.GetAllAsync();
        if (last.Any())
        {
            maxId = last.Max(p => p.Id);
        }
        passenger.Id = maxId + 1;
        if (string.IsNullOrWhiteSpace(dto.PassengerName) || Regex.IsMatch(dto.PassengerName, @"\d"))
            throw new ArgumentException("Passenger name must not be empty or contain digits.");
        var created = await passengerRepository.CreateAsync(passenger);
        return mapper.Map<PassengerDto>(created);
    }

    /// <summary>
    /// Retrieves a passenger by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the passenger.</param>
    /// <returns>The passenger DTO if found; otherwise, null.</returns>
    public async Task<PassengerDto?> GetByIdAsync(int id)
    {
        var entity = await passengerRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<PassengerDto>(entity);
    }

    /// <summary>
    /// Retrieves all passengers in the system.
    /// </summary>
    /// <returns>A list of all passenger DTOs.</returns>
    public async Task<IList<PassengerDto>> GetAllAsync() =>
        (await passengerRepository.GetAllAsync()).Select(mapper.Map<PassengerDto>).ToList();

    /// <summary>
    /// Updates an existing passenger with new data.
    /// Validates that the passenger name is not empty and does not contain digits.
    /// </summary>
    /// <param name="dto">The updated passenger data transfer object.</param>
    /// <param name="id">The unique identifier of the passenger to update.</param>
    /// <returns>The updated passenger DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the passenger does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the passenger name is empty, whitespace, or contains digits.
    /// </exception>
    public async Task<PassengerDto> UpdateAsync(CreatePassengerDto dto, int id)
    {
        var existing = await passengerRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Passenger '{id}' not found.");
        mapper.Map(dto, existing);
        if (string.IsNullOrWhiteSpace(dto.PassengerName) || Regex.IsMatch(dto.PassengerName, @"\d"))
            throw new ArgumentException("Passenger name must not be empty or contain digits.");
        var updated = await passengerRepository.UpdateAsync(existing);
        return mapper.Map<PassengerDto>(updated);
    }

    /// <summary>
    /// Deletes a passenger by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the passenger to delete.</param>
    /// <returns>True if the passenger was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id) => await passengerRepository.DeleteAsync(id);

    /// <summary>
    /// Retrieves all tickets associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of ticket DTOs linked to the passenger.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the passenger does not exist.
    /// </exception>
    public async Task<IList<TicketDto>> GetTicketsAsync(int passengerId)
    {
        var passengerExists = await passengerRepository.GetAsync(passengerId) != null;
        if (!passengerExists)
            throw new KeyNotFoundException($"Passenger with ID '{passengerId}' not found.");
        var all = await ticketRepository.GetAllAsync();
        return all.Where(t => t.PassengerId == passengerId)
                  .Select(mapper.Map<TicketDto>).ToList();
    }

    /// <summary>
    /// Retrieves all flights associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of flight DTOs linked to the passenger.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the passenger does not exist.
    /// </exception>
    public async Task<IList<FlightDto>> GetFlightsAsync(int passengerId)
    {
        var passengerExists = await passengerRepository.GetAsync(passengerId) != null;
        if (!passengerExists)
            throw new KeyNotFoundException($"Passenger with ID '{passengerId}' not found.");
        var ticketDtos = await GetTicketsAsync(passengerId);
        var flightIds = ticketDtos.Select(t => t.FlightId).ToHashSet();
        var allFlights = await flightRepository.GetAllAsync();
        return allFlights.Where(f => flightIds.Contains(f.Id))
                         .Select(mapper.Map<FlightDto>).ToList();
    }
}