using System.Security.Cryptography;

namespace PortalAdm.SharedKernel.Security;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;  // 128-bit salt
    private const int KeySize = 32;   // 256-bit key
    private const int Iterations = 10000;  // Adjust based on security needs
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        // Generate salt
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive key
        var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm).GetBytes(KeySize);

        // Combine salt and key for storage
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        // Derive the key for the provided password using the same salt
        var providedKey = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithm).GetBytes(KeySize);

        // Compare keys
        return CryptographicOperations.FixedTimeEquals(providedKey, key);
    }
}