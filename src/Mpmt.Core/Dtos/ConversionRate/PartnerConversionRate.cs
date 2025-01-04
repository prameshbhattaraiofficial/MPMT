namespace Mpmt.Core.Dtos.ConversionRate
{
    public class PartnerConversionRate
    {
        public string PartnerCode { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public int UnitValue { get; set; }
        public decimal MinRate { get; set; }
        public decimal MaxRate { get; set; }
        public decimal CurrentRate { get; set; }
    }
}
