using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.Ticket;
using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application;

/// <summary>
/// AutoMapper profile that defines mapping configurations between 
/// domain entities and Data Transfer Objects (DTOs).
/// Ensures consistent and safe conversion between layers of the application.
/// </summary>
public class AirlineProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AirlineProfile"/> class
    /// and configures all entity-to-DTO and DTO-to-entity mappings.
    /// </summary>
    public AirlineProfile()
    {
        // ModelFamily mappings
        CreateMap<ModelFamily, ModelFamilyDto>();
        CreateMap<CreateModelFamilyDto, ModelFamily>();

        // PlaneModel mappings
        CreateMap<PlaneModel, PlaneModelDto>();
        CreateMap<CreatePlaneModelDto, PlaneModel>();

        // Flight mappings
        CreateMap<Flight, FlightDto>();
        CreateMap<CreateFlightDto, Flight>();

        // Passenger mappings
        CreateMap<Passenger, PassengerDto>();
        CreateMap<CreatePassengerDto, Passenger>();

        // Ticket mappings
        CreateMap<Ticket, TicketDto>();
        CreateMap<CreateTicketDto, Ticket>();
    }
}