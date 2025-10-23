using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Airline.Domain.Items;
namespace Airline.Domain.DataSeed;

/// <summary>
/// Seeds test data for the airline system, including model families, plane models, passengers, flights, and tickets.
/// </summary>
public class DataSeed
{
    public List<ModelFamily> ModelFamilies { get; }
    public List<PlaneModel> PlaneModels { get; }
    public List<Passenger> Passengers { get; }
    public List<Flight> Flights {  get; }
    public List<Ticket> Tickets { get; }

    public DataSeed()
    {
        ModelFamilies = InitModelFamilies();
        PlaneModels = InitPlaneModels(ModelFamilies);
        Passengers = InitPassengers();
        Flights = InitFlights(PlaneModels);
        Tickets = InitTickets(Flights, Passengers);
    }

    /// <summary>
    /// Initializes the model families with predefined data.
    /// </summary>
    /// <returns>List of <see cref="ModelFamily"/> objects.</returns>
    private static List<ModelFamily> InitModelFamilies() => new()
    {
        new() 
        { 
            ID = 1, 
            NameOfFamily = "A320 Family", 
            ManufacturerName = "Airbus" 
        },
        new() 
        { 
            ID = 2, 
            NameOfFamily = "737 Family", 
            ManufacturerName = "Boeing" 
        },
        new() 
        { 
            ID = 3, 
            NameOfFamily = "777 Family", 
            ManufacturerName = "Boeing" 
        },
        new() 
        { 
            ID = 4, 
            NameOfFamily = "787 Dreamliner", 
            ManufacturerName = "Boeing" 
        },
        new() 
        { 
            ID = 5, 
            NameOfFamily = "A330 Family", 
            ManufacturerName = "Airbus" 
        }
    };

    /// <summary>
    /// Initializes plane models linked to their families.
    /// </summary>
    /// <param name="families">List of model families.</param>
    /// <returns>List of <see cref="PlaneModel"/> objects.</returns>
    private static List<PlaneModel> InitPlaneModels(List<ModelFamily> families) => new()
    {
        new() 
        { 
            ID = 1, 
            ModelName = "A320", 
            PlaneFamily = families[0], 
            MaxRange = 6000, 
            PassengerCapacity = 180, 
            CargoCapacity = 20 
        },
        new() 
        { 
            ID = 2, 
            ModelName = "B737-800", 
            PlaneFamily = families[1], 
            MaxRange = 5500, 
            PassengerCapacity = 189, 
            CargoCapacity = 23 
        },
        new() 
        { 
            ID = 3, 
            ModelName = "B777-300ER", 
            PlaneFamily = families[2], 
            MaxRange = 11000, 
            PassengerCapacity = 370, 
            CargoCapacity = 45 
        },
        new() 
        { 
            ID = 4, 
            ModelName = "B787-9", 
            PlaneFamily = families[3], 
            MaxRange = 12000, 
            PassengerCapacity = 290, 
            CargoCapacity = 40 
        },
        new() 
        { 
            ID = 5, 
            ModelName = "A330-300", 
            PlaneFamily = families[4], 
            MaxRange = 10500, 
            PassengerCapacity = 300, 
            CargoCapacity = 42 
        }
    };

    /// <summary>
    /// Initializes a list of passengers.
    /// </summary>
    /// <returns>List of <see cref="Passenger"/> objects.</returns>
    private static List<Passenger> InitPassengers() => new()
    {
        new() 
        { 
            ID = 1, 
            Passport = "477419070", 
            PassengerName = "Ivanov Ivan", 
            DateOfBirth = "1990-01-15" 
        },
        new() 
        { 
            ID = 2, 
            Passport = "719011722", 
            PassengerName = "Petrov Petr", 
            DateOfBirth = "1985-05-22" 
        },
        new() 
        { 
            ID = 3, 
            Passport = "269997862", 
            PassengerName = "Alyohin Alexey", 
            DateOfBirth = "1992-03-10" 
        },
        new() 
        { 
            ID = 4, 
            Passport = "690256588", 
            PassengerName = "Kuzina Anna", 
            DateOfBirth = "1991-07-30" 
        },
        new() 
        { 
            ID = 5, 
            Passport = "816817823", 
            PassengerName = "Kuzin Dmitry", 
            DateOfBirth = "1988-11-05" 
        },
        new() 
        { 
            ID = 6, 
            Passport = "303776467", 
            PassengerName = "Nikitich Dobrynya", 
            DateOfBirth = "1995-09-18" 
        },
        new() 
        { 
            ID = 7, 
            Passport = "510907182", 
            PassengerName = "Popovich Alex", 
            DateOfBirth = "1993-04-12" 
        },
        new() 
        { 
            ID = 8, 
            Passport = "463835340", 
            PassengerName = "Kolyan", 
            DateOfBirth = "1987-08-25" 
        },
        new() 
        { 
            ID = 9, 
            Passport = "877654233", 
            PassengerName = "Lebedev Nikolay Ivanovich", 
            DateOfBirth = "1960-02-14" 
        },
        new() 
        { 
            ID = 10, 
            Passport = "112971133", 
            PassengerName = "Sokolov Tigran", 
            DateOfBirth = "1994-12-03" 
        }
    };

    /// <summary>
    /// Initializes flights with plane models and schedules.
    /// </summary>
    /// <param name="models">List of plane models.</param>
    /// <returns>List of <see cref="Flight"/> objects.</returns>
    private static List<Flight> InitFlights(List<PlaneModel> models) => new()
    {
        // Moscow → Berlin (2h) — A320
        new() 
        { 
            ID = 1, 
            FlightCode = "SU101", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Berlin",
            DepartureDate = new(2025, 10, 10), 
            ArrivalDate = new(2025, 10, 10),
            DepartureTime = new(8, 0, 0), 
            TravelTime = TimeSpan.FromHours(2), 
            Model = models[0] 
        },

        // Moscow → Paris (3.5h) — B737-800
        new() 
        {
            ID = 2, 
            FlightCode = "SU102", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Paris",    
            DepartureDate = new(2025, 10, 10), 
            ArrivalDate = new(2025, 10, 10),
            DepartureTime = new(9, 0, 0), 
            TravelTime = TimeSpan.FromHours(3.5), 
            Model = models[1] 
        },

        // Berlin → Paris (1.5h) — B777-300ER
        new() 
        { 
            ID = 3, 
            FlightCode = "SU103", 
            DepartureCity = "Berlin", 
            ArrivalCity = "Paris",
            DepartureDate = new(2025, 10, 10), 
            ArrivalDate = new(2025, 10, 10),
            DepartureTime = new(11, 0, 0), 
            TravelTime = TimeSpan.FromHours(1.5), 
            Model = models[2] 
        },

        // Moscow → Berlin (2h) — B787-9
        new() 
        { 
            ID = 4, 
            FlightCode = "SU104", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Berlin",
            DepartureDate = new(2025, 10, 11), 
            ArrivalDate = new(2025, 10, 11),
            DepartureTime = new(14, 0, 0), 
            TravelTime = TimeSpan.FromHours(2), 
            Model = models[3] 
        },

        // Rome → Milan (1h) — A330-300
        new() 
        { 
            ID = 5, 
            FlightCode = "AZ201", 
            DepartureCity = "Rome", 
            ArrivalCity = "Milan",
            DepartureDate = new(2025, 10, 11), 
            ArrivalDate = new(2025, 10, 11),
            DepartureTime = new(7, 0, 0), 
            TravelTime = TimeSpan.FromHours(1), 
            Model = models[4] 
        },

        // Moscow → Tokyo (10h) — A320
        new() 
        { 
            ID = 6, 
            FlightCode = "SU200", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Tokyo",
            DepartureDate = new(2025, 10, 12), 
            ArrivalDate = new(2025, 10, 12),
            DepartureTime = new(1, 0, 0), 
            TravelTime = TimeSpan.FromHours(10), 
            Model = models[0] 
        },

        // New York → London (6h) — B737-800
        new() 
        { 
            ID = 7, 
            FlightCode = "DL100", 
            DepartureCity = "New York", 
            ArrivalCity = "London",
            DepartureDate = new(2025, 10, 12), 
            ArrivalDate = new(2025, 10, 13),
            DepartureTime = new(18, 0, 0), 
            TravelTime = TimeSpan.FromHours(6), 
            Model = models[1] 
        },

        // Paris → Moscow (3h) — A320
        new() 
        { 
            ID = 8, 
            FlightCode = "SU105", 
            DepartureCity = "Paris", 
            ArrivalCity = "Moscow",
            DepartureDate = new(2025, 10, 13), 
            ArrivalDate = new(2025, 10, 13),
            DepartureTime = new(13, 0, 0), 
            TravelTime = TimeSpan.FromHours(3), 
            Model = models[0] 
        }
    };

    /// <summary>
    /// Initializes tickets linking flights to passengers.
    /// </summary>
    /// <param name="flights">List of flights.</param>
    /// <param name="passengers">List of passengers.</param>
    /// <returns>List of <see cref="Ticket"/> objects.</returns>
    private static List<Ticket> InitTickets(List<Flight> flights, List<Passenger> passengers) => new()
    {
        // SU101 (Moscow → Berlin) — 5 пассажиров, 2 без багажа
        new() 
        { 
            ID = 1, 
            Flight = flights[0], 
            Passenger = passengers[0], 
            SeatNumber = "12A", 
            HandLuggage = true, 
            BaggageWeight = 20.0 
        },
        new() 
        { 
            ID = 2, 
            Flight = flights[0], 
            Passenger = passengers[1], 
            SeatNumber = "12B", 
            HandLuggage = false, 
            BaggageWeight = null 
        },
        new() 
        { 
            ID = 3, 
            Flight = flights[0], 
            Passenger = passengers[2], 
            SeatNumber = "12C", 
            HandLuggage = true, 
            BaggageWeight = null 
        },
        new() 
        { 
            ID = 4, 
            Flight = flights[0], 
            Passenger = passengers[3], 
            SeatNumber = "13A", 
            HandLuggage = true, 
            BaggageWeight = 15.0 
        },
        new() 
        { 
            ID = 5, 
            Flight = flights[0], 
            Passenger = passengers[4], 
            SeatNumber = "13B", 
            HandLuggage = true, 
            BaggageWeight = 10.0 
        },

        // SU102 (Moscow → Paris) — 3 пассажира, 1 без багажа
        new() 
        { 
            ID = 6, 
            Flight = flights[1], 
            Passenger = passengers[5], 
            SeatNumber = "15A", 
            HandLuggage = true, 
            BaggageWeight = 12.0 
        },
        new() 
        { 
            ID = 7, 
            Flight = flights[1], 
            Passenger = passengers[6], 
            SeatNumber = "15B", 
            HandLuggage = true, 
            BaggageWeight = 8.0 
        },
        new() 
        { 
            ID = 8, 
            Flight = flights[1], 
            Passenger = passengers[7], 
            SeatNumber = "15C", 
            HandLuggage = false, 
            BaggageWeight = null 
        },

        // SU103 (Berlin → Paris) — 2 пассажира
        new() 
        { 
            ID = 9, 
            Flight = flights[2], 
            Passenger = passengers[8], 
            SeatNumber = "20A", 
            HandLuggage = true, 
            BaggageWeight = 5.0 
        },
        new() 
        { 
            ID = 10, 
            Flight = flights[2], 
            Passenger = passengers[9], 
            SeatNumber = "20B", 
            HandLuggage = true, 
            BaggageWeight = 7.0 
        },

        // SU104 (Moscow → Berlin) — 1 пассажир без багажа
        new() 
        { 
            ID = 11, 
            Flight = flights[3], 
            Passenger = passengers[0], 
            SeatNumber = "10A", 
            HandLuggage = false, 
            BaggageWeight = null 
        },

        // AZ201 (Rome → Milan) — 1 пассажир
        new() 
        { 
            ID = 12, 
            Flight = flights[4], 
            Passenger = passengers[1], 
            SeatNumber = "5A", 
            HandLuggage = true, 
            BaggageWeight = 6.0 
        },

        // SU200 (Moscow → Tokyo) — 1 пассажир
        new() 
        { 
            ID = 13, 
            Flight = flights[5], 
            Passenger = passengers[2], 
            SeatNumber = "1A", 
            HandLuggage = true, 
            BaggageWeight = 25.0 
        },

        // DL100 (New York → London) — 1 пассажир без багажа
        new() 
        { 
            ID = 14, 
            Flight = flights[6], 
            Passenger = passengers[3], 
            SeatNumber = "8A", 
            HandLuggage = false, 
            BaggageWeight = null 
        },

        // SU105 (Paris → Moscow) — 2 пассажира без багажа
        new() 
        { 
            ID = 15, 
            Flight = flights[7], 
            Passenger = passengers[4], 
            SeatNumber = "7A", 
            HandLuggage = true, 
            BaggageWeight = null 
        },
        new() 
        { 
            ID = 16, 
            Flight = flights[7], 
            Passenger = passengers[5], 
            SeatNumber = "7B", 
            HandLuggage = false, 
            BaggageWeight = null 
        }
    };
}