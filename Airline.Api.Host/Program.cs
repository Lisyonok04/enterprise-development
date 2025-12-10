using Airline.Api.Host; 
using Airline.Application;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.ModelFamily;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Contracts.PlaneModel;
using Airline.Application.Contracts.Ticket;
using Airline.Application.Services; 
using Airline.Domain;
using Airline.Domain.DataSeed;
using Airline.Domain.Items;
using Airline.Infrastructure.EfCore; 
using Airline.Infrastructure.EfCore.Repositories;
using Airline.ServiceDefaults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddSingleton<DataSeed>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AirlineProfile());
});

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPlaneModelService, PlaneModelService>();
builder.Services.AddScoped<IModelFamilyService, ModelFamilyService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<IRepository<Flight, int>, FlightRepository>();
builder.Services.AddScoped<IRepository<Passenger, int>, PassengerRepository>();
builder.Services.AddScoped<IRepository<Ticket, int>, TicketRepository>();
builder.Services.AddScoped<IRepository<PlaneModel, int>, PlaneModelRepository>();
builder.Services.AddScoped<IRepository<ModelFamily, int>, ModelFamilyRepository>();

builder.Services.AddDbContext<AirlineDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mongodb");
    if (string.IsNullOrEmpty(connectionString))
        connectionString = "mongodb://localhost:27017";

    options.UseMongoDB(connectionString, "AirlineDb");
});

// Контроллеры и API
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Airline API",
        Version = "v1",
        Description = "REST API для управления авиакомпанией"
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressMapClientErrors = true;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Airline API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();