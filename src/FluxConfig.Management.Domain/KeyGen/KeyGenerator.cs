using System.Security.Cryptography;

namespace FluxConfig.Management.Domain.KeyGen;

internal static class KeyGenerator
{
    internal static string GenerateKey(int length = 64)
    {
        var byteLength = (int)Math.Ceiling(length * 3 / 4.0);

        byte[] bytes = new byte[byteLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        string base64 = Convert.ToBase64String(bytes);
        string base64Url = base64
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        return base64Url[..length];
    }
    
    public static string GenerateCompositeKey(int randomPartLength = 32)
    {
        string guidPart = Guid.NewGuid().ToString("N");
        string randomPart = GenerateKey(randomPartLength);
    
        return $"{guidPart}{randomPart}";
    }
}