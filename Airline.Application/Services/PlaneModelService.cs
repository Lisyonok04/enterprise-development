using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Flight;
using Airline.Domain;
using Airline.Domain.Items;
using AutoMapper;

namespace Airline.Application.Services;

public class AircraftModelService(
    IRepository<PlaneModel, int> modelRepository,
    IRepository<ModelFamily, int> familyRepository,
    IRepository<Flight, int> flightRepository,
    IMapper mapper
) : IPlaneModelService