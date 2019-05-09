using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _repository;

        public RoomsController(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() =>
            View((await _repository.GetAllAsync()).Select(RoomViewModel.FromRoom));

        public async Task<ActionResult> Details(string id) =>
            View(RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id)));

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Room room)
        {
            room.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(room);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(room);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id) =>
            View(RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id)));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Room room)
        {
            if (!ModelState.IsValid)
            {
                return View(room);
            }

            try
            {
                await _repository.UpdateAsync(room);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(room);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id) =>
            View(RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id)));

        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            try
            {
                await _repository.RemoveByAddressAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}