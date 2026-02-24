using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class RefreshTokenRepository(
    AppDbContext dbContext,
    ILogger<RefreshTokenRepository> logger) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        logger.LogDebug("Persisting refresh token for user {UserId}", refreshToken.UserId);

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching refresh token");

        return await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        logger.LogDebug("Updating refresh token {RefreshTokenId}", refreshToken.Id);

        dbContext.RefreshTokens.Update(refreshToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
