using Airline.Domain;
using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

/// <summary>
/// Repository implementation for ticket entities.
/// Provides data access operations for the Ticket domain model using Entity Framework Core with MongoDB.
/// </summary>
public class TicketRepository(AirlineDbContext context) : IRepository<Ticket, int>
{
    /// <summary>
    /// Creates a new ticket in the data store.
    /// </summary>
    /// <param name="entity">The ticket entity to create.</param>
    /// <returns>The created ticket entity with assigned identifier.</returns>
    public async Task<Ticket> CreateAsync(Ticket entity)
    {
        var entry = await context.Tickets.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    /// <summary>
    /// Deletes a ticket from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the ticket was successfully deleted;
    /// otherwise, <see langword="false"/> if the ticket was not found.
    /// </returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await context.Tickets.FindAsync(id);
        if (entity == null) return false;

        context.Tickets.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves a ticket from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ticket.</param>
    /// <returns>
    /// The ticket entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public async Task<Ticket?> GetAsync(int id) =>
        await context.Tickets.FindAsync(id);

    /// <summary>
    /// Retrieves all tickets from the data store.
    /// </summary>
    /// <returns>A list of all ticket entities.</returns>
    public async Task<IList<Ticket>> GetAllAsync() =>
        await context.Tickets.ToListAsync();

    /// <summary>
    /// Updates an existing ticket in the data store.
    /// </summary>
    /// <param name="entity">The ticket entity with updated data.</param>
    /// <returns>The updated ticket entity.</returns>
    public async Task<Ticket> UpdateAsync(Ticket entity)
    {
        context.Tickets.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}