using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models.AccountViewModels
{
    public class IndexViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Арендатор")]
        public string Tenant { get; set; }
        public string StatusMessage { get; set; }
    }
}
