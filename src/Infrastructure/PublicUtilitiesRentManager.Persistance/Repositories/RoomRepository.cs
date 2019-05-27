using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private const string _sqlGetById = "SELECT * FROM Rooms WHERE Id = @Id;";
        private const string _sqlGetAll = "SELECT * FROM Rooms;";
        private const string _sqlGetByAddress = "SELECT * FROM Rooms WHERE Address = @Address;";
        private const string _sqlAdd = @"INSERT INTO Rooms VALUES (@Id, @RoomTypeId, @Address,
                                        @Square, @IsOccupied, @Price, @IncreasingCoefToBaseRate,
                                        @PlacementCoef, @ComfortCoef, @SocialOrientationCoef)";
        private const string _sqlUpdate = @"UPDATE Rooms SET RoomTypeId = @RoomTypeId,
                                            Address = @Address, Square = @Square,
                                            IsOccupied = @IsOccupied, Price = @Price,
                                            IncreasingCoefToBaseRate = @IncreasingCoefToBaseRate,
                                            PlacementCoef = @PlacementCoef, ComfortCoef = @ComfortCoef,
                                            SocialOrientationCoef = @SocialOrientationCoef WHERE Id = @Id";
        private const string _sqlRemove = "DELETE FROM Rooms WHERE Id = @Id";
        private const string _sqlRemoveByAddress = "DELETE FROM Rooms WHERE Address = @Address";

        public RoomRepository(string connectionString) : base(connectionString) { }

        public Room GetById(string id) => QuerySingle(_sqlGetById, new { Id = id });
        public Task<Room> GetByIdAsync(string id) => QuerySingleAsync(_sqlGetById, new { Id = id });
        public IEnumerable<Room> GetAll() => Query(_sqlGetAll);
        public Task<IEnumerable<Room>> GetAllAsync() => QueryAsync(_sqlGetAll);
        public Room GetByAddress(string address) => QuerySingle(_sqlGetByAddress, new { Address = address });
        public Task<Room> GetByAddressAsync(string address) =>
            QuerySingleAsync(_sqlGetByAddress, new { Address = address });
        public void Add(Room item) => Execute(_sqlAdd, item);
        public Task AddAsync(Room item) => ExecuteAsync(_sqlAdd, item);
        public void Update(Room item) => Execute(_sqlUpdate, item);
        public Task UpdateAsync(Room item) => ExecuteAsync(_sqlUpdate, item);
        public void Remove(string id) => Execute(_sqlRemove, new { Id = id });
        public Task RemoveAsync(string id) => ExecuteAsync(_sqlRemove, new { Id = id });
        public void RemoveByAddress(string address) => Execute(_sqlRemoveByAddress, new { Address = address });
        public Task RemoveByAddressAsync(string address) =>
            ExecuteAsync(_sqlRemoveByAddress, new { Address = address });
    }
}
