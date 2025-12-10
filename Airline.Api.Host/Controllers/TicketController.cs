using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

[Route("api/[controller]")]
public class TicketsController(
    ITicketService ticketService,
    ILogger<TicketsController> logger
) : CrudControllerBase<TicketDto, CreateTicketDto, int>(ticketService, logger)
{
    
    protected override int GetEntityId(TicketDto dto) => dto.Id;

    [HttpGet("{ticketId}/flight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FlightDto>> GetFlight(int ticketId)
    {
        logger.LogInformation("Метод GetFlight вызван с ticketId={TicketId}", ticketId);
        try
        {
            var flight = await ticketService.GetFlightAsync(ticketId);
            logger.LogInformation("Метод GetFlight успешно выполнен для ticketId={TicketId}", ticketId);
            return Ok(flight);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Рейс не найден для ticketId={TicketId}", ticketId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetFlight для ticketId={TicketId}", ticketId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{ticketId}/passenger")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PassengerDto>> GetPassenger(int ticketId)
    {
        logger.LogInformation("Метод GetPassenger вызван с ticketId={TicketId}", ticketId);
        try
        {
            var passenger = await ticketService.GetPassengerAsync(ticketId);
            logger.LogInformation("Метод GetPassenger успешно выполнен для ticketId={TicketId}", ticketId);
            return Ok(passenger);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Пассажир не найден для ticketId={TicketId}", ticketId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetPassenger для ticketId={TicketId}", ticketId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}