using System.Security.Cryptography;

namespace Hellclient.World.Utils;

public class AesUtil
{
    public const int IVSize=16;
    public static string Encrypt(string text, string key)
    {
        var iv = new byte[IVSize]; 
        RandomNumberGenerator.Fill(iv);
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream();
        ms.Write(iv, 0, iv.Length); // Prepend IV to the ciphertext
        using (var cs = new System.Security.Cryptography.CryptoStream(ms, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
        using (var sw = new System.IO.StreamWriter(cs))
        {
            sw.Write(text);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText, string key)
    {
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
        aes.IV = new byte[IVSize]; // Initialize IV with the correct size
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new System.IO.MemoryStream(Convert.FromBase64String(cipherText));
        ms.Read(aes.IV, 0, aes.IV.Length); // Read the IV from the beginning of the ciphertext
        using var cs = new System.Security.Cryptography.CryptoStream(ms, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);
        using var sr = new System.IO.StreamReader(cs);
        return sr.ReadToEnd();
    }
}