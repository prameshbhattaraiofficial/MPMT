using Dapper;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.ConversionRate
{
    /// <summary>
    /// The partner conversion rate repo.
    /// </summary>
    public class PartnerConversionRateRepo : IPartnerConversionRateRepo
    {
        /// <summary>
        /// Adds the conversion rate async.
        /// </summary>
        /// <param name="addConversionRate">The add conversion rate.</param>
        /// <param name="partnerConversionRate">The partner conversion rate.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddConversionRateAsync(List<AddPartnerConversionRate> addConversionRate, PartnerConversionRate partnerConversionRate)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var dataTableRmp = GetDataTableServiceCharge();

                foreach (var rmpType in addConversionRate)
                {
                    var row = dataTableRmp.NewRow();
                    row["MinAmountSlab"] = rmpType.MinAmountSlab;
                    row["MaxAmountSlab"] = rmpType.MaxAmountSlab;
                    row["ConversionRate"] = rmpType.ConversionRate;
                    row["ServiceChargePercent"] = rmpType.ServiceChargePercent;
                    row["ServiceChargeFixed"] = rmpType.ServiceChargeFixed;
                    row["MinServiceCharge"] = rmpType.MinServiceCharge;
                    row["MaxServiceCharge"] = rmpType.MaxServiceCharge;
                    row["CommissionPercent"] = rmpType.CommissionPercent ?? 0;
                    row["CommissionFixed"] = rmpType.CommissionFixed ?? 0;
                    row["MinCommission"] = rmpType.MinCommission ?? 0;
                    row["MaxCommission"] = rmpType.MaxCommission ?? 0;
                    dataTableRmp.Rows.Add(row);
                }
                var param = new DynamicParameters();
                param.Add("@PartnerCode", partnerConversionRate.PartnerCode);
                param.Add("@SourceCurrency", partnerConversionRate.SourceCurrency);
                param.Add("@DestinationCurrency", partnerConversionRate.DestinationCurrency);
                param.Add("@ConversionSettingsType", dataTableRmp.AsTableValuedParameter("[dbo].[ConversionSettingsType]"));
                param.Add("@OperationMode", "A");
                param.Add("@LoggedInUser", "Partner");
                param.Add("@UserType", "Partner");
                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync("[dbo].[usp_partner_conversion_rate_insert]", param, commandType: CommandType.StoredProcedure);
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
        /// Gets the conversion rate async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerConversionRate>> GetConversionRateAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", conversionRateFilter.PartnerCode);
            return await connection.QueryAsync<PartnerConversionRate>("[dbo].[use_get_partner_conversion_rate_list]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the conversion rate detail async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        public async Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> GetConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", conversionRateFilter.PartnerCode);
            param.Add("@SourceCurrency", conversionRateFilter.SourceCurrency);
            param.Add("@DestinationCurrency", conversionRateFilter.DestinationCurrency);
            var data = await connection
                .QueryMultipleAsync("[dbo].[use_get_partner_conversion_rate_details]", param: param, commandType: CommandType.StoredProcedure);
            var SelectInfo = await data.ReadFirstAsync<PartnerConversionRate>();
            var ChargeList = await data.ReadAsync<PartnerConversionRateDetails>();
            (List<PartnerConversionRateDetails> ChargeList, PartnerConversionRate SelectInfo) value = (ChargeList.ToList(), SelectInfo);
            return value;
        }

        /// <summary>
        /// Removes the conversion rate async.
        /// </summary>
        /// <param name="removeConversionRate">The remove conversion rate.</param>
        /// <returns>A Task.</returns>
        public Task<SprocMessage> RemoveConversionRateAsync(AddPartnerConversionRate removeConversionRate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Views the conversion rate detail async.
        /// </summary>
        /// <param name="conversionRateFilter">The conversion rate filter.</param>
        /// <returns>A Task.</returns>
        public async Task<(List<PartnerConversionRateDetails>, PartnerConversionRate)> ViewConversionRateDetailAsync(PartnerConversionRateFilter conversionRateFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", conversionRateFilter.PartnerCode);
            param.Add("@SourceCurrency", conversionRateFilter.SourceCurrency);
            param.Add("@DestinationCurrency", conversionRateFilter.DestinationCurrency);
            param.Add("@PaymentTypeId", conversionRateFilter.PaymentTypeId);
            var data = await connection
                .QueryMultipleAsync("[dbo].[use_get_conversion_rate_details]", param: param, commandType: CommandType.StoredProcedure);
            var SelectInfo = await data.ReadFirstAsync<PartnerConversionRate>();
            var ChargeList = await data.ReadAsync<PartnerConversionRateDetails>();
            (List<PartnerConversionRateDetails> ChargeList, PartnerConversionRate SelectInfo) value = (ChargeList.ToList(), SelectInfo);
            return value;
        }

        /// <summary>
        /// Gets the data table service charge.
        /// </summary>
        /// <returns>A DataTable.</returns>
        private DataTable GetDataTableServiceCharge()
        {
            var dataTableRmp = new DataTable();
            dataTableRmp.Columns.Add("MinAmountSlab", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MaxAmountSlab", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("ConversionRate", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("ServiceChargePercent", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("ServiceChargeFixed", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MinServiceCharge", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MaxServiceCharge", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CommissionPercent", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CommissionFixed", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MinCommission", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MaxCommission", typeof(decimal)).AllowDBNull = true;
            return dataTableRmp;
        }
    }
}
