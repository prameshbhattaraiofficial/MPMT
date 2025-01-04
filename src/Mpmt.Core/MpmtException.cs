using System.Runtime.Serialization;

namespace Mpmt.Core
{
    /// <summary>
    /// The mpmt exception.
    /// </summary>
    [Serializable]
    public class MpmtException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpmtException"/> class.
        /// </summary>
        public MpmtException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpmtException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MpmtException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpmtException"/> class.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The args.</param>
        public MpmtException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpmtException"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public MpmtException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpmtException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MpmtException(string message, Exception innerException) : base(message, innerException) { }
    }
}
