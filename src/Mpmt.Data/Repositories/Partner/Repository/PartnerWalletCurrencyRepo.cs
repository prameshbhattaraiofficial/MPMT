using Dapper;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner.IRepository;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner.Repository
{
    /// <summary>
    /// The partner wallet currency repo.
    /// </summary>
    public class PartnerWalletCurrencyRepo : IPartnerWalletCurrencyRepo
    {
        /// <summary>
        /// Adds the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>

        public async Task<SprocMessage> AddUpdateFundRequestAsync(AddUpdateFundRequest addUpdateFundRequest)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", addUpdateFundRequest.Event);
                param.Add("@Id", addUpdateFundRequest.Id);
                param.Add("@WalletId", addUpdateFundRequest.WalletId);
                param.Add("@FundTypeId", addUpdateFundRequest.FundTypeId);
                param.Add("@Amount", addUpdateFundRequest.Amount);
                param.Add("@Sign", addUpdateFundRequest.Sign);
                param.Add("@TxnId", addUpdateFundRequest.TransactionId);
                param.Add("@VoucherImagePath", addUpdateFundRequest.VoucherImgPath);
                param.Add("@Remarks", addUpdateFundRequest.Remarks);
                param.Add("@LoggedInUser", addUpdateFundRequest.LoggedInUser);
                param.Add("@UserType", addUpdateFundRequest.UserType);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_prefund_request_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
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
        /// Adds the update wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddUpdateWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", addUpdateWalletcurrency.OperationMode);
                param.Add("@Id", addUpdateWalletcurrency.Id);
                param.Add("@PartnerCode", addUpdateWalletcurrency.PartnerCode);
                param.Add("@SourceCurrency", addUpdateWalletcurrency.SourceCurrency);
                param.Add("@DestinationCurrency", addUpdateWalletcurrency.DestinationCurrency);
                param.Add("@NotificationBalance", addUpdateWalletcurrency.NotificationBalanceLimit);
                param.Add("@MarkupMinValue", addUpdateWalletcurrency.MarkupMinValue);
                param.Add("@MarkupMaxValue", addUpdateWalletcurrency.MarkupMaxValue);
                param.Add("@TypeCode", addUpdateWalletcurrency.TypeCode);
                param.Add("@Remarks", addUpdateWalletcurrency.Remarks);
                param.Add("@LoggedInUser", addUpdateWalletcurrency.LoggedInUser);
                param.Add("@UserType", addUpdateWalletcurrency.UserType);
                param.Add("@CreditLimit", addUpdateWalletcurrency.CreditLimit);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_partner_wallet_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
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
        /// Adds the update fee wallet async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddUpdateFeeWalletAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", addUpdateWalletcurrency.OperationMode);
                param.Add("@Id", addUpdateWalletcurrency.Id);
                param.Add("@PartnerCode", addUpdateWalletcurrency.PartnerCode);
                param.Add("@SourceCurrency", addUpdateWalletcurrency.SourceCurrency);
                param.Add("@NotificationBalance", addUpdateWalletcurrency.NotificationBalanceLimit);
                param.Add("@MarkupMinValue", addUpdateWalletcurrency.MarkupMinValue);
                param.Add("@MarkupMaxValue", addUpdateWalletcurrency.MarkupMaxValue);
                param.Add("@TypeCode", addUpdateWalletcurrency.TypeCode);
                param.Add("@Remarks", addUpdateWalletcurrency.Remarks);
                param.Add("@LoggedInUser", addUpdateWalletcurrency.LoggedInUser);
                param.Add("@UserType", addUpdateWalletcurrency.UserType);
                param.Add("@CreditLimit", addUpdateWalletcurrency.CreditLimit);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_partner_feewallet_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
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
        /// Adds the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        public Task<SprocMessage> AddWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the partner wallet currency.
        /// </summary>
        /// <param name="partnercode">The partnercode.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<WalletCurrencyDetails>> GetPartnerWalletCurrency(string partnercode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("partnerCode", partnercode);
            return await connection
                .QueryAsync<WalletCurrencyDetails>("[dbo].[usp_get_WalletCurrency]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the partner wallet currency balance.
        /// </summary>
        /// <param name="partnercode">The partnercode.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<WalletCurrencyBalance>> GetPartnerWalletCurrencyBalance(string partnercode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("partnerCode", partnercode);
            return await connection
                .QueryAsync<WalletCurrencyBalance>("[dbo].[usp_get_WalletCurrencyBalance]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the partner wallet currency by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<WalletCurrencyDetails> GetPartnerWalletCurrencyById(int Id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("Id", Id);
            return await connection.QueryFirstOrDefaultAsync<WalletCurrencyDetails>("[dbo].[usp_get_walletCurrency_byid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the fee wallet currency by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<WalletCurrencyDetails> GetFeeWalletCurrencyById(int Id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("Id", Id);
            return await connection.QueryFirstOrDefaultAsync<WalletCurrencyDetails>("[dbo].[usp_get_feewalletCurrency_byid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the wallet currency async.
        /// </summary>
        /// <param name="addUpdateWalletcurrency">The add update walletcurrency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveWalletCurrencyAsync(IUDUpdateWalletCurrency addUpdateWalletcurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", 'D');
                param.Add("@Id", addUpdateWalletcurrency.Id);
                param.Add("@PartnerCode", addUpdateWalletcurrency.PartnerCode);
                param.Add("@SourceCurrency", addUpdateWalletcurrency.SourceCurrency);
                param.Add("@DestinationCurrency", addUpdateWalletcurrency.DestinationCurrency);
                param.Add("@NotificationBalance", addUpdateWalletcurrency.NotificationBalance);
                param.Add("@MarkupMinValue", addUpdateWalletcurrency.MarkupMinValue);
                param.Add("@MarkupMaxValue", addUpdateWalletcurrency.MarkupMaxValue);
                param.Add("@TypeCode", addUpdateWalletcurrency.TypeCode);
                param.Add("@Remarks", addUpdateWalletcurrency.Remarks);
                param.Add("@LoggedInUser", addUpdateWalletcurrency.LoggedInUser);
                param.Add("@UserType", addUpdateWalletcurrency.UserType);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_partner_wallet_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
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

        public async Task<SprocMessage> AddFeeBalanceAsync(AddUpdateFundRequest addUpdateFundRequest)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", addUpdateFundRequest.Event);
                param.Add("@Id", addUpdateFundRequest.Id);
                param.Add("@WalletId", addUpdateFundRequest.WalletId);
                param.Add("@FundTypeId", addUpdateFundRequest.FundTypeId);
                param.Add("@Amount", addUpdateFundRequest.Amount);
                param.Add("@Sign", addUpdateFundRequest.Sign);
                param.Add("@TxnId", addUpdateFundRequest.TransactionId);
                param.Add("@VoucherImagePath", addUpdateFundRequest.VoucherImgPath);
                param.Add("@Remarks", addUpdateFundRequest.Remarks);
                param.Add("@LoggedInUser", addUpdateFundRequest.LoggedInUser);
                param.Add("@UserType", addUpdateFundRequest.UserType);

                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_feewalletbalance_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
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
    }
}