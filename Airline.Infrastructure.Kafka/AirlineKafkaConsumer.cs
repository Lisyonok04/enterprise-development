using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Airline.Infrastructure.Kafka;

/// <summary>
/// Служба для чтения данных из топика Kafka
/// </summary>
/// <param name="consumer">Kafka-консьюмер</param>
/// <param name="scopeFactory">Фабрика контекста</param>
/// <param name="configuration">Конфигурация</param>
/// <param name="logger">Логгер</param>
public class KafkaConsumer(
    IConsumer<string, IList<CreateFlightDto>> consumer,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<KafkaConsumer> logger) : BackgroundService
{
    private readonly string _topicName =
        configuration.GetSection("Kafka")["TopicName"] ?? throw new KeyNotFoundException("TopicName section of Kafka is missing");

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        await Task.Yield();
        await Consume(stoppingToken);
    }

    /// <summary>
    /// Хендлер для обработки получаемого сообщения
    /// </summary>
    /// <param name="stoppingToken">Токен отмены</param>
    private async Task Consume(CancellationToken stoppingToken)
    {
        consumer.Subscribe(_topicName);
        logger.LogInformation("Consumer successfully subscribed to topic {topic}", _topicName);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Consuming from topic {topic} via consumer {consumer}", _topicName, consumer.Name);
                var consumeResult = consumer.Consume(stoppingToken);

                if (consumeResult?.Message?.Value is null || consumeResult.Message.Value.Count == 0)
                    continue;

                using var scope = scopeFactory.CreateScope();
                var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

                foreach (var contract in consumeResult.Message.Value)
                {
                    await flightService.CreateAsync(contract);
                }

                consumer.Commit(consumeResult);
                logger.LogInformation("Successfully consumed message {key} from topic {topic} via consumer {consumer}",
                    consumeResult.Message.Key, _topicName, consumer.Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during receiving contracts from {topic}", _topicName);
        }
    }
}