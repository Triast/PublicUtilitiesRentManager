using PublicUtilitiesRentManager.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class PaymentViewModel
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        [Display(Name = "Арендатор")]
        public string Tenant { get; set; }
        [Display(Name = "Помещение")]
        public string Room { get; set; }
        [Display(Name = "Услуга")]
        public string AccrualType { get; set; }
        [Display(Name = "№ платёжного поручения")]
        public int PaymentOrderNumber { get; set; }
        [Display(Name = "Дата оплаты")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Сумма")]
        public decimal Summ { get; set; }

        public static PaymentViewModel FromPayment(Payment payment) =>
            new PaymentViewModel
            {
                Id = payment.Id,
                ContractId = payment.ContractId,
                PaymentOrderNumber = payment.PaymentOrderNumber,
                PaymentDate = payment.PaymentDate,
                Summ = payment.Summ
            };

        public static Payment FromPaymentViewModel(PaymentViewModel paymentVM) =>
            new Payment
            {
                Id = paymentVM.Id,
                ContractId = paymentVM.ContractId,
                PaymentOrderNumber = paymentVM.PaymentOrderNumber,
                PaymentDate = paymentVM.PaymentDate,
                Summ = paymentVM.Summ
            };
    }
}
