using Concessionaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concessionaria.Context;

public class OrganizadorContext : DbContext
{
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
        {

        }
        public DbSet<Cars> Cars { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        
    //para fazer a chamada do banco de dados

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cars>()
                .HasKey(c => c.IdCar);

        modelBuilder.Entity<Cars>()
            .HasOne(c => c.User) // Define a relação com a entidade User
            .WithMany(u => u.Carros) // Um usuário pode ter muitos carros
            .HasForeignKey(c => c.IdUser); // Chave estrangeira
    }

}

