namespace Airline.Application.Contracts.Flight;

public interface IFlightService
{
	public Task<List<FlightDto>> GetAllAsync();
	public Task<FlightDto?> GetByIdAsync(string id);
	public Task<FlightDto> CreateAsync(FlightDto flight);
	public Task<FlightDto> UpdateAsync(FlightDto flight);
	public Task<bool> DeleteAsync(string id);
}