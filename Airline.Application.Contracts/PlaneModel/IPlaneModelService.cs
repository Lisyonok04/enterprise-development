using Airline.Application.Contracts.ModelFamily;

namespace Airline.Application.Contracts.PlaneModel;

public interface IPlaneModelService : IApplicationService<PlaneModelDto, CreatePlaneModelDto, int>
{
    public Task<ModelFamilyDto> GetModelFamilyAsync(int modelId);
}
