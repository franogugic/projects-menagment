using projects_menagment.Application.Dtos.Auth;

namespace projects_menagment.Application.Interfaces.Services;

public interface IAuthService
{
    Task<SignupResponseDto> SignupAsync(SignupRequestDto request, CancellationToken cancellationToken);
}
