namespace projects_menagment.Application.Dtos.Organizations;

public sealed record InviteOrganizationMemberRequestDto(
    Guid OrganizationId,
    Guid InvitedByUserId,
    string Email,
    string? Role);
