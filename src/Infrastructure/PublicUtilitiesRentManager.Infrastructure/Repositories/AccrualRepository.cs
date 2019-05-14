using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class AccrualRepository : IRepository<Accrual>
    {
        private readonly string _connectionString;

        public AccrualRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Accrual GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Accrual>("SELECT * FROM Accruals WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Accrual> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<Accrual>("SELECT * FROM Accruals WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<Accrual> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Accrual>("SELECT * FROM Accruals;");
            }
        }

        public Task<IEnumerable<Accrual>> GetAllAsync()
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<Accrual>("SELECT * FROM Accruals;")
                .ContinueWith(accruals =>
                {
                    connection.Dispose();

                    return accruals.Result;
                });
        }

        public void Add(Accrual item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Accruals VALUES (@Id, @ContractId, @AccrualDate, @Summ)", item);
            }
        }
        public Task AddAsync(Accrual item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO Accruals VALUES (@Id, @ContractId, @AccrualDate, @Summ)", item);
            }
        }

        public void Update(Accrual item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Accruals SET ContractId = @ContractId, AccrualDate = @AccrualDate, Summ = @Summ WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Accrual item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE Accruals SET ContractId = @ContractId, AccrualDate = @AccrualDate, Summ = @Summ WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Accruals WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM Accruals WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
