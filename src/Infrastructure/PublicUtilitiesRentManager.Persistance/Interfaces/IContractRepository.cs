using PublicUtilitiesRentManager.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<IEnumerable<Contract>> GetAllByTenantIdAsync(string tenantId);
        Task<IEnumerable<Contract>> GetAllByTenantIdAndAccrualTypeIdAsync(string tenantId, string accrualTypeId);
    }
}
