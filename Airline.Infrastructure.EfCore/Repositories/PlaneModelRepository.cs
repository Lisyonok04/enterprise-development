using Airline.Domain.Items;
using Airline.Domain;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for aircraft model entities.
/// Provides data access operations for the PlaneModel domain model using Entity Framework Core with MongoDB.
/// </summary>
public class PlaneModelRepository(AirlineDbContext context) : IRepository<PlaneModel, int>
{
    /// <summary>
    /// Creates a new aircraft model in the data store.
    /// </summary>
    /// <param name="entity">The aircraft model entity to create.</param>
    /// <returns>The created aircraft model entity with assigned identifier.</returns>
    public async Task<PlaneModel> CreateAsync(PlaneModel entity)
    {
        var entry = await context.PlaneModels.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes an aircraft model from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the aircraft model to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the aircraft model was successfully deleted;
    /// otherwise, <see langword="false"/> if the model was not found.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.PlaneModels.FindAsync(id);
        if (entity == null) return false;

        context.PlaneModels.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves an aircraft model from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the aircraft model.</param>
    /// <returns>
    /// The aircraft model entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<PlaneModel?> GetAsync(int id) =>
        await context.PlaneModels.FindAsync(id);

    /// <summary>
    /// Retrieves all aircraft models from the data store.
    /// </summary>
    /// <returns>A list of all aircraft model entities.</returns>
    public async Task<IList<PlaneModel>> GetAllAsync() =>
        await context.PlaneModels.ToListAsync();

    /// <summary>
    /// Updates an existing aircraft model in the data store.
    /// </summary>
    /// <param name="entity">The aircraft model entity with updated data.</param>
    /// <returns>The updated aircraft model entity.</returns>
    public async Task<PlaneModel> UpdateAsync(PlaneModel entity)
    {
        context.PlaneModels.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}