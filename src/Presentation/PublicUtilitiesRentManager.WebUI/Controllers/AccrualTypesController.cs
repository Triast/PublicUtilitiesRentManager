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
    public class AccrualTypesController : Controller
    {
        private readonly IAccrualTypeRepository _repository;

        public AccrualTypesController(IAccrualTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() =>
            View((await _repository.GetAllAsync()).Select(AccrualTypeViewModel.FromAccrualType));

        public async Task<ActionResult> Details(string id) =>
            View(AccrualTypeViewModel.FromAccrualType(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccrualType accrualType)
        {
            accrualType.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(accrualType);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(accrualType);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id) =>
            View(AccrualTypeViewModel.FromAccrualType(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccrualType accrualType)
        {
            if (!ModelState.IsValid)
            {
                return View(accrualType);
            }

            try
            {
                await _repository.UpdateAsync(accrualType);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(accrualType);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id) =>
            View(AccrualTypeViewModel.FromAccrualType(await _repository.GetByNameAsync(id)));

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