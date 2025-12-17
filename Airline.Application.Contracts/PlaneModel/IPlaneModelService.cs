using Airline.Application.Contracts.ModelFamily;

namespace Airline.Application.Contracts.PlaneModel;

/// <summary>
/// Service contract for managing aircraft models in the airline system.
/// Extends the generic CRUD interface with model-specific analytical operations.
/// </summary>
public interface IPlaneModelService : IApplicationService<PlaneModelDto, CreatePlaneModelDto, int>
{
    /// <summary>
    /// Retrieves the model family associated with a specific aircraft model.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <returns>The model family DTO linked to the aircraft model.</returns>
    public Task<ModelFamilyDto> GetModelFamilyAsync(int modelId);
}