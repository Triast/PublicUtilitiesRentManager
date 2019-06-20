using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;

        public HomeController(IAccrualTypeRepository accrualTypeRepository, ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
            _accrualTypeRepository = accrualTypeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tenants = (await _tenantRepository.GetAllAsync()).OrderBy(t => t.Name);
            var accrualTypes = (await _accrualTypeRepository.GetAllAsync()).OrderBy(a => a.Name).ToList();
            accrualTypes.Add(new Domain.Entities.AccrualType() { Id = null, Name = "Сводный отчёт" });
            var tenantSelectList = new SelectList(tenants, "Id", "Name", tenants.First());
            var accrualTypesSelectList = new SelectList(accrualTypes, "Id", "Name", accrualTypes.First(a => String.IsNullOrEmpty(a.Id)));

            var vm = new HomeViewModel
            {
                Tenants = tenantSelectList,
                AccrualTypes = accrualTypesSelectList
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
