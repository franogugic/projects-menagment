namespace projects_menagment.Application.Dtos.Auth;

public sealed record SignupRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password);
