using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models;

namespace Concessionaria.Profiles;

public class CarsProfile : Profile
{
	public CarsProfile()
	{
		CreateMap<Carros, CarsDTO>().ReverseMap();

        CreateMap<Carros, CarsforCreateDTO>().ReverseMap();

		CreateMap<ImageCar, ImageDTO>().ReverseMap();
    }
}
