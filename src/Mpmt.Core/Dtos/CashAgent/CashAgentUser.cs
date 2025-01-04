using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class CashAgentUser
    {
        public string Event { get; set; }
        public string EmployeeId { get; set; }
        public string AgentCode { get; set; }
        public string SuperAgentCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ContactNumber { get; set; }
        public bool ContactNumberConfirmed { get; set; }
        public string LookupName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string AccessCodeHash { get; set; }
        public string AccessCodeSalt { get; set; }
        public string MPINHash { get; set; }
        public string MPINSalt { get; set; }
        public string OrganizationName { get; set; }
        public string OrgEmail { get; set; }
        public bool OrgEmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string LocalLevelCode { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string GMTTimeZone { get; set; }
        public string RegistrationNumber { get; set; }
        public string CompanyLogoImgPath { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string IdFrontImgPath { get; set; }
        public string IdBackImgPath { get; set; }
        public string ExpiryDate { get; set; }
        public string AddressProofTypeId { get; set; }
        public string AddressProofImgPath { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrefunding { get; set; }  
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public int FailedLoginAttempt { get; set; }
        public string TemporaryLockedTillUtcDate { get; set; }
        public string LastLoginDateUtc { get; set; }
        public string LastActivityDateUtc { get; set; }
        public string KycStatusCode { get; set; }
        public bool Is2FAAuthenticated { get; set; }
        public string AccountSecretKey { get; set; }
        public string LicenseDocImgPath { get; set; }
        public string AgentCategoryId { get; set; }
        public string UserType { get; set; }
        public string LoginUserName { get; set; }
        public string Remarks { get; set; } 
        public string LoginUserId { get; set; }
        public bool Is2FARequired { get; set; }
        public List<string> LicensedocImgPath { get; set; } = new List<string>();
    }
}
