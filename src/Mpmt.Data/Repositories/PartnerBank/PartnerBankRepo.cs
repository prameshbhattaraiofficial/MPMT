using Dapper;
using Mpmt.Core.Dtos.PartnerBank;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerBank
{
    /// <summary>
    /// The partner bank repo.
    /// </summary>
    public class PartnerBankRepo : IPartnerBankRepo
    {
        /// <summary>
        /// Adds the partner bank async.
        /// </summary>
        /// <param name="addPartnerBank">The add partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddPartnerBankAsync(IUDPartnerBank addPartnerBank)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addPartnerBank.Id);
                param.Add("@PartnerCode", addPartnerBank.PartnerCode);
                param.Add("@BankCode", addPartnerBank.BankCode);
                param.Add("@AccountNumber", addPartnerBank.AccountNumber);
                param.Add("@AccountName", addPartnerBank.AccountName);
                param.Add("@IsActive", addPartnerBank.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_banks]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the partner bank async.
        /// </summary>
        /// <param name="partnerBankFilter">The partner bank filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerBankDetails>> GetPartnerBankAsync(PartnerBankFilter partnerBankFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerBankFilter.PartnerCode);
            param.Add("@BankCode", partnerBankFilter.BankCode);
            param.Add("@AccountNumber", partnerBankFilter.AccountNumber);
            param.Add("@AccountName", partnerBankFilter.AccountName);
            param.Add("@Status", partnerBankFilter.Status);
            return await connection.QueryAsync<PartnerBankDetails>("[dbo].[usp_get_patnerbank]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the partner bank by partner id async.
        /// </summary>
        /// <param name="partnerId">The partner id.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerBankDetails> GetPartnerBankByPartnerIdAsync(int partnerId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", partnerId);
            return await connection.QueryFirstOrDefaultAsync<PartnerBankDetails>("[dbo].[usp_get_patnerbank_byId]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the partner bank async.
        /// </summary>
        /// <param name="removePartnerBank">The remove partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemovePartnerBankAsync(IUDPartnerBank removePartnerBank)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");
            param.Add("Id", removePartnerBank.Id);
            param.Add("@PartnerCode", removePartnerBank.PartnerCode);
            param.Add("@BankCode", removePartnerBank.BankCode);
            param.Add("@AccountNumber", removePartnerBank.AccountNumber);
            param.Add("@AccountName", removePartnerBank.AccountName);
            param.Add("@IsActive", removePartnerBank.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_banks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the partner bank async.
        /// </summary>
        /// <param name="updatePartnerBank">The update partner bank.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdatePartnerBankAsync(IUDPartnerBank updatePartnerBank)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", updatePartnerBank.Id);
            param.Add("@PartnerCode", updatePartnerBank.PartnerCode);
            param.Add("@BankCode", updatePartnerBank.BankCode);
            param.Add("@AccountNumber", updatePartnerBank.AccountNumber);
            param.Add("@AccountName", updatePartnerBank.AccountName);
            param.Add("@IsActive", updatePartnerBank.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_banks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
