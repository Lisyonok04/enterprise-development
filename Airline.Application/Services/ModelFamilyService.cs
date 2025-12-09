using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class ModelFamilyService(
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<ModelFamily, int> familyRepository,
    IMapper mapper
) : IModelFamilyService
{
    public async Task<ModelFamilyDto> CreateAsync(CreateModelFamilyDto dto)
    {
        var family = mapper.Map<ModelFamily>(dto);
        var created = await familyRepository.CreateAsync(family);
        return mapper.Map<ModelFamilyDto>(created);
    }

    public async Task<ModelFamilyDto?> GetByIdAsync(int id)
    {
        var entity = await familyRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<ModelFamilyDto>(entity);
    }

    public async Task<IList<ModelFamilyDto>> GetAllAsync() =>
        (await familyRepository.GetAllAsync()).Select(mapper.Map<ModelFamilyDto>).ToList();

    public async Task<ModelFamilyDto> UpdateAsync(CreateModelFamilyDto dto, int id)
    {
        var existing = await familyRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Model family '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await familyRepository.UpdateAsync(existing);
        return mapper.Map<ModelFamilyDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id) => await familyRepository.DeleteAsync(id);

    public async Task<IList<PlaneModelDto>> GetPlaneModelsAsync(int familyId)
    {
        var family = await familyRepository.GetAsync(familyId);
        if (family == null)
            throw new KeyNotFoundException($"Model family with ID '{familyId}' not found.");
        var allModels = await planeModelRepository.GetAllAsync();

        var models = allModels
            .Where(m => m.ModelFamilyId == familyId)
            .Select(mapper.Map<PlaneModelDto>)
            .ToList();

        return models;
    }
}