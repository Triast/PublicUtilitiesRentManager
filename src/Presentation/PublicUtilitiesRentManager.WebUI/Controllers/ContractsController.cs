using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IRepository<Contract> _contractRepository;

        public ContractsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IRepository<Contract> contractRepository
            )
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
        }

        // id parameter is used as tenant search.
        public async Task<IActionResult> Index(string id)
        {
            var contracts = await GetContractViewModels();

            if (!String.IsNullOrWhiteSpace(id))
            {
                var tenantObj = await _tenantRepository.GetByNameAsync(id);
                contracts = contracts.Where(c => c.TenantId == tenantObj.Id);
            }

            return View(contracts);
        }

        private async Task<IEnumerable<ContractViewModel>> GetContractViewModels()
        {
            var contracts = (await _contractRepository.GetAllAsync())
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