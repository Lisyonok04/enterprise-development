using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for managing flights and retrieving associated data.
/// Inherits from <see cref="CrudControllerBase{TDto, TCreateUpdateDto, TKey}"/> 
/// to provide standardized CRUD operations.
/// </summary>
[Route("api/[controller]")]
public class FlightsController(
    IFlightService flightService,
    ILogger<FlightsController> logger
) : CrudControllerBase<FlightDto, CreateFlightDto, int>(flightService, logger)
{
    /// <inheritdoc />
    protected override int GetEntityId(FlightDto dto) => dto.Id;

    /// <summary>
    /// Retrieves the aircraft model associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>The aircraft model DTO linked to the flight.</returns>
    /// <response code="200">Returns the associated aircraft model.</response>
    /// <response code="404">If the flight or aircraft model is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{flightId}/model")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlaneModelDto>> GetPlaneModel(int flightId)
    {
        logger.LogInformation("Метод GetPlaneModel вызван с flightId={FlightId}", flightId);
        try
        {
            var model = await flightService.GetPlaneModelAsync(flightId);
            logger.LogInformation("Метод GetPlaneModel успешно выполнен для flightId={FlightId}", flightId);
            return Ok(model);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Модель самолёта не найдена для flightId={FlightId}", flightId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetPlaneModel для flightId={FlightId}", flightId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all tickets associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of ticket DTOs linked to the flight.</returns>
    /// <response code="200">Returns the list of associated tickets.</response>
    /// <response code="404">If the flight is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{flightId}/tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<TicketDto>>> GetTickets(int flightId)
    {
        logger.LogInformation("Метод GetTickets вызван с flightId={FlightId}", flightId);
        try
        {
            var tickets = await flightService.GetTicketsAsync(flightId);
            logger.LogInformation("Метод GetTickets успешно выполнен для flightId={FlightId}", flightId);
            return Ok(tickets);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Билеты не найдены для flightId={FlightId}", flightId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetTickets для flightId={FlightId}", flightId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all passengers associated with a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of passenger DTOs linked to the flight.</returns>
    /// <response code="200">Returns the list of associated passengers.</response>
    /// <response code="404">If the flight is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{flightId}/passengers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<PassengerDto>>> GetPassengers(int flightId)
    {
        logger.LogInformation("Метод GetPassengers вызван с flightId={FlightId}", flightId);
        try
        {
            var passengers = await flightService.GetPassengersAsync(flightId);
            logger.LogInformation("Метод GetPassengers успешно выполнен для flightId={FlightId}", flightId);
            return Ok(passengers);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Пассажиры не найдены для flightId={FlightId}", flightId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetPassengers для flightId={FlightId}", flightId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}