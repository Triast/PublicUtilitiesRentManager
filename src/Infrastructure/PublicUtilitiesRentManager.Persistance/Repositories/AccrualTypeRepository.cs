using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class AccrualTypeRepository : Repository<AccrualType>, IAccrualTypeRepository
    {
        private const string _sqlGetById = "SELECT * FROM AccrualTypes WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM AccrualTypes;";
        private const string _sqlGetByName = "SELECT * FROM AccrualTypes WHERE Name = @Name;";
        private const string _sqlAdd = "INSERT INTO AccrualTypes VALUES (@Id, @Name)";
        private const string _sqlUpdate = "UPDATE AccrualTypes SET Name = @Name WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM AccrualTypes WHERE Id = @Id";
        private const string _sqlRemoveByName = "DELETE FROM AccrualTypes WHERE Name = @Name";

        public AccrualTypeRepository(string connectionString) : base(connectionString) { }

        public AccrualType GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<AccrualType> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<AccrualType> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<AccrualType>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public AccrualType GetByName(string name) => QuerySingle(_sqlGetByName, new { Name = name });
        public Task<AccrualType> GetByNameAsync(string name) =>
            QuerySingleAsync(_sqlGetByName, new { Name = name });
        public void Add(AccrualType item) => Execute(_sqlAdd, item);
        public Task AddAsync(AccrualType item) => ExecuteAsync(_sqlAdd, item);
        public void Update(AccrualType item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(AccrualType item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
        public void RemoveByName(string name) => Execute(_sqlRemoveByName, new { Name = name });
        public Task RemoveByNameAsync(string name) => ExecuteAsync(_sqlRemoveByName, new { Name = name });
    }
}
