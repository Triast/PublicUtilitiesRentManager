using PublicUtilitiesRentManager.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class AccrualViewModel
    {
        public string Id { get; set; }
        [Display(Name = "ID договора (временно)")]
        public string ContractId { get; set; }
        [Display(Name = "Арендатор")]
        public string Tenant { get; set; }
        [Display(Name = "Помещение")]
        public string Room { get; set; }
        [Display(Name = "№ счёт-фактуры")]
        public int InvoiceNumber { get; set; }
        [Display(Name = "Услуга")]
        public string AccrualType { get; set; }
        [Display(Name = "Дата начисления")]
        [DataType(DataType.Date)]
        public DateTime AccrualDate { get; set; }
        [Display(Name = "Сумма")]
        public decimal Summ { get; set; }

        public static AccrualViewModel FromAccrual(Accrual accrual) =>
            new AccrualViewModel
            {
                Id = accrual.Id,
                ContractId = accrual.ContractId,
                InvoiceNumber = accrual.InvoiceNumber,
                AccrualDate = accrual.AccrualDate,
                Summ = accrual.Summ
            };

        public static Accrual FromAccrualViewModel(AccrualViewModel accrualVM) =>
            new Accrual
            {
                Id = accrualVM.Id,
                ContractId = accrualVM.ContractId,
                InvoiceNumber = accrualVM.InvoiceNumber,
                AccrualDate = accrualVM.AccrualDate,
                Summ = accrualVM.Summ
            };
    }
}
