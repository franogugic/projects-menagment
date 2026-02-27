namespace projects_menagment.Api.Dtos.Organizations;

public sealed record AcceptOrganizationInvitationResponseBodyDto(
    Guid OrganizationId,
    string OrganizationName,
    Guid UserId,
    string Role,
    string Message);
