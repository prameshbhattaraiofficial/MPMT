using AutoMapper;
using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Domain.Partners.Applications;
using Mpmt.Core.Domain.Public.Feedbacks;
using Mpmt.Core.Dtos.Logging;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Models.Agents;
using Mpmt.Core.Models.Partners.Applications;
using Mpmt.Core.Models.Public.Feedbacks;

namespace Mpmt.Services.Infrastructure
{
    /// <summary>
    /// The mapping profiles.
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfiles"/> class.
        /// </summary>
        public MappingProfiles()
        {
            // Mappings here
            CreateMap<AppUser, UserLoginActivity>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id))
                .ReverseMap();

            CreateMap<VendorApiLogParam, AppVendorApiLog>().ReverseMap();

            CreateMap<PartnerApplicationRequest, PartnerApplication>()
                .ReverseMap();

            CreateMap<PublicFeedbackRequest, PublicFeedback>()
                .ReverseMap();
            CreateMap<AgentRegister, AgentRegistration>()
                .ReverseMap();
        }
    }
}
