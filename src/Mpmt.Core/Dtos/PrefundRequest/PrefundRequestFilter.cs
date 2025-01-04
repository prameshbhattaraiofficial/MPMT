using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.PrefundRequest
{
    /// <summary>
    /// The prefund request filter.
    /// </summary>
    public class PrefundRequestFilter : PagedRequest
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the txn id.
        /// </summary>
        public string TxnId { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets the export.
        /// </summary>
        public int Export { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
