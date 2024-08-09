using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Extensions;
using Concessionaria.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrganizadorContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BdConnection"));
});//CONEXÃO COM O BANCO DE DADOS

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//necessario para o AutoMapper Funcionar

var app = builder.Build();

app.RegisterCarrosEndpoints();
//Esta classe esta no ENDPOINTROUTEBUILDEREXTENSIONS e serve para que possa aparecer as requisiçoes da API
app.RegisterUsuariosEndPoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
