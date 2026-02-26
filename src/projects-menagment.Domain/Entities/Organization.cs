namespace projects_menagment.Domain.Entities;

public sealed class Organization
{
    private Organization()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Guid PlanId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Organization Create(string name, Guid planId, Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Organization name is required.", nameof(name));
        }

        if (planId == Guid.Empty)
        {
            throw new ArgumentException("Plan id is required.", nameof(planId));
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new ArgumentException("Creator user id is required.", nameof(createdByUserId));
        }

        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            PlanId = planId,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
