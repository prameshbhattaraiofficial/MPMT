namespace Mpmt.Core.Dtos.FeeFundRequest;

public class FeeFundRequestFilter
{
    public string PartnerCode { get; set; }
    public string TxnId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SortingCol { get; set; }
    public string SortType { get; set; }
    public string SearchVal { get; set; }
    public int Export { get; set; }
}
