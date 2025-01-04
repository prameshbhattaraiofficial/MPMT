namespace Mpmt.Core.Dtos.Payment.MyPay
{
    public class MyPayUsePaymentRequest
    {
        public string Amount { get; set; }
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MerchantId { get; set; }
    }
}
