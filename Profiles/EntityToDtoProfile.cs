using AutoMapper;
using UserApi.Models;
using UserApi.Models.Dto;

namespace UserApi.Profiles;

public class EntityToDtoProfile : Profile
{
    public EntityToDtoProfile()
    {
        CreateMap<User, UserDto>();
    }
}
