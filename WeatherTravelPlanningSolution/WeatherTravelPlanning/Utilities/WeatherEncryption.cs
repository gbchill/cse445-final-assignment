using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeatherTravelPlanning.Utilities
{
    public class WeatherEncryption
    {

        private readonly string encryptionKey = "Weather12345Key!";

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    var keyGen = new Rfc2898DeriveBytes(
                        encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e,
                                     0x20, 0x4d, 0x65, 0x64,
                                     0x76, 0x65, 0x64, 0x65,
                                     0x76 });

                    aes.Key = keyGen.GetBytes(32);  // 256‑bit key
                    aes.IV = keyGen.GetBytes(16);  // 128‑bit IV

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle/log in real code
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
                    var keyGen = new Rfc2898DeriveBytes(
                        encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e,
                                     0x20, 0x4d, 0x65, 0x64,
                                     0x76, 0x65, 0x64, 0x65,
                                     0x76 });

                    aes.Key = keyGen.GetBytes(32);
                    aes.IV = keyGen.GetBytes(16);

                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hashed);
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
