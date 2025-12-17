using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for managing passengers and retrieving associated data.
/// Inherits from <see cref="CrudControllerBase{TDto, TCreateUpdateDto, TKey}"/> 
/// to provide standardized CRUD operations.
/// </summary>
[Route("api/[controller]")]
public class PassengersController(
    IPassengerService passengerService,
    ILogger<PassengersController> logger
) : CrudControllerBase<PassengerDto, CreatePassengerDto, int>(passengerService, logger)
{
    /// <inheritdoc />
    protected override int GetEntityId(PassengerDto dto) => dto.Id;

    /// <summary>
    /// Retrieves all tickets associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of ticket DTOs linked to the passenger.</returns>
    /// <response code="200">Returns the list of associated tickets.</response>
    /// <response code="404">If the passenger is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{passengerId}/tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<TicketDto>>> GetTickets(int passengerId)
    {
        logger.LogInformation("GetTickets method called with passengerId={PassengerId}", passengerId);
        try
        {
            var tickets = await passengerService.GetTicketsAsync(passengerId);
            logger.LogInformation("GetTickets method completed successfully for passengerId={PassengerId}", passengerId);
            return Ok(tickets);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Tickets not found for passengerId={PassengerId}", passengerId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetTickets method for passengerId={PassengerId}", passengerId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all flights associated with a specific passenger.
    /// </summary>
    /// <param name="passengerId">The unique identifier of the passenger.</param>
    /// <returns>A list of flight DTOs linked to the passenger.</returns>
    /// <response code="200">Returns the list of associated flights.</response>
    /// <response code="404">If the passenger is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{passengerId}/flights")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<FlightDto>>> GetFlights(int passengerId)
    {
        logger.LogInformation("GetFlights method called with passengerId={PassengerId}", passengerId);
        try
        {
            var flights = await passengerService.GetFlightsAsync(passengerId);
            logger.LogInformation("GetFlights method completed successfully for passengerId={PassengerId}", passengerId);
            return Ok(flights);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Flights not found for passengerId={PassengerId}", passengerId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFlights method for passengerId={PassengerId}", passengerId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}