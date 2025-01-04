using System.Security.Cryptography;

namespace Mpmt.Core.Common
{
    /// <summary>
    /// The password utils.
    /// </summary>
    public static class PasswordUtils
    {
        /// <summary>
        /// The uppercase chars.
        /// </summary>
        private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// The lowercase chars.
        /// </summary>
        private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// The numeric chars.
        /// </summary>
        private const string NumericChars = "0123456789";
        // private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        /// <summary>
        /// The special chars.
        /// </summary>
        private const string SpecialChars = "!@#$%^&*()+[]{}|<>?";

        private static readonly RandomNumberGenerator RandomGenerator = RandomNumberGenerator.Create();

        /// <summary>
        /// Generates random password of given length containing uppercase, lowerchase, numeric and special characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GeneratePassword(int length)
        {
            char[] password = new char[length];
            int charIndex = 0;

            // Generate at least one character from each character set
            password[charIndex++] = GetRandomChar(UppercaseChars);
            password[charIndex++] = GetRandomChar(LowercaseChars);
            password[charIndex++] = GetRandomChar(NumericChars);
            password[charIndex++] = GetRandomChar(SpecialChars);

            // Fill the remaining characters randomly
            for (; charIndex < length; charIndex++)
            {
                string charSet = GetRandomCharSet();
                password[charIndex] = GetRandomChar(charSet);
            }

            // Shuffle the password to ensure randomness
            Shuffle(password);

            return new string(password);
        }

        /// <summary>
        /// Generates random base64 encoded key of specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateBase64Key(int length = 64)
        {
            return Convert.ToBase64String(GenerateKey(length));
        }

        /// <summary>
        /// Generates key of random bytes array of specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateKey(int length = 64)
        {
            var bytesBuffer = new byte[length];

            RandomGenerator.GetBytes(bytesBuffer);

            return bytesBuffer;
        }

        /// <summary>
        /// Gets the random char.
        /// </summary>
        /// <param name="charSet">The char set.</param>
        /// <returns>A char.</returns>
        private static char GetRandomChar(string charSet)
        {
            byte[] randomBytes = new byte[1];
            RandomGenerator.GetBytes(randomBytes);
            int randomIndex = randomBytes[0] % charSet.Length;
            return charSet[randomIndex];
        }

        /// <summary>
        /// Gets the random char set.
        /// </summary>
        /// <returns>A string.</returns>
        private static string GetRandomCharSet()
        {
            string[] charSets = { UppercaseChars, LowercaseChars, NumericChars, SpecialChars };
            return charSets.OrderBy(_ => Guid.NewGuid()).First();
        }

        /// <summary>
        /// Shuffles the.
        /// </summary>
        /// <param name="array">The array.</param>
        private static void Shuffle<T>(T[] array)
        {
            var n = array.Length;
            for (var i = 0; i < n; i++)
            {
                var randomIndex = i + RandomInt(n - i);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }

        /// <summary>
        /// Randoms the int.
        /// </summary>
        /// <param name="exclusiveMax">The exclusive max.</param>
        /// <returns>An int.</returns>
        private static int RandomInt(int exclusiveMax)
        {
            byte[] randomBytes = new byte[4];
            RandomGenerator.GetBytes(randomBytes);
            uint randomInt = BitConverter.ToUInt32(randomBytes, 0);
            return (int)(randomInt % exclusiveMax);
        }
    }
}
