using Airline.Domain.Items;
using Airline.Domain;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for aircraft model family entities.
/// Provides data access operations for the ModelFamily domain model using Entity Framework Core with MongoDB.
/// </summary>
public class ModelFamilyRepository(AirlineDbContext context) : IRepository<ModelFamily, int>
{
    /// <summary>
    /// Creates a new aircraft model family in the data store.
    /// </summary>
    /// <param name="entity">The model family entity to create.</param>
    /// <returns>The created model family entity with assigned identifier.</returns>
    public async Task<ModelFamily> CreateAsync(ModelFamily entity)
    {
        var entry = await context.ModelFamilies.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes an aircraft model family from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model family to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the model family was successfully deleted;
    /// otherwise, <see langword="false"/> if the family was not found.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.ModelFamilies.FindAsync(id);
        if (entity == null) return false;

        context.ModelFamilies.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves an aircraft model family from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model family.</param>
    /// <returns>
    /// The model family entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<ModelFamily?> GetAsync(int id) =>
        await context.ModelFamilies.FindAsync(id);

    /// <summary>
    /// Retrieves all aircraft model families from the data store.
    /// </summary>
    /// <returns>A list of all model family entities.</returns>
    public async Task<IList<ModelFamily>> GetAllAsync() =>
        await context.ModelFamilies.ToListAsync();

    /// <summary>
    /// Updates an existing aircraft model family in the data store.
    /// </summary>
    /// <param name="entity">The model family entity with updated data.</param>
    /// <returns>The updated model family entity.</returns>
    public async Task<ModelFamily> UpdateAsync(ModelFamily entity)
    {
        context.ModelFamilies.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}