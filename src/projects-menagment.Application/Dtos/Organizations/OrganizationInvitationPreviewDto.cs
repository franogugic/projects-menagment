namespace projects_menagment.Application.Dtos.Organizations;

public sealed record OrganizationInvitationPreviewDto(
    Guid OrganizationId,
    string OrganizationName,
    string Email,
    string Role,
    DateTime ExpiresAt,
    bool IsAccepted,
    bool IsExpired);
