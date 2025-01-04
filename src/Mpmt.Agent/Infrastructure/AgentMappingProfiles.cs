using AutoMapper;
using Mpmt.Agent.Models.FundTransfer;
using Mpmt.Agent.Models.TransactionSearch;
using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Core.ViewModel.SuperAgent;
using Mpmt.Services.Services.AgentApplications.AgentFundTransfer;

namespace Mpmt.Agent.Infrastructure
{
    public class AgentMappingProfiles : Profile
    {
        public AgentMappingProfiles()
        {
            CreateMap(typeof(PageInfo), typeof(PagedList<>)).ReverseMap();
            CreateMap<CashAgentUser, CashAgentUserVm>().ReverseMap();
            CreateMap<CashAgentVm, CashAgentUserVm>().ReverseMap();
            CreateMap<AgentLoginActivity, AgentUser>().ReverseMap();
            CreateMap<CashAgentEmployeeVm, CashAgentUser>().ReverseMap();
            CreateMap<AgentPayOutReceipt, AgentPayOutReceiptModel>().ReverseMap();
            CreateMap<AgentFundTransferDto, FundTransferModel>().ReverseMap();
            CreateMap<AppRole, UpdateRoleVm>().ReverseMap();
            CreateMap<AppRole, AddRoleVm>().ReverseMap();
            CreateMap<SignUpAgentStep2, RegisterAgent>().ReverseMap();
            CreateMap<SignUpAgent, RegisterAgent>().ReverseMap();
            CreateMap<RegisterAgent, AgentDetailSignUp>().ReverseMap();
            //CreateMap<AgentDetailSignUp, SignUpAgentStep2>().ReverseMap();
            CreateMap<AgentCommissionRule, AgentCommissionRuleType>().ReverseMap();
            CreateMap<AgentDefaultCommissionRule, AgentCommissionRuleType>().ReverseMap();

        }
    }
}
