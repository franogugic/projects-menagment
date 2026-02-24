namespace projects_menagment.Api.Dtos.Auth;

public sealed class SignupRequestBodyDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
}
