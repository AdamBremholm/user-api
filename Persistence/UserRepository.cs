using System.Data;
using Dapper;
using UserApi.Models;

namespace UserApi.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IDapperContext _context;

    public UserRepository(IDapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var query = "SELECT * FROM Users";

        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<User>(query);
        return users.ToList();
    }

    public async Task<User> Get(int? id)
    {
        var query = "SELECT * FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });
        return user;
    }

    public async Task Create(User user)
    {
        const string query = "INSERT INTO Users (FullName, Cdsid) VALUES (@FullName, @Cdsid)";

        var parameters = new DynamicParameters();
        parameters.Add("FullName", user.FullName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task Update(int id, User user)
    {
        const string query = "INSERT INTO Users (FullName, Cdsid) VALUES (@FullName, @Cdsid WHERE Id = @Id)";
        var parameters = new DynamicParameters();
        parameters.Add("FullName", user.FullName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task Delete(int id)
    {
        var query = "DELETE FROM Users WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { id });
    }
}

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> Get(int? id);
    Task Create(User user);
    Task Update(int id, User user);
    Task Delete(int id);
}
