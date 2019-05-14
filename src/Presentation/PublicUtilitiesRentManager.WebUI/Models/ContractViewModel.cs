using PublicUtilitiesRentManager.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class ContractViewModel
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        [Display(Name = "Арендатор")]
        public string Tenant { get; set; }
        public string RoomId { get; set; }
        [Display(Name = "Помещение")]
        public string Room { get; set; }
        public string AccrualTypeId { get; set; }
        [Display(Name = "Услуга")]
        public string AccrualType { get; set; }
        [Display(Name = "Начало договора")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Окончание договора")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public static ContractViewModel FromContract(Contract contract) =>
            new ContractViewModel
            {
                Id = contract.Id,
                TenantId = contract.TenantId,
                RoomId = contract.RoomId,
                AccrualTypeId = contract.AccrualTypeId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate
            };
    }
}
