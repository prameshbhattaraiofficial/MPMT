namespace Mpmt.Services.Authentication
{
    public class SessionAuthExpiration
    {
        public string UserUniqueId { get; set; }
        public DateTime? ExpireBefore { get; set; }
    }
}
