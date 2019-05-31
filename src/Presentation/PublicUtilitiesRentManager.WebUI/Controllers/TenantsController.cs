using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    public class TenantsController : Controller
    {
        private readonly ITenantRepository _repository;

        public TenantsController(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() =>
            View((await _repository.GetAllAsync()).OrderBy(t => t.Name).Select(TenantViewModel.FromTenant));

        public async Task<ActionResult> Details(string id) =>
            View(TenantViewModel.FromTenant(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tenant tenant)
        {
            tenant.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(tenant);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tenant);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id) =>
            View(TenantViewModel.FromTenant(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return View(tenant);
            }

            try
            {
                await _repository.UpdateAsync(tenant);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tenant);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id) =>
            View(TenantViewModel.FromTenant(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            try
            {
                await _repository.RemoveByNameAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}