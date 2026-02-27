namespace projects_menagment.Api.Dtos.Organizations;

public sealed record OrganizationInvitationPreviewResponseBodyDto(
    Guid OrganizationId,
    string OrganizationName,
    string Email,
    string Role,
    DateTime ExpiresAt,
    bool IsAccepted,
    bool IsExpired);
