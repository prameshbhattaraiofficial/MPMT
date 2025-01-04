namespace Mpmt.Core.Dtos.ConversionRate
{
    public class PartnerConversionRateFilter
    {
        public string PartnerCode { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public int PaymentTypeId { get; set; }
    }
}
