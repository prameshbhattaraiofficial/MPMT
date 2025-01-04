using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Data.Repositories.IncomeSource;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.IncomeSource;

public class IncomeSourceService : BaseService, IIncomeSourceService
{
    private readonly IIncomeSourceRepo _incomeSourceRepo;

    public IncomeSourceService(IIncomeSourceRepo incomeSourceRepo)
    {
        _incomeSourceRepo = incomeSourceRepo;
    }

    public async Task<SprocMessage> AddIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim)
    {
        incomeSource.Event = 'I';
        incomeSource.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        incomeSource.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _incomeSourceRepo.IUDIncomeSourceAsync(incomeSource);
        return response;
    }

    public async Task<SprocMessage> DeleteIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim)
    {
        incomeSource.Event = 'D';
        incomeSource.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        incomeSource.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _incomeSourceRepo.IUDIncomeSourceAsync(incomeSource);
        return response;
    }

    public async Task<IEnumerable<IncomeSourceDetails>> GetIncomeSourceAsync(IncomeSourceFilter sourceFilter)
    {
        var response = await _incomeSourceRepo.GetIncomeSourceAsync(sourceFilter);
        return response;
    }

    public async Task<IncomeSourceDetails> GetIncomeSourceByIdAsync(int id)
    {
        var response = await _incomeSourceRepo.GetIncomeSourceByIdAsync(id);
        return response;
    }

    public async Task<SprocMessage> UpdateIncomeSourceAsync(IUDIncomeSource incomeSource, ClaimsPrincipal claim)
    {
        incomeSource.Event = 'U';
        incomeSource.LoggedInUser = claim?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        incomeSource.UserType = claim?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _incomeSourceRepo.IUDIncomeSourceAsync(incomeSource);
        return response;
    }
}
