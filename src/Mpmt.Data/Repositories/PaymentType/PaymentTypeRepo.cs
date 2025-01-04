using Dapper;
using Mpmt.Core.Dtos.PaymentType;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.PaymentType
{
    /// <summary>
    /// The payment type repo.
    /// </summary>
    public class PaymentTypeRepo : IPaymentTypeRepo
    {
        /// <summary>
        /// Adds the payment type async.
        /// </summary>
        /// <param name="addPaymentType">The add payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddPaymentTypeAsync(IUDPaymentType addPaymentType)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", 'I');
                param.Add("@Id", addPaymentType.Id);
                param.Add("@PaymentTypeName", addPaymentType.PaymentTypeName);
                param.Add("@PaymentTypeCode", addPaymentType.PaymentTypeCode);
                param.Add("@Description", addPaymentType.Description);
                param.Add("@IsActive", addPaymentType.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_payment_type]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the payment type async.
        /// </summary>
        /// <param name="paymentTypeFilter">The payment type filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PaymentTypeDetails>> GetPaymentTypeAsync(PaymentTypeFilter paymentTypeFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PaymentTypeName", paymentTypeFilter.PaymentTypeName);
            param.Add("@PaymentTypeCode", paymentTypeFilter.PaymentTypeCode);
            param.Add("@Status", paymentTypeFilter.Status);
            return await connection.QueryAsync<PaymentTypeDetails>("[dbo].[usp_get_PaymentType]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the payment type by id async.
        /// </summary>
        /// <param name="paymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        public async Task<PaymentTypeDetails> GetPaymentTypeByIdAsync(int paymentTypeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", paymentTypeId);
            return await connection.QueryFirstOrDefaultAsync<PaymentTypeDetails>("[dbo].[usp_get_PaymentType_byid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the payment type async.
        /// </summary>
        /// <param name="removePaymentType">The remove payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemovePaymentTypeAsync(IUDPaymentType removePaymentType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", removePaymentType.Id);
            param.Add("@PaymentTypeName", removePaymentType.PaymentTypeName);
            param.Add("@PaymentTypeCode", removePaymentType.PaymentTypeCode);
            param.Add("@Description", removePaymentType.Description);
            param.Add("@IsActive", removePaymentType.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_payment_type]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the payment type async.
        /// </summary>
        /// <param name="updatePaymentType">The update payment type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdatePaymentTypeAsync(IUDPaymentType updatePaymentType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'U');
            param.Add("@Id", updatePaymentType.Id);
            param.Add("@PaymentTypeName", updatePaymentType.PaymentTypeName);
            param.Add("@PaymentTypeCode", updatePaymentType.PaymentTypeCode);
            param.Add("@Description", updatePaymentType.Description);
            param.Add("@IsActive", updatePaymentType.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_payment_type]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
