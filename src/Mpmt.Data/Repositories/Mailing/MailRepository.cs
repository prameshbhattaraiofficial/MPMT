using Dapper;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.Mailing
{
    public class MailRepository : IMailRepository
    {

        public async Task<MailConfiguration> GetMailConfiguration()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            return await connection.QueryFirstOrDefaultAsync<MailConfiguration>("[dbo].[usp_get_mail_configuration_settings]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MailMessageSettingsModel>> MailMessageSettings()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            return await connection.QueryAsync<MailMessageSettingsModel>("[dbo].[usp_get_mail_message_settings]", param, commandType: CommandType.StoredProcedure);
        }
    }
}
