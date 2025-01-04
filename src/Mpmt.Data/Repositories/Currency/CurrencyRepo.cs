using Dapper;
using Mpmt.Core.Dtos.Currency;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Currency
{
    /// <summary>
    /// The currency repo.
    /// </summary>
    public class CurrencyRepo : ICurrencyRepo
    {
        /// <summary>
        /// Adds the currency async.
        /// </summary>
        /// <param name="addCurrency">The add currency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddCurrencyAsync(IUDCurrency addCurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'I');
            param.Add("@Id", addCurrency.Id);
            param.Add("@CurrencyName", addCurrency.CurrencyName);
            param.Add("@CurrencyImg", addCurrency.CurrencyImgPath);
            param.Add("@ShortName", addCurrency.ShortName);
            param.Add("@Symbol", addCurrency.Symbol);
            param.Add("@CountryCode", addCurrency.CountryCode);
            param.Add("@IsActive", addCurrency.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            try
            {
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_currency]", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                var data = ex.ToString();
            }



            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Gets the currency async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<CurrencyDetails>> GetCurrencyAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<CurrencyDetails>("[dbo].[usp_get_Currency]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the currency by id async.
        /// </summary>
        /// <param name="CurrencyId">The currency id.</param>
        /// <returns>A Task.</returns>
        public async Task<CurrencyDetails> GetCurrencyByIdAsync(int CurrencyId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", CurrencyId);

            return await connection.QueryFirstOrDefaultAsync<CurrencyDetails>("[dbo].[usp_get_Currency_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the currency async.
        /// </summary>
        /// <param name="RemoveCurrency">The remove currency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveCurrencyAsync(IUDCurrency RemoveCurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", RemoveCurrency.Id);
            param.Add("@CurrencyName", RemoveCurrency.CurrencyName);
            param.Add("@CurrencyImg", RemoveCurrency.CurrencyImgPath);
            param.Add("@ShortName", RemoveCurrency.ShortName);
            param.Add("@Symbol", RemoveCurrency.Symbol);
            param.Add("@CountryCode", RemoveCurrency.CountryCode);
            param.Add("@IsActive", RemoveCurrency.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_currency]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the currency async.
        /// </summary>
        /// <param name="updateCurrency">The update currency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateCurrencyAsync(IUDCurrency updateCurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'U');
            param.Add("@Id", updateCurrency.Id);
            param.Add("@CurrencyName", updateCurrency.CurrencyName);
            param.Add("@CurrencyImg", updateCurrency.CurrencyImgPath);
            param.Add("@ShortName", updateCurrency.ShortName);
            param.Add("@Symbol", updateCurrency.Symbol);
            param.Add("@CountryCode", updateCurrency.CountryCode);
            param.Add("@IsActive", updateCurrency.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_currency]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
