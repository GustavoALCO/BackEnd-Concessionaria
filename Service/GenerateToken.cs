using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Concessionaria.Service
{
    public class GenerateToken
    {
        private readonly IConfiguration _configuration;

        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
            //configuração para acessar informaçoes do appsetings
        }

        // Método que gera o token JWT
        public string GerarTokenLogin(string username)
        {
            
            var chaveScreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //busca a chave secreta que esta no appsetings 

            var credentials = new SigningCredentials(chaveScreta, SecurityAlgorithms.HmacSha256);
            //informa a chave o tipo de segurança para a criação do Header do JWT

            var claims = new[]
            {
                new Claim("login", username)
                //adiciona no claim o nome do gerador do token
            };

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    //Audience e o Issuer são assinaturas para que o Jwt funcione corretamente
                    claims: claims,
                    //claims serve para passar dados adicionais do gerador do código
                    expires: DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"])),
                    //Define quantas horas o token vai existir
                    signingCredentials: credentials
                    );
  
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
