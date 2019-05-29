using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class CalculationMonitorEntryViewModel
    {
        public string TenantId { get; set; }
        public string AccrualTypeId { get; set; }
        [Display(Name = "Услуга")]
        public string AccrualTypeName { get; set; }
        [Display(Name = "Остаток на начало периода")]
        public decimal OpeningBalance { get; set; }
        [Display(Name = "Начислено")]
        public decimal AccrualsSum { get; set; }
        [Display(Name = "Оплачено")]
        public decimal PaymentsSum { get; set; }
        [Display(Name = "Остаток на конец периода")]
        public decimal ClosingBalance { get; set; }
    }
}
