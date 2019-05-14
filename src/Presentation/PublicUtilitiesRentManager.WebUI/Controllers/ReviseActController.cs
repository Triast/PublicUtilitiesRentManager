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
    public class ReviseActController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<Accrual> _accrualRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public ReviseActController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IRepository<Contract> contractRepository,
            IRepository<Accrual> accrualRepository, IRepository<Payment> paymentRepository)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
            _accrualRepository = accrualRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var accruals = (await GetAccrualViewModels()).Select(GenericAct.FromAccrualViewModel);
            var payments = (await GetPaymentViewModels()).Select(GenericAct.FromPaymentViewModel);
            var acts = new List<GenericAct>();

            acts.AddRange(accruals);
            acts.AddRange(payments);

            return View(acts.OrderBy(a => a.Date));
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

        private async Task<IEnumerable<PaymentViewModel>> GetPaymentViewModels()
        {
            var payments = (await _paymentRepository.GetAllAsync())
                .Select(PaymentViewModel.FromPayment)
                .ToList();
            var getTasks = new List<Task>();

            foreach (var payment in payments)
            {
                var contract = await _contractRepository.GetByIdAsync(payment.ContractId);

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