using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;

namespace Concessionaria.Profiles;

public class MotoProfiles : Profile
{
	public MotoProfiles()
	{
		CreateMap<Moto, MotoDTO>().ForMember(dest => dest.DateUpload, opt => opt.MapFrom(src => src.Auditable.DateUpload))
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

        CreateMap<Moto, MotoCreateDTO>().ReverseMap();

		CreateMap<Moto, MotoAlterationDTO>().ReverseMap();
    }
}
