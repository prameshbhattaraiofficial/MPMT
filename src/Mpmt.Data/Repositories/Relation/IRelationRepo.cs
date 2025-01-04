using Mpmt.Core.Dtos.Relation;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Relation
{
    /// <summary>
    /// The relation repo.
    /// </summary>
    public interface IRelationRepo
    {
        /// <summary>
        /// Gets the relation async.
        /// </summary>
        /// <param name="relationFilter">The relation filter.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<RelationDetails>> GetRelationAsync(RelationFilter relationFilter);
        /// <summary>
        /// Gets the relation by id async.
        /// </summary>
        /// <param name="relationId">The relation id.</param>
        /// <returns>A Task.</returns>
        Task<RelationDetails> GetRelationByIdAsync(int relationId);
        /// <summary>
        /// Adds the relation async.
        /// </summary>
        /// <param name="addRelation">The add relation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddRelationAsync(IUDRelation addRelation);
        /// <summary>
        /// Updates the relation async.
        /// </summary>
        /// <param name="updateRelation">The update relation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateRelationAsync(IUDRelation updateRelation);
        /// <summary>
        /// Removes the relation async.
        /// </summary>
        /// <param name="removeRelation">The remove relation.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveRelationAsync(IUDRelation removeRelation);
    }
}
