using Dapper;
using Mpmt.Core.Dtos.ServiceChargeCategory;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.ServiceChargeCategory
{
    /// <summary>
    /// The service category repo.
    /// </summary>
    public class ServiceCategoryRepo : IServiceCategoryRepo
    {
        /// <summary>
        /// Adds the service category async.
        /// </summary>
        /// <param name="addServiceCategory">The add service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddServiceCategoryAsync(IUDServiceCategory addServiceCategory)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addServiceCategory.Id);
                param.Add("@CategoryName", addServiceCategory.CategoryName);
                param.Add("@CategoryCode", addServiceCategory.CategoryCode);
                param.Add("@Description", addServiceCategory.Description);
                param.Add("@IsActive", addServiceCategory.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_service_charge_category]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the service category async.
        /// </summary>
        /// <param name="serviceCategoryFilter">The service category filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ServiceCategoryDetails>> GetServiceCategoryAsync(ServiceCategoryFilter serviceCategoryFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@CategoryName", serviceCategoryFilter.CategoryName);
            param.Add("@CategoryCode", serviceCategoryFilter.CategoryCode);
            param.Add("@Status", serviceCategoryFilter.Status);
            return await connection.QueryAsync<ServiceCategoryDetails>("[dbo].[usp_get_service_charge_category]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the service category by id async.
        /// </summary>
        /// <param name="serviceCategoryId">The service category id.</param>
        /// <returns>A Task.</returns>
        public async Task<ServiceCategoryDetails> GetServiceCategoryByIdAsync(int serviceCategoryId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", serviceCategoryId);
            return await connection.QueryFirstOrDefaultAsync<ServiceCategoryDetails>("[dbo].[usp_get_servicecharge_category_byid]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the service category async.
        /// </summary>
        /// <param name="removeServiceCategory">The remove service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveServiceCategoryAsync(IUDServiceCategory removeServiceCategory)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");
            param.Add("Id", removeServiceCategory.Id);
            param.Add("@CategoryName", removeServiceCategory.CategoryName);
            param.Add("@CategoryCode", removeServiceCategory.CategoryCode);
            param.Add("@Description", removeServiceCategory.Description);
            param.Add("@IsActive", removeServiceCategory.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_service_charge_category]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the service category async.
        /// </summary>
        /// <param name="updateServiceCategory">The update service category.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateServiceCategoryAsync(IUDServiceCategory updateServiceCategory)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", updateServiceCategory.Id);
            param.Add("@CategoryName", updateServiceCategory.CategoryName);
            param.Add("@CategoryCode", updateServiceCategory.CategoryCode);
            param.Add("@Description", updateServiceCategory.Description);
            param.Add("@IsActive", updateServiceCategory.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_service_charge_category]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
