// Airline.Api.Host/Controllers/CrudControllerBase.cs
using Airline.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Базовый контроллер для стандартизированных CRUD-операций над сущностями.
/// Обеспечивает единообразную обработку запросов, логирование и возврат HTTP-статусов.
/// </summary>
/// <typeparam name="TDto">DTO для операций чтения</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для создания и обновления</typeparam>
/// <typeparam name="TKey">Тип идентификатора (например, int)</typeparam>
/// <param name="service">Сервис, реализующий IApplicationService</param>
/// <param name="logger">Экземпляр логгера</param>
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
    /// Создаёт новую сущность.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> Create(TCreateUpdateDto dto)
    {
        logger.LogInformation("Метод Create вызван в {Controller} с данными: {@Dto}", GetType().Name, dto);
        try
        {
            var result = await service.CreateAsync(dto);
            logger.LogInformation("Метод Create успешно выполнен в {Controller}", GetType().Name);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(result) }, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе Create контроллера {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Обновляет существующую сущность по идентификатору.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> Update(TKey id, TCreateUpdateDto dto)
    {
        logger.LogInformation("Метод Update вызван в {Controller} с id={Id} и данными: {@Dto}", GetType().Name, id, dto);
        try
        {
            var result = await service.UpdateAsync(dto, id);
            logger.LogInformation("Метод Update успешно выполнен в {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Сущность с id={Id} не найдена в {Controller}", id, GetType().Name);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе Update контроллера {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(TKey id)
    {
        logger.LogInformation("Метод Delete вызван в {Controller} с id={Id}", GetType().Name, id);
        try
        {
            var success = await service.DeleteAsync(id);
            if (!success)
            {
                logger.LogWarning("Сущность с id={Id} не найдена при удалении в {Controller}", id, GetType().Name);
                return NotFound();
            }
            logger.LogInformation("Метод Delete успешно выполнен в {Controller}", GetType().Name);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе Delete контроллера {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает все сущности.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<TDto>>> GetAll()
    {
        logger.LogInformation("Метод GetAll вызван в {Controller}", GetType().Name);
        try
        {
            var result = await service.GetAllAsync();
            logger.LogInformation("Метод GetAll успешно выполнен в {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetAll контроллера {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Возвращает сущность по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TDto>> GetById(TKey id)
    {
        logger.LogInformation("Метод GetById вызван в {Controller} с id={Id}", GetType().Name, id);
        try
        {
            var result = await service.GetByIdAsync(id);
            if (result == null)
            {
                logger.LogWarning("Сущность с id={Id} не найдена в {Controller}", id, GetType().Name);
                return NotFound();
            }
            logger.LogInformation("Метод GetById успешно выполнен в {Controller}", GetType().Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetById контроллера {Controller}", GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Извлекает идентификатор из DTO.
    /// Должен быть переопределён в производных классах.
    /// </summary>
    protected abstract TKey GetEntityId(TDto dto);
}