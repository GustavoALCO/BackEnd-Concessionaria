using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Store;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;


namespace Concessionaria.EndpointHandlers;

public class StoreHandlers
{
    [Authorize]
    public static async Task<Results<Ok<IEnumerable<StoreDTO>>, BadRequest>>GetStoreAsync
                                                    (OrganizadorContext DB,
                                                     IMapper mapper,
                                                     [FromQuery(Name = "Adress")]
                                                     string? Adress)
    {

        var lojas = mapper.Map<IEnumerable<StoreDTO>>( await DB.Store.Where(s => Adress == null|| s.Adress.ToUpper().Contains(Adress.ToUpper())).ToListAsync());

        if (lojas.Count() == 0 || lojas == null)
            return TypedResults.BadRequest();

        return TypedResults.Ok(lojas);
    }

    [Authorize]
    public static async Task<Results<Ok<StoreDTO>, NotFound>>GetStoreId(OrganizadorContext DB,
        IMapper mapper,
        int id)
    {
        var lojas = mapper.Map<StoreDTO>(await DB.Store.FirstOrDefaultAsync(s => s.IdStore ==  id));

        if(lojas == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(lojas);
    }

    [Authorize]
    public static async Task<Results<Ok, BadRequest<string>>> PostStoreAsync(
                                                                    OrganizadorContext DB,
                                                                    IMapper mapper,
                                                                    IValidator<StoreCreateDTO> validator,
                                                                    [FromBody] StoreCreateDTO storeDto)
    {
        
        var validacao = await validator.ValidateAsync(storeDto);
        if (!validacao.IsValid)
            return TypedResults.BadRequest(validacao.Errors.ToString());

        var store = mapper.Map<Store>(storeDto);  

        DB.Add(store);
        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }
    [Authorize]
    public static async Task<Results<Ok, BadRequest<string>>> PutStoreAsync
                                                      (OrganizadorContext DB,
                                                       IMapper mapper,
                                                       IValidator<StoreAlterationDTO> validator,
                                                       [FromBody]
                                                       StoreAlterationDTO Alterationstore,
                                                       int Id)
    {
        var validacao = await validator.ValidateAsync(Alterationstore);
        if (!validacao.IsValid)
            return TypedResults.BadRequest(validacao.Errors.ToString());

        var store = await DB.Store.FindAsync(Id);

        if (store == null)
            return TypedResults.BadRequest("Não foi encontrado");

        mapper.Map(Alterationstore, store);
        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }
    [Authorize]
    public static async Task<Results<Ok,BadRequest>> DeleteStore(OrganizadorContext DB,
                                                                 int Id) 
    {
        var store = await DB.Store.FindAsync(Id);

        if (store == null)
            return TypedResults.BadRequest();

        DB.Remove(store);
        await DB.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
