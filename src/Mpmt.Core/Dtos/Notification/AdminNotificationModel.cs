namespace Mpmt.Core.Dtos.Notification
{
    public class NotificationModel
    {
        public Guid NotificationId { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public string PartnerCode { get; set; }
        public string Message { get; set; }
        public string AdminLink { get; set; }
        public string PartnerLink { get; set; }
        public string ModuleCode { get; set; }
        public bool IsReadByAdmin { get; set; }
        public bool IsReadByPartner { get; set; }
        public string CreatedDate { get; set; }
    }

    public class AdminNotificationModel
    {
        public Guid NotificationId { get; set; }
        public string NotifyId { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public string PartnerCode { get; set; }
        public string Message { get; set; }
        public string AdminLink { get; set; }
        public string PartnerLink { get; set; }

        public bool IsReadByAdmin { get; set; } 
        public bool IsReadByPartner { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string ModuleCode { get; set; }
    }
    public class PartnerNotificationModel
    {
        public Guid NotificationId { get; set; }
        public string NotifyId { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public string PartnerCode { get; set; }
        public string Message { get; set; }
        public string PartnerLink { get; set; }
        public bool IsReadByPartner { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModuleCode { get; set; }  

    }
}
