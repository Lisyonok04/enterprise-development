using Airline.Application.Contracts.Flight;
using Bogus;

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
    /// <param name="departureCity">Pool of existing departure cities identifiers</param>
    /// <param name="arrivalCity">Pool of existing arrival cities identifiers</param>
    /// <returns>Generated list of flight contracts</returns>
    public static List<CreateFlightDto> GenerateContracts(
        int count,
        IList<int> modelIds,
        IList<string> departureCity,
        IList<string> arrivalCity) =>
        new Faker<CreateFlightDto>()
            .CustomInstantiator(f => new CreateFlightDto(
                FlightCode: $"{f.Random.Char('A', 'Z')}{f.Random.Char('A', 'Z')}{f.Random.Number(100, 999)}",
                DepartureCity: f.PickRandom(departureCity),
                ArrivalCity: f.PickRandom(arrivalCity),
                DepartureDateTime: f.Date.Future(),
                ArrivalDateTime: f.Date.Future().AddHours(f.Random.Number(1, 24)),
                ModelId: f.PickRandom(modelIds)
            ))
            .Generate(count);
}