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

        public async Task<IActionResult> Index(string id, string accrualTypeId, DateTime start, DateTime end)
        {
            string userId = User.IsInRole("User") ? _userManager.GetUserId(User) : null;
            List<Contract> contracts = new List<Contract>();

            var accrualType = accrualTypeId != null ? await _accrualTypeRepository.GetByIdAsync(accrualTypeId) : null;

            if (userId != null || id != null)
            {
                var tenant = userId != null ? await _tenantRepository.GetByUserIdAsync(userId) : await _tenantRepository.GetByIdAsync(id);

                if (accrualType != null)
                {
                    contracts = (await _contractRepository.GetAllByTenantIdAndAccrualTypeIdAsync(tenant.Id, accrualType.Id))
                        .ToList();
                }
                else
                {
                    var entries = new List<CalculationMonitorEntryViewModel>();
                    var accrualTypes = await _accrualTypeRepository.GetAllAsync();

                    foreach (var type in accrualTypes)
                    {
                        var contractsByAccrualType = (await _contractRepository.GetAllByTenantIdAndAccrualTypeIdAsync(tenant.Id, type.Id))
                            .ToList();

                        var accrualsByAccrualType = (await _accrualRepository.GetAllAsync())
                            .Where(a => contractsByAccrualType.Exists(c => c.Id == a.ContractId));
                        var paymentsByAccrualType = (await _paymentRepository.GetAllAsync())
                            .Where(p => contractsByAccrualType.Exists(c => c.Id == p.ContractId));

                        var openingBalanceByAccrualType = accrualsByAccrualType.Where(a => a.AccrualDate < start).Sum(a => a.Summ)
                            - paymentsByAccrualType.Where(a => a.PaymentDate < start).Sum(p => p.Summ);
                        var accrualsSum = accrualsByAccrualType
                            .Where(a => a.AccrualDate >= start && a.AccrualDate <= end)
                            .Sum(a => a.Summ);
                        var paymentsSum = paymentsByAccrualType
                            .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
                            .Sum(p => p.Summ);

                        entries.Add(new CalculationMonitorEntryViewModel
                        {
                            TenantId = tenant.Id,
                            AccrualTypeId = type.Id,
                            AccrualTypeName = type.Name,
                            OpeningBalance = openingBalanceByAccrualType,
                            AccrualsSum = accrualsSum,
                            PaymentsSum = paymentsSum,
                            ClosingBalance = openingBalanceByAccrualType + accrualsSum - paymentsSum
                        });
                    }

                    ViewBag.StartDate = start;
                    ViewBag.EndDate = end;

                    return View("EntireMonitor", entries.OrderBy(e => e.AccrualTypeName));
                }
            }

            var accruals = (await _accrualRepository.GetAllAsync()).Where(a => contracts.Exists(c => c.Id == a.ContractId));
            var payments = (await _paymentRepository.GetAllAsync()).Where(p => contracts.Exists(c => c.Id == p.ContractId));

            var openingBalance = accruals.Where(a => a.AccrualDate < start).Sum(a => a.Summ)
                - payments.Where(a => a.PaymentDate < start).Sum(p => p.Summ);

            var accrualsInPeriod = accruals
                .Where(a => a.AccrualDate >= start && a.AccrualDate <= end)
                .OrderBy(a => a.AccrualDate);
            var paymentsInPeriod = payments
                .Where(p => p.PaymentDate >= start && p.PaymentDate <= end)
                .OrderBy(p => p.PaymentDate);

            var closingBalance = openingBalance + accrualsInPeriod.Sum(a => a.Summ) - paymentsInPeriod.Sum(p => p.Summ);

            var viewModel = new CalculationMonitorViewModel
            {
                TenantId = id,
                AccrualTypeName = accrualType?.Name ?? "Сводный",
                OpeningBalance = openingBalance,
                Accruals = accrualsInPeriod.Select(AccrualViewModel.FromAccrual),
                Payments = paymentsInPeriod.Select(PaymentViewModel.FromPayment),
                ClosingBalance = closingBalance,
                Start = start,
                End = end
            };

            return View(viewModel);
        }
    }
}