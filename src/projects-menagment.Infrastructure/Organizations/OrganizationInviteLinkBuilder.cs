using Microsoft.Extensions.Configuration;
using projects_menagment.Application.Interfaces.Organizations;

namespace projects_menagment.Infrastructure.Organizations;

public sealed class OrganizationInviteLinkBuilder(IConfiguration configuration) : IOrganizationInviteLinkBuilder
{
    private readonly string _frontendOrigin = configuration["Frontend:Origin"] ?? "http://localhost:5173";

    public string BuildInviteLink(string token)
    {
        return $"{_frontendOrigin.TrimEnd('/')}/organization-invitations/accept?token={Uri.EscapeDataString(token)}";
    }
}
