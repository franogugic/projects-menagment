using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Domain.Enums;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class OrganizationMemberRepository(
    AppDbContext dbContext,
    ILogger<OrganizationMemberRepository> logger) : IOrganizationMemberRepository
{
    public async Task<OrganizationMemberRole?> GetUserRoleInOrganizationAsync(
        Guid organizationId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching membership role in organization {OrganizationId} for user {UserId}", organizationId, userId);

        return await dbContext.OrganizationMembers
            .AsNoTracking()
            .Where(m => m.OrganizationId == organizationId && m.UserId == userId)
            .Select(m => (OrganizationMemberRole?)m.Role)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.OrganizationMembers
            .AsNoTracking()
            .AnyAsync(m => m.OrganizationId == organizationId && m.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(OrganizationMember member, CancellationToken cancellationToken)
    {
        dbContext.OrganizationMembers.Add(member);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
