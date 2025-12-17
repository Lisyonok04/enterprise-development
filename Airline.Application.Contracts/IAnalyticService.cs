using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

/// <summary>
/// Defines a service contract for analytical and reporting operations over airline data.
/// Provides aggregated queries for business intelligence, operational insights, and passenger analytics.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Retrieves the top N flights by number of passengers.
    /// </summary>
    /// <param name="top">The number of top flights to return (default: 5).</param>
    /// <returns>A list of flight DTOs sorted by passenger count in descending order.</returns>
    public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5);

    /// <summary>
    /// Retrieves flights with the minimal travel time (duration).
    /// </summary>
    /// <returns>A list of flight DTOs with the shortest duration.</returns>
    public Task<List<FlightDto>> GetFlightsWithMinTravelTimeAsync();

    /// <summary>
    /// Retrieves passengers with zero baggage (no checked luggage) for a specific flight.
    /// </summary>
    /// <param name="flightId">The unique identifier of the flight.</param>
    /// <returns>A list of passenger DTOs who have no baggage on the specified flight.</returns>
    public Task<List<PassengerDto>> GetPassengersWithZeroBaggageOnFlightAsync(int flightId);

    /// <summary>
    /// Retrieves flights of a specific aircraft model within a given date period.
    /// </summary>
    /// <param name="modelId">The unique identifier of the aircraft model.</param>
    /// <param name="from">Start date of the period (inclusive).</param>
    /// <param name="to">End date of the period (inclusive).</param>
    /// <returns>A list of flight DTOs matching the model and date range.</returns>
    public Task<List<FlightDto>> GetFlightsByModelInPeriodAsync(int modelId, DateTime from, DateTime to);

    /// <summary>
    /// Retrieves flights by route (departure city to arrival city).
    /// </summary>
    /// <param name="departure">The city of departure.</param>
    /// <param name="arrival">The city of arrival.</param>
    /// <returns>A list of flight DTOs matching the specified route.</returns>
    public Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival);
}