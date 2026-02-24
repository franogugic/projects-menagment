using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}
