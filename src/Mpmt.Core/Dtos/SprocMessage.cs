namespace Mpmts.Core.Dtos
{
    /// <summary>
    /// The sproc message.
    /// </summary>
    public class SprocMessage
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the msg type.
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// Gets or sets the msg text.
        /// </summary>
        public string MsgText { get; set; }
        /// <summary>
        /// Gets or sets the identity val.
        /// </summary>
        public int? IdentityVal { get; set; }

        public void Deconstruct(out object data, out object status)
        {
            throw new NotImplementedException();
        }
    }
}
