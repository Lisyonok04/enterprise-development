using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

[Route("api/[controller]")]
public class PlaneModelsController(
    IPlaneModelService planeModelService,
    ILogger<PlaneModelsController> logger
) : CrudControllerBase<PlaneModelDto, CreatePlaneModelDto, int>(planeModelService, logger)
{
    protected override int GetEntityId(PlaneModelDto dto) => dto.Id;

    [HttpGet("{modelId}/family")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ModelFamilyDto>> GetModelFamily(int modelId)
    {
        logger.LogInformation("Метод GetModelFamily вызван с modelId={ModelId}", modelId);
        try
        {
            var family = await planeModelService.GetModelFamilyAsync(modelId);
            logger.LogInformation("Метод GetModelFamily успешно выполнен для modelId={ModelId}", modelId);
            return Ok(family);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Семейство моделей не найдено для modelId={ModelId}", modelId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetModelFamily для modelId={ModelId}", modelId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}