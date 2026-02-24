namespace projects_menagment.Api.Dtos.Auth;

public sealed record LoginResponseBodyDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt);
