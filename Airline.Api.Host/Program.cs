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
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<DataSeed>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AirlineProfile());
});

// Repositories 
builder.Services.AddTransient<IRepository<Flight, int>, FlightRepository>();
builder.Services.AddTransient<IRepository<Passenger, int>, PassengerRepository>();
builder.Services.AddTransient<IRepository<Ticket, int>, TicketRepository>();
builder.Services.AddTransient<IRepository<PlaneModel, int>, PlaneModelRepository>();
builder.Services.AddTransient<IRepository<ModelFamily, int>, ModelFamilyRepository>();

// Application Services
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPlaneModelService, PlaneModelService>();
builder.Services.AddScoped<IModelFamilyService, ModelFamilyService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("Airline"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }
});

// MongoDB 
builder.AddMongoDBClient("airlineClient");

builder.Services.AddDbContext<AirlineDbContext>((services, o) =>
{
    var db = services.GetRequiredService<IMongoDatabase>();
    o.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName);
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Airline API v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AirlineDbContext>();
    var seed = scope.ServiceProvider.GetRequiredService<DataSeed>();

    var flightsExist = dbContext.Flights.Any();
    if (!flightsExist)
    {
        // ModelFamilies
        foreach (var family in seed.ModelFamilies)
            await dbContext.ModelFamilies.AddAsync(family);

        // PlaneModels
        foreach (var model in seed.PlaneModels)
            await dbContext.PlaneModels.AddAsync(model);

        // Passengers
        foreach (var passenger in seed.Passengers)
            await dbContext.Passengers.AddAsync(passenger);

        // Flights
        foreach (var flight in seed.Flights)
            await dbContext.Flights.AddAsync(flight);

        // Tickets
        foreach (var ticket in seed.Tickets)
            await dbContext.Tickets.AddAsync(ticket);

        await dbContext.SaveChangesAsync();
        app.Logger.LogInformation("Ѕаза данных успешно заполнена тестовыми данными.");
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();