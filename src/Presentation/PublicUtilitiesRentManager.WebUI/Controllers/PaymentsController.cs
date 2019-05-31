using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRepository<Payment> _paymentRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(
            ITenantRepository tenantRepository, IRoomRepository roomRepository,
            IAccrualTypeRepository accrualTypeRepository, IContractRepository contractRepository,
            IRepository<Payment> paymentRepository, UserManager<ApplicationUser> userManager)
        {
            _tenantRepository = tenantRepository;
            _roomRepository = roomRepository;
            _accrualTypeRepository = accrualTypeRepository;
            _contractRepository = contractRepository;
            _paymentRepository = paymentRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string id)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;

            var payments = await GetPaymentViewModels(userId);

            if (!String.IsNullOrWhiteSpace(id))
            {
                var tenant = await _tenantRepository.GetByNameAsync(id);
                payments = payments.Where(a => a.Tenant == tenant.Name);
            }

            return View(payments);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Create(string contractId)
        {
            var contract = await _contractRepository.GetByIdAsync(contractId);
            var payment = new PaymentViewModel
            {
                ContractId = contractId,
                Tenant = (await _tenantRepository.GetByIdAsync(contract.TenantId)).Name,
                Room = (await _roomRepository.GetByIdAsync(contract.RoomId)).Address,
                AccrualType = (await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId)).Name,
                PaymentOrderNumber = 0,
                PaymentDate = DateTime.Now.Date
            };

            return View(payment);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PaymentViewModel paymentVM)
        {
            paymentVM.Id = System.Guid.NewGuid().ToString();
            var payment = PaymentViewModel.FromPaymentViewModel(paymentVM);

            if (!ModelState.IsValid)
            {
                return View(paymentVM);
            }

            try
            {
                await _paymentRepository.AddAsync(payment);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                return View(paymentVM);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Edit(string id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            var contract = await _contractRepository.GetByIdAsync(payment.ContractId);
            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);

            var vm = PaymentViewModel.FromPayment(payment);
            vm.Tenant = tenant.Name;
            vm.AccrualType = accrualType.Name;
            vm.Room = room.Address;

            return View(vm);
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PaymentViewModel payment)
        {
            if (!ModelState.IsValid)
            {
                return View(payment);
            }

            try
            {
                await _paymentRepository.UpdateAsync(PaymentViewModel.FromPaymentViewModel(payment));

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(payment);
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<ActionResult> Delete(string id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            var contract = await _contractRepository.GetByIdAsync(payment.ContractId);
            var tenant = await _tenantRepository.GetByIdAsync(contract.TenantId);
            var accrualType = await _accrualTypeRepository.GetByIdAsync(contract.AccrualTypeId);
            var room = await _roomRepository.GetByIdAsync(contract.RoomId);

            var vm = PaymentViewModel.FromPayment(payment);
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
                await _paymentRepository.RemoveAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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