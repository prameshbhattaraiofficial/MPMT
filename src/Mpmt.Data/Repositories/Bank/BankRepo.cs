using Dapper;
using Mpmt.Core.Dtos.Banks;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Bank
{
    /// <summary>
    /// The bank repo.
    /// </summary>
    public class BankRepo : IBankRepo
    {
        /// <summary>
        /// Adds the bank async.
        /// </summary>
        /// <param name="addbank">The addbank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddBankAsync(IUDBank addbank)
        {
            try
            {

                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", 'I');
                param.Add("@Id", addbank.Id);
                param.Add("@BankName", addbank.BankName);
                param.Add("@BankCode", addbank.BankCode);
                param.Add("@BranchCode", addbank.BranchCode);
                param.Add("@CountryCode", addbank.CountryCode);
                param.Add("@IsActive", addbank.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Banks]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the bank async.
        /// </summary>
        /// <param name="bankFilter">The bank filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<BankDetails>> GetBankAsync(BankFilter bankFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@BankName", bankFilter.BankName);
            param.Add("@countryCode", bankFilter.CountryCode);
            param.Add("@Status", bankFilter.Status);
            return await connection.QueryAsync<BankDetails>("[dbo].[usp_get_Banks]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the bank by id async.
        /// </summary>
        /// <param name="BankId">The bank id.</param>
        /// <returns>A Task.</returns>
        public async Task<BankDetails> GetBankByIdAsync(int BankId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", BankId);

            return await connection.QueryFirstOrDefaultAsync<BankDetails>("[dbo].[usp_get_Banks_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the bank async.
        /// </summary>
        /// <param name="Removebank">The removebank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveBankAsync(IUDBank Removebank)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", Removebank.Id);
            param.Add("@BankName", Removebank.BankName);
            param.Add("@BankCode", Removebank.BankCode);
            param.Add("@BranchCode", Removebank.BranchCode);
            param.Add("@CountryCode", Removebank.CountryCode);
            param.Add("@IsActive", Removebank.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Banks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the bank async.
        /// </summary>
        /// <param name="Updatebank">The updatebank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateBankAsync(IUDBank Updatebank)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'U');
            param.Add("@Id", Updatebank.Id);
            param.Add("@BankName", Updatebank.BankName);
            param.Add("@BankCode", Updatebank.BankCode);
            param.Add("@BranchCode", Updatebank.BranchCode);
            param.Add("@CountryCode", Updatebank.CountryCode);
            param.Add("@IsActive", Updatebank.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Banks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
