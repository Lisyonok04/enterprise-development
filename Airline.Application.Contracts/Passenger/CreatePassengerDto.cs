namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// Data Transfer Object (DTO) for creating a new passenger.
/// Contains personal identification and demographic data.
/// </summary>
/// <param name="Passport">The passport number of the passenger.</param>
/// <param name="PassengerName">The full name of the passenger (must not contain digits).</param>
/// <param name="DateOfBirth">The date of birth of the passenger.</param>
public record CreatePassengerDto(
    string Passport,
    string PassengerName,
    DateOnly DateOfBirth
);