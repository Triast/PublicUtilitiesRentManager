using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManger.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    public class CalcCoefficientsController : Controller
    {
        private readonly ICalcCoefficientRepository _repository;

        public CalcCoefficientsController(ICalcCoefficientRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() => View(await _repository.GetAllAsync());

        public async Task<ActionResult> Details(string id) => View(await _repository.GetByConditionAsync(id));

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CalcCoefficient calcCoefficient)
        {
            calcCoefficient.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(calcCoefficient);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(calcCoefficient);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            var calcCoefficient = await _repository.GetByConditionAsync(id);

            return View(calcCoefficient);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CalcCoefficient calcCoefficient)
        {
            if (!ModelState.IsValid)
            {
                return View(calcCoefficient);
            }

            try
            {
                await _repository.UpdateAsync(calcCoefficient);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(calcCoefficient);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id) => View(await _repository.GetByConditionAsync(id));

        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            try
            {
                await _repository.RemoveByConditionAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}