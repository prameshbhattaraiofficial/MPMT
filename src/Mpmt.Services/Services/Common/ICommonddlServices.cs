using Mpmt.Core.Dtos.DropDown;
using System.Collections;

namespace Mpmt.Services.Services.Common
{
    /// <summary>
    /// The commonddl services.
    /// </summary>
    public interface ICommonddlServices
    {
        /// <summary>
        /// Gets the countryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetCountryddl();
        /// <summary>
        /// Gets the address proof typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetAddressProofTypeddl();
        /// <summary>
        /// Gets the document typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<DocumentTypeddl>> GetDocumentTypeddl();
        /// <summary>
        /// Gets the charge typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetChargeTypeddl();
        Task<IEnumerable<Commonddl>> GetAgentListddl();
        /// <summary>
        /// Gets the charge category typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetChargeCategoryTypeddl();
        /// <summary>
        /// Gets the kyc remarksddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetKycRemarksddl();
        /// <summary>
        /// Gets the kyc statusddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetKycStatusddl();
        /// <summary>
        /// Gets the utc time zoneddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetUtcTimeZoneddl();
        Task<IEnumerable<Commonddl>> GetTransactionTypeddl();
        /// <summary>
        /// Gets the currencyddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetCurrencyddl();
        Task<IEnumerable<Commonddl>> GetTransactionStatusddl();

        /// <summary>
        /// Gets the service charge categoryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetServiceChargeCategoryddl();
        /// <summary>
        /// Gets the payment typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commondropdown>> GetPaymentTypeddl();


        /// <summary>
        /// Gets the fund typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetFundTypeddl();



        // Task<IEnumerable<Commonddl>> GetBankddl();


        /// <summary>
        /// Gets the bankddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commondropdown>> GetBankddl();
        /// <summary>
        /// Gets the income source asyncddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetIncomeSourceAsyncddl();
        /// <summary>
        /// Getoccupations the asyncddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetoccupationAsyncddl();


        /// <summary>
        /// Gets the relation shipddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetRelationShipddl();
        /// <summary>
        /// Gets the districtddl.
        /// </summary>
        /// <param name="ProvinceCode">The province code.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetDistrictddl(string ProvinceCode);
        Task<IEnumerable<Commonddl>> GetAllDistrictddl();
        /// <summary>
        /// Getgenderddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> Getgenderddl();
        /// <summary>
        /// Getprovinceddls the.
        /// </summary>
        /// <param name="Countrycode">The countrycode.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> Getprovinceddl(string Countrycode);
        /// <summary>
        /// Getlocallevelddls the.
        /// </summary>
        /// <param name="DistrictCode">The district code.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> Getlocallevelddl(string DistrictCode);
        /// <summary>
        /// Getmaritalstatusddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> Getmaritalstatusddl();
        /// <summary>
        /// Gettransferpurposeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> Gettransferpurposeddl();
        /// <summary>
        /// Getrecipienttypeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commondropdown>> Getrecipienttypeddl();
        /// <summary>
        /// Gets the calling codeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commondropdown>> GetCallingCodeddl();
        /// <summary>
        /// Gets the walletddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetWalletddl();
        /// <summary>
        /// Gets the partner source currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetPartnerSourceCurrencyddl(string partnerCode);
        /// <summary>
        /// Gets the partner destination currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetPartnerDestinationCurrencyddl(string partnerCode);

      

        Task<IEnumerable<Commonddl>> GetTransferModuleDestinationCurrencyddl(string partnerCode);

            
        /// <summary>
        /// Gets the admin roleddl.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetAdminRoleddl();
        Task<IEnumerable<Commonddl>> GetPartnerRolesddl();  
        Task<IEnumerable<Commonddl>> GetAgentEmployeeRoleddl();
        Task<IEnumerable<Commonddl>> GetPartnerRoleddl(string partnerCode = "");
        Task<IEnumerable<Commonddl>> GetAdminPartnerRoleddl(int Id);    
        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetParentMenu();
        Task<IEnumerable<Commonddl>> GetAgentParentMenu();
        Task<IEnumerable<Commonddl>> GetPartnerParentMenu();
        /// <summary>
        /// Gets the parent module.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetParentModule();

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetAction();

        /// <summary>
        /// Gets the actions by module id.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<Commonddl>> GetActionsByModuleId(string ModuleId);

        Task<IEnumerable<Commonddl>> GetPartnerModule();

        Task<IEnumerable<Commonddl>> GetTransferDestinationCurrency(string PartnerCode);

        Task<GetDocumentCharcterModel> GetDocumentCharacterByDocumentType(string DocumentTypeCode);
        Task<IEnumerable<Commonddl>> GetFrequency();
        Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id);
        Task<IEnumerable<Commonddl>> GetPartnerRolesByIdAsync(int Id);  
        Task<IEnumerable<Commonddl>> GetAgentEmployeeRolesByIdAsync(int id);
        Task<IEnumerable<Commonddl>> GetNotificationModuleRolesByModuleIdAsync(int moduleId);
        Task<IEnumerable<Commonddl>> GetStatusListDdl();
    }
}
