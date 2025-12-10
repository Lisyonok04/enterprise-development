using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

[Route("api/[controller]")]
public class ModelFamiliesController(
    IModelFamilyService modelFamilyService,
    ILogger<ModelFamiliesController> logger
) : CrudControllerBase<ModelFamilyDto, CreateModelFamilyDto, int>(modelFamilyService, logger)
{
    protected override int GetEntityId(ModelFamilyDto dto) => dto.Id;

    [HttpGet("{familyId}/models")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<PlaneModelDto>>> GetPlaneModels(int familyId)
    {
        logger.LogInformation("Метод GetPlaneModels вызван с familyId={FamilyId}", familyId);
        try
        {
            var models = await modelFamilyService.GetPlaneModelsAsync(familyId);
            logger.LogInformation("Метод GetPlaneModels успешно выполнен для familyId={FamilyId}", familyId);
            return Ok(models);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Модели самолётов не найдены для familyId={FamilyId}", familyId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetPlaneModels для familyId={FamilyId}", familyId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}