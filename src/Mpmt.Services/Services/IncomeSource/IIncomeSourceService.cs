using Mpmt.Core.Dtos.IncomeSource;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.IncomeSource;

public interface IIncomeSourceService
{
    Task<IEnumerable<IncomeSourceDetails>> GetIncomeSourceAsync(IncomeSourceFilter sourceFilter);
    Task<IncomeSourceDetails> GetIncomeSourceByIdAsync(int id);
    Task<SprocMessage> AddIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim);
    Task<SprocMessage> UpdateIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim);
    Task<SprocMessage> DeleteIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim);
}
