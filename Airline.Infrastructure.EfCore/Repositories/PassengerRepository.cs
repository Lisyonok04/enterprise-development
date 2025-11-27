using Airline.Domain.Repository;
using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class PassengerRepository(AirlineDbContext context) : IRepository<Passenger, string>
{
    public async Task<Passenger> CreateAsync(Passenger entity)
    {
        var entry = await context.Passengers.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await context.Passengers.FindAsync(id);
        if (entity == null) return false;

        context.Passengers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Passenger?> GetAsync(string id) =>
        await context.Passengers.FindAsync(id);

    public async Task<IList<Passenger>> GetAllAsync() =>
        await context.Passengers.ToListAsync();

    public async Task<Passenger> UpdateAsync(Passenger entity)
    {
        context.Passengers.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}