using AutoMapper;
using Concessionaria.Entities;
using Concessionaria.Models.Store;

namespace Concessionaria.Profiles;

public class StoreProfile :Profile
{
    public StoreProfile() 
    {
        CreateMap<Store, StoreDTO>().ReverseMap();

        CreateMap<Store, StoreCreateDTO>().ReverseMap();

        CreateMap<Store, StoreAlterationDTO>().ReverseMap();
    }
}
