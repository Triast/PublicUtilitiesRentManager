using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Interfaces
{
    public interface ICalcCoefficientRepository : IRepository<CalcCoefficient>
    {
        CalcCoefficient GetByName(string condition);
        Task<CalcCoefficient> GetByNameAsync(string condition);

        void RemoveByName(string condition);
        Task RemoveByNameAsync(string condition);
    }
}
