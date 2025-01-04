using Dapper;
using Mpmt.Core.Dtos.FundType;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.FundType
{
    /// <summary>
    /// The fund type repo.
    /// </summary>
    public class FundTypeRepo : IFundTypeRepo
    {
        /// <summary>
        /// Adds the fund type async.
        /// </summary>
        /// <param name="addFundType">The add fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddFundTypeAsync(IUDFundType addFundType)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addFundType.Id);
                param.Add("@FundType", addFundType.FundType);
                param.Add("@FundTypeCode", addFundType.FundTypeCode);
                param.Add("@IsActive", addFundType.IsActive);
                param.Add("@Remarks", addFundType.Remarks);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_fund_type]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the fund type async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<FundTypeDetails>> GetFundTypeAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<FundTypeDetails>("[dbo].[usp_get_FundType]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the fund type by id async.
        /// </summary>
        /// <param name="fundTypeId">The fund type id.</param>
        /// <returns>A Task.</returns>
        public async Task<FundTypeDetails> GetFundTypeByIdAsync(int fundTypeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", fundTypeId);
            return await connection.QueryFirstOrDefaultAsync<FundTypeDetails>("[dbo].[usp_get_FundType_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the fund type async.
        /// </summary>
        /// <param name="removeFundType">The remove fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveFundTypeAsync(IUDFundType removeFundType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "D");
            param.Add("Id", removeFundType.Id);
            param.Add("@FundType", removeFundType.FundType);
            param.Add("@FundTypeCode", removeFundType.FundTypeCode);
            param.Add("@IsActive", removeFundType.IsActive);
            param.Add("@Remarks", removeFundType.Remarks);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_fund_type]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the fund type async.
        /// </summary>
        /// <param name="updateFundType">The update fund type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateFundTypeAsync(IUDFundType updateFundType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "U");
            param.Add("Id", updateFundType.Id);
            param.Add("@FundType", updateFundType.FundType);
            param.Add("@FundTypeCode", updateFundType.FundTypeCode);
            param.Add("@IsActive", updateFundType.IsActive);
            param.Add("@Remarks", updateFundType.Remarks);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_fund_type]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
