using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.ReceiptDownloadModel;
using SelectPdf;
using System.Data;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Mpmt.Core.Common;

public class ExportHelper
{
    public static string FileExcelFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public static string FileCsvFormat = "text/csv";

    private static String[] units = { "Zero", "One", "Two", "Three",
                    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
                    "Seventeen", "Eighteen", "Nineteen" };
    private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
                    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

    public static (byte[], string fileFormat, string fileName) GenerateCsv<T>(IEnumerable<T> data, string[] columnNames, string topic = null, string fileName = null, bool appendDateTimeToFileName = true)
    {
        var properties = typeof(T).GetProperties();

        var sb = new StringBuilder();
        if (topic is not null || topic == "")
        {
            sb.AppendLine($"\"{topic}\"");
        }

        if (columnNames.Count() <= 0)
        {
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));
        }
        else
        {
            sb.AppendLine(string.Join(",", columnNames));
        }

        foreach (var item in data)
        {
            sb.AppendLine(string.Join(",", properties.Select(p =>
                (p.GetValue(item) ?? "").ToString().Replace(",", ""))));
        }
        fileName = string.IsNullOrWhiteSpace(fileName) ? "ReportCsv" : fileName;
        fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.csv" : fileName;
        return (Encoding.UTF8.GetBytes(sb.ToString()), FileCsvFormat, fileName);
    }

    public static Task<(byte[], string fileFormat, string fileName)> ToExcelAsync(DataTable dataTable, string fileName = null, bool appendDateTimeToFileName = true)
    {
        return Task.Run(() =>
        {
            using var workbook = new XLWorkbook();
            workbook.AddWorksheet(dataTable, "sheet");

            fileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
            fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.xlsx" : fileName;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return (stream.ToArray(), FileExcelFormat, fileName);
        });
    }

    public static Task<(byte[], string fileFormat, string fileName)> ToExcelAsync(
        List<DataTable> dataTableList, string fileName = null, bool appendDateTimeToFileName = true)
    {
        return Task.Run(() =>
        {
            using var workbook = new XLWorkbook();

            int seedIdx = 1;
            foreach (var dataTable in dataTableList)
            {
                workbook.AddWorksheet(dataTable, $"Sheet{seedIdx}");
                seedIdx++;
            }

            fileName = string.IsNullOrWhiteSpace(fileName) ? "Report" : fileName;
            fileName = appendDateTimeToFileName ? $"{fileName}_{DateTime.Now:yyyyMMddhhmmssfff}.xlsx" : fileName;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return (stream.ToArray(), FileExcelFormat, fileName);
        });
    }

    public static async Task<(byte[], string)> TopdfAsync<T>(PagedList<T> kyc, string Title)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        string header = string.Empty;
        foreach (var property in properties)
        {
            header += $"<th>{property.Name}</th>";
        }

        string htmlhead = "<!DOCTYPE html>\r\n<html>\r\n  <head>\r\n    <title>Title of the document</title>\r\n    <style>\r\n      table,\r\n      th,\r\n      td {\r\n        padding: 10px;\r\n        border: 1px solid black;\r\n        border-collapse: collapse;\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n " +
           $" <h1>{Title}</h1>" +
           $" <h3 style=\"text-align:right;\">Date :{DateTime.Now:yyyy-MMM-dd hh:mm:ss tt}</h3>" +

           "<table style=\"margin-left: auto; margin-right: auto;\">    " +
           $"<tr>{header}</tr>";

        string htmlbody = string.Empty;
        string tblrow = string.Empty;
        foreach (var data in kyc.Items)
        {
            htmlbody = string.Empty;
            foreach (var porp in properties)
            {
                htmlbody += $"<td>{porp.GetValue(data)}</td>";
            }
            tblrow += $"<tr>{htmlbody}</tr>";
        }

        string htmlfooter = "</table> </body> </html>";
        string htmldata = htmlhead + tblrow + htmlfooter;

        // instantiate a html to pdf converter object
        HtmlToPdf pdfConverter = new HtmlToPdf();
        pdfConverter.Options.PdfPageSize = PdfPageSize.A4;
        pdfConverter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        pdfConverter.Options.WebPageHeight = 1455;
        //converter.Options.WebPageWidth = 500;
        //converter.Options.WebPageHeight = 100;
        pdfConverter.Options.MarginTop = 5;
        pdfConverter.Options.MarginLeft = 5;
        pdfConverter.Options.MarginRight = 5;
        pdfConverter.Options.MarginBottom = 5;
        pdfConverter.Options.PdfCompressionLevel = PdfCompressionLevel.Normal;
        pdfConverter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
        pdfConverter.Options.KeepImagesTogether = true;
        pdfConverter.Options.CssMediaType = (HtmlToPdfCssMediaType)Enum.Parse(typeof(HtmlToPdfCssMediaType), "Screen", true);
        // create a new pdf document converting an url
        PdfDocument doc = pdfConverter.ConvertHtmlString(htmldata);

        // save pdf document
        var docBytes = doc.Save();

        // close pdf document
        doc.Close();
        return (docBytes, Application.Pdf); ;
    }

    public static String ConvertAmount(double amount)
    {
        try
        {
            Int64 amount_int = (Int64)amount;
            Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
            if (amount_dec == 0)
            {
                return Convert(amount_int) + " Rupees only";
            }
            else
            {
                return Convert(amount_int) + " Rupees and " + Convert(amount_dec) + " Paisa only";
            }
        }
        catch (Exception e)
        {
        }
        return "";
    }

    public static String Convert(Int64 i)
    {
        if (i < 20)
        {
            return units[i];
        }
        if (i < 100)
        {
            return tens[i / 10] + ((i % 10 > 0) ? " " + Convert(i % 10) : "");
        }
        if (i < 1000)
        {
            return units[i / 100] + " Hundred"
                    + ((i % 100 > 0) ? " " + Convert(i % 100) : "");
        }
        if (i < 100000)
        {
            return Convert(i / 1000) + " Thousand "
                    + ((i % 1000 > 0) ? " " + Convert(i % 1000) : "");
        }
        if (i < 10000000)
        {
            return Convert(i / 100000) + " Lakh "
                    + ((i % 100000 > 0) ? " " + Convert(i % 100000) : "");
        }
        if (i < 1000000000)
        {
            return Convert(i / 10000000) + " Crore "
                    + ((i % 10000000 > 0) ? " " + Convert(i % 10000000) : "");
        }
        return Convert(i / 1000000000) + " Arab "
                + ((i % 1000000000 > 0) ? " " + Convert(i % 1000000000) : "");
    }

    public static async Task<string> GenerateHtmlContent(IWebHostEnvironment webHost)
    {
        string webRoot = webHost.WebRootPath;
        string filePath = webRoot + "\\ReportTemplates\\senderReceipt.html";
        return await File.ReadAllTextAsync(filePath);
    }

    [HttpGet]
    public static async Task<(byte[], string)> GenerateReceiptReportPdf([FromQuery] ReceiptDetailModel model, IWebHostEnvironment webHost)
    {
        string htmlContent = await GenerateHtmlContent(webHost);
        var amountInWord = ConvertAmount(model.ReceiveAmount);
        string dynamicTemplate = htmlContent
            .Replace("{{PartnerName}}", model.PartnerName)
            .Replace("{{PartnerContact}}", model.PartnerContact)
            .Replace("{{PartnerAddress}}", model.PartnerAddress)
            .Replace("{{TransactionId}}", model.TransactionId)
            .Replace("{{GatewayTransactionId}}", model.GatewayTransactionId)
            .Replace("{{TransactionDate}}", model.TransactionDate)
            .Replace("{{SendAmount}}", model.SendAmount)
            .Replace("{{SendCurrency}}", model.SendCurrency)
            .Replace("{{SenderName}}", model.SenderName)
            .Replace("{{SenderAddress}}", model.SenderAddress)
            .Replace("{{SenderCountry}}", model.SenderCountry)
            .Replace("{{SenderContact}}", model.SenderContact)
            .Replace("{{ReceiverName}}", model.ReceiverName)
            .Replace("{{ReceiverAddress}}", model.ReceiverAddress)
            .Replace("{{ReceiverContact}}", model.ReceiverContact)
            .Replace("{{PaymentType}}", model.PaymentType)
            .Replace("{{AccountNumber}}", model.AccountNumber)
            .Replace("{{BankName}}", model.BankName)
            .Replace("{{ReceiveAmountString}}", model.ReceiveAmountString)
            .Replace("{{ReceiveCurrency}}", model.ReceiveCurrency)
            .Replace("{{Status}}", model.Status)
            .Replace("{{AmountInWords}}", amountInWord);

        // Instantiate a HTML to PDF converter object
        HtmlToPdf converter = new HtmlToPdf();

        converter.Options.PdfPageSize = PdfPageSize.A4;
        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        converter.Options.DrawBackground = true;
        converter.Options.MarginTop = 5;
        converter.Options.MarginLeft = 5;
        converter.Options.MarginRight = 5;
        converter.Options.PdfCompressionLevel = PdfCompressionLevel.Best;
        converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
        converter.Options.KeepImagesTogether = true;
        converter.Options.CssMediaType = (HtmlToPdfCssMediaType)Enum.Parse(typeof(HtmlToPdfCssMediaType), "Screen", true);

        // Generate PDF
        PdfDocument doc = converter.ConvertHtmlString(dynamicTemplate);

        var docBytes = doc.Save();
        doc.Close();

        return (docBytes, Application.Pdf);
    }
}


