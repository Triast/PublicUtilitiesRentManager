﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class CalculationMonitorViewModel
    {
        public string TenantId { get; set; }
        public string AccrualTypeName { get; set; }
        [Display(Name = "Остаток на начало периода")]
        public decimal OpeningBalance { get; set; }
        public IEnumerable<AccrualViewModel> Accruals { get; set; }
        public IEnumerable<PaymentViewModel> Payments { get; set; }
        [Display(Name = "Остаток на конец периода")]
        public decimal ClosingBalance { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
