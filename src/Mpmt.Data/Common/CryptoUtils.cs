using System.Security.Cryptography;
using System.Text;

namespace Mpmt.Data.Common
{
    /// <summary>
    /// The crypto utils.
    /// </summary>
    public class CryptoUtils
    {
        /// <summary>
        /// Hashes the hmac sha512.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] HashHmacSha512(string text, string secretKey)
        {
            var secretBytes = Encoding.UTF8.GetBytes(secretKey);
            var inputBytes = Encoding.UTF8.GetBytes(text);

            using var hmacSha512 = new HMACSHA512(secretBytes);
            return hmacSha512.ComputeHash(inputBytes);
        }

        /// <summary>
        /// Hashes the hmac sha512 base64.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>A string.</returns>
        public static string HashHmacSha512Base64(string text, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));

            var hashValue = HashHmacSha512(text, secretKey);

            return Convert.ToBase64String(hashValue);
        }

        /// <summary>
        /// Generates the key salt.
        /// </summary>
        /// <param name="saltLength">The salt length.</param>
        /// <returns>A string.</returns>
        public static string GenerateKeySalt(int saltLength = 128)
        {
            byte[] bytesBuffer = new byte[saltLength * 2];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytesBuffer);
            }

            return Convert.ToBase64String(bytesBuffer)[..saltLength];
        }
    }
}
