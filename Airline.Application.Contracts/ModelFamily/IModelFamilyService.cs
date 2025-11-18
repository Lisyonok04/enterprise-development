namespace Airline.Application.Contracts.ModelFamily;

public interface IModelFamilyService
{
    public Task<List<ModelFamilyDto>> GetAllAsync();
    public Task<ModelFamilyDto?> GetByIdAsync(string id);
    public Task<ModelFamilyDto> CreateAsync(ModelFamilyDto family);
    public Task<ModelFamilyDto> UpdateAsync(ModelFamilyDto family);
    public Task<bool> DeleteAsync(string id);
}