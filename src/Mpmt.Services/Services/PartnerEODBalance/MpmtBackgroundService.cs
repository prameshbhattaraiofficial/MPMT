using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.EODBalance;
using Mpmt.Core.Extensions;
using Mpmt.Core.Models.Mail;
using Mpmt.Services.Services.ExchangeRateService;
using Mpmt.Services.Services.MailingService;
using OfficeOpenXml;
using System.Data;

namespace Mpmt.Services.Services.PartnerEODBalance;

public class MpmtBackgroundService : BackgroundService
{
    private readonly IEODBalanceService _EODBalanceService;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;

    public MpmtBackgroundService(IEODBalanceService EODBalanceService, IMailService mailService, IConfiguration configuration, IExchangeRateService exchangeRateService)
    {
        _EODBalanceService = EODBalanceService;
        _mailService = mailService;
        _configuration = configuration;
        _exchangeRateService = exchangeRateService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.Now;

            try
            {
                ////RECURRING EOD BALANCE SENDING EMAIL
                var EodHour = int.Parse(_configuration["BackgroundService:EOD:HOUR"]);
                var EodMinute = int.Parse(_configuration["BackgroundService:EOD:MINUTE"]);

                if (currentTime.Hour == EodHour && currentTime.Minute == EodMinute)
                {
                    var partnerEODBalance = await _EODBalanceService.GetPartnerEODBalanceAsync();
                    var (byteArray, fileFormat, fileName) = await ConvertToExcelByteArray(partnerEODBalance);
                    await SendEmailWithAttachment(byteArray, fileFormat, fileName);
                }

                ////FEDAN EXCHANGE RATE UPDATE AT 10 AM AND 2 PM
                //if ((currentTime.Hour == 10 && currentTime.Minute <= 15) || (currentTime.Hour == 14 && currentTime.Minute <= 15))
                //if ((currentTime.Hour >= 10 && currentTime.Hour <= 11) || (currentTime.Hour >= 14 && currentTime.Hour <= 15))
                //{
                //    var data = ProcessExcelFile(_configuration["ExcelFilePath:ExchangeRateFilePath"]);
                //    if (data.Count > 0)
                //        await _exchangeRateService.UpdateExchangeRates(data);
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
       }
    }

    public async Task<(byte[], string, string)> ConvertToExcelByteArray(IEnumerable<EODBalance> data)
    {
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "PartnerEODBalance", true);

        return (excelFileByteArr, fileFormat, fileName);
    }

    public async Task SendEmailWithAttachment(byte[] fileContent, string fileFormat, string fileName)
    {
        var mailRequest = new MailRequestModel
        {
            MailFor = "eod-balance",
            MailTo = _configuration["BackgroundService:EOD:TO"],
            MailSubject = "Partner EOD Balance",
            RecipientName = "User",
            Content = GenerateMailBody()
        };
        var mailServiceModel = await _mailService.EmailSettings(mailRequest);
        mailServiceModel.MailCc = _configuration["BackgroundService:EOD:CC"];
        mailServiceModel.MailAttachements = new Dictionary<string, byte[]> { { fileName, fileContent } };
        Thread email = new(delegate ()
        {
            _mailService.SendMail(mailServiceModel);
        });
        email.Start();
    }

    public string GenerateMailBody()
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyEmail = "info@mypaymt.com";

        string mailBody =
            $@"
                <p>Partner EOD Balance is attached to this mail.</p>
                <p style='color=red;'>Important! Do not share your File</p>
                <br>             
                <p style='color=orange;>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>If you have any queries, Please contact us at,</p>
                <p>{companyName},<br>
                Radhe Radhe, Bhaktapur, Nepal<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>

                <p style='color=red;'>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";
        return mailBody;
    }

    private bool IsDecimal(string input)
    {
        return decimal.TryParse(input, out _);
    }

    public List<ExchangeRate> ProcessExcelFile(string path)
    {
        List<ExchangeRate> dataList = new();

        if (File.Exists(path))
        {
            try
            {
                using var stream = File.OpenRead(path);
                using var package = new ExcelPackage(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[2];

                int headerRowIndex = 1;
                var sourceCurrency = string.Empty;
                var unitValue = string.Empty;
                var rate10 = string.Empty;
                var rate14 = string.Empty;

                for (int rowIndex = headerRowIndex + 1; rowIndex <= worksheet.Dimension.End.Row; rowIndex++)
                {
                    sourceCurrency = worksheet.Cells[rowIndex, 1].Value?.ToString().Trim();
                    unitValue = worksheet.Cells[rowIndex, 3].Value?.ToString().Trim();
                    rate10 = worksheet.Cells[rowIndex, 4].Value?.ToString().Trim();
                    rate14 = worksheet.Cells[rowIndex, 5].Value?.ToString().Trim();
                    
                    if(!IsDecimal(rate10))
                        rate10 = "0";
                    if (!IsDecimal(rate14))
                        rate14 = "0";
                    if (!IsDecimal(unitValue))
                        unitValue = "1";

                    ExchangeRate rate = new()
                    {
                        SourceCurrency = sourceCurrency,
                        UnitValue = string.IsNullOrWhiteSpace(unitValue) ? "1" : unitValue,
                        Rate10 = string.IsNullOrWhiteSpace(rate10) ? "0" : rate10,
                        Rate14 = string.IsNullOrWhiteSpace(rate14) ? "0" : rate14,
                        BuyingRate = (string.IsNullOrWhiteSpace(rate14) || rate14 == "0")
                            ? ((string.IsNullOrWhiteSpace(rate10) || rate10 == "0") ? "0" : rate10)
                            : rate14,
                        SellingRate = (string.IsNullOrWhiteSpace(rate14) || rate14 == "0")
                            ? ((string.IsNullOrWhiteSpace(rate10) || rate10 == "0") ? "0" : rate10)
                            : rate14
                    };
                    dataList.Add(rate);
                }
                return dataList.ToList();
            }
            catch (Exception ex)
            {
                return new List<ExchangeRate>();
            }
        }
        else
        {
            return new List<ExchangeRate>();
        }
    }
}