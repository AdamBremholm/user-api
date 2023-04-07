using UserApi.Models;
using UserApi.Persistence;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Create(User user)
    {
        await _userRepository.Create(user);
        return user;
    }

    public async Task Delete(int id)
    {
        await _userRepository.Delete(id);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> Get(int id)
    {
        return await _userRepository.Get(id);
    }

    public async Task<User> Update(User user)
    {
        await _userRepository.Update(user.Id, user);
        return user;
    }
}

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User> Get(int id);
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task Delete(int id);
}
