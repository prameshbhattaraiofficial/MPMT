using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Agent
{
    public class AgentWithCredentials
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        public string MobileNumber { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string AccessCodeHash { get; set; }
        public string AccessCodeSalt { get; set; }
        public Guid UserGuid { get; set; }
        public string MPINHash { get; set; }
        public string MPINSalt { get; set; }
        public string OrganizationName { get; set; }
        public string OrgEmail { get; set; }
        public bool OrgEmailConfirmed { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LastIpAddress { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public int FailedLoginAttempt { get; set; }
        public DateTime? TemporaryLockedTillUtcDate { get; set; }
        public DateTime? LastLoginDateUtc { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }
        public string KycStatusCode { get; set; }
        public bool Is2FAAuthenticated { get; set; }
        public string AccountSecretKey { get; set; }
        public string CredentialId { get; set; }
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string ApiKey { get; set; }
        public string SystemPrivateKey { get; set; }
        public string SystemPublicKey { get; set; }
        public string UserPrivateKey { get; set; }
        public string UserPublicKey { get; set; }
        public string IPAddress { get; set; }
        public string AgentCode { get; set; }
    }
}
