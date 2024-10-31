using Concessionaria.Validators;
using FluentValidation;

namespace Concessionaria.Models.Store;

public class StoreCreateDTO 
{
    public string Adress { get; set; }

    public int AdressNumber { get; set; }

    public string CEP { get; set; }

    public string[] PhoneNumbers { get; set; }
}
