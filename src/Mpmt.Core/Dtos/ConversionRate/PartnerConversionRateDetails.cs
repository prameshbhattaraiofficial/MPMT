namespace Mpmt.Core.Dtos.ConversionRate
{
    public class PartnerConversionRateDetails
    {
        public double MinAmountSlab { get; set; }
        public double MaxAmountSlab { get; set; }
        public double ConversionRate { get; set; }
        public double ServiceChargePercent { get; set; }
        public double ServiceChargeFixed { get; set; }
        public double MinServiceCharge { get; set; }
        public double MaxServiceCharge { get; set; }
        public double CommissionPercent { get; set; }
        public double CommissionFixed { get; set; }
        public double MinCommission { get; set; }
        public double MaxCommission { get; set; }
    }
}
