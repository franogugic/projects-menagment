namespace projects_menagment.Application.Dtos.Organizations;

public sealed record CreateOrganizationRequestDto(
    string Name,
    Guid PlanId,
    Guid CreatedByUserId);
