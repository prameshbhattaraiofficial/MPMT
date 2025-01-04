using Dapper;
using Mpmt.Core.Dtos.KYCRemark;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.KYCRemark
{
    /// <summary>
    /// The kyc remark repo.
    /// </summary>
    public class KycRemarkRepo : IKycRemarkRepo
    {
        /// <summary>
        /// Adds the kyc remark async.
        /// </summary>
        /// <param name="addKycRemark">The add kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddKycRemarkAsync(IUDKycRemark addKycRemark)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addKycRemark.Id);
                param.Add("@RemarksName", addKycRemark.RemarksName);
                param.Add("@Description", addKycRemark.Description);
                param.Add("@IsActive", addKycRemark.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_kyc_remarks]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the kyc remark async.
        /// </summary>
        /// <param name="kycRemarkFilter">The kyc remark filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<KycRemarkDetails>> GetKycRemarkAsync(KycRemarkFilter kycRemarkFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@RemarksName", kycRemarkFilter.RemarksName);
            param.Add("@Status", kycRemarkFilter.Status);
            return await connection.QueryAsync<KycRemarkDetails>("[dbo].[usp_get_KycRemarks]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the kyc remark by id async.
        /// </summary>
        /// <param name="kycRemarkId">The kyc remark id.</param>
        /// <returns>A Task.</returns>
        public async Task<KycRemarkDetails> GetKycRemarkByIdAsync(int kycRemarkId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", kycRemarkId);
            return await connection.QueryFirstOrDefaultAsync<KycRemarkDetails>("[dbo].[usp_get_KycRemarks_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the kyc remark async.
        /// </summary>
        /// <param name="removeKycRemark">The remove kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveKycRemarkAsync(IUDKycRemark removeKycRemark)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");
            param.Add("Id", removeKycRemark.Id);
            param.Add("@RemarksName", removeKycRemark.RemarksName);
            param.Add("@Description", removeKycRemark.Description);
            param.Add("@IsActive", removeKycRemark.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_kyc_remarks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the kyc remark async.
        /// </summary>
        /// <param name="updateKycRemark">The update kyc remark.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateKycRemarkAsync(IUDKycRemark updateKycRemark)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", updateKycRemark.Id);
            param.Add("@RemarksName", updateKycRemark.RemarksName);
            param.Add("@Description", updateKycRemark.Description);
            param.Add("@IsActive", updateKycRemark.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_kyc_remarks]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
