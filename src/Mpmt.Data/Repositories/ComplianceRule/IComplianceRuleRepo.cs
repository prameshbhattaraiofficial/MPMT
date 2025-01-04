using Mpmt.Core.Dtos.ComplianceRule;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.ComplianceRule;

public interface IComplianceRuleRepo
{
    Task<SprocMessage> AddComplianceCountryList(string countryListString);
    Task<IEnumerable<CountryComplianceRule>> GetAllCountryList();
    Task<IEnumerable<CountryComplianceRule>> GetComplianceCountryList();
    Task<PagedList<ComplianceRuleList>> GetComplianceRuleAsync(ComplianceRuleFilter filter);
    Task<PagedList<RemitTxnReport>> GetComplianceTransactionAsync(RemitTxnReportFilter txnFilter);
    Task<IEnumerable<Commonddl>> GetFrequency();
    Task<SprocMessage> ReleaseTransaction(string transactionId,string LoggedInuser,string UserType);
    Task<SprocMessage> UpdateComplianceRule(ComplianceRuleDetail list);
}
