using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Dtos.Partner
{
    public class TokenVerification
    {
        public int UserId { get; set; }
        public string  PartnerCode { get; set; }
        public string  UserName { get; set; }
        public string  Email { get; set; }
        public string  CountryCallingCode { get; set; }
        public string  Mobile { get; set; }
        public string  VerificationCode { get; set; }
        public string  VerificationType { get; set; }
        public bool SendToMobile { get; set; }
        public bool SendToEmail { get; set; }
        public DateTime  ExpiredDate { get; set; }
        public DateTime ExpiredUtcDate { get; set; }
        public bool  IsConsumed { get; set; }
        public int OtpAttemptCount { get; set; }
        public string OtpVerificationFor { get; set; }
    
    }
}
