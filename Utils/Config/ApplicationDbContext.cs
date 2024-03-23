using Microsoft.EntityFrameworkCore;
using CoolingGridManager.Models;

// namespace CoolingGridManagementTool.Data // Replace with your actual namespace
// {
public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(ILogger<AppDbContext> logger)
    {
        _logger = logger;
    }
    public void SomeMethod()
    {
        // Example method where you can log information
        _logger.LogInformation("Executing SomeMethod...");
    }
    // DbSet property
    public DbSet<MonthlyBilling> MonthlyBillings { get; set; }
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

        modelBuilder.Entity<GridSection>()
            .HasOne(gs => gs.Grid)
            .WithMany(g => g.GridSection)
            .HasForeignKey(gs => gs.GridID);

        modelBuilder.Entity<MonthlyBilling>()
            .HasOne(gs => gs.Consumer)
            .WithMany()
            .HasForeignKey(gs => gs.ConsumerID);

        modelBuilder.Entity<MonthlyBilling>()
            .Property(mb => mb.TotalConsumption)
            .HasColumnType("decimal(18, 3)");

        modelBuilder.Entity<MonthlyBilling>()
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
            .HasForeignKey(gs => gs.ConsumerID);

        base.OnModelCreating(modelBuilder);
    }

}

// // public class MyClass
// // {
// //     private readonly Serilog.ILogger _logger;

// //     public MyClass(Serilog.ILogger logger)
// //     {
// //         _logger = logger;
// //     }

// //     public void MyMethod()
// //     {
// //         _logger.Information("Logging from MyClass");
// //     }
// // }
