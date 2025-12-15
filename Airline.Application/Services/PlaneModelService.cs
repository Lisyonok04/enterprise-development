using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class PlaneModelService(
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<ModelFamily, int> familyRepository,
    IMapper mapper
) : IPlaneModelService
{

    public async Task<PlaneModelDto> CreateAsync(CreatePlaneModelDto dto)
    {
        if (await familyRepository.GetAsync(dto.ModelFamilyId) == null)
            throw new KeyNotFoundException($"Model family '{dto.ModelFamilyId}' not found.");

        var planeModel = mapper.Map<PlaneModel>(dto);
        var maxId = 0;
        var last = await planeModelRepository.GetAllAsync();
        if (last.Any())
        {
            maxId = last.Max(m => m.Id);
        }
        planeModel.Id = maxId + 1;
        if (dto.PassengerCapacity < 0 || dto.CargoCapacity < 0)
            throw new ArgumentException("Capacity values cannot be negative.");
        var created = await planeModelRepository.CreateAsync(planeModel);
        return mapper.Map<PlaneModelDto>(created);
    }

    public async Task<PlaneModelDto?> GetByIdAsync(int id)
    {
        var entity = await planeModelRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<PlaneModelDto>(entity);
    }

    public async Task<IList<PlaneModelDto>> GetAllAsync() =>
        (await planeModelRepository.GetAllAsync()).Select(mapper.Map<PlaneModelDto>).ToList();

    public async Task<PlaneModelDto> UpdateAsync(CreatePlaneModelDto dto, int id)
    {
        var existing = await planeModelRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Plane model '{id}' not found.");
        mapper.Map(dto, existing);
        if (dto.PassengerCapacity < 0 || dto.CargoCapacity < 0)
            throw new ArgumentException("Capacity values cannot be negative.");
        var updated = await planeModelRepository.UpdateAsync(existing);
        return mapper.Map<PlaneModelDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await planeModelRepository.DeleteAsync(id);

    public async Task<ModelFamilyDto> GetModelFamilyAsync(int modelId)
    {
        var model = await planeModelRepository.GetAsync(modelId)
            ?? throw new KeyNotFoundException($"Plane model '{modelId}' not found.");
        var family = await familyRepository.GetAsync(model.ModelFamilyId)
            ?? throw new InvalidOperationException($"Family '{model.ModelFamilyId}' missing.");
        return mapper.Map<ModelFamilyDto>(family);
    }
}