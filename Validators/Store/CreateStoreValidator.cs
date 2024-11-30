using Concessionaria.Entities;
using Concessionaria.Models.Store;
using FluentValidation;

namespace Concessionaria.Validators.Store;

public class CreateStoreValidator : AbstractValidator<StoreCreateDTO>
{
    public CreateStoreValidator()
    {
        RuleFor(store => store.Adress)
            .NotEmpty().WithMessage("O campo Endereço é obrigatório.");

        RuleFor(store => store.AdressNumber)
            .GreaterThan(0).WithMessage("O número do endereço deve ser maior que zero.");

        RuleFor(store => store.CEP)
            .NotEmpty().WithMessage("O campo CEP é obrigatório.")
            .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000-000.");

        RuleFor(store => store.PhoneNumbers)
            .NotEmpty().WithMessage("Pelo menos um número de telefone é obrigatório.")
            .Must(phoneNumbers => phoneNumbers.Length > 0).WithMessage("É necessário pelo menos um número de telefone.")
            .Must(phoneNumbers => phoneNumbers.Length < 3).WithMessage("Só pode ter apenas 2 Numeros de Telefone Por Loja")
            .ForEach(phoneNumber => phoneNumber
                                    .Must(phone =>
                                          System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\(\d{2}\) (\d{5}-\d{4}|\d{4}-\d{4}$)"))
                                          .WithMessage("O número de telefone deve estar no formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX"));
    }
}

