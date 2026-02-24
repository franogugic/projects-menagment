namespace projects_menagment.Application.Dtos.Auth;

public sealed record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt);
