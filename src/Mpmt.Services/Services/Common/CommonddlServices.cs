using DocumentFormat.OpenXml.Office2010.Excel;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Repositories.Common;

namespace Mpmt.Services.Services.Common
{
    /// <summary>
    /// The commonddl services.
    /// </summary>
    public class CommonddlServices : BaseService, ICommonddlServices
    {
        private readonly ICommonddlRepo _commonddl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonddlServices"/> class.
        /// </summary>
        /// <param name="commonddl">The commonddl.</param>
        public CommonddlServices(ICommonddlRepo commonddl)
        {
            _commonddl = commonddl;
        }

        /// <summary>
        /// Gets the charge typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetChargeTypeddl()
        {
            var data = await _commonddl.GetChargeTypeddl();

            return data;
        }
        /// <summary>
        /// Gets the fund typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetFundTypeddl()
        {
            var data = await _commonddl.GetFundTypeddl();
            return data;
        }
        /// <summary>
        /// Gets the charge category typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetChargeCategoryTypeddl()
        {
            var data = await _commonddl.GetChargeCategoryTypeddl();

            return data;
        }

        /// <summary>
        /// Gets the countryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetCountryddl()
        {
            var data = await _commonddl.GetCountryddl();
            return data;
        }
        /// <summary>
        /// Gets the document typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<DocumentTypeddl>> GetDocumentTypeddl()
        {
            var data = await _commonddl.GetDocumentTypeddl();
            return data;
        }
        /// <summary>
        /// Gets the address proof typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAddressProofTypeddl()
        {
            var data = await _commonddl.GetAddressProofTypeddl();
            return data;
        }

        /// <summary>
        /// Gets the currencyddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetCurrencyddl()
        {
            var data = await _commonddl.GetCurrencyddl();
            return data;
        }


        /// <summary>
        /// Gets the kyc remarksddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetKycRemarksddl()
        {
            var data = await _commonddl.GetKycRemarksddl();
            return data;
        }

        /// <summary>
        /// Gets the kyc statusddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetKycStatusddl()
        {
            var data = await _commonddl.GetKycStatusddl();
            return data;
        }


        /// <summary>
        /// Gets the payment typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> GetPaymentTypeddl()

        {
            var data = await _commonddl.GetPaymentTypeddl();
            return data;
        }

        /// <summary>
        /// Gets the service charge categoryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetServiceChargeCategoryddl()
        {
            var data = await _commonddl.GetServiceChargeCategoryddl();
            return data;
        }

        /// <summary>
        /// Gets the utc time zoneddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetUtcTimeZoneddl()
        {
            var data = await _commonddl.GetUtcTimeZoneddl();
            return data;
        }


        /// <summary>
        /// Gets the bankddl.
        /// </summary>
        /// <returns>A Task.</returns>

        public async Task<IEnumerable<Commondropdown>> GetBankddl()

        {
            var data = await _commonddl.GetBankddl();
            return data;
        }

        /// <summary>
        /// Gets the relation shipddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetRelationShipddl()
        {
            var data = await _commonddl.GetRelationShipddl();
            return data;
        }

        /// <summary>
        /// Gets the districtddl.
        /// </summary>
        /// <param name="ProvinceCode">The province code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetDistrictddl(string ProvinceCode)
        {
            var data = await _commonddl.GetDistrictddl(ProvinceCode);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAllDistrictddl()
        {
            var data = await _commonddl.GetAllDistrictddl();
            return data;
        }

        /// <summary>
        /// Getgenderddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getgenderddl()
        {
            var data = await _commonddl.Getgenderddl();
            return data;
        }
        /// <summary>
        /// Gets the income source asyncddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetIncomeSourceAsyncddl()
        {
            var data = await _commonddl.GetIncomeSource();
            return data;
        }
        /// <summary>
        /// Getoccupations the asyncddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetoccupationAsyncddl()
        {
            var data = await _commonddl.Getoccupation();
            return data;
        }

        /// <summary>
        /// Getprovinceddls the.
        /// </summary>
        /// <param name="Countrycode">The countrycode.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getprovinceddl(string Countrycode)
        {
            var data = await _commonddl.Getprovinceddl(Countrycode);
            return data.ToList();
        }

        /// <summary>
        /// Getlocallevelddls the.
        /// </summary>
        /// <param name="DistrictCode">The district code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getlocallevelddl(string DistrictCode)
        {
            var data = await _commonddl.Getlocallevelddl(DistrictCode);
            return data;
        }

        /// <summary>
        /// Getmaritalstatusddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getmaritalstatusddl()
        {
            var data = await _commonddl.Getmaritalstatusddl();
            return data;
        }

        /// <summary>
        /// Gettransferpurposeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Gettransferpurposeddl()
        {
            var data = await _commonddl.Gettransferpurposeddl();
            return data;
        }

        /// <summary>
        /// Getrecipienttypeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> Getrecipienttypeddl()
        {
            var data = await _commonddl.Getrecipienttypeddl();
            return data;
        }

        /// <summary>
        /// Gets the calling codeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> GetCallingCodeddl()
        {
            var data = await _commonddl.GetCallingCodeddl();

            return data;
        }

        /// <summary>
        /// Gets the walletddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetWalletddl()
        {
            var data = await _commonddl.GetWalletddl();
            return data;
        }

        /// <summary>
        /// Gets the partner source currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetPartnerSourceCurrencyddl(string partnerCode)
        {
            var data = await _commonddl.GetPartnerSourceCurrencyddl(partnerCode);
            return data;
        }

        //public async Task<IEnumerable<Commonddl>> GetPartnerDestinationCurrencyddl(string partnerCode)
        //{
        //    var data = await _commonddl.GetPartnerDestinationCurrencyddl(partnerCode);
        //}
        /// <summary>
        /// Gets the utc time zoneddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAdminRoleddl()
        {
            var data = await _commonddl.GetAdminRoleddl();

            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAgentEmployeeRoleddl()
        {
            var data = await _commonddl.GetAgentEmployeeRoleddl();
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRoleddl(string partnerCode)
        {
            var data = await _commonddl.GetPartnerRoleddl(partnerCode);

            return data;
        }

        /// <summary>
        /// Gets the partner destination currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetPartnerDestinationCurrencyddl(string partnerCode)
        {
            var data = await _commonddl.GetPartnerDestinationCurrencyddl(partnerCode);
            return data;
        }
        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetParentMenu()
        {
            var data = await _commonddl.GetParentMenu();

            return data;
        }
        public async Task<IEnumerable<Commonddl>> GetPartnerParentMenu()
        {
            var data = await _commonddl.GetPartnerParentMenu();

            return data;
        }
        /// <summary>
        /// Gets the parent module.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetParentModule()
        {
            var data = await _commonddl.GetParentModule();

            return data;
        }
        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAction()
        {
            var data = await _commonddl.GetAction();

            return data;
        }

        /// <summary>
        /// Gets the actions by module id.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetActionsByModuleId(string ModuleId)
        {
            var data = await _commonddl.GetActionsByModuleId(ModuleId);

            return data;
        }
        public async Task<IEnumerable<Commonddl>> GetPartnerModule()
        {
            var data = await _commonddl.GetPartnerModule();

            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetTransferDestinationCurrency(string PartnerCode)
        {
            var data = await _commonddl.GetTransferDestinationCurrency(PartnerCode);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetTransferModuleDestinationCurrencyddl(string partnerCode)
        {
            var data = await _commonddl.GetTransferModuleDestinationCurrencyddl(partnerCode);
            return data;    
        }
        public async Task<GetDocumentCharcterModel> GetDocumentCharacterByDocumentType(string DocumentTypeCode)
        {
            var data = await _commonddl.GetDocumentCharacterByDocumentType(DocumentTypeCode);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetFrequency()
        {
            var data = await _commonddl.GetFrequency();
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id)
        {
            var data = await _commonddl.GetPartnerEmployeeRolesByIdAsync(id);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAgentEmployeeRolesByIdAsync(int id)
        {
            var data = await _commonddl.GetAgentEmployeeRolesByIdAsync(id);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetNotificationModuleRolesByModuleIdAsync(int moduleId)
        {
            var data = await _commonddl.GetNotificationModuleRolesByModuleIdAsync(moduleId);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAgentParentMenu()
        {
            var data = await _commonddl.GetAgentParentMenu();
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAgentListddl()
        {
            var data = await _commonddl.GetAgentListddl();
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetAdminPartnerRoleddl(int Id)
        {
            var data = await _commonddl.GetAdminPartnerRoleddl(Id);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRolesByIdAsync(int Id)
        {
            var data = await _commonddl.GetPartnerRolesByIdAsync(Id);
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRolesddl()
        {
            var data = await _commonddl.GetPartnerRolesddl();
            return data;    
        }

        public async Task<IEnumerable<Commonddl>> GetStatusListDdl()
        {
            var data = await _commonddl.GetStatusListDdl();
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetTransactionStatusddl()
        {
            var data = await _commonddl.GetTransactionStatusddl();  
            return data;
        }

        public async Task<IEnumerable<Commonddl>> GetTransactionTypeddl()
        {
            var data = await _commonddl.getTransactionTypeddl();
            return data;
        }
    }
}
