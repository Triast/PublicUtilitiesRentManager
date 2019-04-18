using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Interfaces
{
    public interface ICalcCoefficientRepository : IRepository<CalcCoefficient>
    {
        CalcCoefficient GetByCondition(string condition);
        Task<CalcCoefficient> GetByConditionAsync(string condition);

        void RemoveByCondition(string condition);
        Task RemoveByConditionAsync(string condition);
    }
}
