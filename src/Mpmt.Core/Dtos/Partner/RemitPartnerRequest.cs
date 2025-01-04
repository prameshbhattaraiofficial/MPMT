namespace Mpmt.Core.Dtos.Partner
{
    public class RemitPartnerRequest
    {
        public Guid Id { get; set; }
        public string shortName { get; set; }
        public string OperationMode { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
    }
}
