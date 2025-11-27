using Airline.Domain.Repository;
using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class TicketRepository(AirlineDbContext context) : IRepository<Ticket, string>
{
    public async Task<Ticket> CreateAsync(Ticket entity)
    {
        var entry = await context.Tickets.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await context.Tickets.FindAsync(id);
        if (entity == null) return false;

        context.Tickets.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Ticket?> GetAsync(string id) =>
        await context.Tickets.FindAsync(id);

    public async Task<IList<Ticket>> GetAllAsync() =>
        await context.Tickets.ToListAsync();

    public async Task<Ticket> UpdateAsync(Ticket entity)
    {
        context.Tickets.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}