using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    /// <summary>
    /// The partner recipent repo.
    /// </summary>
    public class PartnerRecipentRepo : IPartnerRecipentRepo
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerRecipentRepo"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public PartnerRecipentRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the update recipient async.
        /// </summary>
        /// <param name="recipientAdd">The recipient add.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddUpdateRecipientAsync(RecipientAddUpdate recipientAdd)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            try
            {
                var param = new DynamicParameters();
                param.Add("@OperationMode", recipientAdd.OperationMode);
                param.Add("@Id", recipientAdd.Id);
                param.Add("@SenderId", recipientAdd.SenderId);
                param.Add("@FirstName", recipientAdd.FirstName);
                param.Add("@SurName", recipientAdd.SurName);
                param.Add("@IsSurNamePresent", recipientAdd.IsSurNamePresent);
                param.Add("@MobileNumber", recipientAdd.MobileNumber);
                param.Add("@Email", recipientAdd.Email);
                param.Add("@GenderId", recipientAdd.GenderId);
                param.Add("@DateOfBirth", recipientAdd.DateOfBirth);
                param.Add("@CountryCode", recipientAdd.CountryCode);
                param.Add("@ProvinceCode", recipientAdd.ProvinceCode);
                param.Add("@DistrictCode", recipientAdd.DistrictCode);
                param.Add("@LocalBodyCode", recipientAdd.LocalBodyCode);
                param.Add("@City", recipientAdd.City);
                param.Add("@Zipcode", recipientAdd.Zipcode);
                param.Add("@Address", recipientAdd.Address);
                param.Add("@SourceCurrency", recipientAdd.SourceCurrency);
                param.Add("@DestinationCurrency", recipientAdd.DestinationCurrency);
                param.Add("@RelationshipId", recipientAdd.RelationshipId);
                param.Add("@PayoutTypeId", recipientAdd.PayoutTypeId);
                param.Add("@BankName", recipientAdd.BankName);
                param.Add("@BankCode", recipientAdd.BankCode);
                param.Add("@Branch", recipientAdd.Branch);
                param.Add("@AccountHolderName", recipientAdd.AccountHolderName);
                param.Add("@AccountNumber", recipientAdd.AccountNumber);
                param.Add("@WalletName", recipientAdd.WalletName);
                param.Add("@WalletId", recipientAdd.WalletId);
                param.Add("@WalletRegisteredName", recipientAdd.WalletRegisteredName);
                param.Add("@GMTTimeZone", recipientAdd.GMTTimeZone);
                param.Add("@IsActive", recipientAdd.IsActive);
                param.Add("@LoggedInUser", recipientAdd.LoggedInuser);
                param.Add("@UserType", recipientAdd.UserType);


                param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_recipient_addupdate]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@ReturnPrimaryId");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the recipients async.
        /// </summary>
        /// <param name="recipientFilter">The recipient filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<RecipientsList>> GetRecipientsAsync(RecipientFilter recipientFilter, string partnercode = "")
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@SenderId", recipientFilter.SenderId);
            param.Add("@partnerCode", partnercode);
            param.Add("@FirstName", recipientFilter.FirstName);
            param.Add("@SurName", recipientFilter.SurName);
            param.Add("@MobileNumber", recipientFilter.MobileNumber);
            param.Add("@Email", recipientFilter.Email);
            param.Add("@UserStatus", recipientFilter.UserStatus);
            param.Add("@PageNumber", recipientFilter.PageNumber);
            param.Add("@PageSize", recipientFilter.PageSize);
            param.Add("@SortingCol", recipientFilter.SortBy);
            param.Add("@SortType", recipientFilter.SortOrder);
            param.Add("@SearchVal", recipientFilter.SearchVal);
            param.Add("@Export", recipientFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_recipients_list_by_senderid]", param: param, commandType: CommandType.StoredProcedure);

            var Result = await data.ReadAsync<RecipientsList>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<RecipientsList>>(pagedInfo);
            mappeddata.Items = Result;
            return mappeddata;
        }

        /// <summary>
        /// Gets the recipients by id async.
        /// </summary>
        /// <param name="recipientid">The recipientid.</param>
        /// <returns>A Task.</returns>
        public async Task<RecipientsList> GetRecipientsByIdAsync(int recipientid)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", recipientid);

            var data = await connection
                .QueryFirstOrDefaultAsync<RecipientsList>("[dbo].[usp_get_recipients_byId]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}
