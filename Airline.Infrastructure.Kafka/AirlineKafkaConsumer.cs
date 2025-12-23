using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Airline.Infrastructure.Kafka;

/// <summary>
/// Kafka background consumer that subscribes to configured topic and creates flights from received contracts
/// </summary>
/// <param name="consumer">Kafka consumer instance</param>
/// <param name="scopeFactory">Service scope factory used to resolve scoped services</param>
/// <param name="configuration">Application configuration used to read Kafka settings</param>
/// <param name="logger">Logger instance</param>
public sealed class KafkaConsumer(
    IConsumer<string, IList<CreateFlightDto>> consumer,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<KafkaConsumer> logger) : BackgroundService
{
    private readonly string _topicName =
        configuration.GetSection("Kafka")["TopicName"] ?? throw new KeyNotFoundException("TopicName section of Kafka is missing");

    /// <summary>
    /// Starts consumer execution loop
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        try
        {
            consumer.Subscribe(_topicName);
            logger.LogInformation("Consumer successfully subscribed to topic {topic}", _topicName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to subscribe consumer {consumer} to topic {topic}", consumer.Name, _topicName);
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);

                if (consumeResult?.Message?.Value is null || consumeResult.Message.Value.Count == 0)
                    continue;

                logger.LogInformation(
                    "Consumed message {key} from topic {topic} via consumer {consumer}",
                    consumeResult.Message.Key, _topicName, consumer.Name);

                using var scope = scopeFactory.CreateScope();
                var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

                foreach (var contract in consumeResult.Message.Value)
                {
                    try
                    {
                        await flightService.CreateAsync(contract);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        logger.LogWarning(ex, "Skipping invalid flight contract ModelId={modelId}", contract.ModelId);
                    }
                }

                consumer.Commit(consumeResult);

                logger.LogInformation(
                    "Successfully processed and committed message {key} from topic {topic} via consumer {consumer}",
                    consumeResult.Message.Key, _topicName, consumer.Name);
            }
            catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
            {
                logger.LogWarning("Topic {topic} is not available yet, waiting...", _topicName);
                await Task.Delay(2000, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Токен отмены — выходим из цикла
                logger.LogInformation("Kafka consumer stopping due to cancellation token");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to consume or process message from topic {topic}", _topicName);
                await Task.Delay(1000, stoppingToken);
            }
        }

        // Закрытие потребителя автоматически происходит при Dispose
        logger.LogInformation("Kafka consumer stopped");
    }

    /// <summary>
    /// Automatically disposes the Kafka consumer when the service is disposed
    /// </summary>
    /// <returns></returns>
    public override void Dispose()
    {
        try
        {
            consumer?.Close();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error during consumer close");
        }
        finally
        {
            consumer?.Dispose();
        }
        base.Dispose();
    }
}