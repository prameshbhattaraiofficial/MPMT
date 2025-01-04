using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Mpmt.Core.Common
{
    /// <summary>
    /// The hash utils.
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        /// Hashes the hmac sha512.
        /// </summary>
        /// <param name="inputBytes">The input bytes.</param>
        /// <param name="secretKeyBytes">The secret key bytes.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] HashHmacSha512(byte[] inputBytes, byte[] secretKeyBytes)
        {
            using var hmacSha512 = new HMACSHA512(secretKeyBytes);
            return hmacSha512.ComputeHash(inputBytes);
        }

        /// <summary>
        /// Hashes the hmac sha512.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] HashHmacSha512(string text, string secretKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var inputBytes = Encoding.UTF8.GetBytes(text);

            return HashHmacSha512(inputBytes, secretKeyBytes);
        }

        /// <summary>
        /// Hashes the hmac sha512.
        /// </summary>
        /// <param name="secretKeyBytes">The secret key bytes.</param>
        /// <param name="model">The model.</param>
        /// <param name="skipKeys">The skip keys.</param>
        /// <returns>An array of byte.</returns>
        public static byte[] HashHmacSha512<T>(byte[] secretKeyBytes, T model, params string[] skipKeys)
        {
            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name);
            var concatenatedData = properties.Aggregate(string.Empty, (acc, property) =>
            {
                if (skipKeys.Any(sk => property.Name.Equals(sk, StringComparison.OrdinalIgnoreCase)))
                    return acc;

                var value = property.GetValue(model)?.ToString();
                acc += value ?? string.Empty;

                return acc;
            });

            return HashHmacSha512(secretKeyBytes, Encoding.UTF8.GetBytes(concatenatedData));
        }

        /// <summary>
        /// Hashes the hmac sha512 to hex lower.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>A string.</returns>
        public static string HashHmacSha512ToHexLower(string text, string secretKey)
        {
            var hashValue = HashHmacSha512(text, secretKey);

            return Convert.ToHexString(hashValue).ToLower();
        }

        /// <summary>
        /// Hashes the hmac sha512 to hex lower.
        /// </summary>
        /// <param name="secretKeyBytes">The secret key bytes.</param>
        /// <param name="model">The model.</param>
        /// <param name="skipKeys">The skip keys.</param>
        /// <returns>A string.</returns>
        public static string HashHmacSha512ToHexLower<T>(byte[] secretKeyBytes, T model, params string[] skipKeys)
        {
            var hashBytes = HashHmacSha512(secretKeyBytes, model, skipKeys);
            return Convert.ToHexString(hashBytes).ToLower();
        }

        /// <summary>
        /// Hashes the hmac sha512 to base64.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>A string.</returns>
        public static string HashHmacSha512ToBase64(string text, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));

            var hashValue = HashHmacSha512(text, secretKey);

            return Convert.ToBase64String(hashValue);
        }

        /// <summary>
        /// Checks the equal base64 hash hmac sha512.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="base64Hash">The base64 hash.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>A bool.</returns>
        public static bool CheckEqualBase64HashHmacSha512(string text, string base64Hash, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(base64Hash)) throw new ArgumentNullException(nameof(base64Hash));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));

            var hashComputed = HashHmacSha512(Encoding.UTF8.GetBytes(text), Encoding.UTF8.GetBytes(secretKey));

            return Convert.ToBase64String(hashComputed).Equals(base64Hash);
        }
        /// <summary>
        /// Checks the equal hex lower hash hmac sha512.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="hexHash">The hex hash.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>A bool.</returns>
        public static bool CheckEqualHexLowerHashHmacSha512(string text, string hexHash, string secretKey)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(hexHash)) throw new ArgumentNullException(nameof(hexHash));
            if (string.IsNullOrWhiteSpace(secretKey)) throw new ArgumentNullException(nameof(secretKey));

            var hashComputedHex = HashHmacSha512ToHexLower(text, secretKey);

            return hashComputedHex.Equals(hexHash);
        }

        /// <summary>
        /// Checks the equal hex lower hash hmac sha512.
        /// </summary>
        /// <param name="secretKeyBytes">The secret key bytes.</param>
        /// <param name="model">The model.</param>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="skipKeys">The skip keys.</param>
        /// <returns>A bool.</returns>
        public static bool CheckEqualHexLowerHashHmacSha512<T>(byte[] secretKeyBytes, T model, string hashKey, params string[] skipKeys)
        {
            var hashProperty = model.GetType().GetProperty(hashKey, BindingFlags.Public | BindingFlags.Instance);
            if (hashProperty is null)
                return false;

            var hash = hashProperty.GetValue(model)?.ToString();
            if (string.IsNullOrWhiteSpace(hash))
                return false;

            var skipKeysAll = new List<string> { hashKey };
            skipKeysAll.AddRange(skipKeys);

            var computedHmac = HashHmacSha512ToHexLower(secretKeyBytes, model, skipKeysAll.ToArray());

            return computedHmac.Equals(hash.ToLower());
        }

        /// <summary>
        /// Checks the equal hex lower hash hmac sha512.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="model">The model.</param>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="skipKeys">The skip keys.</param>
        /// <returns>A bool.</returns>
        public static bool CheckEqualHexLowerHashHmacSha512<T>(string secretKey, T model, string hashKey, params string[] skipKeys)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException(nameof(secretKey));

            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (hashKey is null)
                throw new ArgumentNullException(nameof(hashKey));

            return CheckEqualHexLowerHashHmacSha512(Encoding.UTF8.GetBytes(secretKey), model, hashKey, skipKeys);
        }

    }
}
