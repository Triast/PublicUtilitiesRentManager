using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Interfaces
{
    public interface IAccrualTypeRepository : IRepository<AccrualType>
    {
        AccrualType GetByName(string name);
        Task<AccrualType> GetByNameAsync(string name);

        void RemoveByName(string name);
        Task RemoveByNameAsync(string name);
    }
}
