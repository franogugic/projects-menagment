using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class OrganizationRepository(
    AppDbContext dbContext,
    ILogger<OrganizationRepository> logger) : IOrganizationRepository
{
    public async Task AddAsync(Organization organization, CancellationToken cancellationToken)
    {
        logger.LogDebug("Persisting organization {OrganizationName}", organization.Name);

        dbContext.Organizations.Add(organization);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
