using Microsoft.EntityFrameworkCore;
using LocadoraAPI.Models;

namespace LocadoraAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Carro> Carros { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>()
                .HasIndex(c => c.Placa)
                .IsUnique();

            modelBuilder.Entity<Locacao>()
                .HasOne(l => l.Carro)
                .WithMany(c => c.Locacoes)
                .HasForeignKey(l => l.CarroId);
        }
    }
}
