using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Core.Models.Mail;
using Mpmt.Services.Services.ExchangeRateService;
using Mpmt.Services.Services.MailingService;
using System.Text;

namespace Mpmt.Services.Services.FedanRateService;

public class FedanRateBackgroundService : BackgroundService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IConfiguration _configuration;
    private readonly IMailService _mailService;

    public FedanRateBackgroundService(IExchangeRateService exchangeRateService, IConfiguration configuration, IMailService mailService)
    {
        _exchangeRateService = exchangeRateService;
        _configuration = configuration;
        _mailService = mailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTimer = int.TryParse(_configuration["BackgroundService:FedanExchangeRate:DelayMinute"], out int parsedInt) ? parsedInt : 10;
        var fedanURL = _configuration["BackgroundService:FedanExchangeRate:FedanUrl"];
        var isActive = _configuration["BackgroundService:FedanExchangeRate:IsActive"];

        if (isActive == "1")
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;

                try
                {
                    ////Web Crawler for FEDAN EXCHANGE RATE from URL at 10 AM and 2 PM
                    if ((currentTime.Hour >= 10 && currentTime.Hour <= 12) || (currentTime.Hour >= 14 && currentTime.Hour <= 16))
                    {
                        var rates = FetchFedanExchangeRate(fedanURL);
                        var fedanRates = new List<CurrencyRate>();
                        var exchangeRates = new List<FedanExchangeRate>();
                        var grouped_data = rates.GroupBy(x => x.CurrencyCode).ToDictionary(x => x.Key, x => x.ToList());

                        // Iterate through each currency group and update Rate10 & Rate14
                        foreach (var (code, group) in grouped_data)
                        {
                            if (group.Count >= 2)
                            {
                                group[0].Rate10 = group[0].BuyingRate;
                                group[0].Rate14 = group[1].BuyingRate;
                                var rate = group[0];
                                fedanRates.Add(rate);
                            }
                        }

                        exchangeRates = fedanRates.Select(r => new FedanExchangeRate
                        {
                            Symbol = ExtractCurrencySymbol(r.CurrencyCode),
                            CurrencyCode = r.CurrencyCode,
                            Unit = r.Unit.ToString(),
                            Rate10 = r.Rate10.ToString(),
                            Rate14 = r.Rate14.ToString()
                        }).ToList();

                        if (exchangeRates.Count > 0)
                        {
                            if (currentTime.Hour >= 10 && currentTime.Hour <= 12)
                            {
                                if (exchangeRates.Any(rate => rate.Rate10 != "0"))
                                {
                                    var result = await _exchangeRateService.FedanExchangeRates(exchangeRates);
                                    var groupedPartners = result.GroupBy(p => p.PartnerCode);

                                    foreach (var group in groupedPartners)
                                    {
                                        var changedPartnerRate = group.Select(partner => new ExchangeRateChangedListPartner
                                        {
                                            PartnerCode = partner.PartnerCode,
                                            PartnerName = partner.PartnerName,
                                            Email = partner.Email,
                                            Currency = partner.Currency,
                                            PrevRate = partner.PrevRate,
                                            CurrRate = partner.CurrRate,
                                            UpdatedDate = partner.UpdatedDate
                                        }).ToList();
                                        var mailBody = GenerateMailBody(changedPartnerRate, changedPartnerRate.FirstOrDefault().PartnerName, changedPartnerRate.FirstOrDefault().UpdatedDate);
                                        await SendEmailWithAttachment(mailBody, changedPartnerRate.FirstOrDefault().Email, changedPartnerRate.FirstOrDefault().PartnerName);
                                    }
                                }
                            }
                            else if (currentTime.Hour >= 14 && currentTime.Hour <= 16)
                            {
                                if (exchangeRates.Any(rate => rate.Rate14 != "0"))
                                {
                                    var result = await _exchangeRateService.FedanExchangeRates(exchangeRates);
                                    var groupedPartners = result.GroupBy(p => p.PartnerCode);

                                    foreach (var group in groupedPartners)
                                    {
                                        var changedPartnerRate = group.Select(partner => new ExchangeRateChangedListPartner
                                        {
                                            PartnerCode = partner.PartnerCode,
                                            PartnerName = partner.PartnerName,
                                            Email = partner.Email,
                                            Currency = partner.Currency,
                                            PrevRate = partner.PrevRate,
                                            CurrRate = partner.CurrRate,
                                            UpdatedDate = partner.UpdatedDate
                                        }).ToList();
                                        var mailBody = GenerateMailBody(changedPartnerRate, changedPartnerRate.FirstOrDefault().PartnerName, changedPartnerRate.FirstOrDefault().UpdatedDate);
                                        await SendEmailWithAttachment(mailBody, changedPartnerRate.FirstOrDefault().Email, changedPartnerRate.FirstOrDefault().PartnerName);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                await Task.Delay(TimeSpan.FromMinutes(delayTimer), stoppingToken);
            }
        }
    }

    private string ExtractCurrencySymbol(string input)
    {
        int openingParenthesisIndex = input.IndexOf('(');
        return openingParenthesisIndex >= 0 ? input.Substring(0, openingParenthesisIndex).Trim() : input.Trim();
    }

    private List<CurrencyRate> FetchFedanExchangeRate(string fedanUrl)
    {
        try
        {
            var web = new HtmlWeb();
            var document = web.Load(fedanUrl);

            // List to store currency data for the first table (use a descriptive name)
            List<CurrencyRate> tableRates = new List<CurrencyRate>();

            // XPath expressions to target the parent 'div' for each table
            string firstTableXPath = "//*[@class='uk-width-1-2@m uk-first-column']";
            string secondTableXPath = "//*[@class='uk-width-1-2@m']";

            if (document.DocumentNode.SelectNodes(firstTableXPath + "|" + secondTableXPath) != null)
            {
                // Loop through each table's parent 'div'
                foreach (var tableDiv in document.DocumentNode.SelectNodes(firstTableXPath + "|" + secondTableXPath))
                {
                    // Get the table node within the current 'div'
                    var table = tableDiv.SelectSingleNode(".//table");

                    // Check if table exists
                    if (table != null)
                    {
                        if (table.SelectNodes("tbody/tr") != null)
                        {
                            for (int i = 1; i <= table.SelectNodes("tbody/tr").Count; i++)
                            {
                                var row = table.SelectSingleNode("tbody/tr[" + i + "]");

                                if (row != null)
                                {
                                    // Extract data specific to the first table
                                    var currencyCodeNode = row.SelectSingleNode(".//td[1]/div[1]/div[1]").NextSibling;
                                    var unitNode = row.SelectSingleNode(".//td[2]");
                                    var buyingRateNode = row.SelectSingleNode(".//td[3]");

                                    string currencyCode = currencyCodeNode.InnerText.Trim();
                                    //int unit = int.Parse(unitNode.InnerText.Trim());
                                    //double buyingRate = double.Parse(buyingRateNode.InnerText.Trim());
                                    int unit = int.TryParse(unitNode.InnerText.Trim(), out int parsedUnit) ? parsedUnit : 1;
                                    double buyingRate = double.TryParse(buyingRateNode.InnerText.Trim(), out double parsedBuyingRate) ? parsedBuyingRate : 0.0;

                                    // Add data to the first table list
                                    tableRates.Add(new CurrencyRate(currencyCode, unit, buyingRate));
                                }
                            }
                        }
                    }
                }
            }
            return tableRates;
        }
        catch (Exception ex)
        {
            return new List<CurrencyRate>();
        }
    }

    public async Task SendEmailWithAttachment(string mailBody, string mailTo, string partnerName)
    {
        var mailRequest = new MailRequestModel
        {
            MailFor = "exchange-rate-change",
            MailTo = mailTo,
            MailSubject = "Exchange Rate Update",
            RecipientName = partnerName,
            Content = mailBody
        };
        var mailServiceModel = await _mailService.EmailSettings(mailRequest);
        Thread email = new(delegate ()
        {
            _mailService.SendMail(mailServiceModel);
        });
        email.Start();
    }

    public string GenerateMailBody(List<ExchangeRateChangedListPartner> rateChangedListPartners, string partnerName, DateTime updatedDate)
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyEmail = "info@mypaymt.com";

        string mailBody =
            $@"
                <p>Following Exchange rates have been updated on: <b>{updatedDate.ToString("yyyy-MMM-dd hh:mm:ss tt")} (NST)</b></p>
                    {GetTableHtml(rateChangedListPartners)}
                <br>    
                <p style='font-weight:bold; color: red!important;'>This is automated email regarding Exchange rate as per FEDAN. You will receive email only when rates gets changed.</p>
                <h3><u>MyPay Money Transfer Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: 015908052<br>
                        Email: {companyEmail}<br>
                        Website: https://www.mypaymt.com/<br>
                        Address: Radhe Radhe, Bhaktapur, Nepal<br>
                    </p>
                    <p>Thank you for choosing {companyName} Service!</p>
                <p style='color=orange;>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <br>
                <p style='color=red;'>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";
        return mailBody;
    }

    private string GetTableHtml(List<ExchangeRateChangedListPartner> data)
    {
        if (data == null || data.Count == 0)
        {
            return "<p>No exchange rate data available.</p>";
        }

        StringBuilder tableHtml = new StringBuilder();
        tableHtml.AppendLine("<style>");
        tableHtml.AppendLine("table {");
        tableHtml.AppendLine("  border-collapse: collapse;");
        tableHtml.AppendLine("  width: 100%;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("th, td {");
        tableHtml.AppendLine("  padding: 8px 16px;"); // Increased padding for more space
        tableHtml.AppendLine("  text-align: left;");
        tableHtml.AppendLine("  border-bottom: 1px solid #ddd;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("tr:nth-child(even) {background-color: #f2f2f2}");
        tableHtml.AppendLine("th {");
        tableHtml.AppendLine("  background-color: #2b7f00;");
        tableHtml.AppendLine("  color: white;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("</style>");

        tableHtml.AppendLine("<table style='border-collapse: collapse; width: 100%;'>");
        tableHtml.AppendLine("<thead style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd; background-color: #2b7f00; color: white;'>");
        tableHtml.AppendLine("<tr>");
        tableHtml.AppendLine("<th>SN</th>");
        tableHtml.AppendLine("<th>Currency</th>");
        tableHtml.AppendLine("<th>Current Rate</th>");
        tableHtml.AppendLine("<th>Previous Rate</th>");
        tableHtml.AppendLine("</tr>");
        tableHtml.AppendLine("</thead>");
        tableHtml.AppendLine("<tbody>");

        int sn = 1;

        foreach (var partner in data)
        {
            tableHtml.AppendLine("<tr>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{sn++}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.Currency}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.CurrRate.ToString("F2")}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.PrevRate.ToString("F2")}</td>");
            tableHtml.AppendLine("</tr>");
        }

        tableHtml.AppendLine("</tbody>");
        tableHtml.AppendLine("</table>");

        return tableHtml.ToString();
    }
}
