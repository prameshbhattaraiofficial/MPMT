namespace Mpmt.Core.Dtos.PartnerAction
{
    public class IUDPartnerAction
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created nepali date.
        /// </summary>
        public string CreatedNepaliDate { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the updated nepali date.
        /// </summary>
        public string UpdatedNepaliDate { get; set; }
    }
}
