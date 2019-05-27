using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Interfaces
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Tenant GetByName(string name);
        Task<Tenant> GetByNameAsync(string name);
        Task<Tenant> GetByUserIdAsync(string userId);

        void RemoveByName(string name);
        Task RemoveByNameAsync(string name);
    }
}
