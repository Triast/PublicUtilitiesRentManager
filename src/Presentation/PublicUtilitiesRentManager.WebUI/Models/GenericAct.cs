using System;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class GenericAct
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        [Display(Name = "Арендатор")]
        public string Tenant { get; set; }
        [Display(Name = "Помещение")]
        public string Room { get; set; }
        [Display(Name = "Услуга")]
        public string AccrualType { get; set; }
        [Display(Name = "Дата начисления")]
        public DateTime Date { get; set; }
        [Display(Name = "Сумма")]
        public decimal Summ { get; set; }
        public bool IsPayment { get; set; }

        public static GenericAct FromAccrualViewModel(AccrualViewModel accrual) =>
            new GenericAct
            {
                Id = accrual.Id,
                ContractId = accrual.ContractId,
                Tenant = accrual.Tenant,
                Room = accrual.Room,
                AccrualType = accrual.AccrualType,
                Date = accrual.AccrualDate,
                Summ = accrual.Summ,
                IsPayment = false
            };

        public static GenericAct FromPaymentViewModel(PaymentViewModel payment) =>
            new GenericAct
            {
                Id = payment.Id,
                ContractId = payment.ContractId,
                Tenant = payment.Tenant,
                Room = payment.Room,
                AccrualType = payment.AccrualType,
                Date = payment.PaymentDate,
                Summ = payment.Summ,
                IsPayment = true
            };
    }
}
