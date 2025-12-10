using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application;

public class AirlineProfile : Profile
{
    // Конструктор профиля, создающий связи между Entity и Dto классами
    public AirlineProfile()
    {
        CreateMap<ModelFamily, ModelFamilyDto>();
        CreateMap<CreateModelFamilyDto, ModelFamily>();

        CreateMap<PlaneModel, PlaneModelDto>();
        CreateMap<CreatePlaneModelDto, PlaneModel>();

        CreateMap<Flight, FlightDto>();
        CreateMap<CreateFlightDto, Flight>();

        CreateMap<Passenger, PassengerDto>();
        CreateMap<CreatePassengerDto, Passenger>();

        CreateMap<Ticket, TicketDto>();
        CreateMap<CreateTicketDto, Ticket>();
    }
}