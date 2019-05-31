using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    // Todo: Add authentication rules for controllers. Redesign revise act.
    public class ReviseActController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRepository<Accrual> _accrualRepository;
        private readonly IRepository<Payment> _paymentRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public ReviseActController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IContractRepository contractRepository,
            IRepository<Accrual> accrualRepository, IRepository<Payment> paymentRepository,
            UserManager<ApplicationUser> userManager)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
            _accrualRepository = accrualRepository;
            _paymentRepository = paymentRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string id, string accrualType, DateTime? from, DateTime? to)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;

            var accruals = (await GetAccrualViewModels(userId)).Select(GenericAct.FromAccrualViewModel);
            var payments = (await GetPaymentViewModels(userId)).Select(GenericAct.FromPaymentViewModel);
            var acts = new List<GenericAct>();

            foreach (var accrual in accruals)
            {
                var act = acts.FirstOrDefault(a => a.Date.Month == accrual.Date.Month && a.Date.Year == accrual.Date.Year);

                if (act != null)
                {
                    act.Summ += accrual.Summ;
                }
                else
                {
                    acts.Add(accrual);
                }
            }
            acts.AddRange(payments);

            var actsFiltered = acts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(id))
            {
                var tenant = await _tenantRepository.GetByIdAsync(id);
                actsFiltered = actsFiltered.Where(a => a.Tenant == tenant.Name);
            }

            var fromDate = from?.Date ?? new DateTime();
            var toDate = to?.Date ?? DateTime.Now.Date;

            actsFiltered = actsFiltered.Where(a => a.Date >= fromDate && a.Date <= toDate);
            return View(actsFiltered.OrderBy(a => a.Date));
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
                var contract = contracts.First(c => c.Id == accrual.ContractId);

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

        private async Task<IEnumerable<PaymentViewModel>> GetPaymentViewModels(string userId)
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var payments = (await _paymentRepository.GetAllAsync())
                .Where(a => userId != null ? tenants.First(t => t.Id == contracts.First(c => c.Id == a.ContractId).TenantId).UserId == userId : true)
                .Select(PaymentViewModel.FromPayment)
                .ToList();
            var getTasks = new List<Task>();

            foreach (var payment in payments)
            {
                var contract = contracts.First(c => c.Id == payment.ContractId);

                getTasks.Add(_tenantRepository
                    .GetByIdAsync(contract.TenantId)
                    .ContinueWith(t => { payment.Tenant = t.Result.Name; }));
                getTasks.Add(_roomRepository
                    .GetByIdAsync(contract.RoomId)
                    .ContinueWith(r => { payment.Room = r.Result.Address; }));
                getTasks.Add(_accrualTypeRepository
                    .GetByIdAsync(contract.AccrualTypeId)
                    .ContinueWith(t => { payment.AccrualType = t.Result.Name; }));
            }

            await Task.WhenAll(getTasks);

            return payments;
        }
    }
}