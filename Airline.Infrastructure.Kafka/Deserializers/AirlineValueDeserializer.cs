using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using System.Text.Json;

namespace Airline.Infrastructure.Kafka.Deserializers;

/// <summary>
/// Kafka deserializer for message value represented as JSON array of CreateFlightDto contracts
/// </summary>
public sealed class AirlineValueDeserializer : IDeserializer<IList<CreateFlightDto>>
{
    /// <summary>
    /// Deserializes Kafka message value payload into list of CreateFlightDto contracts
    /// </summary>
    /// <param name="data">Raw message value bytes</param>
    /// <param name="isNull">Indicates that the value is null</param>
    /// <param name="context">Serialization context</param>
    /// <returns>Deserialized list of contracts or empty list when value is null</returns>
    public IList<CreateFlightDto> Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            return [];

        return JsonSerializer.Deserialize<IList<CreateFlightDto>>(data) ?? [];
    }
}