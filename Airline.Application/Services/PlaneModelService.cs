using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Provides business logic for managing aircraft models in the airline system.
/// Handles CRUD operations, validation of technical specifications, 
/// and retrieval of associated model families.
/// </summary>
public class PlaneModelService(
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<ModelFamily, int> familyRepository,
    IMapper mapper
) : IPlaneModelService
{
    /// <summary>
    /// Creates a new aircraft model.
    /// Validates that the model family exists and that capacity values are non-negative.
    /// </summary>
    /// <param name="dto">The aircraft model creation data transfer object.</param>
    /// <returns>The created aircraft model DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the specified model family does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if passenger capacity or cargo capacity is negative.
    /// </exception>
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

    /// <summary>
    /// Retrieves an aircraft model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the aircraft model.</param>
    /// <returns>The aircraft model DTO if found; otherwise, null.</returns>
    public async Task<PlaneModelDto?> GetByIdAsync(int id)
    {
        var entity = await planeModelRepository.GetAsync(id);
        return entity == null ? null : mapper.Map<PlaneModelDto>(entity);
    }

    /// <summary>
    /// Retrieves all aircraft models in the system.
    /// </summary>
    /// <returns>A list of all aircraft model DTOs.</returns>
    public async Task<IList<PlaneModelDto>> GetAllAsync() =>
        (await planeModelRepository.GetAllAsync()).Select(mapper.Map<PlaneModelDto>).ToList();

    /// <summary>
    /// Updates an existing aircraft model with new data.
    /// Validates that capacity values are non-negative.
    /// </summary>
    /// <param name="dto">The updated aircraft model data transfer object.</param>
    /// <param name="id">The unique identifier of the model to update.</param>
    /// <returns>The updated aircraft model DTO.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the aircraft model does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if passenger capacity or cargo capacity is negative.
    /// </exception>
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

    /// <summary>
    /// Deletes an aircraft model by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the model to delete.</param>
    /// <returns>True if the model was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id) => await planeModelRepository.DeleteAsync(id);

    /// <summary>
    /// Retrieves the model family associated with a specific aircraft model.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <returns>The model family DTO associated with the model.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the aircraft model does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the associated model family is missing (data integrity issue).
    /// </exception>
    public async Task<ModelFamilyDto> GetModelFamilyAsync(int modelId)
    {
        var model = await planeModelRepository.GetAsync(modelId)
            ?? throw new KeyNotFoundException($"Plane model '{modelId}' not found.");
        var family = await familyRepository.GetAsync(model.ModelFamilyId)
            ?? throw new InvalidOperationException($"Family '{model.ModelFamilyId}' missing.");
        return mapper.Map<ModelFamilyDto>(family);
    }
}