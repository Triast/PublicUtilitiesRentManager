using Dapper;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly string _connectionString;

        public RoomTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RoomType GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<RoomType>("SELECT * FROM RoomTypes WHERE Id = @Id;", new { Id = id });
            }
        }
        public Task<RoomType> GetByIdAsync(string id)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<RoomType>("SELECT * FROM RoomTypes WHERE Id = @Id;", new { Id = id })
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }

        public IEnumerable<RoomType> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<RoomType>("SELECT * FROM RoomTypes;");
            }
        }

        public Task<IEnumerable<RoomType>> GetAllAsync()
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<RoomType>("SELECT * FROM RoomTypes;")
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }

        public RoomType GetByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .QuerySingle<RoomType>("SELECT * FROM RoomTypes WHERE Name = @Name;", new { Name = name });
            }
        }

        public Task<RoomType> GetByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection
                .QuerySingleAsync<RoomType>("SELECT * FROM RoomTypes WHERE Name = @Name;", new { Name = name })
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }

        public void Add(RoomType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO RoomTypes VALUES (@Id, @Name)", item);
            }
        }
        public Task AddAsync(RoomType item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("INSERT INTO RoomTypes VALUES (@Id, @Name)", item)
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }

        public void Update(RoomType item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("UPDATE RoomTypes SET Name = @Name WHERE Id = @Id", item);
            }
        }

        public Task UpdateAsync(RoomType item)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("UPDATE RoomTypes SET Name = @Name WHERE Id = @Id", item)
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }

        public void Remove(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM RoomTypes WHERE Id = @Id", new { Id = id });
            }
        }
        public Task RemoveAsync(string id)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM RoomTypes WHERE Id = @Id", new { Id = id }).ContinueWith(RoomTypes =>
            {
                connection.Dispose();

                return RoomTypes.Result;
            });
        }

        public void RemoveByName(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM RoomTypes WHERE Name = @Name", new { Name = name });
            }
        }
        public Task RemoveByNameAsync(string name)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync("DELETE FROM RoomTypes WHERE Name = @Name", new { Name = name })
                .ContinueWith(RoomTypes =>
                {
                    connection.Dispose();

                    return RoomTypes.Result;
                });
        }
    }
}
