using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class AccrualRepository : Repository<Accrual>, IRepository<Accrual>
    {
        private const string _sqlGetById = "SELECT * FROM Accruals WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM Accruals;";
        private const string _sqlAdd = "INSERT INTO Accruals VALUES (@Id, @ContractId, @InvoiceNumber, @AccrualDate, @Summ)";
        private const string _sqlUpdate = @"UPDATE Accruals SET ContractId = @ContractId, InvoiceNumber = @InvoiceNumber,
                                            AccrualDate = @AccrualDate, Summ = @Summ WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM Accruals WHERE Id = @Id";

        public AccrualRepository(string connectionString) : base(connectionString) { }

        public Accrual GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<Accrual> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<Accrual> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<Accrual>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public void Add(Accrual item) => Execute(_sqlAdd, item);
        public Task AddAsync(Accrual item) => ExecuteAsync(_sqlAdd, item);
        public void Update(Accrual item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(Accrual item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
    }
}
