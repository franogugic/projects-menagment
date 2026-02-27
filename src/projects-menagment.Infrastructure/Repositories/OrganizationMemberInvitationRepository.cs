using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projects_menagment.Application.Interfaces.Repositories;
using projects_menagment.Domain.Entities;
using projects_menagment.Infrastructure.Persistence;

namespace projects_menagment.Infrastructure.Repositories;

public sealed class OrganizationMemberInvitationRepository(
    AppDbContext dbContext,
    ILogger<OrganizationMemberInvitationRepository> logger) : IOrganizationMemberInvitationRepository
{
    public async Task AddAsync(OrganizationMemberInvitation invitation, CancellationToken cancellationToken)
    {
        logger.LogDebug("Persisting organization invitation for email {Email}", invitation.Email);
        dbContext.OrganizationMemberInvitations.Add(invitation);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<OrganizationMemberInvitation?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await dbContext.OrganizationMemberInvitations
            .FirstOrDefaultAsync(i => i.Token == token, cancellationToken);
    }

    public async Task UpdateAsync(OrganizationMemberInvitation invitation, CancellationToken cancellationToken)
    {
        dbContext.OrganizationMemberInvitations.Update(invitation);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
