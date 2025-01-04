using Dapper;
using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerRegioster
{
    public class RegisterRepository : IRegisterRepository
    {
        public async Task<PartnerDetailSignup> GetPartnerDetail(string Email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", Email);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remitpartner_register]", param: param, commandType: CommandType.StoredProcedure);

            var detail = await data.ReadFirstAsync<PartnerDetailSignup>();
            var director = await data.ReadAsync<DirectorDetail>();
            var imageList = await data.ReadAsync<string>();
            detail.Directors = director.ToList();
            detail.LicensedocImgPath = imageList.ToList();

            return detail;
        }
        public async Task<PartnerDetailSignup> GetPartnerDetailById(string Id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", Id);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_remitpartner_register_byguidid]", param: param, commandType: CommandType.StoredProcedure);

            var detail = await data.ReadFirstAsync<PartnerDetailSignup>();
            var director = await data.ReadAsync<DirectorDetail>();
            var imageList = await data.ReadAsync<string>();
            detail.Directors = director.ToList();
            detail.LicensedocImgPath = imageList.ToList();

            return detail;
        }

        public async Task<SprocMessage> ValidateOtpAsync(string Email, string Opt)
        {


            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", Email);
            param.Add("@Otp", Opt);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_valid_partnerregister_otp]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
            }


            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> ResetOtpAsync(string Email, string Opt, DateTime expire)
        {


            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", Email);
            param.Add("@Otp", Opt);
            param.Add("@OtpExpiryDate", expire);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_remit_partners_register_updateotp]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
            }


            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> RegisterPartner(RegisterPartner partnerregister)
        {
            var dataTableRmp = DirectorTable();
            if (partnerregister.Directors != null)
            {
                foreach (var Director in partnerregister.Directors)
                {
                    var row = dataTableRmp.NewRow();
                    row["FirstName"] = Director.FirstName;
                    row["ContactNumber"] = Director.ContactNumber;
                    row["Email"] = Director.Email;

                    dataTableRmp.Rows.Add(row);
                }
            }
            var ImagedataTable = ImageTable();
            if (partnerregister.LicensedocImgPath != null && partnerregister.LicensedocImgPath.Count > 0)
            {
                foreach (var path in partnerregister.LicensedocImgPath)
                {
                    var row = ImagedataTable.NewRow();
                    row["DocumentImgPath"] = path;
                    ImagedataTable.Rows.Add(row);
                }
            }
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@Event", partnerregister.Event);
            param.Add("@FormNumber", partnerregister.FormNumber);
            param.Add("@OTP", partnerregister.OTP);
            param.Add("@OtpExipiryDate", partnerregister.OtpExipiryDate);
            param.Add("@PartnerCode", partnerregister.PartnerCode);
            param.Add("@FirstName", partnerregister.FirstName);
            //param.Add("@Shortname", partnerregister.ShortName);
            param.Add("@SurName", partnerregister.SurName);
            param.Add("@Withoutfirstname", partnerregister.Withoutfirstname);
            param.Add("@MobileNumber", partnerregister.MobileNumber);
            param.Add("@MobileConfirmed", partnerregister.MobileConfirmed);
            param.Add("@Email", partnerregister.Email);
            param.Add("@EmailConfirmed", partnerregister.EmailConfirmed);
            param.Add("@Post", partnerregister.Post);
            param.Add("@BusinessNumber", partnerregister.BusinessNumber);
            param.Add("@FinancialTransactionRegNo", partnerregister.FinancialTransactionRegNo);
            param.Add("@RemittancRegNumber", partnerregister.RemittancRegNumber);
            param.Add("@LicensedocImgPath", string.Empty);
            param.Add("@LicenseNumber", partnerregister.LicenseNumber);
            param.Add("@ZipCode", partnerregister.ZipCode);
            param.Add("@OrgState", partnerregister.OrgState);
            param.Add("@Address", partnerregister.Address);
            param.Add("@PasswordHash", partnerregister.PasswordHash);
            param.Add("@PasswordSalt", partnerregister.PasswordSalt);
            param.Add("@OrganizationName", partnerregister.OrganizationName);
            param.Add("@OrgEmail", partnerregister.OrgEmail);
            param.Add("@OrgEmailConfirmed", partnerregister.OrgEmailConfirmed);
            param.Add("@CountryCode", partnerregister.CountryCode);
            param.Add("@Callingcode", partnerregister.Callingcode);
            param.Add("@City", partnerregister.City);
            param.Add("@FullAddress", partnerregister.FullAddress);
            param.Add("@GMTTimeZone", partnerregister.GMTTimeZone);
            param.Add("@RegistrationNumber", partnerregister.RegistrationNumber);
            param.Add("@SourceCurrency", partnerregister.SourceCurrency);
            param.Add("@IpAddress", partnerregister.IpAddress);
            param.Add("@CompanyLogoImgPath", partnerregister.CompanyLogoImgPath);
            param.Add("@DocumentTypeId", partnerregister.DocumentTypeId);
            param.Add("@DocumentNumber", partnerregister.DocumentNumber);
            param.Add("@IdFrontImgPath", partnerregister.IdFrontImgPath);
            param.Add("@IdBackImgPath", partnerregister.IdBackImgPath);
            param.Add("@ExpiryDate", partnerregister.ExpiryDate);
            param.Add("@IssueDate", partnerregister.IssueDate);
            param.Add("@AddressProofTypeId", partnerregister.AddressProofTypeId);
            param.Add("@AddressProofImgPath", partnerregister.AddressProofImgPath);
            param.Add("@IsActive", partnerregister.IsActive);
            param.Add("@Maker", partnerregister.Maker);
            param.Add("@Checker", partnerregister.Checker);
            param.Add("@CreatedById", partnerregister.CreatedById);
            param.Add("@CreatedByName", partnerregister.CreatedByName);
            param.Add("@CreatedDate", partnerregister.CreatedDate);
            param.Add("@UpdatedById", partnerregister.UpdatedById);
            param.Add("@UpdatedByName", partnerregister.UpdatedByName);
            param.Add("@UpdatedDate", partnerregister.UpdatedDate);
            param.Add("@Directors", dataTableRmp.AsTableValuedParameter("[dbo].[PartnersDirectorsType]"));
            param.Add("@DocumentImgPaths", ImagedataTable.AsTableValuedParameter("[dbo].[DocumentImgs]"));


            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_remit_partners_register]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
            }


            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        private DataTable DirectorTable()
        {
            var dataTableRmp = new DataTable();
            dataTableRmp.Columns.Add("FirstName", typeof(string));
            dataTableRmp.Columns.Add("ContactNumber", typeof(string));
            dataTableRmp.Columns.Add("Email", typeof(string));

            return dataTableRmp;
        }
        private DataTable ImageTable()
        {
            var dataTableRmp = new DataTable();
            dataTableRmp.Columns.Add("DocumentImgPath", typeof(string));


            return dataTableRmp;
        }
    }
}
