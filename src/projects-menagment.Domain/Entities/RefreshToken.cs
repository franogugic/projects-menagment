namespace projects_menagment.Domain.Entities;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAt;

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token is required.", nameof(token));
        }

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Revoke(DateTime utcNow)
    {
        if (!IsRevoked)
        {
            RevokedAt = utcNow;
        }
    }
}
