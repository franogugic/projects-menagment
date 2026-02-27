namespace projects_menagment.Application.Interfaces.Communication;

public interface IEmailSender
{
    Task SendOrganizationInviteAsync(string toEmail, string organizationName, string inviteLink, CancellationToken cancellationToken);
}
