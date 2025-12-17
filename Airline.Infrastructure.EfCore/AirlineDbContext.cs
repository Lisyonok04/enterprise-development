using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Airline.Infrastructure.EfCore;

/// <summary>
/// Represents the database context for the airline management system.
/// Configures entity-to-collection mappings and property name conventions for MongoDB.
/// </summary>
public class AirlineDbContext(DbContextOptions<AirlineDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of aircraft model families.
    /// Mapped to the 'model_families' collection in MongoDB.
    /// </summary>
    public DbSet<ModelFamily> ModelFamilies { get; set; }

    /// <summary>
    /// Gets or sets the collection of aircraft models.
    /// Mapped to the 'plane_models' collection in MongoDB.
    /// </summary>
    public DbSet<PlaneModel> PlaneModels { get; set; }

    /// <summary>
    /// Gets or sets the collection of flights.
    /// Mapped to the 'flights' collection in MongoDB.
    /// </summary>
    public DbSet<Flight> Flights { get; set; }

    /// <summary>
    /// Gets or sets the collection of passengers.
    /// Mapped to the 'passengers' collection in MongoDB.
    /// </summary>
    public DbSet<Passenger> Passengers { get; set; }

    /// <summary>
    /// Gets or sets the collection of tickets.
    /// Mapped to the 'tickets' collection in MongoDB.
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; }

    /// <summary>
    /// Configures the model by mapping entities to MongoDB collections and customizing field names.
    /// Disables automatic transaction behavior (MongoDB does not support transactions in this context).
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure entity mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        modelBuilder.Entity<ModelFamily>(entity =>
        {
            entity.ToCollection("model_families");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasElementName("_id");
            entity.Property(f => f.NameOfFamily).HasElementName("family_name");
            entity.Property(f => f.ManufacturerName).HasElementName("manufacturer");
        });

        modelBuilder.Entity<PlaneModel>(entity =>
        {
            entity.ToCollection("plane_models");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasElementName("_id");
            entity.Property(m => m.ModelName).HasElementName("model_name");
            entity.Property(m => m.MaxRange).HasElementName("max_range_km");
            entity.Property(m => m.PassengerCapacity).HasElementName("passenger_capacity");
            entity.Property(m => m.CargoCapacity).HasElementName("cargo_capacity_tons");
            entity.Property(m => m.ModelFamilyId).HasElementName("family_id");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.ToCollection("passengers");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasElementName("_id");
            entity.Property(p => p.Passport).HasElementName("passport_number");
            entity.Property(p => p.PassengerName).HasElementName("full_name");
            entity.Property(p => p.DateOfBirth).HasElementName("date_of_birth");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.ToCollection("flights");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasElementName("_id");
            entity.Property(f => f.FlightCode).HasElementName("flight_code");
            entity.Property(f => f.DepartureCity).HasElementName("departure_city");
            entity.Property(f => f.ArrivalCity).HasElementName("arrival_city");
            entity.Property(f => f.DepartureDateTime).HasElementName("departure_datetime");
            entity.Property(f => f.ArrivalDateTime).HasElementName("arrival_datetime");
            entity.Property(f => f.ModelId).HasElementName("plane_model_id");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToCollection("tickets");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasElementName("_id");
            entity.Property(t => t.SeatNumber).HasElementName("seat_number");
            entity.Property(t => t.HandLuggage).HasElementName("has_hand_luggage");
            entity.Property(t => t.BaggageWeight).HasElementName("baggage_weight_kg");
            entity.Property(t => t.FlightId).HasElementName("flight_id");
            entity.Property(t => t.PassengerId).HasElementName("passenger_id");
        });
    }
}