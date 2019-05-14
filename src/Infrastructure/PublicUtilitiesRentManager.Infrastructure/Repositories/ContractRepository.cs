using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class ContractRepository : IRepository<Contract>
    {
        private readonly string _connectionString;

        public ContractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Contract GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Contract>("SELECT * FROM Contracts WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Contract> GetByIdAsync(string id)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QuerySingleAsync<Contract>("SELECT * FROM Contracts WHERE Id = @Id;", new { Id = id })
                .ContinueWith(contracts =>
                {
                    connection.Dispose();

                    return contracts.Result;
                });
        }

        public IEnumerable<Contract> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Contract>("SELECT * FROM Contracts;");
            }
        }

        public Task<IEnumerable<Contract>> GetAllAsync()
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<Contract>("SELECT * FROM Contracts;")
                .ContinueWith(contracts =>
                {
                    connection.Dispose();

                    return contracts.Result;
                });
        }

        public void Add(Contract item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Contracts VALUES (@Id, @TenantId, @RoomId, @AccrualTypeId, @CalcCoefficientId, @StartDate, @EndDate)", item);
            }
        }
        public Task AddAsync(Contract item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO Contracts VALUES (@Id, @TenantId, @RoomId, @AccrualTypeId, @CalcCoefficientId, @StartDate, @EndDate)", item);
            }
        }

        public void Update(Contract item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(@"UPDATE Contracts
                                    SET TenantId = @TenantId, RoomId = @RoomId, AccrualTypeId = @AccrualTypeId,
                                    CalcCoefficientId = @CalcCoefficientId, StartDate = @StartDate,
                                    EndDate = @EndDate WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Contract item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync(@"UPDATE Contracts
                                                SET TenantId = @TenantId, RoomId = @RoomId, AccrualTypeId = @AccrualTypeId,
                                                CalcCoefficientId = @CalcCoefficientId, StartDate = @StartDate,
                                                EndDate = @EndDate WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Contracts WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM Contracts WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
