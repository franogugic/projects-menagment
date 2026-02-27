using projects_menagment.Application.Dtos.Organizations;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task AddWithOwnerAsync(
        Organization organization,
        OrganizationMember ownerMember,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserOrganizationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Organization?> GetByIdAsync(Guid organizationId, CancellationToken cancellationToken);
}
