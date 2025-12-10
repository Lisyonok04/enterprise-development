using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Host.Controllers;

[Route("api/[controller]")]
public class FlightsController(
    IFlightService flightService,
    ILogger<FlightsController> logger
) : CrudControllerBase<FlightDto, CreateFlightDto, int>(flightService, logger)
{

    protected override int GetEntityId(FlightDto dto) => dto.Id;

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