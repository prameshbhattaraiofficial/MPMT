using Mpmt.Core.Dtos.ComplianceRule;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.ComplianceRule;

public interface IComplianceRuleService
{
    Task<SprocMessage> AddComplianceCountryList(string countryListString);
    Task<IEnumerable<CountryComplianceRule>> GetAllCountryList();
    Task<IEnumerable<CountryComplianceRule>> GetComplianceCountryList();
    Task<PagedList<ComplianceRuleList>> GetComplianceRuleAsync(ComplianceRuleFilter filter);
    Task<PagedList<RemitTxnReport>> GetComplianceTransactionAsync(RemitTxnReportFilter reportFilter);
    Task<IEnumerable<Commonddl>> GetFrequency();
    Task<SprocMessage> ReleaseTransaction(string transactionId,ClaimsPrincipal User);
    Task<SprocMessage> UpdateComplianceRule(ComplianceRuleDetail list);
}
