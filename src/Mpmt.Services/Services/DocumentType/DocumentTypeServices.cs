using AutoMapper;
using Mpmt.Core.Dtos.DocumentType;
using Mpmt.Core.ViewModel.DocumentType;
using Mpmt.Data.Repositories.DocumentType;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.DocumentType
{
    /// <summary>
    /// The document type services.
    /// </summary>
    public class DocumentTypeServices : BaseService, IDocumentTypeServices
    {
        private readonly IDocumentTypeRepo _documentTypeRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeServices"/> class.
        /// </summary>
        /// <param name="documentTypeRepo">The document type repo.</param>
        /// <param name="mapper">The mapper.</param>
        public DocumentTypeServices(IDocumentTypeRepo documentTypeRepo, IMapper mapper)
        {
            _documentTypeRepo = documentTypeRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the document type async.
        /// </summary>
        /// <param name="addDocumentType">The add document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddDocumentTypeAsync(AddDocumentTypeVm addDocumentType)
        {
            var mappedData = _mapper.Map<IUDDocumentType>(addDocumentType);
            var response = await _documentTypeRepo.AddDocumentTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the document type async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<DocumentTypeDetails>> GetDocumentTypeAsync()
        {
            var response = await _documentTypeRepo.GetDocumentTypeAsync();
            return response;
        }

        /// <summary>
        /// Gets the document type by id async.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        public async Task<DocumentTypeDetails> GetDocumentTypeByIdAsync(int documentTypeId)
        {
            var response = await _documentTypeRepo.GetDocumentTypeByIdAsync(documentTypeId);
            return response;
        }

        /// <summary>
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveDocumentTypeAsync(UpdateDocumentTypeVm removeDocumentType)
        {
            var mappedData = _mapper.Map<IUDDocumentType>(removeDocumentType);
            var response = await _documentTypeRepo.RemoveDocumentTypeAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateDocumentTypeAsync(UpdateDocumentTypeVm updateDocumentType)
        {
            var mappedData = _mapper.Map<IUDDocumentType>(updateDocumentType);
            var response = await _documentTypeRepo.UpdateDocumentTypeAsync(mappedData);
            return response;
        }
    }
}
