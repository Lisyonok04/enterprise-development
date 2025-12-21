using Bogus;
using Airline.Application.Contracts.Flight;

namespace Airline.Generator.Kafka.Host.Generator;

/// <summary>
/// Generates random flight contracts to emulate an external system sending data
/// </summary>
public static class FlightGenerator
{
    /// <summary>
    /// Generates a list of flight create contracts
    /// </summary>
    /// <param name="count">Number of contracts to generate</param>
    /// <param name="modelIds">Pool of existing aircraft model identifiers</param>
    /// <returns>Generated list of flight contracts</returns>
    public static List<CreateFlightDto> GenerateContracts(int count, IList<int> modelIds) =>
        new Faker<CreateFlightDto>()
            .CustomInstantiator(f => new CreateFlightDto(
                FlightCode: f.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Number(100, 999),
                DepartureCity: f.Address.City(),
                ArrivalCity: f.Address.City(),
                DepartureDateTime: f.Date.Future(),
                ArrivalDateTime: f.Date.Future().AddHours(f.Random.Number(1, 24)),
                ModelId: f.PickRandom(modelIds)
            ))
            .Generate(count);
}