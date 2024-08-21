using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;
using Concessionaria.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    public static async Task<Results<NotFound, Ok<CarsDTO>>>
                                                GetCarsId(OrganizadorContext DB,
                                                           IMapper mapper,
                                                           Guid? Id)
    {
        var carros = mapper.Map<CarsDTO>(await DB.Cars.FirstOrDefaultAsync(cars => cars.IdCar == Id));

        if (carros == null)
        {
            return TypedResults.NotFound();
        }
        else
        {
            return TypedResults.Ok(carros);
        }
    }

    public static async Task<Results<Ok<List<CarsDTO>>, NotFound>>
    GetCarsFilter(OrganizadorContext DB,
                  IMapper mapper,
                  [FromQuery] string ?carBrand,
                  [FromQuery] string ?model,
                  [FromQuery] int[] ?age,
                  [FromQuery] string ?color)
    {

        // Cria uma consulta IQueryable que pode ser modificada dinamicamente
        var query = DB.Cars.AsQueryable();

        // Filtra por marca do carro, ignorando maiúsculas e minúsculas
        if (!string.IsNullOrEmpty(carBrand))
        {
            query = query.Where(c => c.CarBrand.ToLower()
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
        var cars = mapper.Map<List<CarsDTO>>(await query.ToListAsync());

        // Retorna NotFound se a lista estiver vazia, caso contrário retorna Ok com a lista de carros
        if (cars.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(cars);
    }


    [Authorize]
    public static async Task<Results<BadRequest, CreatedAtRoute<CarsDTO>>>
     PostCars(OrganizadorContext DB,
              IMapper mapper,
              [FromBody] CarsforCreateDTO CarsforCreate,
              ImageUpload imageUpload)
    {
        if (CarsforCreate == null)
            return TypedResults.BadRequest();

        var imageUrls = await imageUpload.UploadBase64ImagesAsync(CarsforCreate.IdCar, CarsforCreate.Url, "carros");

        var carro = mapper.Map<Cars>(CarsforCreate);
        carro.Url = imageUrls;

        DB.Add(carro);
        await DB.SaveChangesAsync();

        var ReturnCarro = mapper.Map<CarsDTO>(carro);

        return TypedResults.CreatedAtRoute(ReturnCarro,
                                           "GetCarroId",
                                           new
                                           {
                                               IdCar = ReturnCarro.IdCar
                                           });
    }


    [Authorize]
    public static async Task<Results<NotFound,Ok>> DeleteCar(
                                                    OrganizadorContext DB,
                                                    Guid Id) 
    {
        var carros = await DB.Cars.FirstOrDefaultAsync(c => c.IdCar == Id);
        
        if (carros == null)
            return TypedResults.NotFound();

        
        DB.Remove(carros);
        DB.SaveChanges();
        return TypedResults.Ok();
    }
    [Authorize]
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
