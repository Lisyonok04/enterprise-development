using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Сервис для управления семействами самолётов и получения связанных моделей
/// </summary>
/// <param name="familyRepository">Репозиторий для операций с сущностями семейства самолётов</param>
/// <param name="modelRepository">Репозиторий для операций с сущностями моделей самолётов</param>
/// <param name="mapper">Маппер для преобразования доменных моделей в DTO и обратно</param>
public class AircraftFamilyService(
    IRepository<ModelFamily, int> familyRepository,
    IRepository<PlaneModel, int> modelRepository,
    IMapper mapper
) : IModelFamilyService