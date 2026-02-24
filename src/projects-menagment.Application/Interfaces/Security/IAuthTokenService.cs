using projects_menagment.Application.Dtos.Auth;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Security;

public interface IAuthTokenService
{
    AuthTokensDto GenerateTokens(User user);
}
