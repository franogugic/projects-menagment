using projects_menagment.Domain.Entities;

namespace projects_menagment.Application.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task AddWithOwnerAsync(
        Organization organization,
        OrganizationMember ownerMember,
        CancellationToken cancellationToken);
}
