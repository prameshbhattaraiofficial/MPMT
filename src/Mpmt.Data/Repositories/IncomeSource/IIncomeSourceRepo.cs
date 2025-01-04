using Mpmt.Core.Dtos.IncomeSource;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.IncomeSource;

public interface IIncomeSourceRepo
{
    Task<IEnumerable<IncomeSourceDetails>> GetIncomeSourceAsync(IncomeSourceFilter sourceFilter);
    Task<IncomeSourceDetails> GetIncomeSourceByIdAsync(int id);
    Task<SprocMessage> IUDIncomeSourceAsync(IUDIncomeSource incomeSource);
}
