using System.Data;
using Npgsql;

namespace UserApi.Persistence;

public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }

public interface IDapperContext {
    IDbConnection CreateConnection();
}
