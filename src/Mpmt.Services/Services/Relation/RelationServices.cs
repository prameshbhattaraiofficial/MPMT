using AutoMapper;
using Mpmt.Core.Dtos.Relation;
using Mpmt.Core.ViewModel.Relation;
using Mpmt.Data.Repositories.Relation;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Relation
{
    /// <summary>
    /// The relation services.
    /// </summary>
    public class RelationServices : BaseService, IRelationServices
    {
        private readonly IRelationRepo _relationRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationServices"/> class.
        /// </summary>
        /// <param name="relationRepo">The relation repo.</param>
        /// <param name="mapper">The mapper.</param>
        public RelationServices(IRelationRepo relationRepo, IMapper mapper)
        {
            _relationRepo = relationRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the relation async.
        /// </summary>
        /// <param name="addRelation">The add relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRelationAsync(AddRelationVm addRelation)
        {
            var mappedData = _mapper.Map<IUDRelation>(addRelation);
            var response = await _relationRepo.AddRelationAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Gets the relation async.
        /// </summary>
        /// <param name="relationFilter">The relation filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RelationDetails>> GetRelationAsync(RelationFilter relationFilter)
        {
            var response = await _relationRepo.GetRelationAsync(relationFilter);
            return response;
        }

        /// <summary>
        /// Gets the relation by id async.
        /// </summary>
        /// <param name="relationId">The relation id.</param>
        /// <returns>A Task.</returns>
        public async Task<RelationDetails> GetRelationByIdAsync(int relationId)
        {
            var response = await _relationRepo.GetRelationByIdAsync(relationId);
            return response;
        }

        /// <summary>
        /// Removes the relation async.
        /// </summary>
        /// <param name="removeRelation">The remove relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveRelationAsync(UpdateRelationVm removeRelation)
        {
            var mappedData = _mapper.Map<IUDRelation>(removeRelation);
            var response = await _relationRepo.RemoveRelationAsync(mappedData);
            return response;
        }

        /// <summary>
        /// Updates the relation async.
        /// </summary>
        /// <param name="updateRelation">The update relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRelationAsync(UpdateRelationVm updateRelation)
        {
            var mappedData = _mapper.Map<IUDRelation>(updateRelation);
            var response = await _relationRepo.UpdateRelationAsync(mappedData);
            return response;
        }
    }
}
