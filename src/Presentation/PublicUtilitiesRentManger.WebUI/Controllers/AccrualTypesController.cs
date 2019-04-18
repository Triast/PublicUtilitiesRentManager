using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManger.WebUI.Controllers
{
    public class AccrualTypesController : Controller
    {
        private readonly IAccrualTypeRepository _repository;

        public AccrualTypesController(IAccrualTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() => View(await _repository.GetAllAsync());

        public async Task<ActionResult> Details(string id) => View(await _repository.GetByNameAsync(id));

        public ActionResult Create()
        {
            return View();
        }

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

        public async Task<ActionResult> Edit(string id)
        {
            var accrualType = await _repository.GetByNameAsync(id);

            return View(accrualType);
        }

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

        public async Task<ActionResult> Delete(string id) => View(await _repository.GetByNameAsync(id));

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