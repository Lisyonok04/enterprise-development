using Airline.Domain.Items;
namespace Airline.Domain.DataSeed;

/// <summary>
/// Seeds test data for the airline system, including model families, 
/// plane models, passengers, flights, and tickets.
/// </summary>
public class DataSeed
{
    /// <summary>
    /// Gets the list of seeded model families.
    /// </summary>
    public List<ModelFamily> ModelFamilies { get; }

    /// <summary>
    /// Gets the list of seeded plane models.
    /// </summary>
    public List<PlaneModel> PlaneModels { get; }

    /// <summary>
    /// Gets the list of seeded passengers.
    /// </summary>
    public List<Passenger> Passengers { get; }

    /// <summary>
    /// Gets the list of seeded flights.
    /// </summary>
    public List<Flight> Flights { get; }

    /// <summary>
    /// Gets the list of seeded tickets.
    /// </summary>
    public List<Ticket> Tickets { get; }

    ///<summary>
    ///Initializes the DataSeed class and fills it with data.
    ///</summary>
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
    private static List<ModelFamily> InitModelFamilies() => 
    [
        new ModelFamily
        { 
            Id = 1, 
            NameOfFamily = "A320 Family", 
            ManufacturerName = "Airbus" 
        },
        new ModelFamily
        { 
            Id = 2, 
            NameOfFamily = "767 Family", 
            ManufacturerName = "Boeing" 
        },
        new ModelFamily
        { 
            Id = 3, 
            NameOfFamily = "777 Family", 
            ManufacturerName = "Boeing" 
        },
        new ModelFamily
        { 
            Id = 4, 
            NameOfFamily = "787 Dreamliner", 
            ManufacturerName = "Boeing" 
        },
        new ModelFamily 
        { 
            Id = 5, 
            NameOfFamily = "A330 Family", 
            ManufacturerName = "Airbus" 
        }
    ];

    /// <summary>
    /// Initializes plane models linked to their families.
    /// </summary>
    private static List<PlaneModel> InitPlaneModels(List<ModelFamily> families) =>
    [
        new PlaneModel
        { 
            Id = 1, 
            ModelName = "A320", 
            PlaneFamily = families[0], 
            MaxRange = 6000, 
            PassengerCapacity = 180, 
            CargoCapacity = 20 
        },
        new PlaneModel
        { 
            Id = 2, 
            ModelName = "B767-300", 
            PlaneFamily = families[1], 
            MaxRange = 5500, 
            PassengerCapacity = 189, 
            CargoCapacity = 23 
        },
        new PlaneModel
        { 
            Id = 3, 
            ModelName = "B777-300ER", 
            PlaneFamily = families[2], 
            MaxRange = 11000, 
            PassengerCapacity = 370, 
            CargoCapacity = 45 
        },
        new PlaneModel
        { 
            Id = 4, 
            ModelName = "B787-9", 
            PlaneFamily = families[3], 
            MaxRange = 12000, 
            PassengerCapacity = 290, 
            CargoCapacity = 40 
        },
        new PlaneModel
        { 
            Id = 5, 
            ModelName = "A330-300", 
            PlaneFamily = families[4], 
            MaxRange = 10500, 
            PassengerCapacity = 300, 
            CargoCapacity = 42 
        }
    ];

    /// <summary>
    /// Initializes a list of passengers.
    /// </summary>
    private static List<Passenger> InitPassengers() =>
    [
        new Passenger
        { 
            Id = 1, 
            Passport = "477419070", 
            PassengerName = "Ivanov Ivan", 
            DateOfBirth = new(1990, 01, 15) 
        },
        new Passenger
        { 
            Id = 2, 
            Passport = "719011722", 
            PassengerName = "Petrov Petr", 
            DateOfBirth = new(1985, 05, 22) 
        },
        new Passenger
        { 
            Id = 3, 
            Passport = "269997862", 
            PassengerName = "Alyohin Alexey", 
            DateOfBirth = new(1992, 03, 10) 
        },
        new Passenger
        { 
            Id = 4, 
            Passport = "690256588", 
            PassengerName = "Kuzina Anna", 
            DateOfBirth = new(1991, 07, 30) 
        },
        new Passenger
        { 
            Id = 5, 
            Passport = "816817823", 
            PassengerName = "Kuzin Dmitry", 
            DateOfBirth = new(1988, 11, 05) 
        },
        new Passenger
        { 
            Id = 6, 
            Passport = "303776467", 
            PassengerName = "Nikitich Dobrynya", 
            DateOfBirth = new(1995, 09, 18) 
        },
        new Passenger
        { 
            Id = 7, 
            Passport = "510907182", 
            PassengerName = "Popovich Alex", 
            DateOfBirth = new(1993, 04, 12) 
        },
        new Passenger
        { 
            Id = 8, 
            Passport = "463835340", 
            PassengerName = "Kolyan", 
            DateOfBirth = new(1987, 08, 25) 
        },
        new Passenger
        { 
            Id = 9, 
            Passport = "877654233", 
            PassengerName = "Lebedev Nikolay Ivanovich", 
            DateOfBirth = new(1960, 02, 14) 
        },
        new Passenger
        { 
            Id = 10, 
            Passport = "112971133", 
            PassengerName = "Sokolov Tigran", 
            DateOfBirth = new(1994, 12, 03) 
        }
    ];

    /// <summary>
    /// Initializes flights with plane models and schedules.
    /// </summary>
    private static List<Flight> InitFlights(List<PlaneModel> models) =>
    [

        new Flight
        { 
            Id = 1, 
            FlightCode = "SU101", 
            DepartureCity = "Samara", 
            ArrivalCity = "Wonderland",
            DepartureDateTime = new DateTime(2025, 10, 10, 8, 0, 0), 
            ArrivalDateTime = new DateTime(2025, 10, 10, 15, 0, 0),
            TravelTime = TimeSpan.FromHours(2), 
            Model = models[0] 
        },

        new Flight 
        {
            Id = 2, 
            FlightCode = "SU102", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Paris",
            DepartureDateTime = new(2025, 10, 10, 6, 5, 0),
            ArrivalDateTime = new(2025, 10, 10, 9, 5, 0),
            TravelTime = TimeSpan.FromHours(3), 
            Model = models[1] 
        },

        new Flight
        { 
            Id = 3, 
            FlightCode = "SU103", 
            DepartureCity = "Berlin", 
            ArrivalCity = "Paris",
            DepartureDateTime = new(2025, 10, 10, 5, 0, 0),
            ArrivalDateTime = new(2025, 10, 10, 10, 0, 0),
            TravelTime = TimeSpan.FromHours(5), 
            Model = models[2] 
        },

        new Flight
        { 
            Id = 4, 
            FlightCode = "SU104", 
            DepartureCity = "Samara", 
            ArrivalCity = "Wonderland",
            DepartureDateTime = new(2025, 10, 11, 6, 0, 0),
            ArrivalDateTime = new(2025, 10, 11, 8, 30, 0),
            TravelTime = TimeSpan.FromHours(2.5), 
            Model = models[3] 
        },

        new Flight
        { 
            Id = 5, 
            FlightCode = "AZ201", 
            DepartureCity = "Rome", 
            ArrivalCity = "Milan",
            DepartureDateTime = new(2025, 10, 11, 22, 0, 0),
            ArrivalDateTime = new(2025, 10, 12, 2, 30, 0),
            TravelTime = TimeSpan.FromHours(4.5), 
            Model = models[4] 
        },

        new Flight
        { 
            Id = 6, 
            FlightCode = "SU200", 
            DepartureCity = "Moscow", 
            ArrivalCity = "Tokyo",
            DepartureDateTime = new(2025, 10, 11, 15, 0, 0),
            ArrivalDateTime = new(2025, 10, 12, 6, 0, 0),
            TravelTime = TimeSpan.FromHours(15), 
            Model = models[0] 
        },

        new Flight
        { 
            Id = 7, 
            FlightCode = "DL100", 
            DepartureCity = "New York", 
            ArrivalCity = "London",
            DepartureDateTime = new(2025, 10, 12, 7, 20, 0),
            ArrivalDateTime = new(2025, 10, 13, 13, 20, 0),
            TravelTime = TimeSpan.FromHours(6), 
            Model = models[1] 
        },

        new Flight
        { 
            Id = 8, 
            FlightCode = "SU105", 
            DepartureCity = "Paris", 
            ArrivalCity = "Moscow",
            DepartureDateTime = new(2025, 10, 13, 23, 0, 0),
            ArrivalDateTime = new(2025, 10, 14, 6, 0, 0),
            TravelTime = TimeSpan.FromHours(7), 
            Model = models[0] 
        }
    ];

    /// <summary>
    /// Initializes tickets linking flights to passengers.
    /// </summary>
    private static List<Ticket> InitTickets(List<Flight> flights, List<Passenger> passengers) =>
    [

        new Ticket
        { 
            Id = 1, 
            Flight = flights[0], 
            Passenger = passengers[0], 
            SeatNumber = "12A", 
            HandLuggage = true, 
            BaggageWeight = 15.6 
        },
        new Ticket
        { 
            Id = 2, 
            Flight = flights[0], 
            Passenger = passengers[1], 
            SeatNumber = "12B", 
            HandLuggage = false, 
            BaggageWeight = null 
        },
        new Ticket
        { 
            Id = 3, 
            Flight = flights[0], 
            Passenger = passengers[2], 
            SeatNumber = "12C", 
            HandLuggage = true, 
            BaggageWeight = null 
        },
        new Ticket
        { 
            Id = 4, 
            Flight = flights[0], 
            Passenger = passengers[3], 
            SeatNumber = "13A", 
            HandLuggage = true, 
            BaggageWeight = 1.2 
        },
        new Ticket
        { 
            Id = 5, 
            Flight = flights[0], 
            Passenger = passengers[4], 
            SeatNumber = "13B", 
            HandLuggage = true, 
            BaggageWeight = 10.0 
        },

        new Ticket
        { 
            Id = 6, 
            Flight = flights[1], 
            Passenger = passengers[5], 
            SeatNumber = "15A", 
            HandLuggage = true, 
            BaggageWeight = 5.2 
        },
        new Ticket
        { 
            Id = 7, 
            Flight = flights[1], 
            Passenger = passengers[6], 
            SeatNumber = "15B", 
            HandLuggage = true, 
            BaggageWeight = 18.0 
        },
        new Ticket
        { 
            Id = 8, 
            Flight = flights[1], 
            Passenger = passengers[7], 
            SeatNumber = "15C", 
            HandLuggage = false, 
            BaggageWeight = null 
        },

        new Ticket
        { 
            Id = 9, 
            Flight = flights[2], 
            Passenger = passengers[8], 
            SeatNumber = "20A", 
            HandLuggage = true, 
            BaggageWeight = 3.2 
        },
        new Ticket
        { 
            Id = 10, 
            Flight = flights[2], 
            Passenger = passengers[9], 
            SeatNumber = "20B", 
            HandLuggage = true, 
            BaggageWeight = 7.0 
        },

        new Ticket
        { 
            Id = 11, 
            Flight = flights[3], 
            Passenger = passengers[0], 
            SeatNumber = "10A", 
            HandLuggage = false, 
            BaggageWeight = 4.2 
        },

        new Ticket
        { 
            Id = 12, 
            Flight = flights[4], 
            Passenger = passengers[1], 
            SeatNumber = "5A", 
            HandLuggage = true, 
            BaggageWeight = 6.0 
        },

        new Ticket
        { 
            Id = 13, 
            Flight = flights[5], 
            Passenger = passengers[2], 
            SeatNumber = "1A", 
            HandLuggage = true, 
            BaggageWeight = 25.0 
        },

        new Ticket
        { 
            Id = 14, 
            Flight = flights[6], 
            Passenger = passengers[3], 
            SeatNumber = "8A", 
            HandLuggage = false, 
            BaggageWeight = null 
        },

        new Ticket
        { 
            Id = 15, 
            Flight = flights[7], 
            Passenger = passengers[4], 
            SeatNumber = "7A", 
            HandLuggage = true, 
            BaggageWeight = 11.6
        },
        new Ticket
        { 
            Id = 16, 
            Flight = flights[7], 
            Passenger = passengers[5], 
            SeatNumber = "7B", 
            HandLuggage = false, 
            BaggageWeight = 0.5 
        }
    ];
}