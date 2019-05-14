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
        [Display(Name = "Услуга")]
        public string AccrualType { get; set; }
        [Display(Name = "Дата начисления")]
        public DateTime AccrualDate { get; set; }
        [Display(Name = "Сумма")]
        public decimal Summ { get; set; }

        public static AccrualViewModel FromAccrual(Accrual accrual) =>
            new AccrualViewModel
            {
                Id = accrual.Id,
                ContractId = accrual.ContractId,
                AccrualDate = accrual.AccrualDate,
                Summ = accrual.Summ
            };
    }
}
