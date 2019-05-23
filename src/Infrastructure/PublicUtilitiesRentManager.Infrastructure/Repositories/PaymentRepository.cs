using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class PaymentRepository :  IRepository<Payment>
    {
        private readonly string _connectionString;

        public PaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Payment GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Payment>("SELECT * FROM Payments WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Payment> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<Payment>("SELECT * FROM Payments WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Payment>("SELECT * FROM Payments;");
            }
        }

        public Task<IEnumerable<Payment>> GetAllAsync()
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<Payment>("SELECT * FROM Payments;")
                .ContinueWith(payments =>
                {
                    connection.Dispose();

                    return payments.Result;
                });
        }

        public void Add(Payment item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Payments VALUES (@Id, @ContractId, @PaymentDate, @Summ)", item);
            }
        }
        public Task AddAsync(Payment item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("INSERT INTO Payments VALUES (@Id, @ContractId, @PaymentDate, @Summ)", item)
                .ContinueWith(accruals =>
                {
                    connection.Dispose();

                    return accruals.Result;
                });
        }

        public void Update(Payment item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Payments SET ContractId = @ContractId, PaymentDate = @PaymentDate, Summ = @Summ WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Payment item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE Payments SET ContractId = @ContractId, PaymentDate = @PaymentDate, Summ = @Summ WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Payments WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM Payments WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
