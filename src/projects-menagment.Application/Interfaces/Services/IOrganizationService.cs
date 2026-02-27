using projects_menagment.Application.Dtos.Organizations;

namespace projects_menagment.Application.Interfaces.Services;

public interface IOrganizationService
{
    Task<CreateOrganizationResponseDto> CreateAsync(CreateOrganizationRequestDto request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UserOrganizationDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
