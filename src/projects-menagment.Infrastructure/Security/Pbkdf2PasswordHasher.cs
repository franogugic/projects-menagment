using System.Security.Cryptography;
using projects_menagment.Application.Interfaces.Security;

namespace projects_menagment.Infrastructure.Security;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public string HashPassword(string password)
    {
        Span<byte> salt = stackalloc byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA512,
            KeySize);

        return $"pbkdf2-sha512${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            var parts = passwordHash.Split('$');
            if (parts.Length != 4 || !parts[0].Equals("pbkdf2-sha512", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!int.TryParse(parts[1], out var iterations))
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA512,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
