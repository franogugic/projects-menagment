using projects_menagment.Domain.Entities;
using projects_menagment.Domain.Enums;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IOrganizationMemberRepository
{
    Task<OrganizationMemberRole?> GetUserRoleInOrganizationAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid organizationId, Guid userId, CancellationToken cancellationToken);
    Task AddAsync(OrganizationMember member, CancellationToken cancellationToken);
}
