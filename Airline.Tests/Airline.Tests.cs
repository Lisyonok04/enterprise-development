using Airline.Domain.DataSeed;
using Xunit;

namespace Airline.Tests;

public class AirCompanyTests(DataSeed _seed) : IClassFixture<DataSeed>
{
    /// <summary>
    /// Verifies that the method returns the top 5 flights based on the number of passengers transported.
    /// </summary>
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

    /// <summary>
    /// Verifies that the method correctly finds flights with minimal travel time.
    /// </summary>
    [Fact]
    public void GetFlightsWithMinTravelTime_ReturnsCorrectFlights()
    {
        var validFlights = _seed.Flights.Where(f => f.TravelTime.HasValue).ToList();
        var minTime = validFlights.Min(f => f.TravelTime!.Value);
        var result = validFlights
            .Where(f => f.TravelTime == minTime)
            .ToList();

        Assert.Single(result);
        Assert.Equal(TimeSpan.FromHours(2), result[0].TravelTime);
    }

    /// <summary>
    /// Checks that the method returns a list of passengers on the selected flight without baggage, 
    /// sorted by full name (PassengerName) in alphabetical order.
    /// </summary>
    [Fact]
    public void GetPassengersWithZeroBaggageOnsFlight_ReturnsSortedPassengers()
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

    /// <summary>
    /// Verifies that the method returns all flights of the specified aircraft model 
    /// that departed during the specified date period.
    /// </summary>
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
    }

    /// <summary>
    /// Verifies that the method returns all flights from the specified departure point 
    /// to the specified arrival point.
    /// </summary>
    [Fact]
    public void GetFlightsByRoute_ReturnsCorrectFlights()
    {
        var departure = "Samara";
        var arrival = "Wonderland";

        var result = _seed.Flights
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.FlightCode == "SU101");
        Assert.Contains(result, f => f.FlightCode == "SU104");
    }
}
