namespace Airline.Application.Contracts.Passenger;

/// <summary>
/// Data Transfer Object (DTO) representing a passenger in the airline system.
/// Used for read operations and data exchange with clients.
/// </summary>
/// <param name="Id">The unique identifier of the passenger.</param>
/// <param name="Passport">The passport number of the passenger.</param>
/// <param name="PassengerName">The full name of the passenger.</param>
/// <param name="DateOfBirth">The date of birth of the passenger.</param>
public record PassengerDto(
    int Id,
    string Passport,
    string PassengerName,
    DateOnly DateOfBirth
);