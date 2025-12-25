using Airline.Application.Contracts.Flight;
using Airline.Generator.Kafka.Host.Generator;
using Airline.Generator.Kafka.Host.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Generator.Kafka.Host.Controllers;

/// <summary>
/// Controller used to generate flight contracts and publish them via Kafka message broker
/// </summary>
/// <param name="logger">Logger instance</param>
/// <param name="producerService">Producer service used to send contracts</param>
[Route("api/[controller]")]
[ApiController]
public sealed class GeneratorController(
    ILogger<GeneratorController> logger,
    IProducerService producerService) : ControllerBase
{
    /// <summary>
    /// Generates flight contracts and sends them via Kafka using batches and delay between sends
    /// </summary>
    /// <param name="batchSize">Batch size</param>
    /// <param name="payloadLimit">Total number of contracts to send</param>
    /// <param name="waitTime">Delay in seconds between batches</param>
    /// <returns>List of generated contracts</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CreateFlightDto>>> Get(
        [FromQuery] int batchSize,
        [FromQuery] int payloadLimit,
        [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} contracts via {batchSize} batches and {waitTime}s delay",
            payloadLimit, batchSize, waitTime);

        try
        {
            var results = new List<CreateFlightDto>();
            var counter = 0;

            while (counter < payloadLimit)
            {
                var currentBatchSize = Math.Min(batchSize, payloadLimit - counter);
                var batch = FlightGenerator.GenerateContracts(currentBatchSize);

                await producerService.SendAsync(batch);

                logger.LogInformation("Batch of {batchSize} items has been sent", currentBatchSize);

                results.AddRange(batch);
                counter += currentBatchSize;

                if (counter < payloadLimit && waitTime > 0)
                    await Task.Delay(waitTime * 1000);
            }

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Get), GetType().Name);
            return Ok(results);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(Get), GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}