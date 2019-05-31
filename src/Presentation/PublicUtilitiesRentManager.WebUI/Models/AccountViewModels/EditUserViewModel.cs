using Microsoft.AspNetCore.Mvc.Rendering;

namespace PublicUtilitiesRentManager.WebUI.Models.AccountViewModels
{
    public class EditUserViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string TenantId { get; set; }

        public SelectList Tenants { get; set; }
    }
}
