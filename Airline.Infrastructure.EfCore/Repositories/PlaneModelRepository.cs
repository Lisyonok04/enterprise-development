using Airline.Domain.Items;
using Airline.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class PlaneModelRepository(AirlineDbContext context) : IRepository<PlaneModel, string>
{
    public async Task<PlaneModel> CreateAsync(PlaneModel entity)
    {
        var entry = await context.PlaneModels.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await context.PlaneModels.FindAsync(id);
        if (entity == null) return false;

        context.PlaneModels.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<PlaneModel?> GetAsync(string id) =>
        await context.PlaneModels.FindAsync(id);

    public async Task<IList<PlaneModel>> GetAllAsync() =>
        await context.PlaneModels.ToListAsync();

    public async Task<PlaneModel> UpdateAsync(PlaneModel entity)
    {
        context.PlaneModels.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}