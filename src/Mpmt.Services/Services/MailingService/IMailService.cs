using Mpmt.Core.Models.Mail;

namespace Mpmt.Services.Services.MailingService
{
    public interface IMailService
    {
        Task<bool> SendMail(MailServiceModel mailModel);
        void SetMailConfiguration();
        Task<MailServiceModel> EmailSettings(MailRequestModel mailRequest);
    }
}
