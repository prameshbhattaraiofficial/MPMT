using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.Common
{
    /// <summary>
    /// The commonddl repo.
    /// </summary>
    public class CommonddlRepo : ICommonddlRepo
    {
        /// <summary>
        /// Gets the charge typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetChargeTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_chargetype_ddl]", commandType: CommandType.StoredProcedure);
        }
        //public async Task<IEnumerable<Commonddl>> GetFundTypeddl()
        //{
        //    using var connection = DbConnectionManager.GetDefaultConnection();

        //    return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_Fundtype_ddl]", commandType: CommandType.StoredProcedure);
        //}
        /// <summary>
        /// Gets the charge category typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetChargeCategoryTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_ChargeCategorytype_ddl]", commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the document typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<DocumentTypeddl>> GetDocumentTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<DocumentTypeddl>("[dbo].[usp_get_Document_type_ddl]", commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the address proof typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAddressProofTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_AddressProof_type_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the countryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetCountryddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_Country_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the currencyddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetCurrencyddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_Currency_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the fund typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetFundTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_fundtype_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the kyc remarksddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetKycRemarksddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_kycremarks_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the kyc statusddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetKycStatusddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_kycstatus_ddl]", commandType: CommandType.StoredProcedure);
        }


        /// <summary>
        /// Gets the payment typeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> GetPaymentTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commondropdown>("usp_get_PaymentType_ddl", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the service charge categoryddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetServiceChargeCategoryddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("usp_get_ServiceChargeCatagory_ddl", commandType: CommandType.StoredProcedure);
        }


        /// <summary>
        /// Gets the utc time zoneddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetUtcTimeZoneddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_utctime_ddl]", commandType: CommandType.StoredProcedure);
        }





        /// <summary>
        /// Gets the bankddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> GetBankddl()

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commondropdown>("[dbo].[usp_get_bank_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the relation shipddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetRelationShipddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_RelationShip_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the districtddl.
        /// </summary>
        /// <param name="ProvinceCode">The province code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetDistrictddl(string ProvinceCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ProvinceCode", ProvinceCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_district_ddl]", param: param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Getgenderddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getgenderddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_gender_ddl]", commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the income source.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetIncomeSource()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_Incomesource_ddl]", commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Getoccupations the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getoccupation()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_occupation_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Getprovinceddls the.
        /// </summary>
        /// <param name="Countrycode">The countrycode.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getprovinceddl(string Countrycode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Countrycode", Countrycode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_province_ddl]", param: param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Getlocallevelddls the.
        /// </summary>
        /// <param name="DistrictCode">The district code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getlocallevelddl(string DistrictCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@DistrictCode", DistrictCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_local_level_ddl]", param: param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Getmaritalstatusddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Getmaritalstatusddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_marital_status_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gettransferpurposeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> Gettransferpurposeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_transfer_purpose_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Getrecipienttypeddls the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> Getrecipienttypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commondropdown>("[dbo].[usp_get_recipient_type_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the calling codeddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commondropdown>> GetCallingCodeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commondropdown>("[dbo].[usp_get_Callingcode_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the walletddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetWalletddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_wallet_ddl]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the partner source currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetPartnerSourceCurrencyddl(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_PartnerSourceCurrency_ddl]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the partner destination currencyddl.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetPartnerDestinationCurrencyddl(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_PartnerDestinationCurrency_ddl]", param, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the admin roleddl.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAdminRoleddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_admin_roles_ddl]", commandType: CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Commonddl>> GetAgentEmployeeRoleddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_agentemployee_roles_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRoleddl(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_partner_roles_ddl]", param, commandType: CommandType.StoredProcedure);

        }

        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetParentMenu()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_parent_menu_ddl]", commandType: CommandType.StoredProcedure);

        }
        public async Task<IEnumerable<Commonddl>> GetPartnerParentMenu()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_partner_parent_menu_ddl]", commandType: CommandType.StoredProcedure);

        }
        /// <summary>
        /// Gets the parent menu.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetParentModule()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_parent_module_ddl]", commandType: CommandType.StoredProcedure);

        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetAction()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_action_ddl]", commandType: CommandType.StoredProcedure);

        }
        /// <summary>
        /// Gets the actions by module id.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<Commonddl>> GetActionsByModuleId(string ModuleId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", ModuleId);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_action_ByModuleId]", param, commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Commonddl>> GetPartnerModule()

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_partner_module_ddl]", commandType: CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Commonddl>> GetTransferDestinationCurrency(string PartnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", PartnerCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_gettransfermodule_destinationcurrency]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetTransferModuleDestinationCurrencyddl(string partnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@PartnerCode", partnerCode);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_gettransfermodule_destinationcurrency]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<GetDocumentCharcterModel> GetDocumentCharacterByDocumentType(string DocumentTypeCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@DocumentTypeCode", DocumentTypeCode);
            return await connection.QueryFirstOrDefaultAsync<GetDocumentCharcterModel>("[dbo].[usp_GetDocumentCharaterBy_DocumentType]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetFrequency()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_date_frequency]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_employee_Roles_by_Id]", param, commandType: CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Commonddl>> GetAgentEmployeeRolesByIdAsync(int id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_agentemployee_roles_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetNotificationModuleRolesByModuleIdAsync(int moduleId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@ModuleId", moduleId);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_module_roles_by_moduleId]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetAllDistrictddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_all_district_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetAgentParentMenu()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_parent_agentmenu_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetAgentListddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_agentlist_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetAdminPartnerRoleddl(int Id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", Id);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_admin_partner_role_ddl]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetAdminPartnerRoleddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_admin_partner_roles_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRolesByIdAsync(int Id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", Id);
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_partner_roles_by_Id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetPartnerRolesddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_admin_partner_roles_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetStatusListDdl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_request_status_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> GetTransactionStatusddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_TransactionStatus_ddl]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Commonddl>> getTransactionTypeddl()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<Commonddl>("[dbo].[usp_get_TransactionStatus_ddl]", commandType: CommandType.StoredProcedure);
        }
    }
}
