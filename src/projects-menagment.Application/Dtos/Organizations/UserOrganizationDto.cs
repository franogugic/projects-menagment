namespace projects_menagment.Application.Dtos.Organizations;

public sealed record UserOrganizationDto(
    Guid OrganizationId,
    string Name,
    Guid PlanId,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    string Role);
