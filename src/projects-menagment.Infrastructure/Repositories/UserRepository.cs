using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class UserRepository(
    AppDbContext dbContext,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        logger.LogDebug("Checking if user exists by email {Email}", email);

        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        logger.LogDebug("Persisting user with email {Email}", user.Email);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User {UserId} persisted successfully", user.Id);
    }
}
