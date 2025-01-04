using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.PartnerEmployee
{
    public class PartnerEmployeeList
    {
        public int Id { get; set; }
        public string PartnerCode { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        public string MobileNumber { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Post { get; set; }
        public int GenderId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string AccessCodeHash { get; set; }
        public string AccessCodeSalt { get; set; }
        public string MPINHash { get; set; }
        public string MPINSalt { get; set; }
        public string LastIpAddress { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public int FailedLoginAttempt { get; set; }
        public DateTime? TemporaryLockedTillUtcDate { get; set; }
        public DateTime LastLoginDateUtc { get; set; }
        public DateTime LastActivityDateUtc { get; set; }
        public bool Is2FAAuthenticated { get; set; }
        public string AccountSecretKey { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
