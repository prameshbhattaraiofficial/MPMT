using Mpmt.Core.Models.Mail;

namespace Mpmt.Data.Repositories.Mailing
{
    public interface IMailRepository
    {
        Task<MailConfiguration> GetMailConfiguration();
        Task<IEnumerable<MailMessageSettingsModel>> MailMessageSettings();
    }
}
