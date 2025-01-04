using Dapper;
using Mpmt.Core.Domain.Partners.Applications;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    public class PartnerApplicationRepository : IPartnerApplicationRepository
    {
        public async Task<SprocMessage> InsertAsync(PartnerApplication application)
        {
            const string operationMode = "I";
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Operation", operationMode);
            param.Add("@FirstName", application.FirstName);
            param.Add("@LastName", application.LastName);
            param.Add("@OrganizationName", application.OrganizationName);
            param.Add("@OrganizationEmail", application.OrganizationEmail);
            param.Add("@OrganizationContactNo", application.OrganizationContactNo);
            param.Add("@Designation", application.Designation);
            param.Add("@CountryCode", application.CountryCode);
            param.Add("@Address", application.Address);
            param.Add("@Message", application.Message);
            param.Add("@IpAddress", application.IpAddress);
            param.Add("@CountryName", application.CountryName);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_application]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
