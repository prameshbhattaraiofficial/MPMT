using AutoMapper;
using Mpmt.Core.Dtos.ComplianceRule;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Data.Repositories.ComplianceRule;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.ComplianceRule;

public class ComplianceRuleService : BaseService, IComplianceRuleService
{
    private readonly IComplianceRuleRepo _complianceRule;
    private readonly IMapper _mapper;

    public ComplianceRuleService(IComplianceRuleRepo complianceRule, IMapper mapper)
    {
        _complianceRule = complianceRule;
        _mapper = mapper;
    }

    public async Task<SprocMessage> AddComplianceCountryList(string countryListString)
    {
        var response = await _complianceRule.AddComplianceCountryList(countryListString);
        return response;
    }

    public async Task<IEnumerable<CountryComplianceRule>> GetAllCountryList()
    {
        var response = await _complianceRule.GetAllCountryList();
        return response;
    }

    public async Task<IEnumerable<CountryComplianceRule>> GetComplianceCountryList()
    {
        var response = await _complianceRule.GetComplianceCountryList();
        return response;
    }

    public async Task<PagedList<ComplianceRuleList>> GetComplianceRuleAsync(ComplianceRuleFilter filter)
    {
        var response = await _complianceRule.GetComplianceRuleAsync(filter);
        return response;
    }

    public async Task<PagedList<RemitTxnReport>> GetComplianceTransactionAsync(RemitTxnReportFilter txnFilter)
    {
        var response = await _complianceRule.GetComplianceTransactionAsync(txnFilter);
        return response;
    }

    public async Task<IEnumerable<Commonddl>> GetFrequency()
    {
        var response = await _complianceRule.GetFrequency();
        return response;
    }

    public async Task<SprocMessage> ReleaseTransaction(string transactionId,ClaimsPrincipal User)
    {
        var LoggedInUser = User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var UserType = User?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
        var response = await _complianceRule.ReleaseTransaction(transactionId,LoggedInUser,UserType);
        return response;
    }

    public async Task<SprocMessage> UpdateComplianceRule(ComplianceRuleDetail list)
    {
        var response = await _complianceRule.UpdateComplianceRule(list);
        return response;
    }
}
