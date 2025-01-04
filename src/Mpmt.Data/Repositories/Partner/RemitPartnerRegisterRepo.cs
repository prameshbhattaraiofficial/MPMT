using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    public class RemitPartnerRegisterRepo : IRemitPartnerRegisterRepo
    {
        private readonly IMapper _mapper;

        public RemitPartnerRegisterRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<SprocMessage> ApprovedRejectPartnerRequest(RemitPartnerRequest remitPartnerRequest)
        {

            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", remitPartnerRequest.Id);
            param.Add("@shortName", remitPartnerRequest.shortName);
            param.Add("@LoggedInUser", remitPartnerRequest.LoggedInUser);
            param.Add("@UserType", remitPartnerRequest.UserType);
            param.Add("@OperationMode", remitPartnerRequest.OperationMode);
            param.Add("@Email", remitPartnerRequest.Email);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync(
                "[dbo].[usp_approvedreject_partnerregister]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText, IdentityVal = identityVal };
        }

        public async Task<PagedList<RemitPartnerRegister>> GetRemitPartnerAsync(RemitPartnerRegisterFilter request)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@FullName", request.FullName);
            param.Add("@MobileNo", request.MobileNo);
            param.Add("@Email", request.Email);

            param.Add("@PageNumber", request.PageNumber);
            param.Add("@PageSize", request.PageSize);
            param.Add("@SortingCol", request.SortBy);
            param.Add("@SortType", request.SortOrder);
            param.Add("@SearchVal", request.SearchVal);
            param.Add("@Export", request.Export);

            var resultSets = await connection
                .QueryMultipleAsync("[dbo].[sp_get_remit_partners_registerlist]", param: param, commandType: CommandType.StoredProcedure);

            var partnerRegister = await resultSets.ReadAsync<RemitPartnerRegister>();
            var pageInfo = await resultSets.ReadFirstAsync<PageInfo>();

            var resultData = _mapper.Map<PagedList<RemitPartnerRegister>>(pageInfo);
            resultData.Items = partnerRegister;

            return resultData;
        }
    }
}
