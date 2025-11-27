using Airline.Domain.Items;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace Airline.Infrastructure.EfCore;

/// <summary>
/// Database context for the airline management system.
/// Configures entity mappings and collections for MongoDB.
/// </summary>
public class AirlineDbContext(DbContextOptions<AirlineDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Collection of aircraft model families.
    /// </summary>
    public DbSet<ModelFamily> ModelFamilies => Set<ModelFamily>();

    /// <summary>
    /// Collection of aircraft models.
    /// </summary>
    public DbSet<PlaneModel> PlaneModels => Set<PlaneModel>();

    /// <summary>
    /// Collection of flights.
    /// </summary>
    public DbSet<Flight> Flights => Set<Flight>();

    /// <summary>
    /// Collection of passengers.
    /// </summary>
    public DbSet<Passenger> Passengers => Set<Passenger>();

    /// <summary>
    /// Collection of tickets.
    /// </summary>
    public DbSet<Ticket> Tickets => Set<Ticket>();

    /// <summary>
    /// Configures entity-to-collection mappings and property names for MongoDB.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Отключаем автоматические транзакции (MongoDB не поддерживает)
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        // ModelFamily → collection "model_families"
        modelBuilder.Entity<ModelFamily>(entity =>
        {
            entity.ToCollection("model_families");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).HasElementName("_id");
            entity.Property(f => f.NameOfFamily).HasElementName("family_name");
            entity.Property(f => f.ManufacturerName).HasElementName("manufacturer");
        });

        // PlaneModel → collection "plane_models"
        modelBuilder.Entity<PlaneModel>(entity =>
        {
            entity.ToCollection("plane_models");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasElementName("_id");
            entity.Property(m => m.ModelName).HasElementName("model_name");
            entity.Property(m => m.MaxRange).HasElementName("max_range_km");
            entity.Property(m => m.PassengerCapacity).HasElementName("passenger_capacity");
            entity.Property(m => m.CargoCapacity).HasElementName("cargo_capacity_tons");
            entity.Property(m => m.PlaneFamilyId).HasElementName("family_id"); // ← ссылка на ModelFamily
        });

        // Passenger → collection "passengers"
        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.ToCollection("passengers");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasElementName("_id");
            entity.Property(p => p.Passport).HasElementName("passport_number");
            entity.Property(p => p.PassengerName).HasElementName("full_name");
            entity.Property(p => p.DateOfBirth).HasElementName("date_of_birth");
        });

        // Flight → collection "flights"
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
            entity.Property(f => f.ModelId).HasElementName("plane_model_id"); // ← ссылка на PlaneModel
        });

        // Ticket → collection "tickets"
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