namespace projects_menagment.Application.Dtos.Organizations;

public sealed record AcceptOrganizationInvitationResponseDto(
    Guid OrganizationId,
    Guid UserId,
    string Role,
    string Message);
