using Confluent.Kafka;
using Airline.Application.Contracts.Flight;
using System.Text.Json;

namespace Airline.Generator.Kafka.Host.Serializers;

/// <summary>
/// Kafka serializer that converts a list of flight create contracts into UTF8 JSON bytes
/// </summary>
public sealed class AirlineValueSerializer : ISerializer<IList<CreateFlightDto>>
{
    /// <summary>
    /// Serializes contract batch into JSON byte array representation
    /// </summary>
    /// <param name="data">Batch of flight contracts to serialize</param>
    /// <param name="context">Serialization context provided by Kafka client</param>
    /// <returns>UTF8 JSON byte array</returns>
    public byte[] Serialize(IList<CreateFlightDto> data, SerializationContext context) =>
        JsonSerializer.SerializeToUtf8Bytes(data);
}