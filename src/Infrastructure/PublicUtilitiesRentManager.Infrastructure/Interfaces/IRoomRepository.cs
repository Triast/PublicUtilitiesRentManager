using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Room GetByAddress(string address);
        Task<Room> GetByAddressAsync(string address);

        void RemoveByAddress(string address);
        Task RemoveByAddressAsync(string address);
    }
}
