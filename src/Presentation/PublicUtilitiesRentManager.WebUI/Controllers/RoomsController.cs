using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
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
        public async Task<ActionResult> Create()
        {
            var roomTypes = await _roomTypeRepository.GetAllAsync();
            var emptyRoomViewModel = new RoomViewModel
            {
                RoomTypes = new SelectList(roomTypes, "Id", "Name", roomTypes.First())
            };

            return View(emptyRoomViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoomViewModel room)
        {
            room.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return View(room);
            }

            try
            {
                await _repository.AddAsync(RoomViewModel.FromRoomViewModel(room));

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
            var roomTypes = await _roomTypeRepository.GetAllAsync();

            room.RoomType = (await _roomTypeRepository.GetByIdAsync(room.RoomTypeId)).Name;
            room.RoomTypes = new SelectList(roomTypes, "Id", "Name", roomTypes.First(t => t.Id == room.RoomTypeId));

            return View(room);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoomViewModel room)
        {
            if (!ModelState.IsValid)
            {
                return View(room);
            }

            try
            {
                await _repository.UpdateAsync(RoomViewModel.FromRoomViewModel(room));

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

        [Authorize]
        public async Task<ActionResult> Free()
        {
            var rooms = (await _repository.GetAllAsync())
                .Where(r => !r.IsOccupied)
                .Select(RoomViewModel.FromRoom).ToList();
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
    }
}