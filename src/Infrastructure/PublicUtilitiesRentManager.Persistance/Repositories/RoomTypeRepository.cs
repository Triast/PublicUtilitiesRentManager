using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class RoomTypeRepository : Repository<RoomType>, IRoomTypeRepository
    {
        private const string _sqlGetById = "SELECT * FROM RoomTypes WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM RoomTypes;";
        private const string _sqlGetByName = "SELECT * FROM RoomTypes WHERE Name = @Name;";
        private const string _sqlAdd = "INSERT INTO RoomTypes VALUES (@Id, @Name)";
        private const string _sqlUpdate = "UPDATE RoomTypes SET Name = @Name WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM RoomTypes WHERE Id = @Id";
        private const string _sqlRemoveByName = "DELETE FROM RoomTypes WHERE Name = @Name";

        public RoomTypeRepository(string connectionString) : base(connectionString) { }

        public RoomType GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<RoomType> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<RoomType> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<RoomType>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public RoomType GetByName(string name) => QuerySingle(_sqlGetByName, new { Name = name });
        public Task<RoomType> GetByNameAsync(string name) => QuerySingleAsync(_sqlGetByName, new { Name = name });
        public void Add(RoomType item) => Execute(_sqlAdd, item);
        public Task AddAsync(RoomType item) => ExecuteAsync(_sqlAdd, item);
        public void Update(RoomType item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(RoomType item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
        public void RemoveByName(string name) => Execute(_sqlRemoveByName, new { Name = name });
        public Task RemoveByNameAsync(string name) => ExecuteAsync(_sqlRemoveByName, new { Name = name });
    }
}
