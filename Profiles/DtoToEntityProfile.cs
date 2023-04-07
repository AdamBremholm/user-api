using AutoMapper;
using UserApi.Models;
using UserApi.Models.Dto;

namespace UserApi.Profiles;

public class DtoToEntityProfile : Profile
{
    public DtoToEntityProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}
