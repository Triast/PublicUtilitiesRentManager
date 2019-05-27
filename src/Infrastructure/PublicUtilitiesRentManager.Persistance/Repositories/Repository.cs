using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PublicUtilitiesRentManager.Persistance.Repositories
{
    public class Repository<T>
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IEnumerable<T> Query(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(sql, param);
            }
        }

        protected Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QueryAsync<T>(sql, param)
                .ContinueWith(entities =>
                {
                    connection.Dispose();

                    return entities.Result;
                });
        }
        protected T QuerySingle(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<T>(sql, param);
            }
        }
        protected Task<T> QuerySingleAsync(string sql, object param = null)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.QuerySingleAsync<T>(sql, param)
                .ContinueWith(entity =>
                {
                    connection.Dispose();

                    return entity.Result;
                });
        }

        protected void Execute(string sql, object param = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, param);
            }
        }
        protected Task ExecuteAsync(string sql, object param = null)
        {
            var connection = new SqlConnection(_connectionString);

            return connection.ExecuteAsync(sql, param)
                .ContinueWith(entities =>
                {
                    connection.Dispose();

                    return entities.Result;
                });
        }
    }
}
