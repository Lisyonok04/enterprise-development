using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for executing analytical queries over airline operational data.
/// Provides aggregated insights such as top flights, minimal travel time, passenger baggage analysis,
/// and route-based reporting.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger
) : ControllerBase
{
    /// <summary>
    /// Retrieves the top N flights by number of passengers.
    /// </summary>
    /// <param name="top">The number of top flights to return (default: 5).</param>
    /// <returns>
    /// A list of flight DTOs sorted by passenger count in descending order.
    /// </returns>
    /// <response code="200">Returns the list of top flights.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpGet("top-flights-by-passenger-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetTopFlightsByPassengerCount([FromQuery] int top = 5)
    {
        logger.LogInformation("GetTopFlightsByPassengerCount method called with top={Top}", top);
        try
        {
            var flights = await analyticsService.GetTopFlightsByPassengerCountAsync(top);
            logger.LogInformation("GetTopFlightsByPassengerCount method completed successfully");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetTopFlightsByPassengerCount method");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves flights with the minimal travel time (duration).
    /// </summary>
    /// <returns>
    /// A list of flight DTOs with the shortest duration.
    /// </returns>
    /// <response code="200">Returns the list of flights with minimal travel time.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpGet("flights-with-min-travel-time")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsWithMinTravelTime()
    {
        logger.LogInformation("GetFlightsWithMinTravelTime method called");
        try
        {
            var flights = await analyticsService.GetFlightsWithMinTravelTimeAsync();
            logger.LogInformation("GetFlightsWithMinTravelTime method completed successfully");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFlightsWithMinTravelTime method");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves passengers with zero checked baggage for a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>
    /// A list of passenger DTOs who have no checked baggage on the specified flight.
    /// </returns>
    /// <response code="200">Returns the list of passengers with zero baggage.</response>
    /// <response code="404">If the specified flight does not exist.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpGet("passengers-with-zero-baggage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<PassengerDto>>> GetPassengersWithZeroBaggage([FromQuery] int flightId)
    {
        logger.LogInformation("GetPassengersWithZeroBaggage method called with flightId={FlightId}", flightId);
        try
        {
            var passengers = await analyticsService.GetPassengersWithZeroBaggageOnFlightAsync(flightId);
            logger.LogInformation("GetPassengersWithZeroBaggage method completed successfully for flightId={FlightId}", flightId);
            return Ok(passengers);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Flight not found for flightId={FlightId}", flightId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetPassengersWithZeroBaggage method for flightId={FlightId}", flightId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves flights of a specific aircraft model within a given date period.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <param name="from">Start date of the period (inclusive).</param>
    /// <param name="to">End date of the period (inclusive).</param>
    /// <returns>
    /// A list of flight DTOs matching the model and date range.
    /// </returns>
    /// <response code="200">Returns the list of flights matching the criteria.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpGet("flights-by-model-in-period")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByModelInPeriod(
        [FromQuery] int modelId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        logger.LogInformation("GetFlightsByModelInPeriod method called with modelId={ModelId}, from={From}, to={To}", modelId, from, to);
        try
        {
            var flights = await analyticsService.GetFlightsByModelInPeriodAsync(modelId, from, to);
            logger.LogInformation("GetFlightsByModelInPeriod method completed successfully");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFlightsByModelInPeriod method");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves flights by route (departure city → arrival city).
    /// </summary>
    /// <param name="departure">The city of departure.</param>
    /// <param name="arrival">The city of arrival.</param>
    /// <returns>
    /// A list of flight DTOs matching the specified route.
    /// </returns>
    /// <response code="200">Returns the list of flights matching the route.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpGet("flights-by-route")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<FlightDto>>> GetFlightsByRoute(
        [FromQuery] string departure,
        [FromQuery] string arrival)
    {
        logger.LogInformation("GetFlightsByRoute method called with departure={Departure}, arrival={Arrival}", departure, arrival);
        try
        {
            var flights = await analyticsService.GetFlightsByRouteAsync(departure, arrival);
            logger.LogInformation("GetFlightsByRoute method completed successfully");
            return Ok(flights);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFlightsByRoute method");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}