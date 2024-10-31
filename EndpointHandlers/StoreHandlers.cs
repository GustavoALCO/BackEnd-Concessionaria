using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Store;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Concessionaria.EndpointHandlers;

public class StoreHandlers
{
    public static async Task<Results<Ok<IEnumerable<StoreDTO>>, BadRequest>>GetStoreAsync
                                                    (OrganizadorContext DB,
                                                     IMapper mapper,
                                                     int? Id)
    {

        var lojas = mapper.Map<IEnumerable<StoreDTO>>( await DB.Store.Where(s => s == null|| s.IdStore == Id).ToListAsync());

        if (lojas.Count() == 0 || lojas == null)
            return TypedResults.BadRequest();

        return TypedResults.Ok(lojas);
    }

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
