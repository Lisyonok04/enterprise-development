using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger
) : ControllerBase
{

    [HttpGet("top-flights-by-passenger-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetTopFlightsByPassengerCount([FromQuery] int top = 5)
    {
        logger.LogInformation("Метод GetTopFlightsByPassengerCount вызван с top={Top}", top);
        try
        {
            var flights = await analyticsService.GetTopFlightsByPassengerCountAsync(top);
            logger.LogInformation("Метод GetTopFlightsByPassengerCount успешно выполнен");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetTopFlightsByPassengerCount");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("flights-with-min-travel-time")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsWithMinTravelTime()
    {
        logger.LogInformation("Метод GetFlightsWithMinTravelTime вызван");
        try
        {
            var flights = await analyticsService.GetFlightsWithMinTravelTimeAsync();
            logger.LogInformation("Метод GetFlightsWithMinTravelTime успешно выполнен");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetFlightsWithMinTravelTime");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet("passengers-with-zero-baggage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengersWithZeroBaggage([FromQuery] int flightId)
    {
        logger.LogInformation("Метод GetPassengersWithZeroBaggage вызван с flightId={FlightId}", flightId);
        try
        {
            var passengers = await analyticsService.GetPassengersWithZeroBaggageOnFlightAsync(flightId);
            logger.LogInformation("Метод GetPassengersWithZeroBaggage успешно выполнен для flightId={FlightId}", flightId);
            return Ok(passengers);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Рейс не найден для flightId={FlightId}", flightId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetPassengersWithZeroBaggage для flightId={FlightId}", flightId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("flights-by-model-in-period")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByModelInPeriod(
        [FromQuery] int modelId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        logger.LogInformation("Метод GetFlightsByModelInPeriod вызван с modelId={ModelId}, from={From}, to={To}", modelId, from, to);
        try
        {
            var flights = await analyticsService.GetFlightsByModelInPeriodAsync(modelId, from, to);
            logger.LogInformation("Метод GetFlightsByModelInPeriod успешно выполнен");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetFlightsByModelInPeriod");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("flights-by-route")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByRoute(
        [FromQuery] string departure,
        [FromQuery] string arrival)
    {
        logger.LogInformation("Метод GetFlightsByRoute вызван с departure={Departure}, arrival={Arrival}", departure, arrival);
        try
        {
            var flights = await analyticsService.GetFlightsByRouteAsync(departure, arrival);
            logger.LogInformation("Метод GetFlightsByRoute успешно выполнен");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка в методе GetFlightsByRoute");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}