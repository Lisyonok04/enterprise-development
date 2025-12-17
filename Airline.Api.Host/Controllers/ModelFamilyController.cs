using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for managing aircraft model families and retrieving associated data.
/// Inherits from <see cref="CrudControllerBase{TDto, TCreateUpdateDto, TKey}"/> 
/// to provide standardized CRUD operations.
/// </summary>
[Route("api/[controller]")]
public class ModelFamiliesController(
    IModelFamilyService modelFamilyService,
    ILogger<ModelFamiliesController> logger
) : CrudControllerBase<ModelFamilyDto, CreateModelFamilyDto, int>(modelFamilyService, logger)
{
    /// <inheritdoc />
    protected override int GetEntityId(ModelFamilyDto dto) => dto.Id;

    /// <summary>
    /// Retrieves all aircraft models associated with a specific model family.
    /// </summary>
    /// <param name="familyId">The unique identifier of the model family.</param>
    /// <returns>A list of aircraft model DTOs linked to the family.</returns>
    /// <response code="200">Returns the list of associated aircraft models.</response>
    /// <response code="404">If the model family is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
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