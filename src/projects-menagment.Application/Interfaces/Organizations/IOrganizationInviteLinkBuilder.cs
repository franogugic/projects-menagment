namespace projects_menagment.Application.Interfaces.Organizations;

public interface IOrganizationInviteLinkBuilder
{
    string BuildInviteLink(string token);
}
