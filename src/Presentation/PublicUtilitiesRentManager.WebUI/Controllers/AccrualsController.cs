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
    //Todo: add breadcrumbs for views.
    [Authorize]
    public class AccrualsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRepository<Accrual> _accrualRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public AccrualsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IContractRepository contractRepository,
            IRepository<Accrual> accrualRepository, UserManager<ApplicationUser> userManager)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
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
            var accrual = new AccrualViewModel
            {
                ContractId = contractId,
                Tenant = (await _tenantRepository.GetByIdAsync(contract.TenantId)).Name,
                Room = (await _roomRepository.GetByIdAsync(contract.RoomId)).Address,
                AccrualType = (await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId)).Name,
                AccrualDate = DateTime.Now.Date
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