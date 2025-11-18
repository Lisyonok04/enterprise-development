namespace Airline.Application.Contracts.Flight;
namespace Airline.Application.Contracts.Passenger;

public interface IAnalyticsService
{
	public Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5);
	public Task<List<FlightDto>> GetFlightsWithMinTravelTimeAsync();
	public Task<List<PassengerDto>> GetPassengersWithZeroBaggageOnFlightAsync(string flightId);
	public Task<List<FlightDto>> GetFlightsByModelInPeriodAsync(string modelName, DateTime from, DateTime to);
	public Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival);
}