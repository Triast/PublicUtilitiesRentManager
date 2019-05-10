using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
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
                return connection
                    .QuerySingle<Room>("SELECT * FROM Rooms WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<Room> GetByIdAsync(string id)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<Room>("SELECT * FROM Rooms WHERE Id = @Id;", new { Id = id })
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<Room>("SELECT * FROM Rooms;")
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
        }

        public Room GetByAddress(string address)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .QuerySingle<Room>("SELECT * FROM Rooms WHERE Address = @Address;", new { Address = address });
            }
        }

        public Task<Room> GetByAddressAsync(string address)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<Room>("SELECT * FROM Rooms WHERE Address = @Address;", new { Address = address })
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
        }

        public void Add(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection
                    .Execute("INSERT INTO Rooms VALUES (@Id, @RoomTypeId, @Address, @Square, @IsOccupied, @Price, @IncreasingCoefToBaseRate, @PlacementCoef, @ComfortCoef, @SocialOrientationCoef)", item);
            }
        }
        public Task AddAsync(Room item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .ExecuteAsync("INSERT INTO Rooms VALUES (@Id, @RoomTypeId, @Address, @Square, @IsOccupied, @Price, @IncreasingCoefToBaseRate, @PlacementCoef, @ComfortCoef, @SocialOrientationCoef)", item)
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
        }

        public void Update(Room item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection
                    .Execute("UPDATE Rooms SET RoomTypeId = @RoomTypeId, Address = @Address, Square = @Square, IsOccupied = @IsOccupied, Price = @Price, IncreasingCoefToBaseRate = @IncreasingCoefToBaseRate, PlacementCoef = @PlacementCoef, ComfortCoef = @ComfortCoef, SocialOrientationCoef = @SocialOrientationCoef WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(Room item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .ExecuteAsync("UPDATE Rooms SET RoomTypeId = @RoomTypeId, Address = @Address, Square = @Square, IsOccupied = @IsOccupied, Price = @Price, IncreasingCoefToBaseRate = @IncreasingCoefToBaseRate, PlacementCoef = @PlacementCoef, ComfortCoef = @ComfortCoef, SocialOrientationCoef = @SocialOrientationCoef WHERE Id = @Id", item)
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
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
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM Rooms WHERE Id = @Id", new { Id = id })
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
        }

        public void RemoveByAddress(string address)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Rooms WHERE Address = @Address", new { Address = address });
            }
        }
        public Task RemoveByAddressAsync(string address)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM Rooms WHERE Address = @Address", new { Address = address })
                .ContinueWith(rooms =>
            {
                connection.Dispose();

                return rooms.Result;
            });
        }
    }
}
