namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayValidateBankUserApiResponse : MyPayBaseResponse
    {
        public string AccountStatus { get; set; }
        public string ResponseCode { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string MatchPercentate { get; set; }
        public string kycstatus { get; set; }
        public bool IsAccountValidated { get; set; }    
    }
}
