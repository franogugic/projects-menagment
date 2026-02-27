namespace projects_menagment.Application.Dtos.Organizations;

public sealed record InviteOrganizationMemberResponseDto(
    Guid InvitationId,
    string Email,
    string Role,
    DateTime ExpiresAt,
    string InvitationLink);
