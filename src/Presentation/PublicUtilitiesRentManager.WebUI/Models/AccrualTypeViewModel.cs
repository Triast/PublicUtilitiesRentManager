using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class AccrualTypeViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        public static AccrualTypeViewModel FromAccrualType(AccrualType accrualType) =>
            new AccrualTypeViewModel
            {
                Id = accrualType.Id,
                Name = accrualType.Name
            };
    }
}
