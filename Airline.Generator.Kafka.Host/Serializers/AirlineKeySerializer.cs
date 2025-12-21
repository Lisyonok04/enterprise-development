using Confluent.Kafka;
using System.Text.Json;

namespace Airline.Generator.Kafka.Host.Serializers;

/// <summary>
/// Kafka serializer that converts a string key into UTF8 JSON bytes
/// </summary>
public sealed class AirlineKeySerializer : ISerializer<string>
{
    /// <summary>
    /// Serializes string key into JSON byte array representation
    /// </summary>
    /// <param name="data">Key value to serialize</param>
    /// <param name="context">Serialization context provided by Kafka client</param>
    /// <returns>UTF8 JSON byte array</returns>
    public byte[] Serialize(string data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}