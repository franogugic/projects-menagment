namespace projects_menagment.Application.Dtos.Organizations;

public sealed record CreateOrganizationResponseDto(
    Guid Id,
    string Name,
    Guid PlanId,
    Guid CreatedByUserId,
    DateTime CreatedAt);
