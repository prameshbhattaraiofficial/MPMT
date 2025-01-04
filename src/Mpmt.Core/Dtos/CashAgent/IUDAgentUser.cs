using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.CashAgent
{
    public class IUDAgentUser
    {
        public string Event { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Guid UserGuid { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
        public string MobileNumber { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string AccessCodeHash { get; set; }
        public string AccessCodeSalt { get; set; }
        public int FailedLoginAttempt { get; set; }
        public string ProfileImageUrlPath { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string LastIpAddress { get; set; }
        public string DeviceId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
        public bool Is2FAAuthenticated { get; set; }
        public string AccountSecretKey { get; set; }
        public int RoleId { get; set; }
        public string LoggedInUser { get; set; }
    }
}
