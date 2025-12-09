using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class PassengerService(
    IRepository<Passenger, int> passengerRepository,
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IMapper mapper
) : IPassengerService