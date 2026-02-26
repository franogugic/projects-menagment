namespace projects_menagment.Api.Dtos.Organizations;

public sealed record CreateOrganizationResponseBodyDto(
    Guid Id,
    string Name,
    Guid PlanId,
    Guid CreatedByUserId,
    DateTime CreatedAt);
