using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;

public interface IAnalyticsService
{
	public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5);
	public Task<List<FlightDto>> GetFlightsWithMinTravelTimeAsync();
	public Task<List<PassengerDto>> GetPassengersWithZeroBaggageOnFlightAsync(int flightId);
	public Task<List<FlightDto>> GetFlightsByModelInPeriodAsync(int modelId, DateTime from, DateTime to);
	public Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival);
}