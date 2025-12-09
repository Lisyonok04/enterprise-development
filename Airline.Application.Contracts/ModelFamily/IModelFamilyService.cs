using Airline.Application.Contracts.PlaneModel;

namespace Airline.Application.Contracts.ModelFamily;

public interface IModelFamilyService : IApplicationService<ModelFamilyDto, CreateModelFamilyDto, int>
{
    public Task<IList<PlaneModelDto>> GetPlaneModelsAsync(int familyId);
}
