namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// DTO for creating a new passenger.
/// </summary>
/// <param name="Passport">Passport number.</param>
/// <param name="PassengerName">Full name of the passenger.</param>
/// <param name="DateOfBirth">Date of birth (YYYY-MM-DD).</param>
public record CreatePassengerDto(string Passport, string PassengerName, DateOnly DateOfBirth);