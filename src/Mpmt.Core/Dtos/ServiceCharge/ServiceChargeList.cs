namespace Mpmt.Core.Dtos.ServiceCharge
{
    public class ServiceChargeList
    {
        public int Id { get; set; }
        public decimal MinAmountSlab { get; set; }
        public decimal MaxAmountSlab { get; set; }
        public decimal ServiceChargePercent { get; set; }
        public decimal ServiceChargeFixed { get; set; }
        public decimal MinServiceCharge { get; set; }
        public decimal MaxServiceCharge { get; set; }
        public decimal CommissionPercent { get; set; }
        public decimal CommissionFixed { get; set; }
        public decimal MinComission { get; set; }
        public decimal MaxComission { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
