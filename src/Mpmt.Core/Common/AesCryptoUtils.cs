using System.Security.Cryptography;

namespace Mpmt.Core.Common
{
    /// <summary>
    /// The aes crypto utils.
    /// </summary>
    public static class AesCryptoUtils
    {
        /// <summary>
        /// Generates the key and iv.
        /// </summary>
        /// <param name="keyBytesCount">The key bytes count.</param>
        /// <param name="ivBytesCount">The iv bytes count.</param>
        /// <returns>A (byte[] key, byte[] iv) .</returns>
        public static (byte[] key, byte[] iv) GenerateKeyAndIv(int keyBytesCount = 32, int ivBytesCount = 16)
        {
            using var rng = RandomNumberGenerator.Create();

            var keyBytes = new byte[keyBytesCount];
            var ivBytes = new byte[ivBytesCount];

            rng.GetBytes(keyBytes);
            rng.GetBytes(ivBytes);

            return (keyBytes, ivBytes);
        }

        /// <summary>
        /// Encrypts the.
        /// </summary>
        /// <param name="plainTextBytes">The plain text bytes.</param>
        /// <param name="keyBytes">The key bytes.</param>
        /// <param name="ivBytes">The iv bytes.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] Encrypt(byte[] plainTextBytes, byte[] keyBytes, byte[] ivBytes)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            var encryptor = aes.CreateEncryptor();

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(plainTextBytes, 0, plainTextBytes.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        /// <summary>
        /// Decrypts the.
        /// </summary>
        /// <param name="cipherTextBytes">The cipher text bytes.</param>
        /// <param name="keyBytes">The key bytes.</param>
        /// <param name="ivBytes">The iv bytes.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] Decrypt(byte[] cipherTextBytes, byte[] keyBytes, byte[] ivBytes)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            var decryptor = aes.CreateDecryptor();

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
            cs.Write(cipherTextBytes, 0, cipherTextBytes.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}
