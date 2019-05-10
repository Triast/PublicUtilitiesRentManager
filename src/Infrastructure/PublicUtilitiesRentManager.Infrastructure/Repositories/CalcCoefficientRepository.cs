using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class CalcCoefficientRepository : ICalcCoefficientRepository
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
                return connection
                    .QuerySingle<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<CalcCoefficient> GetByIdAsync(string id)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Id = @Id;", new { Id = id })
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<CalcCoefficient>("SELECT * FROM CalcCoefficients;")
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
        }

        public CalcCoefficient GetByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .QuerySingle<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Name = @Name;", new { Name = name });
            }
        }

        public Task<CalcCoefficient> GetByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<CalcCoefficient>("SELECT * FROM CalcCoefficients WHERE Name = @Name;", new { Name = name })
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
        }

        public void Add(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO CalcCoefficients VALUES (@Id, @Name, @Coefficient)", item);
            }
        }
        public Task AddAsync(CalcCoefficient item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("INSERT INTO CalcCoefficients VALUES (@Id, @Name, @Coefficient)", item)
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
        }

        public void Update(CalcCoefficient item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection
                    .Execute("UPDATE CalcCoefficients SET Name = @Name, Coefficient = @Coefficient WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(CalcCoefficient item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("UPDATE CalcCoefficients SET Name = @Name, Coefficient = @Coefficient WHERE Id = @Id", item).ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM CalcCoefficients WHERE Id = @Id", new { Id = id })
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
        }

        public void RemoveByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection
                    .Execute("DELETE FROM CalcCoefficients WHERE Name = @Name", new { Name = name });
            }
        }
        public Task RemoveByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .ExecuteAsync("DELETE FROM CalcCoefficients WHERE Name = @Name", new { Name = name })
                .ContinueWith(calcCoefficients =>
            {
                connection.Dispose();

                return calcCoefficients.Result;
            });
        }
    }
}
