using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Provides business logic for managing flights in the airline system.
/// Handles CRUD operations, validation of aircraft model existence,
/// and retrieval of associated tickets, passengers, and aircraft models.
/// </summary>
public class FlightService(
    IRepository<Flight, int> flightRepository,
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IFlightService
{
    /// <summary>
    /// Creates a new flight.
    /// Validates that the associated aircraft model exists.
    /// </summary>
    /// <param name="dto">The flight creation data transfer object.</param>
    /// <returns>The created flight DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the specified aircraft model does not exist.
    /// </exception>
    public async Task<FlightDto> CreateAsync(CreateFlightDto dto)
    {
        if (await planeModelRepository.GetAsync(dto.ModelId) == null)
            throw new KeyNotFoundException($"Plane model '{dto.ModelId}' not found.");

        var flight = mapper.Map<Flight>(dto);
        var maxId = 0;
        var last = await flightRepository.GetAllAsync();
        if (last.Any())
        {
            maxId = last.Max(f => f.Id);
        }
        flight.Id = maxId + 1;
        var created = await flightRepository.CreateAsync(flight);
        return mapper.Map<FlightDto>(created);
    }

    /// <summary>
    /// Retrieves a flight by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the flight.</param>
    /// <returns>The flight DTO if found; otherwise, null.</returns>
    public async Task<FlightDto?> GetByIdAsync(int id)
    {
        var entity = await flightRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<FlightDto>(entity);
    }

    /// <summary>
    /// Retrieves all flights in the system.
    /// </summary>
    /// <returns>A list of all flight DTOs.</returns>
    public async Task<IList<FlightDto>> GetAllAsync() =>
        (await flightRepository.GetAllAsync()).Select(mapper.Map<FlightDto>).ToList();

    /// <summary>
    /// Updates an existing flight with new data.
    /// </summary>
    /// <param name="dto">The updated flight data transfer object.</param>
    /// <param name="id">The unique identifier of the flight to update.</param>
    /// <returns>The updated flight DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the flight does not exist.
    /// </exception>
    public async Task<FlightDto> UpdateAsync(CreateFlightDto dto, int id)
    {
        var existing = await flightRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Flight '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await flightRepository.UpdateAsync(existing);
        return mapper.Map<FlightDto>(updated);
    }

    /// <summary>
    /// Deletes a flight by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the flight to delete.</param>
    /// <returns>True if the flight was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id) => await flightRepository.DeleteAsync(id);

    /// <summary>
    /// Retrieves the aircraft model associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>The aircraft model DTO associated with the flight.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the flight does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the associated aircraft model is missing (data integrity issue).
    /// </exception>
    public async Task<PlaneModelDto> GetPlaneModelAsync(int flightId)
    {
        var flight = await flightRepository.GetAsync(flightId)
            ?? throw new KeyNotFoundException($"Flight '{flightId}' not found.");
        var model = await planeModelRepository.GetAsync(flight.ModelId)
            ?? throw new InvalidOperationException($"Model '{flight.ModelId}' missing.");
        return mapper.Map<PlaneModelDto>(model);
    }

    /// <summary>
    /// Retrieves all tickets associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of ticket DTOs linked to the flight.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the flight does not exist.
    /// </exception>
    public async Task<IList<TicketDto>> GetTicketsAsync(int flightId)
    {
        var flight = await flightRepository.GetAsync(flightId) != null;
        if (!flight)
            throw new KeyNotFoundException($"Flight with ID '{flightId}' not found.");
        var all = await ticketRepository.GetAllAsync();
        return all.Where(t => t.FlightId == flightId)
                  .Select(mapper.Map<TicketDto>).ToList();
    }

    /// <summary>
    /// Retrieves all passengers associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of passenger DTOs linked to the flight.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the flight does not exist.
    /// </exception>
    public async Task<IList<PassengerDto>> GetPassengersAsync(int flightId)
    {
        var flight = await flightRepository.GetAsync(flightId) != null;
        if (!flight)
            throw new KeyNotFoundException($"Flight with ID '{flightId}' not found.");
        var ticketDtos = await GetTicketsAsync(flightId);
        var passengerIds = ticketDtos.Select(t => t.PassengerId).ToHashSet();
        var allPassengers = await passengerRepository.GetAllAsync();
        return allPassengers.Where(p => passengerIds.Contains(p.Id))
                            .Select(mapper.Map<PassengerDto>).ToList();
    }
}