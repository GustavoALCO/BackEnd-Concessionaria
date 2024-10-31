using Concessionaria.Context;
using Concessionaria.Extensions;
using Concessionaria.Models.Cars;
using Concessionaria.Models.Store;
using Concessionaria.Models.Users;
using Concessionaria.Service;
using Concessionaria.Validators.Moto;
using Concessionaria.Validators.Store;
using Concessionaria.Validators.User;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Imperio Motos - API"
    });
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Jwt Authentication",
        Description = "Entre com o JWt Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
                                        {
                                            Id= JwtBearerDefaults.AuthenticationScheme,
                                            Type = ReferenceType.SecurityScheme
                                        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securitySchema, new string[] {} }
    });
}
    );

builder.Services.AddScoped<HashService>();
builder.Services.AddScoped<ImageUpload>();
builder.Services.AddDbContext<OrganizadorContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("BdConnection"))
);//CONEXÃO COM O BANCO DE DADOS

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        //para validar o Issuer
        ValidateAudience = true,
        //para valida a audience
        ValidateLifetime = true,
        //para verefica se esta no prazo de vida
        ValidateIssuerSigningKey = true,
        //informa que ira passar uma chave secreta
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

// Declarando validaçoes se todas as variaveis necessarias serão preenchidas 
builder.Services.AddScoped<IValidator<StoreAlterationDTO>, AlterationStoreValidation>();
builder.Services.AddScoped<IValidator<StoreCreateDTO>, CreateStoreValidator>();
builder.Services.AddScoped<IValidator<UserAlterationDTO>, AlterationUserValidation>();
builder.Services.AddScoped<IValidator<UserCreateDTO>, CreateUserValidation>();
builder.Services.AddScoped<IValidator<MotoAlterationDTO>,AlterationMotoValidation>();
builder.Services.AddScoped<IValidator<MotoCreateDTO>, CreateMotoValidation>();
//

//Declarando Classe que gera um token Jwt para o Usuario
builder.Services.AddScoped<GenerateToken>();

//necessario para o AutoMapper Funcionar
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//Declarando no código os Endpoints
app.RegisterCarrosEndpoints();
app.RegisterUsuariosEndPoints();
app.RegisterStoreEndPoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
