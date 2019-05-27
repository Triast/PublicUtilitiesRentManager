using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class CalcCoefficientRepository : Repository<CalcCoefficient>, ICalcCoefficientRepository
    {
        private const string _sqlGetById = "SELECT * FROM CalcCoefficients WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM CalcCoefficients;";
        private const string _sqlGetByName = "SELECT * FROM CalcCoefficients WHERE Name = @Name;";
        private const string _sqlAdd = "INSERT INTO CalcCoefficients VALUES (@Id, @Name, @Coefficient)";
        private const string _sqlUpdate = @"UPDATE CalcCoefficients SET Name = @Name,
                                            Coefficient = @Coefficient WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM CalcCoefficients WHERE Id = @Id";
        private const string _sqlRemoveByName = "DELETE FROM CalcCoefficients WHERE Name = @Name";

        public CalcCoefficientRepository(string connectionString) : base(connectionString) { }

        public CalcCoefficient GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<CalcCoefficient> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<CalcCoefficient> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<CalcCoefficient>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public CalcCoefficient GetByName(string name) => QuerySingle(_sqlGetByName, new { Name = name });
        public Task<CalcCoefficient> GetByNameAsync(string name) =>
            QuerySingleAsync(_sqlGetByName, new { Name = name });
        public void Add(CalcCoefficient item) => Execute(_sqlAdd, item);
        public Task AddAsync(CalcCoefficient item) => ExecuteAsync(_sqlAdd, item);
        public void Update(CalcCoefficient item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(CalcCoefficient item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
        public void RemoveByName(string name) => Execute(_sqlRemoveByName, new { Name = name });
        public Task RemoveByNameAsync(string name) => ExecuteAsync(_sqlRemoveByName, new { Name = name });
    }
}
