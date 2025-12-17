using Airline.Application.Contracts.PlaneModel;

namespace Airline.Application.Contracts.ModelFamily;

/// <summary>
/// Service contract for managing aircraft model families in the airline system.
/// Extends the generic CRUD interface with family-specific analytical operations.
/// </summary>
public interface IModelFamilyService : IApplicationService<ModelFamilyDto, CreateModelFamilyDto, int>
{
    /// <summary>
    /// Retrieves all aircraft models associated with a specific model family.
    /// </summary>
    /// <param name="familyId">The unique identifier of the model family.</param>
    /// <returns>A list of aircraft model DTOs linked to the family.</returns>
    public Task<IList<PlaneModelDto>> GetPlaneModelsAsync(int familyId);
}