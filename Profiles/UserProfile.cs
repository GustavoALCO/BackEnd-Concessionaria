using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models.Users;

namespace Concessionaria.Profiles;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<User, UserDTO>().ReverseMap();

        CreateMap<User, UserForCreateDTO>().ReverseMap();

        CreateMap<User, UserForAlterationDTO>().ReverseMap();
    }
}
