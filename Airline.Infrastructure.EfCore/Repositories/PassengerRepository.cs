using Airline.Domain;
using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for passenger entities.
/// Provides data access operations for the Passenger domain model using Entity Framework Core with MongoDB.
/// </summary>
public class PassengerRepository(AirlineDbContext context) : IRepository<Passenger, int>
{
    /// <summary>
    /// Creates a new passenger in the data store.
    /// </summary>
    /// <param name="entity">The passenger entity to create.</param>
    /// <returns>The created passenger entity with assigned identifier.</returns>
    public async Task<Passenger> CreateAsync(Passenger entity)
    {
        var entry = await context.Passengers.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes a passenger from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the passenger to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the passenger was successfully deleted;
    /// otherwise, <see langword="false"/> if the passenger was not found.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.Passengers.FindAsync(id);
        if (entity == null) return false;

        context.Passengers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves a passenger from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the passenger.</param>
    /// <returns>
    /// The passenger entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<Passenger?> GetAsync(int id) =>
        await context.Passengers.FindAsync(id);

    /// <summary>
    /// Retrieves all passengers from the data store.
    /// </summary>
    /// <returns>A list of all passenger entities.</returns>
    public async Task<IList<Passenger>> GetAllAsync() =>
        await context.Passengers.ToListAsync();

    /// <summary>
    /// Updates an existing passenger in the data store.
    /// </summary>
    /// <param name="entity">The passenger entity with updated data.</param>
    /// <returns>The updated passenger entity.</returns>
    public async Task<Passenger> UpdateAsync(Passenger entity)
    {
        context.Passengers.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}