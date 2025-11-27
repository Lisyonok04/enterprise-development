using Airline.Domain.Repository;
using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class FlightRepository(AirlineDbContext context) : IRepository<Flight, string>
{
    public async Task<Flight> CreateAsync(Flight entity)
    {
        var entry = await context.Flights.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await context.Flights.FindAsync(id);
        if (entity == null) return false;

        context.Flights.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Flight?> GetAsync(string id) =>
        await context.Flights.FindAsync(id);

    public async Task<IList<Flight>> GetAllAsync() =>
        await context.Flights.ToListAsync();

    public async Task<Flight> UpdateAsync(Flight entity)
    {
        context.Flights.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}