using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Models;

// dotnet ef migrations add <MigrationName>
// dotnet ef database update
public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(ILogger<AppDbContext> logger)
    {
        _logger = logger;
    }

    // DbSet property
    public DbSet<Billing> MonthlyBillings { get; set; }
    public DbSet<Consumer> Consumers { get; set; }
    public DbSet<Grid> Grids { get; set; }
    public DbSet<GridSection> GridSections { get; set; }
    public DbSet<Consumption> Consumptions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }


    // Constructor for AppDbContext (optional)
    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options) { _logger = logger ?? throw new ArgumentNullException(nameof(logger)); }


    // Additional DbSet properties and configurations can be added here
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grid>()
                .HasIndex(g => g.GridName)
                .IsUnique();

        modelBuilder.Entity<GridSection>()
                .HasIndex(g => g.GridSectionName)
                .IsUnique();

        modelBuilder.Entity<GridSection>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.GridSection)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<Billing>()
            .HasOne(gs => gs.Consumer)
            .WithMany()
            .HasForeignKey(gs => gs.ConsumerID);

        modelBuilder.Entity<Billing>()
            .Property(mb => mb.TotalConsumption)
            .HasColumnType("decimal(18, 3)");

        modelBuilder.Entity<Billing>()
            .Property(mb => mb.BillingAmount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Consumption>()
            .HasOne(gs => gs.Consumer)
            .WithMany()
            .HasForeignKey(gs => gs.ConsumerID);

        modelBuilder.Entity<Consumption>()
            .Property(mb => mb.ConsumptionValue)
            .HasColumnType("decimal(11, 3)");

        modelBuilder.Entity<Consumer>()
            .HasOne(gs => gs.GridSection)
            .WithMany()
            .HasForeignKey(gs => gs.GridSectionID);

        base.OnModelCreating(modelBuilder);
    }

}


