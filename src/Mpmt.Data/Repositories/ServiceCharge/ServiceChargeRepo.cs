using Dapper;
using Mpmt.Core.Dtos.ServiceCharge;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.ServiceCharge
{
    /// <summary>
    /// The service charge repo.
    /// </summary>
    public class ServiceChargeRepo : IServiceChargeRepo
    {
        /// <summary>
        /// Adds the service charge async.
        /// </summary>
        /// <param name="addServiceCharge">The add service charge.</param>
        /// <param name="chargeategoryid">The chargeategoryid.</param>
        /// <param name="paymenttypeid">The paymenttypeid.</param>
        /// <param name="sourcecurrency">The sourcecurrency.</param>
        /// <param name="destinationcurrency">The destinationcurrency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddServiceChargeAsync(List<AddServiceCharges> addServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var dataTableRmp = GetDataTableServiceCharge();

                foreach (var rmpType in addServiceCharge)
                {
                    var row = dataTableRmp.NewRow();
                    row["MinAmountSlab"] = rmpType.MinAmountSlab;
                    row["MaxAmountSlab"] = rmpType.MaxAmountSlab;
                    row["ServiceChargePercent"] = rmpType.ServiceChargePercent;
                    row["ServiceChargeFixed"] = rmpType.ServiceChargeFixed;
                    row["MinServiceCharge"] = rmpType.MinServiceCharge;
                    row["MaxServiceCharge"] = rmpType.MaxServiceCharge;
                    row["CommissionPercent"] = rmpType.CommissionPercent;
                    row["CommissionFixed"] = rmpType.CommissionFixed;
                    row["MinCommission"] = rmpType.MinComission;
                    row["MaxCommission"] = rmpType.MaxComission;
                    row["FromDate"] = rmpType.FromDate;
                    row["ToDate"] = rmpType.ToDate;
                    dataTableRmp.Rows.Add(row);
                }

                var param = new DynamicParameters();
                param.Add("@ServiceChargeSettingsType", dataTableRmp.AsTableValuedParameter("[dbo].[ServiceChargeSettingsType]"));
                param.Add("@SourceCurrency", sourcecurrency);
                param.Add("@DestinationCurrency", destinationcurrency);
                param.Add("@ChargeCategoryId", chargeategoryid);
                param.Add("@PaymentTypeId", paymenttypeid);
                param.Add("@OperationMode", "A");
                param.Add("@LoggedInUser", "Admin");
                param.Add("@UserType", "Admin");
                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync("[dbo].[usp_ServiceTypeListInsert]", param, commandType: CommandType.StoredProcedure);
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
        /// Gets the service charge async.
        /// </summary>
        /// <param name="serviceChargeFilter">The service charge filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ServiceChargeDetails>> GetServiceChargeAsync(ServiceChargeFilter serviceChargeFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@SourceCurrency", serviceChargeFilter.SourceCurrency);
            param.Add("@DestinationCurrency", serviceChargeFilter.DestinationCurrency);
            param.Add("@paymentType", serviceChargeFilter.PaymentType);
            param.Add("@Category", serviceChargeFilter.ChargeCategory);
            param.Add("@StartDate", serviceChargeFilter.FromDate);
            param.Add("@EndDate", serviceChargeFilter.ToDate);
            return await connection.QueryAsync<ServiceChargeDetails>("[dbo].[sp_get_ServiceCharge_Setting]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the service charge by id async.
        /// </summary>
        /// <param name="CategoryId">The category id.</param>
        /// <param name="SourceCurrency">The source currency.</param>
        /// <param name="DestinationCurrency">The destination currency.</param>
        /// <param name="PaymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        public async Task<(List<ServiceChargeList>, ServiceChargeSelect)> GetServiceChargeByIdAsync(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ChargeCategoryId", CategoryId);
            param.Add("@SourceCurrency", SourceCurrency);
            param.Add("@DestinationCurrency", DestinationCurrency);
            param.Add("@PaymenttypeId", PaymentTypeId);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_servicecharge_setting_byId]", param: param, commandType: CommandType.StoredProcedure);
            var ChargeList = await data.ReadAsync<ServiceChargeList>();
            var SelectInfo = await data.ReadFirstAsync<ServiceChargeSelect>();
            (List<ServiceChargeList> ChargeList, ServiceChargeSelect SelectInfo) value = (ChargeList.ToList(), SelectInfo);
            return value;
        }

        /// <summary>
        /// Removes the service charge async.
        /// </summary>
        /// <param name="serviceChargeSelect">The service charge select.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveServiceChargeAsync(ServiceChargeSelect serviceChargeSelect)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@SourceCurrency", serviceChargeSelect.SourceCurrency);
            param.Add("@DestinationCurrency", serviceChargeSelect.DestinationCurrency);
            param.Add("@ChargeCategoryId", serviceChargeSelect.ChargeCategoryId);
            param.Add("@PaymentTypeId", serviceChargeSelect.PaymentTypeId);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var result = await connection.ExecuteAsync("[dbo].[usp_servicetypesetting_remove]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the service charge async.
        /// </summary>
        /// <param name="updateServiceCharge">The update service charge.</param>
        /// <param name="chargeategoryid">The chargeategoryid.</param>
        /// <param name="paymenttypeid">The paymenttypeid.</param>
        /// <param name="sourcecurrency">The sourcecurrency.</param>
        /// <param name="destinationcurrency">The destinationcurrency.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateServiceChargeAsync(List<AddServiceCharges> updateServiceCharge, int chargeategoryid, int paymenttypeid, string sourcecurrency, string destinationcurrency)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var dataTableRmp = GetDataTableServiceCharge();

            foreach (var rmpType in updateServiceCharge)
            {
                var row = dataTableRmp.NewRow();
                row["MinAmountSlab"] = rmpType.MinAmountSlab;
                row["MaxAmountSlab"] = rmpType.MaxAmountSlab;
                row["ServiceChargePercent"] = rmpType.ServiceChargePercent;
                row["ServiceChargeFixed"] = rmpType.ServiceChargeFixed;
                row["MinServiceCharge"] = rmpType.MinServiceCharge;
                row["MaxServiceCharge"] = rmpType.MaxServiceCharge;
                row["CommissionPercent"] = rmpType.CommissionPercent;
                row["CommissionFixed"] = rmpType.CommissionFixed;
                row["MinCommission"] = rmpType.MinComission;
                row["MaxCommission"] = rmpType.MaxComission;
                row["FromDate"] = rmpType.FromDate;
                row["ToDate"] = rmpType.ToDate;
                dataTableRmp.Rows.Add(row);
            }

            var param = new DynamicParameters();
            param.Add("@ServiceChargeSettingsType", dataTableRmp.AsTableValuedParameter("[dbo].[ServiceChargeSettingsType]"));
            param.Add("@SourceCurrency", sourcecurrency);
            param.Add("@DestinationCurrency", destinationcurrency);
            param.Add("@ChargeCategoryId", chargeategoryid);
            param.Add("@PaymentTypeId", paymenttypeid);
            param.Add("@OperationMode", "U");
            param.Add("@LoggedInUser", "Admin");
            param.Add("@UserType", "Admin");
            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var result = await connection.ExecuteAsync("[dbo].[usp_ServiceTypeListInsert]", param, commandType: CommandType.StoredProcedure);
            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
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
            dataTableRmp.Columns.Add("ServiceChargePercent", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("ServiceChargeFixed", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MinServiceCharge", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MaxServiceCharge", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CommissionPercent", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CommissionFixed", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MinCommission", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("MaxCommission", typeof(decimal)).AllowDBNull = true;
            dataTableRmp.Columns.Add("FromDate", typeof(DateTime)).AllowDBNull = true;
            dataTableRmp.Columns.Add("ToDate", typeof(DateTime)).AllowDBNull = true;
            return dataTableRmp;
        }
    }
}
