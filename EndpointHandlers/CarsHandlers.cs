using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Concessionaria.EndpointHandlers;

public static class CarsHandlers
{

    public static async Task<Results<NotFound,
                                           Ok<IEnumerable<CarsDTO>>
                                           >> GetCarros
                            (OrganizadorContext DB,
                            IMapper mapper,
                            [FromQuery(Name = "model")]
                            string? Modelo
                             ) 
    {
        var carros = mapper.Map<IEnumerable<CarsDTO>>(await DB.Cars.Where(cars => Modelo == null|| cars.Model.ToUpper()
                                                               .Contains(Modelo.ToUpper()))
                                                               .ToListAsync());
        if (carros == null || carros.Count() == 0)
        {
            return TypedResults.NotFound();
        }
        else
        {
            return TypedResults.Ok(carros);
        }
    
    }

    public static async Task<Results<NotFound, Ok<IEnumerable<CarsDTO>>>>
                                                GetCarsId(OrganizadorContext DB,
                                                           IMapper mapper,
                                                           Guid? Id)
    {
        var carros = mapper.Map<IEnumerable<CarsDTO>>(await DB.Cars.FirstOrDefaultAsync(cars => cars.IdCar == Id));

        if (carros.Count() == 0 || carros == null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            return TypedResults.Ok(carros);
        }
    }

    public static async Task<Results<BadRequest,CreatedAtRoute<CarsDTO>>>
                                               PostCars(OrganizadorContext DB,
                                               IMapper mapper,
                                               [FromBody] 
                                               CarsforCreateDTO CarsforCreate)
    {
        if (CarsforCreate == null)
            return TypedResults.BadRequest();
        

        var carro = mapper.Map<Carros>(CarsforCreate);
        //Pega os dados do Body para a variavel
        DB.Add(carro);
        //Coloca no banco de dados
        await DB.SaveChangesAsync();
        //Salvo no banco de dados

        var ReturnCarro = mapper.Map<CarsDTO>(carro);

        return TypedResults.CreatedAtRoute(ReturnCarro,
                                           "GetCarroId",
                                           new 
                                           {
                                            IdCar = ReturnCarro.IdCar
                                           });
    }

    public static async Task<Results<NotFound,Ok>> DeleteCar(
                                                    OrganizadorContext DB,
                                                    Guid id) 
    {
        var carros = await DB.Cars.FirstOrDefaultAsync(c => c.IdCar == id);
        
        if (carros == null)
            return TypedResults.NotFound();

        
        DB.Remove(carros);
        DB.SaveChanges();
        return TypedResults.Ok();
    }

    public static async Task<Results<NotFound, Ok>>
                                            PutCar(OrganizadorContext DB,
                                                    IMapper mapper,
                                                    [FromBody] CarsForAlterationDTO carAlteration,
                                                    Guid Id)
    {
        var carros = await DB.Cars.FirstOrDefaultAsync(c => c.IdCar == Id);

        if (carros == null)
            return TypedResults.NotFound();

        mapper.Map(carAlteration, carros);

        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }

}
