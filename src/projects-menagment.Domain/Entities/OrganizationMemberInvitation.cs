using projects_menagment.Domain.Enums;

namespace projects_menagment.Domain.Entities;

public sealed class OrganizationMemberInvitation
{
    private OrganizationMemberInvitation()
    {
    }

    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public OrganizationMemberRole Role { get; private set; }
    public Guid InvitedByUserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }

    public bool IsAccepted => AcceptedAt.HasValue;
    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;

    public static OrganizationMemberInvitation Create(
        Guid organizationId,
        string email,
        OrganizationMemberRole role,
        Guid invitedByUserId,
        string token,
        DateTime expiresAt)
    {
        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("Organization id is required.", nameof(organizationId));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (invitedByUserId == Guid.Empty)
        {
            throw new ArgumentException("Invited by user id is required.", nameof(invitedByUserId));
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token is required.", nameof(token));
        }

        return new OrganizationMemberInvitation
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Email = email.Trim().ToLowerInvariant(),
            Role = role,
            InvitedByUserId = invitedByUserId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAccepted(DateTime utcNow)
    {
        if (!IsAccepted)
        {
            AcceptedAt = utcNow;
        }
    }
}
