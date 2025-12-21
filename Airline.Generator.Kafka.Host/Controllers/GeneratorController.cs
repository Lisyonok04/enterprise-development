using Airline.Application.Contracts.Flight;
using Airline.Generator.Kafka.Host.Generator;
using Airline.Generator.Kafka.Host.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Generator.Kafka.Host.Controllers;

/// <summary>
/// Controller used to generate flight contracts and publish them via Kafka message broker
/// </summary>
/// <param name="logger">Logger instance</param>
/// <param name="producerService">Producer service used to send contracts</param>
/// <param name="configuration">Configuration instance used to read generator settings</param>
[Route("api/[controller]")]
[ApiController]
public sealed class GeneratorController(
    ILogger<GeneratorController> logger,
    IProducerService producerService,
    IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// Generates flight contracts and sends them via Kafka using batches and delay between sends
    /// </summary>
    /// <param name="batchSize">Number of contracts in each batch</param>
    /// <param name="payloadLimit">Total number of contracts to generate</param>
    /// <param name="waitTime">Delay in seconds between batches</param>
    /// <returns>List of generated flight contracts</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CreateFlightDto>>> Get(
        [FromQuery] int batchSize,
        [FromQuery] int payloadLimit,
        [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} flight contracts via {batchSize} batches with {waitTime}s delay",
            payloadLimit, batchSize, waitTime);

        try
        {
            var list = new List<CreateFlightDto>(payloadLimit);
            var counter = 0;

            // Чтение пула существующих ModelId из конфигурации
            var modelIds = configuration.GetSection("Generator:SeedModelIds")
                .Get<List<int>>() ?? [];

            if (modelIds.Count == 0)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "SeedModelIds configuration is empty or missing");

            while (counter < payloadLimit)
            {
                var currentBatchSize = Math.Min(batchSize, payloadLimit - counter);

                var batch = FlightGenerator.GenerateContracts(currentBatchSize, modelIds);

                await producerService.SendAsync(batch);

                logger.LogInformation("Batch of {batchSize} flight contracts has been sent to Kafka", currentBatchSize);

                counter += currentBatchSize;
                list.AddRange(batch);

                if (counter < payloadLimit && waitTime > 0)
                    await Task.Delay(waitTime * 1000);
            }

            logger.LogInformation("{Method} method of {Controller} executed successfully", nameof(Get), GetType().Name);
            return Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred during {Method} method of {Controller}", nameof(Get), GetType().Name);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}