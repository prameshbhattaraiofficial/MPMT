using AutoMapper;
using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.AddressProofType;
using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.Banks;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.Currency;
using Mpmt.Core.Dtos.DocumentType;
using Mpmt.Core.Dtos.FundType;
using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Core.Dtos.KYCRemark;
using Mpmt.Core.Dtos.Occupation;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerBank;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmt.Core.Dtos.PaymentType;
using Mpmt.Core.Dtos.Relation;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Dtos.ServiceCharge;
using Mpmt.Core.Dtos.ServiceChargeCategory;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.Dtos.TransferPurpose;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.ViewModel.AdminReport;
using Mpmt.Core.ViewModel.AdminUser;
using Mpmt.Core.ViewModel.Bank;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmt.Core.ViewModel.Currency;
using Mpmt.Core.ViewModel.DocumentType;
using Mpmt.Core.ViewModel.FundType;
using Mpmt.Core.ViewModel.KYCRemark;
using Mpmt.Core.ViewModel.Occupation;
using Mpmt.Core.ViewModel.PartnerBank;
using Mpmt.Core.ViewModel.PaymentType;
using Mpmt.Core.ViewModel.Relation;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Core.ViewModel.ServiceCharge;
using Mpmt.Core.ViewModel.ServiceChargeCategory;
using Mpmt.Core.ViewModel.TransferPurpose;
using Mpmt.Core.ViewModel.User;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.AddressProofType;
using Mpmt.Web.Areas.Admin.ViewModels.Agent;
using Mpmt.Web.Areas.Admin.ViewModels.IncomeSource;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner.WalletCurrency;
using Mpmt.Web.Areas.Partner.Models.SendTransactions;
using Mpmt.Web.Areas.Partner.Models.TransferAmount;
using Mpmt.Web.Areas.Partner.ViewModels;


namespace Mpmt.Web.Infrastructure
{
    /// <summary>
    /// The web mapping profiles.
    /// </summary>
    public class WebMappingProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebMappingProfiles"/> class.
        /// </summary>
        public WebMappingProfiles()
        {

            CreateMap(typeof(PageInfo), typeof(PagedList<>)).ReverseMap();

            CreateMap<AppRole, AddRoleVm>().ReverseMap();
            CreateMap<AppRole, UpdateRoleVm>().ReverseMap();
            CreateMap<IUDUpdateWalletCurrency, FeeWalletVm>().ReverseMap();
            CreateMap<WalletCurrencyDetails, FeeWalletVm>().ReverseMap();

            CreateMap<PartnerRole, AddPartnerRoleVm>().ReverseMap();
            CreateMap<PartnerRole, UpdatePartnerRoleVm>().ReverseMap();
            CreateMap<PartnerRoleDetail, UpdatePartnerRoleVm>().ReverseMap();

            //CreateMap<List<AddServiceChargeVm>, List<ServiceChargeList>>().ReverseMap();
            CreateMap<AgentCredentialInsertRequest, AddApiKeysAgent>().ReverseMap();
            CreateMap<AgentCredentialInsertRequest, UpdateApiKeysAgent>().ReverseMap();
            CreateMap<AgentCredentialUpdateRequest, AgentCredential>().ReverseMap();
            CreateMap<AgentCredential, AgentCredentialUpdateRequest>().ReverseMap();


            //CurrencyMapped
            CreateMap<IUDCurrency, AddCurrencyVm>().ReverseMap();
            CreateMap<IUDCurrency, UpdateCurrencyVm>().ReverseMap();
            CreateMap<CurrencyDetails, UpdateCurrencyVm>().ReverseMap();

            //BankMapped
            CreateMap<IUDBank, AddBankVm>().ReverseMap();
            CreateMap<IUDBank, UpdateBankVm>().ReverseMap();
            CreateMap<BankDetails, UpdateBankVm>().ReverseMap();

            //OccupationMapped
            CreateMap<IUDOccupation, AddOccupationVm>().ReverseMap();
            CreateMap<IUDOccupation, UpdateOccupationVm>().ReverseMap();
            CreateMap<OccupationDetails, UpdateOccupationVm>().ReverseMap();

            //RelationMapped
            CreateMap<IUDRelation, AddRelationVm>().ReverseMap();
            CreateMap<IUDRelation, UpdateRelationVm>().ReverseMap();
            CreateMap<RelationDetails, UpdateRelationVm>().ReverseMap();

            //TransferPurposeMapped
            CreateMap<IUDTransferPurpose, AddTransferVm>().ReverseMap();
            CreateMap<IUDTransferPurpose, UpdateTransferVm>().ReverseMap();
            CreateMap<TransferPurposeDetails, UpdateTransferVm>().ReverseMap();

            //KYCRemarkMapped
            CreateMap<IUDKycRemark, AddKycRemarkVm>().ReverseMap();
            CreateMap<IUDKycRemark, UpdateKycRemarkVm>().ReverseMap();
            CreateMap<KycRemarkDetails, UpdateKycRemarkVm>().ReverseMap();

            //DocumentTypeMapped
            CreateMap<IUDDocumentType, AddDocumentTypeVm>().ReverseMap();
            CreateMap<IUDDocumentType, UpdateDocumentTypeVm>().ReverseMap();
            CreateMap<DocumentTypeDetails, UpdateDocumentTypeVm>().ReverseMap();

            //FundTypeMapped
            CreateMap<IUDFundType, AddFundTypeVm>().ReverseMap();
            CreateMap<IUDFundType, UpdateFundTypeVm>().ReverseMap();
            CreateMap<FundTypeDetails, UpdateFundTypeVm>().ReverseMap();

            //FundTypeMapped
            CreateMap<List<MenuSubMenu>, RMPermissionModel>().ReverseMap();
            CreateMap<MenuSubMenu, RMPermissionModel>().ReverseMap();
            

            //ServiceCategoryMapped
            CreateMap<IUDServiceCategory, AddServiceCategoryVm>().ReverseMap();
            CreateMap<IUDServiceCategory, UpdateServiceCategoryVm>().ReverseMap();
            CreateMap<ServiceCategoryDetails, UpdateServiceCategoryVm>().ReverseMap();

            //PartnerBankMapped
            CreateMap<IUDPartnerBank, AddPartnerBankVm>().ReverseMap();
            CreateMap<IUDPartnerBank, UpdatePartnerBankVm>().ReverseMap();
            CreateMap<PartnerBankDetails, UpdatePartnerBankVm>().ReverseMap();


            //ConversionRateMapped
            CreateMap<IUDConversionRate, AddConversionRateVm>().ReverseMap();
            CreateMap<IUDConversionRate, UpdateConversionRateVm>().ReverseMap();
            CreateMap<ConversionRateDetails, UpdateConversionRateVm>().ReverseMap();

            //PaymentTypeMapped
            CreateMap<IUDPaymentType, AddPaymentTypeVm>().ReverseMap();
            CreateMap<IUDPaymentType, UpdatePaymentTypeVm>().ReverseMap();
            CreateMap<PaymentTypeDetails, UpdatePaymentTypeVm>().ReverseMap();

            //ServiceChargeMapped
            CreateMap<AddServiceCharges, AddServiceChargeVm>().ReverseMap();
            // Mappings here

            //Partners
            CreateMap<AppPartner, AddPatnerRequest>().ReverseMap();
            CreateMap<PartnerCredentialInsertRequest, AddApiKeys>().ReverseMap();
            CreateMap<PartnerCredentialUpdateRequest, UpdateApiKeys>().ReverseMap();

            //registerPartner
            CreateMap<RegisterPartner, SignUpPartnerdetail>().ReverseMap();
            CreateMap<RegisterPartner, SignUpStep1>().ReverseMap();
            CreateMap<RegisterPartner, SignUpStep2>().ReverseMap();
            CreateMap<RegisterPartner, SignUpStep3>().ReverseMap();
            CreateMap<PartnerDetailSignup, SignUpStep2>().ReverseMap();
            CreateMap<PartnerDetailSignup, SignUpStep3>().ReverseMap();
            CreateMap<DirectorDetail, Core.Dtos.Partner.Director>().ReverseMap();


            CreateMap<UpdatePartnerrequest, AppPartner>().ReverseMap();
            //CreateMap<UpdateDirector, Director>().ReverseMap();
            CreateMap<Core.Dtos.Partner.Director, PartnerDirectors>().ReverseMap();


            CreateMap<PartnerVM, AppPartner>().ReverseMap();
            CreateMap<IUDUpdateWalletCurrency, PartnerWalletCurrencyVm>().ReverseMap();
            CreateMap<WalletCurrencyDetails, PartnerWalletCurrencyVm>().ReverseMap();
            CreateMap<AddUpdateFundRequest, AddUpdateFundRequestVm>().ReverseMap();
            //End of Partners

            CreateMap<AddServiceChargeVm, ServiceChargeList>().ReverseMap();
            CreateMap<AddPartnerConversionRateVm, AddPartnerConversionRate>().ReverseMap();
            CreateMap<AddPartnerConversionRateVm, PartnerConversionRate>().ReverseMap();
            CreateMap<AddPartnerConversionRateVm, PartnerConversionRateDetails>().ReverseMap();

            // Address Proof Type
            CreateMap<AddressProofTypeVm, IUDAddressProofType>().ReverseMap();
            CreateMap<AddressProofTypeVm, AddressProofTypeDetails>().ReverseMap();

            // Income Source
            CreateMap<IncomeSourceVm, IUDIncomeSource>().ReverseMap();
            CreateMap<IncomeSourceVm, IncomeSourceDetails>().ReverseMap();



            //End Of Prefund
          // CreateMap<CashAgentUserVm, AgentUser >().ReverseMap();
            //Sender
            CreateMap<UpdateUserVM, SenderDto>().ReverseMap();
            CreateMap<SenderAddUpdateDto, AddUserViewModel>().ReverseMap();
            CreateMap<SenderAddUpdateDto, UpdateUserVM>().ReverseMap();
            CreateMap<SenderAddUpdateDto, SenderDto>().ReverseMap();
            CreateMap<IUDPartnerEmployee, PartnerEmployeeList>().ReverseMap();

            //End of Sender

            //Agent
            CreateMap<CashAgentUser, CashAgentUserVm>().ReverseMap();
            CreateMap<CashAgentUser, CashAgentUpdateVm>().ReverseMap();
            CreateMap<AddAgentFundRequestVm, AddAgentFundRequest>().ReverseMap();
            //End of Agent


            //Partners
            CreateMap<RecipientAddUpdate, RecipientsAddUpdateVm>().ReverseMap();
            CreateMap<RecipientAddUpdate, RecipientsAddUpdateViewmodel>().ReverseMap();
            CreateMap<RecipientsAddUpdateVm, RecipientsList>().ReverseMap();
            CreateMap<GetConvertedTransferAmount, GetSendTransferAmountDetailRequest>().ReverseMap();

            CreateMap<GetSendTxnChargeAmountDetailsModel, GetSendTxnChargeAmountDetailsRequest>().ReverseMap();
            CreateMap<AddTransactionModel, AddTransactionDto>();
            CreateMap<AdminUserVm, IUDAdminUser>().ReverseMap();
            CreateMap<AppPartner, UserLoginActivity>().ForMember(d => d.UserId, o => o.MapFrom(s => s.Id)).ReverseMap();
            CreateMap<AppPartnerEmployee, UserLoginActivity>().ForMember(d => d.UserId, o => o.MapFrom(s => s.Id)).ReverseMap();

            //admin report
            CreateMap<AgentCommissionRule, AgentCommissionRuleType>().ReverseMap();
            CreateMap<AgentDefaultCommissionRule, AgentCommissionRuleType>().ReverseMap();
            CreateMap<ReceiverAccountDetails, ReceiverAccountDetailsVM>().ReverseMap();
            CreateMap<AgentFundApproveRejectModel, ApproveRejectFundTransferByAdmin>().ReverseMap();

            //bulk upload
            CreateMap<BulkTransactionDetailsModel, BulkTransactionResponse>().ReverseMap();

            //partner role
            CreateMap<AddAdminPartnerRoleVm, IUDPartnerRole>().ReverseMap();
            CreateMap<UpdateAdminPartnerRoleVm, IUDPartnerRole>().ReverseMap();
            CreateMap<PartnerRoleList, UpdateAdminPartnerRoleVm>().ReverseMap();

            //Wallet Adjustment Partner
            CreateMap<AdjustmentWallet, AdjustmentWalletDTO>().ReverseMap();
            CreateMap<ReceiverCashoutDetails, ReceiverAccountDetailsCashoutVM>().ReverseMap();  
        }
    }
}
