namespace Airline.Domain.Items;

/// <summary>
/// The class for describing a passenger.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Unique passenger's Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The number of passenger's pasport.
    /// </summary>
    public required string Passport { get; set; }

    /// <summary>
    /// Passenger's full name.
    /// </summary>
    public required string PassengerName { get; set; }

    /// <summary>
    /// Passenger's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

}