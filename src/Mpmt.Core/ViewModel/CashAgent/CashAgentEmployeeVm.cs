using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Common.Attribites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.CashAgent
{
    public class CashAgentEmployeeVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        //[Remote(action: "VerifyUserName", controller: "AdminUser")]
        //[Remote(action: "VerifyUserName", controller: "AdminUser")]
        [Remote(action: "VerifyUserName", controller: "EmployeeManagement")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string SuperAgentCode { get; set; }

        [Required(ErrorMessage = "last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        //[Remote(action: "VerifyEmail", controller: "AdminUser")]
        public string Email { get; set; }
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
        public string ContactNumber { get; set; }

        //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long.", MinimumLength = 12)]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
        public string ConfirmPassword { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile ProfileImage { get; set; }
        public string Event { get; set; }
        public string LicenseDocImgPath { get; set; }
        public bool IsActive { get; set; }  
    }
}
