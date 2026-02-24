namespace projects_menagment.Api.Dtos.Auth;

public sealed class LoginRequestBodyDto
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}
