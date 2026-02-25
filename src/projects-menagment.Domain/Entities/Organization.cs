namespace projects_menagment.Domain.Entities;

public sealed class Organization
{
    private Organization()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Domain { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }

    public static Organization Create(string name, string domain)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Organization name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(domain))
        {
            throw new ArgumentException("Organization domain is required.", nameof(domain));
        }

        var normalizedDomain = domain.Trim().ToLowerInvariant();
        if (!normalizedDomain.StartsWith('@') || normalizedDomain.Length < 4 || !normalizedDomain.Contains('.'))
        {
            throw new ArgumentException("Organization domain must look like @company.com.", nameof(domain));
        }

        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Domain = normalizedDomain,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}
