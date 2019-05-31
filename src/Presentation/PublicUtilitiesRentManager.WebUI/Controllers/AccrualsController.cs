using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    [Authorize]
    public class AccrualsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly ICalcCoefficientRepository _calcCoefficientRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRepository<Accrual> _accrualRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public AccrualsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IContractRepository contractRepository,
            IRepository<Accrual> accrualRepository, ICalcCoefficientRepository calcCoefficientRepository,
            UserManager<ApplicationUser> userManager)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _calcCoefficientRepository = calcCoefficientRepository;
            _contractRepository = contractRepository;
            _accrualRepository = accrualRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string id)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;

            var accruals = await GetAccrualViewModels(userId);

            if (!String.IsNullOrWhiteSpace(id))
            {
                var tenant = await _tenantRepository.GetByNameAsync(id);
                accruals = accruals.Where(a => a.Tenant == tenant.Name);
            }

            return View(accruals);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Create(string contractId)
        {
            var contract = await _contractRepository.GetByIdAsync(contractId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);

            var sum = 0m;
            var rate = 0.0;

            switch (accrualType.Name)
            {
                case "Аренда":
                    rate = (await _calcCoefficientRepository
                        .GetByIdAsync("73163d80-28fa-45bf-8636-54dd4c33e5f1"))
                        .Coefficient;
                    sum = (decimal)room.Square * (decimal)rate *
                        room.ComfortCoef * room.IncreasingCoefToBaseRate *
                        room.PlacementCoef * room.SocialOrientationCoef *
                        room.Price;
                    break;
                case "Техобслуживание":
                    rate = (await _calcCoefficientRepository
                        .GetByIdAsync("e557209d-ce5e-4924-952f-3b1751e346dc"))
                        .Coefficient;
                    sum = (decimal)(room.Square * rate);
                    break;
                case "Капремонт":
                    rate = (await _calcCoefficientRepository
                        .GetByIdAsync("7cbbe0f0-b840-409f-aa38-4259f4e1ee82"))
                        .Coefficient;
                    sum = (decimal)(room.Square * rate);
                    break;
            }

            var accrual = new AccrualViewModel
            {
                ContractId = contractId,
                Tenant = (await _tenantRepository.GetByIdAsync(contract.TenantId)).Name,
                Room = room.Address,
                InvoiceNumber = 0,
                AccrualType = (await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId)).Name,
                AccrualDate = DateTime.Now.Date,
                Summ = sum
            };

            return View(accrual);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccrualViewModel accrualVM)
        {
            accrualVM.Id = System.Guid.NewGuid().ToString();
            var accrual = AccrualViewModel.FromAccrualViewModel(accrualVM);

            if (!ModelState.IsValid)
            {
                return View(accrualVM);
            }

            try
            {
                await _accrualRepository.AddAsync(accrual);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                return View(accrualVM);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Edit(string id)
        {
            var accrual = await _accrualRepository.GetByIdAsync(id);
            var contract = await _contractRepository.GetByIdAsync(accrual.ContractId);
            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);

            var vm = AccrualViewModel.FromAccrual(accrual);
            vm.Tenant = tenant.Name;
            vm.AccrualType = accrualType.Name;
            vm.Room = room.Address;

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccrualViewModel accrual)
        {
            if (!ModelState.IsValid)
            {
                return View(accrual);
            }

            try
            {
                await _accrualRepository.UpdateAsync(AccrualViewModel.FromAccrualViewModel(accrual));

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(accrual);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Delete(string id)
        {
            var accrual = await _accrualRepository.GetByIdAsync(id);
            var contract = await _contractRepository.GetByIdAsync(accrual.ContractId);
            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);

            var vm = AccrualViewModel.FromAccrual(accrual);
            vm.Tenant = tenant.Name;
            vm.AccrualType = accrualType.Name;
            vm.Room = room.Address;

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            try
            {
                await _accrualRepository.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<IEnumerable<AccrualViewModel>> GetAccrualViewModels(string userId)
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var accruals = (await _accrualRepository.GetAllAsync())
                .Where(a => userId != null ? tenants.First(t => t.Id == contracts.First(c => c.Id == a.ContractId).TenantId).UserId == userId : true)
                .Select(AccrualViewModel.FromAccrual)
                .ToList();
            var getTasks = new List<Task>();

            foreach (var accrual in accruals)
            {
                var contract = await _contractRepository.GetByIdAsync(accrual.ContractId);

                getTasks.Add(_tenantRepository
                    .GetByIdAsync(contract.TenantId)
                    .ContinueWith(t => { accrual.Tenant = t.Result.Name; }));
                getTasks.Add(_roomRepository
                    .GetByIdAsync(contract.RoomId)
                    .ContinueWith(r => { accrual.Room = r.Result.Address; }));
                getTasks.Add(_accrualTypeRepository
                    .GetByIdAsync(contract.AccrualTypeId)
                    .ContinueWith(t => { accrual.AccrualType = t.Result.Name; }));
            }

            await Task.WhenAll(getTasks);

            return accruals;
        }
    }
}