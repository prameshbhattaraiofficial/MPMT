using Dapper;
using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.IncomeSource;

public class IncomeSourceRepo : IIncomeSourceRepo
{
    public async Task<IEnumerable<IncomeSourceDetails>> GetIncomeSourceAsync(IncomeSourceFilter sourceFilter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@IncomeSourceName", sourceFilter.IncomeSourceName);
        param.Add("@Status", sourceFilter.Status);
        return await connection.QueryAsync<IncomeSourceDetails>("[dbo].[usp_get_Incomesource]", commandType: CommandType.StoredProcedure);
    }

    public async Task<IncomeSourceDetails> GetIncomeSourceByIdAsync(int id)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", id);
        return await connection.QueryFirstOrDefaultAsync<IncomeSourceDetails>("[dbo].[usp_get_Incomesource_ById]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> IUDIncomeSourceAsync(IUDIncomeSource incomeSource)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", incomeSource.Event);
            param.Add("@Id", incomeSource.Id);
            param.Add("@@SourceName", incomeSource.SourceName);
            param.Add("@Description", incomeSource.Description);
            param.Add("@IsActive", incomeSource.IsActive);
            param.Add("@LoggedInUser", incomeSource.LoggedInUser);
            param.Add("@UserType", incomeSource.UserType);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_income_source]", param, commandType: CommandType.StoredProcedure);

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
}
