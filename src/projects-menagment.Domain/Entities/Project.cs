using projects_menagment.Domain.Enums;

namespace projects_menagment.Domain.Entities;

public sealed class Project
{
    private Project()
    {
    }

    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Budget { get; private set; }
    public ProjectStatus Status { get; private set; } = ProjectStatus.PendingApproval;
    public DateTime? Deadline { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }

    public static Project Create(Guid organizationId, Guid createdByUserId, string name, decimal budget, DateTime? deadline)
    {
        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("Organization id is required.", nameof(organizationId));
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new ArgumentException("Creator user id is required.", nameof(createdByUserId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name is required.", nameof(name));
        }

        if (budget < 0)
        {
            throw new ArgumentException("Budget must be zero or greater.", nameof(budget));
        }

        return new Project
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            CreatedByUserId = createdByUserId,
            Name = name.Trim(),
            Budget = budget,
            Status = ProjectStatus.PendingApproval,
            Deadline = deadline,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}
