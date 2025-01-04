namespace Mpmt.Core.Dtos.PrefundRequest
{
    /// <summary>
    /// The fund request status update.
    /// </summary>
    public class FundRequestStatusUpdate
    {
        /// <summary>
        /// Gets or sets the fund request id.
        /// </summary>
        public int FundRequestId { get; set; }
        /// <summary>
        /// Gets or sets the operation mode.
        /// </summary>
        public string OperationMode { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        public string Remarks { get; set; }
    }
}
