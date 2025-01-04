namespace Mpmt.Core.Dtos.CashAgent;

public class AgentLedger
{
    public string TransactionId { get; set; }
    public string NepaliDate { get; set; }
    public string EnglishDate { get; set; }
    public string ReferenceId { get; set; }
    public string Particular { get; set; }
    public string Remarks { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string EntryType { get; set; }
    public decimal TotalBalance { get; set; }
}
