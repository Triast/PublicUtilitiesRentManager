using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class PaymentRepository : Repository<Payment>, IRepository<Payment>
    {
        private const string _sqlGetById = "SELECT * FROM Payments WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM Payments;";
        private const string _sqlAdd = @"INSERT INTO Payments VALUES (@Id, @ContractId, @PaymentOrderNumber,
                                        @PaymentDate, @Summ)";
        private const string _sqlUpdate = @"UPDATE Payments SET ContractId = @ContractId,
                                            PaymentOrderNumber = @PaymentOrderNumber, PaymentDate = @PaymentDate,
                                            Summ = @Summ WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM Payments WHERE Id = @Id";

        public PaymentRepository(string connectionString) : base(connectionString) { }

        public Payment GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<Payment> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<Payment> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<Payment>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public void Add(Payment item) => Execute(_sqlAdd, item);
        public Task AddAsync(Payment item) => ExecuteAsync(_sqlAdd, item);
        public void Update(Payment item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(Payment item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
    }
}
