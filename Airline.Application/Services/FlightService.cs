using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class FlightService(
    IRepository<Flight, int> flightRepository,
    IRepository<PlaneModel, int> planeModelRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IFlightService

{
    public async Task<FlightDto> CreateAsync(CreateFlightDto dto)
    {
        // Проверяем, существует ли модель
        if (await planeModelRepository.GetAsync(dto.ModelId) is null)
            throw new KeyNotFoundException($"Aircraft model with Id = {dto.ModelId} does not exist.");

        // Преобразуем DTO → Domain
        var flight = mapper.Map<Flight>(dto);

        // Сохраняем
        var created = await flightRepository.CreateAsync(flight);

        // Возвращаем Domain → DTO
        return mapper.Map<FlightDto>(created);
    }

    public async Task<FlightDto?> GetByIdAsync(string id)
    {
        var flight = await flightRepository.GetAsync(id);
        return flight is null ? null : mapper.Map<FlightDto>(flight);
    }

    public async Task<List<FlightDto>> GetAllAsync()
    {
        var flights = await flightRepository.GetAllAsync();
        return flights.Select(f => mapper.Map<FlightDto>(f)).ToList();
    }

    public async Task<FlightDto> UpdateAsync(CreateFlightDto dto, int Id)
    {
        var existing = await flightRepository.GetAsync(Id);
        if (existing is null)
            throw new KeyNotFoundException($"Flight with Id = {Id} does not exist.");

        // Маппим обновлённые данные (можно использовать AutoMapper.UpdateFrom)
        mapper.Map(dto, existing);

        var updated = await flightRepository.UpdateAsync(existing);
        return mapper.Map<FlightDto>(updated);
    }

    public async Task<bool> DeleteAsync(string id) =>
        await flightRepository.DeleteAsync(id);
}