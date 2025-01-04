using Dapper;
using Mpmt.Core.Dtos.EODBalance;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerEODBalance;

public class EODBalanceRepository : IEODBalanceRepository
{
    public async Task<IEnumerable<EODBalance>> GetPartnerEODBalanceAsync()
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        return await connection.QueryAsync<EODBalance>("[dbo].[usp_get_partner_eod_balance]", commandType: CommandType.StoredProcedure);
    }
}