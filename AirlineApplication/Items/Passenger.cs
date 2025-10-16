namespace AirlineApplication.Items

/// <summary>
/// The class for describing a passenger.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Unique passenger's ID.
    /// </summary>
    public required int ID { get; set; }

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
    public required string DateOfBirth { get; set; }

}
