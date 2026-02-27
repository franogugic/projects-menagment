using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IOrganizationMemberInvitationRepository
{
    Task AddAsync(OrganizationMemberInvitation invitation, CancellationToken cancellationToken);
    Task<OrganizationMemberInvitation?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task UpdateAsync(OrganizationMemberInvitation invitation, CancellationToken cancellationToken);
}
