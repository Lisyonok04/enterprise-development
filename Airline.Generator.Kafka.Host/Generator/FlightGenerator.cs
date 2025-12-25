using Airline.Application.Contracts.Flight;
using Bogus;

namespace Airline.Generator.Kafka.Host.Generator;

/// <summary>
/// Generates random flight contracts to emulate an external system sending data
/// </summary>
public static class FlightGenerator
{
    private static int[]? _modelFamilyIds;
    private static string[]? _departureCities;
    private static string[]? _arrivalCities;

    /// <summary>
    /// Initializes the generator with configuration data from the specified configuration source.
    /// </summary>
    /// <param name="configuration">Configuration instance containing generator settings.</param>
    public static void Initialize(IConfiguration configuration)
    {
        _modelFamilyIds = configuration.GetSection("FlightGenerator:ModelFamilyId").Get<int[]>() ?? [];
        _departureCities = configuration.GetSection("FlightGenerator:DepartureCity").Get<string[]>() ?? [];
        _arrivalCities = configuration.GetSection("FlightGenerator:ArrivalCity").Get<string[]>() ?? [];
    }

    /// <summary>
    /// Generates a list of flight create contracts
    /// </summary>
    /// <param name="count">Number of contracts to generate</param>
    /// <returns>Generated list of flight contracts</returns>
    public static List<CreateFlightDto> GenerateContracts(int count)
    {
        if (_modelFamilyIds?.Length == 0 ||
            _departureCities?.Length == 0 ||
            _arrivalCities?.Length == 0)
        {
            throw new InvalidOperationException("FlightGenerator configuration is empty");
        }

        try
        {
            return new Faker<CreateFlightDto>()
                .CustomInstantiator(f => new CreateFlightDto(
                    FlightCode: $"{f.Random.Char('A', 'Z')}{f.Random.Char('A', 'Z')}{f.Random.Number(100, 999)}",
                    DepartureCity: f.PickRandom(_departureCities),
                    ArrivalCity: f.PickRandom(_arrivalCities),
                    DepartureDateTime: f.Date.Future(),
                    ArrivalDateTime: f.Date.Future().AddHours(f.Random.Number(1, 10)),
                    ModelId: f.PickRandom(_modelFamilyIds)
                ))
                .Generate(count);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate contracts", ex);
        }
    }
}