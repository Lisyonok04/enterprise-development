using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

/// <summary>
/// Provides analytical and reporting capabilities over airline data.
/// Implements aggregated queries for business intelligence and operational insights.
/// </summary>
public class AnalyticsService(
    IRepository<Flight, int> flightRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IAnalyticsService
{
    /// <summary>
    /// Retrieves the top N flights by number of passengers.
    /// </summary>
    /// <param name="top">The number of top flights to return (default: 5).</param>
    /// <returns>A list of flight DTOs sorted by passenger count in descending order.</returns>
    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5)
    {
        var flights = await flightRepository.GetAllAsync();
        var tickets = await ticketRepository.GetAllAsync();

        var flightPassengerCounts = tickets
            .GroupBy(t => t.FlightId)
            .Select(g => new { FlightId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .Select(x => x.FlightId)
            .ToHashSet();

        var topFlights = flights
            .Where(f => flightPassengerCounts.Contains(f.Id))
            .ToList();

        var flightCountMap = tickets
            .GroupBy(t => t.FlightId)
            .ToDictionary(g => g.Key, g => g.Count());

        return topFlights
            .OrderByDescending(f => flightCountMap.GetValueOrDefault(f.Id, 0))
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }

    /// <summary>
    /// Retrieves flights with the minimal travel time.
    /// </summary>
    /// <returns>A list of flight DTOs with the shortest duration.</returns>
    public async Task<List<FlightDto>> GetFlightsWithMinTravelTimeAsync()
    {
        var flights = await flightRepository.GetAllAsync();
        if (!flights.Any()) return [];

        var minTime = flights.Min(f => f.ArrivalDateTime - f.DepartureDateTime);
        return flights
            .Where(f => f.ArrivalDateTime - f.DepartureDateTime == minTime)
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }

    /// <summary>
    /// Retrieves passengers with zero baggage for a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of passenger DTOs with no baggage, sorted by full name.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the specified flight does not exist.
    /// </exception>
    public async Task<List<PassengerDto>> GetPassengersWithZeroBaggageOnFlightAsync(int flightId)
    {
        var flight = await flightRepository.GetAsync(flightId);
        if (flight == null)
            throw new KeyNotFoundException($"Flight with ID '{flightId}' not found.");

        var tickets = await ticketRepository.GetAllAsync();
        var passengerIds = tickets
            .Where(t => t.FlightId == flightId && t.BaggageWeight == null)
            .Select(t => t.PassengerId)
            .ToHashSet();

        if (!passengerIds.Any()) return [];

        var passengers = await passengerRepository.GetAllAsync();
        return passengers
            .Where(p => passengerIds.Contains(p.Id))
            .OrderBy(p => p.PassengerName) // сортировка по ФИО
            .Select(mapper.Map<PassengerDto>)
            .ToList();
    }

    /// <summary>
    /// Retrieves flights of a specific aircraft model within a date period.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <param name="from">Start date of the period (inclusive).</param>
    /// <param name="to">End date of the period (inclusive).</param>
    /// <returns>A list of flight DTOs matching the criteria.</returns>
    public async Task<List<FlightDto>> GetFlightsByModelInPeriodAsync(int modelId, DateTime from, DateTime to)
    {
        var flights = await flightRepository.GetAllAsync();
        return flights
            .Where(f => f.ModelId == modelId &&
                        f.DepartureDateTime >= from &&
                        f.DepartureDateTime <= to)
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }

    /// <summary>
    /// Retrieves flights by route (departure city → arrival city).
    /// </summary>
    /// <param name="departure">The city of departure.</param>
    /// <param name="arrival">The city of arrival.</param>
    /// <returns>A list of flight DTOs matching the route.</returns>
    public async Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival)
    {
        var flights = await flightRepository.GetAllAsync();
        return flights
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }
}