using Microsoft.Extensions.Logging;
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
}
