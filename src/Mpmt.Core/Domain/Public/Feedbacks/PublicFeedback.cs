namespace Mpmt.Core.Domain.Public.Feedbacks
{
    public class PublicFeedback
    {
        public int FeedbackId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public bool IsReviewed { get; set; }
        public DateTime? ReviewedLocalDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedLocalDate { get; set; }
        public DateTime? UpdatedLocalDate { get; set; }
    }
}
