using Airline.Domain.Items;
using Airline.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Airline.Infrastructure.EfCore.Repositories;

public class ModelFamilyRepository(AirlineDbContext context) : IRepository<ModelFamily, string>
{
    public async Task<ModelFamily> CreateAsync(ModelFamily entity)
    {
        var entry = await context.ModelFamilies.AddAsync(entity);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await context.ModelFamilies.FindAsync(id);
        if (entity == null) return false;

        context.ModelFamilies.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ModelFamily?> GetAsync(string id) =>
        await context.ModelFamilies.FindAsync(id);

    public async Task<IList<ModelFamily>> GetAllAsync() =>
        await context.ModelFamilies.ToListAsync();

    public async Task<ModelFamily> UpdateAsync(ModelFamily entity)
    {
        context.ModelFamilies.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}