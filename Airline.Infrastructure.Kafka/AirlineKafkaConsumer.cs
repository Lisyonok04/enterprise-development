using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using Airline.Domain;
using Airline.Domain.Items;
using Airline.Infrastructure.Kafka.Deserializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Airline.Infrastructure.Kafka;

/// <summary>
/// Kafka consumer service that processes flight contracts from a specified topic and persists them to the database.
/// </summary>
public sealed class FlightKafkaConsumer(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<FlightKafkaConsumer> logger
) : BackgroundService
{
    private readonly string _topicName =
        configuration["Kafka:TopicName"] ?? throw new KeyNotFoundException("Kafka:TopicName is missing");

    /// <summary>
    /// Initializes the Kafka consumer and starts the message processing loop with automatic reconnection.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = (configuration["KAFKA_BOOTSTRAP_SERVERS"] ?? "localhost:9092")
            .Replace("tcp://", "");

        logger.LogInformation("Kafka bootstrap servers: {bootstrapServers}", bootstrapServers);
        logger.LogInformation("Kafka topic name: {topicName}", _topicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var groupId = "airline-consumer-group-permanent";

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = bootstrapServers,
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                    SocketTimeoutMs = 20000,
                    SessionTimeoutMs = 10000,
                    HeartbeatIntervalMs = 3000
                };

                using var consumer = new ConsumerBuilder<string, IList<CreateFlightDto>>(consumerConfig)
                    .SetKeyDeserializer(new AirlineKeyDeserializer())
                    .SetValueDeserializer(new AirlineValueDeserializer())
                    .Build();

                consumer.Subscribe(_topicName);
                logger.LogInformation("Consumer successfully subscribed to topic {topic} with GroupId {groupId}", _topicName, groupId);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value is null || consumeResult.Message.Value.Count == 0)
                            continue;

                        logger.LogInformation(
                            "Consumed message {key} from topic {topic} (Partition: {partition}, Offset: {offset})",
                            consumeResult.Message.Key,
                            _topicName,
                            consumeResult.TopicPartition.Partition,
                            consumeResult.Offset);

                        using var scope = scopeFactory.CreateScope();
                        var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

                        foreach (var contract in consumeResult.Message.Value)
                        {
                            try
                            {
                                var modelExists = await scope.ServiceProvider
                                    .GetRequiredService<IRepository<PlaneModel, int>>()
                                    .GetAsync(contract.ModelId) != null;

                                if (!modelExists)
                                {
                                    logger.LogWarning("Skipping flight {code}: ModelId {modelId} does not exist",
                                        contract.FlightCode, contract.ModelId);
                                    continue;
                                }

                                await flightService.CreateAsync(contract);
                                logger.LogInformation("Successfully created flight {code} in database", contract.FlightCode);
                            }
                            catch (Exception ex)
                            {
                                logger.LogWarning(ex, "Skipping invalid flight contract: Code={code}, ModelId={modelId}",
                                    contract.FlightCode, contract.ModelId);
                            }
                        }

                        consumer.Commit(consumeResult);
                        logger.LogInformation("Successfully processed and committed message {key} from topic {topic}",
                            consumeResult.Message.Key, _topicName);
                    }
                    catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                    {
                        logger.LogWarning("Topic {topic} is not available yet, waiting 5 seconds before retry...", _topicName);
                        await Task.Delay(5000, stoppingToken);
                        break;
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        logger.LogInformation("Consumer operation cancelled due to shutdown request");
                        return;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to consume or process message from topic {topic}", _topicName);
                        await Task.Delay(2000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Consumer encountered an unrecoverable error, restarting in 5 seconds...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Performs graceful shutdown of the Kafka consumer service.
    /// </summary>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Kafka consumer is stopping");
        await base.StopAsync(stoppingToken);
    }
}