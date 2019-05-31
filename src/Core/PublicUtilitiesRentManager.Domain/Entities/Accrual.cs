using System;

namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Accrual
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime AccrualDate { get; set; }
        public decimal Summ { get; set; }
    }
}
