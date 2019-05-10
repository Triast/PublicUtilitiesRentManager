using PublicUtilitiesRentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PublicUtilitiesRentManager.WebUI.Models
{
    public class CalcCoefficientViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Коэффициент")]
        public double Coefficient { get; set; }

        public static CalcCoefficientViewModel FromCalcCoefficient(CalcCoefficient calcCoefficient) =>
            new CalcCoefficientViewModel
            {
                Id = calcCoefficient.Id,
                Name = calcCoefficient.Name,
                Coefficient = calcCoefficient.Coefficient
            };
    }
}
