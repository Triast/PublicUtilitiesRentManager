using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class AccrualTypeRepository : IRepository<AccrualType>
    {
        private readonly string _connectionString;

        public AccrualTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AccrualType GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<AccrualType>("SELECT * FROM AccrualTypes WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<AccrualType> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<AccrualType>("SELECT * FROM AccrualTypes WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<AccrualType> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<AccrualType>("SELECT * FROM AccrualTypes;");
            }
        }

        public Task<IEnumerable<AccrualType>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryAsync<AccrualType>("SELECT * FROM AccrualTypes;");
            }
        }

        public void Add(AccrualType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO AccrualTypes VALUES (@Id, @Name)", item);
            }
        }
        public Task AddAsync(AccrualType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO AccrualTypes VALUES (@Id, @Name)", item);
            }
        }

        public void Update(AccrualType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE AccrualTypes SET Name = @Name WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(AccrualType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE AccrualTypes SET Name = @Name WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM AccrualTypes WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM AccrualTypes WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
