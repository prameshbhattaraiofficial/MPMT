using Dapper;
using Mpmt.Core.Dtos.Occupation;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Occupation
{
    /// <summary>
    /// The occupation repo.
    /// </summary>
    public class OccupationRepo : IOccupationRepo
    {
        /// <summary>
        /// Adds the occupation async.
        /// </summary>
        /// <param name="addOccupation">The add occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddOccupationAsync(IUDOccupation addOccupation)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", 'I');
                param.Add("@Id", addOccupation.Id);
                param.Add("@OccupationName", addOccupation.OccupationName);
                param.Add("@Description", addOccupation.Description);
                param.Add("@IsActive", addOccupation.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_occupation]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the occupation async.
        /// </summary>
        /// <param name="occupationFilter">The occupation filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<OccupationDetails>> GetOccupationAsync(OccupationFilter occupationFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@OccupationName", occupationFilter.OccupationName);
            param.Add("@Status", occupationFilter.Status);
            return await connection.QueryAsync<OccupationDetails>("[dbo].[usp_get_Occupation]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the occupation by id async.
        /// </summary>
        /// <param name="occupationId">The occupation id.</param>
        /// <returns>A Task.</returns>
        public async Task<OccupationDetails> GetOccupationByIdAsync(int occupationId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", occupationId);

            return await connection.QueryFirstOrDefaultAsync<OccupationDetails>("[dbo].[usp_get_Occupation_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the occupation async.
        /// </summary>
        /// <param name="removeOccupation">The remove occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveOccupationAsync(IUDOccupation removeOccupation)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", removeOccupation.Id);
            param.Add("@OccupationName", removeOccupation.OccupationName);
            param.Add("@Description", removeOccupation.Description);
            param.Add("@IsActive", removeOccupation.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_occupation]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the occupation async.
        /// </summary>
        /// <param name="updateOccupation">The update occupation.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateOccupationAsync(IUDOccupation updateOccupation)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'U');
            param.Add("@Id", updateOccupation.Id);
            param.Add("@OccupationName", updateOccupation.OccupationName);
            param.Add("@Description", updateOccupation.Description);
            param.Add("@IsActive", updateOccupation.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_occupation]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
