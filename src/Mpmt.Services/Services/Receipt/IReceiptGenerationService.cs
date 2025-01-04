namespace Mpmt.Services.Services.Receipt
{
    public interface IReceiptGenerationService
    {
        string GenrateMailBody(string CustomerName);
        string GenrateMailBodyforOtp(string Otp);

    }
}
