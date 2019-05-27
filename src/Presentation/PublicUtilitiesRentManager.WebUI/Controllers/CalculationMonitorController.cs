using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models;

namespace PublicUtilitiesRentManager.WebUI.Controllers
{
    public class CalculationMonitorController : Controller
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IAccrualTypeRepository _accrualTypeRepository;
        private readonly IRepository<Accrual> _accrualRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IContractRepository _contractRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public CalculationMonitorController(IAccrualTypeRepository accrualTypeRepository,
            ITenantRepository tenantRepository, IRepository<Accrual> accrualRepository,
            IRepository<Payment> paymentRepository, IContractRepository contractRepository,
            UserManager<ApplicationUser> userManager)
        {
            _accrualTypeRepository = accrualTypeRepository;
            _tenantRepository = tenantRepository;
            _accrualRepository = accrualRepository;
            _paymentRepository = paymentRepository;
            _contractRepository = contractRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Rent(DateTime start, DateTime end)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;
            List<Contract> contracts = null;

            var accrualType = await _accrualTypeRepository.GetByNameAsync("Аренда");

            if (userId != null)
            {
                var tenant = await _tenantRepository.GetByUserIdAsync(userId);
                contracts = (await _contractRepository.GetAllByTenantIdAndAccrualTypeIdAsync(tenant.Id, accrualType.Id))
                    .ToList();
            }

            var accruals = (await _accrualRepository.GetAllAsync()).Where(a => contracts.Exists(c => c.Id == a.ContractId));
            var payments = (await _paymentRepository.GetAllAsync()).Where(p => contracts.Exists(c => c.Id == p.ContractId));

            var openingBalance = accruals.Where(a => a.AccrualDate < start).Sum(a => a.Summ)
                - payments.Where(a => a.PaymentDate < start).Sum(p => p.Summ);

            var accrualsInPeriod = accruals.Where(a => a.AccrualDate >= start && a.AccrualDate <= end);
            var paymentsInPeriod = payments.Where(p => p.PaymentDate >= start && p.PaymentDate <= end);

            var closingBalance = openingBalance + accrualsInPeriod.Sum(a => a.Summ) - paymentsInPeriod.Sum(p => p.Summ);

            var viewModel = new CalculationMonitorViewModel
            {
                OpeningBalance = openingBalance,
                Accruals = accrualsInPeriod,
                Payments = paymentsInPeriod,
                ClosingBalance = closingBalance,
                Start = start,
                End = end
            };

            return View(viewModel);
        }
    }
}