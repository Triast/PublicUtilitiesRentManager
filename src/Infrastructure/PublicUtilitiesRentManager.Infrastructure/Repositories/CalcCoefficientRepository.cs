using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class CalcCoefficientRepository : IRepository<CalcCoefficient>
    {
        private readonly string _connectionString;

        public CalcCoefficientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CalcCoefficient GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<CalcCoefficient> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<CalcCoefficient> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CalcCoefficient>("SELECT * FROM CalcCoefficients;");
            }
        }

        public Task<IEnumerable<CalcCoefficient>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryAsync<CalcCoefficient>("SELECT * FROM CalcCoefficients;");
            }
        }

        public void Add(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO CalcCoefficients VALUES (@Id, @Condition, @Coefficient)", item);
            }
        }
        public Task AddAsync(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO CalcCoefficients VALUES (@Id, @Condition, @Coefficient)", item);
            }
        }

        public void Update(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE CalcCoefficients SET Name = @Name, Condition = @Condition, Coefficient = @Coefficient WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE CalcCoefficients SET Name = @Name, Condition = @Condition, Coefficient = @Coefficient WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM CalcCoefficients WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM CalcCoefficients WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
