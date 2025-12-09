using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class AnalyticsService(
    IRepository<Flight, int> flightRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : IAnalyticsService
{
    public async Task<List<FlightDto>> GetTopFlightsByPassengerCountAsync(int top = 5)
    {
        var flights = await flightRepository.GetAllAsync();
        var tickets = await ticketRepository.GetAllAsync();

        // Группируем билеты по FlightId и считаем количество
        var flightPassengerCounts = tickets
            .GroupBy(t => t.FlightId)
            .Select(g => new { FlightId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top)
            .Select(x => x.FlightId)
            .ToHashSet();

        // Получаем полные объекты рейсов
        var topFlights = flights
            .Where(f => flightPassengerCounts.Contains(f.Id))
            .ToList();

        // Маппим в DTO и сортируем по убыванию числа пассажиров
        var flightCountMap = tickets
            .GroupBy(t => t.FlightId)
            .ToDictionary(g => g.Key, g => g.Count());

        return topFlights
            .OrderByDescending(f => flightCountMap.GetValueOrDefault(f.Id, 0))
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }

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

    public async Task<List<PassengerDto>> GetPassengersWithZeroBaggageOnFlightAsync(int flightId)
    {
        // Убеждаемся, что рейс существует
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

    public async Task<List<FlightDto>> GetFlightsByRouteAsync(string departure, string arrival)
    {
        var flights = await flightRepository.GetAllAsync();
        return flights
            .Where(f => f.DepartureCity == departure && f.ArrivalCity == arrival)
            .Select(mapper.Map<FlightDto>)
            .ToList();
    }
}