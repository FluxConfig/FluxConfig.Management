using System.Security.Cryptography;

namespace FluxConfig.Management.Domain.Hasher;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    
    public static string Hash(string password, int iterations = 10000)
    {
        byte[] salt = new byte[SaltSize];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);
        
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
        
        string passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;
    }

    public static bool Verify(string password, string hashedPassword, int iterations = 10000)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);
        
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        for (int i = 0; i < HashSize; ++i)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}