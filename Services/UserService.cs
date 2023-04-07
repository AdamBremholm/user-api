using AutoMapper;
using UserApi.Models.Dto;
using UserApi.Persistence;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Create(CreateUserDto user)
    {
        var res = await _userRepository.Create(user);
        return res;
    }

    public async Task Delete(int id)
    {
        await _userRepository.Delete(id);
    }

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<UserDto> Get(int id)
    {
        var res = await _userRepository.Get(id);
        return res;
    }

    public async Task<UserDto> Update(UpdateUserDto user)
    {
        var res = await _userRepository.Update(user.Id, user);
        return res;
    }
}

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto> Get(int id);
    Task<UserDto> Create(CreateUserDto user);
    Task<UserDto> Update(UpdateUserDto user);
    Task Delete(int id);
}
