namespace projects_menagment.Api.Dtos.Organizations;

public sealed record OrganizationMemberResponseBodyDto(
    Guid UserId,
    string FirstName,
    string LastName,
    string Role);
