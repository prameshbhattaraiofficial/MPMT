namespace Mpmt.Services.Services.Receipt
{
    public class ReceiptGenerationService : IReceiptGenerationService
    {
        public string GenrateMailBody(string CustomerName)
        {
            var companyName = "MyPay Money Transfer Pvt Ltd";
            var companyEmail = "info@MPMT.com";

            string mailBody =
                $@"
                <br>
              
                 Your Transaction is Completed.Click below attachment for your Invoice!!</p>

                <p style='color=red;'>Important! Do not share your File</p>

                <br>             
                <p>If you have any queries, Please contact us at,</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>


                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }

        public string GenrateMailBodyforOtp(string Otp)
        {
            var companyName = "MyPay Money Transfer Pvt Ltd";
            var companyEmail = "info@MPMT.com";

            string mailBody =
                $@"
                <br>
              
                <p>We are pleased to provide you with the One-Time Password (OTP) necessary to Complete Registation!!</p>
                
                <P>Your OTP is: {Otp} <p>
                  
                <p style='color=red;'>Important! Do not share your File</p>
                <p>Please remember that this OTP is valid for a single use and has a limited timeframe for use. For your security, do not share this OTP with anyone, including our support team. </P>
                <br>             
                <p>If you have any queries, Please contact us at,</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>


                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
    }
}
