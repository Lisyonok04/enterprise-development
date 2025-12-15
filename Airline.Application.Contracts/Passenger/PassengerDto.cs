namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// DTO representing a passenger.
/// </summary>
/// <param name="Id">Unique identifier of the passenger.</param>
/// <param name="Passport">Passport number.</param>
/// <param name="PassengerName">Full name of the passenger.</param>
/// <param name="DateOfBirth">Date of birth (YYYY-MM-DD).</param>
public record PassengerDto(int Id, string Passport, string PassengerName, DateOnly DateOfBirth);