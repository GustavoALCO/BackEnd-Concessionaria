using Concessionaria.Entities;
using Microsoft.EntityFrameworkCore;

namespace Concessionaria.Context;

public class OrganizadorContext : DbContext
{
    public OrganizadorContext(DbContextOptions<OrganizadorContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Store> Store { get; set; }
    //para fazer a chamada do banco de dados

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>()
            .HasOne<Store>() // Relacionamento com Store
            .WithMany() // Assumindo que Store pode ter muitos usuários
            .HasForeignKey(u => u.IdStore);

        modelBuilder.Entity<Moto>()
            .OwnsOne(u => u.Auditable);

        //Configurando a Relação dos Funcionarios com as Lojas
        modelBuilder.Entity<User>()
            .HasOne<Store>() // Relacionamento com Store
            .WithMany() // Assumindo que Store pode ter muitos usuários
            .HasForeignKey(u => u.IdStore);

        //Configurando a classe auditable para os Funcionarios
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Auditable);



        base.OnModelCreating(modelBuilder);
    }

}

