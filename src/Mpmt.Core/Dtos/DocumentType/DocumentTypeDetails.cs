namespace Mpmt.Core.Dtos.DocumentType
{
    /// <summary>
    /// The document type details.
    /// </summary>
    public class DocumentTypeDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public string DocumentType { get; set; }
        /// <summary>
        /// Gets or sets the document type code.
        /// </summary>
        public string DocumentTypeCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is expirable.
        /// </summary>
        public bool IsExpirable { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
