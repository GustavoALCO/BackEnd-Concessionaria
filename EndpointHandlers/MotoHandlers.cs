using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Cars;
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
                                           Ok<IEnumerable<MotoDTO>>
                                           >> GetMotos
                            (OrganizadorContext DB,
                            IMapper mapper,
                            [FromQuery(Name = "model")]
                            string? Modelo,
                            [FromQuery(Name = "page")]
                            int? page
                            
) 
    {
        if (page <= 0 || page == null)
        {
            page = 1;
        }

        var motos = mapper.Map<IEnumerable<MotoDTO>>(await DB.Motos.Where(m => Modelo == null ||m.Model.ToUpper().Contains(Modelo.ToUpper()))
                                                                                                                                       .Skip((page.Value - 1) * 10)
                                                                                                                                       .Take(10)
                                                                                                                                       //Pega os quais apareceram
                                                                                                                                       .ToListAsync());
                                                                                                                                       //Lista todos os selecionados
        if (motos == null || motos.Count() == 0)
        {
            return TypedResults.NotFound();
        }
 
        return TypedResults.Ok(motos);
    
    }

    public static async Task<Results<NotFound,
                                           Ok<IEnumerable<MotoDTO>>
                                           >> GetNovasMotos
                            (OrganizadorContext DB,
                            IMapper mapper
)
    {
        var motos = mapper.Map<IEnumerable<MotoDTO>>(await DB.Motos.Where(m => m.Model == null).Take(10).ToListAsync());

        //var motos = mapper.Map<IEnumerable<MotoDTO>>(await DB.Motos.OrderByDescending(m =>  m.Auditable.DateUpload).Take(10).ToListAsync());

        //Adicionar depois busca por mais nova quando alterar para o postgre
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
        var motos = mapper.Map<MotoDTO>(await DB.Motos.FirstOrDefaultAsync(m => m.IdMoto == Id));

        var numero = await DB.Store.FindAsync(motos.IdStore);

        if (numero == null || motos == null)
            return TypedResults.NotFound();

        motos.phoneNumber = numero.PhoneNumbers;

        if (motos == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(motos);
        
    }

    public static async Task<Results<NotFound,
                                           Ok<IEnumerable<MotoDTO>>
                                           >> GetmotoFilter(OrganizadorContext DB,
                                                       IMapper mapper,
                                                       [FromQuery]
                                                      int page,
                                                       [FromQuery]
                                                      string[]? MotoBrand,
                                                       [FromQuery]
                                                      string? Model,
                                                       [FromQuery]
                                                      string? Color,
                                                       [FromQuery]
                                                      int[]? Age,
                                                       [FromQuery]
                                                      int[]? Km,
                                                       [FromQuery]
                                                      int[]? Price)
    {
        if (page <= 0)
            page = 1;

        // Cria uma consulta IQueryable que pode ser modificada dinamicamente
        var query = DB.Motos.AsQueryable();

        // Filtra por marca da moto
        if (MotoBrand != null && MotoBrand.Any())
        {
            query = query.Where(c => MotoBrand
                                    .Any(brand => c.MotoBrand.ToUpper()
                                    .Contains(brand.ToUpper())));
        }

        // Filtra por modelo da moto
        if (!string.IsNullOrEmpty(Model))
        {
            query = query.Where(c => c.Model.ToUpper()
                                            .Contains(Model.ToUpper()));
        }

        // Filtra por cor da moto
        if (!string.IsNullOrEmpty(Color))
        {
            query = query.Where(c => c.Color.ToUpper()
                                            .Contains(Color.ToUpper()));
        }

        // Filtra por intervalo de anos
        if (Age != null && Age.Length == 2)
        {
            int startYear = Age[0];
            int endYear = Age[1];

            if (startYear > endYear)
            {
                (startYear, endYear) = (endYear, startYear);
            }

            query = query.Where(c => c.Age >= startYear && c.Age <= endYear);
        }
        else if (Age != null && Age.Any())
        {
            query = query.Where(c => c.Age >= Age[0]);
        }

        // Filtra por intervalo de quilometragem
        if (Km != null && Km.Length == 2)
        {
            int startKm = Km[0];
            int endKm = Km[1];

            if (startKm > endKm)
            {
                (startKm, endKm) = (endKm, startKm);
            }

            query = query.Where(c => c.Km >= startKm && c.Km <= endKm);
        }
        else if (Km != null && Km.Any())
        {
            query = query.Where(c => c.Km >= Km[0]);
        }

        // Filtra por intervalo de preço
        if (Price != null && Price.Length == 2)
        {
            int startPrice = Price[0];
            int endPrice = Price[1];

            if (startPrice > endPrice)
            {
                (startPrice, endPrice) = (endPrice, startPrice);
            }

            query = query.Where(c => c.Price >= startPrice && c.Price <= endPrice);
        }
        else if (Price != null && Price.Length > 0)
        {
            query = query.Where(c => c.Price >= Price[0]);
        }
        // Executa a consulta com paginação
        var motos = await query.Skip((page - 1) * 10).Take(10).ToListAsync();

        // Mapeia os resultados para MotoDTO
        var motoDTOs = mapper.Map<IEnumerable<MotoDTO>>(motos);

        // Verifica se há resultados
        if (!motoDTOs.Any())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(motoDTOs);
    }


    [Authorize]
    public static async Task<Results<BadRequest<List<string>>, CreatedAtRoute<MotoDTO>>>
     Postmoto(OrganizadorContext DB,
              IMapper mapper,
              IValidator<MotoCreateDTO> validator,
              [FromBody] MotoCreateDTO motoCreate,
              ImageUpload imageUpload)
    {
        var validate = validator.Validate(motoCreate);
        if (!validate.IsValid)
        {
            var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
            return TypedResults.BadRequest(errors);
        }

        var imageUrls = await imageUpload.UploadBase64ImagesAsync(motoCreate.Url);

        var moto = mapper.Map<Moto>(motoCreate);
        moto.Url = imageUrls;

        moto.Auditable.DateUpload = DateTime.UtcNow;

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
                                                    ImageUpload MotoDelete,
                                                    Guid Id) 
    {
        var motos = await DB.Motos.FirstOrDefaultAsync(c => c.IdMoto == Id);
        
        if (motos == null)
            return TypedResults.NotFound();

        await MotoDelete.DeleteUrlImage(motos.Url, "carros");
        DB.Remove(motos);
        DB.SaveChanges();
        return TypedResults.Ok();
    }

    [Authorize]
    public static async Task<Results<BadRequest<List<string>>, Ok>>
                                            PutMoto(OrganizadorContext DB,
                                                    IValidator<MotoAlterationDTO> validator,
                                                    IMapper mapper,
                                                    ImageUpload image,
                                                    [FromBody] MotoAlterationDTO motoAlteration,
                                                    Guid Id)
    {
        var validate = validator.Validate(motoAlteration);
        if (!validate.IsValid)
        {
            var errors = validate.Errors.Select(e => e.ErrorMessage).ToList();
            return TypedResults.BadRequest(errors);
        }

        var motos = await DB.Motos.FirstOrDefaultAsync(c => c.IdMoto == Id);

        if (motos == null)
            return TypedResults.BadRequest(new List<string> { "Não existe Motos com esse Id" });

        if (!Uri.IsWellFormedUriString(motoAlteration.Url[0], UriKind.Absolute))
        {
           await image.DeleteUrlImage(motos.Url, "carros");

            motoAlteration.Url = await image.UploadBase64ImagesAsync(motoAlteration.Url);
        }
        
        motoAlteration.DateUpdate = DateTime.Now;
        
        mapper.Map(motoAlteration, motos);

        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }

}
