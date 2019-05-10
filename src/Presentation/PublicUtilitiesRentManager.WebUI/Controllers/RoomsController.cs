using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    // Todo: Finish Create and Edit post methods.
    [Authorize(Roles = "Administrator,Manager")]
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _repository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomsController(IRoomRepository repository, IRoomTypeRepository roomTypeRepository)
        {
            _repository = repository;
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<ActionResult> Index()
        {
            var rooms = (await _repository.GetAllAsync()).Select(RoomViewModel.FromRoom).ToList();
            var getRoomTypeTasks = new List<Task>();

            foreach (var room in rooms)
            {
                getRoomTypeTasks.Add(_roomTypeRepository
                    .GetByIdAsync(room.RoomTypeId)
                    .ContinueWith(roomType => { room.RoomType = roomType.Result.Name; }));
            }

            await Task.WhenAll(getRoomTypeTasks);

            return View(rooms);
        }

        public async Task<ActionResult> Details(string id)
        {
            var room = RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id));

            room.RoomType = (await _roomTypeRepository.GetByIdAsync(room.RoomTypeId)).Name;

            return View(room);
        }

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
        public async Task<ActionResult> Edit(string id)
        {
            var room = RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id));

            room.RoomType = (await _roomTypeRepository.GetByIdAsync(room.RoomTypeId)).Name;

            return View(room);
        }

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
        public async Task<ActionResult> Delete(string id)
        {
            var room = RoomViewModel.FromRoom(await _repository.GetByAddressAsync(id));

            room.RoomType = (await _roomTypeRepository.GetByIdAsync(room.RoomTypeId)).Name;

            return View(room);
        }

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