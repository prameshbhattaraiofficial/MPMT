using AutoMapper;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.PublicApi.Infrastructure
{
    /// <summary>
    /// The api mapping profiles.
    /// </summary>
    public class ApiMappingProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiMappingProfiles"/> class.
        /// </summary>
        public ApiMappingProfiles()
        {
            // Mappings here
            CreateMap(typeof(PageInfo), typeof(PagedList<>)).ReverseMap();
        }
    }
}