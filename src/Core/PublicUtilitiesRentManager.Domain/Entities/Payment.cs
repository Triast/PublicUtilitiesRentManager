using System;

namespace PublicUtilitiesRentManager.Domain.Entities
{
    public class Payment
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Summ { get; set; }
    }
}
