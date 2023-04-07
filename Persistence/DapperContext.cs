using System.Data;
using Dapper;
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
        public async Task Init()
            {
                // create database tables if they don't exist
                using var connection = CreateConnection();
                await _initUsers();
        
                async Task _initUsers()
                {
                    const string sql = """
                        CREATE TABLE IF NOT EXISTS 
                        Users (
                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            FullName TEXT,
                            Cdsid TEXT,
                            Role INTEGER
                        ); 
                        """;
                    await connection.ExecuteAsync(sql);
                }
            }
    }

public interface IDapperContext {
    IDbConnection CreateConnection();
}
