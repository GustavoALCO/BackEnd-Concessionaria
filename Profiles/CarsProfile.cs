using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;

namespace Concessionaria.Profiles;

public class CarsProfile : Profile
{
	public CarsProfile()
	{
		CreateMap<Cars, CarsDTO>().ReverseMap();

		CreateMap<Cars, CarsforCreateDTO>().ReverseMap();

		CreateMap<Cars, CarsForAlterationDTO>().ReverseMap();
    }
}
