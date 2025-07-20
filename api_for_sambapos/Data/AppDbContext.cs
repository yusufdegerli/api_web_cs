using api_for_sambapos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tables> Tables { get; set; }
    public DbSet<MenuItemProperties> MenuItemProperties { get; set; }
    public DbSet<MenuItemPropertyGroups> MenuItemPropertyGroups { get; set; }
    public DbSet<MenuItemPortions> MenuItemPortions { get; set; }
    public DbSet<TicketItem> TicketItems { get; set; }
    public DbSet<UserRoles> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Date)
            .HasColumnType("datetime");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.LastUpdateTime)
            .HasColumnType("datetime");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.LastOrderDate)
            .HasColumnType("datetime");

        modelBuilder.Entity<Ticket>()
            .Property(t => t.LastPaymentDate)
            .HasColumnType("datetime");

        modelBuilder.Entity<MenuItemPortions>()
            .Property(m => m.Price_Amount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<TicketItem>()
            .Property(ti => ti.VatRate)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<TicketItem>()
            .Property(ti => ti.VatAmount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserRole)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.UserRole_Id);

        modelBuilder.Entity<MenuItem>().ToTable("MenuItems");
        modelBuilder.Entity<MenuItemPortions>().ToTable("MenuItemPortions");
        modelBuilder.Entity<MenuItemPropertyGroups>().ToTable("MenuItemPropertyGroups");
        modelBuilder.Entity<MenuItemProperties>().ToTable("MenuItemProperties");

        // MenuItemProperties için foreign key ilişkileri
        modelBuilder.Entity<MenuItemProperties>()
            .HasOne(p => p.MenuItem)
            .WithMany(m => m.Properties)
            .HasForeignKey(p => p.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade); // MenuItem silinirse ilgili MenuItemProperties de silinsin

        modelBuilder.Entity<MenuItemProperties>()
            .HasOne(p => p.MenuItemPropertyGroup)
            .WithMany(g => g.Properties)
            .HasForeignKey(p => p.MenuItemPropertyGroupId)
            .OnDelete(DeleteBehavior.Cascade); // MenuItemPropertyGroup silinirse ilgili MenuItemProperties de silinsin

        modelBuilder.Entity<User>()
            .Ignore(u => u.LastUpdateTime);

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Tickets");
            entity.Property(e => e.IsClosed)
                .HasDefaultValue(false);
        });

        modelBuilder.Entity<TicketItem>(entity =>
        {
            entity.ToTable("TicketItems");
            entity.Property(e => e.Quantity)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<User>().ToTable("Users");

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tables>()
            .Property(t => t.TicketId)
            .IsRequired();

        modelBuilder.Entity<TicketItem>()
            .HasOne(ti => ti.Ticket)
            .WithMany(t => t.TicketItems)
            .HasForeignKey(ti => ti.TicketId);

        modelBuilder.Entity<TicketItem>()
            .HasOne(ti => ti.Ticket)
            .WithMany(t => t.TicketItems)
            .HasForeignKey(ti => ti.TicketId);

        //
        modelBuilder.Entity<Tables>()
            .HasOne(t => t.Ticket)
            .WithMany(t => t.Tables)
            .HasForeignKey(t => t.TicketId)
            .IsRequired(false);//TicketId nullable çünkü BU FALSE KALKABİLİR

        //modelBuilder.Entity<Ticket>()
        //   .Ignore(t => t.TableId);
        // Tables ile TicketItem arasında ilişki

        base.OnModelCreating(modelBuilder);
    }
}