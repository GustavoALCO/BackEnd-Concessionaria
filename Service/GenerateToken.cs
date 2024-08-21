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

        // Construtor que recebe a configuração da aplicação
        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método que gera o token JWT
        public string GenerateTokenLogin(string username)
        {
            // Cria a chave de segurança usando a chave secreta definida na configuração
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Define as credenciais de assinatura usando a chave de segurança
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define os claims do token
            var claims = new[]
            {
                // Claim para o assunto (usuário)
                new Claim(JwtRegisteredClaimNames.Sub, username),
                
                // Claim para o identificador único do token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Cria o token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Emissor do token
                audience: _configuration["Jwt:Audience"], // Audiência do token
                claims: claims, // Claims do token
                expires: DateTime.Now.AddHours(double.Parse(_configuration["Jwt:ExpireHours"])), // Data de expiração do token
                signingCredentials: credentials // Credenciais de assinatura
            );

            // Retorna o token JWT como uma string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
