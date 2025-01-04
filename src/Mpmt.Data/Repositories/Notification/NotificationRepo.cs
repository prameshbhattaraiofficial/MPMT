using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Notification
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly IMapper _mapper;

        public NotificationRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<SprocMessage> AssignModuleRole(int moduleid, int[] roleids)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@ModuleId", moduleid);
                param.Add("@UserType", "Admin");
                param.Add("@RoleIds", string.Join(',', roleids));

                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_notification_module]", param, commandType: CommandType.StoredProcedure);

                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = 0, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AdminNotificationModel>> GetAdminNotificationAsync(string userName)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@UserName", userName);
                var data = await connection.QueryAsync<AdminNotificationModel>("[dbo].[use_get_admin_notifications]", param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetAdminNotificationCountAsync(string userName)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@UserName", userName);
                param.Add("@Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[use_get_admin_notifications_count]", param, commandType: CommandType.StoredProcedure);

                var count = param.Get<int>("@Count");
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedList<NotificationModel>> GetNotificationAsync(NotificationsFilter notifications)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", notifications.PartnerCode);
            param.Add("@UserType", notifications.UserType);

            param.Add("@PageNumber", notifications.PageNumber);
            param.Add("@PageSize", notifications.PageSize);
            param.Add("@SortingCol", notifications.SortBy);
            param.Add("@SortType", notifications.SortOrder);
            param.Add("@SearchVal", notifications.SearchVal);
            param.Add("@Export", notifications.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_notifications]", param: param, commandType: CommandType.StoredProcedure);

            var partnerApplications = await data.ReadAsync<NotificationModel>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<NotificationModel>>(pagedInfo);
            mappeddata.Items = partnerApplications;
            return mappeddata;
        }

        public async Task<IEnumerable<NotificationModule>> GetNotificationModuleAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<NotificationModule>("[dbo].[usp_get_notification_module]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PartnerNotificationModel>> GetPartnerNotificationAsync(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            try{
                return await connection.QueryAsync<PartnerNotificationModel>("[dbo].[use_get_partner_notifications]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) 
            { 
                throw; 
            }

            
        }
        public async Task<string> GetSignalRconnectionstring(string ConnectionIdentifier)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ConnectionIdentifier", ConnectionIdentifier);
            var signalrconnection = await connection.QueryFirstOrDefaultAsync<string>("[dbo].[usp_get_SignalRconnection_Id]", param, commandType: CommandType.StoredProcedure);
            return signalrconnection;
        }
       

        public async Task DeleteSignalRconnectionstring(string ConnectionIdentifier)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ConnectionIdentifier", ConnectionIdentifier);
            _ = await connection.ExecuteAsync("[dbo].[usp_Delete_SignalRconnection_Id]", param, commandType: CommandType.StoredProcedure);
            
        }
        public async Task AddSignalRconnectionstring(string ConnectionIdentifier, string connectionstring)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@SignalRconnectionId", connectionstring);
            param.Add("@ConnectionIdentifier", ConnectionIdentifier);
            _ = await connection.ExecuteAsync("[dbo].[usp_add_SignalRconnection_Id]", param, commandType: CommandType.StoredProcedure);

        }
        public async Task<int> GetPartnerNotificationCountAsync(string partnerCode)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@PartnerCode", partnerCode);
                param.Add("@Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[use_get_partner_notifications_count]", param, commandType: CommandType.StoredProcedure);

                var count = param.Get<int>("@Count");
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SprocMessage> IUDNotificationAsync(IUDNotification notification)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", notification.Event);
                param.Add("@UserType", notification.UserType);
                param.Add("@UserId", notification.UserId);
                param.Add("@PartnerCode", notification.PartnerCode);
                param.Add("@Message", notification.Message);
                param.Add("@AdminLink", notification.AdminLink);
                param.Add("@PartnerLink", notification.PartnerLink);
                param.Add("@ModuleCode", notification.ModuleCode);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Notification]", param, commandType: CommandType.StoredProcedure);

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

        public async Task MarkNotificationAsRead(string userName, Guid notificationId)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "R");
                param.Add("@UserName", userName);
                param.Add("@NotificationId", notificationId);
                _ = await connection.QueryAsync<AdminNotificationModel>("[dbo].[use_get_admin_notifications]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task MarkPartnerNotificationAsRead(string partnerCode, Guid notificationId)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "R");
                param.Add("@PartnerCode", partnerCode);
                param.Add("@NotificationId", notificationId);
                _ = await connection.QueryAsync<AdminNotificationModel>("[dbo].[use_get_partner_notifications]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
