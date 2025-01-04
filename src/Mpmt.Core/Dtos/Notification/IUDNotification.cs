namespace Mpmt.Core.Dtos.Notification
{
    public class IUDNotification
    {
        public char Event { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string PartnerCode { get; set; }
        public string Message { get; set; } 
        public string AdminLink { get; set; }
        public string PartnerLink { get; set; }
        public string ModuleCode { get; set; }
    }
}
