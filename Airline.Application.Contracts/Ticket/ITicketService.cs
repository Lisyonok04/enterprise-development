namespace Airline.Application.Contracts.Ticket;

public interface ITicketService
{
    public Task<List<TicketDto>> GetAllAsync();
    public Task<TicketDto?> GetByIdAsync(string id);
    public Task<TicketDto> CreateAsync(TicketDto ticket);
    public Task<TicketDto> UpdateAsync(TicketDto ticket);
    public Task<bool> DeleteAsync(string id);
}