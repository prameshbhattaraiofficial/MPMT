namespace Mpmt.Services.Common
{
    public static class OtpGeneration
    {
        public static string GenerateRandom6DigitCode()
        {
            Random random = new Random();
            int code = random.Next(100000, 999999);
            return code.ToString();
        }
    }
}
