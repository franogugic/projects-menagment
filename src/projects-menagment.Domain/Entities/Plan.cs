using projects_menagment.Domain.Enums;

namespace projects_menagment.Domain.Entities;

public sealed class Plan
{
    private Plan()
    {
    }

    public Guid Id { get; private set; }
    public PlanCode Code { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int MaxProjects { get; private set; }
    public int MaxMembers { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }

    public static Plan Create(
        PlanCode code,
        string name,
        int maxProjects,
        int maxMembers,
        decimal price,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Plan name is required.", nameof(name));
        }

        if (maxProjects < 0)
        {
            throw new ArgumentException("Max projects must be zero or greater.", nameof(maxProjects));
        }

        if (maxMembers < 0)
        {
            throw new ArgumentException("Max members must be zero or greater.", nameof(maxMembers));
        }

        if (price < 0)
        {
            throw new ArgumentException("Price must be zero or greater.", nameof(price));
        }

        return new Plan
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name.Trim(),
            MaxProjects = maxProjects,
            MaxMembers = maxMembers,
            Price = price,
            IsActive = isActive
        };
    }
}
