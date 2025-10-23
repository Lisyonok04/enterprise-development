using Airline.Domain.DataSeed;
using Xunit;

namespace Airline.Tests;

public class AirCompanyTests(DataSeed _seed) : IClassFixture<DataSeed>
{
    [Fact]
    public void GetTop5FlightsByPassengerCount_ReturnsCorrectFlights()
    {
        var flightPassengerCounts = _seed.Tickets
            .GroupBy(t => t.Flight)
            .Select(g => new { Flight = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Select(x => x.Flight)
            .ToList();

        Assert.Equal(5, flightPassengerCounts.Count);
        Assert.Equal("SU101", flightPassengerCounts[0].FlightCode);
    }

    [Fact]
    public void GetFlightsWithMinTravelTime_ReturnsCorrectFlights()
    {
        var validFlights = _seed.Flights.Where(f => f.TravelTime.HasValue).ToList();
        var minTime = validFlights.Min(f => f.TravelTime!.Value);
        var result = validFlights
            .Where(f => f.TravelTime == minTime)
            .ToList();

        Assert.Single(result);
        Assert.Equal("AZ201", result[0].FlightCode);
        Assert.Equal(TimeSpan.FromHours(1), result[0].TravelTime);
    }

    [Fact]
    public void GetPassengersWithZeroBaggageOnFlight_ReturnsSortedPassengers()
    {
        var flight = _seed.Flights.First(f => f.FlightCode == "SU101");
        var passengersWithNoBaggage = _seed.Tickets
            .Where(t => t.Flight == flight && t.BaggageWeight == null)
            .Select(t => t.Passenger)
            .OrderBy(p => p.PassengerName)
            .ToList();

        Assert.Equal(2, passengersWithNoBaggage.Count);
        Assert.Equal("Alyohin Alexey", passengersWithNoBaggage[0].PassengerName);
        Assert.Equal("Petrov Petr", passengersWithNoBaggage[1].PassengerName);
    }

    [Fact]
    public void GetFlightsByModelInPeriod_ReturnsCorrectFlights()
    {
        var modelName = "A320";
        var from = new DateOnly(2025, 10, 10);
        var to = new DateOnly(2025, 10, 12);

        var result = _seed.Flights
            .Where(f => f.Model.ModelName == modelName &&
                        f.DepartureDate >= from &&
                        f.DepartureDate <= to)
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.FlightCode == "SU101");
        Assert.Contains(result, f => f.FlightCode == "SU200");
        Assert.DoesNotContain(result, f => f.FlightCode == "SU105");
    }

    [Fact]
    public void GetFlightsByRoute_ReturnsCorrectFlights()
    {
        var departure = "Moscow";
        var arrival = "Berlin";

        var result = _seed.Flights
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.FlightCode == "SU101");
        Assert.Contains(result, f => f.FlightCode == "SU104");
    }
}