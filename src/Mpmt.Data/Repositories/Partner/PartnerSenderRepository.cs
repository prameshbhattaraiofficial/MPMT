using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Partners.Senders;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner
{
    /// <summary>
    /// The partner sender repository.
    /// </summary>
    public class PartnerSenderRepository : IPartnerSenderRepository
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerSenderRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public PartnerSenderRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the sender async.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddSenderAsync(SenderAddUpdateDto sender)
        {
            const string operationMode = "A";
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", sender.Id);
            param.Add("@PartnerCode", sender.PartnerCode);
            param.Add("@FirstName", sender.FirstName);
            param.Add("@SurName", sender.SurName);
            param.Add("@IsFirstNamePresent", sender.IsFirstNamePresent);
            param.Add("@MobileNumber", sender.MobileNumber);
            param.Add("@Email", sender.Email);
            param.Add("@GenderId", sender.GenderId);
            param.Add("@ProfileImagePath", sender.ProfileImagePath);
            param.Add("@DateOfBirth", sender.DateOfBirth);
            param.Add("@CountryCode", sender.CountryCode);
            param.Add("@Province", sender.Province);
            param.Add("@City", sender.City);
            param.Add("@Zipcode", sender.Zipcode);
            param.Add("@Address", sender.Address);
            param.Add("@OccupationId", sender.OccupationId);
            param.Add("@DocumentTypeId", sender.DocumentTypeId);
            param.Add("@DocumentNumber", sender.DocumentNumber);
            param.Add("@IssuedDate", sender.IssuedDate);
            param.Add("@ExpiryDate", sender.ExpiryDate);
            param.Add("@IdFrontImgPath", sender.IdFrontImgPath);
            param.Add("@IdBackImgPath", sender.IdBackImgPath);
            param.Add("@BankName", sender.BankName);
            param.Add("@BankCode", sender.BankCode);
            param.Add("@Branch", sender.Branch);
            param.Add("@AccountHolderName", sender.AccountHolderName);
            param.Add("@AccountNumber", sender.AccountNumber);
            param.Add("@IncomeSourceId", sender.IncomeSourceId);
            param.Add("@GMTTimeZone", sender.GMTTimeZone);
            param.Add("@IsActive", sender.IsActive);
            param.Add("@OperationMode", operationMode);
            param.Add("@LoggedInUser", sender.LoggedInUser);
            param.Add("@UserType", sender.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_sender_addupdate]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
            }


            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Gets the senders async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<SenderDto>> GetSendersAsync(SenderPagedRequest request)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", request.PartnerCode);
            param.Add("@FirstName", request.FirstName);
            param.Add("@SurName", request.SurName);
            param.Add("@MobileNumber", request.MobileNumber);
            param.Add("@Email", request.Email);
            param.Add("@UserStatus", request.UserStatus);
            param.Add("@PageNumber", request.PageNumber);
            param.Add("@PageSize", request.PageSize);
            param.Add("@SortingCol", request.SortBy);
            param.Add("@SortType", request.SortOrder);
            param.Add("@SearchVal", request.SearchVal);
            param.Add("@Export", request.Export);

            var resultSets = await connection
                .QueryMultipleAsync("[dbo].[usp_get_sender_list]", param: param, commandType: CommandType.StoredProcedure);

            var senders = await resultSets.ReadAsync<SenderDto>();
            var pageInfo = await resultSets.ReadFirstAsync<PageInfo>();

            var resultData = _mapper.Map<PagedList<SenderDto>>(pageInfo);
            resultData.Items = senders;

            return resultData;
        }
        public async Task<SenderDto> GetSenderByIdAsync(int SenderId,string PartnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@SenderId", SenderId);
            param.Add("@PartnerCode", PartnerCode);


            var resultSets = await connection
                .QueryFirstOrDefaultAsync<SenderDto>("[dbo].[usp_get_sender_byId]", param: param, commandType: CommandType.StoredProcedure);

            return resultSets;
        }

        /// <summary>
        /// Removes the sender async.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveSenderAsync(SenderAddUpdateDto sender)
        {
            const string operationMode = "D";
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", sender.Id);
            param.Add("@PartnerCode", sender.PartnerCode);
            param.Add("@FirstName", sender.FirstName);
            param.Add("@SurName", sender.SurName);
            param.Add("@IsFirstNamePresent", sender.IsFirstNamePresent);
            param.Add("@MobileNumber", sender.MobileNumber);
            param.Add("@Email", sender.Email);
            param.Add("@GenderId", sender.GenderId);
            param.Add("@ProfileImagePath", sender.ProfileImagePath);
            param.Add("@DateOfBirth", sender.DateOfBirth);
            param.Add("@CountryCode", sender.CountryCode);
            param.Add("@Province", sender.Province);
            param.Add("@City", sender.City);
            param.Add("@Zipcode", sender.Zipcode);
            param.Add("@Address", sender.Address);
            param.Add("@OccupationId", sender.OccupationId);
            param.Add("@DocumentTypeId", sender.DocumentTypeId);
            param.Add("@DocumentNumber", sender.DocumentNumber);
            param.Add("@IssuedDate", sender.IssuedDate);
            param.Add("@ExpiryDate", sender.ExpiryDate);
            param.Add("@IdFrontImgPath", sender.IdFrontImgPath);
            param.Add("@IdBackImgPath", sender.IdBackImgPath);
            param.Add("@BankName", sender.BankName);
            param.Add("@BankCode", sender.BankCode);
            param.Add("@Branch", sender.Branch);
            param.Add("@AccountHolderName", sender.AccountHolderName);
            param.Add("@AccountNumber", sender.AccountNumber);
            param.Add("@IncomeSourceId", sender.IncomeSourceId);
            param.Add("@GMTTimeZone", sender.GMTTimeZone);
            param.Add("@IsActive", sender.IsActive);
            param.Add("@OperationMode", operationMode);
            param.Add("@LoggedInUser", sender.LoggedInUser);
            param.Add("@UserType", sender.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_sender_addupdate]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the sender async.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateSenderAsync(SenderAddUpdateDto sender)
        {
            const string operationMode = "U";
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", sender.Id);
            param.Add("@PartnerCode", sender.PartnerCode);
            param.Add("@FirstName", sender.FirstName);
            param.Add("@SurName", sender.SurName);
            param.Add("@IsFirstNamePresent", sender.IsFirstNamePresent);
            param.Add("@MobileNumber", sender.MobileNumber);
            param.Add("@Email", sender.Email);
            param.Add("@GenderId", sender.GenderId);
            param.Add("@ProfileImagePath", sender.ProfileImagePath);
            param.Add("@DateOfBirth", sender.DateOfBirth);
            param.Add("@CountryCode", sender.CountryCode);
            param.Add("@Province", sender.Province);
            param.Add("@City", sender.City);
            param.Add("@Zipcode", sender.Zipcode);
            param.Add("@Address", sender.Address);
            param.Add("@OccupationId", sender.OccupationId);
            param.Add("@DocumentTypeId", sender.DocumentTypeId);
            param.Add("@DocumentNumber", sender.DocumentNumber);
            param.Add("@IssuedDate", sender.IssuedDate);
            param.Add("@ExpiryDate", sender.ExpiryDate);
            param.Add("@IdFrontImgPath", sender.IdFrontImgPath);
            param.Add("@IdBackImgPath", sender.IdBackImgPath);
            param.Add("@BankName", sender.BankName);
            param.Add("@BankCode", sender.BankCode);
            param.Add("@Branch", sender.Branch);
            param.Add("@AccountHolderName", sender.AccountHolderName);
            param.Add("@AccountNumber", sender.AccountNumber);
            param.Add("@IncomeSourceId", sender.IncomeSourceId);
            param.Add("@GMTTimeZone", sender.GMTTimeZone);
            param.Add("@IsActive", sender.IsActive);
            param.Add("@OperationMode", operationMode);
            param.Add("@LoggedInUser", sender.LoggedInUser);
            param.Add("@UserType", sender.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_sender_addupdate]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<IEnumerable<ExistingSender>> GetExistingSendersByPartnercodeAsync(string PartnerCode, string MemberId, string FullName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode", PartnerCode);
            param.Add("@MemberId", MemberId);
            param.Add("@FullName", FullName);

            var resultSets = await connection
                .QueryAsync<ExistingSender>("[dbo].[usp_get_existing_senders_by_partnercode]", param: param, commandType: CommandType.StoredProcedure);

            return resultSets;
        }
        public async Task<IEnumerable<ExistingRecipients>> GetExistingRecipientsByPartnercodeAsync(string MemberId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@MemberId", MemberId);

            var resultSets = await connection
                .QueryAsync<ExistingRecipients>("[dbo].[usp_get_existing_recipients_by_memberid]", param: param, commandType: CommandType.StoredProcedure);

            return resultSets;
        }
    }
}
