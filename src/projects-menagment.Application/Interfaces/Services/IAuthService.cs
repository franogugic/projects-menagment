using projects_menagment.Application.Dtos.Auth;

namespace projects_menagment.Application.Interfaces.Services;

public interface IAuthService
{
    Task<SignupResponseDto> SignupAsync(SignupRequestDto request, CancellationToken cancellationToken);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);
}
