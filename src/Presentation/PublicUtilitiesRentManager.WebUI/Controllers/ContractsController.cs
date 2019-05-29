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
    // Todo: add CRUD.
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

            return View(contracts);
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