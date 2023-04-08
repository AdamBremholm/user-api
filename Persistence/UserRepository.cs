using System.Data;
using Dapper;
using UserApi.Models;
using UserApi.Models.Dto;

namespace UserApi.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IDapperContext _context;

    public UserRepository(IDapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        const string query = @"SELECT Id, FirstName, LastName, Cdsid FROM Users";

        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<UserDto>(query);
        return users.ToList();
    }

    public async Task<UserDto> Get(int? id)
    {
        const string query = @"SELECT Id, FirstName, LastName, Cdsid
            FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(query, new { id });
        return user;
    }

    public async Task<UserDto> Create(CreateUserDto user)
    {
        const string query = """
            INSERT INTO Users (FirstName, LastName, Cdsid) 
            VALUES (@FirstName, @LastName, @Cdsid) RETURNING Id, FirstName, LastName, Cdsid
            """;

        var parameters = new DynamicParameters();
        parameters.Add("FirstName", user.FirstName, DbType.String);
        parameters.Add("LastName", user.LastName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleAsync<UserDto>(query, parameters);
    }

    public async Task<UserDto> Update(int id, UpdateUserDto user)
    {
        const string query ="""
            INSERT INTO Users (FirstName, LastName, Cdsid)  VALUES (@FirstName, @LastName, @Cdsid WHERE Id = @Id)
            RETURNING Id, FirstName, LastName, Cdsid
            """;
        var parameters = new DynamicParameters();
        parameters.Add("FirstName", user.FirstName, DbType.String);
        parameters.Add("LastName", user.FirstName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);
        using var connection = _context.CreateConnection();
        return await connection.ExecuteScalarAsync<UserDto>(query, parameters);
    }

    public async Task Delete(int id)
    {
        const string query = "DELETE FROM Users WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { id });
    }
}

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> Get(int? id);
    Task<UserDto> Create(CreateUserDto user);
    Task<UserDto> Update(int id, UpdateUserDto user);
    Task Delete(int id);
}