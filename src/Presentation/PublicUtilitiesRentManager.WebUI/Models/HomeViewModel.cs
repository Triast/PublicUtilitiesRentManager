using Microsoft.AspNetCore.Mvc.Rendering;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class HomeViewModel
    {
        public SelectList Tenants { get; set; }
        public SelectList AccrualTypes { get; set; }
    }
}
