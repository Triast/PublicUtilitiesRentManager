using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        private const string _sqlGetById = "SELECT * FROM Contracts WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM Contracts;";
        private const string _sqlGetAllByTenantId = "SELECT * FROM Contracts WHERE TenantId = @TenantId;";
        private const string _sqlGetAllByTenantIdAndAccrualTypeId = @"SELECT * FROM Contracts
                                WHERE TenantId = @TenantId AND AccrualTypeId = @AccrualTypeId;";
        private const string _sqlAdd = @"INSERT INTO Contracts VALUES (@Id, @TenantId,
                                        @RoomId, @AccrualTypeId, @StartDate, @EndDate)";
        private const string _sqlUpdate = @"UPDATE Contracts
                                            SET TenantId = @TenantId, RoomId = @RoomId, AccrualTypeId = @AccrualTypeId,
                                            StartDate = @StartDate, EndDate = @EndDate WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM Contracts WHERE Id = @Id";

        public ContractRepository(string connectionString) : base(connectionString) { }

        public Contract GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<Contract> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<Contract> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<Contract>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public Task<IEnumerable<Contract>> GetAllByTenantIdAsync(string tenantId) =>
            QueryAsync(_sqlGetAllByTenantId, new { TenantId = tenantId });
        public Task<IEnumerable<Contract>> GetAllByTenantIdAndAccrualTypeIdAsync(string tenantId, string accrualTypeId) =>
            QueryAsync(_sqlGetAllByTenantIdAndAccrualTypeId, new { TenantId = tenantId, AccrualTypeId = accrualTypeId });
        public void Add(Contract item) => Execute(_sqlAdd, item);
        public Task AddAsync(Contract item) => ExecuteAsync(_sqlAdd, item);
        public void Update(Contract item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(Contract item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
    }
}
