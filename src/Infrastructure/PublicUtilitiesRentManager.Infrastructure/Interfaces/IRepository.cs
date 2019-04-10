using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        void Add(T item);
        Task AddAsync(T item);
        void Update(T item);
        Task UpdateAsync(T item);
        void Remove(string id);
        Task RemoveAsync(string id);
    }
}
