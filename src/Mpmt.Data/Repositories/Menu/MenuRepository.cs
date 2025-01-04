using Dapper;
using Mpmt.Core.Dtos.Menu;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Menu
{
    /// <summary>
    /// The menu repository.
    /// </summary>
    public class MenuRepository : IMenuRepository
    {

        /// <summary>
        /// Adds the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddMenuAsync(IUDMenu menu)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", menu.Id);
                param.Add("@Title", menu.Title);
                param.Add("@ParentId", menu.ParentId);
                param.Add("@MenuUrl", menu.MenuUrl);
                param.Add("@IsActive", menu.IsActive);
                param.Add("@DisplayOrder", menu.DisplayOrder);
                param.Add("@ImagePath", menu.ImagePath);

                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_menu]", param, commandType: CommandType.StoredProcedure);
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
          public async Task<SprocMessage> AddPartnerMenuAsync(IUDPartnerMenu menu)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", menu.Id);
                param.Add("@Title", menu.Title);
                param.Add("@ParentId", menu.ParentId);
                param.Add("@Area", menu.Area);
                param.Add("@Controller", menu.Controller);
                param.Add("@Action", menu.Action);
                param.Add("@IsActive", menu.IsActive);
                param.Add("@DisplayOrder", menu.DisplayOrder);
                param.Add("@ImagePath", menu.ImagePath);

                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_menu]", param, commandType: CommandType.StoredProcedure);
                var identityVal = param.Get<int>("@IdentityVal");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the menu async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<MenuModelView>> GetMenuAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<MenuModelView>("[dbo].[usp_get_menu]", commandType: CommandType.StoredProcedure);
        }
         public async Task<IEnumerable<PartnerMenu>> GetPartnerMenuAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<PartnerMenu>("[dbo].[usp_get_partner_menu]", commandType: CommandType.StoredProcedure);
        }
         public async Task<IEnumerable<PartnerMenuWithPermission>> GetPartnerMenuByUserNameAsync(string UserName)
        {
           
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", UserName);
            return await connection.QueryAsync<PartnerMenuWithPermission>("[dbo].[usp_get_partner_menu_byUsername]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IUDMenu> GetMenuByIdAsync(int MenuId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", MenuId);
            return await connection.QueryFirstOrDefaultAsync<IUDMenu>("[dbo].[usp_get_menu_ById]", param, commandType: CommandType.StoredProcedure);
        }
        public async Task<IUDPartnerMenu> GetPartnerMenuByIdAsync(int MenuId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", MenuId);
            return await connection.QueryFirstOrDefaultAsync<IUDPartnerMenu>("[dbo].[usp_get_partner_menu_ById]", param, commandType: CommandType.StoredProcedure);
        }


        public async Task<SprocMessage> RemoveMenuAsync(IUDMenu menu)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");

            param.Add("Id", menu.Id);
            param.Add("@Title", menu.Title);
            param.Add("@ParentId", menu.ParentId);
            param.Add("@MenuUrl", menu.MenuUrl);
            param.Add("@IsActive", menu.IsActive);
            param.Add("@DisplayOrder", menu.DisplayOrder);
            param.Add("@ImagePath", menu.ImagePath);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_menu]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> RemovePartnerMenuAsync(IUDPartnerMenu menu)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");

            param.Add("Id", menu.Id);
            param.Add("@Title", menu.Title);
            param.Add("@ParentId", menu.ParentId);
            param.Add("@Area", menu.Area);
            param.Add("@Controller", menu.Controller);
            param.Add("@Action", menu.Action);
            param.Add("@IsActive", menu.IsActive);
            param.Add("@DisplayOrder", menu.DisplayOrder);
            param.Add("@ImagePath", menu.ImagePath);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_menu]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }


        public async Task<SprocMessage> UpdateMenuAsync(IUDMenu menu)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", menu.Id);
            param.Add("@Title", menu.Title);
            param.Add("@ParentId", menu.ParentId);
            param.Add("@MenuUrl", menu.MenuUrl);
            param.Add("@IsActive", menu.IsActive);
            param.Add("@DisplayOrder", menu.DisplayOrder);
            param.Add("@ImagePath", menu.ImagePath);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_menu]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");


            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> UpdatePartnerMenuAsync(IUDPartnerMenu menu)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", menu.Id);
            param.Add("@Title", menu.Title);
            param.Add("@ParentId", menu.ParentId);
            param.Add("@Area", menu.Area);
            param.Add("@Controller", menu.Controller);
            param.Add("@Action", menu.Action);
            param.Add("@IsActive", menu.IsActive);
            param.Add("@DisplayOrder", menu.DisplayOrder);
            param.Add("@ImagePath", menu.ImagePath);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_menu]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");


            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDMenu menuUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", menuUpdate.Id);
            param.Add("@DisplayOrder", menuUpdate.DisplayOrder);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[sp_menu_displayOrder]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }

        public async Task<SprocMessage> UpdateMenuIsActiveAsync(IUDMenu menuUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", menuUpdate.Id);
            param.Add("@IsActive", menuUpdate.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[sp_menu_UpdateIsActive]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        } 
        public async Task<SprocMessage> UpdatePartnerMenuDisplayOrderAsync(IUDMenu menuUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", menuUpdate.Id);
            param.Add("@DisplayOrder", menuUpdate.DisplayOrder);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[sp_partner_menu_displayOrder]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }

        public async Task<SprocMessage> UpdatePartnerMenuIsActiveAsync(IUDMenu menuUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", menuUpdate.Id);
            param.Add("@IsActive", menuUpdate.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[sp_partner_menu_UpdateIsActive]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }
    }
}
