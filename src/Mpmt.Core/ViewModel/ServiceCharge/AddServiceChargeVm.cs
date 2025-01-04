using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.ServiceCharge
{
    /// <summary>
    /// The add service charge vm.
    /// </summary>
    public class AddServiceChargeVm
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the charge category id.
        /// </summary>
        public int ChargeCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        /// 
        public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the payment type id.
        /// </summary>
        public int PaymentTypeId { get; set; }
        /// <summary>
        /// Gets or sets the min amount slab.
        /// </summary>
        public decimal MinAmountSlab { get; set; }
        /// <summary>
        /// Gets or sets the max amount slab.
        /// </summary>
        public decimal MaxAmountSlab { get; set; }
        /// <summary>
        /// Gets or sets the service charge percent.
        /// </summary>
        public decimal ServiceChargePercent { get; set; }
        /// <summary>
        /// Gets or sets the service charge fixed.
        /// </summary>
        public decimal ServiceChargeFixed { get; set; }
        /// <summary>
        /// Gets or sets the min service charge.
        /// </summary>
        public decimal MinServiceCharge { get; set; }
        /// <summary>
        /// Gets or sets the max service charge.
        /// </summary>
        public decimal MaxServiceCharge { get; set; }
        /// <summary>
        /// Gets or sets the commission percent.
        /// </summary>
        public decimal CommissionPercent { get; set; }
        /// <summary>
        /// Gets or sets the commission fixed.
        /// </summary>
        public decimal CommissionFixed { get; set; }
        /// <summary>
        /// Gets or sets the min comission.
        /// </summary>
        public decimal MinComission { get; set; }
        /// <summary>
        /// Gets or sets the max comission.
        /// </summary>
        public decimal MaxComission { get; set; }
        /// <summary>
        /// Gets or sets the from date.
        /// </summary>
        public DateTime FromDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the to date.
        /// </summary>
        public DateTime ToDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether maker.
        /// </summary>
        public bool Maker { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether checker.
        /// </summary>
        public bool Checker { get; set; }
        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>
        public int CreatedById { get; set; }
        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the updated by id.
        /// </summary>
        public int UpdatedById { get; set; }
        /// <summary>
        /// Gets or sets the updated by name.
        /// </summary>
        public string UpdatedByName { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public string UpdatedDate { get; set; }
    }
}
