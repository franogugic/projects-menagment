namespace projects_menagment.Application.Dtos.Auth;

public sealed record AuthTokensDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt);
