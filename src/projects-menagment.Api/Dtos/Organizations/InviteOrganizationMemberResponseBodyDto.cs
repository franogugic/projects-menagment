namespace projects_menagment.Api.Dtos.Organizations;

public sealed record InviteOrganizationMemberResponseBodyDto(
    Guid InvitationId,
    string Email,
    string Role,
    DateTime ExpiresAt,
    string InvitationLink);
