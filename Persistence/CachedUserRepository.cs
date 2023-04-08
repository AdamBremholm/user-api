using FluentAssertions.Primitives;
using Microsoft.Extensions.Caching.Distributed;
using UserApi.Models.Dto;
using UserApi.Services;

namespace UserApi.Persistence;

public class CachedUserRepository : IUserRepository
{
    private readonly UserRepository _decorated;
    private readonly ICacheService _cacheService;

    public CachedUserRepository(UserRepository decorated, ICacheService cacheService)
    {
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public Task<IEnumerable<UserDto>> GetAll()
    {
        return _decorated.GetAll();
    }

    public Task<UserDto> Get(int? id)
    {
        var key = $"user-{id}";
        _cacheService.GetOrCreateAsync<UserDto>(key,
            async () => await _decorated.Get(id), new DistributedCacheEntryOptions {AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)});
        return _decorated.Get(id);
    }

    public Task<UserDto> Create(CreateUserDto user)
    {
        return _decorated.Create(user);
    }

    public Task<UserDto> Update(int id, UpdateUserDto user)
    {
        return _decorated.Update(id, user);
    }

    public Task Delete(int id)
    {
        return _decorated.Delete(id);
    }
}