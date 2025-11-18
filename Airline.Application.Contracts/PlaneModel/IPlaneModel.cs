namespace Airline.Application.Contracts.PlaneModel;

public interface IPlaneModelService
{
    public Task<List<PlaneModelDto>> GetAllAsync();
    public Task<PlaneModelDto?> GetByIdAsync(string id);
    public Task<PlaneModelDto> CreateAsync(PlaneModelDto model);
    public Task<PlaneModelDto> UpdateAsync(PlaneModelDto model);
    public Task<bool> DeleteAsync(string id);
}