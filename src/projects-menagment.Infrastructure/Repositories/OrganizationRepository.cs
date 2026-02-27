using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Dtos.Organizations;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class OrganizationRepository(
    AppDbContext dbContext,
    ILogger<OrganizationRepository> logger) : IOrganizationRepository
{
    public async Task AddWithOwnerAsync(
        Organization organization,
        OrganizationMember ownerMember,
        CancellationToken cancellationToken)
    {
        logger.LogDebug(
            "Persisting organization {OrganizationName} and owner membership for user {UserId}",
            organization.Name,
            ownerMember.UserId);

        dbContext.Organizations.Add(organization);
        dbContext.OrganizationMembers.Add(ownerMember);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserOrganizationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching organizations for user {UserId}", userId);

        var memberships = await dbContext.OrganizationMembers
            .AsNoTracking()
            .Where(member => member.UserId == userId)
            .Join(
                dbContext.Organizations.AsNoTracking(),
                member => member.OrganizationId,
                organization => organization.Id,
                (member, organization) => new
                {
                    organization.Id,
                    organization.Name,
                    organization.PlanId,
                    organization.CreatedByUserId,
                    organization.CreatedAt,
                    member.Role
                })
            .OrderByDescending(item => item.CreatedAt)
            .ToListAsync(cancellationToken);

        return memberships
            .Select(item => new UserOrganizationDto(
                item.Id,
                item.Name,
                item.PlanId,
                item.CreatedByUserId,
                item.CreatedAt,
                item.Role.ToString().ToUpperInvariant()))
            .ToList();
    }

    public async Task<Organization?> GetByIdAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching organization by id {OrganizationId}", organizationId);

        return await dbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(org => org.Id == organizationId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrganizationMemberDto>> GetMembersByOrganizationIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Fetching organization members for organization {OrganizationId}", organizationId);

        var members = await dbContext.OrganizationMembers
            .AsNoTracking()
            .Where(member => member.OrganizationId == organizationId)
            .Join(
                dbContext.Users.AsNoTracking(),
                member => member.UserId,
                user => user.Id,
                (member, user) => new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    member.Role
                })
            .OrderBy(item => item.FirstName)
            .ThenBy(item => item.LastName)
            .ToListAsync(cancellationToken);

        return members
            .Select(item => new OrganizationMemberDto(
                item.Id,
                item.FirstName,
                item.LastName,
                item.Role.ToString().ToUpperInvariant()))
            .ToList();
    }
}
