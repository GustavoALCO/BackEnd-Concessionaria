using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;
using Concessionaria.Models.Moto;
using Concessionaria.Models.Store;
using Concessionaria.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concessionaria.EndpointHandlers;

public static class MotoHandlers
{

    public static async Task<Results<NotFound,
                                           Ok<IEnumerable<MotoWithNumberDTO>>
                                           >> GetMotos
                            (OrganizadorContext DB,
                            IMapper mapper,
                            [FromQuery(Name = "model")]
                            string? Modelo
                             ) 
    {
        var motos = mapper.Map<IEnumerable<MotoWithNumberDTO>>(await DB.Motos.Where(m => m.Model.ToUpper().Contains(Modelo.ToUpper())).ToListAsync());
        if (motos == null || motos.Count() == 0)
        {
            return TypedResults.NotFound();
        }
 
        return TypedResults.Ok(motos);
    
    }

    public static async Task<Results<NotFound, Ok<MotoDTO>>>
                                                GetmotoId(OrganizadorContext DB,
                                                           IMapper mapper,
                                                           Guid? Id)
    {
        var motos = mapper.Map<MotoDTO>(await DB.Motos.Include(s => s.IdStore).Where(m => m.IdMoto == Id).ToListAsync());

        if (motos == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(motos);
        
    }

    public static async Task<Results<Ok<List<MotoDTO>>, NotFound>>
    GetmotoFilter(OrganizadorContext DB,
                  IMapper mapper,
                  [FromQuery] string ?motoBrand,
                  [FromQuery] string ?model,
                  [FromQuery] int[] ?age,
                  [FromQuery] string ?color)
    {

        // Cria uma consulta IQueryable que pode ser modificada dinamicamente
        var query = DB.Motos.AsQueryable();

        // Filtra por marca do carro, ignorando maiúsculas e minúsculas
        if (!string.IsNullOrEmpty(motoBrand))
        {
            query = query.Where(c => c.MotoBrand.ToLower()
                                                .Contains(motoBrand.ToLower()));
        }

        // Filtra por modelo da moto, ignorando maiúsculas e minúsculas
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

        // Executa a consulta e mapeia o resultado para o tipo motoDTO
        var moto = mapper.Map<List<MotoDTO>>(await query.ToListAsync());

        // Retorna NotFound se a lista estiver vazia, caso contrário retorna Ok com a lista de motos
        if (moto.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(moto);
    }


    [Authorize]
    public static async Task<Results<BadRequest<string>, CreatedAtRoute<MotoDTO>>>
     Postmoto(OrganizadorContext DB,
              IMapper mapper,
              IValidator<MotoCreateDTO> validator,
              [FromBody] MotoCreateDTO motoCreate,
              ImageUpload imageUpload)
    {
        var validate = validator.Validate(motoCreate);
        if(!validate.IsValid)
            return TypedResults.BadRequest(validate.Errors.ToString());

        var imageUrls = await imageUpload.UploadBase64ImagesAsync(motoCreate.IdMoto, motoCreate.Url, "motos");

        var moto = mapper.Map<Moto>(motoCreate);
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
    public static async Task<Results<NotFound,Ok>> DeleteMoto(
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
    public static async Task<Results<BadRequest<string>, Ok>>
                                            PutMoto(OrganizadorContext DB,
                                                    IValidator<MotoAlterationDTO> validator,
                                                    IMapper mapper,
                                                    [FromBody] MotoAlterationDTO carAlteration,
                                                    Guid Id)
    {
        var validate = validator.Validate(carAlteration);
        if(!validate.IsValid)
            return TypedResults.BadRequest(validate.IsValid.ToString());

        var motos = await DB.Motos.FirstOrDefaultAsync(c => c.IdMoto == Id);

        if (motos == null)
            return TypedResults.BadRequest("Não existe Motos com esse Id");

        mapper.Map(carAlteration, motos);

        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }

}
