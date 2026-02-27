namespace projects_menagment.Application.Dtos.Organizations;

public sealed record OrganizationMemberDto(
    Guid UserId,
    string FirstName,
    string LastName,
    string Role);
