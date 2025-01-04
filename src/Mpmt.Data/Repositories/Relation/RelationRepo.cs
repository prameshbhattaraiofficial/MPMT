using Dapper;
using Mpmt.Core.Dtos.Relation;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Relation
{
    /// <summary>
    /// The relation repo.
    /// </summary>
    public class RelationRepo : IRelationRepo
    {
        /// <summary>
        /// Adds the relation async.
        /// </summary>
        /// <param name="addRelation">The add relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRelationAsync(IUDRelation addRelation)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", 'I');
                param.Add("@Id", addRelation.Id);
                param.Add("@RelationName", addRelation.RelationName);
                param.Add("@Description", addRelation.Description);
                param.Add("@IsActive", addRelation.IsActive);
                param.Add("@IsDeleted", addRelation.IsDeleted);
                param.Add("@CreatedById", addRelation.CreatedById);
                param.Add("@CreatedByName", addRelation.CreatedByName);
                param.Add("@UpdatedById", addRelation.UpdatedById);
                param.Add("@UpdatedByName", addRelation.UpdatedByName);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_relation]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@IdentityVal");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the relation async.
        /// </summary>
        /// <param name="relationFilter">The relation filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RelationDetails>> GetRelationAsync(RelationFilter relationFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@RelationName", relationFilter.RelationName);
            param.Add("@Status", relationFilter.Status);
            return await connection.QueryAsync<RelationDetails>("[dbo].[usp_get_relation]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the relation by id async.
        /// </summary>
        /// <param name="relationId">The relation id.</param>
        /// <returns>A Task.</returns>
        public async Task<RelationDetails> GetRelationByIdAsync(int relationId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", relationId);

            return await connection.QueryFirstOrDefaultAsync<RelationDetails>("[dbo].[usp_get_relation_byid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the relation async.
        /// </summary>
        /// <param name="removeRelation">The remove relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveRelationAsync(IUDRelation removeRelation)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", removeRelation.Id);
            param.Add("@RelationName", removeRelation.RelationName);
            param.Add("@Description", removeRelation.Description);
            param.Add("@IsActive", removeRelation.IsActive);
            param.Add("@IsDeleted", removeRelation.IsDeleted);
            param.Add("@CreatedById", removeRelation.CreatedById);
            param.Add("@CreatedByName", removeRelation.CreatedByName);
            param.Add("@UpdatedById", removeRelation.UpdatedById);
            param.Add("@UpdatedByName", removeRelation.UpdatedByName);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_relation]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the relation async.
        /// </summary>
        /// <param name="updateRelation">The update relation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRelationAsync(IUDRelation updateRelation)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'U');
            param.Add("@Id", updateRelation.Id);
            param.Add("@RelationName", updateRelation.RelationName);
            param.Add("@Description", updateRelation.Description);
            param.Add("@IsActive", updateRelation.IsActive);
            param.Add("@IsDeleted", updateRelation.IsDeleted);
            param.Add("@CreatedById", updateRelation.CreatedById);
            param.Add("@CreatedByName", updateRelation.CreatedByName);
            param.Add("@UpdatedById", updateRelation.UpdatedById);
            param.Add("@UpdatedByName", updateRelation.UpdatedByName);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_relation]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
