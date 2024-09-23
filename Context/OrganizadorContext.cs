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
    public DbSet<Store> Stores { get; set; }
    //para fazer a chamada do banco de dados

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>()
            .OwnsOne(u => u.Auditable);

        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Auditable);

        base.OnModelCreating(modelBuilder);
    }

}

