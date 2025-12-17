using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Airline.Api.Host.Controllers;

/// <summary>
/// Controller for managing tickets and retrieving associated data.
/// Inherits from <see cref="CrudControllerBase{TDto, TCreateUpdateDto, TKey}"/> 
/// to provide standardized CRUD operations.
/// </summary>
[Route("api/[controller]")]
public class TicketsController(
    ITicketService ticketService,
    ILogger<TicketsController> logger
) : CrudControllerBase<TicketDto, CreateTicketDto, int>(ticketService, logger)
{
    /// <inheritdoc />
    protected override int GetEntityId(TicketDto dto) => dto.Id;

    /// <summary>
    /// Retrieves the flight associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The flight DTO linked to the ticket.</returns>
    /// <response code="200">Returns the associated flight.</response>
    /// <response code="404">If the ticket or flight is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{ticketId}/flight")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FlightDto>> GetFlight(int ticketId)
    {
        logger.LogInformation("GetFlight method called with ticketId={TicketId}", ticketId);
        try
        {
            var flight = await ticketService.GetFlightAsync(ticketId);
            logger.LogInformation("GetFlight method completed successfully for ticketId={TicketId}", ticketId);
            return Ok(flight);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Flight not found for ticketId={TicketId}", ticketId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFlight method for ticketId={TicketId}", ticketId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves the passenger associated with a specific ticket.
    /// </summary>
    /// <param name="ticketId">The unique identifier of the ticket.</param>
    /// <returns>The passenger DTO linked to the ticket.</returns>
    /// <response code="200">Returns the associated passenger.</response>
    /// <response code="404">If the ticket or passenger is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet("{ticketId}/passenger")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PassengerDto>> GetPassenger(int ticketId)
    {
        logger.LogInformation("GetPassenger method called with ticketId={TicketId}", ticketId);
        try
        {
            var passenger = await ticketService.GetPassengerAsync(ticketId);
            logger.LogInformation("GetPassenger method completed successfully for ticketId={TicketId}", ticketId);
            return Ok(passenger);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Passenger not found for ticketId={TicketId}", ticketId);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetPassenger method for ticketId={TicketId}", ticketId);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}