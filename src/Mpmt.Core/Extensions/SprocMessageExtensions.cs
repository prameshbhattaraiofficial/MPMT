using Mpmt.Core.Dtos;
using Mpmts.Core.Dtos;

namespace Mpmt.Core.Extensions
{
    /// <summary>
    /// The sproc message extensions.
    /// </summary>
    public static class SprocMessageExtensions
    {
        /// <summary>
        /// Maps the to mpmt result.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A MpmtResult.</returns>
        public static MpmtResult MapToMpmtResult(this SprocMessage message)
        {
            var result = new MpmtResult();

            if (message.StatusCode != 200)
            {
                result.AddError(message.StatusCode, message.MsgText);
                return result;
            }

            result.AddSuccess(message.StatusCode, message.MsgText);
            return result;
        }
    }
}
