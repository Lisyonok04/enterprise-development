using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

[Route("api/[controller]")]
public class PassengersController(
    IPassengerService passengerService,
    ILogger<PassengersController> logger
) : CrudControllerBase<PassengerDto, CreatePassengerDto, int>(passengerService, logger)
{
    protected override int GetEntityId(PassengerDto dto) => dto.Id;

    [HttpGet("{passengerId}/tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<TicketDto>>> GetTickets(int passengerId)
    {
        logger.LogInformation("Метод GetTickets вызван с passengerId={PassengerId}", passengerId);
        try
        {
            var tickets = await passengerService.GetTicketsAsync(passengerId);
            logger.LogInformation("Метод GetTickets успешно выполнен для passengerId={PassengerId}", passengerId);
            return Ok(tickets);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Билеты не найдены для passengerId={PassengerId}", passengerId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetTickets для passengerId={PassengerId}", passengerId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{passengerId}/flights")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<FlightDto>>> GetFlights(int passengerId)
    {
        logger.LogInformation("Метод GetFlights вызван с passengerId={PassengerId}", passengerId);
        try
        {
            var flights = await passengerService.GetFlightsAsync(passengerId);
            logger.LogInformation("Метод GetFlights успешно выполнен для passengerId={PassengerId}", passengerId);
            return Ok(flights);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Рейсы не найдены для passengerId={PassengerId}", passengerId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetFlights для passengerId={PassengerId}", passengerId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}