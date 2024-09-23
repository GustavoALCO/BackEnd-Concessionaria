using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;
using Concessionaria.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concessionaria.EndpointHandlers;

public static class CarsHandlers
{

    public static async Task<Results<NotFound,
                                           Ok<IEnumerable<MotoDTO>>
                                           >> GetCarros
                            (OrganizadorContext DB,
                            IMapper mapper,
                            [FromQuery(Name = "model")]
                            string? Modelo
                             ) 
    {
        var motos = mapper.Map<IEnumerable<MotoDTO>>(await DB.Motos.Where(cars => Modelo == null|| cars.Model.ToUpper()
                                                               .Contains(Modelo.ToUpper()))
                                                               .ToListAsync());
        if (motos == null || motos.Count() == 0)
        {
            return TypedResults.NotFound();
        }
 
        return TypedResults.Ok(motos);
    
    }

    public static async Task<Results<NotFound, Ok<MotoDTO>>>
                                                GetCarsId(OrganizadorContext DB,
                                                           IMapper mapper,
                                                           Guid? Id)
    {
        var motos = mapper.Map<MotoDTO>(await DB.Motos.FirstOrDefaultAsync(cars => cars.IdMoto == Id));

        if (motos == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(motos);
        
    }

    public static async Task<Results<Ok<List<MotoDTO>>, NotFound>>
    GetCarsFilter(OrganizadorContext DB,
                  IMapper mapper,
                  [FromQuery] string ?carBrand,
                  [FromQuery] string ?model,
                  [FromQuery] int[] ?age,
                  [FromQuery] string ?color)
    {

        // Cria uma consulta IQueryable que pode ser modificada dinamicamente
        var query = DB.Motos.AsQueryable();

        // Filtra por marca do carro, ignorando maiúsculas e minúsculas
        if (!string.IsNullOrEmpty(carBrand))
        {
            query = query.Where(c => c.MotoBrand.ToLower()
                                                .Contains(carBrand.ToLower()));
        }

        // Filtra por modelo do carro, ignorando maiúsculas e minúsculas
        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(c => c.Model.ToLower()
                                            .Contains(model.ToLower()));
        }

        // Filtra por intervalo de anos se a lista tiver exatamente dois valores
        if (age != null && age.Length == 2)
        {
            int startYear = age[0]; // Ano inicial do intervalo
            int endYear = age[1];   // Ano final do intervalo

            if (startYear > endYear)
            {
                (startYear, endYear) = (endYear, startYear);
            }//garante que o valor do start será menor 

            query = query.Where(c => c.Age >= startYear && c.Age <= endYear);
        }
        // Filtra por anos específicos se a lista tiver um ou mais valores
        else if (age != null && age.Any())
        {
            query = query.Where(c => age.Contains(c.Age));
        }

        // Filtra por cor do carro, ignorando maiúsculas e minúsculas
        if (!string.IsNullOrEmpty(color))
        {
            query = query.Where(c => c.Color.ToLower()
                                            .Contains(color.ToLower()));
        }

        // Executa a consulta e mapeia o resultado para o tipo CarsDTO
        var cars = mapper.Map<List<MotoDTO>>(await query.ToListAsync());

        // Retorna NotFound se a lista estiver vazia, caso contrário retorna Ok com a lista de motos
        if (cars.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(cars);
    }


    [Authorize]
    public static async Task<Results<BadRequest, CreatedAtRoute<MotoDTO>>>
     PostCars(OrganizadorContext DB,
              IMapper mapper,
              [FromBody] MotoCreateDTO CarsforCreate,
              ImageUpload imageUpload)
    {
        if (CarsforCreate == null)
            return TypedResults.BadRequest();

        var imageUrls = await imageUpload.UploadBase64ImagesAsync(CarsforCreate.IdMoto, CarsforCreate.Url, "motos");

        var moto = mapper.Map<Moto>(CarsforCreate);
        moto.Url = imageUrls;

        DB.Add(moto);
        await DB.SaveChangesAsync();

        var ReturnCarro = mapper.Map<MotoDTO>(moto);

        return TypedResults.CreatedAtRoute(ReturnCarro,
                                           "GetCarroId",
                                           new
                                           {
                                               IdCar = ReturnCarro.IdMoto
                                           });
    }


    [Authorize]
    public static async Task<Results<NotFound,Ok>> DeleteCar(
                                                    OrganizadorContext DB,
                                                    Guid Id) 
    {
        var motos = await DB.Motos.FirstOrDefaultAsync(c => c.IdMoto == Id);
        
        if (motos == null)
            return TypedResults.NotFound();

        
        DB.Remove(motos);
        DB.SaveChanges();
        return TypedResults.Ok();
    }
    [Authorize]
    public static async Task<Results<NotFound, Ok>>
                                            PutCar(OrganizadorContext DB,
                                                    IMapper mapper,
                                                    [FromBody] MotoAlterationDTO carAlteration,
                                                    Guid Id)
    {
        var motos = await DB.Motos.FirstOrDefaultAsync(c => c.IdMoto == Id);

        if (motos == null)
            return TypedResults.NotFound();

        mapper.Map(carAlteration, motos);

        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }

}
