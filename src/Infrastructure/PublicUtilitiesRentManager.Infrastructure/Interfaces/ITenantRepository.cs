using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Interfaces
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Tenant GetByName(string name);
        Task<Tenant> GetByNameAsync(string name);

        void RemoveByName(string name);
        Task RemoveByNameAsync(string name);
    }
}
