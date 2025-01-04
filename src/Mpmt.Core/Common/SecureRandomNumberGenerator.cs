using System.Security.Cryptography;

namespace Mpmt.Core.Common
{
    /// <summary>
    ///  Represents the class implementation of cryptographic random number generator derive
    /// </summary>
    public class SecureRandomNumberGenerator : RandomNumberGenerator
    {
        #region Field

        private bool _disposed = false;
        private readonly RandomNumberGenerator _rng;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureRandomNumberGenerator"/> class.
        /// </summary>
        public SecureRandomNumberGenerator()
        {
            _rng = Create();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Nexts the.
        /// </summary>
        /// <returns>An int.</returns>
        public int Next()
        {
            var data = new byte[sizeof(int)];
            _rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & int.MaxValue - 1;
        }

        /// <summary>
        /// Nexts the.
        /// </summary>
        /// <param name="maxValue">The max value.</param>
        /// <returns>An int.</returns>
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        /// <summary>
        /// Nexts the.
        /// </summary>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        /// <returns>An int.</returns>
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            return (int)Math.Floor(minValue + ((double)maxValue - minValue) * NextDouble());
        }

        /// <summary>
        /// Nexts the double.
        /// </summary>
        /// <returns>A double.</returns>
        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            _rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void GetBytes(byte[] data)
        {
            _rng.GetBytes(data);
        }

        /// <summary>
        /// Gets the non zero bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        public override void GetNonZeroBytes(byte[] data)
        {
            _rng.GetNonZeroBytes(data);
        }

        /// <summary>
        /// Dispose secure random
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        /// <summary>
        /// Disposes the.
        /// </summary>
        /// <param name="disposing">If true, disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _rng?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
