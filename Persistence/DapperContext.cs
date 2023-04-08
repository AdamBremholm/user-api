using System.Data;
using Dapper;
using Npgsql;

namespace UserApi.Persistence;

public class DapperContext : IDapperContext
{
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new ArgumentNullException(nameof(configuration));
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task Init()
    {
        // create database tables if they don't exist
        using var connection = CreateConnection();
        const string sql = """
                    CREATE TABLE IF NOT EXISTS Users (
                      Id SERIAL PRIMARY KEY,
                      FirstName VARCHAR(255),
                      LastName VARCHAR(255),
                      Cdsid VARCHAR(255) NOT NULL
                    );
                    """;

        await connection.ExecuteAsync(sql);
    }
}

public interface IDapperContext
{
    IDbConnection CreateConnection();
    Task Init();
}