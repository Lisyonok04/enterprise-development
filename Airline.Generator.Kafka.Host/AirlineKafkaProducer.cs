using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using Airline.Generator.Kafka.Host.Interfaces;

namespace Airline.Generator.Kafka.Host;

/// <summary>
/// Kafka producer service that publishes batches of flight create contracts to a configured topic
/// </summary>
/// <param name="configuration">Application configuration used to resolve Kafka topic name</param>
/// <param name="producer">Kafka producer used to send messages</param>
/// <param name="logger">Logger instance</param>
public sealed class AirlineKafkaProducer(
    IConfiguration configuration,
    IProducer<string, IList<CreateFlightDto>> producer,
    ILogger<AirlineKafkaProducer> logger) : IProducerService
{
    private readonly string _topicName =
        configuration.GetSection("Kafka")["TopicName"] ?? throw new KeyNotFoundException("TopicName section of Kafka is missing");

    /// <summary>
    /// Sends a batch of flight contracts to Kafka topic using flight code as message key
    /// </summary>
    /// <param name="batch">Batch of flight create contracts</param>
    public async Task SendAsync(IList<CreateFlightDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} contracts to {topic}", batch.Count, _topicName);

            var key = batch.FirstOrDefault()?.FlightCode ?? Guid.NewGuid().ToString();

            var message = new Message<string, IList<CreateFlightDto>>
            {
                Key = key,
                Value = batch
            };

            await producer.ProduceAsync(_topicName, message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} contracts to {topic}", batch.Count, _topicName);
        }
    }
}