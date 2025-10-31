using System.Security.Cryptography;
using System.Text;

namespace TodoApp.Infrastructure.Security;

/// <summary>
/// Provides functionality to hash and verify passwords using a secure, modern algorithm.
/// This implementation uses PBKDF2 (Password-Based Key Derivation Function 2).
/// </summary>
public static class PasswordHasher
{
    // Constants for hashing algorithm configuration.
    // Using higher iteration counts increases security but also slows down the process.
    private const int SaltSize = 16; // 128 bit salt
    private const int KeySize = 32;  // 256 bit hash
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    /// <summary>
    /// Hashes a plain text password using PBKDF2 with a cryptographically secure random salt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A string containing the hashed password, salt, and parameters, ready to be stored in the database.</returns>
    public static string HashPassword(string password)
    {
        // 1. Generate a random salt.
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        // 2. Create the hash using PBKDF2.
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            _hashAlgorithm,
            KeySize);

        // 3. Combine salt and hash into a single string for storage.
        // Format: "Base64(salt);Base64(hash)"
        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    /// <summary>
    /// Verifies that a plain text password matches a stored hashed password.
    /// </summary>
    /// <param name="password">The plain text password entered by the user.</param>
    /// <param name="hashedPassword">The hashed password string from the database.</param>
    /// <returns>True if the password is correct; otherwise, false.</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // 1. Split the stored string to get the salt and the original hash.
        var parts = hashedPassword.Split(Delimiter);
        if (parts.Length != 2)
        {
            // The stored hash is not in the expected format.
            // This could indicate data corruption or an old hash format.
            return false;
        }

        // 2. Decode the salt and hash from Base64.
        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        // 3. Hash the incoming plain text password using the *same* salt and parameters.
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt, // Use the original salt from the database.
            Iterations,
            _hashAlgorithm,
            KeySize);

        // 4. Compare the two hashes in a way that prevents timing attacks.
        // CryptographicOperations.FixedTimeEquals is specifically designed for this.
        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }
}