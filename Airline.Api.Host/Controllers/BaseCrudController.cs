using Airline.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Base controller that provides standardized CRUD (Create, Read, Update, Delete) operations 
/// for entities in the airline management system.
/// Implements common HTTP methods with unified error handling, logging, and response formatting.
/// </summary>
/// <typeparam name="TDto">
/// The Data Transfer Object (DTO) type used for read operations.
/// Represents the shape of data exposed to API clients.
/// </typeparam>
/// <typeparam name="TCreateUpdateDto">
/// The Data Transfer Object (DTO) type used for create and update operations.
/// Contains only the fields required for mutation.
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the unique identifier for the entity (e.g., <see cref="int"/>).
/// Must be a value type (<see langword="struct"/>).
/// </typeparam>
/// <param name="service">The application service that implements business logic.</param>
/// <param name="logger">The logger instance for diagnostics and monitoring.</param>
[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(
    IApplicationService<TDto, TCreateUpdateDto, TKey> service,
    ILogger<CrudControllerBase<TDto, TCreateUpdateDto, TKey>> logger
) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing creation data.</param>
    /// <returns>
    /// An <see cref="ActionResult{TDto}"/> representing the result of the operation:
    /// <list type="bullet">
    /// <item><description><c>201 Created</c> with the created entity if successful.</description></item>
    /// <item><description><c>500 Internal Server Error</c> if an exception occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> Create(TCreateUpdateDto dto)
    {
        logger.LogInformation("Create method called in {Controller} with data: {@Dto}", GetType().Name, dto);
        try
        {
            var result = await service.CreateAsync(dto);
            logger.LogInformation("Create method completed successfully in {Controller}", GetType().Name);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(result) }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Create method of controller {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="dto">The data transfer object containing updated data.</param>
    /// <returns>
    /// An <see cref="ActionResult{TDto}"/> representing the result of the operation:
    /// <list type="bullet">
    /// <item><description><c>200 OK</c> with the updated entity if successful.</description></item>
    /// <item><description><c>404 Not Found</c> if the entity does not exist.</description></item>
    /// <item><description><c>500 Internal Server Error</c> if an exception occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> Update(TKey id, TCreateUpdateDto dto)
    {
        logger.LogInformation("Update method called in {Controller} with id={Id} and data: {@Dto}", GetType().Name, id, dto);
        try
        {
            var result = await service.UpdateAsync(dto, id);
            logger.LogInformation("Update method completed successfully in {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Entity with id={Id} not found in {Controller}", id, GetType().Name);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Update method of controller {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the result of the operation:
    /// <list type="bullet">
    /// <item><description><c>204 No Content</c> if the entity was successfully deleted.</description></item>
    /// <item><description><c>404 Not Found</c> if the entity does not exist.</description></item>
    /// <item><description><c>500 Internal Server Error</c> if an exception occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(TKey id)
    {
        logger.LogInformation("Delete method called in {Controller} with id={Id}", GetType().Name, id);
        try
        {
            var success = await service.DeleteAsync(id);
            if (!success)
            {
                logger.LogWarning("Entity with id={Id} not found during deletion in {Controller}", id, GetType().Name);
                return NotFound();
            }
            logger.LogInformation("Delete method completed successfully in {Controller}", GetType().Name);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Delete method of controller {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <returns>
    /// An <see cref="ActionResult{IList{TDto}}"/> representing the result of the operation:
    /// <list type="bullet">
    /// <item><description><c>200 OK</c> with the list of entities if successful.</description></item>
    /// <item><description><c>500 Internal Server Error</c> if an exception occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<TDto>>> GetAll()
    {
        logger.LogInformation("GetAll method called in {Controller}", GetType().Name);
        try
        {
            var result = await service.GetAllAsync();
            logger.LogInformation("GetAll method completed successfully in {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetAll method of controller {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// An <see cref="ActionResult{TDto}"/> representing the result of the operation:
    /// <list type="bullet">
    /// <item><description><c>200 OK</c> with the entity if found.</description></item>
    /// <item><description><c>404 Not Found</c> if the entity does not exist.</description></item>
    /// <item><description><c>500 Internal Server Error</c> if an exception occurs.</description></item>
    /// </list>
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> GetById(TKey id)
    {
        logger.LogInformation("GetById method called in {Controller} with id={Id}", GetType().Name, id);
        try
        {
            var result = await service.GetByIdAsync(id);
            if (result == null)
            {
                logger.LogWarning("Entity with id={Id} not found in {Controller}", id, GetType().Name);
                return NotFound();
            }
            logger.LogInformation("GetById method completed successfully in {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetById method of controller {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Extracts the unique identifier from a DTO.
    /// Must be implemented by derived controllers to support routing.
    /// </summary>
    /// <param name="dto">The DTO from which to extract the identifier.</param>
    /// <returns>The unique identifier of the entity.</returns>
    protected abstract TKey GetEntityId(TDto dto);
}