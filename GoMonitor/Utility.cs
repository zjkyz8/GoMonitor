using System;
using System.Security.Cryptography;

public static class Utility
{
    public static string CalculateContentMD5(byte[] content)
    {
        var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
        var computeHash = md5CryptoServiceProvider.ComputeHash(content);
        return BitConverter.ToString(computeHash).Replace("-", string.Empty);
        
    }
}