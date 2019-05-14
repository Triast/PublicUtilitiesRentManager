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
    public class AccrualsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<Accrual> _accrualRepository;

        public AccrualsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IRepository<Contract> contractRepository,
            IRepository<Accrual> accrualRepository)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
            _accrualRepository = accrualRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            var accruals = await GetAccrualViewModels();

            if (!String.IsNullOrWhiteSpace(id))
            {
                var tenant = await _tenantRepository.GetByNameAsync(id);
                accruals = accruals.Where(a => a.Tenant == tenant.Name);
            }

            return View(accruals);
        }

        private async Task<IEnumerable<AccrualViewModel>> GetAccrualViewModels()
        {
            var accruals = (await _accrualRepository.GetAllAsync())
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