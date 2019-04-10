using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class RoomRepository
    {
        private readonly string _connectionString;

        public RoomRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Room GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<Room>("SELECT * FROM Rooms WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Room> GetByIdAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingleAsync<Room>("SELECT * FROM Rooms WHERE Id = @Id;", new { Id = id });
            }
        }

        public IEnumerable<Room> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Room>("SELECT * FROM Rooms;");
            }
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryAsync<Room>("SELECT * FROM Rooms;");
            }
        }

        public void Add(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Rooms VALUES (@Id, @Address, @RoomType, @Square, @Price, @IsOccupied)", item);
            }
        }
        public Task AddAsync(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("INSERT INTO Rooms VALUES (@Id, @Address, @RoomType, @Square, @Price, @IsOccupied)", item);
            }
        }

        public void Update(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE Rooms SET Address = @Address, RoomType = @RoomType, Square = @Square, Price = @Price, IsOccupied = @IsOccupied WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("UPDATE Rooms SET Address = @Address, RoomType = @RoomType, Square = @Square, Price = @Price, IsOccupied = @IsOccupied WHERE Id = @Id", item);
            }
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Rooms WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync("DELETE FROM Rooms WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
