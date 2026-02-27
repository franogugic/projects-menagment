namespace projects_menagment.Api.Dtos.Organizations;

public sealed record AcceptOrganizationInvitationResponseBodyDto(
    Guid OrganizationId,
    Guid UserId,
    string Role,
    string Message);
