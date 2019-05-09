using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class CalcCoefficientViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Условие")]
        public string Condition { get; set; }
        [Display(Name = "Коэффициент")]
        public double Coefficient { get; set; }

        public static CalcCoefficientViewModel FromCalcCoefficient(CalcCoefficient calcCoefficient) =>
            new CalcCoefficientViewModel
            {
                Id = calcCoefficient.Id,
                Condition = calcCoefficient.Condition,
                Coefficient = calcCoefficient.Coefficient
            };
    }
}
