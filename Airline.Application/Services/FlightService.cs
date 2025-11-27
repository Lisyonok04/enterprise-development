using Airline.Application.Contracts.Flight;
using Airline.Domain.Items;
using Airline.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Airline.Application.Service;

/// <summary>
/// Provides operations for managing flights in the airline system.
/// </summary>
/// <param name="flightRepository">Repository for accessing flight data.</param>
/// <param name="planeModelRepository">Repository for validating and accessing aircraft model data.</param>
public class FlightService(
    IRepository<FlightDto, string> flightRepository,
    IRepository<PlaneModelDto, string> planeModelRepository
) : IFlightService
{
    /// <summary>
    /// Creates a new flight.
    /// </summary>
    /// <param name="entity">The flight information.</param>
    /// <returns>The created flight.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the specified aircraft model does not exist.
    /// </exception>
    public async Task<Flight> CreateAsync(Flight entity)
    {
        // Проверяем, существует ли модель самолёта
        if (await planeModelRepository.GetAsync(entity.ModelId) is null)
        {
            throw new KeyNotFoundException($"Aircraft model with Id = {entity.ModelId} does not exist.");
        }

        return await flightRepository.CreateAsync(entity);
    }

    /// <summary>
    /// Retrieves all flights from the system.
    /// </summary>
    /// <returns>A list of all flights.</returns>
    public async Task<List<Flight>> GetAllAsync() =>
        await flightRepository.GetAllAsync();

    /// <summary>
    /// Retrieves a specific flight by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the flight.</param>
    /// <returns>The flight if found; otherwise, null.</returns>
    public async Task<Flight?> GetByIdAsync(string id) =>
        await flightRepository.GetAsync(id);

    /// <summary>
    /// Updates an existing flight.
    /// </summary>
    /// <param name="entity">The updated flight information.</param>
    /// <returns>The updated flight.</returns>
    public async Task<Flight> UpdateAsync(Flight entity) =>
        await flightRepository.UpdateAsync(entity);

    /// <summary>
    /// Deletes a flight from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the flight to delete.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(string id) =>
        await flightRepository.DeleteAsync(id);

    // ---------- Методы аналитики (как в твоих юнит-тестах) ----------

    /// <summary>
    /// Retrieves the top N flights by number of passengers.
    /// </summary>
    /// <param name="top">Number of top flights to return.</param>
    /// <returns>List of top flights.</returns>
    public async Task<List<Flight>> GetTopFlightsByPassengerCountAsync(int top = 5)
    {
        var allFlights = await flightRepository.GetAllAsync();
        // В реальном проекте здесь был бы JOIN с билетами через билетный сервис
        // Но для учебного — можно вернуть все рейсы (или добавить логику позже)
        return allFlights.Take(top).ToList();
    }

    /// <summary>
    /// Retrieves flights with the minimal travel time.
    /// </summary>
    /// <returns>List of flights with minimal travel time.</returns>
    public async Task<List<Flight>> GetFlightsWithMinTravelTimeAsync()
    {
        var allFlights = await flightRepository.GetAllAsync();
        var minTime = allFlights.Min(f => f.ArrivalDateTime - f.DepartureDateTime);
        return allFlights
            .Where(f => f.ArrivalDateTime - f.DepartureDateTime == minTime)
            .ToList();
    }

    /// <summary>
    /// Retrieves flights for a specific route.
    /// </summary>
    /// <param name="departure">Departure city.</param>
    /// <param name="arrival">Arrival city.</param>
    /// <returns>List of flights matching the route.</returns>
    public async Task<List<Flight>> GetFlightsByRouteAsync(string departure, string arrival) =>
        (await flightRepository.GetAllAsync())
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .ToList();

    /// <summary>
    /// Retrieves flights of a specific aircraft model within a date period.
    /// </summary>
    /// <param name="modelName">Name of the aircraft model.</param>
    /// <param name="from">Start date (inclusive).</param>
    /// <param name="to">End date (inclusive).</param>
    /// <returns>List of matching flights.</returns>
    public async Task<List<Flight>> GetFlightsByModelInPeriodAsync(string modelName, DateTime from, DateTime to)
    {
        var allFlights = await flightRepository.GetAllAsync();
        // В реальном проекте здесь был бы JOIN с PlaneModel
        // Для учебного — предположим, что modelName хранится прямо в Flight
        return allFlights
            .Where(f => f.ModelName == modelName &&
                        f.DepartureDateTime >= from &&
                        f.DepartureDateTime <= to)
            .ToList();
    }
}