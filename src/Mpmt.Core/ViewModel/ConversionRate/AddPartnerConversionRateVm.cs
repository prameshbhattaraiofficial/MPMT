namespace Mpmt.Core.ViewModel.ConversionRate
{
    public class AddPartnerConversionRateVm
    {
        public decimal MinAmountSlab { get; set; }
        public decimal MaxAmountSlab { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ServiceChargePercent { get; set; }
        public decimal ServiceChargeFixed { get; set; }
        public decimal MinServiceCharge { get; set; }
        public decimal MaxServiceCharge { get; set; }
        public decimal CommissionPercent { get; set; }
        public decimal CommissionFixed { get; set; }
        public decimal MinCommission { get; set; }
        public decimal MaxCommission { get; set; }
    }
}
