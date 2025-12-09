using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class TicketService(
    IRepository<Ticket, int> ticketRepository,
    IRepository<Flight, int> flightRepository,
    IRepository<Passenger, int> passengerRepository,
    IMapper mapper
) : ITicketService