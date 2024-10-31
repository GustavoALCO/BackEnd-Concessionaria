using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models.Users;

namespace Concessionaria.Profiles;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<User, UserDTO>()
           .ForMember(dest => dest.DateUpload, opt => opt.MapFrom(src => src.Auditable.DateUpload))
           .ForMember(dest => dest.CreateUserId, opt => opt.MapFrom(src => src.Auditable.CreateUserId))
           .ForMember(dest => dest.DateUpdate, opt => opt.MapFrom(src => src.Auditable.DateUpdate))
           .ForMember(dest => dest.AlterationUserId, opt => opt.MapFrom(src => src.Auditable.AlterationUserId))
           .ReverseMap()
           .ForMember(dest => dest.Auditable, opt => opt.MapFrom(src => new Auditable
           {
               DateUpload = src.DateUpload,
               CreateUserId = src.CreateUserId,
               DateUpdate = src.DateUpdate,
               AlterationUserId = (Guid)src.AlterationUserId
           }));

        CreateMap<User, UserCreateDTO>().ReverseMap();

        CreateMap<UserAlterationDTO, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}
