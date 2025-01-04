namespace Mpmt.Core.ViewModel.DocumentType
{
    /// <summary>
    /// The add document type vm.
    /// </summary>
    public class AddDocumentTypeVm
    {
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
    }
}
