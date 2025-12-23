using Confluent.Kafka;
using System.Text.Json;

namespace Airline.Infrastructure.Kafka.Deserializers;

/// <summary>
/// Kafka deserializer for message key represented as string encoded in JSON
/// </summary>
public class AirlineKeyDeserializer : IDeserializer<string>
{
    /// <summary>
    /// Deserializes Kafka message key payload into string value
    /// </summary>
    /// <param name="data">Raw message key bytes</param>
    /// <param name="isNull">Indicates that the key is null</param>
    /// <param name="context">Serialization context</param>
    /// <returns>Deserialized string key</returns>
    public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            return null;

        return JsonSerializer.Deserialize<string>(data);
    }
}