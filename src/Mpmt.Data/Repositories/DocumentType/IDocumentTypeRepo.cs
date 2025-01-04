using Mpmt.Core.Dtos.DocumentType;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.DocumentType
{
    /// <summary>
    /// The document type repo.
    /// </summary>
    public interface IDocumentTypeRepo
    {
        /// <summary>
        /// Gets the document type async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<DocumentTypeDetails>> GetDocumentTypeAsync();
        /// <summary>
        /// Gets the document type by id async.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        Task<DocumentTypeDetails> GetDocumentTypeByIdAsync(int documentTypeId);
        /// <summary>
        /// Adds the document type async.
        /// </summary>
        /// <param name="addDocumentType">The add document type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddDocumentTypeAsync(IUDDocumentType addDocumentType);
        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateDocumentTypeAsync(IUDDocumentType updateDocumentType);
        /// <summary>
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveDocumentTypeAsync(IUDDocumentType removeDocumentType);
    }
}
