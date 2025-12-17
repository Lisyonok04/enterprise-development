using Airline.Domain.Items;
using Airline.Domain;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for flight entities.
/// Provides data access operations for the Flight domain model using Entity Framework Core with MongoDB.
/// </summary>
public class FlightRepository(AirlineDbContext context) : IRepository<Flight, int>
{
    /// <summary>
    /// Creates a new flight in the data store.
    /// </summary>
    /// <param name="entity">The flight entity to create.</param>
    /// <returns>The created flight entity with assigned identifier.</returns>
    public async Task<Flight> CreateAsync(Flight entity)
    {
        var entry = await context.Flights.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes a flight from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the flight to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the flight was successfully deleted;
    /// otherwise, <see langword="false"/> if the flight was not found.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.Flights.FindAsync(id);
        if (entity == null) return false;

        context.Flights.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves a flight from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the flight.</param>
    /// <returns>
    /// The flight entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<Flight?> GetAsync(int id) =>
        await context.Flights.FindAsync(id);

    /// <summary>
    /// Retrieves all flights from the data store.
    /// </summary>
    /// <returns>A list of all flight entities.</returns>
    public async Task<IList<Flight>> GetAllAsync() =>
        await context.Flights.ToListAsync();

    /// <summary>
    /// Updates an existing flight in the data store.
    /// </summary>
    /// <param name="entity">The flight entity with updated data.</param>
    /// <returns>The updated flight entity.</returns>
    public async Task<Flight> UpdateAsync(Flight entity)
    {
        context.Flights.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}