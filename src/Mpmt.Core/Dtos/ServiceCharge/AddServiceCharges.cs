namespace Mpmt.Core.Dtos.ServiceCharge
{
    public class AddServiceCharges
    {
        public int ChargeCategoryId { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public int PaymentTypeId { get; set; }
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
        public bool IsDeleted { get; set; }
        public bool Maker { get; set; }
        public bool Checker { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int UpdatedById { get; set; }
        public string UpdatedByName { get; set; }
        public string UpdatedDate { get; set; }
    }
}
