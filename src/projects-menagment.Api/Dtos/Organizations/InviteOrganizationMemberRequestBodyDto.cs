namespace projects_menagment.Api.Dtos.Organizations;

public sealed record InviteOrganizationMemberRequestBodyDto(
    Guid InvitedByUserId,
    string? Email,
    string? Role);
