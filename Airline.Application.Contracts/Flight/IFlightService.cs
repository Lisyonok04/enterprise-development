namespace Airline.Application.Contracts.Flight;

public interface IFlightService
{
    public Task<FlightDto> CreateAsync(CreateFlightDto dto);
    public Task<FlightDto?> GetByIdAsync(string id);
    public Task<List<FlightDto>> GetAllAsync();
    public Task<FlightDto> UpdateAsync(CreateFlightDto dto);
    public Task<bool> DeleteAsync(string id);
}