using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for managing aircraft models and retrieving associated data.
/// Inherits from <see cref="CrudControllerBase{TDto, TCreateUpdateDto, TKey}"/> 
/// to provide standardized CRUD operations.
/// </summary>
[Route("api/[controller]")]
public class PlaneModelsController(
    IPlaneModelService planeModelService,
    ILogger<PlaneModelsController> logger
) : CrudControllerBase<PlaneModelDto, CreatePlaneModelDto, int>(planeModelService, logger)
{
    /// <inheritdoc />
    protected override int GetEntityId(PlaneModelDto dto) => dto.Id;

    /// <summary>
    /// Retrieves the model family associated with a specific aircraft model.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <returns>The model family DTO linked to the aircraft model.</returns>
    /// <response code="200">Returns the associated model family.</response>
    /// <response code="404">If the aircraft model or model family is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
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