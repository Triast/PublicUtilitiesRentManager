using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IContractRepository _contractRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public ContractsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IContractRepository contractRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
            _userManager = userManager;
        }

        // id parameter is used as tenant search.
        public async Task<IActionResult> Index(string id)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;

            var contracts = await GetContractViewModels(userId);

            if (!String.IsNullOrWhiteSpace(id))
            {
                var tenantObj = await _tenantRepository.GetByNameAsync(id);
                contracts = contracts.Where(c => c.TenantId == tenantObj.Id);
            }

            return View(contracts.OrderBy(c => c.Tenant));
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Create()
        {
            var tenants = (await _tenantRepository.GetAllAsync()).OrderBy(t => t.Name);
            var rooms = (await _roomRepository.GetAllAsync()).Where(r => !r.IsOccupied).OrderBy(r => r.Address);
            var accrualTypes = (await _accrualTypeRepository.GetAllAsync()).OrderBy(t => t.Name);

            var tenantsSelectList = new SelectList(tenants, "Id", "Name", tenants.First());
            var roomsSelectList = new SelectList(rooms, "Id", "Address", rooms.First());
            var accrualTypesSelectList = new SelectList(accrualTypes, "Id", "Name", accrualTypes.First());

            var vm = new ContractViewModel
            {
                Tenants = tenantsSelectList,
                Rooms = roomsSelectList,
                AccrualTypes = accrualTypesSelectList
            };

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContractViewModel contract)
        {
            contract.Id = System.Guid.NewGuid().ToString();
            var accrual = ContractViewModel.FromContractViewModel(contract);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);
            room.IsOccupied = true;

            if (!ModelState.IsValid)
            {
                return View(contract);
            }

            try
            {
                await _contractRepository.AddAsync(accrual);
                await _roomRepository.UpdateAsync(room);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                var tenants = (await _tenantRepository.GetAllAsync()).OrderBy(t => t.Name);
                var rooms = (await _roomRepository.GetAllAsync()).Where(r => !r.IsOccupied).OrderBy(r => r.Address);
                var accrualTypes = (await _accrualTypeRepository.GetAllAsync()).OrderBy(t => t.Name);

                var tenantsSelectList = new SelectList(tenants, "Id", "Name", tenants.First(t => t.Id == contract.TenantId));
                var roomsSelectList = new SelectList(rooms, "Id", "Address", rooms.First(r => r.Id == contract.RoomId));
                var accrualTypesSelectList = new SelectList(accrualTypes, "Id", "Name", accrualTypes.First(t => t.Id == contract.AccrualTypeId));

                return View(contract);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Edit(string id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);

            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);

            var vm = ContractViewModel.FromContract(contract);
            vm.Tenant = tenant.Name;
            vm.Room = room.Address;
            vm.AccrualType = accrualType.Name;

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContractViewModel contract)
        {
            if (!ModelState.IsValid)
            {
                return View(contract);
            }

            try
            {
                await _contractRepository.UpdateAsync(ContractViewModel.FromContractViewModel(contract));

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contract);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Delete(string id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);

            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);

            var vm = ContractViewModel.FromContract(contract);
            vm.Tenant = tenant.Name;
            vm.Room = room.Address;
            vm.AccrualType = accrualType.Name;

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);
            room.IsOccupied = false;

            try
            {
                await _contractRepository.RemoveAsync(id);
                await _roomRepository.UpdateAsync(room);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<IEnumerable<ContractViewModel>> GetContractViewModels(string userId)
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var contracts = (await _contractRepository.GetAllAsync())
                .Where(c => userId != null ? tenants.First(t => t.Id == c.TenantId).UserId == userId : true)
                .Select(ContractViewModel.FromContract)
                .ToList();

            var getTasks = new List<Task>();

            foreach (var contract in contracts)
            {
                getTasks.Add(_tenantRepository
                    .GetByIdAsync(contract.TenantId)
                    .ContinueWith(t => { contract.Tenant = t.Result.Name; }));
                getTasks.Add(_roomRepository
                    .GetByIdAsync(contract.RoomId)
                    .ContinueWith(r => { contract.Room = r.Result.Address; }));
                getTasks.Add(_accrualTypeRepository
                    .GetByIdAsync(contract.AccrualTypeId)
                    .ContinueWith(t => { contract.AccrualType = t.Result.Name; }));
            }

            await Task.WhenAll(getTasks);

            return contracts;
        }
    }
}