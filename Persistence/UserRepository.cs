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
        const string query = @"SELECT Id, FullName, Cdsid FROM Users";

        using var connection = _context.CreateConnection();
        var users = await connection.QueryAsync<UserDto>(query);
        return users.ToList();
    }

    public async Task<UserDto> Get(int? id)
    {
        const string query = @"SELECT Id, FullName, Cdsid
            FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<UserDto>(query, new { id });
        return user;
    }

    public async Task<UserDto> Create(CreateUserDto user)
    {
        const string query = "INSERT INTO Users (FullName, Cdsid) output inserted.Id VALUES (@FullName, @Cdsid) ";

        var parameters = new DynamicParameters();
        parameters.Add("FullName", user.FullName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);
        using var connection = _context.CreateConnection();
        var insertedId =  connection.QuerySingle<int>(query, parameters);
        
        return await Get(insertedId);
    }

    public async Task<UserDto> Update(int id, UpdateUserDto user)
    {
        const string query = "INSERT INTO Users (FullName, Cdsid)  VALUES (@FullName, @Cdsid WHERE Id = @Id)";
        var parameters = new DynamicParameters();
        parameters.Add("FullName", user.FullName, DbType.String);
        parameters.Add("Cdsid", user.Cdsid, DbType.String);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
        return await Get(id);
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
