using Airline.Application.Contracts.Flight;

namespace Airline.Generator.Kafka.Host.Interfaces;

/// <summary>
/// Abstraction for producing batches of flight create contracts to a Kafka message broker
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Sends a batch of flight create contracts
    /// </summary>
    /// <param name="batch">Batch of flight create contracts to send</param>
    /// <returns>Task representing asynchronous send operation</returns>
    public Task SendAsync(IList<CreateFlightDto> batch);
}