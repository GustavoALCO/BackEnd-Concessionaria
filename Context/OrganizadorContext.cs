using Concessionaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concessionaria.Context;

public class OrganizadorContext : DbContext
{
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
        {

        }
        public DbSet<Carros> Cars { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null;
        public DbSet<ImageCar> Images { get; set; } = null!;
    //para fazer a chamada do banco de dados

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Carros>()
                .HasKey(c => c.IdCar);

        modelBuilder.Entity<Carros>()
            .HasOne(c => c.User) // Define a relação com a entidade User
            .WithMany(u => u.Carros) // Um usuário pode ter muitos carros
            .HasForeignKey(c => c.IdUser); // Chave estrangeira

        // Configuração da chave primária e relações para a entidade ImageCar
        modelBuilder.Entity<ImageCar>()
            .HasKey(ic => ic.IdImage);

        modelBuilder.Entity<ImageCar>()
            .HasOne(ic => ic.Carros) // Define a relação com a entidade Carros
            .WithMany(c => c.Images) // Um carro pode ter muitas imagens
            .HasForeignKey(ic => ic.IdCar); // Chave estrangeira
    }

}

