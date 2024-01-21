using Core.Domain;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure
{
    public class FoodWasteDbContext : DbContext
    {
        public FoodWasteDbContext(DbContextOptions<FoodWasteDbContext> options) : base(options)
        {
        }

        public DbSet<Pakket> Pakketten { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Product> Producten { get; set; }
        public DbSet<Kantine> Kantines { get; set; }
        public DbSet<Medewerker> Medewerkers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pakket>()
                .Property(m => m.prijs)
                .HasColumnType("decimal(18, 2)");

        }
    }
}