using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        private const string _sqlGetById = "SELECT * FROM Tenants WHERE Id = @Id;";
        private const string _sqlGetByUserId = "SELECT * FROM Tenants WHERE UserId = @UserId;";
        private const string _sqlGetAll = "SELECT * FROM Tenants;";
        private const string _sqlGetByName = "SELECT * FROM Tenants WHERE Name = @Name;";
        private const string _sqlAdd = "INSERT INTO Tenants VALUES (@Id, @UserId, @Name, @Address, @PhoneNumber)";
        private const string _sqlUpdate = "UPDATE Tenants SET UserId = @UserId, Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM Tenants WHERE Id = @Id";
        private const string _sqlRemoveByName = "DELETE FROM Tenants WHERE Name = @Name";

        public TenantRepository(string connectionString) : base(connectionString) { }

        public Tenant GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<Tenant> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public Task<Tenant> GetByUserIdAsync(string userId) =>
            QuerySingleAsync(_sqlGetByUserId, new { UserId = userId });
        public IEnumerable<Tenant> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<Tenant>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public Tenant GetByName(string name) => QuerySingle(_sqlGetByName, new { Name = name });
        public Task<Tenant> GetByNameAsync(string name) => QuerySingleAsync(_sqlGetByName, new { Name = name });
        public void Add(Tenant item) => Execute(_sqlAdd, item);
        public Task AddAsync(Tenant item) => ExecuteAsync(_sqlAdd, item);
        public void Update(Tenant item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(Tenant item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
        public void RemoveByName(string name) => Execute(_sqlRemoveByName, new { Name = name });
        public Task RemoveByNameAsync(string name) => ExecuteAsync(_sqlRemoveByName, new { Name = name });
    }
}
