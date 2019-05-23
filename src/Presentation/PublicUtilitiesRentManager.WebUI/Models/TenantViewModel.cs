using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class TenantViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Телефонный номер")]
        public string PhoneNumber { get; set; }

        public static TenantViewModel FromTenant(Tenant tenant) =>
            new TenantViewModel
            {
                Id = tenant.Id,
                UserId = tenant.UserId,
                Name = tenant.Name,
                Address = tenant.Address,
                PhoneNumber = tenant.PhoneNumber
            };
    }
}
