using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
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
            var connection = new SqlConnection(_connectionString);

            return connection.QuerySingleAsync<Tenant>("SELECT * FROM Tenants WHERE Id = @Id;", new { Id = id }).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<Tenant>("SELECT * FROM Tenants;").ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
        }

        public Tenant GetByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Tenant>("SELECT * FROM Tenants WHERE Name = @Name;", new { Name = name });
            }
        }

        public Task<Tenant> GetByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QuerySingleAsync<Tenant>("SELECT * FROM Tenants WHERE Name = @Name;", new { Name = name }).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
        }

        public void Add(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Tenants VALUES (@Id, @UserId, @Name, @Address, @PhoneNumber)", item);
            }
        }
        public Task AddAsync(Tenant item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("INSERT INTO Tenants VALUES (@Id, @UserId, @Name, @Address, @PhoneNumber)", item).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
        }

        public void Update(Tenant item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Tenants SET UserId = @UserId, Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Tenant item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("UPDATE Tenants SET UserId = @UserId, Name = @Name, Address = @Address, PhoneNumber = @PhoneNumber WHERE Id = @Id", item).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM Tenants WHERE Id = @Id", new { Id = id }).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
        }

        public void RemoveByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Tenants WHERE Name = @Name", new { Name = name });
            }
        }
        public Task RemoveByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM Tenants WHERE Name = @Name", new { Name = name }).ContinueWith(tenants =>
            {
                connection.Dispose();

                return tenants.Result;
            });
        }
    }
}
