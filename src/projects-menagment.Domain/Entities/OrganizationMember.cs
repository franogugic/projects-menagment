using projects_menagment.Domain.Enums;

namespace projects_menagment.Domain.Entities;

public sealed class OrganizationMember
{
    private OrganizationMember()
    {
    }

    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid UserId { get; private set; }
    public OrganizationMemberRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static OrganizationMember Create(Guid organizationId, Guid userId, OrganizationMemberRole role)
    {
        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("Organization id is required.", nameof(organizationId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required.", nameof(userId));
        }

        return new OrganizationMember
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            UserId = userId,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
    }
}
