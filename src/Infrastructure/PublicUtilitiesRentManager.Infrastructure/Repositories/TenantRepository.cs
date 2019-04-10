using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class TenantRepository : IRepository<Tenant>
    {
        private readonly string _connectionString;

        public TenantRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Tenant GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Tenant>("SELECT * FROM Tenants WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Tenant> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<Tenant>("SELECT * FROM Tenants WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<Tenant> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Tenant>("SELECT * FROM Tenants;");
            }
        }

        public Task<IEnumerable<Tenant>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryAsync<Tenant>("SELECT * FROM Tenants;");
            }
        }

        public void Add(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Tenants VALUES (@Id, @Name, @Address, @PhoneNumber)", item);
            }
        }
        public Task AddAsync(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO Tenants VALUES (@Id, @Name, @Address, @PhoneNumber)", item);
            }
        }

        public void Update(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Tenants SET Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE Tenants SET Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Tenants WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM Tenants WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
