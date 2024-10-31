using Concessionaria.Models.Cars;
using FluentValidation;
using System.Data;
using System.Security.Policy;

namespace Concessionaria.Validators.Moto;

public class CreateMotoValidation : AbstractValidator<MotoCreateDTO>
{
    public CreateMotoValidation()
    {
        RuleFor(moto => moto.MotoBrand)
            .NotEmpty().WithMessage("A marca da moto não pode ser Nula");

        RuleFor(moto => moto.Model)
            .NotEmpty().WithMessage("O nome da moto não pode ser Nula");

        RuleFor(moto => moto.Fuel)
            .NotEmpty().WithMessage("O Combustivel da moto não pode ser Nula")
            .Length(4,8).WithMessage("Apenas Flex ou Gasolina");
        
        RuleFor(moto => moto.Color)
            .NotEmpty().WithMessage("A cor da moto não pode ser Nula");

        RuleFor(moto => moto.Plate)
            .NotEmpty().WithMessage("A Placa da moto não pode ser Nula")
            .Matches(@"^[A-Z]{3}-\d[A-Z]?\d{2,3}$").WithMessage("A Placa deve seguir o formato ABC-1234 ou ABC-1D23");

        RuleFor(moto => moto.Age)
            .NotEmpty().WithMessage("O Ano da moto não pode estar vazio");

        RuleFor(moto => moto.Km)
            .NotEmpty().WithMessage("A Kilometragem da moto não pode estar vazio");

        RuleFor(moto => moto.Price)
            .NotEmpty().WithMessage("O preço da moto não pode ser vazia");

        RuleFor(moto => moto.CreateUserId)
            .NotEmpty().WithMessage("Precisa de um Usuario para Criar um anúncio");

        RuleFor(moto => moto.Url)
            .NotEmpty().WithMessage("precisa de ao menos 3 imagens para ser criado um anúncio")
            .Must(Url => Url.Length >= 3).WithMessage("precisa de ao menos 3 imagens para ser criado um anúncio")
            .Must(Url => Url.Length <= 8).WithMessage("O maximo de imagens é de 8");
    }
}
