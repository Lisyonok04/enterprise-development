namespace Airline.Application.Contracts.Passenger;

public interface IPassengerService
{
    public Task<List<PassengerDto>> GetAllAsync();
    public Task<PassengerDto?> GetByIdAsync(string id);
    public Task<PassengerDto> CreateAsync(PassengerDto passenger);
    public Task<PassengerDto> UpdateAsync(PassengerDto passenger);
    public Task<bool> DeleteAsync(string id);
}