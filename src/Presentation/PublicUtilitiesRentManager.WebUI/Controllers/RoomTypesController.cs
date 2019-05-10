using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    [Authorize(Roles = "Administrator,Manager")]
    public class RoomTypesController : Controller
    {
        private readonly IRoomTypeRepository _repository;

        public RoomTypesController(IRoomTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index() =>
            View((await _repository.GetAllAsync()).Select(RoomTypeViewModel.FromRoomType));

        public async Task<ActionResult> Details(string id) =>
            View(RoomTypeViewModel.FromRoomType(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoomType RoomType)
        {
            RoomType.Id = System.Guid.NewGuid().ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.AddAsync(RoomType);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(RoomType);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(string id) =>
            View(RoomTypeViewModel.FromRoomType(await _repository.GetByNameAsync(id)));

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoomType RoomType)
        {
            if (!ModelState.IsValid)
            {
                return View(RoomType);
            }

            try
            {
                await _repository.UpdateAsync(RoomType);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(RoomType);
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(string id) =>
            View(RoomTypeViewModel.FromRoomType(await _repository.GetByNameAsync(id)));

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