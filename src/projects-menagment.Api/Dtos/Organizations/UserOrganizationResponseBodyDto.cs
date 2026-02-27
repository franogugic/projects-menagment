namespace projects_menagment.Api.Dtos.Organizations;

public sealed record UserOrganizationResponseBodyDto(
    Guid OrganizationId,
    string Name,
    Guid PlanId,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    string Role);
