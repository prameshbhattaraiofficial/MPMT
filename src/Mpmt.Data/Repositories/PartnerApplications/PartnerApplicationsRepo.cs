using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerApplications;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerApplications
{
    public class PartnerApplicationsRepo : IPartnerApplicationsRepo
    {
        private readonly IMapper _mapper;

        public PartnerApplicationsRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<PartnerApplicationsModel>> GetPartnerApplicationsAsync(PartnerApplicationsFilter requestFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@FullName", requestFilter.FullName);
            param.Add("@OrganizationName", requestFilter.OrganizationName);
            param.Add("@OrganizationEmail", requestFilter.OrganizationEmail);
            param.Add("@OrganizationContactNo", requestFilter.OrganizationContactNo);
            param.Add("@StartDate", requestFilter.StartDate);
            param.Add("@EndDate", requestFilter.EndDate);

            param.Add("@PageNumber", requestFilter.PageNumber);
            param.Add("@PageSize", requestFilter.PageSize);
            param.Add("@SortingCol", requestFilter.SortBy);
            param.Add("@SortType", requestFilter.SortOrder);
            param.Add("@SearchVal", requestFilter.SearchVal);
            param.Add("@Export", requestFilter.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_partner_applications]", param: param, commandType: CommandType.StoredProcedure);

            var partnerApplications = await data.ReadAsync<PartnerApplicationsModel>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<PartnerApplicationsModel>>(pagedInfo);
            mappeddata.Items = partnerApplications;
            return mappeddata;
        }

        public async Task<PagedList<PublicFeedbacksModel>> GetPublicFeedbacksAsync(PublicFeedbacksFilter requestFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@FullName", requestFilter.FullName);
            param.Add("@Email", requestFilter.Email);
            param.Add("@ContactNo", requestFilter.ContactNo);
            param.Add("@StartDate", requestFilter.StartDate);
            param.Add("@EndDate", requestFilter.EndDate);

            param.Add("@PageNumber", requestFilter.PageNumber);
            param.Add("@PageSize", requestFilter.PageSize);
            param.Add("@SortingCol", requestFilter.SortBy);
            param.Add("@SortType", requestFilter.SortOrder);
            param.Add("@SearchVal", requestFilter.SearchVal);
            param.Add("@Export", requestFilter.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_public_feedbacks]", param: param, commandType: CommandType.StoredProcedure);

            var publicFeedbacks = await data.ReadAsync<PublicFeedbacksModel>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<PublicFeedbacksModel>>(pagedInfo);
            mappeddata.Items = publicFeedbacks;
            return mappeddata;
        }
    }
}
