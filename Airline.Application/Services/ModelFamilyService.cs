using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Provides business logic for managing aircraft model families in the airline system.
/// Handles CRUD operations and retrieval of associated aircraft models.
/// </summary>
public class ModelFamilyService(
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<ModelFamily, int> familyRepository,
    IMapper mapper
) : IModelFamilyService
{
    /// <summary>
    /// Creates a new aircraft model family.
    /// </summary>
    /// <param name="dto">The model family creation data transfer object.</param>
    /// <returns>The created model family DTO.</returns>
    public async Task<ModelFamilyDto> CreateAsync(CreateModelFamilyDto dto)
    {
        var modelFamily = mapper.Map<ModelFamily>(dto);
        var maxId = 0;
        var last = await familyRepository.GetAllAsync();
        if (last.Any())
        {
            maxId = last.Max(f => f.Id);
        }
        modelFamily.Id = maxId + 1;
        var created = await familyRepository.CreateAsync(modelFamily);
        return mapper.Map<ModelFamilyDto>(created);
    }

    /// <summary>
    /// Retrieves a model family by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model family.</param>
    /// <returns>The model family DTO if found; otherwise, null.</returns>
    public async Task<ModelFamilyDto?> GetByIdAsync(int id)
    {
        var entity = await familyRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<ModelFamilyDto>(entity);
    }

    /// <summary>
    /// Retrieves all aircraft model families in the system.
    /// </summary>
    /// <returns>A list of all model family DTOs.</returns>
    public async Task<IList<ModelFamilyDto>> GetAllAsync() =>
        (await familyRepository.GetAllAsync()).Select(mapper.Map<ModelFamilyDto>).ToList();

    /// <summary>
    /// Updates an existing model family with new data.
    /// </summary>
    /// <param name="dto">The updated model family data transfer object.</param>
    /// <param name="id">The unique identifier of the model family to update.</param>
    /// <returns>The updated model family DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the model family does not exist.
    /// </exception>
    public async Task<ModelFamilyDto> UpdateAsync(CreateModelFamilyDto dto, int id)
    {
        var existing = await familyRepository.GetAsync(id)
            ?? throw new KeyNotFoundException($"Model family '{id}' not found.");
        mapper.Map(dto, existing);
        var updated = await familyRepository.UpdateAsync(existing);
        return mapper.Map<ModelFamilyDto>(updated);
    }

    /// <summary>
    /// Deletes a model family by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model family to delete.</param>
    /// <returns>True if the model family was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id) => await familyRepository.DeleteAsync(id);

    /// <summary>
    /// Retrieves all aircraft models associated with a specific model family.
    /// </summary>
    /// <param name="familyId">The unique identifier of the model family.</param>
    /// <returns>A list of aircraft model DTOs linked to the family.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the model family does not exist.
    /// </exception>
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