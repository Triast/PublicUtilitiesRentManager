using PublicUtilitiesRentManager.Domain.Entities;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Interfaces
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        RoomType GetByName(string name);
        Task<RoomType> GetByNameAsync(string name);

        void RemoveByName(string name);
        Task RemoveByNameAsync(string name);
    }
}
