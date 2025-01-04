using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.ConversionRateHistory;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.ConversionRate
{
    /// <summary>
    /// The conversion rate repo.
    /// </summary>
    public class ConversionRateRepo : IConversionRateRepo
    {
        private readonly IMapper _mapper;

        public ConversionRateRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the conversion rate async.
        /// </summary>
        /// <param name="addConversionRate">The add conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddConversionRateAsync(IUDConversionRate addConversionRate)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", 'I');
                param.Add("@Id", addConversionRate.Id);
                param.Add("@SourceCurrency", addConversionRate.SourceCurrency);
                param.Add("@DestinationCurrency", addConversionRate.DestinationCurrency);
                param.Add("@UnitValue", addConversionRate.UnitValue);
                param.Add("@BuyingRate", addConversionRate.BuyingRate);
                param.Add("@SellingRate", addConversionRate.SellingRate);
                param.Add("@CurrentRate", addConversionRate.CurrentRate);
                param.Add("@IsActive", addConversionRate.IsActive);
                param.Add("@UserType", addConversionRate.UserType);
                param.Add("@LoggedInUserName", addConversionRate.LoggedInUserName);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_exchange_rate]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the conversion rate async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ConversionRateDetails>> GetConversionRateAsync(ConversionRateFilter conversionRateFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@SourceCurrency", conversionRateFilter.SourceCurrency);
            param.Add("@DestinationCurrency", conversionRateFilter.DestinationCurrency);
            param.Add("@Status", conversionRateFilter.Status);
            return await connection.QueryAsync<ConversionRateDetails>("[dbo].[usp_get_exchangerate]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the conversion rate by id async.
        /// </summary>
        /// <param name="conversionRateId">The conversion rate id.</param>
        /// <returns>A Task.</returns>
        public async Task<ConversionRateDetails> GetConversionRateByIdAsync(int conversionRateId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", conversionRateId);
            return await connection.QueryFirstOrDefaultAsync<ConversionRateDetails>("[dbo].[usp_get_exchangerate_byid]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<PagedList<ExchangeRateHistoryDetails>> GetExchangeRateHistoryAsync(ExchangeRateFilter exchangeRateFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Wallet", exchangeRateFilter.WalletCurrency);
            param.Add("@StartDate", exchangeRateFilter.StartDate);
            param.Add("@EndDate", exchangeRateFilter.EndDate);
            param.Add("@Export", exchangeRateFilter.Export);

            param.Add("@PageNumber", exchangeRateFilter.PageNumber);
            param.Add("@PageSize", exchangeRateFilter.PageSize);
            param.Add("@SortingCol", exchangeRateFilter.SortOrder);
            param.Add("@SortType", exchangeRateFilter.SortBy);
            param.Add("@SearchVal", exchangeRateFilter.SearchVal);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_exchange_rate_history]", param: param, commandType: CommandType.StoredProcedure);

            var rateHistory = await data.ReadAsync<ExchangeRateHistoryDetails>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<ExchangeRateHistoryDetails>>(pagedInfo);
            mappeddata.Items = rateHistory;
            return mappeddata;
        }

        /// <summary>
        /// Removes the conversion rate async.
        /// </summary>
        /// <param name="removeConversionRate">The remove conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveConversionRateAsync(IUDConversionRate removeConversionRate)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", removeConversionRate.Id);
            param.Add("@SourceCurrency", removeConversionRate.SourceCurrency);
            param.Add("@DestinationCurrency", removeConversionRate.DestinationCurrency);
            param.Add("@UnitValue", removeConversionRate.UnitValue);
            param.Add("@BuyingRate", removeConversionRate.BuyingRate);
            param.Add("@SellingRate", removeConversionRate.SellingRate);
            param.Add("@CurrentRate", removeConversionRate.CurrentRate);
            param.Add("@IsActive", removeConversionRate.IsActive);
            param.Add("@UserType", removeConversionRate.UserType);
            param.Add("@LoggedInUserName", removeConversionRate.LoggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_exchange_rate]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the conversion rate async.
        /// </summary>
        /// <param name="updateConversionRate">The update conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, IEnumerable<ExchangeRateChangedListPartner>)> UpdateConversionRateAsync(IUDConversionRate updateConversionRate)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Event", 'U');
                param.Add("@Id", updateConversionRate.Id);
                param.Add("@SourceCurrency", updateConversionRate.SourceCurrency);
                param.Add("@DestinationCurrency", updateConversionRate.DestinationCurrency);
                param.Add("@UnitValue", updateConversionRate.UnitValue);
                param.Add("@BuyingRate", updateConversionRate.BuyingRate);
                param.Add("@SellingRate", updateConversionRate.SellingRate);
                param.Add("@CurrentRate", updateConversionRate.CurrentRate);
                param.Add("@IsActive", updateConversionRate.IsActive);
                param.Add("@IsSendNotificationEmail", updateConversionRate.IsSendNotificationEmail);
                param.Add("@UserType", updateConversionRate.UserType);
                param.Add("@LoggedInUserName", updateConversionRate.LoggedInUserName);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection.QueryAsync("[dbo].[Usp_IUD_exchange_rate]", param, commandType: CommandType.StoredProcedure);
                var data = _mapper.Map<IEnumerable<ExchangeRateChangedListPartner>>(result);

                var identityVal = param.Get<int>("@IdentityVal");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return (new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText }, data);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
