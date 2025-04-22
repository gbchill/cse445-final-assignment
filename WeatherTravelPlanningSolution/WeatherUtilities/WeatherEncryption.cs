using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeatherUtilities
{
    public class WeatherEncryption
    {
        // In a real application, this key should be stored securely, not hardcoded
        private readonly string encryptionKey = "Weather12345Key!";

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    // Generate a key from our password
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    aes.Key = key.GetBytes(32);  // 256-bit key
                    aes.IV = key.GetBytes(16);   // 128-bit IV

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            cs.Write(plainBytes, 0, plainBytes.Length);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real application, proper error handling should be implemented
                return "Error: " + ex.Message;
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    // Generate the same key from our password
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    aes.Key = key.GetBytes(32);  // 256-bit key
                    aes.IV = key.GetBytes(16);   // 128-bit IV

                    byte[] cipherBytes = Convert.FromBase64String(cipherText);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                        }

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real application, proper error handling should be implemented
                return "Error: " + ex.Message;
            }
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}