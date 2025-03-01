using Microsoft.EntityFrameworkCore;
using web_api_using_crud_with_swagger.Models;

namespace web_api_using_crud_with_swagger.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Buraya veritabanı tablolarını temsil eden DbSet'leri ekleyeceğiz.
        public DbSet<Product> Products { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().ToTable("Tickets"); // for dbo.Tickets
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
